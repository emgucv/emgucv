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
using Emgu.CV.Structure;
using Emgu.Util;


namespace Emgu.CV.Models
{
    /// <summary>
    /// Face recognizer using the SFace model from the OpenCV Zoo.
    /// Wraps <see cref="FaceRecognizerSF"/> and handles model downloading.
    /// Typical use: detect faces with <see cref="FaceDetectorYNModel"/>, then call
    /// <see cref="Match(IInputArray, IInputArray, IInputArray, IInputArray, FaceRecognizerSF.DisType)"/>
    /// to compare two faces.
    /// </summary>
    public class FaceRecognizerSFModel : DisposableObject
    {
        private readonly string _modelFolderName = Path.Combine("emgu", "face_recognizer_sface");

        private FaceRecognizerSF _faceRecognizerSF = null;

        /// <summary>
        /// Return true if the model is initialized.
        /// </summary>
        public bool Initialized
        {
            get { return _faceRecognizerSF != null; }
        }

        /// <summary>
        /// Download and initialize the SFace recognition model.
        /// </summary>
        /// <param name="onDownloadProgressChanged">Optional callback invoked during download.</param>
        /// <param name="initOptions">Not used; any value is ignored.</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, object initOptions = null)
#endif
        {
            if (_faceRecognizerSF == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://media.githubusercontent.com/media/opencv/opencv_zoo/main/models/face_recognition_sface/face_recognition_sface_2021dec.onnx",
                    _modelFolderName,
                    "0BA9FBFA01B5270C96627C4EF784DA859931E02F04419C829E83484087C34E79");

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;

#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _faceRecognizerSF = new FaceRecognizerSF(
                        manager.Files[0].LocalFile,
                        "",
                        Dnn.Backend.Default,
                        Dnn.Target.Cpu);
                }
            }
        }

        /// <summary>
        /// Extract the face feature vector from a single detected face.
        /// </summary>
        /// <param name="image">The original full image.</param>
        /// <param name="faceDetectionRow">
        /// A single-row Mat from a <see cref="FaceDetectorYN"/> detection result
        /// (e.g. <c>detectionMat.Row(i)</c>).
        /// </param>
        /// <param name="faceFeature">Output feature vector (128-dim float32).</param>
        public void Feature(IInputArray image, IInputArray faceDetectionRow, IOutputArray faceFeature)
        {
            using (Mat alignedFace = new Mat())
            {
                _faceRecognizerSF.AlignCrop(image, faceDetectionRow, alignedFace);
                _faceRecognizerSF.Feature(alignedFace, faceFeature);
            }
        }

        /// <summary>
        /// Compare two faces detected in (possibly different) images and return their distance.
        /// </summary>
        /// <param name="image1">First full image.</param>
        /// <param name="faceDetectionRow1">Single-row detection result for the face in <paramref name="image1"/>.</param>
        /// <param name="image2">Second full image.</param>
        /// <param name="faceDetectionRow2">Single-row detection result for the face in <paramref name="image2"/>.</param>
        /// <param name="disType">Distance metric to use.</param>
        /// <returns>
        /// For <see cref="FaceRecognizerSF.DisType.Cosine"/>: higher score means more similar
        /// (same person if ≥ 0.363).
        /// For <see cref="FaceRecognizerSF.DisType.NormL2"/>: lower score means more similar
        /// (same person if ≤ 1.128).
        /// </returns>
        public double Match(
            IInputArray image1,
            IInputArray faceDetectionRow1,
            IInputArray image2,
            IInputArray faceDetectionRow2,
            FaceRecognizerSF.DisType disType = FaceRecognizerSF.DisType.Cosine)
        {
            using (Mat feature1 = new Mat())
            using (Mat feature2 = new Mat())
            {
                Feature(image1, faceDetectionRow1, feature1);
                Feature(image2, faceDetectionRow2, feature2);
                return _faceRecognizerSF.Match(feature1, feature2, disType);
            }
        }

        /// <summary>
        /// Compare two pre-computed face feature vectors.
        /// </summary>
        /// <param name="faceFeature1">Feature vector from the first face.</param>
        /// <param name="faceFeature2">Feature vector from the second face.</param>
        /// <param name="disType">Distance metric to use.</param>
        /// <returns>The distance between the two feature vectors.</returns>
        public double MatchFeatures(
            IInputArray faceFeature1,
            IInputArray faceFeature2,
            FaceRecognizerSF.DisType disType = FaceRecognizerSF.DisType.Cosine)
        {
            return _faceRecognizerSF.Match(faceFeature1, faceFeature2, disType);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_faceRecognizerSF != null)
            {
                _faceRecognizerSF.Dispose();
                _faceRecognizerSF = null;
            }
        }
    }
}
