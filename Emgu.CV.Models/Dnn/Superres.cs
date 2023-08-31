//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.DnnSuperres;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Superres model
    /// </summary>
    public class Superres : DisposableObject, IProcessAndRenderModel
    {
        private String _modelFolderName = "superres";

        private DnnSuperResImpl _superresModel = null;

        /// <summary>
        /// The rendering method
        /// </summary>
        public RenderType RenderMethod
        {
            get
            {
                return RenderType.Overwrite;
            }
        }

        /// <summary>
        /// The Superres model version
        /// </summary>
        public enum SuperresVersion
        {   
            /// <summary>
            /// Superres Edsr x2
            /// </summary>
            EdsrX2,
            /// <summary>
            /// Superres Edsr x3
            /// </summary>
            EdsrX3,
            /// <summary>
            /// Superres Edsr x4
            /// </summary>
            EdsrX4,

            /// <summary>
            /// Superres Espcn x2
            /// </summary>
            EspcnX2,
            /// <summary>
            /// Superres Espcn x3
            /// </summary>
            EspcnX3,
            /// <summary>
            /// Superres Espcn x4
            /// </summary>
            EspcnX4,
            /// <summary>
            /// Superres Fsrcnn x2
            /// </summary>
            FsrcnnX2,
            /// <summary>
            /// Superres Fsrcnn x3
            /// </summary>
            FsrcnnX3,
            /// <summary>
            /// Superres Fsrcnn x4
            /// </summary>
            FsrcnnX4,
            /// <summary>
            /// Superres Lapsrn x2
            /// </summary>
            LapsrnX2,
            /// <summary>
            /// Superres Lapsrn x4
            /// </summary>
            LapsrnX4,
            /// <summary>
            /// Superres Lapsrn x8
            /// </summary>
            LapsrnX8


        }

        /// <summary>
        /// Return true if the model is initialized
        /// </summary>
        public bool Initialized
        {
            get
            {
                if (_superresModel == null)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Download and initialize the superres model
        /// </summary>
        /// <param name="version">The model version</param>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <returns>Asyn task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(
            SuperresVersion version = SuperresVersion.EdsrX4,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(
            SuperresVersion version = SuperresVersion.EdsrX4,
            FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_superresModel == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                String algorithm = String.Empty;
                int scale = -1;
                if (version == SuperresVersion.EdsrX2)
                {
                    manager.AddFile(
                        "https://github.com/Saafke/EDSR_Tensorflow/raw/master/models/EDSR_x2.pb",
                        _modelFolderName,
                        null);
                    algorithm = "edsr";
                    scale = 2;
                }
                else if (version == SuperresVersion.EdsrX3)
                {
                    manager.AddFile(
                        "https://github.com/Saafke/EDSR_Tensorflow/raw/master/models/EDSR_x3.pb",
                        _modelFolderName,
                        null);
                    algorithm = "edsr";
                    scale = 3;
                }
                else if (version == SuperresVersion.EdsrX4)
                {
                    manager.AddFile(
                        "https://github.com/Saafke/EDSR_Tensorflow/raw/master/models/EDSR_x4.pb",
                        _modelFolderName,
                        null);
                    algorithm = "edsr";
                    scale = 4;
                } else if (version == SuperresVersion.EspcnX2)
                {
                    manager.AddFile(
                        "https://github.com/fannymonori/TF-ESPCN/raw/master/export/ESPCN_x2.pb",
                        _modelFolderName,
                        null);
                    algorithm = "espcn";
                    scale = 2;
                }
                else if (version == SuperresVersion.EspcnX3)
                {
                    manager.AddFile(
                        "https://github.com/fannymonori/TF-ESPCN/raw/master/export/ESPCN_x3.pb",
                        _modelFolderName,
                        null);
                    algorithm = "espcn";
                    scale = 3;
                }
                else if (version == SuperresVersion.EspcnX4)
                {
                    manager.AddFile(
                        "https://github.com/fannymonori/TF-ESPCN/raw/master/export/ESPCN_x4.pb",
                        _modelFolderName,
                        null);
                    algorithm = "espcn";
                    scale = 4;
                }
                else if (version == SuperresVersion.FsrcnnX2)
                {
                    manager.AddFile(
                        "https://github.com/Saafke/FSRCNN_Tensorflow/raw/master/models/FSRCNN_x2.pb",
                        _modelFolderName,
                        null);
                    algorithm = "fsrcnn";
                    scale = 2;
                }
                else if (version == SuperresVersion.FsrcnnX3)
                {
                    manager.AddFile(
                        "https://github.com/Saafke/FSRCNN_Tensorflow/raw/master/models/FSRCNN_x3.pb",
                        _modelFolderName,
                        null);
                    algorithm = "fsrcnn";
                    scale = 3;
                }
                else if (version == SuperresVersion.FsrcnnX4)
                {
                    manager.AddFile(
                        "https://github.com/Saafke/FSRCNN_Tensorflow/raw/master/models/FSRCNN_x4.pb",
                        _modelFolderName,
                        null);
                    algorithm = "fsrcnn";
                    scale = 4;
                }
                else if (version == SuperresVersion.LapsrnX2)
                {
                    manager.AddFile(
                        "https://github.com/fannymonori/TF-LapSRN/raw/master/export/LapSRN_x2.pb",
                        _modelFolderName,
                        null);
                    algorithm = "lapsrn";
                    scale = 2;
                }
                else if (version == SuperresVersion.LapsrnX4)
                {
                    manager.AddFile(
                        "https://github.com/fannymonori/TF-LapSRN/raw/master/export/LapSRN_x4.pb",
                        _modelFolderName,
                        null);
                    algorithm = "lapsrn";
                    scale = 4;
                }
                else if (version == SuperresVersion.LapsrnX8)
                {
                    manager.AddFile(
                        "https://github.com/fannymonori/TF-LapSRN/raw/master/export/LapSRN_x8.pb",
                        _modelFolderName,
                        null);
                    algorithm = "lapsrn";
                    scale = 8;
                }

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    try
                    {
                        _superresModel = new DnnSuperResImpl();
                        _superresModel.ReadModel(manager.Files[0].LocalFile);
                        _superresModel.SetModel(algorithm, scale);
                        
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine( String.Format("Failed to initialize model: {0}", ex.Message));
                    }
                }
                else
                {
                    Trace.WriteLine("Failed to download model.");
                }
            }
        }


        /// <summary>
        /// Upsample via neural network.
        /// </summary>
        /// <param name="img">Image to upscale</param>
        /// <param name="result">Destination upscaled image</param>
        public void Upsample(IInputArray img, IOutputArray result)
        {
            if (!Initialized)
            {
                throw new Exception("Please initialize the model first");
            }

            _superresModel.Upsample(img, result);
        }

        /// <summary>
        /// Clear and reset the model. Required Init function to be called again before calling ProcessAndRender.
        /// </summary>
        public void Clear()
        {
            if (_superresModel != null)
            {
                _superresModel.Dispose();
                _superresModel = null;
            }
            
        }

        /// <summary>
        /// Release the memory associated with this Superres model.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }


        /// <summary>
        /// Download and initialize the yolo model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">A string, can be either "EdsrX2", "EdsrX3", "EdsrX4", "EspcnX2", "EspcnX3", "EspcnX4", specify the superres model to use. Deafult to use "EdsrX2". </param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#endif
        {
            SuperresVersion v = SuperresVersion.EdsrX2;
            if (initOptions != null && ((initOptions as String) != null))
            {
                String versionStr = initOptions as String;
                if (versionStr.Equals("EdsrX2"))
                    v = SuperresVersion.EdsrX2;
                else if (versionStr.Equals("EdsrX3"))
                    v = SuperresVersion.EdsrX3;
                else if (versionStr.Equals("EdsrX4"))
                    v = SuperresVersion.EdsrX4;
                else if (versionStr.Equals("EspcnX2"))
                    v = SuperresVersion.EspcnX2;
                else if (versionStr.Equals("EspcnX3"))
                    v = SuperresVersion.EspcnX3;
                else if (versionStr.Equals("EspcnX4"))
                    v = SuperresVersion.EspcnX4;
                else if (versionStr.Equals("FsrcnnX2"))
                    v = SuperresVersion.FsrcnnX2;
                else if (versionStr.Equals("FsrcnnX3"))
                    v = SuperresVersion.FsrcnnX3;
                else if (versionStr.Equals("FsrcnnX4"))
                    v = SuperresVersion.FsrcnnX4;
                else if (versionStr.Equals("LapsrnX2"))
                    v = SuperresVersion.LapsrnX2;
                else if (versionStr.Equals("LapsrnX4"))
                    v = SuperresVersion.LapsrnX4;
                else if (versionStr.Equals("LapsrnX8"))
                    v = SuperresVersion.LapsrnX8;
            }
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
            yield return Init(v, onDownloadProgressChanged);
#else
            await Init(v, onDownloadProgressChanged);
#endif
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">
        /// The output image, can be the same as <paramref name="imageIn"/>, in which case we will render directly into the input image.
        /// Note that if no object is detected, <paramref name="imageOut"/> will remain unchanged.
        /// If objects are detected, we will draw the label and (rectangle) regions on top of the existing pixels of <paramref name="imageOut"/>.
        /// If the <paramref name="imageOut"/> is not the same object as <paramref name="imageIn"/>, it is a good idea to copy the pixels over from the input image before passing it to this function.
        /// </param>
        /// <returns>The messages that we want to display.</returns>
        public String ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            Stopwatch watch = Stopwatch.StartNew();
            
            Upsample(imageIn, imageOut);
            watch.Stop();

            using (InputArray imageInArray = imageIn.GetInputArray())
            using (InputOutputArray imageOutArray = imageOut.GetInputOutputArray())
            {
                Size inSize = imageInArray.GetSize();
                Size outSize = imageOutArray.GetSize();

                return String.Format(
                    "Upsampled from {1}x{2} to {3}x{4} in {0} milliseconds.",
                    watch.ElapsedMilliseconds,
                    inSize.Width,
                    inSize.Height,
                    outSize.Width,
                    outSize.Height);
            }
        }
    }
}
