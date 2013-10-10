//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public class SimpleBlobDetector : UnmanagedObject, IFeatureDetector
   {
      /// <summary>
      /// Create a simple blob detector
      /// </summary>
      public SimpleBlobDetector()
      {
         _ptr = CvInvoke.CvSimpleBlobDetectorCreate();
      }

      #region IFeatureDetector Members
      /// <summary>
      /// Get the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr IFeatureDetector.FeatureDetectorPtr
      {
         get
         {
            return _ptr;
         }
      }
      #endregion

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.CvSimpleBlobDetectorRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSimpleBlobDetectorCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSimpleBlobDetectorRelease(ref IntPtr detector);
   }
}