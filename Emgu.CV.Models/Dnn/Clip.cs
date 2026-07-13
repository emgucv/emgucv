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
    /// CLIP (ViT-B/32) joint text and image embedding model running on the dnn
    /// module. Text and images are embedded into a shared 512 dimensional
    /// space; the cosine similarity between the embeddings measures how well
    /// the text describes the image, enabling text-to-image search and
    /// zero-shot classification.
    ///
    /// The ONNX model (~580 MB, fp32) is the raw `optimum-cli export onnx
    /// --model openai/clip-vit-base-patch32` export, a single graph containing
    /// both the text and the vision tower. The CLIP BPE tokenizer is
    /// implemented in this class (cv::dnn::Tokenizer does not support the CLIP
    /// vocabulary format).
    /// </summary>
    public class Clip : DisposableObject
    {
        private const int SotId = 49406;   // <|startoftext|>
        private const int EotId = 49407;   // <|endoftext|>
        private const int ImageSize = 224;

        private String _modelFolderName = Path.Combine("emgu", "clip_vit_base_patch32_onnx");

        private Net _net = null;
        private ClipBpeTokenizer _tokenizer = null;

        //A cached blob fed to the tower that is not being queried: the exported
        //graph is a single network with both towers, so both inputs must be set.
        private Mat _dummyPixelValues = null;
        private long[] _dummyInputIds = new long[] { SotId, EotId };

        /// <summary>
        /// Create a CLIP embedding model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public Clip(String modelFolderName = null)
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
        /// Download and initialize the CLIP model and its tokenizer.
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
                    "https://huggingface.co/openai/clip-vit-base-patch32/resolve/main/vocab.json",
                    _modelFolderName,
                    "5047B556CE86CCAF6AA22B3FFCCFC52D391EA4ACCDAB9C2F2407DA5B742D4363");

                manager.AddFile(
                    "https://huggingface.co/openai/clip-vit-base-patch32/resolve/main/merges.txt",
                    _modelFolderName,
                    "9FD691F7C8039210E0FCED15865466C65820D09B63988B0174BFE25DE299051A");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/clip_vit_base_patch32_onnx/model.onnx",
                    _modelFolderName,
                    "75BA5833675245A35B3E1A7091E117B1470D8D10041E04C4D49A96926FD94046");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _tokenizer = new ClipBpeTokenizer(manager.Files[0].LocalFile, manager.Files[1].LocalFile);
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                    _net = DnnInvoke.ReadNetFromONNX(manager.Files[2].LocalFile, EngineType.New);
#else
                    String modelPath = manager.Files[2].LocalFile;
                    _net = await Task.Run(() => DnnInvoke.ReadNetFromONNX(modelPath, EngineType.New));
