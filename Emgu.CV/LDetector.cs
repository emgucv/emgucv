using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// V. Lepetit keypoint detector
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct LDetector
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
      [MarshalAs(UnmanagedType.I1)]
      public bool Verbose;

      /// <summary>
      /// Base feature size
      /// </summary>
      public double BaseFeatureSize;
      /// <summary>
      /// Clustering Distance
      /// </summary>
      public double ClusteringDistance;

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvLDetectorDetectKeyPoints(
         ref LDetector detector,
         IntPtr image,
         IntPtr keypoints,
         int maxCount,
         [MarshalAs(UnmanagedType.I1)]
         bool scaleCoords);

      /// <summary>
      /// Set the parameters to default value
      /// </summary>
      public void SetDefaultParameters()
      {
         Radius = 7;
         Threshold = 20;
         NOctaves = 3;
         NViews = 1000;
         Verbose = false; BaseFeatureSize = 32;
         ClusteringDistance = 2;
      }

      /// <summary>
      /// Detect the Lepetit keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract Lepetit keypoints</param>
      /// <param name="maxCount">The maximum number of keypoints to be extracted</param>
      /// <param name="scaleCoords">Indicates if the coordinates should be scaled</param>
      /// <returns>The array of Lepetit keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image, int maxCount, bool scaleCoords)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<MKeyPoint> seq = new Seq<MKeyPoint>(stor);
            CvLDetectorDetectKeyPoints(ref this, image, seq.Ptr, maxCount, scaleCoords);
            return seq.ToArray();
         }
      }
   }
}
