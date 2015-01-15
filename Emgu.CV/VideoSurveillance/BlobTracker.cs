/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A Blob Tracker
   /// </summary>
   public class BlobTracker : BlobSeqBase
   {
      static BlobTracker()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a blob trakcer of the specific type
      /// </summary>
      /// <param name="type">The type of the blob tracker</param>
      public BlobTracker(CvEnum.BlobTrackerType type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BlobTrackerType.CC:
               _ptr = CvCreateBlobTrackerCC();
               break;
            case Emgu.CV.CvEnum.BlobTrackerType.CCMSPF:
               _ptr = CvCreateBlobTrackerCCMSPF();
               break;
            case Emgu.CV.CvEnum.BlobTrackerType.MS:
               _ptr = CvCreateBlobTrackerMS();
               break;
            case Emgu.CV.CvEnum.BlobTrackerType.MSFG:
               _ptr = CvCreateBlobTrackerMSFG();
               break;
            case Emgu.CV.CvEnum.BlobTrackerType.MSFGS:
               _ptr = CvCreateBlobTrackerMSFGS();
               break;
            case Emgu.CV.CvEnum.BlobTrackerType.MSPF:
               _ptr = CvCreateBlobTrackerMSPF();
               break;
         }
      }

      /// <summary>
      /// Add new blob to track it and assign to this blob personal ID
      /// </summary>
      /// <param name="blob">Structure with blob parameters (ID is ignored)</param>
      /// <param name="currentImage">current image</param>
      /// <param name="currentForegroundMask">Current foreground mask</param>
      /// <returns>Newly added blob</returns>
      public MCvBlob Add<TColor, TDepth>(MCvBlob blob, Image<TColor, TDepth> currentImage, Image<Gray, Byte> currentForegroundMask)
         where TColor : struct, IColor
         where TDepth : new()
      {
         IntPtr bobPtr = CvBlobTrackerAddBlob(
            _ptr,
            ref blob,
            currentImage == null ? IntPtr.Zero : currentImage.Ptr,
            currentForegroundMask);
         return (MCvBlob)Marshal.PtrToStructure(bobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Delete blob by its index
      /// </summary>
      /// <param name="blobIndex">The index of the blob</param>
      public void RemoveAt(int blobIndex)
      {
         CvBlobTrackerDelBlob(_ptr, blobIndex);
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
            return (MCvBlob)Marshal.PtrToStructure(CvBlobTrackerGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }

      /// <summary>
      /// Get the blob with the specific id
      /// </summary>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>The blob of the specific id, if it doesn't exist, MCvBlob.Empty is returned</returns>
      public override MCvBlob GetBlobByID(int blobID)
      {
         IntPtr blobPtr = CvBlobTrackerGetBlobByID(_ptr, blobID);
         if (blobPtr == IntPtr.Zero) return MCvBlob.Empty;
         return (MCvBlob)Marshal.PtrToStructure(blobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Get the number of blobs in this tracker
      /// </summary>
      public override int Count
      {
         get
         {
            return CvBlobTrackerGetBlobNum(_ptr);
         }
      }

      /// <summary>
      /// Release the blob trakcer
      /// </summary>
      protected override void DisposeObject()
      {
         CvBlobTrackerRealease(ref _ptr);
      }
      #endregion

      /// <summary>
      /// Simple blob tracker based on connected component tracking
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobTrackerCC();

      /// <summary>
      /// Connected component tracking and mean-shift particle filter collion-resolver
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobTrackerCCMSPF();

      /// <summary>
      /// Blob tracker that integrates meanshift and connected components
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobTrackerMSFG();

      /// <summary>
      /// Blob tracker that integrates meanshift and connected components:
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobTrackerMSFGS();

      /// <summary>
      /// Meanshift without connected-components
      /// </summary>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobTrackerMS();

      /// <summary>
      /// Particle filtering via Bhattacharya coefficient, which
      /// is roughly the dot-product of two probability densities.
      /// </summary>
      /// <remarks>See: Real-Time Tracking of Non-Rigid Objects using Mean Shift Comanicius, Ramesh, Meer, 2000, 8p</remarks>
      /// <returns>Pointer to the blob tracker</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobTrackerMSPF();

      /// <summary>
      /// Release the blob tracker
      /// </summary>
      /// <param name="tracker">The tracker to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBlobTrackerRealease(ref IntPtr tracker);

      /// <summary>
      /// Return number of currently tracked blobs
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <returns>Number of currently tracked blobs</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvBlobTrackerGetBlobNum(IntPtr tracker);

      /// <summary>
      /// Return pointer to specified by index blob
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobIndex">The index of the blob</param>
      /// <returns>Pointer to the blob with the specific index</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBlobTrackerGetBlob(IntPtr tracker, int blobIndex);

      /// <summary>
      /// Return pointer to specified by index blob
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobId">The id of the blob</param>
      /// <returns>Pointer to the blob with specific id</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBlobTrackerGetBlobByID(IntPtr tracker, int blobId);

      /// <summary>
      /// Delete blob by its index
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blobIndex">The index of the blob</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBlobTrackerDelBlob(IntPtr tracker, int blobIndex);

      /// <summary>
      /// Add new blob to track it and assign to this blob personal ID
      /// </summary>
      /// <param name="tracker">The tracker</param>
      /// <param name="blob">pointer to structure with blob parameters (ID is ignored)</param>
      /// <param name="currentImage">current image</param>
      /// <param name="currentForegroundMask">current foreground mask</param>
      /// <returns>Pointer to new added blob</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBlobTrackerAddBlob(IntPtr tracker, ref MCvBlob blob, IntPtr currentImage, IntPtr currentForegroundMask);
   }
}
*/