using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;
using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// Wrapped CvSURFParams structure
   /// </summary>
   public class SIFTDetector : UnmanagedObject
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSIFTDetectorDetectKeyPoints(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSIFTDetectorDetectFeature(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static int CvSIFTDetectorGetDescriptorSize(IntPtr detector);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSIFTDetectorComputeDescriptors(
         IntPtr detector,
         IntPtr image,
         IntPtr mask,
         IntPtr keypoints,
         int numberOfKeyPoints,
         IntPtr descriptors);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static IntPtr CvSIFTDetectorCreate(
         int nOctaves, int nOctaveLayers, int firstOctave, //common parameters
         double threshold, double edgeThreshold, AngleMode angleMode, //detector parameters
         double magnification, [MarshalAs(UnmanagedType.I1)] bool isNormalize); //descriptor parameters

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvSIFTDetectorRelease(ref IntPtr detector);

      /// <summary>
      /// The angle mode for the key point detector
      /// </summary>
      public enum AngleMode
      {
         /// <summary>
         /// First angle
         /// </summary>
         FIRST_ANGLE = 0,
         /// <summary>
         /// Average angle
         /// </summary>
         AVERAGE_ANGLE = 1
      }

      /// <summary>
      /// Create a SIFTDetector using the specific values
      /// </summary>
      /// <param name="nOctaves">The number of octaves. Use 4 for default</param>
      /// <param name="nOctaveLayers">The number of octaves layers. Use 3 for default</param>
      /// <param name="firstOctave">Use -1 for default</param>
      /// <param name="threshold">Use 0.04 / nOctavesLayers / 2.0 as default</param>
      /// <param name="edgeThreshold">Use 10.0 as default</param>
      /// <param name="angleMode">Angle mode</param>
      /// <param name="magnification">Use 3.0 as default</param>
      /// <param name="isNormalize">Use true as default</param>
      public SIFTDetector(
         int nOctaves, int nOctaveLayers, int firstOctave, //common parameters
         double threshold, double edgeThreshold, AngleMode angleMode, //detector parameters
         double magnification, bool isNormalize) //descriptor parameters
      {
         _ptr = CvSIFTDetectorCreate(nOctaves, nOctaveLayers, firstOctave, threshold, edgeThreshold, angleMode, magnification, isNormalize);
      }

      /// <summary>
      /// Create a SIFT detector with the default parameters
      /// </summary>
      public SIFTDetector()
         :this(4, 3, -1, 0.04 / 3 / 2.0, 10.0, SIFTDetector.AngleMode.AVERAGE_ANGLE, 3.0, true)
      {
      }

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
            CvSIFTDetectorDetectKeyPoints(_ptr, image, mask, seq.Ptr);
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
            CvSIFTDetectorDetectFeature(_ptr, image, mask, pts, descs);
            MKeyPoint[] kpts = pts.ToArray();
            int n = kpts.Length;
            long add = descs.StartAddress.ToInt64();

            ImageFeature[] features = new ImageFeature[n];
            int sizeOfdescriptor = DescriptorSize;
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
      /// Get the size of the descriptor
      /// </summary>
      public int DescriptorSize
      {
         get
         {
            return CvSIFTDetectorGetDescriptorSize(_ptr);
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
            CvSIFTDetectorComputeDescriptors(_ptr, image, mask, handle.AddrOfPinnedObject(), keyPoints.Length, descs);
            handle.Free();

            int n = keyPoints.Length;
            long address = descs.StartAddress.ToInt64();

            ImageFeature[] features = new ImageFeature[n];
            int sizeOfdescriptor = DescriptorSize;
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

      /// <summary>
      /// Release the unmanaged resources associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvSIFTDetectorRelease(ref _ptr);
      }
   }
}
