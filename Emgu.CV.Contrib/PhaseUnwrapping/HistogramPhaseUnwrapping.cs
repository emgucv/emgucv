//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.PhaseUnwrapping
{
    /// <summary>
    /// Class implementing two-dimensional phase unwrapping.
    /// </summary>
    /// <remarks>This algorithm belongs to the quality-guided phase unwrapping methods. First, it computes a reliability map from second differences between a pixel and its eight neighbours. Reliability values lie between 0 and 16*pi*pi. Then, this reliability map is used to compute the reliabilities of "edges". An edge is an entity defined by two pixels that are connected horizontally or vertically. Its reliability is found by adding the reliabilities of the two pixels connected through it. Edges are sorted in a histogram based on their reliability values. This histogram is then used to unwrap pixels, starting from the highest quality pixel. </remarks>
    public class HistogramPhaseUnwrapping : UnmanagedObject
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a HistogramPhaseUnwrapping instance
        /// </summary>
        /// <param name="width">Phase map width.</param>
        /// <param name="height">Phase map height.</param>
        /// <param name="histThresh">Bins in the histogram are not of equal size. Default value is 3*pi*pi. The one before "histThresh" value are smaller.</param>
        /// <param name="nbrOfSmallBins">Number of bins between 0 and "histThresh". Default value is 10.</param>
        /// <param name="nbrOfLargeBins">Number of bins between "histThresh" and 32*pi*pi (highest edge reliability value). Default value is 5.</param>
        public HistogramPhaseUnwrapping(
            int width = 800,
            int height = 600,
            float histThresh = (float)(3 * Math.PI * Math.PI),
            int nbrOfSmallBins = 10,
            int nbrOfLargeBins = 5)
        {
            _ptr = PhaseUnwrappingInvoke.cveHistogramPhaseUnwrappingCreate(
                width,
                height,
                histThresh,
                nbrOfSmallBins,
                nbrOfLargeBins,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with the HistogramPhaseUnwrapping
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                PhaseUnwrappingInvoke.cveHistogramPhaseUnwrappingRelease(ref _ptr, ref _sharedPtr);
        }

        /// <summary>
        /// Get the reliability map computed from the wrapped phase map.
        /// </summary>
        /// <param name="reliabilityMap">Image where the reliability map is stored.</param>
        public void GetInverseReliabilityMap(IOutputArray reliabilityMap)
        {
            using (OutputArray oaReliabilityMap = reliabilityMap.GetOutputArray())
                PhaseUnwrappingInvoke.cveHistogramPhaseUnwrappingGetInverseReliabilityMap(_ptr, oaReliabilityMap);
        }

        /// <summary>
        /// Unwraps a 2D phase map.
        /// </summary>
        /// <param name="wrappedPhaseMap">The wrapped phase map that needs to be unwrapped.</param>
        /// <param name="unwrappedPhaseMap">The unwrapped phase map.</param>
        /// <param name="shadowMask">Optional parameter used when some pixels do not hold any phase information in the wrapped phase map.</param>
        public void UnwrapPhaseMap(
            IInputArray wrappedPhaseMap,
            IOutputArray unwrappedPhaseMap,
            IInputArray shadowMask = null)
        {
            using (InputArray iaWrappedPhaseMap = wrappedPhaseMap.GetInputArray())
            using (OutputArray oaUnwrappedPhaseMap = unwrappedPhaseMap.GetOutputArray())
            using (InputArray iaShadowMask = shadowMask == null ? InputArray.GetEmpty() : shadowMask.GetInputArray())
            {
                PhaseUnwrappingInvoke.cveHistogramPhaseMapUnwrappingUnwrapPhaseMap(_ptr, iaWrappedPhaseMap, oaUnwrappedPhaseMap, iaShadowMask);
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV PhaseUnwrapping functions
    /// </summary>
    public static partial class PhaseUnwrappingInvoke
    {
        static PhaseUnwrappingInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveHistogramPhaseUnwrappingCreate(
            int width,
            int height,
            float histThresh,
            int nbrOfSmallBins,
            int nbrOfLargeBins,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHistogramPhaseUnwrappingRelease(ref IntPtr phaseUnwrapping, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHistogramPhaseUnwrappingGetInverseReliabilityMap(
            IntPtr phaseUnwrapping,
            IntPtr reliabilityMap);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHistogramPhaseMapUnwrappingUnwrapPhaseMap(
            IntPtr phaseUnwrapping,
            IntPtr wrappedPhaseMap,
            IntPtr unwrappedPhaseMap,
            IntPtr shadowMask);

    }
}