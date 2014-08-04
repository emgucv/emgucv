//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
      static SimpleBlobDetector()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Create a simple blob detector
      /// </summary>
      public SimpleBlobDetector()
      {
         _ptr = CvSimpleBlobDetectorCreate();
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

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get
         {
            return CvInvoke.cveAlgorithmFromFeatureDetector(((IFeatureDetector)this).FeatureDetectorPtr);
         }
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvSimpleBlobDetectorRelease(ref _ptr);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvSimpleBlobDetectorCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvSimpleBlobDetectorRelease(ref IntPtr detector);
   }
}