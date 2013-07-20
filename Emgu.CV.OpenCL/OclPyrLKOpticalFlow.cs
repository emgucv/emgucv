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
   /// PyrLK optical flow
   /// </summary>
   public class OclPyrLKOpticalFlow : UnmanagedObject
   {
      /// <summary>
      /// Create the PyrLK optical flow solver
      /// </summary>
      /// <param name="winSize">Windows size. Use 21x21 for default</param>
      /// <param name="maxLevel">The maximum number of pyramid leveles. Use 3 for default</param>
      /// <param name="iters">The number of iterations. Use 30 for default.</param>
      /// <param name="useInitialFlow">Weather or not use the initial flow in the input matrix. Use false for default.</param>
      public OclPyrLKOpticalFlow(Size winSize, int maxLevel, int iters, bool useInitialFlow)
      {
         _ptr = OclInvoke.oclPyrLKOpticalFlowCreate(winSize, maxLevel, iters, useInitialFlow);
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
         OclInvoke.oclPyrLKOpticalFlowDense(_ptr, frame0, frame1, u, v, IntPtr.Zero);
      }

      /// <summary>
      /// Calculate an optical flow for a sparse feature set.
      /// </summary>
      /// <param name="frame0">First 8-bit input image (supports both grayscale and color images).</param>
      /// <param name="frame1">Second input image of the same size and the same type as <paramref name="frame0"/></param>
      /// <param name="points0">
      /// Vector of 2D points for which the flow needs to be found. It must be one row
      /// matrix with 2 channels
      /// </param>
      /// <param name="points1">
      /// Output vector of 2D points (with single-precision two channel floating-point coordinates)
      /// containing the calculated new positions of input features in the second image.</param>
      /// <param name="status">
      /// Output status vector (CV_8UC1 type). Each element of the vector is set to 1 if the
      /// flow for the corresponding features has been found. Otherwise, it is set to 0.
      /// </param>
      /// <param name="err">
      /// Output vector (CV_32FC1 type) that contains the difference between patches around
      /// the original and moved points or min eigen value if getMinEigenVals is checked. It can be
      /// null, if not needed.
      /// </param>
      public void Sparse(OclImage<Gray, byte> frame0, OclImage<Gray, byte> frame1, OclMat<float> points0, out OclMat<float> points1, out OclMat<Byte> status, out OclMat<float> err)
      {
         points1 = new OclMat<float>();
         status = new OclMat<byte>();
         err = new OclMat<float>();
         OclInvoke.oclPyrLKOpticalFlowSparse(_ptr, frame0, frame1, points0, points1, status, err);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this optical flow solver.
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.oclPyrLKOpticalFlowRelease(ref _ptr);
      }
   }

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr oclPyrLKOpticalFlowCreate(
         Size winSize, int maxLevel, int iters,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useInitialFlow);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclPyrLKOpticalFlowDense(IntPtr flow, IntPtr prevImg, IntPtr nextImg, IntPtr u, IntPtr v, IntPtr err);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclPyrLKOpticalFlowSparse(
         IntPtr flow,
         IntPtr prevImg,
         IntPtr nextImg,
         IntPtr prevPts,
         IntPtr nextPts,
         IntPtr status,
         IntPtr err);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void oclPyrLKOpticalFlowRelease(ref IntPtr flow);
   }
}
