//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Xamarin.Forms;
using Emgu.CV.Dnn;
using Emgu.CV.Models;
using Emgu.CV.Util;
//using Emgu.Models;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace Emgu.CV.XamarinForms
{

    public class MaskRcnnPage
#if __ANDROID__
        : AndroidCameraPage
#else
        : ButtonTextImagePage
#endif
    {
        private String[] _objectsOfInterest = null;
        private String _defaultImage = "dog416.png";

        private MaskRcnn _maskRcnnDetector = null;

        
        public String[] ObjectsOfInterest
        {
            get { return _objectsOfInterest; }
            set { _objectsOfInterest = value; }
        }

        public String DefaultImage
        {
            get { return _defaultImage; }
            set { _defaultImage = value; }
        }


        /// <summary>
        /// Initiate the DNN model. If needed, it will download the model from internet.
        /// </summary>
        private async Task InitDetector()
        {
            if (_maskRcnnDetector == null)
            {
                
                _maskRcnnDetector = new MaskRcnn();
                _maskRcnnDetector.ObjectsOfInterest = _objectsOfInterest;
                await _maskRcnnDetector.Init(DownloadManager_OnDownloadProgressChanged);
            }
        }

        /*
        public async Task ProcessImageAsync(Mat m)
        {
            await Task.Run(() => { ProcessImage(m); });
        }*/

        private VideoCapture _capture = null;
        private Mat _mat = null;
        private String _defaultButtonText = "Perform Mask-rcnn Detection";

#if __ANDROID__
        private String _StopCameraButtonText = "Stop Camera";
        private bool _isBusy = false;
#endif

        public MaskRcnnPage()
         : base()
        {
#if __ANDROID__
            HasCameraOption = true;
#endif

            var button = this.GetButton();
            button.Text = _defaultButtonText;

            button.Clicked += OnButtonClicked;

            BackendTargetPair[] availableBackends = Emgu.CV.Dnn.DnnInvoke.AvailableBackends;

            StringBuilder availableBackendsStr = new StringBuilder("Available backends: " + System.Environment.NewLine);
            foreach (BackendTargetPair p in availableBackends)
            {
                availableBackendsStr.Append(
                    String.Format(
                        "Backend: {0}, Target: {1}{2}",
                        p.Backend, p.Target,
                        System.Environment.NewLine));
            }
            SetMessage(availableBackendsStr.ToString());

        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            _capture.Retrieve(_mat);
            Stopwatch watch = Stopwatch.StartNew();
            _maskRcnnDetector.DetectAndRender(_mat);
            
            watch.Stop();
            SetImage(_mat);
            //this.DisplayImage.BackgroundColor = Color.Black;
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
            Mat[] images = await LoadImages(new string[] { _defaultImage });

            if (images == null || (images.Length > 0 && images[0] == null))
                return;

            if (images.Length == 0)
            {
                //Use Camera
                await InitDetector();

#if __ANDROID__
                button.Text = _StopCameraButtonText;
                //AndroidImageView.Visibility = ViewStates.Visible;
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
                            await Task.Run(() => { _maskRcnnDetector.DetectAndRender(m); });
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

                await InitDetector();
                Stopwatch watch = Stopwatch.StartNew();
                await Task.Run(() =>
                {
                    _maskRcnnDetector.DetectAndRender(images[0]);
                });
                watch.Stop();
                String msg = String.Format("Mask RCNN inception completed in {0} milliseconds",
                    watch.ElapsedMilliseconds);
                SetImage(images[0]);
                SetMessage(msg);
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
