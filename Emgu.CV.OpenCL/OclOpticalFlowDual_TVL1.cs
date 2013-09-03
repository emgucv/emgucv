//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// Dual TV L1 Optical Flow Algorithm.
   /// </summary>
   public class OclOpticalFlowDual_TVL1 : UnmanagedObject
   {
      /// <summary>
      /// Create the Dual TV L1 optical flow solver
      /// </summary>
      public OclOpticalFlowDual_TVL1()
      {
         _ptr = OclInvoke.oclOpticalFlowDualTVL1Create();
      }

      /// <summary>
      /// Compute the dense optical flow.
      /// </summary>
      /// <param name="frame0">Source frame</param>
      /// <param name="frame1">Frame to track (with the same size as <paramref name="frame0"/>)</param>
      /// <param name="u">Flow horizontal component (along x axis)</param>
      /// <param name="v">Flow vertical component (along y axis)</param>
      public void Dense(OclImage<Gray, byte> frame0, OclImage<Gray, byte> frame1, OclImage<Gray, float> u, OclImage<Gray, float> v)
      {
         OclInvoke.oclOpticalFlowDualTVL1Compute(_ptr, frame0, frame1, u, v);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.oclOpticalFlowDualTVL1Release(ref _ptr);
      }
   }

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr oclOpticalFlowDualTVL1Create();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclOpticalFlowDualTVL1Compute(IntPtr flow, IntPtr prevImg, IntPtr nextImg, IntPtr u, IntPtr v);


      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclOpticalFlowDualTVL1Release(ref IntPtr flow);
   }
}
