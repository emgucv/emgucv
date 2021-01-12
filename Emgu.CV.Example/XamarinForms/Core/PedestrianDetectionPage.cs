//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Cuda;
using PedestrianDetection;

namespace Emgu.CV.XamarinForms
{
    public class PedestrianDetectionPage
#if __ANDROID__
        : AndroidCameraPage
#else
        : ButtonTextImagePage
#endif
    {
        private VideoCapture _capture = null;
        private Mat _mat = null;
        private String _defaultButtonText = "Perform Pedestrian Detection";
        private CudaHOG _hogCuda = null;
        private HOGDescriptor _hog;

#if __ANDROID__
        private String _StopCameraButtonText = "Stop Camera";
        private bool _isBusy = false;
#endif
        public PedestrianDetectionPage()
           : base()
        {

#if __ANDROID__
            HasCameraOption = true;
#endif
            var button = this.GetButton();
            button.Text = _defaultButtonText;

            button.Clicked += OnButtonClicked;

            _hog = new HOGDescriptor();
            _hog.SetSVMDetector(HOGDescriptor.GetDefaultPeopleDetector());
            if (CudaInvoke.HasCuda)
            {
                _hogCuda = new CudaHOG(
                    new Size(64, 128),
                    new Size(16, 16),
                    new Size(8, 8),
                    new Size(8, 8));
                _hogCuda.SetSVMDetector(_hogCuda.GetDefaultPeopleDetector());
            }
        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            _capture.Retrieve(_mat);
            Stopwatch watch = Stopwatch.StartNew();
            ProcessImage(_mat);
            watch.Stop();
            SetImage(_mat);
            this.DisplayImage.BackgroundColor = Color.Black;
            this.DisplayImage.IsEnabled = true;
            SetMessage(String.Format("Detected in {0} milliseconds.", watch.ElapsedMilliseconds));
        }

        public void ProcessImage(Mat m)
        {
            Rectangle[] pedestrians;
            if (m.NumberOfChannels == 4)
            {
                //if the png file is loaded with alpha channel
                using (Mat bgr = new Mat())
                {
                    CvInvoke.CvtColor(m, bgr, ColorConversion.Bgra2Bgr);
                    pedestrians = FindPedestrian.Find(bgr, _hog, _hogCuda);
                }
            }
            else
                pedestrians = FindPedestrian.Find(m, _hog, _hogCuda);

            foreach (Rectangle rect in pedestrians)
            {
                CvInvoke.Rectangle(m, rect, new MCvScalar(0, 0, 255), 2);
            }

        }

        private async void OnButtonClicked(object sender, EventArgs e)
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
            Mat[] images = await LoadImages(new String[] { "pedestrian.png" });
            if (images == null)
            {
                //User cancel
                return;
            } else if (images.Length == 0)
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
                            await Task.Run(() => { ProcessImage(m); });
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
            else if (images[0] == null)
            {
                return; //user cancel
            } else
            {
                SetMessage("please wait...");
                SetImage(null);

                Stopwatch watch = Stopwatch.StartNew();
                await Task.Run(() => ProcessImage(images[0]));
                watch.Stop();

                String computeDevice = CvInvoke.UseOpenCL ? "OpenCL: " + Ocl.Device.Default.Name : "CPU";
                SetMessage(String.Format("Detection completed with {1} in {0} milliseconds.", computeDevice, watch.ElapsedMilliseconds));
                SetImage(images[0]);
            }
        }

        
    }
}

