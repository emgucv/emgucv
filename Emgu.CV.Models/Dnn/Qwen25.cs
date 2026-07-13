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
    /// Qwen2.5 0.5B instruction-tuned language model running on the dnn module,
    /// the C# equivalent of the OpenCV qwen_inference.py sample. Text is
    /// generated autoregressively with the KV-cache enabled: the prompt is
    /// prefilled once and each following forward pass only processes the newly
    /// generated token.
    ///
    /// The ONNX model (~2.4 GB, fp32) is the raw
    /// `optimum-cli export onnx --model Qwen/Qwen2.5-0.5B-Instruct
    /// --task causal-lm-with-past` export (Apache-2.0). Note that graph
    /// optimized exports (e.g. the transformers.js ones) contain onnxruntime
    /// contrib operations such as SimplifiedLayerNormalization that the dnn
    /// module does not support; only the raw export can be loaded.
    /// </summary>
    public class Qwen25 : DisposableObject
    {
        private const int EosId = 151643;    // <|endoftext|>
        private const int ImEndId = 151645;  // <|im_end|>

        private String _modelFolderName = Path.Combine("emgu", "qwen2.5_0.5b_instruct_onnx");

        private Tokenizer _tokenizer = null;
        private Net _net = null;

        /// <summary>
        /// Create a Qwen2.5 language model.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be downloaded to. An absolute path can also be used; files already present in the folder are not downloaded again.</param>
        public Qwen25(String modelFolderName = null)
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
        /// Download and initialize the Qwen2.5 model and its tokenizer.
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
                    "https://github.com/opencv/opencv_extra/raw/5.x/testdata/dnn/llm/qwen2.5/tokenizer.json",
                    _modelFolderName,
                    "C0382117EA329CDF097041132F6D735924B697924D6F6FC3945713E96CE87539");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/qwen2.5_0.5b_instruct_onnx/model.onnx",
                    _modelFolderName,
                    "74156907D86EDC92D259C35E875CF58684C9015ED144F15112DB8F07E092B313");

                manager.AddFile(
                    "https://emgu-public.s3.amazonaws.com/qwen2.5_0.5b_instruct_onnx/model.onnx_data",
                    _modelFolderName,
                    "72303EF1D77C02E4EFC46A35B5CC60A841213518BAA0DDAE6247D44F1686265A");

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

        private static long[] Sequence(int count)
        {
            long[] result = new long[count];
            for (int i = 0; i < count; i++)
                result[i] = i;
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
        /// Generate a response to the given user prompt using greedy decoding.
        /// </summary>
        /// <param name="prompt">The user prompt, e.g. "What is OpenCV?". It is wrapped in the ChatML format automatically.</param>
        /// <param name="maxNewTokens">Maximum number of new tokens to generate.</param>
        /// <returns>The generated response text.</returns>
        public String Generate(String prompt, int maxNewTokens = 64)
        {
            if (!Initialized)
                throw new InvalidOperationException("The model is not initialized. Call Init first.");

            //Qwen2.5 ChatML format
            String chatPrompt = "<|im_start|>user\n" + prompt + "<|im_end|>\n<|im_start|>assistant\n";

            int[] encoded = _tokenizer.Encode(chatPrompt);
            long[] inputIds = new long[encoded.Length];
            for (int i = 0; i < encoded.Length; i++)
                inputIds[i] = encoded[i];
            int promptLength = inputIds.Length;

            List<int> generated = new List<int>();

            _net.EnableKVCache();
            //Discard the state of any previous Generate call.
            _net.ResetKVCache();

            //Prefill: process the full prompt once to populate the KV-cache.
            SetInt64Input(inputIds, "input_ids");
            SetInt64Input(Ones(promptLength), "attention_mask");
            SetInt64Input(Sequence(promptLength), "position_ids");
            int newId = ForwardArgmax();
            generated.Add(newId);

            //Generate: feed one new token per step; the dnn module routes the
            //present key/values of each step back into the cache.
            for (int i = 0; i < maxNewTokens - 1; i++)
            {
                if (newId == EosId || newId == ImEndId)
                    break;

                int currentLength = promptLength + generated.Count;
                SetInt64Input(new long[] { newId }, "input_ids");
                SetInt64Input(Ones(currentLength), "attention_mask");
                SetInt64Input(new long[] { currentLength - 1 }, "position_ids");
                newId = ForwardArgmax();
                generated.Add(newId);
            }

            if (generated.Count > 0 && (generated[generated.Count - 1] == EosId || generated[generated.Count - 1] == ImEndId))
                generated.RemoveAt(generated.Count - 1);

            return _tokenizer.Decode(generated.ToArray());
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

            if (_net != null)
            {
                _net.Dispose();
                _net = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this language model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }
    }
}
