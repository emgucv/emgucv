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
    /// PaliGemma2 vision-language model. Given an image and a text prompt, it
    /// generates a text response (e.g. a caption). The model is split into three
    /// ONNX files: the SigLIP vision encoder (image -> 256 image-feature
    /// tokens), the embedding network (prompt token ids -> text embeddings) and
    /// the Gemma2 language model ([image_features | text_embeds] -> logits).
    /// All three networks run on the new dnn engine.
    ///
    /// The models are ~15 GB across ~300 files and the official tokenizer
    /// descends from a gated huggingface repository, so this class does not
    /// download them. The model folder passed to the constructor must contain:
    ///   vision_model.onnx                 - SigLIP vision encoder
    ///   embedding.onnx, embedding.weight  - embedding + external weights
    ///   gemma2_3b.onnx, onnx__MatMul_*,
    ///   onnx__Mul_*                       - Gemma2 + external weights
    ///   config.json, tokenizer.json       - OpenCV-format Gemma tokenizer
    /// ONNX models: https://huggingface.co/nklskyoy/paligemma2-3b-pt-224-onnx
    ///
    /// config.json must be { "method": "SentencePiece" }: that tokenizer method
    /// prepends the &lt;bos&gt; token on encode like the HuggingFace Gemma
    /// tokenizer does; without &lt;bos&gt; the model generates a degenerate
    /// response. tokenizer.json is the standard Gemma-2 tokenizer (e.g. from an
    /// ungated mirror such as https://huggingface.co/unsloth/gemma-2-2b).
    /// </summary>
    public class VlmPaliGemma2 : DisposableObject
    {
        private const int EosId = 1;

        private readonly String _modelFolder;

        private Tokenizer _tokenizer = null;
        private Net _siglipNet = null;
        private Net _embedNet = null;
        private Net _gemmaNet = null;

        /// <summary>
        /// Create a PaliGemma2 vision-language model.
        /// </summary>
        /// <param name="modelFolder">The folder containing the ONNX models and the tokenizer files. See the class summary for the expected content.</param>
        public VlmPaliGemma2(String modelFolder)
        {
            _modelFolder = modelFolder;
        }

        /// <summary>
        /// Return true if the model is initialized.
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _tokenizer != null && _siglipNet != null && _embedNet != null && _gemmaNet != null;
            }
        }

        private void LoadModels()
        {
            if (_tokenizer == null)
                _tokenizer = Tokenizer.Load(Path.Combine(_modelFolder, "config.json"));
            if (_siglipNet == null)
                _siglipNet = DnnInvoke.ReadNetFromONNX(Path.Combine(_modelFolder, "vision_model.onnx"), EngineType.New);
            if (_embedNet == null)
                _embedNet = DnnInvoke.ReadNetFromONNX(Path.Combine(_modelFolder, "embedding.onnx"), EngineType.New);
            if (_gemmaNet == null)
                _gemmaNet = DnnInvoke.ReadNetFromONNX(Path.Combine(_modelFolder, "gemma2_3b.onnx"), EngineType.New);
        }

        /// <summary>
        /// Initialize the model by loading the tokenizer and the three ONNX
        /// networks from the model folder. No file is downloaded; the model
        /// folder must already contain all the files.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Not used, present for signature consistency with the other models.</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            LoadModels();
            yield return null;
        }
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
        {
            await Task.Run(() => LoadModels());
        }
