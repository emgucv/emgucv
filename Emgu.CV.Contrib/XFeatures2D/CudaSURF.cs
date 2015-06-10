#if !(IOS || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_METRO || UNITY_EDITOR_WIN || NETFX_CORE)
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
using Emgu.CV.Cuda;

namespace Emgu.CV.XFeatures2D
{
   /// <summary>
   /// A SURF detector using Cuda
   /// </summary>
   public class CudaSURF : UnmanagedObject
   {
      /// <summary>
      /// Create a Cuda SURF detector
      /// </summary>
      /// <param name="hessianThreshold">The interest operator threshold.</param>
      /// <param name="nOctaves">The number of octaves to process.</param>
      /// <param name="nOctaveLayers">The number of layers in each octave.</param>
      /// <param name="extended">True, if generate 128-len descriptors, false - 64-len descriptors.</param>
      /// <param name="featuresRatio">Max features = featuresRatio * img.size().srea().</param>
      /// <param name="upright">If set to true, the orientation is not computed for the keypoints</param>
      public CudaSURF(
         float hessianThreshold = 100.0f,
         int nOctaves = 4,
         int nOctaveLayers = 2, 
         bool extended = true,
         float featuresRatio = 0.01f,
         bool upright = false)
      {
         _ptr = ContribInvoke.cudaSURFDetectorCreate(hessianThreshold, nOctaves, nOctaveLayers, extended, featuresRatio, upright);
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
      public GpuMat DetectKeyPointsRaw(GpuMat img, GpuMat mask = null)
      {
         GpuMat result = new GpuMat();
         ContribInvoke.cudaSURFDetectorDetectKeyPoints(_ptr, img, mask, result);
         return result;
      }

      /// <summary>
      /// Detect keypoints in the CudaImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(GpuMat img, GpuMat mask)
      {
         using (GpuMat tmp = DetectKeyPointsRaw(img, mask))
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
      public void DownloadKeypoints(GpuMat src, VectorOfKeyPoint dst)
      {
         ContribInvoke.cudaSURFDownloadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Obtain a GpuMat from the keypoints array
      /// </summary>
      /// <param name="src">The keypoints array</param>
      /// <param name="dst">A GpuMat that represent the keypoints</param>
      public void UploadKeypoints(VectorOfKeyPoint src, GpuMat dst)
      {
         ContribInvoke.cudaSURFUploadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. The order of the keypoints might be changed unless the GPU_SURF detector is UP-RIGHT.</param>
      /// <returns>The image features founded on the keypoint location</returns>
      public GpuMat ComputeDescriptorsRaw(GpuMat image, GpuMat mask, GpuMat keyPoints)
      {
         GpuMat descriptors = new GpuMat();
         ContribInvoke.cudaSURFDetectorCompute(_ptr, image, mask, keyPoints, descriptors, true);
         return descriptors;
      }

      /// <summary>
      /// Return the size of the descriptor (64/128)
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return ContribInvoke.cudaSURFDetectorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         ContribInvoke.cudaSURFDetectorRelease(ref _ptr);
      }
   }

   public static partial class ContribInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaSURFDetectorCreate(
         double hessianThreshold,
         int nOctaves,
         int nOctaveLayers,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool extended,
         float keypointsRatio,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool upright);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaSURFDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaSURFDetectorDetectKeyPoints(IntPtr detector, IntPtr img, IntPtr mask, IntPtr keypoints);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaSURFDownloadKeypoints(IntPtr detector, IntPtr keypointsGPU, IntPtr keypoints);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaSURFUploadKeypoints(IntPtr detector, IntPtr keypoints, IntPtr keypointsGPU);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaSURFDetectorCompute(
         IntPtr detector,
         IntPtr img,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useProvidedKeypoints);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int cudaSURFDetectorGetDescriptorSize(IntPtr detector);
   }
}
#endif