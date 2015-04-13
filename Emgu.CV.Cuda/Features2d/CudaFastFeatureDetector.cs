//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// A FAST detector using Cuda
   /// </summary>
   public class CudaFastFeatureDetector : FastDetector, IFeature2DAsync
   {
      private IntPtr _feature2DAsyncPtr;


      /// <summary>
      /// Create a fast detector with the specific parameters
      /// </summary>
      /// <param name="threshold">Threshold on difference between intensity of center pixel and pixels on circle around
      /// this pixel. Use 10 for default.</param>
      /// <param name="nonmaxSupression">Specifiy if non-maximum supression should be used.</param>
      public CudaFastFeatureDetector(int threshold = 10, bool nonmaxSupression = true, FastDetector.DetectorType type = DetectorType.Type9_16, int maxNKeypoints = 5000)
      {
         _ptr = CudaInvoke.cveCudaFastFeatureDetectorCreate(threshold, nonmaxSupression, type, maxNKeypoints, ref _feature2D, ref _feature2DAsyncPtr);
      }


      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            CudaInvoke.cveCudaFastFeatureDetectorRelease(ref _ptr);
            _feature2D = IntPtr.Zero;
            _feature2DAsyncPtr = IntPtr.Zero;
         }
      }

      IntPtr IFeature2DAsync.Feature2DAsyncPtr
      {
         get { return _feature2DAsyncPtr; }
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveCudaFastFeatureDetectorCreate(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmaxSupression,
         FastDetector.DetectorType type,
         int maxPoints,
         ref IntPtr feature2D,
         ref IntPtr feature2DAsync);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveCudaFastFeatureDetectorRelease(ref IntPtr detector);

   }
}
