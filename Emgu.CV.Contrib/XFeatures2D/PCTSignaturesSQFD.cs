//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// Class implementing Signature Quadratic Form Distance (SQFD).
    /// </summary>
    /// <remarks>See also: Christian Beecks, Merih Seran Uysal, Thomas Seidl. Signature quadratic form distance. In Proceedings of the ACM International Conference on Image and Video Retrieval, pages 438-445. ACM, 2010.</remarks>
    public partial class PCTSignaturesSQFD : SharedPtrObject
    {
        /// <summary>
        /// Lp distance function selector.       
        /// </summary>
        public enum DistanceFunction
        {
            /// <summary>
            /// L0_25
            /// </summary>
            L0_25,
            /// <summary>
            /// L0_5
            /// </summary>
            L0_5,
            /// <summary>
            /// L1
            /// </summary>
            L1,
            /// <summary>
            /// L2
            /// </summary>
            L2,
            /// <summary>
            /// L2 squared
            /// </summary>
            L2Squared,
            /// <summary>
            /// L5
            /// </summary>
            L5,
            /// <summary>
            /// L infinity
            /// </summary>
            LInfinity
        }

        /// <summary>
        ///Similarity function selector.
        /// </summary>
        public enum SimilarityFunction
        {
            /// <summary>
            /// -d(c_i, c_j)
            /// </summary>
            Minus,
            /// <summary>
            /// e^{ -alpha * d^2(c_i, c_j)}
            /// </summary>
            Gaussian,
            /// <summary>
            /// 1 / (alpha + d(c_i, c_j))
            /// </summary>
            Heuristic
        }

        /// <summary>
        /// Creates the algorithm instance using selected distance function, similarity function and similarity function parameter.
        /// </summary>
        /// <param name="distanceFunction">Distance function selector.</param>
        /// <param name="similarityFunction">Similarity function selector.</param>
        /// <param name="similarityParameter">Parameter of the similarity function.</param>
        public PCTSignaturesSQFD(
            DistanceFunction distanceFunction = DistanceFunction.L2,
            SimilarityFunction similarityFunction = SimilarityFunction.Heuristic,
            float similarityParameter = 1.0f)
        {
            _ptr = XFeatures2DInvoke.cvePCTSignaturesSQFDCreate(distanceFunction, similarityFunction, similarityParameter, ref _sharedPtr);
        }

        /// <summary>
        /// Computes Signature Quadratic Form Distance of two signatures.
        /// </summary>
        /// <param name="signature0">The first signature.</param>
        /// <param name="signature1">The second signature.</param>
        /// <returns>The Signature Quadratic Form Distance of two signatures</returns>
        public float ComputeQuadraticFormDistance(IInputArray signature0, IInputArray signature1)
        {
            using (InputArray iaSignature0 = signature0.GetInputArray())
            using (InputArray iaSignature1 = signature1.GetInputArray())
                return XFeatures2DInvoke.cvePCTSignaturesSQFDComputeQuadraticFormDistance(_ptr, iaSignature0, iaSignature1);
        }

        /// <summary>
        /// Computes Signature Quadratic Form Distance between the reference signature and each of the other image signatures.
        /// </summary>
        /// <param name="sourceSignature">The signature to measure distance of other signatures from.</param>
        /// <param name="imageSignatures">Vector of signatures to measure distance from the source signature.</param>
        /// <param name="distances">Output vector of measured distances.</param>
        public void ComputeQuadraticFormDistances(
            Mat sourceSignature,
            VectorOfMat imageSignatures,
            VectorOfFloat distances)
        {
            XFeatures2DInvoke.cvePCTSignaturesSQFDComputeQuadraticFormDistances(_ptr, sourceSignature, imageSignatures,
                distances);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this PCTSignaturesSQFD object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XFeatures2DInvoke.cvePCTSignaturesSQFDRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesSQFDCreate(
            PCTSignaturesSQFD.DistanceFunction distanceFunction,
            PCTSignaturesSQFD.SimilarityFunction similarityFunction,
            float similarityParameter, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static float cvePCTSignaturesSQFDComputeQuadraticFormDistance(
            IntPtr sqfd,
            IntPtr signature0,
            IntPtr signature1);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesSQFDComputeQuadraticFormDistances(
            IntPtr sqfd,
            IntPtr sourceSignature,
            IntPtr imageSignatures,
            IntPtr distances);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesSQFDRelease(ref IntPtr sharedPtr);
    }
}
