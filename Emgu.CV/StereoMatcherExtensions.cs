//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV
{
   public static class StereoMatcherExtensions
   {
      static StereoMatcherExtensions()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      public static void Compute(this IStereoMatcher matcher, IInputArray left, IInputArray right, IOutputArray disparity)
      {
         CvStereoMatcherCompute(matcher.StereoMatcherPtr, left.InputArrayPtr, right.InputArrayPtr, disparity.OutputArrayPtr);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoMatcherCompute(IntPtr disparitySolver, IntPtr left, IntPtr right, IntPtr disparity);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoMatcherRelease(ref IntPtr matcher);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvStereoBMCreate(int numberOfDisparities, int blockSize);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvStereoSGBMCreate(
         int minDisparity, int numDisparities, int blockSize,
         int P1, int P2, int disp12MaxDiff,
         int preFilterCap, int uniquenessRatio,
         int speckleWindowSize, int speckleRange,
         StereoSGBM.Mode mode, ref IntPtr stereoMatcher);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoSGBMRelease(ref IntPtr obj);
   }
}
