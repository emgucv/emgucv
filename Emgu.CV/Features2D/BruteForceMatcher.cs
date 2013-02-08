//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// The match distance type
   /// </summary>
   public enum DistanceType
   {
      /// <summary>
      /// 
      /// </summary>
      Inf = 1,
      /// <summary>
      /// Manhattan distance (city block distance)
      /// </summary>
      L1 = 2,
      /// <summary>
      /// Squared Euclidean distance
      /// </summary>
      L2 = 4, 
      /// <summary>
      /// Euclidean distance
      /// </summary>
      L2Sqr = 5,
      /// <summary>
      /// Hamming distance functor - counts the bit differences between two strings - useful for the Brief descriptor, 
      /// bit count of A exclusive XOR'ed with B. 
      /// </summary>
      Hamming = 6,
      /// <summary>
      /// Hamming distance functor - counts the bit differences between two strings - useful for the Brief descriptor, 
      /// bit count of A exclusive XOR'ed with B. 
      /// </summary>
      Hamming2 = 7, //TODO: update the documentation
      /*
      TypeMask = 7, 
      Relative = 8, 
      MinMax = 32 */
   }

   /// <summary>
   /// Wrapped BruteForceMatcher
   /// </summary>
   /// <typeparam name="T">The type of data to be matched. Can be either float or Byte</typeparam>
   public class BruteForceMatcher<T> : DescriptorMatcher<T>
      where T : struct
   {
      private DistanceType _distanceType;

      /// <summary>
      /// Create a BruteForceMatcher of the specific distance type, without cross check.
      /// </summary>
      /// <param name="distanceType">The distance type</param>
      public BruteForceMatcher(DistanceType distanceType)
         : this (distanceType, false)
      {
      }

      /// <summary>
      /// Create a BruteForceMatcher of the specific distance type
      /// </summary>
      /// <param name="distanceType">The distance type</param>
      /// <param name="crossCheck">Specify whether or not cross check is needed. Use false for default.</param>
      public BruteForceMatcher(DistanceType distanceType, bool crossCheck)
      {
         if (distanceType == DistanceType.Hamming || distanceType == DistanceType.Hamming2)
         { 
            if (typeof(T) != typeof(byte))
               throw new ArgumentException("Hamming distance type requires model descriptor to be Matrix<Byte>");
         }

         if (typeof(T) != typeof(byte) && typeof(T) != typeof(float))
         {
            throw new NotImplementedException(String.Format("Data type of {0} is not supported", typeof(T).ToString()));
         }

         _distanceType = distanceType;
         _ptr = CvInvoke.CvBruteForceMatcherCreate(_distanceType, crossCheck);
      }

      /// <summary>
      /// Release the unmanaged resource associated with the BruteForceMatcher
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.CvBruteForceMatcherRelease(ref _ptr);
      }
   }
}

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBruteForceMatcherCreate(
         Features2D.DistanceType distanceType,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool crossCheck);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBruteForceMatcherRelease(ref IntPtr matcher);
   }
}