#endif

        /// <summary>
        /// Run the embedding network on the given token ids, returning the (1, count, hidden) embeddings.
        /// </summary>
        private Mat ForwardEmbedding(int[] tokenIds)
        {
            long[] ids = new long[tokenIds.Length];
            for (int i = 0; i < ids.Length; i++)
                ids[i] = tokenIds[i];
            GCHandle handle = GCHandle.Alloc(ids, GCHandleType.Pinned);
            try
            {
                using (Mat inputIds = new Mat(new int[] { 1, ids.Length }, DepthType.Cv64S, handle.AddrOfPinnedObject()))
                {
                    _embedNet.SetInput(inputIds, "input_ids");
                    return _embedNet.Forward();
                }
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Create a 2D (rows x cols) header over a (1, rows, cols) tensor.
        /// </summary>
        private static Mat TensorAsRows(Mat tensor)
        {
            int[] shape = tensor.SizeOfDimension;
            return new Mat(new int[] { shape[1], shape[2] }, tensor.Depth, tensor.DataPointer);
        }

        /// <summary>
        /// Run the language model on the (sequence, hidden) embeddings and
        /// return the argmax token id of the last sequence position.
        /// </summary>
        private int ForwardLanguageModel(Mat inputsEmbeds2D)
        {
            using (Mat inputsEmbeds = new Mat(
                new int[] { 1, inputsEmbeds2D.Rows, inputsEmbeds2D.Cols },
                DepthType.Cv32F,
                inputsEmbeds2D.DataPointer))
            {
                _gemmaNet.SetInput(inputsEmbeds, "inputs_embeds");
                using (Mat logits = _gemmaNet.Forward())
                {
                    int[] shape = logits.SizeOfDimension;   //(1, sequence, vocabulary)
                    int vocab = shape[2];
                    long lastRowOffset = (long)(shape[1] - 1) * vocab * sizeof(float);
                    using (Mat lastLogits = new Mat(
                        new int[] { 1, vocab },
                        DepthType.Cv32F,
                        new IntPtr(logits.DataPointer.ToInt64() + lastRowOffset)))
                    {
                        double minVal = 0, maxVal = 0;
                        Point minLoc = new Point(), maxLoc = new Point();
                        CvInvoke.MinMaxLoc(lastLogits, ref minVal, ref maxVal, ref minLoc, ref maxLoc);
                        return maxLoc.X;
                    }
                }
            }
        }

        /// <summary>
        /// Generate a text response for the given image and prompt.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="prompt">The task prompt, e.g. "cap en\n" to caption in English.</param>
        /// <param name="maxNewTokens">Maximum number of new tokens to generate.</param>
        /// <returns>The generated text response.</returns>
        public String Generate(IInputArray image, String prompt = "cap en\n", int maxNewTokens = 64)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            //Resize to 224x224 and normalize to [-1, 1] in RGB NCHW order
            //(SigLIP: mean=0.5, std=0.5): (x/255 - 0.5)/0.5 = (x - 127.5)/127.5
            using (Mat pixelValues = DnnInvoke.BlobFromImage(
                image,
                1.0 / 127.5,
                new Size(224, 224),
                new MCvScalar(127.5, 127.5, 127.5),
                true,
                false))
            {
                //SigLIP vision encoder: image -> image-feature tokens (1, 256, 2304)
                _siglipNet.SetInput(pixelValues, "pixel_values");
                using (Mat imageFeatures = _siglipNet.Forward())
                {
                    List<int> generated = new List<int>();

                    //The growing (sequence, hidden) embedding matrix; the
                    //(1, sequence, hidden) inputs_embeds tensor without the
                    //leading batch-1 dimension.
                    Mat inputsEmbeds = new Mat();
                    try
                    {
                        //Combine [image_features | text_embeds]
                        int[] tokens = _tokenizer.Encode(prompt);
                        using (Mat textEmbeds = ForwardEmbedding(tokens))
                        using (Mat imageFeatures2D = TensorAsRows(imageFeatures))
                        using (Mat textEmbeds2D = TensorAsRows(textEmbeds))
                        {
                            CvInvoke.VConcat(imageFeatures2D, textEmbeds2D, inputsEmbeds);
                        }

                        //Prefill
                        int newId = ForwardLanguageModel(inputsEmbeds);
                        generated.Add(newId);

                        //Decode (no KV-cache: feed the full growing sequence each step)
                        for (int i = 0; i < maxNewTokens - 1; i++)
                        {
                            if (newId == EosId)
                                break;

                            using (Mat newEmbed = ForwardEmbedding(new int[] { newId }))
                            using (Mat newEmbed2D = TensorAsRows(newEmbed))
                            {
                                Mat grown = new Mat();
                                CvInvoke.VConcat(inputsEmbeds, newEmbed2D, grown);
                                inputsEmbeds.Dispose();
                                inputsEmbeds = grown;
                            }

                            newId = ForwardLanguageModel(inputsEmbeds);
                            generated.Add(newId);
                        }
                    }
                    finally
                    {
                        inputsEmbeds.Dispose();
                    }

                    if (generated.Count > 0 && generated[generated.Count - 1] == EosId)
                        generated.RemoveAt(generated.Count - 1);

                    return _tokenizer.Decode(generated.ToArray());
                }
            }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling Generate.
        /// </summary>
        public void Clear()
        {
            if (_tokenizer != null)
            {
                _tokenizer.Dispose();
                _tokenizer = null;
            }

            if (_siglipNet != null)
            {
                _siglipNet.Dispose();
                _siglipNet = null;
            }

            if (_embedNet != null)
            {
                _embedNet.Dispose();
                _embedNet = null;
            }

            if (_gemmaNet != null)
            {
                _gemmaNet.Dispose();
                _gemmaNet = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this vision language model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