#endif
                }
            }
        }

        /// <summary>
        /// Tokenize the given text with the CLIP BPE tokenizer, wrapped in the
        /// start/end of text tokens.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <returns>The token ids, including the start/end of text tokens.</returns>
        public int[] Tokenize(String text)
        {
            if (_tokenizer == null)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");
            List<int> ids = new List<int>();
            ids.Add(SotId);
            ids.AddRange(_tokenizer.Encode(text));
            ids.Add(EotId);
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
        /// CLIP image preprocessing: resize the shortest side to 224 (bicubic),
        /// center crop 224x224, convert to RGB and normalize with the CLIP
        /// per-channel mean and std.
        /// </summary>
        private static Mat PreprocessImage(Mat image)
        {
            int w = image.Width, h = image.Height;
            double scale = ImageSize / (double)Math.Min(w, h);
            int resizedW = Math.Max(ImageSize, (int)Math.Round(w * scale));
            int resizedH = Math.Max(ImageSize, (int)Math.Round(h * scale));

            using (Mat resized = new Mat())
            using (ScalarArray mean = new ScalarArray(new MCvScalar(0.48145466, 0.4578275, 0.40821073)))
            using (ScalarArray std = new ScalarArray(new MCvScalar(0.26862954, 0.26130258, 0.27577711)))
            {
                CvInvoke.Resize(image, resized, new Size(resizedW, resizedH), 0, 0, Inter.Cubic);
                Rectangle crop = new Rectangle((resizedW - ImageSize) / 2, (resizedH - ImageSize) / 2, ImageSize, ImageSize);
                using (Mat cropped = new Mat(resized, crop))
                using (Mat rgb = new Mat())
                using (Mat f32 = new Mat())
                {
                    CvInvoke.CvtColor(cropped, rgb, ColorConversion.Bgr2Rgb);
                    rgb.ConvertTo(f32, DepthType.Cv32F, 1.0 / 255.0);
                    CvInvoke.Subtract(f32, mean, f32, null, DepthType.Cv32F);
                    CvInvoke.Divide(f32, std, f32, 1.0, DepthType.Cv32F);
                    return DnnInvoke.BlobFromImage(f32);
                }
            }
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
        /// <param name="text">The text, e.g. "a photo of a cat".</param>
        /// <returns>The L2-normalized embedding (512 values for ViT-B/32).</returns>
        public float[] EmbedText(String text)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            int[] tokens = Tokenize(text);
            long[] inputIds = new long[tokens.Length];
            long[] attention = new long[tokens.Length];
            for (int i = 0; i < tokens.Length; i++)
            {
                inputIds[i] = tokens[i];
                attention[i] = 1;
            }

            SetInt64Input(inputIds, "input_ids");
            SetInt64Input(attention, "attention_mask");
            _net.SetInput(GetDummyPixelValues(), "pixel_values");
            return ReadNormalizedEmbedding("text_embeds");
        }

        /// <summary>
        /// Embed the given image into the shared text-image embedding space.
        /// </summary>
        /// <param name="image">The image (BGR).</param>
        /// <returns>The L2-normalized embedding (512 values for ViT-B/32).</returns>
        public float[] EmbedImage(IInputArray image)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            using (Mat pixelValues = PreprocessImage(mat))
            {
                long[] attention = new long[_dummyInputIds.Length];
                for (int i = 0; i < attention.Length; i++)
                    attention[i] = 1;
                SetInt64Input(_dummyInputIds, "input_ids");
                SetInt64Input(attention, "attention_mask");
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
            if (a.Length != b.Length)
                throw new ArgumentException("The embeddings must have the same length.");
            double dot = 0, normA = 0, normB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dot += (double)a[i] * b[i];
                normA += (double)a[i] * a[i];
                normB += (double)b[i] * b[i];
            }
            if (normA == 0 || normB == 0)
                return 0;
            return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
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
        /// Release the memory associated with this CLIP model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// The CLIP BPE tokenizer: lowercased text is split with the CLIP
        /// regex, each word is mapped to unicode characters byte-by-byte
        /// (GPT-2 style byte-to-unicode table) with "&lt;/w&gt;" appended to the
        /// last character, and byte-pair merges are applied in rank order.
        /// </summary>
        private class ClipBpeTokenizer
        {
            private readonly Dictionary<String, int> _vocab = new Dictionary<String, int>();
            private readonly Dictionary<String, int> _mergeRanks = new Dictionary<String, int>();
            private readonly Dictionary<byte, char> _byteToUnicode = new Dictionary<byte, char>();
            private static readonly Regex _splitter = new Regex(
                @"<\|startoftext\|>|<\|endoftext\|>|'s|'t|'re|'ve|'m|'ll|'d|[\p{L}]+|[\p{N}]|[^\s\p{L}\p{N}]+",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            public ClipBpeTokenizer(String vocabJsonPath, String mergesTxtPath)
            {
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(vocabJsonPath)))
                {
                    foreach (System.Text.Json.JsonProperty p in doc.RootElement.EnumerateObject())
                        _vocab[p.Name] = p.Value.GetInt32();
                }

                String[] mergeLines = File.ReadAllLines(mergesTxtPath);
                int rank = 0;
                foreach (String line in mergeLines)
                {
                    if (line.StartsWith("#") || line.Trim().Length == 0)
                        continue;
                    _mergeRanks[line] = rank;
                    rank++;
                }

                //GPT-2 style byte-to-unicode table: printable bytes map to
                //themselves, the rest are shifted into the 256+ range.
                List<int> bs = new List<int>();
                for (int b = '!'; b <= '~'; b++) bs.Add(b);
                for (int b = 0xA1; b <= 0xAC; b++) bs.Add(b);
                for (int b = 0xAE; b <= 0xFF; b++) bs.Add(b);
                List<int> cs = new List<int>(bs);
                int n = 0;
                for (int b = 0; b < 256; b++)
                {
                    if (!bs.Contains(b))
                    {
                        bs.Add(b);
                        cs.Add(256 + n);
                        n++;
                    }
                }
                for (int i = 0; i < bs.Count; i++)
                    _byteToUnicode[(byte)bs[i]] = (char)cs[i];
            }

            /// <summary>
            /// Apply byte-pair merges to a word (a list of symbols, the last of
            /// which carries the "&lt;/w&gt;" suffix) in merge rank order.
            /// </summary>
            private List<String> Bpe(List<String> word)
            {
                while (word.Count > 1)
                {
                    int bestRank = int.MaxValue;
                    int bestIndex = -1;
                    for (int i = 0; i < word.Count - 1; i++)
                    {
                        int r;
                        if (_mergeRanks.TryGetValue(word[i] + " " + word[i + 1], out r) && r < bestRank)
                        {
                            bestRank = r;
                            bestIndex = i;
                        }
                    }
                    if (bestIndex < 0)
                        break;
                    word[bestIndex] = word[bestIndex] + word[bestIndex + 1];
                    word.RemoveAt(bestIndex + 1);
                }
                return word;
            }

            /// <summary>
            /// Encode the text to CLIP BPE token ids (without the start/end of text tokens).
            /// </summary>
            public List<int> Encode(String text)
            {
                List<int> ids = new List<int>();
                //CLIP canonicalizes the text: collapse whitespace and lowercase.
                text = Regex.Replace(text, @"\s+", " ").Trim().ToLowerInvariant();

                foreach (Match match in _splitter.Matches(text))
                {
                    byte[] utf8 = Encoding.UTF8.GetBytes(match.Value);
                    List<String> word = new List<String>();
                    foreach (byte b in utf8)
                        word.Add(_byteToUnicode[b].ToString());
                    if (word.Count == 0)
                        continue;
                    word[word.Count - 1] = word[word.Count - 1] + "</w>";

                    foreach (String piece in Bpe(word))
                    {
                        int id;
                        if (_vocab.TryGetValue(piece, out id))
                            ids.Add(id);
                    }
                }
                return ids;
            }
        }
    }
}
