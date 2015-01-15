/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// A blob detector
   /// </summary>
   public class BlobDetector : UnmanagedObject
   {
      static BlobDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a blob detector of specific type
      /// </summary>
      /// <param name="type">The type of the detector</param>
      public BlobDetector(CvEnum.BlobDetectorType type)
      {
         switch (type)
         {
            case Emgu.CV.CvEnum.BlobDetectorType.Simple:
               _ptr = CvCreateBlobDetectorSimple();
               break;
            case Emgu.CV.CvEnum.BlobDetectorType.CC:
               _ptr = CvCreateBlobDetectorCC();
               break;
         }
      }

      /// <summary>
      /// Detect new blobs
      /// </summary>
      /// <param name="imageForeground">The foreground mask</param>
      /// <param name="newBlob">The new blob list</param>
      /// <param name="oldBlob">The old blob list, can be null if not needed</param>
      /// <returns>True if new blob is detected</returns>
      public bool DetectNewBlob(Image<Gray, Byte> imageForeground, BlobSeq newBlob, BlobSeq oldBlob)
      {
         return CvBlobDetectorDetectNewBlob(_ptr, IntPtr.Zero, imageForeground.Ptr, newBlob.Ptr, oldBlob);
      }

      /// <summary>
      /// Release the detector
      /// </summary>
      protected override void DisposeObject()
      {
         CvBlobDetectorRelease(ref _ptr);
      }

      /// <summary>
      /// Release the blob detector
      /// </summary>
      /// <param name="detector">the detector to be released</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBlobDetectorRelease(ref IntPtr detector);

      /// <summary>
      /// Detect new blobs.
      /// </summary>
      /// <param name="detector">The blob detector</param>
      /// <param name="img">The image</param>
      /// <param name="imgFG">The foreground mask</param>
      /// <param name="newBlobList">The new blob list</param>
      /// <param name="oldBlobList">The old blob list</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolToIntMarshalType)]
      internal extern static bool CvBlobDetectorDetectNewBlob(IntPtr detector, IntPtr img, IntPtr imgFG, IntPtr newBlobList, IntPtr oldBlobList);

      /// <summary>
      /// Get a simple blob detector 
      /// </summary>
      /// <returns>Pointer to the blob detector</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobDetectorSimple();

      /// <summary>
      /// Get a CC blob detector 
      /// </summary>
      /// <returns>Pointer to the blob detector</returns>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvCreateBlobDetectorCC();
   }
}
*/