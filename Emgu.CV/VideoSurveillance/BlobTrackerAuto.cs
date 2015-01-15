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
         _ptr = BlobTrackerAutoInvoke.CvCreateBlobTrackerAuto1(ref p);
      }

      /// <summary>
      /// Create a default auto blob tracker 
      /// </summary>
      public BlobTrackerAuto()
      {
         BlobTrackerAutoParam<TColor> param = new BlobTrackerAutoParam<TColor>();
         param.FGDetector = new FGDetector<TColor>(Emgu.CV.CvEnum.ForgroundDetectorType.Fgd);
         _param = param;
         MCvBlobTrackerAutoParam1 p = _param.MCvBlobTrackerAutoParam1;
         _ptr = BlobTrackerAutoInvoke.CvCreateBlobTrackerAuto1(ref p);
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
      /// <param name="foregroundMask">the foreground mask to be used</param>
      public void Process(Image<TColor, Byte> currentFrame, Image<Gray, Byte> foregroundMask)
      {
         BlobTrackerAutoInvoke.CvBlobTrackerAutoProcess(_ptr, currentFrame.Ptr, foregroundMask == null ? IntPtr.Zero : foregroundMask.Ptr);
      }

      /// <summary>
      /// Get the foreground mask
      /// </summary>
      /// <returns>The foreground mask</returns>
      public Image<Gray, Byte> ForegroundMask
      {
         get
         {
            IntPtr foreground = BlobTrackerAutoInvoke.CvBlobTrackerAutoGetFGMask(_ptr);
            if (foreground == IntPtr.Zero) return null;
            MIplImage iplImage = (MIplImage)Marshal.PtrToStructure(foreground, typeof(MIplImage));
            return new Image<Gray, byte>(iplImage.Width, iplImage.Height, iplImage.WidthStep, iplImage.ImageData);
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
            return (MCvBlob)Marshal.PtrToStructure(BlobTrackerAutoInvoke.CvBlobTrackerAutoGetBlob(_ptr, i), typeof(MCvBlob));
         }
      }

      /// <summary>
      /// Get the blob with the specific id
      /// </summary>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>The blob of the specific id, if it doesn't exist, MCvBlob.Empty is returned</returns>
      public override MCvBlob GetBlobByID(int blobID)
      {
         IntPtr blobPtr = BlobTrackerAutoInvoke.CvBlobTrackerAutoGetBlobByID(_ptr, blobID);
         if (blobPtr == IntPtr.Zero) return MCvBlob.Empty;
         return (MCvBlob)Marshal.PtrToStructure(blobPtr, typeof(MCvBlob));
      }

      /// <summary>
      /// Release the blob tracker auto
      /// </summary>
      protected override void DisposeObject()
      {
         BlobTrackerAutoInvoke.CvBlobTrackerAutoRelease(ref _ptr);
      }

      /// <summary>
      /// Get the number of blobs in this tracker
      /// </summary>
      public override int Count
      {
         get
         {
            return BlobTrackerAutoInvoke.CvBlobTrackerAutoGetBlobNum(_ptr);
         }
      }
      #endregion
   }

   internal static class BlobTrackerAutoInvoke
   {
      static BlobTrackerAutoInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create blob tracker auto ver1
      /// </summary>
      /// <param name="param">The parameters for the tracker</param>
      /// <returns>Pointer to the blob tracker auto</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobTrackerAuto1(ref MCvBlobTrackerAutoParam1 param);

      /// <summary>
      /// Release the blob tracker auto
      /// </summary>
      /// <param name="tracker">The tracker to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBlobTrackerAutoRelease(ref IntPtr tracker);

      /// <summary>
      /// Get the blob of specific index from the auto blob tracker
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="index">The index of the blob</param>
      /// <returns>Pointer to the the blob</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBlobTrackerAutoGetBlob(IntPtr tracker, int index);

      /// <summary>
      /// Get the blob of specific id from the auto blob tracker
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="blobID">The id of the blob</param>
      /// <returns>Pointer to the blob of specific ID</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBlobTrackerAutoGetBlobByID(IntPtr tracker, int blobID);

      /// <summary>
      /// Get the number of blobs in the auto blob tracker 
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <returns>The number of blobs</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static int CvBlobTrackerAutoGetBlobNum(IntPtr tracker);

      /// <summary>
      /// Process a image frame
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <param name="pImg">The frame to process</param>
      /// <param name="pMask">The foreground mask, can be IntPtr.Zero if not needed</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBlobTrackerAutoProcess(IntPtr tracker, IntPtr pImg, IntPtr pMask);

      /// <summary>
      /// Get the foreground mask
      /// </summary>
      /// <param name="tracker">The auto blob tracker</param>
      /// <returns>Pointer to the foreground mask</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBlobTrackerAutoGetFGMask(IntPtr tracker);
   }

}
*/