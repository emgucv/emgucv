using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// a Blob Seq
   /// </summary>
   public class BlobSeq : UnmanagedObject
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
         _ptr = CvInvoke.CvBlobSeqCreate(Marshal.SizeOf(typeof(MCvBlob)));
      }

      /// <summary>
      /// Get the total number of blob in the sequence
      /// </summary>
      public int Count
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
      public MCvBlob this[int i]
      {
         get
         {
            return (MCvBlob)Marshal.PtrToStructure(CvInvoke.CvBlobSeqGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }


      /// <summary>
      /// Release the BlobSeq
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBlobSeqRelease(_ptr);
      }
   }
}
