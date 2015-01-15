//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      static SimpleBlobDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a simple blob detector
      /// </summary>
      public SimpleBlobDetector()
      {
         _ptr = CvSimpleBlobDetectorCreate(ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvSimpleBlobDetectorRelease(ref _ptr);

         base.DisposeObject();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSimpleBlobDetectorCreate(ref IntPtr feature2DPtr);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSimpleBlobDetectorRelease(ref IntPtr detector);
   }
}