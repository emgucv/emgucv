using System;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// a Blob Seq
   /// </summary>
   public class BlobSeq : BlobSeqBase
   {
      /// <summary>
      /// Create a BlobSeq from the given pointer
      /// </summary>
      /// <param name="ptr">The pointer to the unmanaged BlobSeq</param>
      public BlobSeq(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Create a empty BlobSeq
      /// </summary>
      public BlobSeq()
      {
         _ptr = CvInvoke.CvBlobSeqCreate(StructSize.MCvBlob);
      }

      #region BolbSeqBase Members
      /// <summary>
      /// Get the total number of blob in the sequence
      /// </summary>
      public override int Count
      {
         get
         {
            return CvInvoke.CvBlobSeqGetBlobNum(_ptr);
         }
      }

      /// <summary>
      /// Return the specific blob 
      /// </summary>
      /// <param name="i">the index of the blob</param>
      /// <returns>The specific blob</returns>
      public override MCvBlob this[int i]
      {
         get
         {
            return (MCvBlob)Marshal.PtrToStructure(CvInvoke.CvBlobSeqGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }

      /// <summary>
      /// Get the blob with the specific id
      /// </summary>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>The blob of the specific id, if it doesn't exist, null is returned</returns>
      public override MCvBlob? GetBlobByID(int blobID)
      {
         IntPtr blobPtr = CvInvoke.CvBlobSeqGetBlobByID(_ptr, blobID);
         if (blobPtr == IntPtr.Zero) return null;
         return (MCvBlob?)Marshal.PtrToStructure(blobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Release the BlobSeq
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobSeqRelease(_ptr);
      }
      #endregion
   }
}
