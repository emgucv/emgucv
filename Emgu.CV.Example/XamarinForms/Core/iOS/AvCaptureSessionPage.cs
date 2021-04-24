//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__ || __MACOS__

using System;
using CoreGraphics;
using Xamarin.Forms;

using System.Threading;
using Foundation;
using AVFoundation;
using CoreVideo;
using CoreMedia;
using CoreImage;
using CoreFoundation;

namespace Emgu.Util
{
    public abstract class AvCaptureSessionPage : Xamarin.Forms.ContentPage
    {
        protected AVCaptureSession session;
        protected OutputRecorder outputRecorder = new OutputRecorder();
        protected DispatchQueue queue;

        public bool AllowAvCaptureSession { get; set; }

        protected void CheckVideoPermissionAndStart()
        {
            AVFoundation.AVAuthorizationStatus authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            switch (authorizationStatus)
            {
                case AVAuthorizationStatus.NotDetermined:
                    AVCaptureDevice.RequestAccessForMediaType(AVMediaType.Video, delegate (bool granted)
                    {
                        if (granted)
                        {
                            SetupCaptureSession();
                        }
                        else
                        {
                            SetMessage("Please grant Video Capture permission");
                            //RenderImageMessage("Please grant Video Capture permission");
                        }
                    });
                    break;
                case AVAuthorizationStatus.Authorized:
                    SetupCaptureSession();
                    break;
                case AVAuthorizationStatus.Denied:
                case AVAuthorizationStatus.Restricted:
                    SetMessage("Please grant Video Capture permission");
                    break;
                default:

                    break;
                    //do nothing
            }
        }

        public virtual void SetMessage(String message, int heightRequest = 60)
        {

        }

        protected void SetupCaptureSession()
        {
            if (session == null)
            {
                // configure the capture session for low resolution, change this if your code
                // can cope with more data or volume
                session = new AVCaptureSession()
                {
                    SessionPreset = AVCaptureSession.PresetMedium
                };

                // create a device input and attach it to the session
                var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
                if (captureDevice == null)
                {
                    //RenderImageMessage("Capture device not found.");
                    //_label.Text = "Capture device not found.";
                    SetMessage("Capture device not found.");
                    return;
                }
                var input = AVCaptureDeviceInput.FromDevice(captureDevice);
                if (input == null)
                {
                    //RenderImageMessage("No input device");
                    //_label.Text = "No input device";
                    SetMessage("No input from the capture Device.");
                    return;
                }
                session.AddInput(input);

                // create a VideoDataOutput and add it to the sesion
                AVVideoSettingsUncompressed settingUncomp = new AVVideoSettingsUncompressed();
                settingUncomp.PixelFormatType = CVPixelFormatType.CV32BGRA;
                var output = new AVCaptureVideoDataOutput()
                {
                    UncompressedVideoSetting = settingUncomp,

                    // If you want to cap the frame rate at a given speed, in this sample: 15 frames per second
                    //MinFrameDuration = new CMTime (1, 15)
                };


                // configure the output
                queue = new DispatchQueue("myQueue");

                output.SetSampleBufferDelegateQueue(outputRecorder, queue);
                session.AddOutput(output);
            }
            session.StartRunning();

        }

        public void StopCaptureSession()
        {
            session.StopRunning();
            session.Dispose();
            session = null;
        }
    }
}

#endif