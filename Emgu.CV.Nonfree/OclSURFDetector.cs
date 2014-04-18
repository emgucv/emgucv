/*
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
using Emgu.CV.OpenCL;

namespace Emgu.CV.Nonfree
{
   /// <summary>
   /// A SURF detector using OpenCL
   /// </summary>
   public class OclSURFDetector : UnmanagedObject
   {
      /// <summary>
      /// Create an OpenCL SURF detector using the default parameters
      /// </summary>
      /// <param name="detector">The surf detector where the parameters will be borrow from</param>
      /// <param name="FeaturesRatio">Max features = featuresRatio * img.size().srea(). Use 0.01 for default</param>
      public OclSURFDetector(MCvSURFParams detector, float FeaturesRatio)
         : this((float)detector.HessianThreshold, detector.NOctaves, detector.NOctaveLayers, (detector.Extended != 0), 0.01f, (detector.Upright != 0))
      {
      }

      /// <summary>
      /// Create an OpenCL SURF detector
      /// </summary>
      /// <param name="hessianThreshold">The interest operator threshold. Use 100 for default</param>
      /// <param name="nOctaves">The number of octaves to process. Use 4 for default</param>
      /// <param name="nIntervals">The number of intervals in each octave. Use 4 for default</param>
      /// <param name="extended">True, if generate 128-len descriptors, false - 64-len descriptors. Use true for default.</param>
      /// <param name="featuresRatio">Max features = featuresRatio * img.size().srea().</param>
      /// <param name="upright">If set to true, the orientation is not computed for the keypoints</param>
      public OclSURFDetector(
         float hessianThreshold = 100,
         int nOctaves = 4,
         int nIntervals = 4,
         bool extended = true,
         float featuresRatio = 0.01f,
         bool upright = false)
      {
         _ptr = NonfreeInvoke.oclSURFDetectorCreate(hessianThreshold, nOctaves, nIntervals, extended, featuresRatio, upright);
      }

      /// <summary>
      /// Detect keypoints in the OclImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>
      /// The keypoints OclMat that will have 1 row.
      /// keypoints.at&lt;float[6]&gt;(1, i) contains i'th keypoint
      /// format: (x, y, size, response, angle, octave)
      /// </returns>
      public OclMat<float> DetectKeyPointsRaw(OclImage<Gray, Byte> img, OclImage<Gray, Byte> mask)
      {
         OclMat<float> result = new OclMat<float>();
         NonfreeInvoke.oclSURFDetectorDetectKeyPoints(_ptr, img, mask, result);
         return result;
      }

      /// <summary>
      /// Detect keypoints in the OclImage
      /// </summary>
      /// <param name="img">The image where keypoints will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(OclImage<Gray, Byte> img, OclImage<Gray, Byte> mask)
      {
         using (OclMat<float> tmp = DetectKeyPointsRaw(img, mask))
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            DownloadKeypoints(tmp, kpts);
            return kpts.ToArray();
         }
      }

      /// <summary>
      /// Obtain the keypoints array from OclMat
      /// </summary>
      /// <param name="src">The keypoints obtained from DetectKeyPointsRaw</param>
      /// <param name="dst">The vector of keypoints</param>
      public void DownloadKeypoints(OclMat<float> src, VectorOfKeyPoint dst)
      {
         NonfreeInvoke.oclSURFDownloadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Obtain an OclMat from the keypoints array
      /// </summary>
      /// <param name="src">The keypoints array</param>
      /// <param name="dst">An OclMat that represent the keypoints</param>
      public void UploadKeypoints(VectorOfKeyPoint src, OclMat<float> dst)
      {
         NonfreeInvoke.oclSURFUploadKeypoints(_ptr, src, dst);
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from. The order of the keypoints might be changed unless the GPU_SURF detector is UP-RIGHT.</param>
      /// <returns>The image features founded on the keypoint location</returns>
      public OclMat<float> ComputeDescriptorsRaw(OclImage<Gray, Byte> image, OclImage<Gray, byte> mask, OclMat<float> keyPoints)
      {
         OclMat<float> descriptors = new OclMat<float>(keyPoints.Size.Height, DescriptorSize, 1);
         NonfreeInvoke.oclSURFDetectorCompute(_ptr, image, mask, keyPoints, descriptors, true);
         return descriptors;
      }

      /// <summary>
      /// Return the size of the descriptor (64/128)
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return NonfreeInvoke.oclSURFDetectorGetDescriptorSize(_ptr);
         }
      }

      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         NonfreeInvoke.oclSURFDetectorRelease(ref _ptr);
      }
   }

   public static partial class NonfreeInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclSURFDetectorCreate(
         double hessianThreshold,
         int nOctaves,
         int nOctaveLayers,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool extended,
         float keypointsRatio,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool upright);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclSURFDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclSURFDetectorDetectKeyPoints(IntPtr detector, IntPtr img, IntPtr mask, IntPtr keypoints);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclSURFDownloadKeypoints(IntPtr detector, IntPtr keypointsGPU, IntPtr keypoints);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclSURFUploadKeypoints(IntPtr detector, IntPtr keypoints, IntPtr keypointsGPU);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclSURFDetectorCompute(
         IntPtr detector,
         IntPtr img,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useProvidedKeypoints);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclSURFDetectorGetDescriptorSize(IntPtr detector);
   }
}
*/