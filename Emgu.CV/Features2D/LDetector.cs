using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// V. Lepetit keypoint detector
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct LDetector : IKeyPointDetector
   {
      /// <summary>
      /// Radius
      /// </summary>
      public int Radius;
      /// <summary>
      /// Threshold
      /// </summary>
      public int Threshold;
      /// <summary>
      /// Number of Octaves
      /// </summary>
      public int NOctaves;
      /// <summary>
      /// Number of views
      /// </summary>
      public int NViews;

      /// <summary>
      /// Verbose
      /// </summary>
      [MarshalAs(CvInvoke.BoolMarshalType)]
      public bool Verbose;

      /// <summary>
      /// Base feature size
      /// </summary>
      public double BaseFeatureSize;
      /// <summary>
      /// Clustering Distance
      /// </summary>
      public double ClusteringDistance;

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvLDetectorDetectKeyPoints(
         ref LDetector detector,
         IntPtr image,
         IntPtr keypoints,
         int maxCount,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool scaleCoords);

      /// <summary>
      /// Set the parameters to default value
      /// </summary>
      public void Init()
      {
         Radius = 7;
         Threshold = 20;
         NOctaves = 3;
         NViews = 1000;
         Verbose = false;
         BaseFeatureSize = 32;
         ClusteringDistance = 2;
      }

      /// <summary>
      /// Detect the Lepetit keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract Lepetit keypoints</param>
      /// <param name="maxCount">The maximum number of keypoints to be extracted, use 0 to ignore the max count</param>
      /// <param name="scaleCoords">Indicates if the coordinates should be scaled</param>
      /// <returns>The array of Lepetit keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image, int maxCount, bool scaleCoords)
      {
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            CvLDetectorDetectKeyPoints(ref this, image, kpts, maxCount, scaleCoords);
            return kpts.ToArray();
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
         return DetectKeyPoints(image, 0, false);
      }

      #endregion
   }
}
