//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !NETFX_CORE

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Preferences;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.XamarinForms
{
    public class FaceLandmarkDetectionPage : ButtonTextImagePage
    {

        private String _modelFolderName = "face_landmark_data";
        private String _path = null;
        private Net _faceDetector = null;
        private FacemarkLBF _facemark = null;


        private void InitPath()
        {
            if (_path == null)
            {
#if __ANDROID__
                _path = System.IO.Path.Combine(
                    Android.OS.Environment.ExternalStorageDirectory.AbsolutePath,
                    Android.OS.Environment.DirectoryDownloads, 
                    _modelFolderName);
#elif __IOS__
               _path = System.IO.Path.Combine (
                 Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments),
                 _modelFolderName);
#else
                _path = String.Format("./{0}/", _modelFolderName);
#endif
         }
        }

        private void InitFaceDetector()
        {
            if (_faceDetector == null)
            {
                InitPath();
                String ssdFileLocal = DnnPage.DnnDownloadFile(
                    "https://github.com/opencv/opencv_3rdparty/raw/dnn_samples_face_detector_20170830/",
                    "res10_300x300_ssd_iter_140000.caffemodel",
                    _path);

                String ssdProtoFileLocal = DnnPage.DnnDownloadFile(
                    "https://raw.githubusercontent.com/opencv/opencv/4.0.1/samples/dnn/face_detector/",
                    "deploy.prototxt",
                    _path);
                _faceDetector = DnnInvoke.ReadNetFromCaffe(ssdProtoFileLocal, ssdFileLocal);
                
            }
        }

        private void InitFacemark()
        {
            if (_facemark == null)
            {
                InitPath();
                String facemarkFileName = "lbfmodel.yaml";
                String facemarkFileUrl = "https://raw.githubusercontent.com/kurnianggoro/GSOC2017/master/data/";
                String facemarkFileLocal = DnnPage.DnnDownloadFile(
                    facemarkFileUrl,
                    facemarkFileName,
                    _path);

                using (FacemarkLBFParams facemarkParam = new CV.Face.FacemarkLBFParams())
                {
                    _facemark = new CV.Face.FacemarkLBF(facemarkParam);
                    _facemark.LoadModel(facemarkFileLocal);
                }
            }
        }

        public FaceLandmarkDetectionPage()
            : base()
        {

            var button = this.GetButton();
            button.Text = "Perform Face Landmark Detection";
            button.Clicked += OnButtonClicked;

            OnImagesLoaded += async (sender, image) =>
            {
                if (image == null || image[0] == null)
                    return;
                SetMessage("Please wait...");
                SetImage(null);
                Task<Tuple<IInputArray, long>> t = new Task<Tuple<IInputArray, long>>(
                    () =>
                    {
                        InitFaceDetector();
                        InitFacemark();

                        int imgDim = 300;
                        MCvScalar meanVal = new MCvScalar(104, 177, 123);
                        Stopwatch watch = Stopwatch.StartNew();
                        Size imageSize = image[0].Size;
                        using (Mat inputBlob = DnnInvoke.BlobFromImage(
                            image[0],
                            1.0,
                            new Size(imgDim, imgDim),
                            meanVal,
                            false,
                            false))
                            _faceDetector.SetInput(inputBlob, "data");
                        using (Mat detection = _faceDetector.Forward("detection_out"))
                        {
                            float confidenceThreshold = 0.5f;

                            List<Rectangle> faceRegions = new List<Rectangle>();

                            float[,,,] values = detection.GetData(true) as float[,,,];
                            for (int i = 0; i < values.GetLength(2); i++)
                            {
                                float confident = values[0, 0, i, 2];

                                if (confident > confidenceThreshold)
                                {
                                    float xLeftBottom = values[0, 0, i, 3] * imageSize.Width;
                                    float yLeftBottom = values[0, 0, i, 4] * imageSize.Height;
                                    float xRightTop = values[0, 0, i, 5] * imageSize.Width;
                                    float yRightTop = values[0, 0, i, 6] * imageSize.Height;
                                    RectangleF objectRegion = new RectangleF(
                                        xLeftBottom,
                                        yLeftBottom,
                                        xRightTop - xLeftBottom,
                                        yRightTop - yLeftBottom);
                                    Rectangle faceRegion = Rectangle.Round(objectRegion);
                                    faceRegions.Add(faceRegion);
                                }
                            }

                            using (VectorOfRect vr = new VectorOfRect(faceRegions.ToArray()))
                            using (VectorOfVectorOfPointF landmarks = new VectorOfVectorOfPointF())
                            {
                                _facemark.Fit(image[0], vr, landmarks);

                                foreach (Rectangle face in faceRegions)
                                {
                                    CvInvoke.Rectangle(image[0], face, new MCvScalar(0, 255, 0));
                                }

                                int len = landmarks.Size;
                                for (int i = 0; i < landmarks.Size; i++)
                                {
                                    using (VectorOfPointF vpf = landmarks[i])
                                        FaceInvoke.DrawFacemarks(image[0], vpf, new MCvScalar(255, 0, 0));
                                }

                            }
                            watch.Stop();
                            return new Tuple<IInputArray, long>(image[0], watch.ElapsedMilliseconds);
                        }
                    });
                t.Start();

                var result = await t;
                SetImage(t.Result.Item1);
                String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";

                SetMessage(String.Format("Detected in {0} milliseconds.", t.Result.Item2));
            };
        }

        private void OnButtonClicked(Object sender, EventArgs args)
        {
            LoadImages(new string[] { "lena.jpg" });
        }


    }
}

#endif