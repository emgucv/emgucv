//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

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
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvStereoSGBMCreate(
         int minDisparity, int numDisparities, int SADWindowSize,
         int P1, int P2, int disp12MaxDiff,
         int preFilterCap, int uniquenessRatio,
         int speckleWindowSize, int speckleRange,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool fullDP);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvStereoSGBMRelease(IntPtr obj);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvStereoSGBMFindCorrespondence(IntPtr disparitySolver, IntPtr left, IntPtr right, IntPtr disparity);
      #endregion

      /// <summary>
      /// Create a stereo disparity solver using StereoSGBM algorithm (combination of H. Hirschmuller + K. Konolige approaches) 
      /// </summary>
      /// <param name="minDisparity"></param>
      /// <param name="numDisparities"></param>
      /// <param name="SADWindowSize">Set this to 0 for default</param>
      /// <param name="P1">Use 0 for default</param>
      /// <param name="P2">Use 0 for default</param>
      /// <param name="disp12MaxDiff">Use 0 for default</param>
      /// <param name="preFilterCap">Use 0 for default</param>
      /// <param name="uniquenessRatio">Use 0 for default</param>
      /// <param name="speckleWindowSize">Use 0 for default</param>
      /// <param name="speckleRange">Use 0 for default</param>
      /// <param name="fullDP">Use false for default</param>
      public StereoSGBM(int minDisparity, int numDisparities, int SADWindowSize,
         int P1, int P2, int disp12MaxDiff,
         int preFilterCap, int uniquenessRatio,
         int speckleWindowSize, int speckleRange,
         bool fullDP)
      {
         _ptr = CvStereoSGBMCreate(minDisparity, numDisparities, SADWindowSize, P1, P2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, fullDP);
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
         CvStereoSGBMFindCorrespondence(_ptr, left, right, disparity);
      }

      /// <summary>
      /// Release the unmanged memory associated with this stereo solver
      /// </summary>
      protected override void DisposeObject()
      {
         CvStereoSGBMRelease(_ptr);
      }
   }
}