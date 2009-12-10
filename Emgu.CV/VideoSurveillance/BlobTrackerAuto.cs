using System;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A blob tracker auto
   /// </summary>
   /// <typeparam name="TColor">The type of color for the image to be tracked.</typeparam>
   public class BlobTrackerAuto<TColor> : BlobSeqBase
      where TColor : struct, IColor
   {
      private BlobTrackerAutoParam<TColor> _param;

      /// <summary>
      /// Create a auto blob tracker using the specific parameters
      /// </summary>
      /// <param name="param">The parameters for this blob tracker auto</param>
      public BlobTrackerAuto(BlobTrackerAutoParam<TColor> param)
      {
         _param = param;
         MCvBlobTrackerAutoParam1 p = _param.MCvBlobTrackerAutoParam1;
         _ptr = CvInvoke.CvCreateBlobTrackerAuto1(ref p);
      }

      /// <summary>
      /// Create a default auto blob tracker 
      /// </summary>
      public BlobTrackerAuto()
      {
         BlobTrackerAutoParam<TColor> param = new BlobTrackerAutoParam<TColor>();
         param.FGDetector = new FGDetector<TColor>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
         _param = param;
         MCvBlobTrackerAutoParam1 p = _param.MCvBlobTrackerAutoParam1;
         _ptr = CvInvoke.CvCreateBlobTrackerAuto1(ref p);
      }

      /// <summary>
      /// Process a frame
      /// </summary>
      /// <param name="currentFrame">The frame to be processed</param>
      public void Process(Image<TColor, Byte> currentFrame)
      {
         Process(currentFrame, null);
      }

      /// <summary>
      /// Process a frame
      /// </summary>
      /// <param name="currentFrame">The frame to be processed</param>
      /// <param name="forgroundMask">the forground mask to be used</param>
      public void Process(Image<TColor, Byte> currentFrame, Image<Gray, Byte> forgroundMask)
      {
         CvInvoke.CvBlobTrackerAutoProcess(_ptr, currentFrame.Ptr, forgroundMask == null ? IntPtr.Zero : forgroundMask.Ptr);
      }

      /// <summary>
      /// Get the forground mask
      /// </summary>
      /// <returns>The forground mask</returns>
      public Image<Gray, Byte> ForgroundMask
      {
         get
         {
            IntPtr forground = CvInvoke.CvBlobTrackerAutoGetFGMask(_ptr);
            if (forground == IntPtr.Zero) return null;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(forground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.width, iplImage.height, iplImage.widthStep, iplImage.imageData);
         }
      }

      /// <summary>
      /// The parameters for this blob tracker auto
      /// </summary>
      public BlobTrackerAutoParam<TColor> Param
      {
         get
         {
            return _param;
         }
         //set
         //{
         //   _param = value;
         //}
      }

      #region BolbSeqBase Members
      /// <summary>
      /// Return the blob given the specific index
      /// </summary>
      /// <param name="i">The index of the blob</param>
      /// <returns>The blob in the specific index</returns>
      public override MCvBlob this[int i]
      {
         get
         {
            return (MCvBlob)Marshal.PtrToStructure(CvInvoke.CvBlobTrackerAutoGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }

      /// <summary>
      /// Get the blob with the specific id
      /// </summary>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>The blob of the specific id, if it doesn't exist, null is returned</returns>
      public override MCvBlob? GetBlobByID(int blobID)
      {
         IntPtr blobPtr = CvInvoke.CvBlobTrackerAutoGetBlobByID(_ptr, blobID);
         if (blobPtr == IntPtr.Zero) return null;
         return (MCvBlob?)Marshal.PtrToStructure(blobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Release the blob tracker auto
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobTrackerAutoRelease(_ptr);
      }

      /// <summary>
      /// Get the number of blobs in this tracker
      /// </summary>
      public override int Count
      {
         get
         {
            return CvInvoke.CvBlobTrackerAutoGetBlobNum(_ptr);
         }
      }
      #endregion
   }
}
