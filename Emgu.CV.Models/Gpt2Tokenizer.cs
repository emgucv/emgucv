//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// A dnn Tokenizer using the GPT-2 BPE model. The tokenizer model file is
    /// downloaded on Init.
    /// </summary>
    public class Gpt2Tokenizer : DisposableObject
    {
        private String _modelFolderName = Path.Combine("emgu", "tokenizer", "gpt2");

        private Tokenizer _tokenizer = null;

        /// <summary>
        /// Create a GPT-2 tokenizer.
        /// </summary>
        /// <param name="modelFolderName">The subfolder name where the model will be saved to.</param>
        public Gpt2Tokenizer(String modelFolderName = null)
        {
            if (modelFolderName != null)
                _modelFolderName = modelFolderName;
        }

        /// <summary>
        /// Get the underlying dnn Tokenizer. Null if Init has not been called (or failed).
        /// </summary>
        public Tokenizer Tokenizer
        {
            get
            {
                return _tokenizer;
            }
        }

        /// <summary>
        /// Return true if the tokenizer is initialized.
        /// </summary>
        public bool Initialized
        {
            get
            {
                return _tokenizer != null;
            }
        }

        /// <summary>
        /// Download and initialize the GPT-2 tokenizer.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_tokenizer == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://huggingface.co/openai-community/gpt2/resolve/main/tokenizer.json",
                    _modelFolderName,
                    "8414CAB924D8B9B33013F0D221C5862F365EE9BE39C5C2BFAE8A5A9E970478A6");

                manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    //cv::dnn::Tokenizer::load expects the path to a config.json
                    //that declares the model type, with tokenizer.json in the
                    //same directory. The config is tiny and fixed for GPT-2, so
                    //we write it next to the downloaded tokenizer.json instead
                    //of downloading it.
                    String dir = Path.GetDirectoryName(manager.Files[0].LocalFile);
                    String configPath = Path.Combine(dir, "config.json");
                    if (!File.Exists(configPath))
                        File.WriteAllText(configPath, "{ \"model_type\": \"gpt2\" }");
                    _tokenizer = Tokenizer.Load(configPath);
                }
            }
        }

        /// <summary>
        /// Encode UTF-8 text to token ids.
        /// </summary>
        /// <param name="text">UTF-8 input string.</param>
        /// <returns>The token ids.</returns>
        public int[] Encode(String text)
        {
            return _tokenizer.Encode(text);
        }

        /// <summary>
        /// Decode token ids back to UTF-8 text.
        /// </summary>
        /// <param name="tokens">The token ids.</param>
        /// <returns>The decoded UTF-8 string.</returns>
        public String Decode(int[] tokens)
        {
            return _tokenizer.Decode(tokens);
        }

        /// <summary>
        /// Release the memory associated with this tokenizer.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_tokenizer != null)
            {
                _tokenizer.Dispose();
                _tokenizer = null;
            }
        }
    }
}
