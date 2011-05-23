//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// An interface for a descriptor generator
   /// </summary>
   public interface IDescriptorExtractor
   {
      /// <summary>
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The descriptors from the given keypoints</returns>
      Matrix<float> ComputeDescriptorsRaw(Image<Gray, Byte> image, Image<Gray, Byte> mask, VectorOfKeyPoint keyPoints);

      /// <summary>
      /// Get the pointer to the descriptor extractor. 
      /// </summary>
      /// <returns>The descriptor extractor</returns>
      IntPtr DescriptorExtratorPtr { get; }
   }
}
