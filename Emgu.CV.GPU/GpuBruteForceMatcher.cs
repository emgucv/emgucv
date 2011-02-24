using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.GPU
{
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
         L1 = 0,
         L2
      }

      public GpuBruteForceMatcher(DistanceType distType)
      {
         _ptr = gpuBruteForceMatcherCreate(distType);
      }

      public void KnnMatch(GpuMat<float> queryDescriptors, GpuMat<float> modelDescriptors, GpuMat<int> modelIdx, GpuMat<float> distance, int k, GpuMat<Byte> mask)
      {
         gpuBruteForceMatcherKnnMatch(_ptr, queryDescriptors, modelDescriptors, modelIdx, distance, k, mask);
      }

      protected override void DisposeObject()
      {
         gpuBruteForceMatcherRelease(ref _ptr);
      }
   }
}
