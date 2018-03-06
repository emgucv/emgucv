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

        /// <summary>>
        /// A {@link CameraCaptureSession.CaptureCallback} that handles events for the preview and
        /// pre-capture sequence.
        /// </summary>
        static CameraCaptureSession.CaptureCallback mPreCaptureCallback;

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

        public class YUV420Converter : Emgu.Util.DisposableObject
        {
            private RenderScript rs;
            private ScriptIntrinsicYuvToRGB _yuvToRgbIntrinsic;
            private int _yuvLength;
            private Allocation _input;
            private int _width;
            private int _height;
            private Allocation _output;

            public YUV420Converter(Context context)
            {
                rs = RenderScript.Create(context);
                _yuvToRgbIntrinsic = ScriptIntrinsicYuvToRGB.Create(rs, Element.U8_4(rs));
            }
            
            public void YUV_420_888_toRGBIntrinsics(int width, int height, byte[] yuv, Bitmap bmpOut)
            {
                YUV_420_888_toRGBIntrinsicsProcess(width, height, yuv);
                _output.CopyTo(bmpOut);
            }

            public void YUV_420_888_toRGBIntrinsics(int width, int height, byte[] yuv, byte dataOut)
            {
                YUV_420_888_toRGBIntrinsicsProcess(width, height, yuv);
                _output.CopyTo(dataOut);
            }

            private void YUV_420_888_toRGBIntrinsicsProcess(int width, int height, byte[] yuv)
            {
                if (yuv.Length != _yuvLength)
                {

                    Android.Renderscripts.Type.Builder yuvType =
                        new Android.Renderscripts.Type.Builder(rs, Element.U8(rs)).SetX(yuv.Length);
                    if (_input != null)
                        _input.Destroy();
                    _input = Allocation.CreateTyped(rs, yuvType.Create(), AllocationUsage.Script);
                    _yuvLength = yuv.Length;
                }

                if (_width != width || _height != height)
                {
                    Android.Renderscripts.Type.Builder rgbaType = new Android.Renderscripts.Type.Builder(rs, Element.RGBA_8888(rs)).SetX(width).SetY(height);
                    if (_output != null)
                        _output.Destroy();
                    _output = Allocation.CreateTyped(rs, rgbaType.Create(), AllocationUsage.Script);
                    _width = width;
                    _height = height;
                }


                _input.CopyFromUnchecked(yuv);

                _yuvToRgbIntrinsic.SetInput(_input);
                _yuvToRgbIntrinsic.ForEach(_output);
               
            }

            public Bitmap YUV_420_888_toRGBIntrinsics(int width, int height, byte[] yuv)
            {
                Bitmap bmpOut = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
                YUV_420_888_toRGBIntrinsics(width, height, yuv, bmpOut);
                return bmpOut;
            }

            protected override void DisposeObject()
            {
                if (_input != null)
                {
                    _input.Destroy();
                    _input = null;
                }
                if (_yuvToRgbIntrinsic != null)
                {
                    _yuvToRgbIntrinsic.Destroy();
                    _yuvToRgbIntrinsic = null;
                }
                if (rs != null)
                {
                    rs.Destroy();
                    rs = null;
                }
            }
        }



        /// <summary>
        /// This a callback object for the {@link ImageReader}. "onImageAvailable" will be called when a
        /// RAW image is ready to be saved.
        /// </summary>
        private OnYuvImageAvailableListener mOnYuvImageAvailableListener = new OnYuvImageAvailableListener();
        class OnYuvImageAvailableListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
        {
            private Camera2Activity _activity;

            public Camera2Activity Activity
            {
               get { return _activity; }
                set { _activity = value; }
            }
            
            private byte[] _data = null;
            private Mat _bgrMat = new Mat();
            private Mat _rotatedMat = new Mat();

            private YUV420Converter _yuv420Converter;
            private Bitmap[] _bitmapSrcBuffer = new Bitmap[2];
            private int _bitmapBufferIdx = 0;
            
            public void OnImageAvailable(ImageReader reader)
            {
                
                Image image  = reader.AcquireLatestImage();
                if (image == null)
                    return;

                Image.Plane[] planes = image.GetPlanes();
                int totalLength = 0;
                for (int i = 0; i < planes.Length; i++)
                {
                    Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                    totalLength += buffer.Remaining();          
                }

                if (_data == null || _data.Length != totalLength)
                {
                    _data = new byte[totalLength];
                }
                int offset = 0;
                for (int i = 0; i < planes.Length; i++)
                {
                    Java.Nio.ByteBuffer buffer = planes[i].Buffer;
                    int length = buffer.Remaining();
                    
                    buffer.Get(_data, offset, length);
                    offset += length;
                }

                if (_yuv420Converter == null)
                    _yuv420Converter = new YUV420Converter(_activity);

                if (_bitmapSrcBuffer[_bitmapBufferIdx] == null || image.Width != (_bitmapSrcBuffer[_bitmapBufferIdx].Width) || (image.Height != _bitmapSrcBuffer[_bitmapBufferIdx].Height))
                {
                    _bitmapSrcBuffer[_bitmapBufferIdx] = Bitmap.CreateBitmap(image.Width, image.Height, Bitmap.Config.Argb8888);
                }
                Bitmap bmpSrc = _bitmapSrcBuffer[_bitmapBufferIdx];

                _yuv420Converter.YUV_420_888_toRGBIntrinsics(image.Width, image.Height, _data, bmpSrc);
                

                using (Mat m = new Mat(bmpSrc.Height, bmpSrc.Width, DepthType.Cv8U, 4, bmpSrc.LockPixels(),
                    bmpSrc.Width * 4))
                {
                    bmpSrc.UnlockPixels();
                    CvInvoke.CvtColor(m, _bgrMat, ColorConversion.Bgra2Bgr);

                    //Rotate 90 degree by transpose and flip
                    CvInvoke.Transpose(_bgrMat, _rotatedMat);
                    CvInvoke.Flip(_rotatedMat, _rotatedMat, FlipType.Horizontal);

                    //apply a simple invert filter
                    CvInvoke.BitwiseNot(_rotatedMat, _rotatedMat);
                }
                
                _activity.SetImage( _rotatedMat);
                _bitmapBufferIdx = (_bitmapBufferIdx + 1) % _bitmapSrcBuffer.Length;

                
                
                /*
                GCHandle dataHandle = GCHandle.Alloc(_data, GCHandleType.Pinned);
                using (Mat m = new Mat((image.Height << 1) + (image.Height >> 1), image.Width, DepthType.Cv8U, 1,
                    dataHandle.AddrOfPinnedObject(), image.Width))
                {
                    CvInvoke.CvtColor(m, _bgrMat, ColorConversion.Yuv420Sp2Bgr);

                    if (_activity != null)
                        _activity.SetImage(_bgrMat);
                }
                dataHandle.Free();
                */
                //int bytesPerPixel = ImageFormat.GetBitsPerPixel(Android.Graphics.ImageFormatType.Yuv420888) / 8;
                //int dataSize = width * height * bytesPerPixel;

                    /*
                    byte[] rowData = new byte[planes[0].RowStride];

                    for (int i = 0; i < planes.Length; i++)
                    {
                        buffer = planes[i].Buffer;
                        rowStride = planes[i].RowStride;
                        pixelStride = planes[i].PixelStride;
                        int w = (i == 0) ? width : width / 2;
                        int h = (i == 0) ? height : height / 2;
                        for (int row = 0; row < h; row++)
                        {
                            if (pixelStride == bytesPerPixel)
                            {
                                int length = w * bytesPerPixel;
                                buffer.Get(_data, offset, length);

                                // Advance buffer the remainder of the row stride, unless on the last row.
                                // Otherwise, this will throw an IllegalArgumentException because the buffer
                                // doesn't include the last padding.
                                if (h - row != 1)
                                {
                                    buffer.Position(buffer.Position() + rowStride - length);
                                }
                                offset += length;
                            }
                            else
                            {

                                // On the last row only read the width of the image minus the pixel stride
                                // plus one. Otherwise, this will throw a BufferUnderflowException because the
                                // buffer doesn't include the last padding.
                                if (h - row == 1)
                                {
                                    buffer.Get(rowData, 0, width - pixelStride + 1);
                                }
                                else
                                {
                                    buffer.Get(rowData, 0, rowStride);
                                }

                                for (int col = 0; col < w; col++)
                                {
                                    _data[offset++] = rowData[col * pixelStride];
                                }
                            }
                        }
                    }*/

                image.Close();
                //image.Dispose();
                
            }
        }

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
                // Find a CameraDevice that supports RAW captures, and configure state.
                foreach (string cameraId in manager.GetCameraIdList())
                {
                    CameraCharacteristics characteristics
                    = manager.GetCameraCharacteristics(cameraId);

                    /*
                    // We only use a camera that supports RAW in this sample.
                    if (!Contains(characteristics.Get(
                            CameraCharacteristics.RequestAvailableCapabilities).ToArray<int>(),
                            (int)RequestAvailableCapabilities.))
                    {
                        continue;
                    }*/

                    StreamConfigurationMap map = (StreamConfigurationMap)characteristics.Get(
                                                     CameraCharacteristics.ScalerStreamConfigurationMap);

                    // For still image captures, we use the largest available size.
                    Android.Util.Size[] yuvs = map.GetOutputSizes((int)ImageFormatType.Yuv420888);
                    Android.Util.Size largestYuv = yuvs.OrderByDescending(element => element.Width * element.Height).Last();

                    //Android.Util.Size[] raws = map.GetOutputSizes((int)ImageFormatType.RawSensor);
                    //Android.Util.Size largestRaw = raws.OrderByDescending(element => element.Width * element.Height).First();

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

            // If we found no suitable cameras for capturing RAW, warn the user.
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
        /// A wrapper for an {@link AutoCloseable} object that implements reference counting to allow
        /// for resource management.
        /// </summary>
        public class RefCountedAutoCloseable<T> : Java.Lang.Object, Java.Lang.IAutoCloseable where T : Java.Lang.Object
        {
            T mObject;
            long mRefCount = 0;

            /// <summary>
            /// Wrap the given object.
            /// </summary>
            /// <param name="obj">object an object to wrap.</param>
            public RefCountedAutoCloseable(T obj)
            {
                if (obj == null)
                    throw new Java.Lang.NullPointerException();

                mObject = obj;
            }

            /// <summary>
            /// the reference count and return the wrapped object.
            /// </summary>
            /// <returns>the wrapped object, or null if the object has been released.</returns>
            public T GetAndRetain()
            {
                if (mRefCount < 0)
                    return default(T);

                mRefCount++;
                return mObject;
            }

            /// <summary>
            /// Return the wrapped object.
            /// </summary>
            /// <returns>the wrapped object, or null if the object has been released.</returns>
            public T Get()
            {
                return mObject;
            }

            /// <summary>
            /// Decrement the reference count and release the wrapped object if there are no other
            /// users retaining this object.
            /// </summary>
            public void Close()
            {
                if (mRefCount >= 0)
                {
                    mRefCount--;
                    if (mRefCount < 0)
                    {
                        try
                        {
                            var obj = (mObject as Java.Lang.IAutoCloseable);
                            if (obj == null)
                                throw new Java.Lang.Exception("unclosable");
                            obj.Close();
                        }
                        catch (Java.Lang.Exception e)
                        {
                            if (e.Message != "unclosable")
                                throw new Java.Lang.RuntimeException(e);
                        }
                        finally
                        {
                            mObject = default(T);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// {@link CameraDevice.StateCallback} is called when the currently active {@link CameraDevice}
        /// changes its state.
        /// </summary>
        CameraDevice.StateCallback mStateCallback;
        class StateCallback : CameraDevice.StateCallback
        {
            Camera2Activity Activity { get; set; }

            public StateCallback(Camera2Activity activity)
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

        /// <summary>
        /// Number of pending user requests to capture a photo.
        /// </summary>
        static int mPendingUserCaptures = 0;

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
                    mPendingUserCaptures = 0;
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

            this.mOnYuvImageAvailableListener.Activity = this;
            
            var activity = this;
            CameraManager manager = (CameraManager)activity.GetSystemService(Context.CameraService);
            mStateCallback = new StateCallback(this);
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


            this.OnButtonClick  += (sender, args) =>
            {
                CreateCaptureSession();
            };

        }

        public void CreateCaptureSession()
        {
            var surface = mYuvImageReader.Get().Surface;
            
            //surface.DescribeContents()

            mCameraDevice.CreateCaptureSession(new List<Surface>() {surface}, sessionStateCallback, mBackgroundHandler);
        }

        private CameraCaptureSessionCallback sessionStateCallback = new CameraCaptureSessionCallback();

        class CameraCaptureSessionCallback : CameraCaptureSession.StateCallback
        {
            public override void OnConfigured(CameraCaptureSession cameraCaptureSession)
            {
                lock (mCameraStateLock)
                {
                    // The camera is already closed
                    if (null == mCameraDevice)
                        return;

                    try
                    {
                        //Setup3AControlsLocked(mPreviewRequestBuilder);
                        // Finally, we start displaying the camera preview.
                        var captureRequest = CreateCaptureRequest();
                        
                        cameraCaptureSession.SetRepeatingRequest(
                            captureRequest,
                            mPreCaptureCallback, mBackgroundHandler);
                        mState = STATE_PREVIEW;
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
                    // When the session is ready, we start displaying the preview.
                    mCaptureSession = cameraCaptureSession;
                }
            }

            /// <summary>
            /// A {@link CameraCaptureSession } for camera preview.
            /// </summary>
            static CameraCaptureSession mCaptureSession;

            public override void OnConfigureFailed(CameraCaptureSession session)
            {
                Log.Error(TAG, "Failed to configure camera.");
                //ShowToast("Failed to configure camera.");
            }

            private CaptureRequest CreateCaptureRequest()
            {
                try
                {
                    CaptureRequest.Builder builder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
                    builder.AddTarget(mYuvImageReader.Get().Surface);
                    return builder.Build();
                }
                catch (CameraAccessException e)
                {
                    Log.Error(TAG, e.Message);
                    return null;
                }
            }
        }
    }
}

