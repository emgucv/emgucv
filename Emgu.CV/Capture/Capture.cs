//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

//#define TEST_CAPTURE
using System;
using System.ServiceModel;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary> 
   /// Capture images from either camera or video file. 
   /// </summary>
#if (ANDROID || IOS)
#else
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
#endif
   public class Capture :
       UnmanagedObject,
#if (ANDROID || IOS)
#else
 IDuplexCapture,
#endif
 ICapture
   {
      private Thread _captureThread = null;

      /// <summary>
      /// the type of flipping
      /// </summary>
      private CvEnum.FLIP _flipType = Emgu.CV.CvEnum.FLIP.NONE;

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
         /*
         /// <summary>
         /// Capture from file using FFMPEG
         /// </summary>
         FFMPEG,*/
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
      public CvEnum.FLIP FlipType
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
            return (_flipType & Emgu.CV.CvEnum.FLIP.HORIZONTAL) == Emgu.CV.CvEnum.FLIP.HORIZONTAL;
         }
         set
         {
            if (value != FlipHorizontal)
               _flipType ^= Emgu.CV.CvEnum.FLIP.HORIZONTAL;
         }
      }

      /// <summary>
      /// Get or Set if the captured image should be flipped vertically
      /// </summary>
      public bool FlipVertical
      {
         get
         {
            return (_flipType & Emgu.CV.CvEnum.FLIP.VERTICAL) == Emgu.CV.CvEnum.FLIP.VERTICAL;
         }
         set
         {
            if (value != FlipVertical)
               _flipType ^= Emgu.CV.CvEnum.FLIP.VERTICAL;
         }
      }

      ///<summary> The width of this capture</summary>
      public int Width
      {
         get
         {
            return Convert.ToInt32(GetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH));
         }
      }

      ///<summary> The height of this capture </summary>
      public int Height
      {
         get
         {
            return Convert.ToInt32(GetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT));
         }
      }
      #endregion

      #region constructors
      /// <summary>
      /// Create a capture using the specific camera
      /// </summary>
      /// <param name="captureType">The capture type</param>
      public Capture(CvEnum.CaptureType captureType)
         : this((int)captureType)
      {
      }

      ///<summary> Create a capture using the default camera </summary>
      public Capture()
         : this(0)
      {
      }

      ///<summary> Create a capture using the specific camera</summary>
      ///<param name="camIndex"> The index of the camera to create capture from, starting from 0</param>
      public Capture(int camIndex)
      {
         _captureModuleType = CaptureModuleType.Camera;

#if TEST_CAPTURE
#else
         _ptr = CvInvoke.cvCreateCameraCapture(camIndex);
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
      public Capture(String fileName)
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
            _ptr = CvInvoke.cvCreateFileCapture(fileName);
         }

         if (_ptr == IntPtr.Zero)
            throw new NullReferenceException(String.Format("Unable to create capture from {0}", fileName));
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
         CvInvoke.cvReleaseCapture(ref _ptr);
         if (_captureThread != null && _captureThread.IsAlive)
         {
            _captureThread.Abort();
         }
