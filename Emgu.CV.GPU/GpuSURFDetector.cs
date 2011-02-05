using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// A SURF detector using GPU
   /// </summary>
   public class GpuSURFDetector : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr gpuSURFDetectorCreate(
         float threshold,
         int nOctaves,
         int nIntervals,
         float initialScale,
         float l1,
         float l2,
         float l3,
         float l4,
         float edgeScale,
         int initialStep,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool extended,
         float featuresRatio);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuSURFDetectorRelease(ref IntPtr detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuSURFDetectorDetectKeyPoints(IntPtr detector, IntPtr img, IntPtr mask, IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDownloadKeypoints(IntPtr keypointsGPU, IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuUploadKeypoints(IntPtr keypoints, IntPtr keypointsGPU);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuSURFDetectorCompute(
         IntPtr detector,
         IntPtr img,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useProvidedKeypoints,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool calcOrientation);
      #endregion

      /// <summary>
      /// Create a GPU SURF detector using the default parameters
      /// </summary>
      public GpuSURFDetector()
         : this(0.1f, 4, 5, 2.0f, 3.0f/1.5f, 5.0f/1.5f, 3.0f/ 1.5f, 1.0f/1.5f, 0.81f, 1, true, 0.01f)
      {
      }

      /// <summary>
      /// Create a GPU SURF detector
      /// </summary>
      /// <param name="Threshold">The interest operator threshold. Use 0.1 for default</param>
      /// <param name="NOctaves">The number of octaves to process. Use 4 for default</param>
      /// <param name="NIntervals">The number of intervals in each octave. Use 4 for default</param>
      /// <param name="InitialScale">The scale associated with the first interval of the first octave. Use 2.0 for default</param>
      /// <param name="L1">Mask parameter l_1. Use 3.0 / 1.5 for default</param>
      /// <param name="L2">Mask parameter l_2. Use 5.0 / 1.5 for default</param>
      /// <param name="L3">Mask parameter l_3. Use 3.0 / 1.5 for default</param>
      /// <param name="L4">Mask parameter l_4. Use 1.0 / 1.5 for default</param>
      /// <param name="EdgeScale">The amount to scale the edge rejection mask. Use 0.81 for default</param>
      /// <param name="InitialStep">The initial sampling step in pixels. Use 1 for default</param>
      /// <param name="Extended">True, if generate 128-len descriptors, false - 64-len descriptors. Use true for default.</param>
      /// <param name="FeaturesRatio">Max features = featuresRatio * img.size().srea(). Use 0.01 for default</param>
      public GpuSURFDetector(
            float Threshold,
            int NOctaves,
            int NIntervals,
            float InitialScale,
            float L1,
            float L2,
            float L3,
            float L4,
            float EdgeScale,
            int InitialStep,
            bool Extended,
            float FeaturesRatio)
      {
         _ptr = gpuSURFDetectorCreate(Threshold, NOctaves, NIntervals, InitialScale, L1, L2, L3, L4, EdgeScale, InitialStep, Extended, FeaturesRatio);
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
         GpuMat<float> result = new GpuMat<float>(GpuInvoke.GpuMatCreateDefault());
         gpuSURFDetectorDetectKeyPoints(_ptr, img, mask, result);
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
         {
            return DownloadKeypoints(tmp);
         }
      }

      /// <summary>
      /// Obtain the keypoints array from GpuMat
      /// </summary>
      /// <param name="keypoints">The keypoints obtained from DetectKeyPointsRaw</param>
      /// <returns>An array of keypoints</returns>
      public static MKeyPoint[] DownloadKeypoints(GpuMat<float> keypoints)
      {
         using (Util.VectorOfKeyPoint vec = new Util.VectorOfKeyPoint())
         {
            gpuDownloadKeypoints(keypoints, vec);
            return vec.ToArray();
         }
      }

      /// <summary>
      /// Ontain a GpuMat from the keypoints array
      /// </summary>
      /// <param name="keypoints">The keypoints array</param>
      /// <returns>A GpuMat that represent the keypoints</returns>
      public static GpuMat<float> UploadKeypoints(MKeyPoint[] keypoints)
      {
         using (Util.VectorOfKeyPoint vec = new Util.VectorOfKeyPoint())
         {
            vec.Push(keypoints);
            GpuMat<float> result = new GpuMat<float>(GpuInvoke.GpuMatCreateDefault());
            gpuUploadKeypoints(vec, result);
            return result;
         }
      }

      /// <summary>
      /// Release the unmanaged resource associate to the Detector
      /// </summary>
      protected override void DisposeObject()
      {
         gpuSURFDetectorRelease(ref _ptr);
      }
   }
}
