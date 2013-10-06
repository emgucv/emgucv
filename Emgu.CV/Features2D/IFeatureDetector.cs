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
   /// An interface for feature detector
   /// </summary>
   public interface IFeatureDetector
   {
      /// <summary>
      /// Get the pointer to the feature detector. 
      /// </summary>
      /// <returns>The feature detector</returns>
      IntPtr FeatureDetectorPtr { get; }
   }
}
