using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// CvStarDetectorParams
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct StarDetector : IKeyPointDetector
   {
      /// <summary>
      /// Maximum size of the features. The following
      /// values of the parameter are supported:
      /// 4, 6, 8, 11, 12, 16, 22, 23, 32, 45, 46, 64, 90, 128
      /// </summary>
      public int MaxSize;
      /// <summary>
      /// Threshold for the approximated laplacian,
      /// used to eliminate weak features. The larger it is,
      /// the less features will be retrieved
      /// </summary>
      public int ResponseThreshold;
      /// <summary>
      /// Another threshold for the laplacian to
      /// eliminate edges.
      /// The larger the threshold, the more points you get.
      /// </summary>
      public int LineThresholdProjected;
      /// <summary>
      /// Another threshold for the feature
      /// size to eliminate edges. 
      /// The larger the threshold, the more points you get.
      /// </summary>
      public int LineThresholdBinarized;
      /// <summary>
      /// 
      /// </summary>
      public int SuppressNonmaxSize;

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private extern static void CvStarDetectorDetectKeyPoints(
         ref StarDetector detector,
         IntPtr image,
         IntPtr keypoints);

      /// <summary>
      /// Get the default star detector parameters
      /// </summary>
      /// <returns>The default star detector parameters</returns>
      public void SetDefaultParameters()
      {
         MaxSize = 45;
         ResponseThreshold = 30;
         LineThresholdProjected = 10;
         LineThresholdBinarized = 8;
         SuppressNonmaxSize = 5;
      }

      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <returns>The key pionts in the image</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<MKeyPoint> seq = new Seq<MKeyPoint>(stor);
            CvStarDetectorDetectKeyPoints(ref this, image, seq.Ptr);
            return seq.ToArray();
         }
      }
   }
}
