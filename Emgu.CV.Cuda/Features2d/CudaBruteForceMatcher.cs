//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    public abstract class DescriptorMatcher : SharedPtrObject, IAlgorithm
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

        /// <summary>
        /// Find the k-nearest match
        /// </summary>
        /// <param name="queryDescriptors">An n x m matrix of descriptors to be query for nearest neighbors. n is the number of descriptor and m is the size of the descriptor</param>
        /// <param name="k">Number of nearest neighbors to search for</param>
        /// <param name="masks">Can be null if not needed. An n x 1 matrix. If 0, the query descriptor in the corresponding row will be ignored.</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        /// <param name="compactResult">Parameter used when the mask (or masks) is not empty. If compactResult is false, the matches vector has the same size as queryDescriptors rows. If compactResult is true, the matches vector does not contain matches for fully masked-out query descriptors.</param>
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

        /// <summary>
        /// Finds the k best matches for each descriptor from a query set (asynchronous version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
        /// <param name="matches">Matches array stored in GPU memory. Internal representation is not defined. Use DescriptorMatcher::knnMatchConvert method to retrieve results in standard representation.</param>
        /// <param name="k">Count of best matches found per each query descriptor or less if a query descriptor has less than k possible matches in total.</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="stream">CUDA stream.</param>
        public void KnnMatchAsync(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            IOutputArray matches,
            int k,
            IInputArray mask = null,
            Stream stream = null)
        {
            using (InputArray iaQueryDescriptors = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptors = trainDescriptors.GetInputArray())
            using (OutputArray oaMatches = matches.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherKnnMatchAsync1(
                    _ptr,
                    iaQueryDescriptors,
                    iaTrainDescriptors,
                    oaMatches,
                    k,
                    iaMask,
                    stream);
            }
        }

        /// <summary>
        /// Finds the k best matches for each descriptor from a query set (asynchronous version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="matches">Matches array stored in GPU memory. Internal representation is not defined. Use DescriptorMatcher::knnMatchConvert method to retrieve results in standard representation.</param>
        /// <param name="k">Count of best matches found per each query descriptor or less if a query descriptor has less than k possible matches in total.</param>
        /// <param name="masks">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="stream">CUDA stream.</param>
        public void KnnMatchAsync(
            IInputArray queryDescriptors,
            IOutputArray matches,
            int k,
            VectorOfGpuMat masks = null,
            Stream stream = null)
        {
            using (InputArray iaQueryDescriptor = queryDescriptors.GetInputArray())
            using (OutputArray oaMatches = matches.GetOutputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherKnnMatchAsync2(
                    _ptr,
                    iaQueryDescriptor,
                    oaMatches,
                    k,
                    masks == null ? IntPtr.Zero : masks.Ptr,
                    stream);
            }
        }

        /// <summary>
        /// Converts matches array from internal representation to standard matches vector.
        /// </summary>
        /// <param name="gpuMatches">Matches</param>
        /// <param name="matches">Vector of DMatch objects.</param>
        /// <param name="compactResult">Parameter used when the mask (or masks) is not empty. If compactResult is false, the matches vector has the same size as queryDescriptors rows. If compactResult is true, the matches vector does not contain matches for fully masked-out query descriptors.</param>
        public void KnnMatchConvert(
            IInputArray gpuMatches,
            VectorOfVectorOfDMatch matches,
            bool compactResult = false)
        {
            using (InputArray iaGpuMatches = gpuMatches.GetInputArray())
                CudaInvoke.cveCudaDescriptorMatcherKnnMatchConvert(
                    _ptr,
                    iaGpuMatches,
                    matches,
                    compactResult
                    );
        }

        /// <summary>
        /// Finds the best match for each descriptor from a query set (blocking version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
        /// <param name="matches">Matches. If a query descriptor is masked out in mask , no match is added for this descriptor. So, matches size may be smaller than the query descriptors count.</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        public void Match(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            VectorOfDMatch matches,
            IInputArray mask = null)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptor = trainDescriptors.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherMatch1(_ptr, iaQueryDesccriptor, iaTrainDescriptor, matches, iaMask);
            }
        }

        /// <summary>
        /// Finds the best match for each descriptor from a query set (blocking version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="matches">Matches. If a query descriptor is masked out in mask , no match is added for this descriptor. So, matches size may be smaller than the query descriptors count.</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
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
        /// Finds the best match for each descriptor from a query set (asynchronous version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
        /// <param name="matches">Matches array stored in GPU memory. Internal representation is not defined. Use DescriptorMatcher::matchConvert method to retrieve results in standard representation.</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="stream">CUDA stream.</param>
        public void MatchAsync(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            IOutputArray matches,
            IInputArray mask = null,
            Stream stream = null)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptor = trainDescriptors.GetInputArray())
            using (OutputArray oaMatches = matches.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherMatchAsync1(
                    _ptr,
                    iaQueryDesccriptor,
                    iaTrainDescriptor,
                    oaMatches,
                    iaMask,
                    stream
                    );
            }
        }

        /// <summary>
        /// Finds the best match for each descriptor from a query set (asynchronous version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="matches">Matches array stored in GPU memory. Internal representation is not defined. Use DescriptorMatcher::matchConvert method to retrieve results in standard representation.</param>
        /// <param name="masks">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="stream">CUDA stream.</param>
        public void MatchAsync(
            IInputArray queryDescriptors,
            IOutputArray matches,
            VectorOfGpuMat masks = null,
            Stream stream = null)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (OutputArray oaMatches = matches.GetOutputArray())
                CudaInvoke.cveCudaDescriptorMatcherMatchAsync2(
                    _ptr,
                    iaQueryDesccriptor,
                    oaMatches,
                    masks == null ? IntPtr.Zero : masks.Ptr,
                    stream
                    );
        }

        /// <summary>
        /// Converts matches array from internal representation to standard matches vector.
        /// </summary>
        /// <param name="gpuMatches">Matches, returned from MatchAsync.</param>
        /// <param name="matches">Vector of DMatch objects.</param>
        public void MatchConvert(
            IInputArray gpuMatches,
            VectorOfDMatch matches)
        {
            using (InputArray iaGpuMatches = gpuMatches.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherMatchConvert(
                    _ptr,
                    iaGpuMatches,
                    matches);
            }
        }

        /// <summary>
        /// For each query descriptor, finds the training descriptors not farther than the specified distance (blocking version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
        /// <param name="matches">Found matches.</param>
        /// <param name="maxDistance">Threshold for the distance between matched descriptors. Distance means here metric distance (e.g. Hamming distance), not the distance between coordinates (which is measured in Pixels)!</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="compactResult">Parameter used when the mask (or masks) is not empty. If compactResult is false, the matches vector has the same size as queryDescriptors rows. If compactResult is true, the matches vector does not contain matches for fully masked-out query descriptors.</param>
        public void RadiusMatch(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            VectorOfVectorOfDMatch matches,
            float maxDistance,
            IInputArray mask = null,
            bool compactResult = false)
        {
            using (InputArray iaQueryDescriptors = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptors = trainDescriptors.GetInputArray())
            using (InputArray iaMask = (mask == null ? InputArray.GetEmpty() : mask.GetInputArray()))
                CudaInvoke.cveCudaDescriptorMatcherRadiusMatch1(
                    _ptr,
                    iaQueryDescriptors,
                    iaTrainDescriptors,
                    matches,
                    maxDistance,
                    iaMask,
                    compactResult);
        }

        /// <summary>
        /// For each query descriptor, finds the training descriptors not farther than the specified distance (blocking version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="matches">Found matches.</param>
        /// <param name="maxDistance">Threshold for the distance between matched descriptors. Distance means here metric distance (e.g. Hamming distance), not the distance between coordinates (which is measured in Pixels)!</param>
        /// <param name="masks">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="compactResult">Parameter used when the mask (or masks) is not empty. If compactResult is false, the matches vector has the same size as queryDescriptors rows. If compactResult is true, the matches vector does not contain matches for fully masked-out query descriptors.</param>
        public void RadiusMatch(
            IInputArray queryDescriptors,
            VectorOfVectorOfDMatch matches,
            float maxDistance,
            VectorOfGpuMat masks = null,
            bool compactResult = false)
        {
            using (InputArray iaQueryDescriptors = queryDescriptors.GetInputArray())
                CudaInvoke.cveCudaDescriptorMatcherRadiusMatch2(
                    _ptr,
                    iaQueryDescriptors,
                    matches,
                    maxDistance,
                    masks == null ? IntPtr.Zero : masks.Ptr,
                    compactResult
                    );
        }

        /// <summary>
        /// For each query descriptor, finds the training descriptors not farther than the specified distance (asynchronous version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
        /// <param name="matches">Matches array stored in GPU memory. Internal representation is not defined.</param>
        /// <param name="maxDistance">Threshold for the distance between matched descriptors. Distance means here metric distance (e.g. Hamming distance), not the distance between coordinates (which is measured in Pixels)!</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="stream">CUDA stream.</param>
        public void RadiusMatchAsync(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            IOutputArray matches,
            float maxDistance,
            IInputArray mask = null,
            Stream stream = null)
        {
            using (InputArray iaQueryDescriptors = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptors = trainDescriptors.GetInputArray())
            using (OutputArray oaMatches = matches.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                CudaInvoke.cveCudaDescriptorMatcherRadiusMatchAsync1(
                    _ptr,
                    iaQueryDescriptors,
                    iaTrainDescriptors,
                    oaMatches,
                    maxDistance,
                    iaMask,
                    stream);
        }

        /// <summary>
        /// For each query descriptor, finds the training descriptors not farther than the specified distance (asynchronous version).
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="matches">Matches array stored in GPU memory. Internal representation is not defined.</param>
        /// <param name="maxDistance">Threshold for the distance between matched descriptors. Distance means here metric distance (e.g. Hamming distance), not the distance between coordinates (which is measured in Pixels)!</param>
        /// <param name="masks">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="stream">CUDA stream.</param>
        public void RadiusMatchAsync(
            IInputArray queryDescriptors,
            IOutputArray matches,
            float maxDistance,
            VectorOfGpuMat masks,
            Stream stream)
        {
            using (InputArray iaQueryDescriptor = queryDescriptors.GetInputArray())
            using (OutputArray oaMatches = matches.GetOutputArray())
                CudaInvoke.cveCudaDescriptorMatcherRadiusMatchAsync2(
                    _ptr,
                    iaQueryDescriptor,
                    oaMatches,
                    maxDistance,
                    masks == null ? IntPtr.Zero : masks.Ptr,
                    stream);
        }

        /// <summary>
        /// Converts matches array from internal representation to standard matches vector.
        /// </summary>
        /// <param name="gpuMatches">Matches, returned from DescriptorMatcher.RadiusMatchAsync.</param>
        /// <param name="matches">Vector of DMatch objects.</param>
        /// <param name="compactResult">Parameter used when the mask (or masks) is not empty. If compactResult is false, the matches vector has the same size as queryDescriptors rows. If compactResult is true, the matches vector does not contain matches for fully masked-out query descriptors.</param>
        public void RadiusMatchConvert(
            IInputArray gpuMatches,
            VectorOfVectorOfDMatch matches,
            bool compactResult)
        {
            using (InputArray iaGpuMatches = gpuMatches.GetInputArray())
            {
                CudaInvoke.cveCudaDescriptorMatcherRadiusMatchConvert(
                    _ptr,
                    iaGpuMatches,
                    matches,
                    compactResult);
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

        /// <summary>
        /// Return True if mask is supported
        /// </summary>
        public bool IsMaskSupported
        {
            get
            {
                return CudaInvoke.cveCudaDescriptorMatcherIsMaskSupported(_ptr);
            }
        }

        /// <summary>
        /// Clear the matcher
        /// </summary>
        public void Clear()
        {
            CudaInvoke.cveCudaDescriptorMatcherClear(_ptr);
        }

        /// <summary>
        /// Return True if the matcher is empty 
        /// </summary>
        public bool Empty
        {
            get
            {
                return CudaInvoke.cveCudaDescriptorMatcherEmpty(_ptr);
            }
        }

        /// <summary>
        /// Trains a descriptor matcher.
        /// </summary>
        public void Train()
        {
            CudaInvoke.cveCudaDescriptorMatcherTrain(_ptr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this matcher
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cveCudaDescriptorMatcherRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
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
            _ptr = CudaInvoke.cveCudaDescriptorMatcherCreateBFMatcher(distanceType, ref _algorithmPtr, ref _sharedPtr);
        }

    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveCudaDescriptorMatcherCreateBFMatcher(DistanceType distType, ref IntPtr algorithm, ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherRelease(ref IntPtr sharedPtr);

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
        internal extern static void cveCudaDescriptorMatcherMatchAsync1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            IntPtr mask,
            IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveCudaDescriptorMatcherMatchAsync2(
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
        internal extern static void cveCudaDescriptorMatcherKnnMatchAsync2(
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
            IntPtr gpuMatches,
            IntPtr matches,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

    }
}
