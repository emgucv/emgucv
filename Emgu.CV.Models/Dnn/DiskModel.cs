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
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// DISK deep learning feature extractor.
    /// Automatically downloads the required ONNX model on first use.
    /// </summary>
    public class DiskModel : DisposableObject
    {
        private readonly String _modelFolderName = Path.Combine("emgu", "disk");

        private DISK _disk = null;

        private String _diskModelFile = null;

        private readonly int _maxKeypoints;
        private readonly float _scoreThreshold;
        private readonly Size _imageSize;

        /// <summary>
        /// Create a DiskModel instance.
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
        public DiskModel(int maxKeypoints = -1, float scoreThreshold = 0.0f, Size imageSize = new Size())
        {
            _maxKeypoints = maxKeypoints;
            _scoreThreshold = scoreThreshold;
            _imageSize = imageSize;
        }

        /// <summary>
        /// Return true if the model has been initialised.
        /// </summary>
        public bool Initialized => _diskModelFile != null;

        /// <summary>
        /// Download and initialise the DISK model.
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
                    _diskModelFile = manager.Files[0].LocalFile;
                }
            }
        }

        private DISK GetDisk()
        {
            if (_disk == null)
            {
                Size imageSize = _imageSize;
                _disk = new DISK(_diskModelFile, _maxKeypoints, _scoreThreshold, imageSize);
            }
            return _disk;
        }

        /// <summary>
        /// Detect keypoints and compute descriptors for an image.
        /// </summary>
        /// <param name="image">The input image.</param>
        /// <param name="keypoints">The detected keypoints.</param>
        /// <param name="descriptors">The computed descriptors (one row per keypoint).</param>
        public void DetectAndDescribe(IInputArray image, out MKeyPoint[] keypoints, out Mat descriptors)
        {
            descriptors = new Mat();
            using (VectorOfKeyPoint vkp = new VectorOfKeyPoint())
            {
                GetDisk().DetectAndCompute(image, null, vkp, descriptors, false);
                keypoints = vkp.ToArray();
            }
        }

        /// <inheritdoc />
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// Release the DISK model.
        /// </summary>
        public void Clear()
        {
            if (_disk != null)
            {
                _disk.Dispose();
                _disk = null;
            }
        }
    }
}
