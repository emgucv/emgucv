//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

 using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
 using Emgu.CV.Util;
 using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// A Brute force matcher using Cuda
   /// </summary>
   public class CudaBruteForceMatcher : UnmanagedObject
   {
      /// <summary>
      /// Create a CudaBruteForceMatcher using the specific distance type
      /// </summary>
      /// <param name="distanceType">The distance type</param>
      public CudaBruteForceMatcher(DistanceType distanceType)
      {
         _ptr = CudaInvoke.cudaBruteForceMatcherCreate(distanceType);
      }

      /*
      /// <summary>
      /// Add the model descriptors
      /// </summary>
      /// <param name="modelDescriptors">The model discriptors</param>
      public void Add(Matrix<Byte> modelDescriptors)
      {
         if (!(_distanceType == DistanceType.HammingDist))
            throw new ArgumentException("Hamming distance type requires model descriptor to be Matrix<Byte>");
         gpuBruteForceMatcherAdd(_ptr, modelDescriptors);
      }

      /// <summary>
      /// Add the model descriptors
      /// </summary>
      /// <param name="modelDescriptors">The model discriptors</param>
      public void Add(Matrix<float> modelDescriptors)
      {
         if (!(_distanceType == DistanceType.L2 || _distanceType == DistanceType.L1))
            throw new ArgumentException("L1 / L2 distance type requires model descriptor to be Matrix<float>");
         gpuBruteForceMatcherAdd(_ptr, modelDescriptors);
      }*/

      /// <summary>
      /// Find the k nearest neighbour using the brute force matcher. 
      /// </summary>
      /// <param name="queryDescriptors">The query descriptors</param>
      /// <param name="modelDescriptors">The model descriptors</param>
      
      /// <param name="k">The number of nearest neighbours to be searched</param>
      /// <param name="mask">The mask</param>
      
      public void KnnMatch(GpuMat queryDescriptors, GpuMat modelDescriptors, VectorOfVectorOfDMatch matches, int k, GpuMat mask = null, bool compactResult = false)
      {
         CudaInvoke.cudaBruteForceMatcherKnnMatch(_ptr, queryDescriptors, modelDescriptors, matches, k, mask, compactResult);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this matcher
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaBruteForceMatcherRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cudaBruteForceMatcherCreate(DistanceType distType);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaBruteForceMatcherRelease(ref IntPtr ptr);

      //[DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      //private extern static void gpuBruteForceMatcherAdd(IntPtr matcher, IntPtr trainDescs);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cudaBruteForceMatcherKnnMatch(
         IntPtr matcher,
         IntPtr queryDescs, IntPtr trainDescs,
         IntPtr matches,
         int k, IntPtr mask,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool compactResult);
   }
}
