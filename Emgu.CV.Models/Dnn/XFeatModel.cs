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
    /// XFeat deep learning feature extractor with automatic ONNX model download.
    /// Inherits from <see cref="XFeat"/> and adds an <see cref="Init"/> method that
    /// downloads the model before creating the native object.
    /// </summary>
    public class XFeatModel : XFeat
    {
        private readonly String _modelFolderName = Path.Combine("emgu", "xfeat");

        private readonly int _maxKeypoints;
        private readonly float _scoreThreshold;
        private readonly Size _inputSize;
        private readonly Emgu.CV.Dnn.Backend _backend;
        private readonly Emgu.CV.Dnn.Target _target;

        /// <summary>
        /// Create an XFeatModel instance. Parameters mirror those of <see cref="XFeat"/>;
        /// the native object is not created until <see cref="Init"/> is called.
        /// </summary>
        /// <param name="maxKeypoints">
        /// Maximum number of keypoints to return per image. The strongest responses are kept;
        /// -1 keeps all detections.
        /// </param>
        /// <param name="scoreThreshold">Discard keypoints with network score not greater than this value.</param>
        /// <param name="inputSize">Input size fed to the network. Use the default empty size for 640x640.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public XFeatModel(
            int maxKeypoints = -1,
            float scoreThreshold = 0.5f,
            Size inputSize = new Size(),
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
            : base()
        {
            _maxKeypoints = maxKeypoints;
            _scoreThreshold = scoreThreshold;
            _inputSize = inputSize;
            _backend = backend;
            _target = target;
        }

        /// <summary>
        /// Return true if the model has been initialised.
        /// </summary>
        public bool Initialized => _ptr != IntPtr.Zero;

        /// <summary>
        /// Download the XFeat ONNX model and initialise the native feature extractor.
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

                //The XFeat model that OpenCV itself uses for its regression tests.
                manager.AddFile(
                    "https://raw.githubusercontent.com/opencv/opencv_extra/5.1.0/testdata/dnn/onnx/models/xfeat.onnx",
                    _modelFolderName,
                    "BE217628F409DDF3D10148276F99B96EEBD5B8A07A40F7B285652E18DE0E7EC4");

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
                        _inputSize,
                        _backend,
                        _target);
                }
            }
        }
    }
}
