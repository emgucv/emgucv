//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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


        protected String _preferredCameraId = null;

        public enum AndroidCameraBackend
        {
            AndroidCamera2,
            OpenCV
        }

        private AndroidCameraBackend _androidCameraBackend = AndroidCameraBackend.AndroidCamera2;
        //private AndroidCameraBackend _androidCameraBackend = AndroidCameraBackend.OpenCV;

        public AndroidCameraBackend CameraBackend
        {
            get
            {
                return _androidCameraBackend;
            }
            set
            {
                _androidCameraBackend = value;
            }
        }

        private bool _isAndroidCamera2Busy = false;

        public bool IsAndroidCamera2Busy
        {
            get
            {
                return _isAndroidCamera2Busy;
            }
            set
            {
                _isAndroidCamera2Busy = value;
            }
        }


        public void StartCapture(EventHandler<Mat> matHandler, int preferredPreviewImageSize = -1, String preferedCameraId = null)
        {
            if (_cameraManager == null)
            {
                if (preferredPreviewImageSize <= 0)
                {
                    //prefer preview image that is slightly smaller than the screen resolution
                    //preferredPreviewImageSize = (int) Math.Round(this.Width * this.Height) / 2;
                    preferredPreviewImageSize = Math.Max(preferredPreviewImageSize, 480 * 600);
                }

                _cameraManager = new AndroidCameraManager( preferredPreviewImageSize, preferedCameraId);
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