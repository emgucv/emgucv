//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;


#if __ANDROID__ && __USE_ANDROID_CAMERA2__
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
using Emgu.Util;
using Xamarin.Forms;
using Color = Xamarin.Forms.Color;
using Environment = System.Environment;
using Point = System.Drawing.Point;

namespace Emgu.CV.XamarinForms
{

    public class ProcessAndRenderPage
#if __ANDROID__ && __USE_ANDROID_CAMERA2__
        : AndroidCameraPage
#else
        : ButtonTextImagePage
#endif
    {
        private VideoCapture _capture = null;
        private Mat _mat;
        private Mat _renderMat;
        private String _defaultButtonText;
        private IProcessAndRenderModel _model;

        private String _StopCameraButtonText = "Stop Camera";
        private String _deaultImage;

#if __ANDROID__ && __USE_ANDROID_CAMERA2__
        private bool _isBusy = false;
#endif
        public ProcessAndRenderPage( 
            IProcessAndRenderModel model, 
            String defaultButtonText,
            String defaultImage,
            String defaultLabelText = null
            )
            : base()
        {
            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveVideoio = (openCVConfigDict["HAVE_OPENCV_VIDEOIO"] != 0);
            if (haveVideoio && (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Android))
            {
                HasCameraOption = true;
            }
            
            /*
            if (haveVideoio)
            {
                if (CvInvoke.Backends.Length > 0)
                {
                    _capture = new VideoCapture(0, VideoCapture.API.Android);
                    if (_capture.IsOpened)
                    {
                        _capture.ImageGrabbed += _capture_ImageGrabbed;
                        HasCameraOption = true;
                    }
                    else
                    {
                        _capture.Dispose();
                        _capture = null;
                        HasCameraOption = false;
                    }
                }
            }*/

            _deaultImage = defaultImage;
            _defaultButtonText = defaultButtonText;

            var button = this.GetButton();
            button.Text = _defaultButtonText;
            button.Clicked += OnButtonClicked;

            var label = this.GetLabel();
            label.Text = defaultLabelText;

            _model = model;

            Picker.SelectedIndexChanged += Picker_SelectedIndexChanged;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _model.Clear();
        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            _capture.Retrieve(_mat);
            
            if (_renderMat == null)
                _renderMat = new Mat();

            String msg = _model.ProcessAndRender(_mat, _renderMat);
            
            SetImage(_renderMat);
            this.DisplayImage.BackgroundColor = Color.Black;
            this.DisplayImage.IsEnabled = true;
            SetMessage(msg);
        }

        private async void OnButtonClicked(Object sender, EventArgs args)
        {
            var button = GetButton();

            if (button.Text.Equals(_StopCameraButtonText))
            {
#if __ANDROID__ && __USE_ANDROID_CAMERA2__
                StopCapture();
                AndroidImageView.Visibility = ViewStates.Invisible;
#else
                _capture.Stop();
                _capture.Dispose();
                _capture = null;
#endif
                button.Text = _defaultButtonText;
                Picker.IsEnabled = true;
                return;
            }
            else
            {
                Picker.IsEnabled = false;
            }

            Mat[] images = await LoadImages(new string[] { _deaultImage });

            if (images == null || (images.Length > 0 && images[0] == null))
                return;

            SetMessage("Please wait...");
            SetImage(null);

            Picker p = this.Picker;
            if ((!p.IsVisible) || p.SelectedIndex < 0)
                await _model.Init(DownloadManager_OnDownloadProgressChanged, null);
            else
            {
                await _model.Init(DownloadManager_OnDownloadProgressChanged, p.Items[p.SelectedIndex].ToString());
            }

            if (images.Length == 0)
            {
#if __ANDROID__ && __USE_ANDROID_CAMERA2__
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
                            String message = String.Empty;
                            await Task.Run(() => 
                            {
                                if (_renderMat == null)
                                    _renderMat = new Mat();
                                message = _model.ProcessAndRender(m, _renderMat);
                            });
                            SetImage(_renderMat);
                            SetMessage(message);

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
                    if (Emgu.Util.Platform.OperationSystem == Emgu.Util.Platform.OS.Android)
                    {
                        _capture = new VideoCapture(0, VideoCapture.API.Android);
                    }
                    else
                    {
                        _capture = new VideoCapture();
                    }

                    _capture.ImageGrabbed += _capture_ImageGrabbed;
                }

                _capture.Start();
                button.Text = _StopCameraButtonText;
#endif
            }
            else
            {
                if (_renderMat == null)
                    _renderMat = new Mat();
                String message = _model.ProcessAndRender(images[0], _renderMat);
                
                SetImage(_renderMat);
                SetMessage(message);
                Picker.IsEnabled = true;
            }
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
        
    }
}
