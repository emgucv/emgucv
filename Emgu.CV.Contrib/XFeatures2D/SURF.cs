//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.XFeatures2D
{
   /// <summary>
   /// Class for extracting Speeded Up Robust Features from an image
   /// </summary>
   public class SURF : Feature2D
   {

      /// <summary>
      /// Create a SURF detector using the specific values
      /// </summary>
      /// <param name="hessianThresh">      
      /// Only features with keypoint.hessian larger than that are extracted.
      /// good default value is ~300-500 (can depend on the average local contrast and sharpness of the image).
      /// user can further filter out some features based on their hessian values and other characteristics
      /// </param>
      /// <param name="extended">      
      /// false means basic descriptors (64 elements each),
      /// true means extended descriptors (128 elements each)
      /// </param>
      /// <param name="nOctaves">
      /// The number of octaves to be used for extraction.
      /// With each next octave the feature size is doubled
      /// </param>
      /// <param name="nOctaveLayers">
      /// The number of layers within each octave
      /// </param>
      /// <param name="upright">
      /// False means that detector computes orientation of each feature. 
      /// True means that the orientation is not computed (which is much, much faster). 
      /// For example, if you match images from a stereo pair, or do image stitching, the matched features likely have very similar angles, and you can speed up feature extraction by setting upright=true.</param>
      public SURF(double hessianThresh, int nOctaves = 4, int nOctaveLayers = 2, bool extended = true, bool upright = false)
      {
         _ptr = ContribInvoke.cveSURFCreate(hessianThresh, nOctaves, nOctaveLayers, extended, upright, ref _feature2D);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this detector.
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            ContribInvoke.cveSURFRelease(ref _ptr);
         base.DisposeObject();
      }
   }

   /// <summary>
   /// This class wraps the functional calls to the opencv contrib modules
   /// </summary>
   public static partial class ContribInvoke
   {
      static ContribInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveSURFCreate(
         double hessianThresh, int nOctaves, int nOctaveLayers, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool extended, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool upright, 
         ref IntPtr feature2D);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveSURFRelease(ref IntPtr detector);
   }
}