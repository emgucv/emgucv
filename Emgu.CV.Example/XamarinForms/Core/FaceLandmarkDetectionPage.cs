//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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
using Emgu.Models;
using Emgu.Util;
using Color = Xamarin.Forms.Color;
using Environment = System.Environment;

namespace Emgu.CV.XamarinForms
{
    public class FaceLandmarkDetectionPage 
        : ButtonTextImagePage
    {
        private String _modelFolderName = "dnn_samples_face_detector_20170830";
        private Net _faceDetector = null;
        private FacemarkLBF _facemark = null;

        private async Task InitFaceDetector()
        {
            if (_faceDetector == null)
            {
                FileDownloadManager manager = new FileDownloadManager();
                manager.AddFile(
                    "https://github.com/opencv/opencv_3rdparty/raw/dnn_samples_face_detector_20170830/res10_300x300_ssd_iter_140000.caffemodel",
                    _modelFolderName);
                manager.AddFile("https://raw.githubusercontent.com/opencv/opencv/4.0.1/samples/dnn/face_detector/deploy.prototxt",
                    _modelFolderName);
                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
                await manager.Download();
                _faceDetector = DnnInvoke.ReadNetFromCaffe(manager.Files[1].LocalFile, manager.Files[0].LocalFile);
            }
        }

        private async Task InitFacemark()
        {
            if (_facemark == null)
            {
                FileDownloadManager manager = new FileDownloadManager();
                manager.AddFile("https://raw.githubusercontent.com/kurnianggoro/GSOC2017/master/data/lbfmodel.yaml", "facemark");
                manager.OnDownloadProgressChanged += DownloadManager_OnDownloadProgressChanged;
                await manager.Download();
                using (FacemarkLBFParams facemarkParam = new CV.Face.FacemarkLBFParams())
                {
                    _facemark = new CV.Face.FacemarkLBF(facemarkParam);
                    _facemark.LoadModel(manager.Files[0].LocalFile);
                }
            }
        }

        private void DetectAndRender(Mat image)
        {
            int imgDim = 300;
            MCvScalar meanVal = new MCvScalar(104, 177, 123);

            Size imageSize = image.Size;
            using (Mat inputBlob = DnnInvoke.BlobFromImage(
                image,
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
                    _facemark.Fit(image, vr, landmarks);

                    foreach (Rectangle face in faceRegions)
                    {
                        CvInvoke.Rectangle(image, face, new MCvScalar(0, 255, 0));
                    }

                    int len = landmarks.Size;
                    for (int i = 0; i < landmarks.Size; i++)
                    {
                        using (VectorOfPointF vpf = landmarks[i])
                            FaceInvoke.DrawFacemarks(image, vpf, new MCvScalar(255, 0, 0));
                    }
                }
            }
        }

        private VideoCapture _capture = null;
        private Mat _mat = null;

        public FaceLandmarkDetectionPage()
            : base()
        {
            //HasCameraOption = true;

            var button = this.GetButton();
            button.Text = "Perform Face Landmark Detection";
            button.Clicked += OnButtonClicked;

            OnImagesLoaded += async (sender, image) =>
            {
                if (image == null || (image.Length > 0 && image[0] == null))
                    return;

                if (image.Length == 0)
                {
                    await InitFaceDetector();
                    await InitFacemark();
                    //Handle video
                    if (_capture == null)
                    {
                        _capture = new VideoCapture();
                        _capture.ImageGrabbed += _capture_ImageGrabbed;
                    }
                    _capture.Start();
                }
                else
                {
                    SetMessage("Please wait...");
                    SetImage(null);
                    
                    await InitFaceDetector();
                    await InitFacemark();
                    Stopwatch watch = Stopwatch.StartNew();

                    DetectAndRender(image[0]);
                    watch.Stop();
                            
                    SetImage(image[0]);
                    String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";

                    SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
                }
            };
        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            _capture.Retrieve(_mat);
            Stopwatch watch = Stopwatch.StartNew();
            DetectAndRender(_mat);
            watch.Stop();
            SetImage(_mat);
            this.DisplayImage.BackgroundColor = Color.Black;
            this.DisplayImage.IsEnabled = true;
            SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
        }

        private void OnButtonClicked(Object sender, EventArgs args)
        {
            LoadImages(new string[] { "lena.jpg" });
        }

        private void DownloadManager_OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive <= 0)
                SetMessage(String.Format("{0} bytes downloaded.", e.BytesReceived));
            else
                SetMessage(String.Format("{0} of {1} bytes downloaded ({2}%)", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage));
        }
    }
}
