//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
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


namespace Emgu.CV.Models
{

    /// <summary>
    /// Face detector using FaceDetectorYN
    /// </summary>
    public class FaceDetectorYNModel : DisposableObject, IProcessAndRenderModel
    {
        private String _modelFolderName = "face_detector_yn";

        private FaceDetectorYN _faceDetectionModel = null;

        private String _downloadedFile = null;

        /// <summary>
        /// Download and initialize the face detection model
        /// </summary>
        /// <param name="onDownloadProgressChanged">Call back method during download</param>
        /// <param name="initOptions">Initialization options. None supported at the moment, any value passed will be ignored.</param>
        /// <returns>Asyn task</returns>
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
        public IEnumerator Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#else
        public async Task Init(FileDownloadManager.DownloadProgressChangedEventHandler onDownloadProgressChanged = null, Object initOptions = null)
#endif
        {
            if (_faceDetectionModel == null)
            {
                FileDownloadManager manager = new FileDownloadManager();

                manager.AddFile(
                    "https://github.com/opencv/opencv_zoo/raw/master/models/face_detection_yunet/face_detection_yunet_2022mar.onnx",
                    _modelFolderName,
                    "50EF07F702A31741CA46A4C0D947773B64143B9362780237BF0D427D6C79BAB7");


                if (onDownloadProgressChanged != null)
                    manager.OnDownloadProgressChanged += onDownloadProgressChanged;
#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL
                yield return manager.Download();
#else
                await manager.Download();
#endif

                if (manager.AllFilesDownloaded)
                {
                    _downloadedFile = manager.Files[0].LocalFile;
                }
            }
        }

        /// <summary>
        /// Detect faces on the image
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="detectionResult">The detection result</param>
        public void Detect(IInputArray image, Mat detectionResult)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
                if (_faceDetectionModel != null && !_faceDetectionModel.InputSize.Equals(iaImage.GetSize()))
                {
                    // if the face detector is created with different image size, disposed it
                    // (and we will re-create one with the right image size)
                    Clear();
                }

                if (_faceDetectionModel == null)
                {
                    _faceDetectionModel = new FaceDetectorYN(
                        _downloadedFile,
                        "",
                        iaImage.GetSize(),
                        0.6f,
                        0.3f,
                        5000,
                        Dnn.Backend.Default,
                        Dnn.Target.Cpu);
                }

                _faceDetectionModel.Detect(image, detectionResult);
                
            }
        }


        /// <summary>
        /// Detect faces on the image
        /// </summary>
        /// <param name="image">The image</param>
        /// <returns>The array of FaceDetectorYNResult</returns>
        public FaceDetectorYNResult[] Detect(IInputArray image)
        {
            using (Mat detectionResult = new Mat())
            {
                Detect(image, detectionResult);
                return ConvertMatToFaceDetectorYNResult(detectionResult);
            }
        }

        /// <summary>
        /// Convert the raw result in Mat format into more readable result
        /// </summary>
        /// <param name="rawResult">The raw result in Mat format</param>
        /// <returns>The array of FaceDetectorYNResult</returns>
        public static FaceDetectorYNResult[] ConvertMatToFaceDetectorYNResult(Mat rawResult)
        {
            List<FaceDetectorYNResult> faces = new List<FaceDetectorYNResult>();

            float[,] results = rawResult.GetData() as float[,];
            if (results == null)
                return new FaceDetectorYNResult[0];

            int count = results.GetLength(0);
            for (int i = 0; i < count; i++)
            {
                FaceDetectorYNResult face = new FaceDetectorYNResult(results, i);
                faces.Add(face);
            }

            return faces.ToArray();
        }

        /// <summary>
        /// Results from FaceDetectorYN
        /// </summary>
        public struct FaceDetectorYNResult
        {
            internal FaceDetectorYNResult(float[,] data, int row)
            {
                Region = new RectangleF(data[row, 0], data[row, 1], data[row, 2], data[row, 3]);
                RightEye = new PointF(data[row, 4], data[row, 5]);
                LeftEye = new PointF(data[row, 6], data[row, 7]);
                NoseTip = new PointF(data[row, 8], data[row, 9]);
                RightMouthCorner = new PointF(data[row, 10], data[row, 11]);
                LeftMouthCorner = new PointF(data[row, 12], data[row, 13]);
            }

            /// <summary>
            /// The facial region
            /// </summary>
            public RectangleF Region;

            /// <summary>
            /// The right eye center            
            /// </summary>
            public PointF RightEye;

            /// <summary>
            /// The left eye center             
            /// </summary>
            public PointF LeftEye;

            /// <summary>
            /// The nose tip
            /// </summary>
            public PointF NoseTip;

            /// <summary>
            /// The right mouth corner
            /// </summary>
            public PointF RightMouthCorner;

            /// <summary>
            /// The left mouth corner
            /// </summary>
            public PointF LeftMouthCorner;

        }

        /// <summary>
        /// Release the memory associated with this face detector.
        /// </summary>
        protected override void DisposeObject()
        {
            Clear();
        }

        /// <summary>
        /// Clear the model. 
        /// </summary>
        public void Clear()
        {
            if (_faceDetectionModel != null)
            {
                _faceDetectionModel.Dispose();
                _faceDetectionModel = null;
            }
        }

        /// <inheritdoc />
        public string ProcessAndRender(IInputArray imageIn, IInputOutputArray imageOut)
        {
            FaceDetectorYNResult[] faces = Detect(imageIn);

            using (InputArray iaImageIn = imageIn.GetInputArray())
            {
                iaImageIn.CopyTo(imageOut);
                foreach (FaceDetectorYNResult face in faces)
                {
                    Rectangle r = Rectangle.Round(face.Region);
                    CvInvoke.Rectangle(imageOut, r, new MCvScalar(255, 255, 255, 255), 2);
                    CvInvoke.Circle(imageOut, Point.Round(face.LeftEye), 3, new MCvScalar(255, 0, 0), -1);
                    CvInvoke.Circle(imageOut, Point.Round(face.RightEye), 3, new MCvScalar(255, 0, 0), -1);
                    CvInvoke.Circle(imageOut, Point.Round(face.NoseTip), 3, new MCvScalar(0, 0, 255), -1);
                    CvInvoke.Circle(imageOut, Point.Round(face.LeftMouthCorner), 3, new MCvScalar(0, 255, 0), -1);
                    CvInvoke.Circle(imageOut, Point.Round(face.RightMouthCorner), 3, new MCvScalar(0, 255, 0), -1);
                }
            }

            return String.Empty;
        }
    }
}
