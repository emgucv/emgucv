using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Wrapped CvSURFParams structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSURFParams
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSURFDetectorDetectKeyPoints(
         ref MCvSURFParams detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSURFDetectorDetectFeature(
         ref MCvSURFParams detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSURFDetectorComputeDescriptors(
         ref MCvSURFParams detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         int numberOfKeyPoints,
         IntPtr descriptors);

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
      public MCvSURFParams(double hessianThresh, bool extendedFlag)
      {
         MCvSURFParams p = CvInvoke.cvSURFParams(hessianThresh, extendedFlag ? 1 : 0);
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
         using (MemStorage stor = new MemStorage())
         {
            Seq<MKeyPoint> seq = new Seq<MKeyPoint>(stor);
            CvSURFDetectorDetectKeyPoints(ref this, image, mask, seq.Ptr);
            return seq.ToArray();
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
         using (MemStorage stor = new MemStorage())
         using (VectorOfFloat descs = new VectorOfFloat())
         {
            Seq<MKeyPoint> pts = new Seq<MKeyPoint>(stor);
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
         {
            GCHandle handle = GCHandle.Alloc(keyPoints, GCHandleType.Pinned);
            CvSURFDetectorComputeDescriptors(ref this, image, mask, handle.AddrOfPinnedObject(), keyPoints.Length, descs );
            handle.Free();

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
   }
}
