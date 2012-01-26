//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// Use Block Matching algorithm to find stereo correspondence
   /// </summary>
   public class StereoBM : DisposableObject
   {
      private IntPtr _ptr;

      /// <summary>
      /// The state structure
      /// </summary>
      public MCvStereoBMState State;

      /// <summary>
      /// Create a stereoBMState
      /// </summary>
      /// <param name="type">ID of one of the pre-defined parameter sets. Any of the parameters can be overridden after creating the structure.</param>
      /// <param name="numberOfDisparities">The number of disparities. If the parameter is 0, it is taken from the preset, otherwise the supplied value overrides the one from preset. </param>
      public StereoBM(CvEnum.STEREO_BM_TYPE type, int numberOfDisparities)
      {
         _ptr = CvInvoke.cvCreateStereoBMState(type, numberOfDisparities);
         State = (MCvStereoBMState) Marshal.PtrToStructure(_ptr, typeof(MCvStereoBMState));
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
         CvInvoke.cvFindStereoCorrespondenceBM(left, right, disparity, ref State);
      }

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The output single-channel 16-bit signed disparity map of the same size as input images. Its elements will be the computed disparities, multiplied by 16 and rounded to integer's</param>
      /// <remarks>Invalid pixels (for which disparity can not be computed) are set to state-&gt;minDisparity - 1 </remarks>
      public void FindStereoCorrespondence(Image<Gray, Byte> left, Image<Gray, Byte> right, Image<Gray, float> disparity)
      {
         CvInvoke.cvFindStereoCorrespondenceBM(left, right, disparity, ref State);
      }

      /// <summary>
      /// Release the stereo state and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         Marshal.StructureToPtr(State, _ptr, false);
         CvInvoke.cvReleaseStereoBMState(ref _ptr);
      }
   }
}
