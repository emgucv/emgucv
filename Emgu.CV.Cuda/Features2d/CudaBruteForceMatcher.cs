//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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
   /// Descriptor matcher
   /// </summary>
   public abstract class DescriptorMatcher : UnmanagedObject, IAlgorithm
   {
      /// <summary>
      /// Pointer to the native cv::Algorithm
      /// </summary>
      protected IntPtr _algorithmPtr;

      /// <summary>
      /// Find the k-nearest match
      /// </summary>
      /// <param name="queryDescriptors">An n x m matrix of descriptors to be query for nearest neighbors. n is the number of descriptor and m is the size of the descriptor</param>
      /// <param name="k">Number of nearest neighbors to search for</param>
      /// <param name="mask">Can be null if not needed. An n x 1 matrix. If 0, the query descriptor in the corresponding row will be ignored.</param>
      /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
      /// <param name="compactResult">Parameter used when the mask (or masks) is not empty. If compactResult is false, the matches vector has the same size as queryDescriptors rows. If compactResult is true, the matches vector does not contain matches for fully masked-out query descriptors.</param>
      /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
      public void KnnMatch(IInputArray queryDescriptors, IInputArray trainDescriptors, VectorOfVectorOfDMatch matches, int k, IInputArray mask = null, bool compactResult = false)
      {
         using (InputArray iaQueryDescriptors = queryDescriptors.GetInputArray())
         using (InputArray iaTrainDescriptors = trainDescriptors.GetInputArray() )
         using (InputArray iaMask = (mask == null ? InputArray.GetEmpty() : mask.GetInputArray()))
            CudaInvoke.cveCudaDescriptorMatcherKnnMatch(_ptr, iaQueryDescriptors, iaTrainDescriptors, matches, k, iaMask, compactResult);
      }

      /// <summary>
      /// Add the model descriptors
      /// </summary>
      /// <param name="modelDescriptors">The model descriptors</param>
      public void Add(IInputArray modelDescriptors)
      {
         using (InputArray iaModelDescriptors = modelDescriptors.GetInputArray())
           CudaInvoke.cveCudaDescriptorMatcherAdd(_ptr, iaModelDescriptors);
      }

      IntPtr IAlgorithm.AlgorithmPtr
      {
         get { return _algorithmPtr; }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this matcher
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cveCudaDescriptorMatcherRelease(ref _ptr);
      }
   }


   /// <summary>
   /// A Brute force matcher using Cuda
   /// </summary>
   public class CudaBFMatcher : DescriptorMatcher
   {
      /// <summary>
      /// Create a CudaBruteForceMatcher using the specific distance type
      /// </summary>
      /// <param name="distanceType">The distance type</param>
      public CudaBFMatcher(DistanceType distanceType)
      {
         _ptr = CudaInvoke.cveCudaDescriptorMatcherCreateBFMatcher(distanceType, ref _algorithmPtr);
      }
      
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveCudaDescriptorMatcherCreateBFMatcher(DistanceType distType, ref IntPtr algorithm);


      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCudaDescriptorMatcherRelease(ref IntPtr ptr);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCudaDescriptorMatcherAdd(IntPtr matcher, IntPtr trainDescs);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveCudaDescriptorMatcherKnnMatch(
         IntPtr matcher,
         IntPtr queryDescs, IntPtr trainDescs,
         IntPtr matches, 
         int k, IntPtr masks, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool compactResult);

         
   }
}
