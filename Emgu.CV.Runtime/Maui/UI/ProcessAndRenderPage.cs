//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
//using String=System.String;
#endif

#if __IOS__
using UIKit;
using CoreGraphics;
#endif

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using Emgu.CV.Face;
using Emgu.CV.Models;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Environment = System.Environment;
using Point = System.Drawing.Point;

namespace Emgu.CV.Platform.Maui.UI
{
    /// <summary>
    /// Generic page that can take a IProcessAndRenderModel and apply it to images / camera stream
    /// </summary>
    public class ProcessAndRenderPage
        : ButtonTextImagePage
    {
        private VideoCapture _capture = null;
        private Mat _mat;
        private Mat _renderMat;
        private Mat _buffer;
        private String _defaultButtonText;
        private IProcessAndRenderModel _model;
        private String _deaultImage;

        /// <summary>
        /// The text displayed on the button that stops the camera.
        /// </summary>
        protected String _StopCameraButtonText = "Stop Camera";

        /// <summary>
        /// Get the model associated with this page
        /// </summary>
        public IProcessAndRenderModel Model
        {
            get
            {
                return _model;
            }
        }

        /// <summary>
        /// Initialize video capture
        /// </summary>
        /// <returns>True if video capture initialized</returns>
        public bool InitVideoCapture()
        {
            var openCVConfigDict = CvInvoke.ConfigDict;
            bool haveVideoio = (openCVConfigDict["HAVE_OPENCV_VIDEOIO"] != 0);
            if (haveVideoio && (
                    Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android
                || Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.MacCatalyst
                || Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.WinUI))
            {
#if __ANDROID__ 
                if (CameraBackend == AndroidCameraBackend.AndroidCamera2)
                    return true;
#endif

                if (CvInvoke.Backends.Length > 0)
                {
                    if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        _capture = new VideoCapture(0, VideoCapture.API.Android);
                    }
                    else
                    {
                        _capture = new VideoCapture();
                    }
                    if (_capture.IsOpened)
                    {
                        _capture.ImageGrabbed += _capture_ImageGrabbed;
                        return true;
                    }
                    else
                    {
                        _capture.Dispose();
                        _capture = null;
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// Determines the default camera option based on the device platform.
        /// </summary>
        /// <returns>
        /// Returns true if the device platform is iOS, otherwise it initializes video capture and returns the result of that operation.
        /// </returns>
        protected override bool GetDefaultCameraOption()
        {
            if (Microsoft.Maui.Devices.DeviceInfo.Platform == DevicePlatform.iOS)
                return true;
            else
                return InitVideoCapture();
        }

        /// <summary>
        /// Create a Generic page that can take a IProcessAndRenderModel and apply it to images / camera stream
        /// </summary>
        /// <param name="model">The IProcessAndRenderModel</param>
        /// <param name="defaultButtonText">The text to be displayed on the button</param>
        /// <param name="defaultImage">The default image file name to use for processing. If set to null, it will use realtime camera stream instead.</param>
        /// <param name="defaultLabelText">The text to be displayed on the label</param>
        /// <param name="additionalButtons">Additional buttons to be added to the page if needed</param>
        public ProcessAndRenderPage(
            IProcessAndRenderModel model,
            String defaultButtonText,
            String defaultImage,
            String defaultLabelText = null,
            Microsoft.Maui.Controls.Button[] additionalButtons = null
            )
            : base(additionalButtons)
        {
#if __IOS__
            AllowAvCaptureSession = true;
            outputRecorder.BufferReceived += OutputRecorder_BufferReceived;
#endif
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

#if __IOS__

        //private int _counter = 0;
        private void OutputRecorder_BufferReceived(object sender, OutputRecorder.BufferReceivedEventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            try
            {
                //_counter++;
                #region read image into _mat
                var sampleBuffer = e.Buffer;
                using (CoreVideo.CVPixelBuffer pixelBuffer = sampleBuffer.GetImageBuffer() as CoreVideo.CVPixelBuffer)
                {
                    // Lock the base address
                    pixelBuffer.Lock(CoreVideo.CVPixelBufferLock.ReadOnly);
                    using (CoreImage.CIImage ciImage = new CoreImage.CIImage(pixelBuffer))
                    {
                        ciImage.ToArray(_mat, ImreadModes.Color);
                    }
                    pixelBuffer.Unlock(CoreVideo.CVPixelBufferLock.ReadOnly);
                }
                #endregion

                if (_renderMat == null)
                    _renderMat = new Mat();

                using (InputArray iaImage = _mat.GetInputArray())
                    iaImage.CopyTo(_renderMat);

                String msg = _model.ProcessAndRender(_mat, _renderMat);
                SetImage(_renderMat);
                SetMessage(msg);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                SetImage(null);
                SetMessage(exception.Message);
            }
        }
#endif

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            _model.Clear();
        }

        private bool ProcessFrame(IInputArray m)
        {
            if (_renderMat == null)
                _renderMat = new Mat();

            try
            {
                String msg;
                if (_model.RenderMethod == RenderType.Update)
                {
                    if (_buffer == null)
                        _buffer = new Mat();
                    using (InputArray iaM = m.GetInputArray())
                    {
                        iaM.CopyTo(_buffer);
                        msg = _model.ProcessAndRender(m, _buffer);
                        _buffer.CopyTo(_renderMat);
                    }
                }
                else
                {
                    msg = _model.ProcessAndRender(m, _renderMat);
                }

                SetImage(_renderMat);
                SetMessage(msg);
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                SetImage(null);
                SetMessage(exception.Message);
#if DEBUG
                throw;
#else
                return false;
#endif

            }
        }

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_mat == null)
                _mat = new Mat();
            _capture.Retrieve(_mat);

            ProcessFrame(_mat);

        }

        /// <summary>
        /// Function to call when the button is clicked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">Additional arguments</param>
        protected virtual async void OnButtonClicked(Object sender, EventArgs args)
        {
            var button = GetButton();

            if (button.Text.Equals(_StopCameraButtonText))
            {
#if __ANDROID__
                if (CameraBackend == AndroidCameraBackend.AndroidCamera2)
                    StopCapture();
                else
                {
                    _capture.Stop();
                    _capture.Dispose();
                    _capture = null;
                }
#elif __IOS__
                this.StopCaptureSession();
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

            Mat[] images;
            if (_deaultImage == null)
            {
                //Force to use Camera
                images = new Mat[0];
            }
            else
            {
                images = await LoadImages(new string[] { _deaultImage });

                if (images == null || (images.Length > 0 && images[0] == null))
                    return;
            }

            SetMessage("Please wait while we initialize the model...");
            SetImage(null);

            Picker p = this.Picker;
            if ((!p.IsVisible) || p.SelectedIndex < 0)
                await _model.Init(DownloadManager_OnDownloadProgressChanged, null);
            else
            {
                await _model.Init(DownloadManager_OnDownloadProgressChanged, p.Items[p.SelectedIndex].ToString());
            }

            if (!_model.Initialized)
            {
                String failMsg = "Failed to initialize model";
                Console.WriteLine(failMsg);
                SetImage(null);
                SetMessage(failMsg);
                return;
            }
            else
            {
                SetMessage("Model initialized");
            }

            if (images.Length == 0)
            {
#if __ANDROID__
                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        SetMessage("Failed to get camera permission.");
                        return;
                    }
                }
                
                if (CameraBackend == AndroidCameraBackend.AndroidCamera2)
                {
                    StartCapture(
                        async delegate (Object captureSender, Mat m)
                        {
                            //Skip the frame if busy, 
                            //Otherwise too many frames arriving and will eventually saturated the memory.
                            if (!IsAndroidCamera2Busy)
                            {
                                IsAndroidCamera2Busy = true;
                                try
                                {
                                    //This run on the main thread, using the async pattern with the ProcessFrame is required,
                                    //otherwise it will freeze up the main thread/app.
                                    await Task.Run(
                                        () =>
                                        {
                                            ProcessFrame(m); 
                                        });
                                }
                                finally
                                {
                                    IsAndroidCamera2Busy = false;
                                }
                            }
                        },
                        -1,
                        _preferredCameraId
                        );
                } else
                {
                    //Handle video
                    if (_capture == null)
                    {
                        InitVideoCapture();
                    }

                    if (_capture != null)
                        _capture.Start();
                }
#elif __IOS__
                CheckVideoPermissionAndStart();
#else
                //Handle video
                if (_capture == null)
                {
                    SetMessage("Initializing camera, please wait...");
                    await Task.Run(() => { this.InitVideoCapture(); });
                    SetMessage("Camera initialized.");

                }

                if (_capture != null)
                    _capture.Start();

#endif
                button.Text = _StopCameraButtonText;
            }
            else
            {
                ProcessFrame(images[0]);
                Picker.IsEnabled = true;
                /*
                if (_renderMat == null)
                    _renderMat = new Mat();
                images[0].CopyTo(_renderMat);

                try
                {
                    String message = _model.ProcessAndRender(images[0], _renderMat);

                    SetImage(_renderMat);
                    SetMessage(message);
                    Picker.IsEnabled = true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    SetImage(null);
                    SetMessage(exception.Message);

#if DEBUG
                    throw;
#endif
                }*/
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

        /// <summary>
        /// Function to call when the model download progress has been changed
        /// </summary>
        /// <param name="totalBytesToReceive">The total number of bytes to be received</param>
        /// <param name="bytesReceived">The bytes received so far</param>
        /// <param name="progressPercentage">The download progress percentage</param>
        protected void DownloadManager_OnDownloadProgressChanged(long? totalBytesToReceive, long bytesReceived, double? progressPercentage)
        {
            String msg;
            if (totalBytesToReceive != null)
                msg = String.Format("{0} of {1} downloaded ({2}%)", ByteToSizeStr(bytesReceived), ByteToSizeStr(totalBytesToReceive.Value), progressPercentage);
            else
                msg = String.Format("{0} downloaded", ByteToSizeStr(bytesReceived));
            SetMessage(msg);
        }

    }
}
