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
   /// An interface for keypoint detector
   /// </summary>
   public interface IKeyPointDetector
   {
      /// <summary>
      /// Detect the keypoints in the image
      /// </summary>
      /// <param name="image">The image from which the key point will be detected from</param>
      /// <param name="mask">The optional mask, can be null if not needed</param>
      /// <returns>The key pionts in the image</returns>
      VectorOfKeyPoint DetectKeyPointsRaw(Image<Gray, Byte> image, Image<Gray, Byte> mask);

      /// <summary>
      /// Get the pointer to the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr FeatureDetectorPtr { get; }
   }
}
