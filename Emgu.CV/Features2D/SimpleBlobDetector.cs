//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Simple Blob detector
   /// </summary>
   public class SimpleBlobDetector : Feature2D
   {
      /// <summary>
      /// Create a simple blob detector
      /// </summary>
      public SimpleBlobDetector(SimpleBlobDetectorParams parameters = null)
      {
         if (parameters == null)
            _ptr = CvInvoke.cveSimpleBlobDetectorCreate(ref _feature2D);
         else
         {
            _ptr = CvInvoke.cveSimpleBlobDetectorCreateWithParams(ref _feature2D, parameters);
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveSimpleBlobDetectorRelease(ref _ptr);

         base.DisposeObject();
      }
   }

   /// <summary>
   /// Parameters for the simple blob detector
   /// </summary>
   public partial class SimpleBlobDetectorParams : UnmanagedObject
   {
      /// <summary>
      /// Create parameters for simple blob detector and use default values.
      /// </summary>
      public SimpleBlobDetectorParams()
      {
         _ptr = CvInvoke.cveSimpleBlobDetectorParamsCreate();
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this simple blob detector parameter.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            CvInvoke.cveSimpleBlobDetectorParamsRelease(ref _ptr);
         }
      }
   }

}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveSimpleBlobDetectorCreate(ref IntPtr feature2DPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveSimpleBlobDetectorCreateWithParams(ref IntPtr feature2DPtr, IntPtr parameters);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveSimpleBlobDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveSimpleBlobDetectorParamsCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveSimpleBlobDetectorParamsRelease(ref IntPtr parameters);

   }
}