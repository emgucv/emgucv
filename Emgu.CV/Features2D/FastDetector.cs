using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// FAST(Features from Accelerated Segment Test) keypoint detector
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct FastDetector : IKeyPointDetector
   {
      /// <summary>
      /// FAST threshold
      /// </summary>
      public int Threshold;
      /// <summary>
      /// Specifiy if non-maximum supression should be used
      /// </summary>
      public bool NonmaxSupression;

      /// <summary>
      /// Create a fast detector with the specific parameters
      /// </summary>
      /// <param name="threshold">FAST threshold</param>
      /// <param name="nonmaxSupression">Specifiy if non-maximum supression should be used</param>
      public FastDetector(int threshold, bool nonmaxSupression)
      {
         Threshold = threshold;
         NonmaxSupression = nonmaxSupression;
      }

      #region IKeyPointDetector Members
      /// <summary>
      /// Detect the Fast keypoints from the image
      /// </summary>
      /// <param name="image">The image to extract keypoints from</param>
      /// <returns>The array of fast keypoints</returns>
      public MKeyPoint[] DetectKeyPoints(Image<Gray, byte> image)
      {
         using (MemStorage stor = new MemStorage())
         {
            Seq<MKeyPoint> keypoints = new Seq<MKeyPoint>(stor);
            CvInvoke.CvFASTKeyPoints(image, keypoints, Threshold, NonmaxSupression);
            return keypoints.ToArray();
         }
      }

      #endregion
   }
}
