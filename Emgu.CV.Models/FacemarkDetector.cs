//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Facial landmark detector
    /// </summary>
    public class FacemarkDetector : DisposableObject
    {
        private FacemarkLBF _facemark = null;

        /// <summary>
        /// Download and initialize the facial landmark detector
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_facemark == null)
            {
                FileDownloadManager manager = new FileDownloadManager();
                manager.AddFile(
                    "https://raw.githubusercontent.com/kurnianggoro/GSOC2017/master/data/lbfmodel.yaml", 
                    "facemark",
                    "70DD8B1657C42D1595D6BD13D97D932877B3BED54A95D3C4733A0F740D1FD66B");
                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif
                if (manager.AllFilesDownloaded)
                {
                    using (FacemarkLBFParams facemarkParam = new CV.Face.FacemarkLBFParams())
                    {
                        _facemark = new FacemarkLBF(facemarkParam);
                        _facemark.LoadModel(manager.Files[0].LocalFile);
                    }
                } 
            }
        }

        /// <summary>
        /// Detect the facial landmarks from the face regions
        /// </summary>
        /// <param name="image">The image to detect facial landmarks from</param>
        /// <param name="fullFaceRegions">The face regions to detect landmarks from</param>
        /// <returns>Vector of facial landmarks</returns>
        public VectorOfVectorOfPointF Detect(IInputArray image, Rectangle[] fullFaceRegions)
        {
            using (VectorOfRect vr = new VectorOfRect(fullFaceRegions))
            {
                VectorOfVectorOfPointF landmarks = new VectorOfVectorOfPointF();
                _facemark.Fit(image, vr, landmarks);
                return landmarks;
            }
        }

        /// <summary>
        /// Release the memory associated with this facemark detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_facemark != null)
            {
                _facemark.Dispose();
                _facemark = null;
            }
        }
    }
}
