//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__ANDROID__ || __UNIFIED__ || NETFX_CORE || UNITY_WSA || NETSTANDARD || UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR || UNITY_STANDALONE)
#define WITH_SERVICE_MODEL
#endif

//#define TEST_CAPTURE

using System;
#if WITH_SERVICE_MODEL
using System.ServiceModel;
#endif
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
//using System.Threading.Tasks;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
    /// <summary> 
    /// Capture images from either camera or video file. 
    /// </summary>
    /// <remarks>VideoCapture class is NOT implemented in Open CV for Android, iOS or UWP platforms</remarks>
#if WITH_SERVICE_MODEL
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
#endif
    public partial class VideoCapture :
        UnmanagedObject,
#if WITH_SERVICE_MODEL
        IDuplexCapture,
#endif
        ICapture
    {

        /// <summary>
        /// VideoCapture API backends identifier.
        /// </summary>
        public enum API
        {
            /// <summary>
            /// Auto detect
            /// </summary>
            Any = 0,

            /// <summary>
            /// Video For Windows (obsolete, removed)
            /// </summary>
            Vfw = 200,
            /// <summary>
            /// V4L/V4L2 capturing support
            /// </summary>
            V4L = 200,
            /// <summary>
            /// Same as CAP_V4L
            /// </summary>
            V4L2 = V4L,

            /// <summary>
            /// IEEE 1394 drivers
            /// </summary>
            Firewire = 300,
            /// <summary>
            /// IEEE 1394 drivers
            /// </summary>
            IEEE1394 = Firewire,
            /// <summary>
            /// IEEE 1394 drivers
            /// </summary>
            DC1394 = Firewire,
            /// <summary>
            /// IEEE 1394 drivers
            /// </summary>
            CMU1394 = Firewire,

            /// <summary>
            ///  QuickTime (obsolete, removed)
            /// </summary>
            QT = 500,

            /// <summary>
            /// Unicap drivers (obsolete, removed)
            /// </summary>
            Unicap = 600,

            /// <summary>
            /// DirectShow (via videoInput)
            /// </summary>
            DShow = 700,

            /// <summary>
            /// PvAPI, Prosilica GigE SDK
            /// </summary>
            Pvapi = 800,

            /// <summary>
            /// OpenNI (for Kinect)
            /// </summary>
            OpenNI = 900,

            /// <summary>
            /// OpenNI (for Asus Xtion)
            /// </summary>
            OpenNIAsus = 910,

            /// <summary>
            /// Android - not used
            /// </summary>
            Android = 1000,

            /// <summary>
            /// XIMEA Camera API
            /// </summary>
            XiApi = 1100,

            /// <summary>
            /// AVFoundation framework for iOS (OS X Lion will have the same API)
            /// </summary>
            AVFoundation = 1200,

            /// <summary>
            ///  Smartek Giganetix GigEVisionSDK
            /// </summary>
            Giganetix = 1300,

            /// <summary>
            /// Microsoft Media Foundation (via videoInput)
            /// </summary>
            Msmf = 1400,

            /// <summary>
            /// Microsoft Windows Runtime using Media Foundation
            /// </summary>
            Winrt = 1410,
            /// <summary>
            /// Intel Perceptual Computing SDK
            /// </summary>
            IntelPerc = 1500,
            /// <summary>
            /// OpenNI2 (for Kinect)
            /// </summary>
            Openni2 = 1600,
            /// <summary>
            /// OpenNI2 (for Asus Xtion and Occipital Structure sensors)
            /// </summary>
            Openni2Asus = 1610,
            /// <summary>
            /// gPhoto2 connection
            /// </summary>
            Gphoto2 = 1700,
            /// <summary>
            /// GStreamer
            /// </summary>
            Gstreamer = 1800,
            /// <summary>
            /// Open and record video file or stream using the FFMPEG library
            /// </summary>
            Ffmpeg = 1900,
            /// <summary>
            /// OpenCV Image Sequence (e.g. img_%02d.jpg)
            /// </summary>
            Images = 2000,
            /// <summary>
            /// Aravis SDK
            /// </summary>
            Aravis = 2100,
            /// <summary>
            /// Built-in OpenCV MotionJPEG codec
            /// </summary>
            OpencvMjpeg = 2200,
            /// <summary>
            /// Intel MediaSDK
            /// </summary>
            IntelMfx = 2300,
            /// <summary>
            /// XINE engine (Linux)
            /// </summary>
            Xine = 2400,
        }

        AutoResetEvent _pauseEvent = new AutoResetEvent(false);

        /// <summary>
        /// the type of flipping
        /// </summary>
        private CvEnum.FlipType _flipType = Emgu.CV.CvEnum.FlipType.None;

        /// <summary>
        /// The type of capture source
        /// </summary>
        public enum CaptureModuleType
        {
            /// <summary>
            /// Capture from camera
            /// </summary>
            Camera,
            /// <summary>
            /// Capture from file using HighGUI
            /// </summary>
            Highgui,

        }

        private CaptureModuleType _captureModuleType;

        #region Properties
        /// <summary>
        /// Get the type of the capture module
        /// </summary>
        public CaptureModuleType CaptureSource
        {
            get
            {
                return _captureModuleType;
            }
        }

        /// <summary>
        /// Get and set the flip type
        /// </summary>
        public CvEnum.FlipType FlipType
        {
            get
            {
                return _flipType;
            }
            set
            {
                _flipType = value;
            }
        }

        /// <summary>
        /// Get or Set if the captured image should be flipped horizontally
        /// </summary>
        public bool FlipHorizontal
        {
            get
            {
                return (_flipType & Emgu.CV.CvEnum.FlipType.Horizontal) == Emgu.CV.CvEnum.FlipType.Horizontal;
            }
            set
            {
                if (value != FlipHorizontal)
                    _flipType ^= Emgu.CV.CvEnum.FlipType.Horizontal;
            }
        }

        /// <summary>
        /// Get or Set if the captured image should be flipped vertically
        /// </summary>
        public bool FlipVertical
        {
            get
            {
                return (_flipType & Emgu.CV.CvEnum.FlipType.Vertical) == Emgu.CV.CvEnum.FlipType.Vertical;
            }
            set
            {
                if (value != FlipVertical)
                    _flipType ^= Emgu.CV.CvEnum.FlipType.Vertical;
            }
        }

        /// <summary> The width of this capture</summary>
        public int Width
        {
            get
            {
                return Convert.ToInt32(GetCaptureProperty(CvEnum.CapProp.FrameWidth));
            }
        }

        /// <summary> The height of this capture </summary>
        public int Height
        {
            get
            {
                return Convert.ToInt32(GetCaptureProperty(CvEnum.CapProp.FrameHeight));
            }
        }
        #endregion

        #region constructors
        /*
        /// <summary>
        /// Create a capture using the specific camera
        /// </summary>
        /// <param name="captureType">The capture type</param>
        public VideoCapture(API captureType)
           : this((int)captureType)
        {
        }*/

        /// <summary> Create a capture using the specific camera</summary>
        /// <param name="camIndex"> The index of the camera to create capture from, starting from 0</param>
        /// <param name="captureApi">The preferred Capture API backends to use. Can be used to enforce a specific reader implementation if multiple are available.</param>
        public VideoCapture(int camIndex = 0, API captureApi = API.Any)
        {
            _captureModuleType = CaptureModuleType.Camera;

#if TEST_CAPTURE
#else
            _ptr = CvInvoke.cveVideoCaptureCreateFromDevice(camIndex, captureApi);
            if (_ptr == IntPtr.Zero)
            {
                throw new NullReferenceException(String.Format("Error: Unable to create capture from camera {0}", camIndex));
            }
#endif
        }

        /// <summary>
        /// Create a capture from file or a video stream
        /// </summary>
        /// <param name="fileName">The name of a file, or an url pointed to a stream.</param>
        /// <param name="captureApi">The preferred Capture API backends to use. Can be used to enforce a specific reader implementation if multiple are available.</param>
        public VideoCapture(String fileName, API captureApi = API.Any)
        {
            using (CvString s = new CvString(fileName))
            {
                /*
                if (Util.CvToolbox.HasFFMPEG)
                {
                   _captureModuleType = CaptureModuleType.FFMPEG;
                   _ptr = CvInvoke.cvCreateFileCapture_FFMPEG(fileName);
                }
                else*/
                {
                    _captureModuleType = CaptureModuleType.Highgui;
                    _ptr = CvInvoke.cveVideoCaptureCreateFromFile(s, captureApi);
                }

                if (_ptr == IntPtr.Zero)
                    throw new NullReferenceException(String.Format("Unable to create capture from {0}", fileName));
            }
        }
        #endregion

        #region implement UnmanagedObject
        /// <summary>
        /// Release the resource for this capture
        /// </summary>
        protected override void DisposeObject()
        {
#if TEST_CAPTURE
#else
            Stop();
            CvInvoke.cveVideoCaptureRelease(ref _ptr);

#endif
        }
        #endregion

        /// <summary>
        /// Obtain the capture property
        /// </summary>
        /// <param name="index">The index for the property</param>
        /// <returns>The value of the specific property</returns>
        public double GetCaptureProperty(CvEnum.CapProp index)
        {
            return CvInvoke.cveVideoCaptureGet(_ptr, index);
        }

        /// <summary>
        /// Sets the specified property of video capturing
        /// </summary>
        /// <param name="property">Property identifier</param>
        /// <param name="value">Value of the property</param>
        /// <returns>True if success</returns>
        public bool SetCaptureProperty(CvEnum.CapProp property, double value)
        {
            return CvInvoke.cveVideoCaptureSet(Ptr, property, value);
        }

        /// <summary>
        /// Grab a frame
        /// </summary>
        /// <returns>True on success</returns>
        public virtual bool Grab()
        {
            if (_ptr == IntPtr.Zero)
                return false;

            bool grabbed = CvInvoke.cveVideoCaptureGrab(Ptr);

            if (grabbed && ImageGrabbed != null)
                ImageGrabbed(this, new EventArgs());
            return grabbed;
        }

        #region Grab process
        /// <summary>
        /// The event to be called when an image is grabbed
        /// </summary>
        public event EventHandler ImageGrabbed;

        private enum GrabState
        {
            Stopped,
            Running,
            Pause,
            Stopping,
        }

        private volatile GrabState _grabState = GrabState.Stopped;

        private void Run(
#if WITH_SERVICE_MODEL
                System.ServiceModel.Dispatcher.ExceptionHandler eh = null
#endif
            )
        {
            try
            {
                while (_grabState == GrabState.Running || _grabState == GrabState.Pause)
                {
                    if (_grabState == GrabState.Pause)
                    {
                        _pauseEvent.WaitOne();
                    }
                    else if (IntPtr.Zero.Equals(_ptr) || !Grab())
                    {
                        //capture has been released, or
                        //no more frames to grab, this is the end of the stream. 
                        //We should stop.
                        _grabState = GrabState.Stopping;
                    }
                }
            }
            catch (Exception e)
            {
#if WITH_SERVICE_MODEL
                if (eh != null && eh.HandleException(e))
                        return;
#endif
                throw new Exception("Capture error", e);
            }
            finally
            {
                _grabState = GrabState.Stopped;
            }
        }

        private static void Wait(int millisecond)
        {
            //Task t = Task.Delay(millisecond);
            //t.Wait();
            Thread.Sleep(millisecond);
        }


#if WITH_SERVICE_MODEL
        /// <summary>
        /// Start the grab process in a separate thread. Once started, use the ImageGrabbed event handler and RetrieveGrayFrame/RetrieveBgrFrame to obtain the images.
        /// </summary>
        /// <param name="eh">An exception handler. If provided, it will be used to handle exception in the capture thread.</param>
        public void Start(System.ServiceModel.Dispatcher.ExceptionHandler eh = null)
#else
        /// <summary>
        /// Start the grab process in a separate thread. Once started, use the ImageGrabbed event handler and RetrieveGrayFrame/RetrieveBgrFrame to obtain the images.
        /// </summary>
        public void Start()
#endif
        {
            if (_grabState == GrabState.Pause)
            {
                _grabState = GrabState.Running;
                _pauseEvent.Set();

            }
            else if (_grabState == GrabState.Stopped || _grabState == GrabState.Stopping)
            {
                _grabState = GrabState.Running;

//              Task t = new Task(Run);
//              t.Start();

#if WITH_SERVICE_MODEL
                ThreadPool.QueueUserWorkItem(delegate { Run(eh); });
#else
                ThreadPool.QueueUserWorkItem(delegate { Run(); });
#endif
            }
        }

        /// <summary>
        /// Pause the grab process if it is running.
        /// </summary>
        public void Pause()
        {
            if (_grabState == GrabState.Running)
                _grabState = GrabState.Pause;
        }

        /// <summary>
        /// Stop the grabbing thread
        /// </summary>
        public void Stop()
        {
            if (_grabState == GrabState.Pause)
            {
                _grabState = GrabState.Stopping;
                _pauseEvent.Set();
            }
            else
               if (_grabState == GrabState.Running)
                _grabState = GrabState.Stopping;
        }
        #endregion

        /// <summary> 
        /// Decodes and returns the grabbed video frame.
        /// </summary>
        /// <param name="image">The video frame is returned here. If no frames has been grabbed the image will be empty.</param>
        /// <param name="flag">It could be a frame index or a driver specific flag</param>
        /// <returns>False if no frames has been grabbed</returns>
        public virtual bool Retrieve(IOutputArray image, int flag = 0)
        {
            using (OutputArray oaImage = image.GetOutputArray())
            {
                if (FlipType == CvEnum.FlipType.None)
                {
                    return CvInvoke.cveVideoCaptureRetrieve(Ptr, oaImage, flag);
                }
                else
                {
                    bool success = CvInvoke.cveVideoCaptureRetrieve(Ptr, oaImage, flag);
                    if (success)
                        CvInvoke.Flip(image, image, FlipType);
                    return success;
                }
            }
        }

        /// <summary>
        /// Similar to the C++ implementation of cv::Capture &gt;&gt; Mat. It first call Grab() function follows by Retrieve()
        /// </summary>
        /// <param name="m">The matrix the image will be read into.</param>
        public void Read(Mat m)
        {
            CvInvoke.cveVideoCaptureReadToMat(Ptr, m);
        }

        /// <summary>
        /// The name of the backend used by this VideoCapture
        /// </summary>
        public String BackendName
        {
            get
            {
                using (CvString s = new CvString())
                {
                    CvInvoke.cveVideoCaptureGetBackendName(Ptr, s);
                    return s.ToString();
                }
            }
        }

        #region implement ICapture
        /// <summary> 
        /// Capture a Bgr image frame
        /// </summary>
        /// <returns> A Bgr image frame. If no more frames are available, null will be returned.</returns>
        public virtual Mat QueryFrame()
        {
            if (Grab())
            {
                Mat image = new Mat();
                Retrieve(image);
                return image;
            }
            else
            {
                return null;
            }
        }

        ///<summary> 
        /// Capture a Bgr image frame that is half width and half height. 
        /// Mainly used by WCF when sending image to remote locations in a bandwidth conservative scenario 
        ///</summary>
        ///<remarks>Internally, this is a cvQueryFrame operation follow by a cvPyrDown</remarks>
        ///<returns> A Bgr image frame that is half width and half height</returns>
        public virtual Mat QuerySmallFrame()
        {
            Mat tmp = QueryFrame();

            if (tmp != null)
            {
                if (!tmp.IsEmpty)
                {
                    CvInvoke.PyrDown(tmp, tmp);
                    return tmp;
                }
                else
                {
                    tmp.Dispose();
                }
            }
            return null;

        }
        #endregion

        /*
          ///<summary> Capture Bgr image frame with timestamp</summary>
          ///<returns> A timestamped Bgr image frame</returns>
          public TimedImage<Bgr, Byte> QueryTimedFrame()
          {
              IntPtr img = CvInvoke.cvQueryFrame(_ptr);
              TimedImage<Bgr, Byte> res = new TimedImage<Bgr, Byte>(Width, Height);

              res.Timestamp = System.DateTime.Now;

              if (FlipType == Emgu.CV.CvEnum.FLIP.None)
              {
                  CvInvoke.cvCopy(img, res.Ptr, IntPtr.Zero);
                  return res;
              }
              else
              {
                  //code = 0 indicates vertical flip only
                  int code = 0;
                  //code = -1 indicates vertical and horizontal flip
                  if (FlipType == (Emgu.CV.CvEnum.FLIP.HORIZONTAL | Emgu.CV.CvEnum.FLIP.VERTICAL)) code = -1;
                  //code = 1 indicates horizontal flip only
                  else if (FlipType == Emgu.CV.CvEnum.FLIP.HORIZONTAL) code = 1;
                  CvInvoke.cvFlip(img, res.Ptr, code);
                  return res;
              }
          }*/

#if WITH_SERVICE_MODEL
        /// <summary>
        /// Query a frame duplexly over WCF
        /// </summary>
        public virtual void DuplexQueryFrame()
        {
            IDuplexCaptureCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexCaptureCallback>();
            using (Mat img = QueryFrame())
            {
                callback.ReceiveFrame(img);
            }
        }

        /// <summary>
        /// Query a small frame duplexly over WCF
        /// </summary>
        public virtual void DuplexQuerySmallFrame()
        {
            IDuplexCaptureCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexCaptureCallback>();
            using (Mat img = QuerySmallFrame())
            {
                callback.ReceiveFrame(img);
            }
        }
#endif
    }

    /// <summary>
    /// The backend for video
    /// </summary>
    public class Backend
    {
        private int _id;

        /// <summary>
        /// Create a backend given its id
        /// </summary>
        /// <param name="id">The id of the backend</param>
        public Backend(int id)
        {
            _id = id;
        }

        /// <summary>
        /// The ID of the backend.
        /// </summary>
        public int ID
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// The name of the backend
        /// </summary>
        public String Name
        {
            get
            {
                using (CvString cvs = new CvString())
                {
                    CvInvoke.cveGetBackendName(_id, cvs);
                    return cvs.ToString();
                }
            }
        }
    }

    partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVideoCaptureReadToMat(IntPtr capture, IntPtr mat);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVideoCaptureCreateFromDevice(int index, VideoCapture.API apiPreference);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVideoCaptureCreateFromFile(IntPtr filename, VideoCapture.API api);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVideoCaptureRelease(ref IntPtr capture);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
        internal static extern bool cveVideoCaptureRead(IntPtr capture, IntPtr frame);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
        internal static extern bool cveVideoCaptureGrab(IntPtr capture);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
        internal static extern bool cveVideoCaptureRetrieve(IntPtr capture, IntPtr image, int flag);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveVideoCaptureGet(IntPtr capture, CvEnum.CapProp prop);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
        internal static extern bool cveVideoCaptureSet(IntPtr capture, CvEnum.CapProp propertyId, double value);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVideoCaptureGetBackendName(IntPtr capture, IntPtr backendName);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGetBackendName(int api, IntPtr name);

        /// <summary>
        /// Returns list of all built-in backends
        /// </summary>
        public static Backend[] Backends
        {
            get
            {
                using (VectorOfInt vi = new VectorOfInt())
                {
                    cveGetBackends(vi);
                    int[] ids = vi.ToArray();
                    Backend[] backends = new Backend[ids.Length];
                    for (int i = 0; i < ids.Length; i++)
                    {
                        backends[i] = new Backend(ids[i]);
                    }

                    return backends;
                }
            }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGetBackends(IntPtr backends);

        /// <summary>
        /// Returns list of available backends which works via cv::VideoCapture(int index)
        /// </summary>
        public static Backend[] CameraBackends
        {
            get
            {
                using (VectorOfInt vi = new VectorOfInt())
                {
                    cveGetCameraBackends(vi);
                    int[] ids = vi.ToArray();
                    Backend[] backends = new Backend[ids.Length];
                    for (int i = 0; i < ids.Length; i++)
                    {
                        backends[i] = new Backend(ids[i]);
                    }

                    return backends;
                }
            }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGetCameraBackends(IntPtr backends);

        /// <summary>
        /// Returns list of available backends which works via cv::VideoCapture(filename)
        /// </summary>
        public static Backend[] StreamBackends
        {
            get
            {
                using (VectorOfInt vi = new VectorOfInt())
                {
                    cveGetStreamBackends(vi);
                    int[] ids = vi.ToArray();
                    Backend[] backends = new Backend[ids.Length];
                    for (int i = 0; i < ids.Length; i++)
                    {
                        backends[i] = new Backend(ids[i]);
                    }

                    return backends;
                }
            }
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGetStreamBackends(IntPtr backends);

        /// <summary>
        /// Returns list of available backends which works via cv::VideoWriter()
        /// </summary>
        public static Backend[] WriterBackends
        {
            get
            {
                using (VectorOfInt vi = new VectorOfInt())
                {
                    cveGetWriterBackends(vi);
                    int[] ids = vi.ToArray();
                    Backend[] backends = new Backend[ids.Length];
                    for (int i = 0; i < ids.Length; i++)
                    {
                        backends[i] = new Backend(ids[i]);
                    }

                    return backends;
                }
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGetWriterBackends(IntPtr backends);
    }
}
