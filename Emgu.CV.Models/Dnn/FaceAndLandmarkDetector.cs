//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Models
{
    /// <summary>
    /// Face and facial landmark detector
    /// </summary>
    public class FaceAndLandmarkDetector : DisposableObject, IProcessAndRenderModel
    {
        private FaceDetector _faceDetector = null;
        private FacemarkDetector _facemarkDetector = null;

        ///<inheritdoc/>
        public void Clear()
        {
            if (_faceDetector != null)
            {
                _faceDetector.Dispose();
                _faceDetector = null;
            }

            if (_facemarkDetector != null)
            {
                _facemarkDetector.Dispose();
                _facemarkDetector = null;
            }
        }

        /// <summary>
        /// Release the memory associated with this face and facial landmark detector
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        private IEnumerator InitFaceDetector(DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        private async Task InitFaceDetector(DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_faceDetector == null)
            {
                _faceDetector = new FaceDetector();
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return _faceDetector.Init(onDownloadProgressChanged);
#else
                await _faceDetector.Init(onDownloadProgressChanged);
#endif
            }
        }

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        private IEnumerator InitFacemark(DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        private async Task InitFacemark(DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_facemarkDetector == null)
            {
                _facemarkDetector = new FacemarkDetector();
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return _facemarkDetector.Init(onDownloadProgressChanged);
#else
                await _facemarkDetector.Init(onDownloadProgressChanged);
#endif
            }
        }

        /// <summary>
        /// Download and initialize the facemark detector as well as the DNN face detector
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <param name="initOptions">Initialization options. None supported at the moment, any value passed will be ignored.</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator
#else
        public async Task
#endif
            Init(
            DownloadProgressChangedEventHandler onDownloadProgressChanged = null,
            Object initOptions = null)
        {
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
            yield return InitFaceDetector(onDownloadProgressChanged);
            yield return InitFacemark(onDownloadProgressChanged);
#else
            await InitFaceDetector(onDownloadProgressChanged);
            await InitFacemark(onDownloadProgressChanged);
#endif
        }

        /// <summary>
        /// Process the input image and render into the output image
        /// </summary>
        /// <param name="imageIn">The input image</param>
        /// <param name="imageOut">The output image, can be the same as imageIn, in which case we will render directly into the input image</param>
        /// <returns>The messages that we want to display.</returns>
        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            if (imageOut != imageIn)
            {
                using (InputArray iaImageIn = imageIn.GetInputArray())
                {
                    iaImageIn.CopyTo(imageOut);
                }
            }

            Stopwatch watch = Stopwatch.StartNew();

            List<DetectedObject> fullFaceRegions = new List<DetectedObject>();
            List<DetectedObject> partialFaceRegions = new List<DetectedObject>();
            _faceDetector.Detect(imageIn, fullFaceRegions, partialFaceRegions);

            if (partialFaceRegions.Count > 0)
            {
                foreach (DetectedObject face in partialFaceRegions)
                {
                    CvInvoke.Rectangle(imageOut, face.Region, new MCvScalar(0, 255, 0));
                }
            }

            if (fullFaceRegions.Count > 0)
            {
                foreach (DetectedObject face in fullFaceRegions)
                {
                    CvInvoke.Rectangle(imageOut, face.Region, new MCvScalar(0, 255, 0));
                }

                var fullFaceRegionsArr = fullFaceRegions.ToArray();
                var rectRegionArr = Array.ConvertAll(fullFaceRegionsArr, r => r.Region);

                using (VectorOfVectorOfPointF landmarks = _facemarkDetector.Detect(imageIn, rectRegionArr))
                {
                    int len = landmarks.Size;
                    for (int i = 0; i < len; i++)
                    {
                        using (VectorOfPointF vpf = landmarks[i])
                            FaceInvoke.DrawFacemarks(imageOut, vpf, new MCvScalar(255, 0, 0));
                    }
                }
            }
            watch.Stop();
            return String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds);
        }
    }
}
