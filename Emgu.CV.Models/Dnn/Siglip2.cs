//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// SigLIP 2 (base, patch16, 224) joint text and image embedding model
    /// running on the dnn module. Text (multilingual) and images are embedded
    /// into a shared 768 dimensional space; the cosine similarity between the
    /// embeddings measures how well the text describes the image, enabling
    /// text-to-image search and zero-shot classification.
    ///
    /// The ONNX model (~1.4 GB, fp32) is the raw `optimum-cli export onnx
    /// --model google/siglip2-base-patch16-224` export, a single graph
    /// containing both the text and the vision tower. SigLIP 2 uses the Gemma
    /// tokenizer, which is loaded with the dnn Tokenizer (no custom tokenizer
    /// implementation is needed, unlike CLIP and SigLIP 1).
    /// </summary>
    public class Siglip2 : DisposableObject
    {
        private const int PadId = 0;   // <pad>
        private const int EosId = 1;   // <eos>
        private const int ImageSize = 224;
        //SigLIP 2 is trained with text padded to a fixed 64 token sequence; the
        //exported graph has no attention mask, so the padding is part of the
        //model semantics.
        private const int TextSequenceLength = 64;

        private String _modelFolderName = Path.Combine("emgu", "siglip2_base_patch16_224_onnx");

        private Net _net = null;
        private Tokenizer _tokenizer = null;

        //A cached input fed to the tower that is not being queried: the exported
        //graph is a single network with both towers, so both inputs must be set.
        private Mat _dummyPixelValues = null;

        /// <summary>
        /// Create a SigLIP 2 embedding model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public Siglip2(String modelFolderName = null)
        {
            if (modelFolderName != null)
                _modelFolderName = modelFolderName;
        }

        /// <summary>
        /// Return true if the model is initialized.
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _net != null && _tokenizer != null;
            }
        }

        /// <summary>
        /// Download and initialize the SigLIP 2 model and its tokenizer.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (!Initialized)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/siglip2_base_patch16_224_onnx/tokenizer.json",
                    _modelFolderName,
                    "CB9140FAE3AC5122C972D37ADF83E1248471A38147AD76F8215C8872C6FD8322");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/siglip2_base_patch16_224_onnx/model.onnx",
                    _modelFolderName,
                    "9BAF596593B3D82F389F21C68B7777EE510C4AF42C3C6FEB2F51F6B383327A25");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    String modelFolder = Path.GetDirectoryName(manager.Files[0].LocalFile);

                    //The tokenizer config is tiny and fixed, write it locally
                    //instead of downloading. The "Gemma" method parses the
                    //Gemma BPE tokenizer.json without prepending <bos>,
                    //matching the SigLIP 2 text format.
                    String configPath = Path.Combine(modelFolder, "opencv_tokenizer_config.json");
                    if (!File.Exists(configPath))
                        File.WriteAllText(configPath, "{ \"method\": \"Gemma\" }");

                    _tokenizer = Tokenizer.Load(configPath);
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                    _net = DnnInvoke.ReadNetFromONNX(Path.Combine(modelFolder, "model.onnx"), EngineType.New);
#else
                    String modelPath = Path.Combine(modelFolder, "model.onnx");
                    _net = await Task.Run(() => DnnInvoke.ReadNetFromONNX(modelPath, EngineType.New));
