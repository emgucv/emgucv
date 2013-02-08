//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   /// <typeparam name="TColor">The type of color the descriptor extractor operates on</typeparam>
   /// <typeparam name="TDescriptor">The type of data in the descriptor. Can be either float or byte</typeparam>
   public interface IDescriptorExtractor<TColor, TDescriptor>
      where TColor : struct, IColor
      where TDescriptor : struct
   {
      /*
      /// <summary>
      /// Compute the descriptors on the image from the given keypoint locations.
      /// </summary>
      /// <param name="image">The image to compute descriptors from</param>
      /// <param name="keyPoints">The keypoints where the descriptor computation is perfromed</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The descriptors from the given keypoints</returns>
      Matrix<TDescriptor> ComputeDescriptorsRaw(Image<Gray, Byte> image, Image<Gray, Byte> mask, VectorOfKeyPoint keyPoints);
      */
      /// <summary>
      /// Get the pointer to the descriptor extractor. 
      /// </summary>
      /// <returns>The descriptor extractor</returns>
      IntPtr DescriptorExtratorPtr { get; }

      /*
      /// <summary>
      /// Get the size of the descriptor.
      /// </summary>
      int DescriptorSize { get; }*/
   }
}
