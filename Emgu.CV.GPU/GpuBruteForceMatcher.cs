using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// A Brute force matcher using GPU
   /// </summary>
   public class GpuBruteForceMatcher : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr gpuBruteForceMatcherCreate(DistanceType distType);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuBruteForceMatcherRelease(ref IntPtr ptr);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuBruteForceMatcherKnnMatch(
         IntPtr matcher,
         IntPtr queryDescs, IntPtr trainDescs,
         IntPtr trainIdx, IntPtr distance,
         int k, IntPtr mask);
      #endregion

      /// <summary>
      /// The distance type
      /// </summary>
      public enum DistanceType
      {
         /// <summary>
         /// Sum of absolute difference
         /// </summary>
         L1 = 0,
         /// <summary>
         /// Euclidean distance
         /// </summary>
         L2
      }

      /// <summary>
      /// Create a GPUBruteForce Matcher using the specific distance type
      /// </summary>
      /// <param name="distType">The distance type</param>
      public GpuBruteForceMatcher(DistanceType distType)
      {
         _ptr = gpuBruteForceMatcherCreate(distType);
      }

      /// <summary>
      /// Find the k nearest neighbour using the brute force matcher
      /// </summary>
      /// <param name="queryDescriptors">The query descriptors</param>
      /// <param name="modelDescriptors">The model descriptors</param>
      /// <param name="modelIdx">The model index. A n x <paramref name="k"/> matrix where n = <paramref name="queryDescriptors"/>.Cols</param>
      /// <param name="distance">The matrix where the distance valus is stored. A n x <paramref name="k"/> matrix where n = <paramref name="queryDescriptors"/>.Size.Height</param>
      /// <param name="k">The number of nearest neighbours to be searched</param>
      /// <param name="mask">The mask</param>
      public void KnnMatch(GpuMat<float> queryDescriptors, GpuMat<float> modelDescriptors, GpuMat<int> modelIdx, GpuMat<float> distance, int k, GpuMat<Byte> mask)
      {
         gpuBruteForceMatcherKnnMatch(_ptr, queryDescriptors, modelDescriptors, modelIdx, distance, k, mask);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this matcher
      /// </summary>
      protected override void DisposeObject()
      {
         gpuBruteForceMatcherRelease(ref _ptr);
      }
   }
}
