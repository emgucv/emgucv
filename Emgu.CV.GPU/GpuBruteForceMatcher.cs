//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Features2D;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /*
   internal enum GpuMatcherDistanceType
   {
      /// <summary>
      /// Manhattan distance (city block distance)
      /// </summary>
      L1Dist = 0,
      /// <summary>
      /// Squared Euclidean distance
      /// </summary>
      L2Dist,
      /// <summary>
      /// Hamming distance functor - counts the bit differences between two strings - useful for the Brief descriptor, 
      /// bit count of A exclusive XOR'ed with B. 
      /// </summary>
      HammingDist
   }*/

   /// <summary>
   /// A Brute force matcher using GPU
   /// </summary>
   /// <typeparam name="T">The type of data to be matched. Can be either float or Byte</typeparam>
   public class GpuBruteForceMatcher<T> : UnmanagedObject
      where T : struct
   {
      //private GpuMatcherDistanceType _distanceType;



      /// <summary>
      /// Create a GPUBruteForce Matcher using the specific distance type
      /// </summary>
      /// <param name="distanceType">The distance type</param>
      public GpuBruteForceMatcher(DistanceType distanceType)
      {
         if (distanceType == DistanceType.Hamming)
         {
            if (typeof(T) != typeof(byte))
               throw new ArgumentException("Hamming distance type requires model descriptor to be Matrix<Byte>");
         }

         if (typeof(T) != typeof(byte) && typeof(T) != typeof(float))
         {
            throw new NotImplementedException(String.Format("Data type of {0} is not supported", typeof(T).ToString()));
         }

         /*
         switch (distanceType)
         {
            case (DistanceType.Hamming):
               _distanceType = GpuMatcherDistanceType.HammingDist;
               break;
            case (DistanceType.L1):
               _distanceType = GpuMatcherDistanceType.L1Dist;
               break;
            case (DistanceType.L2):
               _distanceType = GpuMatcherDistanceType.L2Dist;
               break;
            default:
               throw new NotImplementedException(String.Format("Distance type of {0} is not implemented in GPU.", distanceType.ToString()));
         }*/
         _ptr = GpuInvoke.gpuBruteForceMatcherCreate(distanceType);
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
      /// <param name="modelIdx">The model index. A n x <paramref name="k"/> matrix where n = <paramref name="queryDescriptors"/>.Cols</param>
      /// <param name="distance">The matrix where the distance valus is stored. A n x <paramref name="k"/> matrix where n = <paramref name="queryDescriptors"/>.Size.Height</param>
      /// <param name="k">The number of nearest neighbours to be searched</param>
      /// <param name="mask">The mask</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void KnnMatchSingle(GpuMat<T> queryDescriptors, GpuMat<T> modelDescriptors, GpuMat<int> modelIdx, GpuMat<float> distance, int k, GpuMat<Byte> mask, Stream stream)
      {
         if (k == 2 && !(modelIdx.IsContinuous && distance.IsContinuous))
         {
            throw new ArgumentException("For k == 2, the allocated index matrix and distance matrix must be continuous");
         }
         GpuInvoke.gpuBruteForceMatcherKnnMatchSingle(_ptr, queryDescriptors, modelDescriptors, modelIdx, distance, k, mask, stream);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this matcher
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuBruteForceMatcherRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr gpuBruteForceMatcherCreate(DistanceType distType);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuBruteForceMatcherRelease(ref IntPtr ptr);

      //[DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      //private extern static void gpuBruteForceMatcherAdd(IntPtr matcher, IntPtr trainDescs);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void gpuBruteForceMatcherKnnMatchSingle(
         IntPtr matcher,
         IntPtr queryDescs, IntPtr trainDescs,
         IntPtr trainIdx, IntPtr distance,
         int k, IntPtr mask,
         IntPtr stream);
   }
}
