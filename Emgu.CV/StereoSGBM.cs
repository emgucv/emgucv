//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// This is a variation of
   /// "Stereo Processing by Semiglobal Matching and Mutual Information"
   /// by Heiko Hirschmuller.
   /// We match blocks rather than individual pixels, thus the algorithm is called
   /// SGBM (Semi-global block matching)
   /// </summary>
   public class StereoSGBM : UnmanagedObject
   {
      public enum Mode
      {
         SGBM = 0,
         HH = 1
      }

      /// <summary>
      /// Create a stereo disparity solver using StereoSGBM algorithm (combination of H. Hirschmuller + K. Konolige approaches) 
      /// </summary>
      /// <param name="minDisparity"></param>
      /// <param name="numDisparities"></param>
      /// <param name="blockSize">Use 0 for default</param>
      /// <param name="P1">Use 0 for default</param>
      /// <param name="P2">Use 0 for default</param>
      /// <param name="disp12MaxDiff">Use 0 for default</param>
      /// <param name="preFilterCap">Use 0 for default</param>
      /// <param name="uniquenessRatio">Use 0 for default</param>
      /// <param name="speckleWindowSize">Use 0 for default</param>
      /// <param name="speckleRange">Use 0 for default</param>
      /// <param name="mode">Use SGBM for default</param>
      public StereoSGBM(int minDisparity, int numDisparities, int blockSize,
         int P1, int P2, int disp12MaxDiff,
         int preFilterCap, int uniquenessRatio,
         int speckleWindowSize, int speckleRange,
         Mode mode)
      {
         _ptr = CvInvoke.CvStereoSGBMCreate(minDisparity, numDisparities, blockSize, P1, P2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, (int) mode);
      }

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <remarks>Invalid pixels (for which disparity can not be computed) are set to (state-&gt;minDisparity-1)*16</remarks>
      public void FindStereoCorrespondence(Image<Gray, Byte> left, Image<Gray, Byte> right, Image<Gray, Int16> disparity)
      {
         CvInvoke.CvStereoSGBMFindCorrespondence(_ptr, left, right, disparity);
      }

      /// <summary>
      /// Release the unmanged memory associated with this stereo solver
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvStereoSGBMRelease(_ptr);
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvStereoSGBMCreate(
         int minDisparity, int numDisparities, int blockSize,
         int P1, int P2, int disp12MaxDiff,
         int preFilterCap, int uniquenessRatio,
         int speckleWindowSize, int speckleRange,
         int mode);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoSGBMRelease(IntPtr obj);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvStereoSGBMFindCorrespondence(IntPtr disparitySolver, IntPtr left, IntPtr right, IntPtr disparity);
   }
}