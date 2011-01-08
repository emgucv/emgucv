using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;

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

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvStarDetectorDetectKeyPoints(
         ref StarDetector detector,
         IntPtr image,
         IntPtr keypoints);

      /// <summary>
      /// Get the default star detector parameters
      /// </summary>
      /// <returns>The default star detector parameters</returns>
      public void Init()
      {
         MaxSize = 45;
         ResponseThreshold = 30;
         LineThresholdProjected = 10;
         LineThresholdBinarized = 8;
         SuppressNonmaxSize = 5;
      }

      /// <summary>
      /// Create a star detector with the specific parameters
      /// </summary>
      /// <param name="maxSize">
      /// Use 45 as default. Maximum size of the features. The following
      /// values of the parameter are supported:
      /// 4, 6, 8, 11, 12, 16, 22, 23, 32, 45, 46, 64, 90, 128</param>
      /// <param name="responseThreshold">
      /// Use 30 as default. Threshold for the approximated laplacian,
      /// used to eliminate weak features. The larger it is,
      /// the less features will be retrieved
      /// </param>
      /// <param name="lineThresholdProjected">
      /// Use 10 as default. Another threshold for the laplacian to eliminate edges.
      /// The larger the threshold, the more points you get.
      /// </param>
      /// <param name="lineThresholdBinarized">
      /// Use 8 as default. Another threshold for the feature size to eliminate edges. 
      /// The larger the threshold, the more points you get.</param>
      /// <param name="suppressNonmaxSize">
      /// Use 5 as default.
      /// </param>
      public StarDetector(int maxSize, int responseThreshold, int lineThresholdProjected, int lineThresholdBinarized, int suppressNonmaxSize)
      {
         MaxSize = maxSize;
         ResponseThreshold = responseThreshold;
         LineThresholdProjected = lineThresholdProjected;
         LineThresholdBinarized = lineThresholdBinarized;
         SuppressNonmaxSize = suppressNonmaxSize;

      }

      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <returns>The key pionts in the image</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image)
      {
         using (VectorOfKeyPoint kpts = new VectorOfKeyPoint())
         {
            CvStarDetectorDetectKeyPoints(ref this, image, kpts);
            return kpts.ToArray();
         }
      }
   }
}
