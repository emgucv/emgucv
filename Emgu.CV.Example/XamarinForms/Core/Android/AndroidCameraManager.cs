//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using Type = System.Type;


namespace Emgu.CV.XamarinForms
{
    public class AndroidCameraManager
    {
        public EventHandler<Mat> OnImageCaptured;

        public AndroidCameraManager(int? preferredPreviewSize = null)
        {
            if (!SetUpCameraOutputs(preferredPreviewSize))
                return;

            this._onYuvImageAvailableListener.OnImageProcessed += (sender, mat) =>
            {
                this.OnImageCaptured(sender, mat);
            };

            //var activity = this;
            CameraManager manager =
                (CameraManager)Android.App.Application.Context.GetSystemService(Context.CameraService);
            _stateCallback = new CameraStateListener();
            try
            {
                Handler backgroundHandler;

                var list = manager.GetCameraIdList();
                var cameraId = list[0];
                backgroundHandler = _backgroundHandler;

                // Attempt to open the camera. mStateCallback will be called on the background handler's
                // thread when this succeeds or fails.
                manager.OpenCamera(cameraId, _stateCallback, backgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (Java.Lang.InterruptedException e)
            {
                throw new Java.Lang.RuntimeException("Interrupted while trying to lock camera opening.", e);
            }
        }



        // Camera state: Device is closed.
        const int STATE_CLOSED = 0;

        // Camera state: Device is opened, but is not capturing.
        const int STATE_OPENED = 1;

        // Camera state: Showing camera preview.
        const int STATE_PREVIEW = 2;

        // Camera state: Waiting for 3A convergence before capturing a photo.
        const int STATE_WAITING_FOR_3A_CONVERGENCE = 3;


        // Tag for the {@link Log}.
        const string TAG = "Camera2RawFragment";


        /// <summary>
        /// The state of the camera device.
        /// 
        /// @see #mPreCaptureCallback
        /// </summary>
        static int _state = STATE_CLOSED;

        /// <summary>
        /// A lock protecting camera state.
        /// </summary>
        static readonly object _cameraStateLock = new object();

        /// <summary>
        /// A reference to the open {@link CameraDevice}.
        /// </summary>
        static CameraDevice _cameraDevice;

        public CameraDevice CameraDevice
        {
            get { return _cameraDevice; }
        }

        /*
        /// <summary>>
        /// A {@link CameraCaptureSession.CaptureCallback} that handles events for the preview and
        /// pre-capture sequence.
        /// </summary>
        static CameraCaptureSession.CaptureCallback mPreCaptureCallback;
        */

        /// <summary>
        /// A {@link Semaphore} to prevent the app from exiting before closing the camera.
        /// </summary>
        static readonly Semaphore _cameraOpenCloseLock = new Semaphore(1);

        /// <summary>
        /// The {@link CameraCharacteristics} for the currently configured camera device.
        /// </summary>
        static CameraCharacteristics _characteristics;


        /// <summary>
        /// ID of the current {@link CameraDevice}.
        /// </summary>
        string _cameraId;

        /// <summary>
        /// Return true if the given array contains the given integer.
        /// </summary>
        /// <returns><c>true</c>, if the array contains the given integer, <c>false</c> otherwise.</returns>
        /// <param name="modes">array to check.</param>
        /// <param name="mode">integer to get for.</param>
        static bool Contains(int[] modes, int mode)
        {
            if (modes == null)
            {
                return false;
            }

            foreach (int i in modes)
            {
                if (i == mode)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// This a callback object for the {@link ImageReader}. "onImageAvailable" will be called when a
        /// RAW image is ready to be saved.
        /// </summary>
        private OnYuvImageAvailableListener _onYuvImageAvailableListener = new OnYuvImageAvailableListener();

        /// <summary>
        /// Sets up state related to camera that is needed before opening a {@link CameraDevice}.
        /// </summary>
        /// <returns><c>true</c>, if up camera outputs was set, <c>false</c> otherwise.</returns>
        bool SetUpCameraOutputs(int? preferredPreviewSize = null)
        {
            //var activity = this;
            CameraManager manager =
                (CameraManager)Android.App.Application.Context.GetSystemService(Context.CameraService);
            if (manager == null)
            {
                Log.Error(TAG, "This device doesn't support Camera2 API.");

                return false;
            }

            try
            {
                // Find a CameraDevice that supports YUV captures, and configure state.
                foreach (string cameraId in manager.GetCameraIdList())
                {
                    CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);

                    /*
                    // We only use a camera that supports YUV in this sample.
                    if (!Contains(characteristics.Get(
                            CameraCharacteristics.RequestAvailableCapabilities).ToArray<int>(),
                            (int)RequestAvailableCapabilities.YuvReprocessing))
                    {
                        continue;
                    }*/

                    StreamConfigurationMap map = (StreamConfigurationMap)characteristics.Get(
                        CameraCharacteristics.ScalerStreamConfigurationMap);

                    // For still image captures, we use the largest available size.
                    Android.Util.Size[] yuvs = map.GetOutputSizes((int)ImageFormatType.Yuv420888);
                    Android.Util.Size yuvSize;
                    if (preferredPreviewSize == null)
                    {
                        //Choose the smallest size for performance.
                        yuvSize =
                            yuvs.OrderByDescending(element => element.Width * element.Height).Last();
                    }
                    else
                    {
                        yuvSize =
                            yuvs.OrderByDescending(element => Math.Abs( (element.Width * element.Height) - (int) preferredPreviewSize ) ).Last();
                    }

                    lock (_cameraStateLock)
                    {
                        // Set up ImageReaders for JPEG and RAW outputs.  Place these in a reference
                        // counted wrapper to ensure they are only closed when all background tasks
                        // using them are finished.
                        if (_yuvImageReader == null || _yuvImageReader.GetAndRetain() == null)
                        {
                            _yuvImageReader = new RefCountedAutoCloseable<ImageReader>(
                                ImageReader.NewInstance(yuvSize.Width,
                                    yuvSize.Height, ImageFormatType.Yuv420888, /*maxImages*/5));
                        }

                        _yuvImageReader.Get().SetOnImageAvailableListener(
                            _onYuvImageAvailableListener, _backgroundHandler);

                        _characteristics = characteristics;
                        _cameraId = cameraId;
                    }

                    return true;
                }
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }

            // If we found no suitable cameras for capturing YUV, warn the user.
            Log.Error(TAG, "This device doesn't support capturing YUV photos");

            return false;
        }

        /// <summary>
        /// A reference counted holder wrapping the {@link ImageReader} that handles RAW image captures.
        /// This is used to allow us to clean up the {@link ImageReader} when all background tasks using
        /// its {@link Image}s have completed.
        /// </summary>
        static RefCountedAutoCloseable<ImageReader> _yuvImageReader;

        /// <summary>
        /// CameraStateListener is called when the currently active {@link CameraDevice}
        /// changes its state.
        /// </summary>
        CameraDevice.StateCallback _stateCallback;

        public class CameraStateListener : CameraDevice.StateCallback
        {

            public override void OnOpened(CameraDevice camera)
            {

                // This method is called when the camera is opened.  We start camera preview here if
                // the TextureView displaying this has been set up.
                lock (_cameraStateLock)
                {

                    _state = STATE_OPENED;
                    _cameraOpenCloseLock.Release();
                    _cameraDevice = camera;
                    /*
                    // Start the preview session if the TextureView has been set up already.
                    if (mPreviewSize != null && mTextureView.IsAvailable)
                    {
                        CreateCameraPreviewSessionLocked();
                    }*/

                }
            }

            public override void OnDisconnected(CameraDevice camera)
            {
                lock (_cameraStateLock)
                {

                    _state = STATE_CLOSED;
                    _cameraOpenCloseLock.Release();
                    camera.Close();
                    _cameraDevice = null;
                }
            }

            public override void OnError(CameraDevice camera, Android.Hardware.Camera2.CameraError error)
            {

                Log.Error(TAG, "Received camera device error: " + error);
                lock (_cameraStateLock)
                {
                    _state = STATE_CLOSED;
                    _cameraOpenCloseLock.Release();
                    camera.Close();
                    _cameraDevice = null;
                }

                /*
                var activity = Parent;
                if (null != activity)
                {
                    //TODO: clean up parent
                    //activity.Finish();
                }*/
            }
        }

        /// <summary>
        /// A {@link Handler} for running tasks in the background.
        /// </summary>
        static Handler _backgroundHandler;

        /// <summary>
        /// An additional thread for running tasks that shouldn't block the UI.  This is used for all
        /// callbacks from the {@link CameraDevice} and {@link CameraCaptureSession}s.
        /// </summary>
        HandlerThread _backgroundThread;


        /// <summary>
        /// A {@link CameraCaptureSession } for camera preview.
        /// </summary>
        static CameraCaptureSession _captureSession;



        /// <summary>
        /// Closes the current {@link CameraDevice}.
        /// </summary>
        public void CloseCamera()
        {
            try
            {
                _cameraOpenCloseLock.Acquire();
                lock (_cameraStateLock)
                {

                    // Reset state and clean up resources used by the camera.
                    // Note: After calling this, the ImageReaders will be closed after any background
                    // tasks saving Images from these readers have been completed.
                    //mPendingUserCaptures = 0;
                    _state = STATE_CLOSED;
                    if (null != _captureSession)
                    {
                        _captureSession.Close();
                        _captureSession = null;
                    }

                    if (null != _cameraDevice)
                    {
                        _cameraDevice.Close();
                        _cameraDevice = null;
                    }

                    if (null != _yuvImageReader)
                    {
                        _yuvImageReader.Close();
                        _yuvImageReader = null;
                    }

                }
            }
            catch (Java.Lang.InterruptedException e)
            {
                throw new Java.Lang.RuntimeException("Interrupted while trying to lock camera closing.", e);
            }
            finally
            {
                _cameraOpenCloseLock.Release();
            }
        }


        /// <summary>
        /// Starts a background thread and its {@link Handler}.
        /// </summary>
        public void StartBackgroundThread()
        {
            _backgroundThread = new HandlerThread("CameraBackground");
            _backgroundThread.Start();
            lock (_cameraStateLock)
            {
                _backgroundHandler = new Handler(_backgroundThread.Looper);
            }
        }

        /// <summary>
        /// Stops the background thread and its {@link Handler}.
        /// </summary>
        public void StopBackgroundThread()
        {

            try
            {
                if (_backgroundThread != null)
                {
                    _backgroundThread.QuitSafely();
                    _backgroundThread.Join();
                    _backgroundThread = null;
                }

                lock (_cameraStateLock)
                {
                    _backgroundHandler = null;
                }
            }
            catch (Java.Lang.InterruptedException e)
            {
                e.PrintStackTrace();
            }
        }

        /// <summary>
        /// Maximum 10 seconds of wait time.
        /// </summary>
        private static int _maxWaitTimeMilliseconds = 20000;

        public async void CreateCaptureSession()
        {
            int totalWaitTime = 0;
            int increment = 100;
            //wait for the camera device to initialize
            while (_cameraDevice == null)
            {
                await Task.Delay(increment);
                totalWaitTime = totalWaitTime + increment;
                if (totalWaitTime > _maxWaitTimeMilliseconds)
                    throw new Exception(String.Format("Timeout ({0} milliseconds) waiting for camera device to be created.", _maxWaitTimeMilliseconds));
            }

            var surface = _yuvImageReader.Get().Surface;
            //surface.DescribeContents()
            CameraCaptureSessionCallback ccsc =
                new CameraCaptureSessionCallback(_cameraDevice, surface, _cameraStateLock, _backgroundHandler, TAG);
            ccsc.OnConfigurationComplete += (sender, args) =>
            {
                _state = STATE_PREVIEW;
                _captureSession = args;
            };
            _cameraDevice.CreateCaptureSession(new List<Surface>() { surface }, ccsc, _backgroundHandler);
        }

    }
}

#endif