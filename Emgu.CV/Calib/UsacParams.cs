//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Sampling method for USAC
    /// </summary>
    public enum SamplingMethod
    {
        /// <summary>
        /// Uniform sampling
        /// </summary>
        Uniform = 0,
        /// <summary>
        /// Progressive NAPSAC sampling
        /// </summary>
        ProgressiveNapsac = 1,
        /// <summary>
        /// NAPSAC sampling
        /// </summary>
        Napsac = 2,
        /// <summary>
        /// PROSAC sampling
        /// </summary>
        Prosac = 3
    }

    /// <summary>
    /// Local optimization method for USAC
    /// </summary>
    public enum LocalOptimMethod
    {
        /// <summary>
        /// No local optimization
        /// </summary>
        Null = 0,
        /// <summary>
        /// Inner local optimization
        /// </summary>
        InnerLo = 1,
        /// <summary>
        /// Inner and iterative local optimization
        /// </summary>
        InnerAndIterLo = 2,
        /// <summary>
        /// Graph cut local optimization
        /// </summary>
        Gc = 3,
        /// <summary>
        /// Sigma consensus local optimization
        /// </summary>
        Sigma = 4
    }

    /// <summary>
    /// Score method for USAC
    /// </summary>
    public enum ScoreMethod
    {
        /// <summary>
        /// RANSAC score
        /// </summary>
        Ransac = 0,
        /// <summary>
        /// MSAC score
        /// </summary>
        Msac = 1,
        /// <summary>
        /// MAGSAC score
        /// </summary>
        Magsac = 2,
        /// <summary>
        /// LMedS score
        /// </summary>
        Lmeds = 3
    }

    /// <summary>
    /// Neighbor search method for USAC
    /// </summary>
    public enum NeighborSearchMethod
    {
        /// <summary>
        /// FLANN KNN search
        /// </summary>
        FlannKnn = 0,
        /// <summary>
        /// Grid search
        /// </summary>
        Grid = 1,
        /// <summary>
        /// FLANN radius search
        /// </summary>
        FlannRadius = 2
    }

    /// <summary>
    /// Polishing method for USAC
    /// </summary>
    public enum PolishingMethod
    {
        /// <summary>
        /// No polisher
        /// </summary>
        NonePolisher = 0,
        /// <summary>
        /// Least squares polisher
        /// </summary>
        LsqPolisher = 1,
        /// <summary>
        /// MAGSAC polisher
        /// </summary>
        Magsac = 2,
        /// <summary>
        /// Covariance polisher
        /// </summary>
        CovPolisher = 3
    }

    /// <summary>
    /// USAC parameters, used by the USAC versions of SolvePnPRansac and other robust estimators.
    /// The layout matches cv::UsacParams.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct UsacParams
    {
        /// <summary>
        /// The confidence level
        /// </summary>
        public double Confidence;
        /// <summary>
        /// If true, the estimation runs in parallel
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool IsParallel;
        /// <summary>
        /// The number of local optimization iterations
        /// </summary>
        public int LoIterations;
        /// <summary>
        /// The local optimization method
        /// </summary>
        public LocalOptimMethod LoMethod;
        /// <summary>
        /// The local optimization sample size
        /// </summary>
        public int LoSampleSize;
        /// <summary>
        /// The maximum number of iterations
        /// </summary>
        public int MaxIterations;
        /// <summary>
        /// The neighbor search method
        /// </summary>
        public NeighborSearchMethod NeighborsSearch;
        /// <summary>
        /// The state of the random generator
        /// </summary>
        public int RandomGeneratorState;
        /// <summary>
        /// The sampling method
        /// </summary>
        public SamplingMethod Sampler;
        /// <summary>
        /// The score method
        /// </summary>
        public ScoreMethod Score;
        /// <summary>
        /// The inlier threshold
        /// </summary>
        public double Threshold;
        /// <summary>
        /// The final polishing method
        /// </summary>
        public PolishingMethod FinalPolisher;
        /// <summary>
        /// The number of final polisher iterations
        /// </summary>
        public int FinalPolisherIterations;

        /// <summary>
        /// Get the UsacParams with the default values
        /// </summary>
        /// <returns>The UsacParams with the default values</returns>
        public static UsacParams GetDefault()
        {
            UsacParams p = new UsacParams();
            CvInvoke.cveUsacParamsGetDefault(ref p);
            return p;
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveUsacParamsGetDefault(ref UsacParams usacParams);
    }
}
