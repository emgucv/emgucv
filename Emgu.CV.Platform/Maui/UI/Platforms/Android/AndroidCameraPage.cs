//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Android.Graphics;
using Android.Widget;

namespace Emgu.CV.Platform.Maui.UI
{
    public class AndroidCameraPage : ContentPage
    {
        private AndroidCameraManager _cameraManager;

        private EventHandler<Mat> _onImageCapturedHandler = null;

        public void StartCapture(EventHandler<Mat> matHandler, int preferredPreviewImageSize = -1)
        {
            if (_cameraManager == null)
            {
                if (preferredPreviewImageSize <= 0)
                {
                    //prefer preview image that is slightly smaller than the screen resolution
                    //preferredPreviewImageSize = (int) Math.Round(this.Width * this.Height) / 2;
                    preferredPreviewImageSize = Math.Max(preferredPreviewImageSize, 480 * 600);
                }

                _cameraManager = new AndroidCameraManager( preferredPreviewImageSize );
                _onImageCapturedHandler = matHandler;
                _cameraManager.OnImageCaptured += _onImageCapturedHandler;
                _cameraManager.StartBackgroundThread();
            }
            _cameraManager.CreateCaptureSession();
        }

        public void StopCapture()
        {
            if (_cameraManager != null)
            {
                _cameraManager.CloseCamera();
                _cameraManager.OnImageCaptured -= _onImageCapturedHandler;
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

    }
}

#endif