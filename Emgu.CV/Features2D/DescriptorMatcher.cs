//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    public abstract class DescriptorMatcher : UnmanagedObject, IAlgorithm
    {
        /// <summary>
        /// The pointer to the Descriptor matcher
        /// </summary>
        protected IntPtr _descriptorMatcherPtr;

        /// <summary>
        /// Finds the k best matches for each descriptor from a query set.
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        /// <param name="k">Count of best matches found per each query descriptor or less if a query descriptor has less than k possible matches in total.</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        /// <param name="compactResult">Parameter used when the mask (or masks) is not empty. If compactResult is false, the matches vector has the same size as queryDescriptors rows. If compactResult is true, the matches vector does not contain matches for fully masked-out query descriptors.</param>
        public void KnnMatch(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            VectorOfVectorOfDMatch matches,
            int k,
            IInputArray mask = null,
            bool compactResult = false)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptot = trainDescriptors.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                Features2DInvoke.cveDescriptorMatcherKnnMatch1(
                    _descriptorMatcherPtr, 
                    iaQueryDesccriptor, 
                    iaTrainDescriptot, 
                    matches,
                    k,
                    iaMask,
                    compactResult);
            }
        }

        /// <summary>
        /// Find the k-nearest match
        /// </summary>
        /// <param name="queryDescriptor">An n x m matrix of descriptors to be query for nearest neighbours. n is the number of descriptor and m is the size of the descriptor</param>
        /// <param name="k">Number of nearest neighbors to search for</param>
        /// <param name="mask">Can be null if not needed. An n x 1 matrix. If 0, the query descriptor in the corresponding row will be ignored.</param>
        /// <param name="matches">Matches. Each matches[i] is k or less matches for the same query descriptor.</param>
        /// <param name="compactResult">
        /// Parameter used when the mask (or masks) is not empty. If compactResult is
        /// false, the matches vector has the same size as queryDescriptors rows.If compactResult is true,
        /// the matches vector does not contain matches for fully masked-out query descriptors.
        /// </param>
        public void KnnMatch(
            IInputArray queryDescriptor, 
            VectorOfVectorOfDMatch matches, 
            int k, 
            IInputArray mask = null, 
            bool compactResult = false)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptor.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                Features2DInvoke.cveDescriptorMatcherKnnMatch2(_descriptorMatcherPtr, iaQueryDesccriptor, matches, k, iaMask, compactResult);
        }

        /// <summary>
        /// Add the model descriptors
        /// </summary>
        /// <param name="modelDescriptors">The model descriptors</param>
        public void Add(IInputArray modelDescriptors)
        {
            using (InputArray iaModelDescriptors = modelDescriptors.GetInputArray())
                Features2DInvoke.cveDescriptorMatcherAdd(_descriptorMatcherPtr, iaModelDescriptors);
        }

        IntPtr IAlgorithm.AlgorithmPtr
        {
            get { return Features2DInvoke.cveDescriptorMatcherGetAlgorithm(_ptr); }
        }

        /// <summary>
        /// Reset the native pointer upon object disposal
        /// </summary>
        protected override void DisposeObject()
        {
            _descriptorMatcherPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Clears the train descriptor collections.
        /// </summary>
        public void Clear()
        {
            Features2DInvoke.cveDescriptorMatcherClear(_descriptorMatcherPtr);
        }

        /// <summary>
        /// Returns true if there are no train descriptors in the both collections.
        /// </summary>
        public bool Empty
        {
            get
            {
                return Features2DInvoke.cveDescriptorMatcherEmpty(_descriptorMatcherPtr);
            }
        }

        /// <summary>
        /// Returns true if the descriptor matcher supports masking permissible matches.
        /// </summary>
        public bool IsMaskSupported
        {
            get
            {
                return Features2DInvoke.cveDescriptorMatcherIsMaskSupported(_descriptorMatcherPtr);
            }
        }

        /// <summary>
        /// Trains a descriptor matcher (for example, the flann index). In all methods to match, the method
        /// train() is run every time before matching.Some descriptor matchers(for example, BruteForceMatcher)
        /// have an empty implementation of this method.Other matchers really train their inner structures (for
        /// example, FlannBasedMatcher trains flann::Index ).
        /// </summary>
        public void Train()
        {
            Features2DInvoke.cveDescriptorMatcherTrain(_descriptorMatcherPtr);
        }

        /// <summary>
        /// Finds the best match for each descriptor from a query set.
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="trainDescriptors">Train set of descriptors. This set is not added to the train descriptors collection stored in the class object.</param>
        /// <param name="matches">If a query descriptor is masked out in mask , no match is added for this descriptor. So, matches size may be smaller than the query descriptors count.</param>
        /// <param name="mask">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        public void Match(
            IInputArray queryDescriptors,
            IInputArray trainDescriptors,
            VectorOfDMatch matches,
            IInputArray mask = null)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using(InputArray iaTrainDescriptor = trainDescriptors.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                Features2DInvoke.cveDescriptorMatcherMatch1(_descriptorMatcherPtr, iaQueryDesccriptor, iaTrainDescriptor, matches, iaMask);
            }
        }

        /// <summary>
        /// Finds the best match for each descriptor from a query set. Train descriptors collection that was set by the Add function is used.
        /// </summary>
        /// <param name="queryDescriptors">Query set of descriptors.</param>
        /// <param name="matches">If a query descriptor is masked out in mask , no match is added for this descriptor. So, matches size may be smaller than the query descriptors count.</param>
        /// <param name="masks">Mask specifying permissible matches between an input query and train matrices of descriptors.</param>
        public void Match(
            IInputArray queryDescriptors,
            VectorOfDMatch matches,
            IInputArrayOfArrays masks = null
            )
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (InputArray iaMasks = masks == null ? InputArray.GetEmpty() : masks.GetInputArray())
            {
                Features2DInvoke.cveDescriptorMatcherMatch2(_descriptorMatcherPtr, iaQueryDesccriptor, matches, iaMasks);
            }
        }

        /// <summary>
        /// For each query descriptor, finds the training descriptors not farther than the specified distance.
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
            IInputArrayOfArrays mask = null,
            bool compactResult = false)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (InputArray iaTrainDescriptot = trainDescriptors.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                Features2DInvoke.cveDescriptorMatcherRadiusMatch1(_descriptorMatcherPtr, iaQueryDesccriptor, iaTrainDescriptot, matches, maxDistance, iaMask, compactResult);
            }
        }

        /// <summary>
        /// For each query descriptor, finds the training descriptors not farther than the specified distance.
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
            IInputArray masks = null,
            bool compactResult = false)
        {
            using (InputArray iaQueryDesccriptor = queryDescriptors.GetInputArray())
            using (InputArray iaMasks = masks == null ? InputArray.GetEmpty() : masks.GetInputArray())
            {
                Features2DInvoke.cveDescriptorMatcherRadiusMatch2(_descriptorMatcherPtr, iaQueryDesccriptor, matches, maxDistance, iaMasks, compactResult);
            }
        }
    }

    public static partial class Features2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherAdd(IntPtr matcher, IntPtr trainDescriptor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherKnnMatch1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            int k,
            IntPtr mask,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherKnnMatch2(
            IntPtr matcher, 
            IntPtr queryDescriptors,
            IntPtr matches, 
            int k,
            IntPtr mask,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveDescriptorMatcherGetAlgorithm(IntPtr matcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherClear(IntPtr matcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveDescriptorMatcherEmpty(IntPtr matcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveDescriptorMatcherIsMaskSupported(IntPtr matcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherTrain(IntPtr matcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherMatch1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            IntPtr mask);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherMatch2(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            IntPtr masks);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherRadiusMatch1(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr trainDescriptors,
            IntPtr matches,
            float maxDistance,
            IntPtr mask,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveDescriptorMatcherRadiusMatch2(
            IntPtr matcher,
            IntPtr queryDescriptors,
            IntPtr matches,
            float maxDistance,
            IntPtr masks,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool compactResult);
    }
}
