//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Models;
using Emgu.CV.Structure;
using Emgu.CV.Util;
//using Emgu.Models;
using Emgu.Util;
using Color = Xamarin.Forms.Color;
using Environment = System.Environment;
using Point = System.Drawing.Point;

namespace Emgu.CV.XamarinForms
{


    public class FaceLandmarkDetectionPage
#if __ANDROID__
        : AndroidCameraPage
#else
        : ButtonTextImagePage
#endif
    {

        private FaceDetector _faceDetector = null;
        private FacemarkDetector _facemarkDetector = null;

  
        private async Task InitFaceDetector()
        {
            if (_faceDetector == null)
            {
                _faceDetector = new FaceDetector();
                await _faceDetector.Init(DownloadManager_OnDownloadProgressChanged);
            }
        }

        private async Task InitFacemark()
        {
            if (_facemarkDetector == null)
            {
                _facemarkDetector = new FacemarkDetector();
                await _facemarkDetector.Init(DownloadManager_OnDownloadProgressChanged);
            }
        }

        private void DetectAndRender(Mat image)
        {
            List<Rectangle> fullFaceRegions = new List<Rectangle>();
            List<Rectangle> partialFaceRegions = new List<Rectangle>();
            _faceDetector.Detect(image, fullFaceRegions, partialFaceRegions);

            if (partialFaceRegions.Count > 0)
            {
                foreach (Rectangle face in partialFaceRegions)
                {
                    CvInvoke.Rectangle(image, face, new MCvScalar(0, 255, 0));
                }
            }

            if (fullFaceRegions.Count > 0)
            {
                foreach (Rectangle face in fullFaceRegions)
                {
                    CvInvoke.Rectangle(image, face, new MCvScalar(0, 255, 0));
                }

                using (VectorOfVectorOfPointF landmarks = _facemarkDetector.Detect(image, fullFaceRegions.ToArray()))
                {
                    int len = landmarks.Size;
                    for (int i = 0; i < len; i++)
                    {
                        using (VectorOfPointF vpf = landmarks[i])
                            FaceInvoke.DrawFacemarks(image, vpf, new MCvScalar(255, 0, 0));
                    }
                }
            }
        }

        private VideoCapture _capture = null;
        private Mat _mat = null;
        private String _defaultButtonText = "Perform Face Landmark Detection";

#if __ANDROID__
        private String _StopCameraButtonText = "Stop Camera";
        private bool _isBusy = false;
#endif
        public FaceLandmarkDetectionPage()
            : base()
        {
#if __ANDROID__
            HasCameraOption = true;
#endif

            var button = this.GetButton();
            button.Text = _defaultButtonText;
            button.Clicked += OnButtonClicked;
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

        private async void OnButtonClicked(Object sender, EventArgs args)
        {
#if __ANDROID__
            var button = GetButton();
            if (button.Text.Equals(_StopCameraButtonText))
            {
                StopCapture();
                button.Text = _defaultButtonText;
                //AndroidImageView.Visibility = ViewStates.Invisible;
                return;
            }
#endif
            Mat[] images = await LoadImages(new string[] { "lena.jpg" });

            if (images == null || (images.Length > 0 && images[0] == null))
                return;

            if (images.Length == 0)
            {

                await InitFaceDetector();
                await InitFacemark();

#if __ANDROID__
                button.Text = _StopCameraButtonText;
                StartCapture(async delegate (Object captureSender, Mat m)
                {
                    //Skip the frame if busy, 
                    //Otherwise too many frames arriving and will eventually saturated the memory.
                    if (!_isBusy)
                    {
                        _isBusy = true;
                        try
                        {
                            Stopwatch watch = Stopwatch.StartNew();
                            await Task.Run(() => { DetectAndRender(m); });
                            watch.Stop();
                            SetImage(m);
                            SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
                        }
                        finally
                        {
                            _isBusy = false;
                        }
                    }
                });
#else
                //Handle video
                if (_capture == null)
                {
                    _capture = new VideoCapture();
                    _capture.ImageGrabbed += _capture_ImageGrabbed;
                }
                _capture.Start();
#endif
            }
            else
            {
                SetMessage("Please wait...");
                SetImage(null);

                await InitFaceDetector();
                await InitFacemark();
                Stopwatch watch = Stopwatch.StartNew();

                DetectAndRender(images[0]);
                watch.Stop();

                SetImage(images[0]);
                String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";

                SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
            }
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
