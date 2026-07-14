//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// SigLIP (base, patch16, 224) joint text and image embedding model running
    /// on the dnn module. Text and images are embedded into a shared 768
    /// dimensional space; the cosine similarity between the embeddings measures
    /// how well the text describes the image, enabling text-to-image search and
    /// zero-shot classification.
    ///
    /// The ONNX model (~780 MB, fp32) is the raw `optimum-cli export onnx
    /// --model google/siglip-base-patch16-224` export, a single graph
    /// containing both the text and the vision tower. The SentencePiece Unigram
    /// tokenizer is implemented in this class (cv::dnn::Tokenizer only supports
    /// Gemma-style BPE tokenizer files).
    /// </summary>
    public class Siglip : DisposableObject
    {
        private const int EosId = 1;      // </s>, also used as the padding token
        private const int ImageSize = 224;
        //SigLIP is trained with text padded to a fixed 64 token sequence; the
        //exported graph has no attention mask, so the padding is part of the
        //model semantics.
        private const int TextSequenceLength = 64;

        private String _modelFolderName = Path.Combine("emgu", "siglip_base_patch16_224_onnx");

        private Net _net = null;
        private UnigramTokenizer _tokenizer = null;

        //A cached input fed to the tower that is not being queried: the exported
        //graph is a single network with both towers, so both inputs must be set.
        private Mat _dummyPixelValues = null;

        /// <summary>
        /// Create a SigLIP embedding model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public Siglip(String modelFolderName = null)
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
        /// Download and initialize the SigLIP model and its tokenizer.
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
                    "https://emgu-public.s3.amazonaws.com/siglip_base_patch16_224_onnx/tokenizer.json",
                    _modelFolderName,
                    "C6E405CB7C670D56636A9402C81023A55BC6C3C53D89CF02B92F5C5005BFE920");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/siglip_base_patch16_224_onnx/model.onnx",
                    _modelFolderName,
                    "EFD49F3BCA9C3587CD41BD5E23996EC356E31B21D6A0E02773F3633427D4E5D1");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _tokenizer = new UnigramTokenizer(manager.Files[0].LocalFile);
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                    _net = DnnInvoke.ReadNetFromONNX(manager.Files[1].LocalFile, EngineType.New);
#else
                    String modelPath = manager.Files[1].LocalFile;
                    _net = await Task.Run(() => DnnInvoke.ReadNetFromONNX(modelPath, EngineType.New));
