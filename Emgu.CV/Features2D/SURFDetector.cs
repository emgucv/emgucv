using System;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped CvSURFParams structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct SURFDetector : IKeyPointDetector, IDescriptorExtractor
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDetectorDetectKeyPoints(
         ref SURFDetector detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDetectorDetectFeature(
         ref SURFDetector detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvSURFDetectorComputeDescriptors(
         ref SURFDetector detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);
      #endregion

      /// <summary>
      /// Create a MCvSURFParams using the specific values
      /// </summary>
      /// <param name="hessianThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extendedFlag">      
      /// false means basic descriptors (64 elements each),
      /// true means extended descriptors (128 elements each)
      /// </param>
      public SURFDetector(double hessianThresh, bool extendedFlag)
      {
         SURFDetector p = CvInvoke.cvSURFParams(hessianThresh, extendedFlag ? 1 : 0);
         extended = p.extended;
         hessianThreshold = p.hessianThreshold;
         nOctaves = p.nOctaves;
         nOctaveLayers = p.nOctaveLayers;
      }

      /// <summary>
      /// 0 means basic descriptors (64 elements each),
      /// 1 means extended descriptors (128 elements each)
      /// </summary>
      public int extended;

      /// <summary>
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </summary>
      public double hessianThreshold;

      /// <summary>
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled (3 by default)
      /// </summary>
      public int nOctaves;

      /// <summary>
      /// The number of layers within each octave (4 by default)
      /// </summary>
      public int nOctaveLayers;

      /// <summary>
      /// Detect the SURF keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract SURF features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>An array of SURF key points</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint keypoints = new VectorOfKeyPoint())
         {
            CvSURFDetectorDetectKeyPoints(ref this, image, mask, keypoints);
            return keypoints.ToArray();
         }
      }

      /// <summary>
      /// Detect image features from the given image
      /// </summary>
      /// <param name="image">The image to detect features from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The Image features detected from the given image</returns>
      public ImageFeature[] DetectFeatures(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (VectorOfKeyPoint pts = new VectorOfKeyPoint())
         using (VectorOfFloat descs = new VectorOfFloat())
         {
            CvSURFDetectorDetectFeature(ref this, image, mask, pts, descs);
            MKeyPoint[] kpts = pts.ToArray();
            int n = kpts.Length;
            long add = descs.StartAddress.ToInt64();

            ImageFeature[] features = new ImageFeature[n];
            int sizeOfdescriptor = extended == 0 ? 64 : 128;
            for (int i = 0; i < n; i++, add += sizeOfdescriptor * sizeof(float))
            {
               features[i].KeyPoint = kpts[i];
               float[] desc = new float[sizeOfdescriptor];
               Marshal.Copy(new IntPtr(add), desc, 0, sizeOfdescriptor);
               features[i].Descriptor = desc;
            }
            return features;
         }
      }

      /// <summary>
      /// Compute the descriptor given the image and the point location
      /// </summary>
      /// <param name="image">The image where the descriptor will be computed from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <param name="keyPoints">The keypoint where the descriptor will be computed from</param>
      /// <returns>The image features founded on the keypoint location</returns>
      public ImageFeature[] ComputeDescriptors(Image<Gray, Byte> image, Image<Gray, byte> mask, MKeyPoint[] keyPoints)
      {
         using (VectorOfFloat descs = new VectorOfFloat())
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            kpts.Push(keyPoints);
            CvSURFDetectorComputeDescriptors(ref this, image, mask, kpts, descs);

            int n = keyPoints.Length;
            long address = descs.StartAddress.ToInt64();

            ImageFeature[] features = new ImageFeature[n];
            int sizeOfdescriptor = extended == 0 ? 64 : 128;
            for (int i = 0; i < n; i++, address += sizeOfdescriptor * sizeof(float))
            {
               features[i].KeyPoint = keyPoints[i];
               float[] desc = new float[sizeOfdescriptor];
               Marshal.Copy(new IntPtr(address), desc, 0, sizeOfdescriptor);
               features[i].Descriptor = desc;
            }
            return features;
         }
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <returns>The key pionts in the image</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, byte> image)
      {
         return DetectKeyPoints(image, null);
      }

      #endregion

      #region IDescriptorGenerator Members
      /// <summary>
      /// Compute the ImageFeature on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <returns>The ImageFeature from the given keypoints</returns>
      public ImageFeature[] ComputeDescriptors(Image<Gray, byte> image, MKeyPoint[] keyPoints)
      {
         return ComputeDescriptors(image, null, keyPoints);
      }

      #endregion
   }
}
