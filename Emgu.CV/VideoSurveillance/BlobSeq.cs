//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A Blob Seq
   /// </summary>
   public class BlobSeq : BlobSeqBase
   {
      #region Pinvoke
      /// <summary>
      /// Create a BlobSeq
      /// </summary>
      /// <param name="blobSize">The size of the blob in bytes</param>
      /// <returns>Pointer to the BlobSeq</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobSeqCreate(int blobSize);

      /// <summary>
      /// Release the blob sequence
      /// </summary>
      /// <param name="blobSeq">The BlobSeq to be released</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobSeqRelease(ref IntPtr blobSeq);

      /// <summary>
      /// Get the specific blob from the blob sequence
      /// </summary>
      /// <param name="blobSeq">the blob sequence</param>
      /// <param name="blobIndex">the index of the blob to be retrieved</param>
      /// <returns>Pointer to the specific blob</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobSeqGetBlob(IntPtr blobSeq, int blobIndex);

      /// <summary>
      /// Get the specific blob from the blob sequence
      /// </summary>
      /// <param name="blobSeq">the blob sequence</param>
      /// <param name="blobIndex">the index of the blob to be retrieved</param>
      /// <returns>Pointer to the specific blob</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr CvBlobSeqGetBlobByID(IntPtr blobSeq, int blobIndex);

      /// <summary>
      /// Get the number of blob in the blob sequence
      /// </summary>
      /// <param name="blobSeq">The blob sequence</param>
      /// <returns>The number of blob in the blob sequence</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static int CvBlobSeqGetBlobNum(IntPtr blobSeq);

      /// <summary>
      /// Clear the blob sequence
      /// </summary>
      /// <param name="blobSeq">The blob sequence to be cleared</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvBlobSeqClear(IntPtr blobSeq);
      #endregion

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
         _ptr = CvBlobSeqCreate(StructSize.MCvBlob);
      }

      /// <summary>
      /// Clear the BlobSeq
      /// </summary>
      public void Clear()
      {
         CvBlobSeqClear(_ptr);
      }

      #region BolbSeqBase Members
      /// <summary>
      /// Get the total number of blob in the sequence
      /// </summary>
      public override int Count
      {
         get
         {
            return CvBlobSeqGetBlobNum(_ptr);
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
            return (MCvBlob)Marshal.PtrToStructure(CvBlobSeqGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }

      /// <summary>
      /// Get the blob with the specific id
      /// </summary>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>The blob of the specific id, if it doesn't exist, null is returned</returns>
      public override MCvBlob GetBlobByID(int blobID)
      {
         IntPtr blobPtr = CvBlobSeqGetBlobByID(_ptr, blobID);
         if (blobPtr == IntPtr.Zero) return MCvBlob.Empty;
         return (MCvBlob)Marshal.PtrToStructure(blobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Release the BlobSeq
      /// </summary>
      protected override void DisposeObject()
      {
         CvBlobSeqRelease(ref _ptr);
      }
      #endregion
   }
}
