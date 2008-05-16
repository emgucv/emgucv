//#define TEST_CAPTURE
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Emgu;

namespace Emgu.CV
{
    ///<summary> Create a image capture base on opencv's capture object </summary>
#if NET_2_0
#else
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
#endif
    public class Capture : 
        UnmanagedObject,
#if NET_2_0
#else
        IDuplexCapture,
#endif
        ICapture
    {
        /// <summary>
        /// the width of this capture
        /// </summary>
        private int _width = 0;
        /// <summary>
        /// the height of this capture
        /// </summary>
        private int _height = 0;

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
        /// <param name="filename">The file name of the movie</param>
        public Capture(String filename)
        {
            _ptr = CvInvoke.cvCreateFileCapture(filename);
            if (_ptr == IntPtr.Zero)
            {
                throw new Emgu.PrioritizedException(Emgu.ExceptionLevel.Critical, "Unable to create capture from file:" + filename);
            }
        }

        /// <summary>
        /// Release the resource for this capture
        /// </summary>
        protected override void FreeUnmanagedObjects()
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

        ///<summary> Capture a RGB image frame</summary>
        ///<returns> A RGB image frame</returns>
        public virtual Image<Bgr, Byte> QueryFrame()
        {
#if TEST_CAPTURE
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(1024, 800, new Bgr());
            img.Draw(System.DateTime.Now.Ticks.ToString(),
                new Font( CvEnum.FONT.CV_FONT_HERSHEY_PLAIN, 1.0, 1.0),
                new Point2D<int>(10, 50),
                new Bgr(255.0, 255.0, 255.0));
            return img;
#else

            IntPtr img = CvInvoke.cvQueryFrame(_ptr);
            Image<Bgr, Byte> res = new Image<Bgr, Byte>(Width, Height);
            CvInvoke.cvCopy(img, res.Ptr, IntPtr.Zero);
            return res;
#endif
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
            return res;
#endif
        }

#if NET_2_0
#else
        /// <summary>
        /// Query a frame duplexly over WCF
        /// </summary>
        public virtual void DuplexQueryFrame()
        {
            IDuplexCaptureCallback callback = OperationContext.Current.GetCallbackChannel<IDuplexCaptureCallback>();

            using (Image<Bgr, Byte> img = QueryFrame())
            {
                //try
                {
                    callback.ReceiveFrame(img);
                }
 /*             catch (System.Exception)
                {
                } 
 */
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
                //try
                {
                    callback.ReceiveFrame(img);
                }
                /*
                catch (System.Exception)
                {
                }*/
            }
        }
#endif

        ///<summary> Capture Bgr image frame with timestamp</summary>
        ///<returns> A timestamped Bgr image frame</returns>
        public TimedImage<Bgr, Byte> QueryTimedFrame()
        {
            IntPtr img = CvInvoke.cvQueryFrame(_ptr);
            TimedImage<Bgr, Byte> res = new TimedImage<Bgr, Byte>(Width, Height);
            CvInvoke.cvCopy(img, res.Ptr, IntPtr.Zero);
            res.Timestamp = System.DateTime.Now;
            return res;
        }
    };

}
