using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// An interface for a descriptor generator
   /// </summary>
   public interface IDescriptorExtractor
   {
      /// <summary>
      /// Compute the ImageFeature on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <returns>The ImageFeature from the given keypoints</returns>
      ImageFeature[] ComputeDescriptors(Image<Gray, Byte> image, MKeyPoint[] keyPoints);
   }
}
