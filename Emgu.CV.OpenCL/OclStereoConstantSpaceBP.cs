//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// A Constant-Space Belief Propagation Algorithm for Stereo Matching.
   /// Qingxiong Yang, Liang Wang, Narendra Ahuja.
   /// http://vision.ai.uiuc.edu/~qyang6/
   /// </summary>
   public class OclStereoConstantSpaceBP : UnmanagedObject
   {
      /// <summary>
      /// A Constant-Space Belief Propagation Algorithm for Stereo Matching
      /// </summary>
      /// <param name="ndisp">The number of disparities. Use 128 as default</param>
      /// <param name="iters">The number of BP iterations on each level. Use 8 as default.</param>
      /// <param name="levels">The number of levels. Use 4 as default</param>
      /// <param name="nrPlane">The number of active disparity on the first level. Use 4 as default.</param>
      public OclStereoConstantSpaceBP(int ndisp, int iters, int levels, int nrPlane)
      {
         _ptr = OclInvoke.oclStereoConstantSpaceBPCreate(ndisp, iters, levels, nrPlane);
      }

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The disparity map</param>
      public void FindStereoCorrespondence(OclImage<Gray, Byte> left, OclImage<Gray, Byte> right, OclImage<Gray, Byte> disparity)
      {
         OclInvoke.oclStereoConstantSpaceBPFindStereoCorrespondence(_ptr, left, right, disparity);
      }

      /// <summary>
      /// Release the unmanaged memory
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.oclStereoConstantSpaceBPRelease(ref _ptr);
      }
   }

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclStereoConstantSpaceBPFindStereoCorrespondence(IntPtr stereoBM, IntPtr left, IntPtr right, IntPtr disparity);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclStereoConstantSpaceBPRelease(ref IntPtr stereoBM);
   }
}
