//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
   /// <summary>
   /// Descriptor matcher
   /// </summary>
   public abstract class DescriptorMatcher : UnmanagedObject
   {
      /// <summary>
      /// Find the k-nearest match
      /// </summary>
      /// <param name="queryDescriptor">An n x m matrix of descriptors to be query for nearest neighbours. n is the number of descriptor and m is the size of the descriptor</param>
      /// <param name="k">Number of nearest neighbors to search for</param>
      /// <param name="mask">Can be null if not needed. An n x 1 matrix. If 0, the query descriptor in the corresponding row will be ignored.</param>
      public void KnnMatch(IInputArray queryDescriptor, VectorOfVectorOfDMatch matches, int k, IInputArray mask)
      {
         using (InputArray iaQueryDesccriptor = queryDescriptor.GetInputArray())
         using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            DescriptorMatcherInvoke.CvDescriptorMatcherKnnMatch(Ptr, iaQueryDesccriptor, matches, k, iaMask);
      }

      /// <summary>
      /// Add the model descriptors
      /// </summary>
      /// <param name="modelDescriptors">The model discriptors</param>
      public void Add(IInputArray modelDescriptors)
      {
         using (InputArray iaModelDescriptors = modelDescriptors.GetInputArray())
            DescriptorMatcherInvoke.CvDescriptorMatcherAdd(_ptr, iaModelDescriptors);
      }
   }

   internal static partial class DescriptorMatcherInvoke
   {
      static DescriptorMatcherInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvDescriptorMatcherAdd(IntPtr matcher, IntPtr trainDescriptor);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void CvDescriptorMatcherKnnMatch(IntPtr matcher, IntPtr queryDescriptors,
                   IntPtr matches, int k,
                   IntPtr mask);
   }
}
