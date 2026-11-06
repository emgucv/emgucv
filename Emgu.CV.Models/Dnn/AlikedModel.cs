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
    /// ALIKED deep learning feature extractor with automatic ONNX model download.
    /// Inherits from <see cref="ALIKED"/> and adds an <see cref="Init"/> method that
    /// downloads the model before creating the native object.
    /// </summary>
    public class AlikedModel : ALIKED
    {
        private readonly String _modelFolderName = Path.Combine("emgu", "aliked");

        private readonly Size _inputSize;
        private readonly bool _normalizeDescriptors;
        private readonly Emgu.CV.Dnn.EngineType _engine;
        private readonly Emgu.CV.Dnn.Backend _backend;
        private readonly Emgu.CV.Dnn.Target _target;

        /// <summary>
        /// Create an AlikedModel instance. Parameters mirror those of <see cref="ALIKED"/>;
        /// the native object is not created until <see cref="Init"/> is called.
        /// </summary>
        /// <param name="inputSize">Input image size for the network. Use an empty size (the default) for the network's default of 640×640.</param>
        /// <param name="normalizeDescriptors">Whether to L2-normalize descriptors.</param>
        /// <param name="engine">DNN engine type.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public AlikedModel(
            Size inputSize = new Size(),
            bool normalizeDescriptors = true,
            Emgu.CV.Dnn.EngineType engine = Emgu.CV.Dnn.EngineType.OpenCV,
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
            : base()
        {
            _inputSize = inputSize;
            _normalizeDescriptors = normalizeDescriptors;
            _engine = engine;
            _backend = backend;
            _target = target;
        }

        /// <summary>
        /// Return true if the model has been initialised.
        /// </summary>
        public bool Initialized => _ptr != IntPtr.Zero;

        /// <summary>
        /// Download the ALIKED ONNX model and initialise the native feature extractor.
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
                    "https://raw.githubusercontent.com/YangGuanyuhan/lightglue_opencv_project/main/model/aliked-n16rot-top1k-640.onnx",
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
                        _inputSize,
                        _normalizeDescriptors,
                        _engine,
                        _backend,
                        _target);
                }
            }
        }
    }
}
