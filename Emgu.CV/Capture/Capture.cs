//#define TEST_CAPTURE
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.InteropServices;
using Emgu;

namespace Emgu.CV
{
    ///<summary> Create a image capture base on opencv's capture object </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Capture : 
        UnmanagedObject,
        IDuplexCapture,
        ICapture
    {
        //private Type CaptureDepthType;

        /// <summary>
        /// the width of this capture
        /// </summary>
        private int _width;
        /// <summary>
        /// the height of this capture
        /// </summary>
        private int _height;

        /// <summary>
        /// the type of flipping
        /// </summary>
        private CvEnum.FLIP _flipType = Emgu.CV.CvEnum.FLIP.NONE;

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

        ///<summary> Create a capture using the default camera </summary>
        public Capture()
            : this(-1)
        {
        }

        ///<summary> Create a capture using the specific camera</summary>
        ///<param name="camIndex"> The index of the camera to create capture from, starting from 0</param>
        public Capture(int camIndex)
        {
#if TEST_CAPTURE
#else
            _ptr = CvInvoke.cvCreateCameraCapture(camIndex);
            if (_ptr == IntPtr.Zero)
            {
                throw new Emgu.PrioritizedException(Emgu.ExceptionLevel.Medium, "Error: Unable to connect to camera");
            }
#endif
        }

        /// <summary>
        /// Create a capture from file
        /// </summary>
        /// <param name="fileName">The file name of the movie</param>
        public Capture(String fileName)
        {
            _ptr = CvInvoke.cvCreateFileCapture(fileName);
            if (_ptr == IntPtr.Zero)
            {
                throw new Emgu.PrioritizedException(Emgu.ExceptionLevel.Critical, "Unable to create capture from file:" + fileName);
            }
        }

        /// <summary>
        /// Release the resource for this capture
        /// </summary>
        protected override void DisposeObject()
        {
#if TEST_CAPTURE
#else
            CvInvoke.cvReleaseCapture(ref _ptr);
#endif
        }

        ///<summary> The width of this capture</summary>
        public int Width 
        { 
            get 
            { 
                if (_width == 0) _width = System.Convert.ToInt32(GetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH));
                return _width;
            } 
        }
        
        ///<summary> The height of this capture </summary>
        public int Height 
        { 
            get 
            {
                if (_height == 0) _height = System.Convert.ToInt32(GetCaptureProperty(CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT));
                return _height;
            } 
        }

        /// <summary>
        /// Obtain the capture property
        /// </summary>
        /// <param name="index">The index for the property</param>
        /// <returns>The value of the specific property</returns>
        public double GetCaptureProperty(CvEnum.CAP_PROP index)
        {
            return CvInvoke.cvGetCaptureProperty(_ptr, index);
        }

        /*
        private Type GetImageDepthType(IntPtr img)
        {
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(img, typeof(MIplImage));
            //if (iplImage.depth == 
        }*/

        #region implement ICapture
        ///<summary> Capture a RGB image frame</summary>
        ///<returns> A RGB image frame</returns>
        public virtual Image<Bgr, Byte> QueryFrame()
        {
#if TEST_CAPTURE
            Image<Bgr, Byte> tmp = new Image<Bgr, Byte>(320, 240, new Bgr());
            tmp.Draw(System.DateTime.Now.Ticks.ToString(),
                new Font( CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0, 1.0),
                new Point2D<int>(10, 50),
                new Bgr(255.0, 255.0, 255.0));
            IntPtr img = tmp;
            Image<Bgr, Byte> res = tmp.BlankClone();
#else
            IntPtr img = CvInvoke.cvQueryFrame(_ptr);

            Image<Bgr, Byte> res = new Image<Bgr, Byte>(Width, Height);
#endif
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
        }

        ///<summary> 
        ///Capture a RGB image frame that is half width and half heigh. 
        ///Internally, this is a cvQueryFrame operation follow by a cvPyrDown
        ///</summary>
        ///<returns> A RGB image frame that is half width and half height</returns>
        public virtual Image<Bgr, Byte> QuerySmallFrame()
        {
#if TEST_CAPTURE
            return QueryFrame().PyrDown();
#else
            IntPtr img = CvInvoke.cvQueryFrame(_ptr);
            Image<Bgr, Byte> res = new Image<Bgr, Byte>(Width >> 1, Height >> 1);
            CvInvoke.cvPyrDown(img, res.Ptr, CvEnum.FILTER_TYPE.CV_GAUSSIAN_5x5);

            if (FlipType == Emgu.CV.CvEnum.FLIP.NONE)
            {
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
                CvInvoke.cvFlip(res.Ptr, res.Ptr, code);
                return res;
            }
#endif
        }
        #endregion 

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
        }

        /// <summary>
        /// Query a frame duplexly over WCF
        /// </summary>
        public virtual void DuplexQueryFrame()
        {
            IDuplexCaptureCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexCaptureCallback>();

            using (Image<Bgr, Byte> img = QueryFrame())
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

            using (Image<Bgr, Byte> img = QuerySmallFrame())
            {
                callback.ReceiveFrame(img);
            }
        }

    };

}
