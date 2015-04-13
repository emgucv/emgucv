//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// A Constant-Space Belief Propagation Algorithm for Stereo Matching.
   /// Qingxiong Yang, Liang Wang, Narendra Ahuja.
   /// http://vision.ai.uiuc.edu/~qyang6/
   /// </summary>
   public class CudaStereoConstantSpaceBP : UnmanagedObject
   {
      /// <summary>
      /// A Constant-Space Belief Propagation Algorithm for Stereo Matching
      /// </summary>
      /// <param name="ndisp">The number of disparities. Use 128 as default</param>
      /// <param name="iters">The number of BP iterations on each level. Use 8 as default.</param>
      /// <param name="levels">The number of levels. Use 4 as default</param>
      /// <param name="nrPlane">The number of active disparity on the first level. Use 4 as default.</param>
      public CudaStereoConstantSpaceBP(int ndisp = 128, int iters = 8, int levels = 4, int nrPlane = 4)
      {
         _ptr = CudaInvoke.cudaStereoConstantSpaceBPCreate(ndisp, iters, levels, nrPlane);
      }

      /// <summary>
      /// Computes disparity map for the input rectified stereo pair.
      /// </summary>
      /// <param name="left">The left single-channel, 8-bit image</param>
      /// <param name="right">The right image of the same size and the same type</param>
      /// <param name="disparity">The disparity map</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void FindStereoCorrespondence(IInputArray left, IInputArray right, IOutputArray disparity, Stream stream = null)
      {
         using (InputArray iaLeft = left.GetInputArray())
         using (InputArray iaRight = right.GetInputArray())
         using (OutputArray oaDisparity = disparity.GetOutputArray())
            CudaInvoke.cudaStereoConstantSpaceBPFindStereoCorrespondence(_ptr, iaLeft, iaRight, oaDisparity, stream);
      }

      /// <summary>
      /// Release the unmanaged memory
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaStereoConstantSpaceBPRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaStereoConstantSpaceBPCreate(int ndisp, int iters, int levels, int nr_plane);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaStereoConstantSpaceBPFindStereoCorrespondence(IntPtr stereoBM, IntPtr left, IntPtr right, IntPtr disparity, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaStereoConstantSpaceBPRelease(ref IntPtr stereoBM);
   }
}
