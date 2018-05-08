//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
        public void KnnMatch(
            IInputArray queryDescriptors, 
            IInputArray trainDescriptors, 
            VectorOfVectorOfDMatch matches, 
            int k, 
            IInputArray mask = null, 
            bool compactResult = false)
        {
            using (InputArray iaQueryDescriptors = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptors = trainDescriptors.GetInputArray())
            using (InputArray iaMask = (mask == null ? InputArray.GetEmpty() : mask.GetInputArray()))
                CudaInvoke.cveCudaDescriptorMatcherKnnMatch1(_ptr, iaQueryDescriptors, iaTrainDescriptors, matches, k, iaMask, compactResult);
        }

        public void KnnMatch(
            IInputArray queryDescriptors,
            VectorOfVectorOfDMatch matches,
            int k,
            VectorOfGpuMat masks = null,
            bool compactResult = false)
        {
            using (InputArray iaQueryDescriptors = queryDescriptors.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherKnnMatch2(
                    _ptr,
                    iaQueryDescriptors,
                    matches,
                    k,
                    masks == null ? IntPtr.Zero : masks.Ptr,
                    compactResult);
            }
        }

        public void Match(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            VectorOfDMatch matches,
            IInputArray mask)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptor = trainDescriptors.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherMatch1(_ptr, iaQueryDesccriptor, iaTrainDescriptor, matches, iaMask);
            }
        }

        public void Match(
            IInputArray queryDescriptors,
            VectorOfDMatch matches,
            VectorOfGpuMat mask = null)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherMatch2(_ptr, iaQueryDesccriptor, matches, mask == null ? IntPtr.Zero : mask.Ptr);
            }
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


        public bool IsMaskSupported
        {
            get
            {
                return CudaInvoke.cveCudaDescriptorMatcherIsMaskSupported(_ptr);
            }
        }

        public void Clear()
        {
            CudaInvoke.cveCudaDescriptorMatcherClear(_ptr);
        }

        public bool Empty
        {
            get
            {
                return CudaInvoke.cveCudaDescriptorMatcherEmpty(_ptr);
            }
        }

        public void Train()
        {
            CudaInvoke.cveCudaDescriptorMatcherTrain(_ptr);
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
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveCudaDescriptorMatcherIsMaskSupported(IntPtr matcher);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherClear(IntPtr matcher);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveCudaDescriptorMatcherEmpty(IntPtr matcher);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherTrain(IntPtr matcher);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherMatch1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            IntPtr mask);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherMatch2(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            IntPtr masks);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherMatchAsync(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            IntPtr masks,
            IntPtr stream);
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherMatchConvert(
            IntPtr matcher,
            IntPtr gpuMatches,
            IntPtr matches);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherKnnMatch1(
            IntPtr matcher,
            IntPtr queryDescs,
            IntPtr trainDescs,
            IntPtr matches,
            int k,
            IntPtr masks,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherKnnMatch2(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            int k,
            IntPtr masks,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherKnnMatchAsync1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            int k,
            IntPtr mask,
            IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherknnMatchAsync2(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            int k,
            IntPtr masks,
            IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherKnnMatchConvert(
            IntPtr matcher,
            IntPtr gpuMatches,
            IntPtr matches,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherRadiusMatch1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            float maxDistance,
            IntPtr mask,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherRadiusMatch2(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            float maxDistance,
            IntPtr masks,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherRadiusMatchAsync1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            float maxDistance,
            IntPtr mask,
            IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherRadiusMatchAsync2(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            float maxDistance,
            IntPtr masks,
            IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherRadiusMatchConvert(
            IntPtr matcher,
            IntPtr gpu_matches,
            IntPtr matches,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

    }
}
