//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
    /// <summary>
    /// Class implementing PCT (position-color-texture) signature extraction as described in:
    /// Martin Krulis, Jakub Lokoc, and Tomas Skopal. Efficient extraction of clustering-based feature signatures using GPU architectures. Multimedia Tools Appl., 75(13):8071–8103, 2016.
    /// The algorithm is divided to a feature sampler and a clusterizer. Feature sampler produces samples at given set of coordinates. Clusterizer then produces clusters of these samples using k-means algorithm. Resulting set of clusters is the signature of the input image.
    /// A signature is an array of SIGNATURE_DIMENSION-dimensional points.Used dimensions are: weight, x, y position; lab color, contrast, entropy.
    /// </summary>
    public partial class PCTSignatures : SharedPtrObject
    {

        /// <summary>
        ///Point distributions supported by random point generator.
        /// </summary>
        public enum PointDistributionType
        {
            /// <summary>
            /// Generate numbers uniformly.
            /// </summary>
            Uniform,
            /// <summary>
            /// Generate points in a regular grid.
            /// </summary>
            Regular,
            /// Generate points with normal (gaussian) distribution.
            Normal
        }

        /// <summary>
        /// Creates PCTSignatures algorithm using sample and seed count. It generates its own sets of sampling points and clusterization seed indexes.
        /// </summary>
        /// <param name="initSampleCount">Number of points used for image sampling.</param>
        /// <param name="initSeedCount">Number of initial clusterization seeds. Must be lower or equal to initSampleCount</param>
        /// <param name="pointDistribution">Distribution of generated points.</param>
        public PCTSignatures(int initSampleCount = 2000, int initSeedCount = 400, PointDistributionType pointDistribution = PointDistributionType.Uniform)
        {
            _ptr = XFeatures2DInvoke.cvePCTSignaturesCreate(initSampleCount, initSeedCount, pointDistribution, ref _sharedPtr);
        }

        /// <summary>
        /// Creates PCTSignatures algorithm using pre-generated sampling points and number of clusterization seeds. It uses the provided sampling points and generates its own clusterization seed indexes.
        /// </summary>
        /// <param name="initSamplingPoints">Sampling points used in image sampling.</param>
        /// <param name="initSeedCount">Number of initial clusterization seeds. Must be lower or equal to initSamplingPoints.size().</param>
        public PCTSignatures(VectorOfPointF initSamplingPoints, int initSeedCount)
        {
            _ptr = XFeatures2DInvoke.cvePCTSignaturesCreate2(initSamplingPoints, initSeedCount, ref _sharedPtr);
        }

        /// <summary>
        /// Creates PCTSignatures algorithm using pre-generated sampling points and clusterization seeds indexes.
        /// </summary>
        /// <param name="initSamplingPoints">Sampling points used in image sampling.</param>
        /// <param name="initClusterSeedIndexes">Indexes of initial clusterization seeds. Its size must be lower or equal to initSamplingPoints.size().</param>
        public PCTSignatures(VectorOfPointF initSamplingPoints, VectorOfInt initClusterSeedIndexes)
        {
            _ptr = XFeatures2DInvoke.cvePCTSignaturesCreate3(initSamplingPoints, initClusterSeedIndexes, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this PCTSignatures object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                XFeatures2DInvoke.cvePCTSignaturesRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Computes signature of given image.
        /// </summary>
        /// <param name="image">Input image of CV_8U type.</param>
        /// <param name="signature">Output computed signature.</param>
        public void ComputeSignature(IInputArray image, IOutputArray signature)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaSignature = signature.GetOutputArray())
            {
                XFeatures2DInvoke.cvePCTSignaturesComputeSignature(_ptr, iaImage, oaSignature);
            }
        }

        /// <summary>
        /// Draws signature in the source image and outputs the result. Signatures are visualized as a circle with radius based on signature weight and color based on signature color. Contrast and entropy are not visualized.
        /// </summary>
        /// <param name="source">Source image.</param>
        /// <param name="signature">Image signature.</param>
        /// <param name="result">Output result.</param>
        /// <param name="radiusToShorterSideRatio">Determines maximal radius of signature in the output image.</param>
        /// <param name="borderThickness">Border thickness of the visualized signature.</param>
        public static void DrawSignature(
            IInputArray source,
            IInputArray signature,
            IOutputArray result,
            float radiusToShorterSideRatio = 1.0f/8,
            int borderThickness = 1)
        {
            using (InputArray iaSource = source.GetInputArray())
            using (InputArray iaSigniture = signature.GetInputArray())
            using (OutputArray oaResult = result.GetOutputArray())
                XFeatures2DInvoke.cvePCTSignaturesDrawSignature(iaSource, iaSigniture, oaResult, radiusToShorterSideRatio, borderThickness);
        }
    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesCreate(int initSampleCount, int initSeedCount, PCTSignatures.PointDistributionType pointDistribution, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesCreate2(IntPtr initSamplingPoints, int initSeedCount, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesCreate3(IntPtr initSamplingPoints, IntPtr initClusterSeedIndexes, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesRelease(ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesComputeSignature(IntPtr pct, IntPtr image, IntPtr signature);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesDrawSignature(IntPtr source, IntPtr signature, IntPtr result, float radiusToShorterSideRatio, int borderThickness);

    }
}
