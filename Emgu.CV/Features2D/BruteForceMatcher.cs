//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Wrapped BruteForceMatcher
   /// </summary>
   public class BruteForceMatcher : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr CvBruteForceMatcherCreate(DistanceType distanceType);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void CvBruteForceMatcherRelease(ref IntPtr matcher, DistanceType distanceType);
      #endregion

      private UnmanagedObject _modelDescriptors;

      /// <summary>
      /// The match distance type
      /// </summary>
      public enum DistanceType
      {
         /// <summary>
         /// Manhattan distance (city block distance) on float
         /// </summary>
         L1F32 = 0,
         /// <summary>
         /// Squared Euclidean distance on float
         /// </summary>
         L2F32 = 1,
         /// <summary>
         /// Hamming distance functor - counts the bit differences between two strings - useful for the Brief descriptor, 
         /// bit count of A exclusive XOR'ed with B
         /// </summary>
         HammingLUT = 2,
         /// <summary>
         /// Hamming distance functor, this one will try to use gcc's __builtin_popcountl
         /// but will fall back on HammingLUT if not available
         /// bit count of A exclusive XOR'ed with B
         /// </summary>
         Hamming = 3
      }

      /// <summary>
      /// Find the k-nearest match for DistanceType of L1F32 or L2F32
      /// </summary>
      /// <param name="queryDescriptor">An n x m matrix of descriptors to be query for nearest neighbours. n is the number of descriptor and m is the size of the descriptor</param>
      /// <param name="trainIdx">The resulting n x <paramref name="k"/> matrix of descriptor index from the training descriptors</param>
      /// <param name="distance">The resulting n x <paramref name="k"/> matrix of distance value from the training descriptors</param>
      /// <param name="k">Number of nearest neighbors to search for</param>
      /// <param name="mask">Can be null if not needed. An n x 1 matrix. If 0, the query descriptor in the corresponding row will be ignored.</param>
      public void KnnMatch(Matrix<float> queryDescriptor, Matrix<int> trainIdx, Matrix<float> distance, int k, Matrix<Byte> mask)
      {
         Debug.Assert(_distanceType == DistanceType.L1F32 || _distanceType == DistanceType.L2F32);
         DescriptorMatcherInvoke.CvDescriptorMatcherKnnMatch(Ptr, queryDescriptor, trainIdx, distance, k, mask);
      }

      /// <summary>
      /// Find the k-nearest match for DistanceType of Hamming or HammingLUT
      /// </summary>
      /// <param name="queryDescriptor">An n x m matrix of descriptors to be query for nearest neighbours. n is the number of descriptor and m is the size of the descriptor</param>
      /// <param name="trainIdx">The resulting n x <paramref name="k"/> matrix of descriptor index from the training descriptors</param>
      /// <param name="distance">The resulting n x <paramref name="k"/> matrix of distance value from the training descriptors</param>
      /// <param name="k">Number of nearest neighbors to search for</param>
      /// <param name="mask">Can be null if not needed. An n x 1 matrix. If 0, the query descriptor in the corresponding row will be ignored.</param>
      public void KnnMatch(Matrix<Byte> queryDescriptor, Matrix<int> trainIdx, Matrix<float> distance, int k, Matrix<Byte> mask)
      {
         Debug.Assert(_distanceType == DistanceType.Hamming || _distanceType == DistanceType.HammingLUT);
         DescriptorMatcherInvoke.CvDescriptorMatcherKnnMatch(Ptr, queryDescriptor, trainIdx, distance, k, mask);
      }

      private DistanceType _distanceType;

      private void Init(DistanceType distanceType, UnmanagedObject modelDescriptors)
      {
         _distanceType = distanceType;
         _ptr = CvBruteForceMatcherCreate(_distanceType);
         _modelDescriptors = modelDescriptors; //keep a reference to the model descriptors
         DescriptorMatcherInvoke.CvDescriptorMatcherAdd(Ptr, modelDescriptors);
      }

      /// <summary>
      /// Create a BruteForceMatcher with the specific distance type and model descriptors
      /// </summary>
      /// <param name="distanceType">The distance type</param>
      /// <param name="modelDescriptors">The model discriptor</param>
      public BruteForceMatcher(DistanceType distanceType, Matrix<float> modelDescriptors)
      {
         if (!(distanceType == DistanceType.L2F32 || distanceType == DistanceType.L1F32))
            throw new ArgumentException("L1 / L2 distance type requires model descriptor to be Matrix<float>");

         Init(distanceType, modelDescriptors);
      }

      /// <summary>
      /// Create a BruteForceMatcher with the specific distance type and model descriptors
      /// </summary>
      /// <param name="distanceType">The distance type</param>
      /// <param name="modelDescriptors">The model discriptor</param>
      public BruteForceMatcher(DistanceType distanceType, Matrix<Byte> modelDescriptors)
      {
         if (!(distanceType == DistanceType.Hamming || distanceType == DistanceType.HammingLUT))
            throw new ArgumentException("Hamming distance type requires model descriptor to be Matrix<Byte>");

         Init(distanceType, modelDescriptors);
      }

      /// <summary>
      /// Release the unmanaged resource associated with the BruteForceMatcher
      /// </summary>
      protected override void DisposeObject()
      {
         CvBruteForceMatcherRelease(ref _ptr, _distanceType);
      }
   }

   internal static class DescriptorMatcherInvoke
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvDescriptorMatcherAdd(IntPtr matcher, IntPtr trainDescriptor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void CvDescriptorMatcherKnnMatch(IntPtr matcher, IntPtr queryDescriptors,
                   IntPtr trainIdx, IntPtr distance, int k,
                   IntPtr mask);
      #endregion
   }
}
