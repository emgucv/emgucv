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
    /// SmolVLM2 (256M, video instruct) vision-language model running on the
    /// dnn module. Given an image and a text prompt it generates a text
    /// response (e.g. a caption or an answer about the image) with KV-cached
    /// autoregressive decoding, making the generation interactive.
    ///
    /// The pipeline uses three ONNX networks: the SigLIP-style vision encoder
    /// turns each 512x512 image tile into 64 feature tokens, the embedding
    /// network turns prompt token ids into embeddings, and the Llama-based
    /// decoder (SmolLM2-135M backbone) generates the response from the
    /// combined embeddings. The vision encoder and the decoder are custom raw
    /// exports: the official ONNX exports contain onnxruntime contrib
    /// operations (SimplifiedLayerNormalization) and data dependent mask
    /// handling (GatherND over booleans) that the dnn module does not support;
    /// see https://github.com/emgucv/emgucv for the export recipe.
    /// </summary>
    public class SmolVlm2 : DisposableObject
    {
        private const int Hidden = 576;
        private const int TileSize = 512;
        private const int MaxResizedEdge = 2048;
        private const int ImageTokensPerTile = 64;
        private const int ImageTokenId = 49190;
        private const int EndOfUtteranceId = 49279;
        private const int EndOfTextId = 0;

        private String _modelFolderName = Path.Combine("emgu", "smolvlm2_256m_video_instruct_onnx");

        private Net _visionNet = null;
        private Net _embedNet = null;
        private Net _decoderNet = null;
        private SmolLmTokenizer _tokenizer = null;

        /// <summary>
        /// Create a SmolVLM2 vision-language model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public SmolVlm2(String modelFolderName = null)
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
                return _visionNet != null && _embedNet != null && _decoderNet != null && _tokenizer != null;
            }
        }

        /// <summary>
        /// Download and initialize the SmolVLM2 model and its tokenizer.
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
                String baseUrl = "https://emgu-public.s3.amazonaws.com/smolvlm2_256m_video_instruct_onnx/";

                manager.AddFile(baseUrl + "tokenizer.json", _modelFolderName,
                    "5ECE781DC8D2B2F3E2F289CA0AE50B17CFC27DD27BFE7971BB8241E0B964331A");
                manager.AddFile(baseUrl + "vision_raw.onnx", _modelFolderName,
                    "6681BE9BFFF8A837E2150E17FB4AF2C54D9700E2D957ECFF8251C8FAD8DC479D");
                manager.AddFile(baseUrl + "embed_tokens.onnx", _modelFolderName,
                    "05C2C945E08B8F87088A1E855FC363782C8E65F1F7283C30382D93B568D2BBF8");
                manager.AddFile(baseUrl + "decoder_raw.onnx", _modelFolderName,
                    "488A01BA791E2AA5D714D1F62A8CD854B7DE6BAB0F9AEC57C0B3CB22A219C53D");
                manager.AddFile(baseUrl + "decoder_raw.onnx_data", _modelFolderName,
                    "0563DD9AE5D9148BE536AEA259162BCE21C1D974BD5E4B093ABF607D94F254DF");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    String folder = Path.GetDirectoryName(manager.Files[0].LocalFile);
                    _tokenizer = new SmolLmTokenizer(Path.Combine(folder, "tokenizer.json"));
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                    _visionNet = DnnInvoke.ReadNetFromONNX(Path.Combine(folder, "vision_raw.onnx"), EngineType.New);
                    _embedNet = DnnInvoke.ReadNetFromONNX(Path.Combine(folder, "embed_tokens.onnx"), EngineType.New);
                    _decoderNet = DnnInvoke.ReadNetFromONNX(Path.Combine(folder, "decoder_raw.onnx"), EngineType.New);
