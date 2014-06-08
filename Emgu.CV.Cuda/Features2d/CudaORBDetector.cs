//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// An ORB detector using Cuda
   /// </summary>
   public class CudaORBDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a ORBDetector using the specific values
      /// </summary>
      /// <param name="numberOfFeatures">The number of desired features.</param>
      /// <param name="scaleFactor">Coefficient by which we divide the dimensions from one scale pyramid level to the next.</param>
      /// <param name="nLevels">The number of levels in the scale pyramid.</param>
      /// <param name="firstLevel">The level at which the image is given. If 1, that means we will also look at the image.<paramref name="scaleFactor"/> times bigger</param>
      /// <param name="edgeThreshold">How far from the boundary the points should be.</param>
      /// <param name="WTK_A">How many random points are used to produce each cell of the descriptor (2, 3, 4 ...).</param>
      /// <param name="scoreType">Type of the score to use.</param>
      /// <param name="patchSize">Patch size.</param>
      public CudaORBDetector(
         int numberOfFeatures = 500, 
         float scaleFactor = 1.2f, 
         int nLevels = 8, 
         int edgeThreshold = 31, 
         int firstLevel = 0, 
         int WTK_A = 2, 
         ORBDetector.ScoreType scoreType = ORBDetector.ScoreType.Harris, 
         int patchSize = 31)
      {
         _ptr = CudaInvoke.cudaORBDetectorCreate(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTK_A, scoreType, patchSize);
      }

      /// <summary>
      /// Detect keypoints in the CudaImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>
      /// The keypoints GpuMat that will have 1 row.
      /// keypoints.at&lt;float[6]&gt;(1, i) contains i'th keypoint
      /// format: (x, y, size, response, angle, octave)
      /// </returns>
      public void DetectKeyPointsRaw(GpuMat img, GpuMat mask, GpuMat keyPoints)
      {
         CudaInvoke.cudaORBDetectorDetectKeyPoints(_ptr, img, mask, keyPoints);
      }

      /// <summary>
      /// Detect keypoints in the CudaImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(CudaImage<Gray, Byte> img, CudaImage<Gray, Byte> mask)
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
         CudaInvoke.cudaORBDownloadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Compute the keypoints and descriptors given the image
      /// </summary>
      /// <param name="image">The image where the keypoints and descriptors will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="descriptors">The resulting descriptors</param>
      /// <param name="keyPoints">The resulting keypoints</param>
      public void ComputeRaw(GpuMat image, GpuMat mask, GpuMat keyPoints, GpuMat descriptors)
      {
         CudaInvoke.cudaORBDetectorCompute(_ptr, image, mask, keyPoints, descriptors);
      }

      /// <summary>
      /// Return the size of the descriptor (64/128)
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return CudaInvoke.cudaORBDetectorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaORBDetectorRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaORBDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, ORBDetector.ScoreType scoreType, int patchSize);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaORBDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaORBDetectorDetectKeyPoints(IntPtr detector, IntPtr img, IntPtr mask, IntPtr keypoints);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaORBDownloadKeypoints(IntPtr detector, IntPtr keypointsGPU, IntPtr keypoints);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaORBDetectorCompute(
         IntPtr detector,
         IntPtr img,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cudaORBDetectorGetDescriptorSize(IntPtr detector);
   }
}
