using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// An interface for keypoint detector
   /// </summary>
   public interface IKeyPointDetector
   {
      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <returns>The key pionts in the image</returns>
      MKeyPoint[] DetectKeyPoints(Image<Gray, Byte> image);
   }
}
