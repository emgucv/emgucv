//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Features;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// LightGlue descriptor matcher with automatic ONNX model download.
    /// Inherits from <see cref="LightGlueMatcher"/> and adds an <see cref="Init"/> method
    /// that downloads the model before creating the native object.
    /// </summary>
    public class LightGlueMatcherModel : LightGlueMatcher
    {
        private readonly String _modelFolderName = Path.Combine("emgu", "lightglue");

        private readonly float _scoreThreshold;
        private readonly Emgu.CV.Dnn.Backend _backend;
        private readonly Emgu.CV.Dnn.Target _target;

        /// <summary>
        /// Create a LightGlueMatcherModel instance. Parameters mirror those of
        /// <see cref="LightGlueMatcher"/>; the native object is not created until
        /// <see cref="Init"/> is called.
        /// </summary>
        /// <param name="scoreThreshold">Match confidence threshold.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public LightGlueMatcherModel(
            float scoreThreshold = 0.0f,
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
            : base()
        {
            _scoreThreshold = scoreThreshold;
            _backend = backend;
            _target = target;
        }

        /// <summary>
        /// Return true if the model has been initialised.
        /// </summary>
        public bool Initialized => _ptr != IntPtr.Zero;

        /// <summary>
        /// Download the LightGlue ONNX model and initialise the native matcher.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback invoked during file download.</param>
        /// <param name="initOptions">Reserved; pass null.</param>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#endif
        {
            if (!Initialized)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://raw.githubusercontent.com/YangGuanyuhan/lightglue_opencv_project/main/model/aliked_lightglue.onnx",
                    _modelFolderName);

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    InitFromModelPath(
                        manager.Files[0].LocalFile,
                        _scoreThreshold,
                        _backend,
                        _target);
                }
            }
        }
    }
}
