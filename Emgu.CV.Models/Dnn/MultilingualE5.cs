//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// multilingual-e5-small sentence embedding model running on the dnn
    /// module. Sentences in ~100 languages are embedded into a shared 384
    /// dimensional space where semantically similar sentences are close, even
    /// across languages, enabling multilingual and cross-lingual semantic
    /// search.
    ///
    /// E5 models are trained with instruction prefixes: search queries (and
    /// texts compared symmetrically) should be embedded with the "query: "
    /// prefix and documents with the "passage: " prefix; EmbedText applies the
    /// prefix automatically.
    ///
    /// The ONNX model (~450 MB, fp32) is the raw `optimum-cli export onnx
    /// --model intfloat/multilingual-e5-small` export. The XLM-RoBERTa
    /// SentencePiece Unigram tokenizer is implemented in this class
    /// (cv::dnn::Tokenizer only supports Gemma-style BPE tokenizer files).
    /// The precompiled SentencePiece normalization table is approximated with
    /// unicode NFKC normalization, which matches it for typical text.
    /// </summary>
    public class MultilingualE5 : DisposableObject
    {
        private const int BosId = 0;   // <s>
        private const int EosId = 2;   // </s>
        private const int MaxSequenceLength = 512;

        private String _modelFolderName = Path.Combine("emgu", "multilingual_e5_small_onnx");

        private Net _net = null;
        private UnigramTokenizer _tokenizer = null;

        /// <summary>
        /// Create a multilingual-e5-small sentence embedding model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public MultilingualE5(String modelFolderName = null)
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
        /// Download and initialize the multilingual E5 model and its tokenizer.
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
                    "https://emgu-public.s3.amazonaws.com/multilingual_e5_small_onnx/tokenizer.json",
                    _modelFolderName,
                    "0B44A9D7B51C3C62626640CDA0E2C2F70FDACDC25BBBD68038369D14EBDF4C39");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/multilingual_e5_small_onnx/model.onnx",
                    _modelFolderName,
                    "D84989604FA2EACF522487750C14ED6A1E06D4DF73C0ABEFAF80069FC1F879FB");

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
        /// Tokenize the given text (without the E5 prefix) with the XLM-RoBERTa
        /// SentencePiece tokenizer, wrapped in the &lt;s&gt; and &lt;/s&gt; tokens.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <returns>The token ids, including &lt;s&gt; and &lt;/s&gt;.</returns>
        public int[] Tokenize(String text)
        {
            if (_tokenizer == null)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            List<int> ids = _tokenizer.Encode(text);
            if (ids.Count > MaxSequenceLength - 2)
                ids.RemoveRange(MaxSequenceLength - 2, ids.Count - (MaxSequenceLength - 2));
            ids.Insert(0, BosId);
            ids.Add(EosId);
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
        /// Embed the given text into the multilingual sentence embedding space.
        /// </summary>
        /// <param name="text">The sentence to embed, in any of the ~100 supported languages.</param>
        /// <param name="isPassage">False (default) applies the "query: " prefix, used for search queries and symmetric text comparison; true applies the "passage: " prefix, used for the documents of a retrieval corpus.</param>
        /// <returns>The L2-normalized sentence embedding (384 values), the mean of the token embeddings.</returns>
        public float[] EmbedText(String text, bool isPassage = false)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            int[] tokens = Tokenize((isPassage ? "passage: " : "query: ") + text);
            long[] inputIds = new long[tokens.Length];
            long[] attention = new long[tokens.Length];
            long[] tokenTypes = new long[tokens.Length];
            for (int i = 0; i < tokens.Length; i++)
            {
                inputIds[i] = tokens[i];
                attention[i] = 1;
            }

            SetInt64Input(inputIds, "input_ids");
            SetInt64Input(attention, "attention_mask");
            SetInt64Input(tokenTypes, "token_type_ids");

            using (Mat lastHiddenState = _net.Forward())   //(1, tokens, dim)
            {
                int tokenCount = lastHiddenState.SizeOfDimension[1];
                int dim = lastHiddenState.SizeOfDimension[2];
                float[] hidden = new float[tokenCount * dim];
                Marshal.Copy(lastHiddenState.DataPointer, hidden, 0, hidden.Length);

                //Average pooling over the token embeddings (all tokens are
                //real, no padding is used for a single sentence).
                float[] result = new float[dim];
                for (int t = 0; t < tokenCount; t++)
                    for (int d = 0; d < dim; d++)
                        result[d] += hidden[t * dim + d];

                double norm = 0;
                for (int d = 0; d < dim; d++)
                {
                    result[d] /= tokenCount;
                    norm += (double)result[d] * result[d];
                }
                norm = Math.Sqrt(norm);
                if (norm > 0)
                {
                    for (int d = 0; d < dim; d++)
                        result[d] = (float)(result[d] / norm);
                }
                return result;
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

            _tokenizer = null;
        }

        /// <summary>
        /// Release the memory associated with this model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// The XLM-RoBERTa SentencePiece Unigram tokenizer: the text is NFKC
        /// normalized with repeated spaces collapsed (approximating the
        /// precompiled SentencePiece normalization table), split into words
        /// each prefixed with the metaspace character (U+2581), and every word
        /// is segmented with Viterbi search maximizing the sum of the piece
        /// scores. Characters not covered by the vocabulary map to
        /// &lt;unk&gt;.
        /// </summary>
        private class UnigramTokenizer
        {
            private const char Metaspace = '▁';
            private const int UnkId = 3;

            private readonly Dictionary<String, int> _pieceToId = new Dictionary<String, int>();
            private readonly Dictionary<String, double> _pieceScores = new Dictionary<String, double>();
            private readonly int _maxPieceLength = 1;
            private readonly double _minScore = 0;

            public UnigramTokenizer(String tokenizerJsonPath)
            {
                using (FileStream stream = File.OpenRead(tokenizerJsonPath))
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(stream))
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
                    //Characters with no covering piece fall back to a single
                    //character with a score below any real piece, mapped to
                    //<unk> when emitting ids.
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
                        ids.Add(UnkId);
                }
                return ids;
            }

            /// <summary>
            /// Encode the text to token ids (without the &lt;s&gt; / &lt;/s&gt; tokens).
            /// </summary>
            public List<int> Encode(String text)
            {
                //Approximation of the precompiled SentencePiece normalization:
                //unicode NFKC, then collapse repeated spaces (the explicit
                //Replace rule of the tokenizer).
                text = text.Normalize(NormalizationForm.FormKC);
                text = Regex.Replace(text, @" {2,}", " ").Trim();

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
