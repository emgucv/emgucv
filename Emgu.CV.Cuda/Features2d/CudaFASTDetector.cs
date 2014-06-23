//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   public class CudaFASTDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a fast detector with the specific parameters
      /// </summary>
      /// <param name="threshold">Threshold on difference between intensity of center pixel and pixels on circle around
      /// this pixel. Use 10 for default.</param>
      /// <param name="nonmaxSupression">Specifiy if non-maximum supression should be used.</param>
      /// <param name="keypointsRatio">MaxKeypoints = keypointsRatio * img.size().area()</param>
      public CudaFASTDetector(int threshold, bool nonmaxSupression = true, double keypointsRatio = 0.05)
      {
         _ptr = CudaInvoke.cudaFASTDetectorCreate(threshold, nonmaxSupression, keypointsRatio);
      }

      /// <summary>
      /// Detect keypoints in the CudaImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keypoints">
      /// The keypoints GpuMat that will have 1 row.
      /// keypoints.at&lt;float[6]&gt;(1, i) contains i'th keypoint
      /// format: (x, y, size, response, angle, octave)
      /// </param>
      public void DetectKeyPointsRaw(GpuMat img, GpuMat mask, GpuMat keypoints)
      {
         
         CudaInvoke.cudaFASTDetectorDetectKeyPoints(_ptr, img, mask, keypoints);
         
      }

      /// <summary>
      /// Detect keypoints in the CudaImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(GpuMat img, GpuMat mask)
      {
         using (GpuMat tmp = new GpuMat())
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            DetectKeyPointsRaw(img, mask, tmp);
            DownloadKeypoints(tmp, kpts);
            return kpts.ToArray();
         }
      }

      /// <summary>
      /// Obtain the keypoints array from GpuMat
      /// </summary>
      /// <param name="src">The keypoints obtained from DetectKeyPointsRaw</param>
      /// <param name="dst">The vector of keypoints</param>
      public void DownloadKeypoints(GpuMat src, VectorOfKeyPoint dst)
      {
         CudaInvoke.cudaFASTDownloadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaFASTDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaFASTDetectorCreate(
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmaxSupression,
         double keypointsRatio);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaFASTDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaFASTDetectorDetectKeyPoints(IntPtr detector, IntPtr img, IntPtr mask, IntPtr keypoints);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaFASTDownloadKeypoints(IntPtr detector, IntPtr keypointsGpu, IntPtr keypoints);
   }
}