#endif
                }
            }
        }

        /// <summary>
        /// Tokenize the given text with the SigLIP SentencePiece tokenizer,
        /// terminated with the end of sequence token and padded to the fixed
        /// 64 token sequence length the model was trained with.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <returns>The token ids, exactly 64 values.</returns>
        public int[] Tokenize(String text)
        {
            if (_tokenizer == null)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            List<int> ids = _tokenizer.Encode(text);
            if (ids.Count > TextSequenceLength - 1)
                ids.RemoveRange(TextSequenceLength - 1, ids.Count - (TextSequenceLength - 1));
            ids.Add(EosId);
            while (ids.Count < TextSequenceLength)
                ids.Add(EosId);   //the padding token equals </s>
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
        /// SigLIP image preprocessing: resize to 224x224 (bicubic, not aspect
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
        /// <param name="text">The text, e.g. "a photo of a dog".</param>
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

            _tokenizer = null;
        }

        /// <summary>
        /// Release the memory associated with this SigLIP model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// SentencePiece Unigram tokenizer, following the huggingface
        /// tokenizer.json specification of the SigLIP tokenizer: the text is
        /// lowercased, a punctuation set is removed and whitespace collapsed;
        /// the text is then split into words, each prefixed with the metaspace
        /// character (U+2581), and every word is segmented with Viterbi search
        /// maximizing the sum of the piece scores. Characters not covered by
        /// the vocabulary fall back to their UTF-8 bytes (the &lt;0xNN&gt;
        /// pieces).
        /// </summary>
        private class UnigramTokenizer
        {
            private const char Metaspace = '▁';

            private readonly Dictionary<String, int> _pieceToId = new Dictionary<String, int>();
            private readonly Dictionary<String, double> _pieceScores = new Dictionary<String, double>();
            private readonly int _maxPieceLength = 1;
            private readonly double _minScore = 0;

            //The punctuation set removed by the SigLIP normalizer.
            private static readonly Regex _punctuation = new Regex(
                "[!\"\\#\\$%\\&'\\(\\)\\*\\+,\\-\\.:;=\\?@\\[\\\\\\]\\^_`\\{\\|\\}\\~]",
                RegexOptions.Compiled);

            public UnigramTokenizer(String tokenizerJsonPath)
            {
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(tokenizerJsonPath)))
                {
                    System.Text.Json.JsonElement vocab = doc.RootElement.GetProperty("model").GetProperty("vocab");
                    int id = 0;
                    foreach (System.Text.Json.JsonElement entry in vocab.EnumerateArray())
                    {
                        String piece = entry[0].GetString();
                        double score = entry[1].GetDouble();
                        _pieceToId[piece] = id;
                        _pieceScores[piece] = score;
                        if (piece.Length > _maxPieceLength)
                            _maxPieceLength = piece.Length;
                        if (score < _minScore)
                            _minScore = score;
                        id++;
                    }
                }
            }

            /// <summary>
            /// Viterbi segmentation of a single word (already metaspace
            /// prefixed) into the highest scoring sequence of vocabulary pieces.
            /// </summary>
            private List<int> SegmentWord(String word)
            {
                int n = word.Length;
                double[] bestScore = new double[n + 1];
                int[] bestLength = new int[n + 1];
                for (int i = 1; i <= n; i++)
                    bestScore[i] = double.NegativeInfinity;

                for (int i = 0; i < n; i++)
                {
                    if (double.IsNegativeInfinity(bestScore[i]))
                        continue;
                    int maxLen = Math.Min(_maxPieceLength, n - i);
                    for (int len = 1; len <= maxLen; len++)
                    {
                        String piece = word.Substring(i, len);
                        double score;
                        if (_pieceScores.TryGetValue(piece, out score))
                        {
                            if (bestScore[i] + score > bestScore[i + len])
                            {
                                bestScore[i + len] = bestScore[i] + score;
                                bestLength[i + len] = len;
                            }
                        }
                    }
                    //Byte fallback for characters with no vocabulary piece:
                    //give the single character a score below any real piece so
                    //it is only used when nothing else covers the position.
                    if (double.IsNegativeInfinity(bestScore[i + 1]))
                    {
                        bestScore[i + 1] = bestScore[i] + _minScore - 10;
                        bestLength[i + 1] = 1;
                    }
                }

                //Backtrack into pieces
                List<String> pieces = new List<String>();
                for (int i = n; i > 0; i -= bestLength[i])
                    pieces.Insert(0, word.Substring(i - bestLength[i], bestLength[i]));

                List<int> ids = new List<int>();
                foreach (String piece in pieces)
                {
                    int id;
                    if (_pieceToId.TryGetValue(piece, out id))
                        ids.Add(id);
                    else
                    {
                        //Encode the UTF-8 bytes of the piece with the <0xNN>
                        //byte pieces.
                        foreach (byte b in Encoding.UTF8.GetBytes(piece))
                        {
                            String bytePiece = String.Format("<0x{0:X2}>", b);
                            if (_pieceToId.TryGetValue(bytePiece, out id))
                                ids.Add(id);
                        }
                    }
                }
                return ids;
            }

            /// <summary>
            /// Encode the text to token ids (without the end of sequence token).
            /// </summary>
            public List<int> Encode(String text)
            {
                //Normalizer: lowercase, remove punctuation, collapse whitespace, strip.
                text = text.ToLowerInvariant();
                text = _punctuation.Replace(text, "");
                text = Regex.Replace(text, @"\s+", " ").Trim();

                List<int> ids = new List<int>();
                if (text.Length == 0)
                    return ids;

                //Metaspace pre-tokenizer: split into words, each prefixed with
                //the metaspace character.
                foreach (String word in text.Split(' '))
                {
                    if (word.Length == 0)
                        continue;
                    ids.AddRange(SegmentWord(Metaspace + word));
                }
                return ids;
            }
        }
    }
}
