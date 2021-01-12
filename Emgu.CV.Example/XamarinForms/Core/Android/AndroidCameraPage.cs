//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

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

        public AndroidCameraPage()
            : base()
        {
            _imageView = new ImageView(Android.App.Application.Context);
            Xamarin.Forms.View xView = _imageView.ToView();
            //xView.BackgroundColor = Color.Aqua;
            MainLayout.Children.Add(xView);
            base.DisplayImage.IsVisible = false;
            
        }

        public void StartCapture(EventHandler<Mat> matHandler, int preferredPreviewImageSize = -1)
        {
            if (_cameraManager == null)
            {
                if (preferredPreviewImageSize <= 0)
                {
                    //prefer preview image that is slightly smaller than the screen resolution
                    preferredPreviewImageSize = (int) Math.Round(MainLayout.Width * MainLayout.Width) / 2;
                    preferredPreviewImageSize = Math.Max(preferredPreviewImageSize, 480 * 600);
                }

                _cameraManager = new AndroidCameraManager( preferredPreviewImageSize );
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