using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Wrapped CvSURFParams structure
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvSURFParams
   {
      /// <summary>
      /// Create a MCvSURFParams using the specific values
      /// </summary>
      /// <param name="hessianThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extendedFlag">      
      /// false means basic descriptors (64 elements each),
      /// true means extended descriptors (128 elements each)
      /// </param>
      public MCvSURFParams(double hessianThresh, bool extendedFlag)
      {
         MCvSURFParams p = CvInvoke.cvSURFParams(hessianThresh, extendedFlag ? 1 : 0);
         extended = p.extended;
         hessianThreshold = p.hessianThreshold;
         nOctaves = p.nOctaves;
         nOctaveLayers = p.nOctaveLayers;
      }

      /// <summary>
      /// 0 means basic descriptors (64 elements each),
      /// 1 means extended descriptors (128 elements each)
      /// </summary>
      public int extended;

      /// <summary>
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </summary>
      public double hessianThreshold;

      /// <summary>
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled (3 by default)
      /// </summary>
      public int nOctaves;

      /// <summary>
      /// The number of layers within each octave (4 by default)
      /// </summary>
      public int nOctaveLayers;
   }
}
