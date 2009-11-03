using System;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A Blob Tracker
   /// </summary>
   public class BlobTracker : BlobSeqBase
   {
      /// <summary>
      /// Create a blob trakcer of the specific type
      /// </summary>
      /// <param name="type">The type of the blob tracker</param>
      public BlobTracker(CvEnum.BLOBTRACKER_TYPE type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.CC:
               _ptr = CvInvoke.CvCreateBlobTrackerCC();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.CCMSPF:
               _ptr = CvInvoke.CvCreateBlobTrackerCCMSPF();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MS:
               _ptr = CvInvoke.CvCreateBlobTrackerMS();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFG:
               _ptr = CvInvoke.CvCreateBlobTrackerMSFG();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSFGS:
               _ptr = CvInvoke.CvCreateBlobTrackerMSFGS();
               break;
            case Emgu.CV.CvEnum.BLOBTRACKER_TYPE.MSPF:
               _ptr = CvInvoke.CvCreateBlobTrackerMSPF();
               break;
         }
      }

      /// <summary>
      /// Add new blob to track it and assign to this blob personal ID
      /// </summary>
      /// <param name="blob">Structure with blob parameters (ID is ignored)</param>
      /// <param name="currentImage">current image</param>
      /// <param name="currentForgroundMask">Current foreground mask</param>
      /// <returns>Newly added blob</returns>
      public MCvBlob Add(MCvBlob blob, IImage currentImage, Image<Gray, Byte> currentForgroundMask)
      {
         IntPtr bobPtr = CvInvoke.CvBlobTrackerAddBlob(
            _ptr, 
            ref blob, 
            currentImage == null ? IntPtr.Zero : currentImage.Ptr, 
            currentForgroundMask);
         return (MCvBlob)Marshal.PtrToStructure(bobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Delete blob by its index
      /// </summary>
      /// <param name="blobIndex">The index of the blob</param>
      public void RemoveAt(int blobIndex)
      {
         CvInvoke.CvBlobTrackerDelBlob(_ptr, blobIndex);
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
            return (MCvBlob)Marshal.PtrToStructure(CvInvoke.CvBlobTrackerGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }

      /// <summary>
      /// Get the blob with the specific id
      /// </summary>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>The blob of the specific id, if it doesn't exist, null is returned</returns>
      public override MCvBlob? GetBlobByID(int blobID)
      {
         IntPtr blobPtr = CvInvoke.CvBlobTrackerGetBlobByID(_ptr, blobID);
         if (blobPtr == IntPtr.Zero) return null;
         return (MCvBlob?)Marshal.PtrToStructure(blobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Get the number of blobs in this tracker
      /// </summary>
      public override int Count
      {
         get
         {
            return CvInvoke.CvBlobTrackerGetBlobNum(_ptr);
         }
      }

      /// <summary>
      /// Release the blob trakcer
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobTrackerRealease(_ptr);
      }
      #endregion

   }
}
