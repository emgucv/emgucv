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
      private extern static void CvSURFDetectorDetect(
         ref MCvSURFParams detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors,
         [MarshalAs(UnmanagedType.I1)]
         bool useProvidedKeyPoints);

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

      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<MKeyPoint> seq = new Seq<MKeyPoint>(stor);
            CvSURFDetectorDetectKeyPoints(ref this, image, mask, seq.Ptr);
            return seq.ToArray();
         }
      }

      /*
      public void Detect(Image<Gray, Byte> image, Image<Gray, byte> mask)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<MKeyPoint> pts = new Seq<MKeyPoint>(stor);
            Seq<float> desc = new Seq<float>(stor);

            CvSURFDetectorDetect(ref this, image, mask, pts, desc, false);
         }
      }*/
   }
}
