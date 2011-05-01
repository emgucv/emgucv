//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// CvSURFParams
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSURFParams
   {
      /// <summary>
      /// 0 means basic descriptors (64 elements each),
      /// 1 means extended descriptors (128 elements each)
      /// </summary>
      public int Extended;

      /// <summary>
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </summary>
      public double HessianThreshold;

      /// <summary>
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled (3 by default)
      /// </summary>
      public int NOctaves;

      /// <summary>
      /// The number of layers within each octave (4 by default)
      /// </summary>
      public int NOctaveLayers;
   }
}
