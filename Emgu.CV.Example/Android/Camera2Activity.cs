//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

//
// Camera preview based on monodroid API-samples
// 
// https://developer.xamarin.com/samples/monodroid/android5.0/Camera2Basic/



using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
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
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
//using Java.Lang;
using Paint = Android.Graphics.Paint;
using Java.Util.Concurrent;
using Camera = Android.Hardware.Camera;
using Type = System.Type;

namespace AndroidExamples
{
    [Activity(Label = "Android Camera")]
    public class Camera2Activity : ButtonMessageImageActivity
    {
        public Camera2Activity()
            : this("Capture from Camera")
        {

        }
        public Camera2Activity(String buttonText)
            : base(buttonText)
        {

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
        static int mState = STATE_CLOSED;

        /// <summary>
        /// A lock protecting camera state.
        /// </summary>
        static readonly object mCameraStateLock = new object();

        /// <summary>
        /// A reference to the open {@link CameraDevice}.
        /// </summary>
        static CameraDevice mCameraDevice;

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
        static readonly Semaphore mCameraOpenCloseLock = new Semaphore(1);

        /// <summary>
        /// The {@link CameraCharacteristics} for the currently configured camera device.
        /// </summary>
        static CameraCharacteristics mCharacteristics;


        /// <summary>
        /// ID of the current {@link CameraDevice}.
        /// </summary>
        string mCameraId;

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
        private OnYuvImageAvailableListener mOnYuvImageAvailableListener = new OnYuvImageAvailableListener();

        /// <summary>
        /// Sets up state related to camera that is needed before opening a {@link CameraDevice}.
        /// </summary>
        /// <returns><c>true</c>, if up camera outputs was set, <c>false</c> otherwise.</returns>
        bool SetUpCameraOutputs()
        {
            var activity = this;
            CameraManager manager = (CameraManager)activity.GetSystemService(Context.CameraService);
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
                    Android.Util.Size largestYuv = yuvs.OrderByDescending(element => element.Width * element.Height).Last();

                    lock (mCameraStateLock)
                    {
                        // Set up ImageReaders for JPEG and RAW outputs.  Place these in a reference
                        // counted wrapper to ensure they are only closed when all background tasks
                        // using them are finished.
                        if (mYuvImageReader == null || mYuvImageReader.GetAndRetain() == null)
                        {
                            mYuvImageReader = new RefCountedAutoCloseable<ImageReader>(
                                ImageReader.NewInstance(largestYuv.Width,
                                    largestYuv.Height, ImageFormatType.Yuv420888, /*maxImages*/5));
                        }

                        mYuvImageReader.Get().SetOnImageAvailableListener(
                            mOnYuvImageAvailableListener, mBackgroundHandler);

                        mCharacteristics = characteristics;
                        mCameraId = cameraId;
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
        static RefCountedAutoCloseable<ImageReader> mYuvImageReader;

        /// <summary>
        /// CameraStateListener is called when the currently active {@link CameraDevice}
        /// changes its state.
        /// </summary>
        CameraDevice.StateCallback mStateCallback;

        public class CameraStateListener : CameraDevice.StateCallback
        {
            Camera2Activity Activity { get; set; }

            public CameraStateListener(Camera2Activity activity)
            {
                Activity = activity;
            }

            public override void OnOpened(CameraDevice camera)
            {

                // This method is called when the camera is opened.  We start camera preview here if
                // the TextureView displaying this has been set up.
                lock (mCameraStateLock)
                {

                    mState = STATE_OPENED;
                    mCameraOpenCloseLock.Release();
                    mCameraDevice = camera;
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
                lock (mCameraStateLock)
                {

                    mState = STATE_CLOSED;
                    mCameraOpenCloseLock.Release();
                    camera.Close();
                    mCameraDevice = null;
                }
            }

            public override void OnError(CameraDevice camera, Android.Hardware.Camera2.CameraError error)
            {

                Log.Error(TAG, "Received camera device error: " + error);
                lock (mCameraStateLock)
                {
                    mState = STATE_CLOSED;
                    mCameraOpenCloseLock.Release();
                    camera.Close();
                    mCameraDevice = null;
                }
                var activity = Activity;
                if (null != activity)
                {
                    activity.Finish();
                }
            }
        }

        /// <summary>
        /// A {@link Handler} for running tasks in the background.
        /// </summary>
        static Handler mBackgroundHandler;

        /// <summary>
        /// An additional thread for running tasks that shouldn't block the UI.  This is used for all
        /// callbacks from the {@link CameraDevice} and {@link CameraCaptureSession}s.
        /// </summary>
        HandlerThread mBackgroundThread;


        /// <summary>
        /// A {@link CameraCaptureSession } for camera preview.
        /// </summary>
        static CameraCaptureSession mCaptureSession;

        protected override void OnResume()
        {
            base.OnResume();
            StartBackgroundThread();
            /*
            if (CanOpenCamera())
            {

                // When the screen is turned off and turned back on, the SurfaceTexture is already
                // available, and "onSurfaceTextureAvailable" will not be called. In that case, we should
                // configure the preview bounds here (otherwise, we wait until the surface is ready in
                // the SurfaceTextureListener).
                if (mTextureView.IsAvailable)
                {
                    ConfigureTransform(mTextureView.Width, mTextureView.Height, Activity);
                }
                else
                {
                    mTextureView.SurfaceTextureListener = mSurfaceTextureListener;
                }
                if (mOrientationListener != null && mOrientationListener.CanDetectOrientation())
                {
                    mOrientationListener.Enable();
                }
            }*/
        }

        /*
        /// <summary>
        /// Number of pending user requests to capture a photo.
        /// </summary>
        static int mPendingUserCaptures = 0;
        */

        /// <summary>
        /// Closes the current {@link CameraDevice}.
        /// </summary>
        void CloseCamera()
        {
            try
            {
                mCameraOpenCloseLock.Acquire();
                lock (mCameraStateLock)
                {

                    // Reset state and clean up resources used by the camera.
                    // Note: After calling this, the ImageReaders will be closed after any background
                    // tasks saving Images from these readers have been completed.
                    //mPendingUserCaptures = 0;
                    mState = STATE_CLOSED;
                    if (null != mCaptureSession)
                    {
                        mCaptureSession.Close();
                        mCaptureSession = null;
                    }
                    if (null != mCameraDevice)
                    {
                        mCameraDevice.Close();
                        mCameraDevice = null;
                    }
                    if (null != mYuvImageReader)
                    {
                        mYuvImageReader.Close();
                        mYuvImageReader = null;
                    }

                }
            }
            catch (Java.Lang.InterruptedException e)
            {
                throw new Java.Lang.RuntimeException("Interrupted while trying to lock camera closing.", e);
            }
            finally
            {
                mCameraOpenCloseLock.Release();
            }
        }

        protected override void OnPause()
        {
            /*
            if (mOrientationListener != null)
            {
                mOrientationListener.Disable();
            }*/
            CloseCamera();
            StopBackgroundThread();
            base.OnPause();
        }

        /// <summary>
        /// Starts a background thread and its {@link Handler}.
        /// </summary>
        void StartBackgroundThread()
        {
            mBackgroundThread = new HandlerThread("CameraBackground");
            mBackgroundThread.Start();
            lock (mCameraStateLock)
            {
                mBackgroundHandler = new Handler(mBackgroundThread.Looper);
            }
        }

        /// <summary>
        /// Stops the background thread and its {@link Handler}.
        /// </summary>
        void StopBackgroundThread()
        {
            mBackgroundThread.QuitSafely();
            try
            {
                mBackgroundThread.Join();
                mBackgroundThread = null;
                lock (mCameraStateLock)
                {
                    mBackgroundHandler = null;
                }
            }
            catch (Java.Lang.InterruptedException e)
            {
                e.PrintStackTrace();
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (!SetUpCameraOutputs())
                return;

            this.mOnYuvImageAvailableListener.OnImageProcessed += (sender, mat) =>
            {
                SetImage(mat);
            };

            var activity = this;
            CameraManager manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            mStateCallback = new CameraStateListener(this);
            try
            {
                Handler backgroundHandler;

                var list = manager.GetCameraIdList();
                var cameraId = list[0];
                backgroundHandler = mBackgroundHandler;

                // Attempt to open the camera. mStateCallback will be called on the background handler's
                // thread when this succeeds or fails.
                manager.OpenCamera(cameraId, mStateCallback, backgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            catch (Java.Lang.InterruptedException e)
            {
                throw new Java.Lang.RuntimeException("Interrupted while trying to lock camera opening.", e);
            }

            this.OnButtonClick += (sender, args) =>
            {
                CreateCaptureSession();
            };
        }

        public void CreateCaptureSession()
        {
            var surface = mYuvImageReader.Get().Surface;
            //surface.DescribeContents()
            CameraCaptureSessionCallback ccsc =
                new CameraCaptureSessionCallback(mCameraDevice, surface, mCameraStateLock, mBackgroundHandler, TAG);
            ccsc.OnConfigurationComplete += (sender, args) =>
            {
                mState = STATE_PREVIEW;
                mCaptureSession = args;
            };
            mCameraDevice.CreateCaptureSession(new List<Surface>() { surface }, ccsc, mBackgroundHandler);
        }

        //private CameraCaptureSessionCallback sessionStateCallback = new CameraCaptureSessionCallback();


    }
}

