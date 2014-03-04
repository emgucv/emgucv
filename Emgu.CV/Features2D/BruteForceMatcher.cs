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
   public class BruteForceMatcher : DescriptorMatcher
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
         _distanceType = distanceType;
         _ptr = BruteForceMatcherInvoke.CvBruteForceMatcherCreate(_distanceType, crossCheck);
      }

      /// <summary>
      /// Release the unmanaged resource associated with the BruteForceMatcher
      /// </summary>
      protected override void DisposeObject()
      {
         BruteForceMatcherInvoke.CvBruteForceMatcherRelease(ref _ptr);
      }
   }

   internal static partial class BruteForceMatcherInvoke
   {
      static BruteForceMatcherInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr CvBruteForceMatcherCreate(
         Features2D.DistanceType distanceType,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool crossCheck);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvBruteForceMatcherRelease(ref IntPtr matcher);
   }
}
