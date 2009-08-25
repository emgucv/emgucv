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
      public int Radius;
      public int Threshold ;
      public int NOctaves ;
      public int NViews ;
      [MarshalAs(UnmanagedType.I1)]
      public bool Verbose ;

      public double BaseFeatureSize ;
      public double ClusteringDistance ;

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
