//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// An ORB detector using GPU
   /// </summary>
   public class GpuORBDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a ORBDetector using the specific values
      /// </summary>
      /// <param name="numberOfFeatures">The number of desired features. Use 500 for default.</param>
      /// <param name="scaleFactor">Coefficient by which we divide the dimensions from one scale pyramid level to the next. Use 1.2f for default value</param>
      /// <param name="nLevels">The number of levels in the scale pyramid. Use 3 for default value.</param>
      /// <param name="firstLevel">The level at which the image is given. If 1, that means we will also look at the image.<paramref name="scaleFactor"/> times bigger</param>
      /// <param name="edgeThreshold">How far from the boundary the points should be. Use 0 for default.</param>
      /// <param name="WTK_A">How many random points are used to produce each cell of the descriptor (2, 3, 4 ...). Use 2 for default.</param>
      /// <param name="scoreType">Type of the score to use. Use Harris for default.</param>
      /// <param name="patchSize">Patch size. Use 31 for default.</param>
      public GpuORBDetector(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTK_A, ORBDetector.ScoreType scoreType, int patchSize)
      {
         _ptr = GpuInvoke.gpuORBDetectorCreate(numberOfFeatures, scaleFactor, nLevels, edgeThreshold, firstLevel, WTK_A, scoreType, patchSize);
      }

      /// <summary>
      /// Create a GpuORBDetector with the default parameters
      /// </summary>
      /// <param name="numberOfFeatures">The number of desired features. Use 500 for default.</param>
      public GpuORBDetector(int numberOfFeatures)
         : this(numberOfFeatures, 1.2f, 3, 31, 0, 2, ORBDetector.ScoreType.Harris, 31)
      {
      }

      /// <summary>
      /// Detect keypoints in the GpuImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>
      /// The keypoints GpuMat that will have 1 row.
      /// keypoints.at&lt;float[6]&gt;(1, i) contains i'th keypoint
      /// format: (x, y, size, response, angle, octave)
      /// </returns>
      public GpuMat<float> DetectKeyPointsRaw(GpuImage<Gray, Byte> img, GpuImage<Gray, Byte> mask)
      {
         GpuMat<float> result = new GpuMat<float>();
         GpuInvoke.gpuORBDetectorDetectKeyPoints(_ptr, img, mask, result);
         return result;
      }

      /// <summary>
      /// Detect keypoints in the GpuImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(GpuImage<Gray, Byte> img, GpuImage<Gray, Byte> mask)
      {
         using (GpuMat<float> tmp = DetectKeyPointsRaw(img, mask))
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            DownloadKeypoints(tmp, kpts);
            return kpts.ToArray();
         }
      }

      /// <summary>
      /// Obtain the keypoints array from GpuMat
      /// </summary>
      /// <param name="src">The keypoints obtained from DetectKeyPointsRaw</param>
      /// <param name="dst">The vector of keypoints</param>
      public void DownloadKeypoints(GpuMat<float> src, VectorOfKeyPoint dst)
      {
         GpuInvoke.gpuORBDownloadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Compute the keypoints and descriptors given the image
      /// </summary>
      /// <param name="image">The image where the keypoints and descriptors will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="descriptors">The resulting descriptors</param>
      /// <param name="keyPoints">The resulting keypoints</param>
      public void ComputeRaw(GpuImage<Gray, Byte> image, GpuImage<Gray, byte> mask, out GpuMat<float> keyPoints, out GpuMat<Byte> descriptors)
      {
         keyPoints = new GpuMat<float>();
         descriptors = new GpuMat<byte>();
         GpuInvoke.gpuORBDetectorCompute(_ptr, image, mask, keyPoints, descriptors);
      }

      /// <summary>
      /// Return the size of the descriptor (64/128)
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return GpuInvoke.gpuORBDetectorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuORBDetectorRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuORBDetectorCreate(int numberOfFeatures, float scaleFactor, int nLevels, int edgeThreshold, int firstLevel, int WTA_K, ORBDetector.ScoreType scoreType, int patchSize);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuORBDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuORBDetectorDetectKeyPoints(IntPtr detector, IntPtr img, IntPtr mask, IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuORBDownloadKeypoints(IntPtr detector, IntPtr keypointsGPU, IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuORBDetectorCompute(
         IntPtr detector,
         IntPtr img,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int gpuORBDetectorGetDescriptorSize(IntPtr detector);
   }
}
