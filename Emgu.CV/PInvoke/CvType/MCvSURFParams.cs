//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
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
      /// Create CvSURFParams with the specific value
      /// </summary>
      /// <param name="hessianThreshold">Only features with keypoint.hessian larger than this are extracted.</param>
      /// <param name="extended">
      /// False means basic descriptors (64 elements each),
      /// True means extended descriptors (128 elements each)
      ///</param>
      ///<param name="nOctaves">
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled
      ///</param>
      ///<param name="nOctaveLayers">
      /// The number of layers within each octave
      /// </param>
      public MCvSURFParams(double hessianThreshold, bool extended, int nOctaves, int nOctaveLayers)
      {
         HessianThreshold = hessianThreshold;
         Extended = extended;
         Upright = false;
         NOctaves = nOctaves;
         NOctaveLayers = nOctaveLayers;
      }

      /// <summary>
      /// Creage CvSURFParams with the specific value
      /// </summary>
      /// <param name="hessianThreshold">Only features with keypoint.hessian larger than this are extracted.</param>
      /// <param name="extended">
      /// False means basic descriptors (64 elements each),
      /// True means extended descriptors (128 elements each)
      ///</param>
      public MCvSURFParams(double hessianThreshold, bool extended)
         : this(hessianThreshold, extended, 4, 2)
      {
      }

      /// <summary>
      /// False means basic descriptors (64 elements each),
      /// True means extended descriptors (128 elements each)
      /// </summary>
      public bool Extended;

      /// <summary>
      /// Upright SURF
      /// </summary>
      public bool Upright;

      /// <summary>
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </summary>
      public double HessianThreshold;

      /// <summary>
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled
      /// </summary>
      public int NOctaves;

      /// <summary>
      /// The number of layers within each octave
      /// </summary>
      public int NOctaveLayers;
   }
}