#endif
                }
            }
        }

        /// <summary>
        /// Tokenize the given text with the Gemma tokenizer, terminated with
        /// the end of sequence token and padded to the fixed 64 token sequence
        /// length the model was trained with.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <returns>The token ids, exactly 64 values.</returns>
        public int[] Tokenize(String text)
        {
            if (_tokenizer == null)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            List<int> ids = new List<int>(_tokenizer.Encode(text));
            if (ids.Count > TextSequenceLength - 1)
                ids.RemoveRange(TextSequenceLength - 1, ids.Count - (TextSequenceLength - 1));
            ids.Add(EosId);
            while (ids.Count < TextSequenceLength)
                ids.Add(PadId);
            return ids.ToArray();
        }

        private void SetInt64Input(long[] values, String inputName)
        {
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
            try
            {
                using (Mat input = new Mat(new int[] { 1, values.Length }, DepthType.Cv64S, handle.AddrOfPinnedObject()))
                    _net.SetInput(input, inputName);
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// SigLIP 2 image preprocessing: resize to 224x224 (bicubic, not aspect
        /// preserving), convert to RGB and normalize to [-1, 1]
        /// (mean=0.5, std=0.5): (x/255 - 0.5)/0.5 = (x - 127.5)/127.5.
        /// </summary>
        private static Mat PreprocessImage(Mat image)
        {
            using (Mat resized = new Mat())
            {
                CvInvoke.Resize(image, resized, new Size(ImageSize, ImageSize), 0, 0, Inter.Cubic);
                return DnnInvoke.BlobFromImage(
                    resized,
                    1.0 / 127.5,
                    new Size(ImageSize, ImageSize),
                    new MCvScalar(127.5, 127.5, 127.5),
                    true,
                    false);
            }
        }

        private Mat GetDummyPixelValues()
        {
            if (_dummyPixelValues == null)
            {
                using (Mat gray = new Mat(ImageSize, ImageSize, DepthType.Cv8U, 3))
                {
                    gray.SetTo(new MCvScalar(128, 128, 128));
                    _dummyPixelValues = PreprocessImage(gray);
                }
            }
            return _dummyPixelValues;
        }

        /// <summary>
        /// Read the (1, dim) embedding output with the given name, L2-normalized.
        /// </summary>
        private float[] ReadNormalizedEmbedding(String outputName)
        {
            using (Mat embeds = _net.Forward(outputName))
            {
                float[] result = new float[embeds.SizeOfDimension[1]];
                Marshal.Copy(embeds.DataPointer, result, 0, result.Length);
                double norm = 0;
                for (int i = 0; i < result.Length; i++)
                    norm += (double)result[i] * result[i];
                norm = Math.Sqrt(norm);
                if (norm > 0)
                {
                    for (int i = 0; i < result.Length; i++)
                        result[i] = (float)(result[i] / norm);
                }
                return result;
            }
        }

        /// <summary>
        /// Embed the given text into the shared text-image embedding space.
        /// </summary>
        /// <param name="text">The text, e.g. "a photo of a dog", in any of the supported languages.</param>
        /// <returns>The L2-normalized embedding (768 values for the base model).</returns>
        public float[] EmbedText(String text)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            int[] tokens = Tokenize(text);
            long[] inputIds = new long[tokens.Length];
            for (int i = 0; i < tokens.Length; i++)
                inputIds[i] = tokens[i];

            SetInt64Input(inputIds, "input_ids");
            _net.SetInput(GetDummyPixelValues(), "pixel_values");
            return ReadNormalizedEmbedding("text_embeds");
        }

        /// <summary>
        /// Embed the given image into the shared text-image embedding space.
        /// </summary>
        /// <param name="image">The image (BGR).</param>
        /// <returns>The L2-normalized embedding (768 values for the base model).</returns>
        public float[] EmbedImage(IInputArray image)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            using (Mat pixelValues = PreprocessImage(mat))
            {
                SetInt64Input(new long[TextSequenceLength], "input_ids");   //all <pad>
                _net.SetInput(pixelValues, "pixel_values");
                return ReadNormalizedEmbedding("image_embeds");
            }
        }

        /// <summary>
        /// Compute the cosine similarity between two embeddings.
        /// </summary>
        /// <param name="a">The first embedding.</param>
        /// <param name="b">The second embedding.</param>
        /// <returns>The cosine similarity, in [-1, 1] for L2-normalized embeddings.</returns>
        public static double CosineSimilarity(float[] a, float[] b)
        {
            return Clip.CosineSimilarity(a, b);
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before use.
        /// </summary>
        public void Clear()
        {
            if (_net != null)
            {
                _net.Dispose();
                _net = null;
            }

            if (_dummyPixelValues != null)
            {
                _dummyPixelValues.Dispose();
                _dummyPixelValues = null;
            }

            if (_tokenizer != null)
            {
                _tokenizer.Dispose();
                _tokenizer = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this SigLIP 2 model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
