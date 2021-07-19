//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;

using AppKit;
using Foundation;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;
using System.Collections.Generic;

namespace Emgu.CV.Example.Mac
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            messageLabel.StringValue = String.Empty;

            // Do any additional setup after loading the view.
            //HelloWorld();

        }

        void RunHelloWorld()
        {
            Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
            img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

            //Draw "Hello, world." on the image using the specific font
            CvInvoke.PutText(
               img,
               "Hello, world",
               new System.Drawing.Point(10, 80),
               FontFace.HersheyComplex,
               1.0,
               new Bgr(0, 255, 0).MCvScalar);
            mainImageView.Image = img.ToNSImage();
        }

        void RunPlannarSubdivision()
        {
            Mat img = PlanarSubdivisionExample.DrawSubdivision.Draw(400, 20);
            mainImageView.Image = img.ToNSImage();
        }

        async void RunDetectFace()
        {
            try
            {
                //Read the files as an 8-bit Bgr image  
                Mat image = CvInvoke.Imread("lena.jpg", ImreadModes.Color);
                Emgu.CV.Models.FaceAndLandmarkDetector detector = new Models.FaceAndLandmarkDetector();
                await detector.Init(DownloadManager_OnDownloadProgressChanged);
                SetMessage(detector.ProcessAndRender(image, image));
                mainImageView.Image = image.ToNSImage();
            }
            catch (Exception e)
            {
                SetMessage(e.Message);
            }
        }

        public void SetMessage(String msg)
        {
            InvokeOnMainThread(() =>
            {
                messageLabel.StringValue = msg;
                messageLabel.InvalidateIntrinsicContentSize();
            });
        }

        private static String ByteToSizeStr(long byteCount)
        {
            if (byteCount < 1024)
            {
                return String.Format("{0} B", byteCount);
            }
            else if (byteCount < 1024 * 1024)
            {
                return String.Format("{0} KB", byteCount / 1024);
            }
            else
            {
                return String.Format("{0} MB", byteCount / (1024 * 1024));
            }
        }

        protected void DownloadManager_OnDownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            String msg;
            if (e.TotalBytesToReceive > 0)
                msg = String.Format("{0} of {1} downloaded ({2}%)", ByteToSizeStr(e.BytesReceived), ByteToSizeStr(e.TotalBytesToReceive), e.ProgressPercentage);
            else
                msg = String.Format("{0} downloaded", ByteToSizeStr(e.BytesReceived));
            SetMessage(msg);
        }


        void RunFeatureMatching()
        {
            long matchTime;
            using (Mat modelImage = CvInvoke.Imread("box.png", ImreadModes.Grayscale))
            using (Mat observedImage = CvInvoke.Imread("box_in_scene.png", ImreadModes.Grayscale))
            using (Features2D.Feature2D f = new Features2D.KAZE())
            {
                Mat result = FeatureMatchingExample.DrawMatches.Draw(modelImage, observedImage, f, out matchTime);
                //ImageViewer.Show(result, String.Format("Matched in {0} milliseconds", matchTime));
                mainImageView.Image = result.ToNSImage();
            }

        }

        async void RunPedestrianDetection()
        {
            try
            {
                //Read the files as an 8-bit Bgr image  
                using (Mat image = new Mat("pedestrian.png"))
                {
                    Emgu.CV.Models.PedestrianDetector detector = new Models.PedestrianDetector();
                    await detector.Init(DownloadManager_OnDownloadProgressChanged);

                    SetMessage(detector.ProcessAndRender(image, image));
                    mainImageView.Image = image.ToNSImage();
                }
            }
            catch (Exception e)
            {
                SetMessage(e.Message);
            }
        }

        private VideoCapture _capture = null;
        private Mat _frame = null;
        private bool _captureInProgress = false;

        private void ProcessFrame(object sender, EventArgs arg)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero && _capture.IsOpened)
            {
                _capture.Retrieve(_frame, 0);
                var nsImage = _frame.ToNSImage();
                InvokeOnMainThread(() =>
                {
                    mainImageView.Image = nsImage;
                });
            }
        }

        void RunCameraCapture()
        {
            if (_capture == null)
            {
                _capture = new VideoCapture();
                if (_capture == null || !_capture.IsOpened)
                {
                    _capture = null;
                    InvokeOnMainThread(() =>
                    {
                        messageLabel.StringValue = "unable to create capture";
                    });
                    return;
                }
                _capture.ImageGrabbed += ProcessFrame;
                _frame = new Mat();

            }
            if (_captureInProgress)
            {  //stop the capture
               //captureButton.Text = "Start Capture";
                _capture.Pause();
            }
            else
            {
                //start the capture
                //captureButton.Text = "Stop";
                _capture.Start();
            }

            _captureInProgress = !_captureInProgress;

        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void helloWorldClicked(NSObject sender)
        {
            RunHelloWorld();
        }

        partial void plannarSubdivisionClicked(NSObject sender)
        {
            RunPlannarSubdivision();
        }

        partial void faceDetectionClicked(NSObject sender)
        {
            RunDetectFace();
        }

        partial void featureMatchingClicked(Foundation.NSObject sender)
        {
            RunFeatureMatching();
        }

        partial void pedestrianDetectionClicked(Foundation.NSObject sender)
        {
            RunPedestrianDetection();
        }

        partial void CameraCaptureClicked(NSObject sender)
        {
            RunCameraCapture();
        }
    }
}