#endif
      }
      #endregion

      /// <summary>
      /// Obtain the capture property
      /// </summary>
      /// <param name="index">The index for the property</param>
      /// <returns>The value of the specific property</returns>
      public double GetCaptureProperty(CvEnum.CAP_PROP index)
      {
         return CvInvoke.cvGetCaptureProperty(_ptr, index);
      }

      /// <summary>
      /// Sets the specified property of video capturing
      /// </summary>
      /// <param name="property">Property identifier</param>
      /// <param name="value">Value of the property</param>
      public void SetCaptureProperty(CvEnum.CAP_PROP property, double value)
      {
         CvInvoke.cvSetCaptureProperty(Ptr, property, value);
      }

      /// <summary>
      /// Grab a frame
      /// </summary>
      /// <returns>True on success</returns>
      public virtual bool Grab()
      {
         bool grabbed = CvInvoke.cvGrabFrame(_ptr);
         if (grabbed && ImageGrabbed != null)
            ImageGrabbed(this, new EventArgs());
         return grabbed;
      }

      #region Grab process
      /// <summary>
      /// The event handler when an image is grabbed
      /// </summary>
      /// <param name="sender">The capture</param>
      /// <param name="e">The event args</param>
      public delegate void GrabEventHandler(object sender, EventArgs e);

      /// <summary>
      /// The event to be called when an image is grabbed
      /// </summary>
      public event GrabEventHandler ImageGrabbed;

      private volatile bool _pauseGrabProcess;

      private void Run()
      {
         while (this.Grab())
         {
            if (_pauseGrabProcess)
               try
               {
                  Thread.Sleep(Timeout.Infinite);
               }
               catch (ThreadInterruptedException)
               {
               }
               catch (ThreadAbortException)
               {
                  break;
               }
         }
      }

      /// <summary>
      /// Start the grab process in a sperate thread. Once started, use the ImageGrabbed event handler and RetrieveGrayFrame/RetrieveBgrFrame to obtain the images.
      /// </summary>
      public void Start()
      {
         _pauseGrabProcess = false;

         if (_captureThread == null || !_captureThread.IsAlive)
         {
            //create a new thread
            ThreadStart ts = new ThreadStart(Run);
            _captureThread = new Thread(ts);
            _captureThread.Start();
         }
         else if (_captureThread.ThreadState == ThreadState.WaitSleepJoin)
         {
            _captureThread.Interrupt();
         }
         else if (_captureThread.ThreadState != ThreadState.Running)
         {
            _captureThread.Start();
         }
      }

      /// <summary>
      /// Pause the grab process if it is running.
      /// </summary>
      public void Pause()
      {
         if (_captureThread != null && _captureThread.IsAlive)
         {
            _pauseGrabProcess = true;
         }
      }

      /// <summary>
      /// Wait for the grabbing thread to complete
      /// </summary>
      public void Wait()
      {
         if (_captureThread != null)
         {
            _captureThread.Join();
         }
      }

      /// <summary>
      /// Stop the grabbing thread
      /// </summary>
      public void Stop()
      {
         if (_captureThread != null)
         {
            _pauseGrabProcess = false;
            _captureThread.Abort();
         }
      }

      /// <summary>
      /// The current state of the grab process
      /// </summary>
      public ThreadState GrabProcessState
      {
         get
         {
            if (_captureThread == null)
               return ThreadState.Unstarted;
            else
               return _captureThread.ThreadState;
         }
      }
      #endregion

      /// <summary> 
      /// Retrieve a Gray image frame after Grab()
      /// </summary>
      /// <returns> A Gray image frame</returns>
      public virtual Image<Gray, Byte> RetrieveGrayFrame()
      {
         return RetrieveGrayFrame(0);
      }

      /// <summary> 
      /// Retrieve a Gray image frame after Grab()
      /// </summary>
      /// <param name="streamIdx">Stream index. Use 0 for default.</param>
      /// <returns> A Gray image frame</returns>
      public virtual Image<Gray, Byte> RetrieveGrayFrame(int streamIdx)
      {
         IntPtr img = CvInvoke.cvRetrieveFrame(Ptr, streamIdx);
         if (img == IntPtr.Zero)
            return null;
         MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(img, typeof(MIplImage));

         Image<Gray, Byte> res;
         if (iplImage.nChannels == 3)
         {  //if the image captured is Bgr, convert it to Grayscale
            res = new Image<Gray, Byte>(iplImage.width, iplImage.height);
            CvInvoke.cvCvtColor(img, res.Ptr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
         }
         else
         {
            res = new Image<Gray, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
         }

         //inplace flip the image if necessary
         res._Flip(FlipType);

         return res;
      }

      /// <summary> 
      /// Retrieve a Bgr image frame after Grab()
      /// </summary>
      /// <returns> A Bgr image frame</returns>
      public virtual Image<Bgr, Byte> RetrieveBgrFrame()
      {
         return RetrieveBgrFrame(0);
      }

      /// <summary> 
      /// Retrieve a Bgr image frame after Grab()
      /// </summary>
      /// <param name="streamIdx">Stream index</param>
      /// <returns> A Bgr image frame</returns>
      public virtual Image<Bgr, Byte> RetrieveBgrFrame(int streamIdx)
      {
         IntPtr img = CvInvoke.cvRetrieveFrame(Ptr, streamIdx);
         if (img == IntPtr.Zero)
            return null;

         MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(img, typeof(MIplImage));

         Image<Bgr, Byte> res;
         if (iplImage.nChannels == 1)
         {  //if the image captured is Grayscale, convert it to BGR
            res = new Image<Bgr, Byte>(iplImage.width, iplImage.height);
            CvInvoke.cvCvtColor(img, res.Ptr, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGR);
         }
         else
         {
            res = new Image<Bgr, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
         }

         //inplace flip the image if necessary
         res._Flip(FlipType);

         return res;
      }

      /// <summary> 
      /// Capture a Gray image frame
      /// </summary>
      /// <returns> A Gray image frame</returns>
      public virtual Image<Gray, Byte> QueryGrayFrame()
      {
         return Grab() ? RetrieveGrayFrame(0) : null;
      }

      #region implement ICapture
      /// <summary> 
      /// Capture a Bgr image frame
      /// </summary>
      /// <returns> A Bgr image frame</returns>
      public virtual Image<Bgr, Byte> QueryFrame()
      {
         return Grab() ? RetrieveBgrFrame(0) : null;
      }

      ///<summary> 
      /// Capture a Bgr image frame that is half width and half height. 
      /// Mainly used by WCF when sending image to remote locations in a bandwidth conservative scenario 
      ///</summary>
      ///<remarks>Internally, this is a cvQueryFrame operation follow by a cvPyrDown</remarks>
      ///<returns> A Bgr image frame that is half width and half height</returns>
      public virtual Image<Bgr, Byte> QuerySmallFrame()
      {
         using (Image<Bgr, Byte> frame = QueryFrame())
            return frame == null ? null : frame.PyrDown();
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

            if (FlipType == Emgu.CV.CvEnum.FLIP.NONE)
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

#if (ANDROID || IOS)
#else
      /// <summary>
      /// Query a frame duplexly over WCF
      /// </summary>
      public virtual void DuplexQueryFrame()
      {
         IDuplexCaptureCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexCaptureCallback>();

         Image<Bgr, Byte> img = QueryFrame();
         callback.ReceiveFrame(img);
      }

      /// <summary>
      /// Query a small frame duplexly over WCF
      /// </summary>
      public virtual void DuplexQuerySmallFrame()
      {
         IDuplexCaptureCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexCaptureCallback>();

         Image<Bgr, Byte> img = QuerySmallFrame();
         callback.ReceiveFrame(img);
      }
#endif
   }
}
