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
      /// <summary>
      /// The SGBM mode
      /// </summary>
      public enum Mode
      {
         /// <summary>
         /// This is the defult mode, the algorithm is single-pass, which means that you consider only 5 directions instead of 8
         /// </summary>
         SGBM = 0,
         /// <summary>
         /// Run the full-scale two-pass dynamic programming algorithm. It will consume O(W*H*numDisparities) bytes, which is large for 640x480 stereo and huge for HD-size pictures.
         /// </summary>
         HH = 1
      }

      /// <summary>
      /// Create a stereo disparity solver using StereoSGBM algorithm (combination of H. Hirschmuller + K. Konolige approaches) 
      /// </summary>
      /// <param name="minDisparity">Minimum possible disparity value. Normally, it is zero but sometimes rectification algorithms can shift images, so this parameter needs to be adjusted accordingly.</param>
      /// <param name="numDisparities">Maximum disparity minus minimum disparity. The value is always greater than zero. In the current implementation, this parameter must be divisible by 16.</param>
      /// <param name="blockSize">Matched block size. It must be an odd number &gt;=1 . Normally, it should be somewhere in the 3..11 range. Use 0 for default. </param>
      /// <param name="p1">The first parameter controlling the disparity smoothness. It is the penalty on the disparity change by plus or minus 1 between neighbor pixels. Reasonably good value is 8*number_of_image_channels*SADWindowSize*SADWindowSize. Use 0 for default</param>
      /// <param name="p2">The second parameter controlling the disparity smoothness. It is the penalty on the disparity change by more than 1 between neighbor pixels. The algorithm requires <paramref name="p2"/> &gt; <paramref name="p1"/>. Reasonably good value is 32*number_of_image_channels*SADWindowSize*SADWindowSize. Use 0 for default</param>
      /// <param name="disp12MaxDiff">Use 0 for default</param>
      /// <param name="preFilterCap">Use 0 for default</param>
      /// <param name="uniquenessRatio">Use 0 for default</param>
      /// <param name="speckleWindowSize">Use 0 for default</param>
      /// <param name="speckleRange">Use 0 for default</param>
      /// <param name="mode">Use SGBM for default</param>
      public StereoSGBM(int minDisparity, int numDisparities, int blockSize,
         int p1, int p2, int disp12MaxDiff,
         int preFilterCap, int uniquenessRatio,
         int speckleWindowSize, int speckleRange,
         Mode mode)
      {
         _ptr = CvInvoke.CvStereoSGBMCreate(minDisparity, numDisparities, blockSize, p1, p2, disp12MaxDiff, preFilterCap, uniquenessRatio, speckleWindowSize, speckleRange, (int) mode);
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