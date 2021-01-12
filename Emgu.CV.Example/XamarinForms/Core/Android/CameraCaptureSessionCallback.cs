//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Emgu.CV.XamarinForms
{
    public class CameraCaptureSessionCallback : CameraCaptureSession.StateCallback
    {
        private CameraDevice _cameraDevice;
        private String _tag;
        private Surface _surface;
        private object _cameraStateLock;
        private Handler _backgroundHandler;

        public CameraCaptureSessionCallback(CameraDevice cameraDevice, Surface surface, object cameraStateLock, Handler backgroundHandler, String tag)
        {
            _cameraDevice = cameraDevice;
            _surface = surface;
            _cameraStateLock = cameraStateLock;
            _backgroundHandler = backgroundHandler;
            _tag = tag;
        }

        public EventHandler<CameraCaptureSession> OnConfigurationComplete;

        public override void OnConfigured(CameraCaptureSession cameraCaptureSession)
        {
            lock (_cameraStateLock)
            {
                
                try
                {
                    //Setup3AControlsLocked(mPreviewRequestBuilder);
                    // Finally, we start displaying the camera preview.
                    var captureRequest = CreateCaptureRequest();

                    cameraCaptureSession.SetRepeatingRequest(
                        captureRequest,
                        null, 
                        _backgroundHandler);
                    
                    if (OnConfigurationComplete != null)
                        OnConfigurationComplete(this, cameraCaptureSession);
                }
                catch (CameraAccessException e)
                {
                    e.PrintStackTrace();
                    return;
                }
                catch (Java.Lang.IllegalStateException e)
                {
                    e.PrintStackTrace();
                    return;
                }
                
            }
        }



        public override void OnConfigureFailed(CameraCaptureSession session)
        {
            Log.Error(_tag, "Failed to configure camera.");
            
        }

        private CaptureRequest CreateCaptureRequest()
        {
            try
            {
                CaptureRequest.Builder builder = _cameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
                builder.AddTarget(_surface);
                return builder.Build();
            }
            catch (CameraAccessException e)
            {
                Log.Error(_tag, e.Message);
                return null;
            }
        }
    }
}

#endif