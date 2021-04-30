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
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Models
{

    /// <summary>
    /// Face detector using DNN
    /// </summary>
    public class FaceDetector : DisposableObject
    {
        private String _modelFolderName = "dnn_samples_face_detector_20170830";
        
        private DetectionModel _faceDetectionModel = null;

        /// <summary>
        /// Download and initialize the DNN face detector
        /// </summary>
        /// <param name="onDownloadProgressChanged">Callback when download progress has been changed</param>
        /// <returns>Async task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#else
        public async Task Init(System.Net.DownloadProgressChangedEventHandler onDownloadProgressChanged = null)
#endif
        {
            if (_faceDetectionModel == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/opencv/opencv_3rdparty/raw/dnn_samples_face_detector_20170830/res10_300x300_ssd_iter_140000.caffemodel",
                    _modelFolderName,
                    "2A56A11A57A4A295956B0660B4A3D76BBDCA2206C4961CEA8EFE7D95C7CB2F2D");

                manager.AddFile(
                    "https://raw.githubusercontent.com/opencv/opencv/4.0.1/samples/dnn/face_detector/deploy.prototxt",
                    _modelFolderName,
                    "F62621CAC923D6F37BD669298C428BB7EE72233B5F8C3389BB893E35EBBCF795");

                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _faceDetectionModel = new DetectionModel(manager.Files[0].LocalFile, manager.Files[1].LocalFile);
                    _faceDetectionModel.SetInputMean(new MCvScalar(104, 177, 123));
                    _faceDetectionModel.SetInputSize(new Size(300, 300));
                    _faceDetectionModel.SetInputSwapRB(false);
                    _faceDetectionModel.SetInputScale(1.0);
                    _faceDetectionModel.SetInputCrop(false);

#if !(UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE  || UNITY_WEBGL)
                    if (Emgu.CV.Cuda.CudaInvoke.HasCuda)
                    {
                        _faceDetectionModel.SetPreferableBackend(Emgu.CV.Dnn.Backend.Cuda);
                        _faceDetectionModel.SetPreferableTarget(Emgu.CV.Dnn.Target.Cuda);
                    }
#endif
                }
            }
        }

        /// <summary>
        /// Detect faces on the image
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="fullFaceRegions">The faces where a full facial region is detected. These images can be send to facial landmark recognition for further processing.</param>
        /// <param name="partialFaceRegions">The face region of which is close to the edge of the images. Because if may not contains all the facial landmarks, it is not recommended to send these regions to facial landmark detection.</param>
        /// <param name="confidenceThreshold">The confident threshold for face detection</param>
        /// <param name="nmsThreshold">The non maximum suppression threshold for face detection.</param>
        public void Detect(IInputArray image, List<DetectedObject> fullFaceRegions, List<DetectedObject> partialFaceRegions, float confidenceThreshold = 0.5f, float nmsThreshold = 0.0f)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
                DetectedObject[] detectedFaces = _faceDetectionModel.Detect(image, confidenceThreshold, nmsThreshold);
                Rectangle imageRegion = new Rectangle(Point.Empty, iaImage.GetSize());
                foreach (DetectedObject face in detectedFaces)
                {
                    if (imageRegion.Contains(face.Region))
                        fullFaceRegions.Add(face);
                    else
                    {
                        partialFaceRegions.Add(face);
                    }
                }
            }
        }


        /// <summary>
        /// Release the memory associated with this face detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_faceDetectionModel != null)
            {
                _faceDetectionModel.Dispose();
                _faceDetectionModel = null;
            }
        }
    }
}