#else
                    await Task.Run(() =>
                    {
                        _visionNet = DnnInvoke.ReadNetFromONNX(Path.Combine(folder, "vision_raw.onnx"), EngineType.New);
                        _embedNet = DnnInvoke.ReadNetFromONNX(Path.Combine(folder, "embed_tokens.onnx"), EngineType.New);
                        _decoderNet = DnnInvoke.ReadNetFromONNX(Path.Combine(folder, "decoder_raw.onnx"), EngineType.New);
                    });
#endif
                }
            }
        }

        /// <summary>
        /// Tokenize the given text (the special tokens of the prompt template
        /// are recognized and mapped to their ids).
        /// </summary>
        /// <param name="text">The text to tokenize.</param>
        /// <returns>The token ids.</returns>
        public int[] Tokenize(String text)
        {
            if (_tokenizer == null)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");
            return _tokenizer.Encode(text).ToArray();
        }

        /// <summary>
        /// Preprocess the image into vision encoder tiles: the image is resized
        /// to the 2048 longest edge, extended to multiples of the 512 tile size
        /// and split into a grid of 512x512 tiles, followed by a 512x512 global
        /// view of the whole image. Each tile is converted to RGB and
        /// normalized to [-1, 1].
        /// </summary>
        private static float[] PreprocessImage(Mat image, out int rows, out int cols)
        {
            int w = image.Width, h = image.Height;
            double scale = MaxResizedEdge / (double)Math.Max(w, h);
            int resizedW = Math.Max(1, (int)Math.Round(w * scale));
            int resizedH = Math.Max(1, (int)Math.Round(h * scale));
            cols = (resizedW + TileSize - 1) / TileSize;
            rows = (resizedH + TileSize - 1) / TileSize;

            int tileCount = rows * cols + 1;   //grid tiles + the global view
            int tileFloats = 3 * TileSize * TileSize;
            float[] result = new float[tileCount * tileFloats];

            using (Mat resized = new Mat())
            {
                //Resize to exact multiples of the tile size so the grid split is exact.
                CvInvoke.Resize(image, resized, new Size(cols * TileSize, rows * TileSize), 0, 0, Inter.Lanczos4);

                int tileIndex = 0;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        Rectangle roi = new Rectangle(c * TileSize, r * TileSize, TileSize, TileSize);
                        using (Mat tile = new Mat(resized, roi))
                        using (Mat blob = DnnInvoke.BlobFromImage(tile, 1.0 / 127.5, new Size(TileSize, TileSize), new MCvScalar(127.5, 127.5, 127.5), true, false))
                        {
                            Marshal.Copy(blob.DataPointer, result, tileIndex * tileFloats, tileFloats);
                        }
                        tileIndex++;
                    }
                }

                //Global view of the whole image
                using (Mat blob = DnnInvoke.BlobFromImage(image, 1.0 / 127.5, new Size(TileSize, TileSize), new MCvScalar(127.5, 127.5, 127.5), true, false))
                    Marshal.Copy(blob.DataPointer, result, tileIndex * tileFloats, tileFloats);
            }
            return result;
        }

        /// <summary>
        /// Build the SmolVLM2 chat prompt for a single image, expanding the
        /// image placeholders for the given tile grid.
        /// </summary>
        private static String BuildPrompt(String userPrompt, int rows, int cols)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<|im_start|>User:");
            String imageRun = String.Concat(System.Linq.Enumerable.Repeat("<image>", ImageTokensPerTile));
            for (int r = 1; r <= rows; r++)
            {
                for (int c = 1; c <= cols; c++)
                {
                    sb.Append("<fake_token_around_image>");
                    sb.Append(String.Format("<row_{0}_col_{1}>", r, c));
                    sb.Append(imageRun);
                }
                sb.Append("\n");
            }
            sb.Append("\n<fake_token_around_image><global-img>");
            sb.Append(imageRun);
            sb.Append("<fake_token_around_image>");
            sb.Append(userPrompt);
            sb.Append("<end_of_utterance>\nAssistant:");
            return sb.ToString();
        }

        private void SetInt64Input(long[] values, String inputName, Net net)
        {
            GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
            try
            {
                using (Mat input = new Mat(new int[] { 1, values.Length }, DepthType.Cv64S, handle.AddrOfPinnedObject()))
                    net.SetInput(input, inputName);
            }
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Run the embedding network on the given token ids, returning the flattened (count, hidden) embeddings.
        /// </summary>
        private float[] EmbedTokens(long[] tokenIds)
        {
            SetInt64Input(tokenIds, "input_ids", _embedNet);
            using (Mat output = _embedNet.Forward())
            {
                float[] result = new float[tokenIds.Length * Hidden];
                Marshal.Copy(output.DataPointer, result, 0, result.Length);
                return result;
            }
        }

        /// <summary>
        /// Run the decoder on the given (count, hidden) embeddings and return
        /// the argmax token id of the last position. The KV-cache holds the
        /// earlier part of the sequence.
        /// </summary>
        private int ForwardDecoder(float[] embeds, int count, int totalLength)
        {
            GCHandle handle = GCHandle.Alloc(embeds, GCHandleType.Pinned);
            try
            {
                long[] mask = new long[totalLength];
                for (int i = 0; i < totalLength; i++)
                    mask[i] = 1;
                long[] positions = new long[count];
                for (int i = 0; i < count; i++)
                    positions[i] = totalLength - count + i;

                using (Mat inputsEmbeds = new Mat(new int[] { 1, count, Hidden }, DepthType.Cv32F, handle.AddrOfPinnedObject()))
                {
                    _decoderNet.SetInput(inputsEmbeds, "inputs_embeds");
                    SetInt64Input(mask, "attention_mask", _decoderNet);
                    SetInt64Input(positions, "position_ids", _decoderNet);
                    using (Mat logits = _decoderNet.Forward("logits"))
                    {
                        int[] shape = logits.SizeOfDimension;
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
            finally
            {
                handle.Free();
            }
        }

        /// <summary>
        /// Generate a text response for the given image and prompt.
        /// </summary>
        /// <param name="image">The input image (BGR).</param>
        /// <param name="prompt">The user prompt, e.g. "Describe this image briefly.".</param>
        /// <param name="maxNewTokens">Maximum number of new tokens to generate.</param>
        /// <returns>The generated text response.</returns>
        public String Generate(IInputArray image, String prompt = "Describe this image briefly.", int maxNewTokens = 64)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            {
                //Vision: encode all the tiles into image feature tokens.
                int rows, cols;
                float[] pixelValues = PreprocessImage(mat, out rows, out cols);
                int tileCount = rows * cols + 1;
                float[] imageFeatures;
                GCHandle handle = GCHandle.Alloc(pixelValues, GCHandleType.Pinned);
                try
                {
                    using (Mat pv = new Mat(new int[] { tileCount, 3, TileSize, TileSize }, DepthType.Cv32F, handle.AddrOfPinnedObject()))
                    {
                        _visionNet.SetInput(pv, "pixel_values");
                        using (Mat features = _visionNet.Forward("image_features"))
                        {
                            imageFeatures = new float[tileCount * ImageTokensPerTile * Hidden];
                            Marshal.Copy(features.DataPointer, imageFeatures, 0, imageFeatures.Length);
                        }
                    }
                }
                finally
                {
                    handle.Free();
                }

                //Text: tokenize the chat prompt and embed the tokens.
                List<int> promptIds = _tokenizer.Encode(BuildPrompt(prompt, rows, cols));
                int promptLength = promptIds.Count;
                long[] promptIdsLong = new long[promptLength];
                for (int i = 0; i < promptLength; i++)
                    promptIdsLong[i] = promptIds[i];
                float[] promptEmbeds = EmbedTokens(promptIdsLong);

                //Splice the image features into the image token positions.
                int featureRow = 0;
                for (int t = 0; t < promptLength; t++)
                {
                    if (promptIds[t] == ImageTokenId)
                    {
                        Array.Copy(imageFeatures, featureRow * Hidden, promptEmbeds, t * Hidden, Hidden);
                        featureRow++;
                    }
                }

                //Decode with the KV-cache: prefill the full prompt once, then
                //feed a single new token per step with greedy decoding.
                _decoderNet.EnableKVCache();
                _decoderNet.ResetKVCache();

                List<int> generated = new List<int>();
                int newId = ForwardDecoder(promptEmbeds, promptLength, promptLength);
                generated.Add(newId);
                for (int i = 0; i < maxNewTokens - 1; i++)
                {
                    if (newId == EndOfUtteranceId || newId == EndOfTextId)
                        break;
                    float[] embeds = EmbedTokens(new long[] { newId });
                    newId = ForwardDecoder(embeds, 1, promptLength + generated.Count);
                    generated.Add(newId);
                }
                if (generated.Count > 0 && (generated[generated.Count - 1] == EndOfUtteranceId || generated[generated.Count - 1] == EndOfTextId))
                    generated.RemoveAt(generated.Count - 1);

                return _tokenizer.Decode(generated);
            }
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before use.
        /// </summary>
        public void Clear()
        {
            if (_visionNet != null) { _visionNet.Dispose(); _visionNet = null; }
            if (_embedNet != null) { _embedNet.Dispose(); _embedNet = null; }
            if (_decoderNet != null) { _decoderNet.Dispose(); _decoderNet = null; }
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
        /// The SmolLM2 byte-level BPE tokenizer, following the huggingface
        /// tokenizer.json specification: the text is split around the special
        /// (added) tokens, digits are isolated individually, the remaining
        /// segments are split with the GPT-2 regex and mapped to unicode
        /// characters byte-by-byte, and byte-pair merges are applied in rank
        /// order.
        /// </summary>
        private class SmolLmTokenizer
        {
            private readonly Dictionary<String, int> _vocab = new Dictionary<String, int>();
            private readonly Dictionary<int, String> _idToPiece = new Dictionary<int, String>();
            private readonly Dictionary<String, int> _mergeRanks = new Dictionary<String, int>();
            private readonly Dictionary<String, int> _specialTokens = new Dictionary<String, int>();
            private readonly HashSet<int> _specialIds = new HashSet<int>();
            private readonly List<String> _specialsByLength = new List<String>();
            private readonly Dictionary<byte, char> _byteToUnicode = new Dictionary<byte, char>();
            private readonly Dictionary<char, byte> _unicodeToByte = new Dictionary<char, byte>();

            private static readonly Regex _gpt2Splitter = new Regex(
                @"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+",
                RegexOptions.Compiled);

            public SmolLmTokenizer(String tokenizerJsonPath)
            {
                using (FileStream stream = File.OpenRead(tokenizerJsonPath))
                using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(stream))
                {
                    System.Text.Json.JsonElement model = doc.RootElement.GetProperty("model");
                    foreach (System.Text.Json.JsonProperty p in model.GetProperty("vocab").EnumerateObject())
                    {
                        _vocab[p.Name] = p.Value.GetInt32();
                        _idToPiece[p.Value.GetInt32()] = p.Name;
                    }

                    int rank = 0;
                    foreach (System.Text.Json.JsonElement merge in model.GetProperty("merges").EnumerateArray())
                    {
                        String pair = merge.ValueKind == System.Text.Json.JsonValueKind.Array
                            ? merge[0].GetString() + " " + merge[1].GetString()
                            : merge.GetString();
                        _mergeRanks[pair] = rank;
                        rank++;
                    }

                    System.Text.Json.JsonElement addedTokens;
                    if (doc.RootElement.TryGetProperty("added_tokens", out addedTokens))
                    {
                        foreach (System.Text.Json.JsonElement added in addedTokens.EnumerateArray())
                        {
                            String content = added.GetProperty("content").GetString();
                            int id = added.GetProperty("id").GetInt32();
                            _specialTokens[content] = id;
                            _specialIds.Add(id);
                            _idToPiece[id] = content;
                        }
                    }
                }

                _specialsByLength.AddRange(_specialTokens.Keys);
                _specialsByLength.Sort((a, b) => b.Length.CompareTo(a.Length));

                //GPT-2 style byte-to-unicode table
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
                {
                    _byteToUnicode[(byte)bs[i]] = (char)cs[i];
                    _unicodeToByte[(char)cs[i]] = (byte)bs[i];
                }
            }

            /// <summary>
            /// Apply byte-pair merges to a pre-token in merge rank order.
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
            /// BPE-encode a plain text segment (no special tokens).
            /// </summary>
            private void EncodeSegment(String segment, List<int> ids)
            {
                //Digits pre-tokenizer: every digit is isolated into its own
                //pre-token before the GPT-2 regex runs on the remaining parts.
                List<String> parts = new List<String>();
                StringBuilder current = new StringBuilder();
                foreach (char ch in segment)
                {
                    if (Char.IsDigit(ch))
                    {
                        if (current.Length > 0) { parts.Add(current.ToString()); current.Clear(); }
                        parts.Add(ch.ToString());
                    }
                    else
                        current.Append(ch);
                }
                if (current.Length > 0)
                    parts.Add(current.ToString());

                foreach (String part in parts)
                {
                    IEnumerable<String> preTokens;
                    if (part.Length == 1 && Char.IsDigit(part[0]))
                        preTokens = new String[] { part };
                    else
                    {
                        List<String> matches = new List<String>();
                        foreach (Match m in _gpt2Splitter.Matches(part))
                            matches.Add(m.Value);
                        preTokens = matches;
                    }

                    foreach (String preToken in preTokens)
                    {
                        List<String> word = new List<String>();
                        foreach (byte b in Encoding.UTF8.GetBytes(preToken))
                            word.Add(_byteToUnicode[b].ToString());
                        foreach (String piece in Bpe(word))
                        {
                            int id;
                            if (_vocab.TryGetValue(piece, out id))
                                ids.Add(id);
                        }
                    }
                }
            }

            /// <summary>
            /// Encode the text to token ids. Special (added) tokens in the text
            /// are recognized and mapped to their ids.
            /// </summary>
            public List<int> Encode(String text)
            {
                List<int> ids = new List<int>();
                int position = 0;
                int segmentStart = 0;
                while (position < text.Length)
                {
                    String matched = null;
                    if (text[position] == '<')
                    {
                        foreach (String special in _specialsByLength)
                        {
                            if (position + special.Length <= text.Length
                                && String.CompareOrdinal(text, position, special, 0, special.Length) == 0)
                            {
                                matched = special;
                                break;
                            }
                        }
                    }

                    if (matched != null)
                    {
                        if (position > segmentStart)
                            EncodeSegment(text.Substring(segmentStart, position - segmentStart), ids);
                        ids.Add(_specialTokens[matched]);
                        position += matched.Length;
                        segmentStart = position;
                    }
                    else
                        position++;
                }
                if (position > segmentStart)
                    EncodeSegment(text.Substring(segmentStart, position - segmentStart), ids);
                return ids;
            }

            /// <summary>
            /// Decode token ids back to text, skipping the special tokens.
            /// </summary>
            public String Decode(List<int> ids)
            {
                List<byte> bytes = new List<byte>();
                foreach (int id in ids)
                {
                    if (_specialIds.Contains(id))
                        continue;
                    String piece;
                    if (!_idToPiece.TryGetValue(id, out piece))
                        continue;
                    foreach (char c in piece)
                    {
                        byte b;
                        if (_unicodeToByte.TryGetValue(c, out b))
                            bytes.Add(b);
                    }
                }
                return Encoding.UTF8.GetString(bytes.ToArray());
            }
        }
    }
}
