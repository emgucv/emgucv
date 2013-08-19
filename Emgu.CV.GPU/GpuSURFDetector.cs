//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// A SURF detector using GPU
   /// </summary>
   public class GpuSURFDetector : UnmanagedObject
   {
      /// <summary>
      /// Create a GPU SURF detector using the default parameters
      /// </summary>
      public GpuSURFDetector()
         : this(100.0f, 4, 2, true, 0.01f, false)
      {
      }

      /// <summary>
      /// Create a GPU SURF detector using the default parameters
      /// </summary>
      /// <param name="detector">The surf detector where the parameters will be borrow from</param>
      /// <param name="featuresRatio">Max features = featuresRatio * img.size().srea(). Use 0.01 for default</param>
      public GpuSURFDetector(MCvSURFParams detector, float featuresRatio)
         : this((float)detector.HessianThreshold, detector.NOctaves, detector.NOctaveLayers, (detector.Extended != 0), featuresRatio, (detector.Upright != 0))
      {
      }

      /// <summary>
      /// Create a GPU SURF detector
      /// </summary>
      /// <param name="hessianThreshold">The interest operator threshold. Use 100 for default</param>
      /// <param name="nOctaves">The number of octaves to process. Use 4 for default</param>
      /// <param name="nIntervals">The number of intervals in each octave. Use 4 for default</param>
      /// <param name="extended">True, if generate 128-len descriptors, false - 64-len descriptors. Use true for default.</param>
      /// <param name="featuresRatio">Max features = featuresRatio * img.size().srea(). Use 0.01 for default</param>
      /// <param name="upright">Use false for default. If set to true, the orientation is not computed for the keypoints</param>
      public GpuSURFDetector(
         float hessianThreshold,
         int nOctaves,
         int nIntervals,
         bool extended,
         float featuresRatio,
         bool upright)
      {
         _ptr = GpuInvoke.gpuSURFDetectorCreate(hessianThreshold, nOctaves, nIntervals, extended, featuresRatio, upright);
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
         GpuInvoke.gpuSURFDetectorDetectKeyPoints(_ptr, img, mask, result);
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
         GpuInvoke.gpuSURFDownloadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Obtain a GpuMat from the keypoints array
      /// </summary>
      /// <param name="src">The keypoints array</param>
      /// <param name="dst">A GpuMat that represent the keypoints</param>
      public void UploadKeypoints(VectorOfKeyPoint src, GpuMat<float> dst)
      {
         GpuInvoke.gpuSURFUploadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. The order of the keypoints might be changed unless the GPU_SURF detector is UP-RIGHT.</param>
      /// <returns>The image features founded on the keypoint location</returns>
      public GpuMat<float> ComputeDescriptorsRaw(GpuImage<Gray, Byte> image, GpuImage<Gray, byte> mask, GpuMat<float> keyPoints)
      {
         GpuMat<float> descriptors = new GpuMat<float>(keyPoints.Size.Height, DescriptorSize, 1);
         GpuInvoke.gpuSURFDetectorCompute(_ptr, image, mask, keyPoints, descriptors, true);
         return descriptors;
      }

      /// <summary>
      /// Return the size of the descriptor (64/128)
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return GpuInvoke.gpuSURFDetectorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuSURFDetectorRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuSURFDetectorCreate(
         double hessianThreshold,
         int nOctaves,
         int nOctaveLayers,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool extended,
         float keypointsRatio,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool upright);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuSURFDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuSURFDetectorDetectKeyPoints(IntPtr detector, IntPtr img, IntPtr mask, IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuSURFDownloadKeypoints(IntPtr detector, IntPtr keypointsGPU, IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuSURFUploadKeypoints(IntPtr detector, IntPtr keypoints, IntPtr keypointsGPU);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuSURFDetectorCompute(
         IntPtr detector,
         IntPtr img,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useProvidedKeypoints);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int gpuSURFDetectorGetDescriptorSize(IntPtr detector);
   }
}
