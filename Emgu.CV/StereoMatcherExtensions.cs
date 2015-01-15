//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// Extension methods for StereoMather
   /// </summary>
   public static class StereoMatcherExtensions
   {
      static StereoMatcherExtensions()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Computes disparity map for the specified stereo pair
      /// </summary>
      /// <param name="matcher">The stereo matcher</param>
      /// <param name="left">Left 8-bit single-channel image.</param>
      /// <param name="right">Right image of the same size and the same type as the left one.</param>
      /// <param name="disparity">Output disparity map. It has the same size as the input images. Some algorithms, like StereoBM or StereoSGBM compute 16-bit fixed-point disparity map (where each disparity value has 4 fractional bits), whereas other algorithms output 32-bit floating-point disparity map</param>
      public static void Compute(this IStereoMatcher matcher, IInputArray left, IInputArray right, IOutputArray disparity)
      {
         using (InputArray iaLeft = left.GetInputArray())
         using (InputArray iaRight = right.GetInputArray())
         using (OutputArray oaDisparity = disparity.GetOutputArray())
            CvStereoMatcherCompute(matcher.StereoMatcherPtr, iaLeft, iaRight, oaDisparity);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoMatcherCompute(IntPtr disparitySolver, IntPtr left, IntPtr right, IntPtr disparity);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoMatcherRelease(ref IntPtr matcher);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvStereoBMCreate(int numberOfDisparities, int blockSize);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvStereoSGBMCreate(
         int minDisparity, int numDisparities, int blockSize,
         int P1, int P2, int disp12MaxDiff,
         int preFilterCap, int uniquenessRatio,
         int speckleWindowSize, int speckleRange,
         StereoSGBM.Mode mode, ref IntPtr stereoMatcher);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoSGBMRelease(ref IntPtr obj);
   }
}
