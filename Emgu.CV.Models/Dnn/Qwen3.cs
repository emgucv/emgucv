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
    /// Qwen3 instruction-tuned language model (0.6B or 1.7B) running on the
    /// dnn module.
    /// Text is generated autoregressively with the KV-cache enabled: the
    /// prompt is prefilled once and each following forward pass only processes
    /// the newly generated token. The model is used in the non-thinking mode:
    /// an empty think block is inserted at the start of every assistant turn.
    ///
    /// The ONNX models (fp32, ~2.9 GB for 0.6B and ~7.6 GB for 1.7B) are the
    /// raw `optimum-cli export onnx --task causal-lm-with-past` exports
    /// (Apache-2.0). Note that graph
    /// optimized exports contain onnxruntime contrib operations that the dnn
    /// module does not support; only the raw export can be loaded.
    /// </summary>
    public class Qwen3 : DisposableObject
    {
        private const int EosId = 151643;    // <|endoftext|>
        private const int ImEndId = 151645;  // <|im_end|>

        //The empty think block appended after every assistant header for the
        //non-thinking mode: <think> "\n\n" </think> "\n\n". The think tokens
        //are new in qwen3 and unknown to the dnn tokenizer, so they are
        //appended as constant ids.
        private static readonly int[] EmptyThinkBlock = new int[] { 151667, 271, 151668, 271 };

        /// <summary>
        /// The Qwen3 model version.
        /// </summary>
        public enum Qwen3Version
        {
            /// <summary>
            /// Qwen3 0.6B: ~2.9 GB download, the fastest variant (~150 ms per token on an Apple Silicon CPU).
            /// </summary>
            Qwen3_0_6B,
            /// <summary>
            /// Qwen3 1.7B: ~7.6 GB download, better response quality at a slower generation speed.
            /// </summary>
            Qwen3_1_7B
        }

        private String _modelFolderName = null;

        private Tokenizer _tokenizer = null;
        private Net _net = null;

        /// <summary>
        /// Create a Qwen3 language model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public Qwen3(String modelFolderName = null)
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
                return _tokenizer != null && _net != null;
            }
        }

        /// <summary>
        /// Download and initialize the Qwen3 model and its tokenizer.
        /// </summary>
        /// <param name="version">The version of the Qwen3 model to use</param>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            Qwen3Version version = Qwen3Version.Qwen3_0_6B,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(
            Qwen3Version version = Qwen3Version.Qwen3_0_6B,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (!Initialized)
            {
                String folder = _modelFolderName ?? Path.Combine(
                    "emgu",
                    version == Qwen3Version.Qwen3_1_7B ? "qwen3_1.7b_onnx" : "qwen3_0.6b_onnx");

                FileDownloadManager manager = new FileDownloadManager();

                //The tokenizer is identical for all the qwen3 model sizes.
                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/qwen3_0.6b_onnx/tokenizer.json",
                    folder,
                    "AEB13307A71ACD8FE81861D94AD54AB689DF773318809EED3CBE794B4492DAE4");

                if (version == Qwen3Version.Qwen3_1_7B)
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/qwen3_1.7b_onnx/model.onnx",
                        folder,
                        "70A4E2C9A71E08A3AD2A2D11F26184D982CC3D836964F128508C4C068714DEFE");

                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/qwen3_1.7b_onnx/model.onnx_data",
                        folder,
                        "99A641B868EC75006BA984424ABC658250B3B0A9B7E33607EA587BC5F6E877FD");
                }
                else
                {
                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/qwen3_0.6b_onnx/model.onnx",
                        folder,
                        "D7D521EE92156D0DA75C22A04463824B79B754D995335EA65E3EBDB10C4C1F28");

                    manager.AddFile(
                        "https://emgu-public.s3.amazonaws.com/qwen3_0.6b_onnx/model.onnx_data",
                        folder,
                        "AA3AFC2DFA63B532D10E7DFA02F6584BD1B03CC2359098FE39F026D43616B477");
                }

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
                    //instead of downloading. The name avoids clashing with the
                    //huggingface model config.json that may reside in the same
                    //folder.
                    String configPath = Path.Combine(modelFolder, "opencv_tokenizer_config.json");
                    if (!File.Exists(configPath))
                        File.WriteAllText(configPath, "{ \"method\": \"BPE\", \"model_type\": \"qwen2.5\" }");

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
        /// Set the given values as an int64 (1, count) named input of the network.
        /// </summary>
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

        private static long[] Ones(int count)
        {
            long[] result = new long[count];
            for (int i = 0; i < count; i++)
                result[i] = 1;
            return result;
        }

        private static long[] Sequence(int start, int count)
        {
            long[] result = new long[count];
            for (int i = 0; i < count; i++)
                result[i] = start + i;
            return result;
        }

        /// <summary>
        /// Run a forward pass and return the argmax token id of the last sequence position.
        /// </summary>
        private int ForwardArgmax()
        {
            using (Mat logits = _net.Forward())   //(1, sequence, vocabulary)
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

        /// <summary>
        /// The number of tokens whose key/values currently reside in the KV-cache.
        /// </summary>
        private int _cachedTokens = 0;

        /// <summary>
        /// The full ChatML transcript of the chat session as token ids.
        /// </summary>
        private List<long> _transcript = new List<long>();

        /// <summary>
        /// Reset the chat session, clearing the conversation history and the KV-cache.
        /// </summary>
        public void ResetChat()
        {
            _cachedTokens = 0;
            _transcript.Clear();
            if (_net != null)
                _net.ResetKVCache();
        }

        /// <summary>
        /// Feed a chunk of tokens into the network (populating the KV-cache) and
        /// return the argmax token id predicted after the last token.
        /// </summary>
        private int ForwardChunk(long[] tokens)
        {
            SetInt64Input(tokens, "input_ids");
            SetInt64Input(Ones(_cachedTokens + tokens.Length), "attention_mask");
            SetInt64Input(Sequence(_cachedTokens, tokens.Length), "position_ids");
            int newId = ForwardArgmax();
            _cachedTokens += tokens.Length;
            return newId;
        }

        /// <summary>
        /// Send a user message to the chat session and generate the response
        /// using greedy decoding. The model remembers the previous turns of the
        /// session: the full conversation transcript is re-prefilled in a single
        /// forward pass at the start of each turn (the dnn KV-cache does not yet
        /// support appending multiple tokens to a non-empty cache, so the cache
        /// cannot be carried across turns), and the response tokens are then
        /// generated one at a time with the KV-cache. Use ResetChat to start a
        /// new conversation.
        /// </summary>
        /// <param name="userMessage">The user message.</param>
        /// <param name="maxNewTokens">Maximum number of new tokens to generate.</param>
        /// <returns>The generated response text.</returns>
        public String Chat(String userMessage, int maxNewTokens = 64)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            //Qwen2.5 ChatML format. The first turn opens the conversation; the
            //following turns continue after the previous assistant response.
            String turn = (_transcript.Count == 0)
                ? "<|im_start|>user\n" + userMessage + "<|im_end|>\n<|im_start|>assistant\n"
                : "\n<|im_start|>user\n" + userMessage + "<|im_end|>\n<|im_start|>assistant\n";

            foreach (int id in _tokenizer.Encode(turn))
                _transcript.Add(id);
            //Non-thinking mode: start the assistant turn with an empty think block.
            foreach (int id in EmptyThinkBlock)
                _transcript.Add(id);

            //Prefill: process the full transcript in one pass into a fresh
            //KV-cache.
            _net.EnableKVCache();
            _net.ResetKVCache();
            _cachedTokens = 0;
            int newId = ForwardChunk(_transcript.ToArray());

            List<int> generated = new List<int>();
            generated.Add(newId);

            //Generate: feed one new token per step; the dnn module routes the
            //present key/values of each step back into the cache.
            for (int i = 0; i < maxNewTokens - 1; i++)
            {
                if (newId == EosId || newId == ImEndId)
                    break;

                newId = ForwardChunk(new long[] { newId });
                generated.Add(newId);
            }

            if (generated.Count > 0 && (generated[generated.Count - 1] == EosId || generated[generated.Count - 1] == ImEndId))
                generated.RemoveAt(generated.Count - 1);

            //Append the assistant response to the transcript, closing the turn
            //with <|im_end|>.
            foreach (int id in generated)
                _transcript.Add(id);
            _transcript.Add(ImEndId);

            return _tokenizer.Decode(generated.ToArray());
        }

        /// <summary>
        /// Generate a response to a single user prompt using greedy decoding.
        /// Any ongoing chat session is discarded; this is equivalent to
        /// ResetChat followed by the first Chat turn.
        /// </summary>
        /// <param name="prompt">The user prompt, e.g. "What is OpenCV?". It is wrapped in the ChatML format automatically.</param>
        /// <param name="maxNewTokens">Maximum number of new tokens to generate.</param>
        /// <returns>The generated response text.</returns>
        public String Generate(String prompt, int maxNewTokens = 64)
        {
            ResetChat();
            return Chat(prompt, maxNewTokens);
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling Generate.
        /// </summary>
        public void Clear()
        {
            _cachedTokens = 0;
            _transcript.Clear();

            if (_tokenizer != null)
            {
                _tokenizer.Dispose();
                _tokenizer = null;
            }

            if (_net != null)
            {
                _net.Dispose();
                _net = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this Qwen3 language model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
