//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// all-MiniLM-L6-v2 sentence embedding model running on the dnn module.
    /// Sentences are embedded into a 384 dimensional space where semantically
    /// similar sentences are close, enabling semantic text search, clustering
    /// and deduplication.
    ///
    /// The ONNX model (~87 MB, fp32) is the raw export shipped in the official
    /// sentence-transformers/all-MiniLM-L6-v2 repository. The BERT WordPiece
    /// tokenizer is implemented in this class (cv::dnn::Tokenizer does not
    /// support WordPiece vocabularies). The sentence embedding is the attention
    /// weighted mean of the token embeddings, L2-normalized, following the
    /// sentence-transformers pooling configuration.
    /// </summary>
    public class MiniLM : DisposableObject
    {
        private const int ClsId = 101;   // [CLS]
        private const int SepId = 102;   // [SEP]
        private const int UnkId = 100;   // [UNK]
        //The maximum sequence length of the sentence-transformers configuration.
        private const int MaxSequenceLength = 256;

        private String _modelFolderName = Path.Combine("emgu", "all_minilm_l6_v2_onnx");

        private Net _net = null;
        private WordPieceTokenizer _tokenizer = null;

        /// <summary>
        /// Create an all-MiniLM-L6-v2 sentence embedding model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public MiniLM(String modelFolderName = null)
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
        /// Download and initialize the MiniLM model and its tokenizer.
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
                    "https://emgu-public.s3.amazonaws.com/all_minilm_l6_v2_onnx/vocab.txt",
                    _modelFolderName,
                    "07ECED375CEC144D27C900241F3E339478DEC958F92FDDBC551F295C992038A3");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/all_minilm_l6_v2_onnx/model.onnx",
                    _modelFolderName,
                    "6FD5D72FE4589F189F8EBC006442DBB529BB7CE38F8082112682524616046452");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _tokenizer = new WordPieceTokenizer(manager.Files[0].LocalFile);
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
        /// Tokenize the given text with the BERT WordPiece tokenizer, wrapped
        /// in the [CLS] and [SEP] tokens.
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <returns>The token ids, including [CLS] and [SEP].</returns>
        public int[] Tokenize(String text)
        {
            if (_tokenizer == null)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            List<int> ids = _tokenizer.Encode(text);
            if (ids.Count > MaxSequenceLength - 2)
                ids.RemoveRange(MaxSequenceLength - 2, ids.Count - (MaxSequenceLength - 2));
            ids.Insert(0, ClsId);
            ids.Add(SepId);
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
        /// Embed the given text into the sentence embedding space.
        /// </summary>
        /// <param name="text">The sentence to embed.</param>
        /// <returns>The L2-normalized sentence embedding (384 values), the mean of the token embeddings.</returns>
        public float[] EmbedText(String text)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            int[] tokens = Tokenize(text);
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

                //Mean pooling over the token embeddings (all tokens are real,
                //no padding is used for a single sentence).
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
        /// Release the memory associated with this MiniLM model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// The BERT uncased WordPiece tokenizer: text is cleaned, lowercased
        /// and accent-stripped, punctuation is split into individual tokens,
        /// and every word is segmented with greedy longest-match against the
        /// vocabulary, continuation pieces carrying the "##" prefix.
        /// </summary>
        private class WordPieceTokenizer
        {
            private const int MaxWordLength = 100;

            private readonly Dictionary<String, int> _vocab = new Dictionary<String, int>();

            public WordPieceTokenizer(String vocabTxtPath)
            {
                String[] lines = File.ReadAllLines(vocabTxtPath);
                for (int i = 0; i < lines.Length; i++)
                    _vocab[lines[i]] = i;
            }

            private static bool IsBertPunctuation(char c)
            {
                //BERT treats the ASCII symbol ranges as punctuation in addition
                //to the unicode punctuation categories.
                if ((c >= 33 && c <= 47) || (c >= 58 && c <= 64) || (c >= 91 && c <= 96) || (c >= 123 && c <= 126))
                    return true;
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);
                return category == UnicodeCategory.ConnectorPunctuation
                    || category == UnicodeCategory.DashPunctuation
                    || category == UnicodeCategory.OpenPunctuation
                    || category == UnicodeCategory.ClosePunctuation
                    || category == UnicodeCategory.InitialQuotePunctuation
                    || category == UnicodeCategory.FinalQuotePunctuation
                    || category == UnicodeCategory.OtherPunctuation;
            }

            private static bool IsCjk(char c)
            {
                return (c >= 0x4E00 && c <= 0x9FFF) || (c >= 0x3400 && c <= 0x4DBF)
                    || (c >= 0xF900 && c <= 0xFAFF);
            }

            /// <summary>
            /// BERT basic tokenization: clean, lowercase, strip accents, space
            /// out CJK characters and split punctuation, then split on
            /// whitespace.
            /// </summary>
            private static List<String> BasicTokenize(String text)
            {
                //Lowercase and strip accents (NFD, dropping combining marks).
                text = text.ToLowerInvariant().Normalize(NormalizationForm.FormD);

                StringBuilder builder = new StringBuilder();
                foreach (char c in text)
                {
                    if (c == 0 || c == 0xFFFD || Char.IsControl(c))
                        continue;
                    if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark)
                        continue;
                    if (Char.IsWhiteSpace(c))
                    {
                        builder.Append(' ');
                    }
                    else if (IsCjk(c) || IsBertPunctuation(c))
                    {
                        //CJK characters and punctuation become standalone tokens.
                        builder.Append(' ');
                        builder.Append(c);
                        builder.Append(' ');
                    }
                    else
                    {
                        builder.Append(c);
                    }
                }

                List<String> tokens = new List<String>();
                foreach (String token in builder.ToString().Split(' '))
                {
                    if (token.Length > 0)
                        tokens.Add(token);
                }
                return tokens;
            }

            /// <summary>
            /// Encode the text to WordPiece token ids (without [CLS] / [SEP]).
            /// </summary>
            public List<int> Encode(String text)
            {
                List<int> ids = new List<int>();
                foreach (String word in BasicTokenize(text))
                {
                    if (word.Length > MaxWordLength)
                    {
                        ids.Add(UnkId);
                        continue;
                    }

                    //Greedy longest-match segmentation; continuation pieces are
                    //prefixed with "##".
                    List<int> wordIds = new List<int>();
                    int start = 0;
                    while (start < word.Length)
                    {
                        int end = word.Length;
                        int pieceId = -1;
                        while (end > start)
                        {
                            String piece = (start > 0 ? "##" : "") + word.Substring(start, end - start);
                            if (_vocab.TryGetValue(piece, out pieceId))
                                break;
                            pieceId = -1;
                            end--;
                        }
                        if (pieceId < 0)
                        {
                            //No piece covers this position: the whole word maps
                            //to [UNK].
                            wordIds = null;
                            break;
                        }
                        wordIds.Add(pieceId);
                        start = end;
                    }

                    if (wordIds == null)
                        ids.Add(UnkId);
                    else
                        ids.AddRange(wordIds);
                }
                return ids;
            }
        }
    }
}
