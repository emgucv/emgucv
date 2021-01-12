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
using Emgu.Models;
using Emgu.Util;

using Color = Xamarin.Forms.Color;
using Environment = System.Environment;
using Point = System.Drawing.Point;

namespace Emgu.CV.XamarinForms
{


    public class YoloPage
#if __ANDROID__
        : AndroidCameraPage
#else
        : ButtonTextImagePage
#endif
    {
        private Yolo _yolo = null;

        private VideoCapture _capture = null;
        private Mat _mat = null;
        private String _defaultButtonText = "Yolo Detection";

#if __ANDROID__
        private String _StopCameraButtonText = "Stop Camera";
        private bool _isBusy = false;
#endif
        public YoloPage()
            : base()
        {
#if __ANDROID__
            HasCameraOption = true;
#endif
            _yolo = new Yolo();
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
            var detectedObjects = _yolo.Detect(_mat);
            _yolo.Render(_mat, detectedObjects);
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
            Mat[] images = await LoadImages(new string[] { "dog416.png" });

            if (images == null || (images.Length > 0 && images[0] == null))
                return;

            SetMessage("Please wait...");
            SetImage(null);
            await _yolo.Init(Yolo.YoloVersion.YoloV3Tiny, DownloadManager_OnDownloadProgressChanged);
            if (images.Length == 0)
            {

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
                            await Task.Run(() => 
                            {
                                var detectedObjects = _yolo.Detect(m);
                                _yolo.Render(m, detectedObjects);          
                            });
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

                Stopwatch watch = Stopwatch.StartNew();

                var detectedObjects = _yolo.Detect(images[0]);
                _yolo.Render(images[0], detectedObjects);
                
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
