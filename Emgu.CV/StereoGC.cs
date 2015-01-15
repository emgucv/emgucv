/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Use Graph Cut algorithm to find stereo correspondence
   /// </summary>
   public class StereoGC : DisposableObject
   {
      private IntPtr _ptr;

      /// <summary>
      /// The state structure
      /// </summary>
      public MCvStereoGCState State;

      /// <summary>
      /// Creates the stereo correspondence state and initializes it. 
      /// </summary>
      /// <param name="numberOfDisparities">The number of disparities. The disparity search range will be state.minDisparity &lt;= disparity &lt; state.minDisparity + state.numberOfDisparities</param>
      /// <param name="maxIters">Maximum number of iterations. On each iteration all possible (or reasonable) alpha-expansions are tried. The algorithm may terminate earlier if it could not find an alpha-expansion that decreases the overall cost function value</param>
      public StereoGC(
         int numberOfDisparities,
         int maxIters)
      {
         _ptr = CvInvoke.cvCreateStereoGCState(numberOfDisparities, maxIters);
         State = (MCvStereoGCState)Marshal.PtrToStructure(_ptr, typeof(MCvStereoGCState));
      }

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="leftDisparity">The optional output single-channel 16-bit signed left disparity map of the same size as input images.</param>
      /// <param name="rightDisparity">The optional output single-channel 16-bit signed right disparity map of the same size as input images</param>
      public void FindStereoCorrespondence(Image<Gray, Byte> left, Image<Gray, Byte> right, Image<Gray, Int16> leftDisparity, Image<Gray, Int16> rightDisparity)
      {
         CvInvoke.cvFindStereoCorrespondenceGC(left, right, leftDisparity, rightDisparity, ref State, 0);
      }

      /// <summary>
      /// Release the stereo state and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         Marshal.StructureToPtr(State, _ptr, false);
         CvInvoke.cvReleaseStereoGCState(ref _ptr);
      }
   }
}
*/