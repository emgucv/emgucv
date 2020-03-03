//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.OS;
using Android.Renderscripts;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Paint = Android.Graphics.Paint;
using Java.Util.Concurrent;
using Xamarin.Forms.Platform.Android;
using Camera = Android.Hardware.Camera;
using Color = Xamarin.Forms.Color;
using Type = System.Type;


namespace Emgu.CV.XamarinForms
{
    public class AndroidCameraPage : ButtonTextImagePage
    {
        private AndroidCameraManager _cameraManager;
        private Bitmap[] _renderBuffer = new Bitmap[2];
        private int _renderBufferIdx = 0;

        private ImageView _imageView;

        public ImageView AndroidImageView
        {
            get { return _imageView; }
        }

        //private String _defaultButtonText = "Open Camera";
        //private String _stopCameraText = "Stop Camera";

        public AndroidCameraPage()
            : base()
        {
            _imageView = new ImageView(Android.App.Application.Context);
            Xamarin.Forms.View xView = _imageView.ToView();
            //xView.BackgroundColor = Color.Aqua;
            MainLayout.Children.Add(xView);
            base.DisplayImage.IsVisible = false;
            
            /*
            var button = this.GetButton();
            button.Text = _defaultButtonText;
            button.Clicked += async (Object sender, EventArgs args) =>
            {
                if (button.Text.Equals(_defaultButtonText))
                {
                    button.Text = _stopCameraText;
                    StartCapture(ProcessImage);
                }
                else
                {
                    StopCapture();
                    button.Text = _defaultButtonText;
                }
            };*/
        }

        /*
        public virtual void ProcessImage(Object sender, Mat mat)
        {
            //Do some image processing

            //Render it.
            SetImage(mat);
        }*/

        public void StartCapture(EventHandler<Mat> matHandler)
        {
            if (_cameraManager == null)
            {
                //prefer preview image that is slightly smaller than the screen resolution
                int preferredSize = (int) Math.Round(MainLayout.Width * MainLayout.Width) / 2;
                preferredSize = Math.Max(preferredSize, 480 * 600);
                _cameraManager = new AndroidCameraManager( preferredSize );
                _cameraManager.OnImageCaptured += matHandler;
                _cameraManager.StartBackgroundThread();
            }
            _cameraManager.CreateCaptureSession();
        }

        public void StopCapture()
        {
            if (_cameraManager != null)
            {
                _cameraManager.CloseCamera();
                _cameraManager.StopBackgroundThread();
                _cameraManager = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
        }

        protected override void OnDisappearing()
        {
            StopCapture();
            base.OnDisappearing();
        }

        //private CameraCaptureSessionCallback sessionStateCallback = new CameraCaptureSessionCallback();

        public override void SetImage(IInputArray image)
        {
            if (image == null)
            {
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    _imageView.SetImageBitmap(null);
                });
                return;
            }

            int bufferIdx = _renderBufferIdx;
            Bitmap buffer;
            _renderBufferIdx = (_renderBufferIdx + 1) % _renderBuffer.Length;

            using (InputArray iaImage = image.GetInputArray())
            using (Mat mat = iaImage.GetMat())
            {
                if (_renderBuffer[bufferIdx] == null)
                {
                    buffer = mat.ToBitmap();
                    _renderBuffer[bufferIdx] = buffer;
                }
                else
                {
                    var size = iaImage.GetSize();
                    buffer = _renderBuffer[bufferIdx];
                    if (buffer.Width != size.Width || buffer.Height != size.Height)
                    {
                        buffer.Dispose();
                        _renderBuffer[bufferIdx] = mat.ToBitmap();
                    }
                    else
                    {
                        mat.ToBitmap(buffer);
                    }
                }
            }

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                _imageView.SetImageBitmap(buffer);
            });
        }
    }
}

#endif