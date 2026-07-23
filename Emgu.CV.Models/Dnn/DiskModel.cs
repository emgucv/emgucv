//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Features;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// DISK deep learning feature extractor with automatic ONNX model download.
    /// Inherits from <see cref="DISK"/> and adds an <see cref="Init"/> method that
    /// downloads the model before creating the native object.
    /// </summary>
    public class DiskModel : DISK
    {
        private readonly String _modelFolderName = Path.Combine("emgu", "disk");

        private readonly int _maxKeypoints;
        private readonly float _scoreThreshold;
        private readonly Size _imageSize;
        private readonly Emgu.CV.Dnn.Backend _backend;
        private readonly Emgu.CV.Dnn.Target _target;

        /// <summary>
        /// Create a DiskModel instance. Parameters mirror those of <see cref="DISK"/>;
        /// the native object is not created until <see cref="Init"/> is called.
        /// </summary>
        /// <param name="maxKeypoints">
        /// Maximum number of keypoints to return per image. The strongest responses are kept;
        /// -1 keeps all detections.
        /// </param>
        /// <param name="scoreThreshold">Discard keypoints with network score below this value.</param>
        /// <param name="imageSize">
        /// Input size fed to the network. Use the default empty size to use the network's
        /// built-in fixed input shape of 1024×1024. When overriding, both dimensions must be
        /// positive multiples of 16.
        /// </param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public DiskModel(
            int maxKeypoints = -1,
            float scoreThreshold = 0.0f,
            Size imageSize = new Size(),
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
            : base()
        {
            _maxKeypoints = maxKeypoints;
            _scoreThreshold = scoreThreshold;
            _imageSize = imageSize;
            _backend = backend;
            _target = target;
        }

        /// <summary>
        /// Return true if the model has been initialised.
        /// </summary>
        public bool Initialized => _ptr != IntPtr.Zero;

        /// <summary>
        /// Download the DISK ONNX model and initialise the native feature extractor.
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
                    "https://github.com/fabio-sim/LightGlue-ONNX/releases/download/v0.1.0/disk.onnx",
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
                        _maxKeypoints,
                        _scoreThreshold,
                        _imageSize,
                        _backend,
                        _target);
                }
            }
        }
    }
}
