using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// FAST(Features from Accelerated Segment Test) keypoint detector
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct FastDetector : IKeyPointDetector
   {
      #region PInvoke
      /// <summary>
      /// Extract FAST keypoints
      /// </summary>
      /// <param name="image">The image to extract keypoint from</param>
      /// <param name="KeyPointSeq">The pre-allocated sequence of MKeyPoints where the result will be stored</param>
      /// <param name="threshold"></param>
      /// <param name="nonmaxSupression">Indicates if nonmaximum supression should be used</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvFASTKeyPoints(
         IntPtr image,
         IntPtr KeyPointSeq,
         int threshold,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool nonmaxSupression);
      #endregion

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
         using (VectorOfKeyPoint keypoints = new VectorOfKeyPoint())
         {
            CvFASTKeyPoints(image, keypoints, Threshold, NonmaxSupression);
            return keypoints.ToArray();
         }
      }

      #endregion
   }
}
