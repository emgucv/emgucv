//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
    public class HistogramPhaseUnwrapping : UnmanagedObject
    {
        public HistogramPhaseUnwrapping(
            int width,
            int height,
            float histThresh,
            int nbrOfSmallBins,
            int nbrOfLargeBins)
        {
            _ptr = PhaseUnwrappingInvoke.cveHistogramPhaseUnwrappingCreate(
                width,
                height,
                histThresh,
                nbrOfSmallBins,
                nbrOfLargeBins);
        }

        /// <summary>
        /// Release the unmanaged resources assocuated with the HistogramPhaseUnwrapping
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                PhaseUnwrappingInvoke.cveHistogramPhaseUnwrappingRelease(ref _ptr);
        }

        public void GetInverseReliabilityMap(IOutputArray reliabilityMap)
        {
            using (OutputArray oaReliabilityMap = reliabilityMap.GetOutputArray())
                PhaseUnwrappingInvoke.cveHistogramPhaseUnwrappingGetInverseReliabilityMap(_ptr, oaReliabilityMap);
        }

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
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveHistogramPhaseUnwrappingCreate(
            int width,
            int height,
            float histThresh,
            int nbrOfSmallBins,
            int nbrOfLargeBins);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHistogramPhaseUnwrappingRelease(ref IntPtr phaseUnwrapping);

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