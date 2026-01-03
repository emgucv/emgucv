//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__ || __MACCATALYST__

using System;
using CoreGraphics;

using System.Threading;
using Foundation;
using AVFoundation;
using CoreVideo;
using CoreMedia;
using CoreImage;
using CoreFoundation;

namespace Emgu.Util
{
    /// <summary>
    /// Represents an abstract class for handling video capture sessions in the Emgu Util library.
    /// </summary>
    /// <remarks>
    /// This class provides the base functionality for managing video capture sessions, including methods for checking video permissions, setting up capture sessions, and stopping capture sessions. 
    /// It also includes a property for allowing capture sessions and a method for setting messages. 
    /// Derived classes should implement additional functionality as needed.
    /// </remarks>
    public abstract class AvCaptureSessionPage : ContentPage
    {
        /// <summary>
        /// Represents the AVCaptureSession instance used for managing the capture activity.
        /// </summary>
        /// <remarks>
        /// This session is responsible for coordinating the data flow from the input devices to the output objects.
        /// It is initialized with medium preset settings and is started when the capture device is authorized and available.
        /// </remarks>
        protected AVCaptureSession session;
        
        /// <summary>
        /// An instance of the OutputRecorder class that handles the output of the AVCaptureSession.
        /// </summary>
        /// <remarks>
        /// This field is used to delegate the handling of the output data from the AVCaptureSession. 
        /// It is set to a new instance of OutputRecorder, which overrides the DidOutputSampleBuffer method 
        /// to define the behavior when a new sample buffer of video data is outputted by the AVCaptureSession.
        /// </remarks>
        protected OutputRecorder outputRecorder = new OutputRecorder();

        /// <summary>
        /// Represents a dispatch queue used for handling video data output in a separate thread.
        /// </summary>
        /// <remarks>
        /// This queue is used to delegate the processing of video data output to a separate thread, 
        /// allowing for non-blocking video capture and processing within the AVCaptureSession.
        /// </remarks>
        protected DispatchQueue queue;

        /// <summary>
        /// Gets or sets a value indicating whether the AV Capture Session is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if AV Capture Session is allowed; otherwise, <c>false</c>.
        /// </value>
        public bool AllowAvCaptureSession { get; set; }

        /// <summary>
        /// Checks the video capture permission and starts the capture session if permission is granted.
        /// </summary>
        /// <remarks>
        /// If the permission is not determined, it requests the user for video capture permission. 
        /// If the permission is granted, it sets up the capture session. 
        /// If the permission is denied or restricted, it prompts the user to grant video capture permission.
        /// </remarks>
        protected void CheckVideoPermissionAndStart()
        {
            AVFoundation.AVAuthorizationStatus authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVAuthorizationMediaType.Video);
            switch (authorizationStatus)
            {
                case AVAuthorizationStatus.NotDetermined:
                    AVCaptureDevice.RequestAccessForMediaType(AVAuthorizationMediaType.Video, delegate (bool granted)
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

        /// <summary>
        /// Sets a message to be displayed, typically used for error messages or notifications.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="heightRequest">Optional parameter specifying the height request for the message. Default value is 60.</param>
        public virtual void SetMessage(String message, int heightRequest = 60)
        {

        }

        /// <summary>
        /// Sets up the capture session for video capturing.
        /// </summary>
        /// <remarks>
        /// This method configures the capture session for medium resolution video capturing. It creates a device input and attaches it to the session.
        /// It also creates a VideoDataOutput and adds it to the session. If the session is null, a new session is created and started.
        /// If the capture device is not found or there is no input from the capture device, appropriate messages are set.
        /// </remarks>
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
                var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaTypes.Video);
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

                output.SetSampleBufferDelegate(outputRecorder, queue);
                session.AddOutput(output);
            }
            session.StartRunning();

        }

        /// <summary>
        /// Stops the capture session and releases the associated resources.
        /// </summary>
        public void StopCaptureSession()
        {
            session.StopRunning();
            session.Dispose();
            session = null;
        }
    }
}

#endif