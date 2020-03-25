//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Base class for a seam estimator.
    /// </summary>
    public abstract class SeamFinder : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native SeamFinder object.
        /// </summary>
        protected IntPtr _seamFinderPtr;

        /// <summary>
        /// Get the pointer to the native SeamFinder object.
        /// </summary>
        public IntPtr SeamFinderPtr
        {
            get { return _seamFinderPtr; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_seamFinderPtr != IntPtr.Zero)
                _seamFinderPtr = IntPtr.Zero;
        }
    }


    /// <summary>
    /// Stub seam estimator which does nothing.
    /// </summary>
    public class NoSeamFinder : SeamFinder
    {
        /// <summary>
        /// Create a stub seam estimator which does nothing.
        /// </summary>
        public NoSeamFinder()
        {
            _ptr = StitchingInvoke.cveNoSeamFinderCreate(ref _seamFinderPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this seam finder
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveNoSeamFinderRelease(ref _ptr);
            }
        }
    }

    /*
    public class PairwiseSeamFinder : SeamFinder
    {

        public PairwiseSeamFinder()
        {
            _ptr = StitchingInvoke.cvePairwiseSeamFinderCreate(ref _seamFinderPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this seam finder
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cvePairwiseSeamFinderRelease(ref _ptr);
            }
        }
    }
    */

    /// <summary>
    /// Voronoi diagram-based seam estimator.
    /// </summary>
    public class VoronoiSeamFinder : SeamFinder
    {
        /// <summary>
        /// Create a new Voronoi diagram-based seam estimator
        /// </summary>
        public VoronoiSeamFinder()
        {
            _ptr = StitchingInvoke.cveVoronoiSeamFinderCreate(ref _seamFinderPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this seam finder
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveVoronoiSeamFinderRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Dp Seam Finder
    /// </summary>
    public class DpSeamFinder : SeamFinder
    {
        /// <summary>
        /// The cost function
        /// </summary>
        public enum CostFunction
        {
            /// <summary>
            /// Color
            /// </summary>
            Color, 
            /// <summary>
            /// Color Grad
            /// </summary>
            ColorGrad
        }

        /// <summary>
        /// Create a new DP Seam Finder
        /// </summary>
        /// <param name="costFunc">The cost function</param>
        public DpSeamFinder(CostFunction costFunc= CostFunction.Color)
        {
            _ptr = StitchingInvoke.cveDpSeamFinderCreate(costFunc, ref _seamFinderPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this seam finder
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDpSeamFinderRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Minimum graph cut-based seam estimator.
    /// </summary>
    public class GraphCutSeamFinder : SeamFinder
    {
        /// <summary>
        /// The cost function
        /// </summary>
        public enum CostFunction
        {
            /// <summary>
            /// The color
            /// </summary>
            Color,
            /// <summary>
            /// The color grad
            /// </summary>
            ColorGrad
        }

        /// <summary>
        /// Create a new minimum graph cut-based seam estimator.
        /// </summary>
        /// <param name="costFunc">The cost function</param>
        /// <param name="terminalCost">The terminal cost</param>
        /// <param name="badRegionPenalty">Bad Region penalty</param>
        public GraphCutSeamFinder(
            CostFunction costFunc = CostFunction.Color,
            float terminalCost = 1.0f,
            float badRegionPenalty = 1.0f)
        {
            _ptr = StitchingInvoke.cveGraphCutSeamFinderCreate(costFunc, terminalCost, badRegionPenalty, ref _seamFinderPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this seam finder
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveGraphCutSeamFinderRelease(ref _ptr);
            }
        }
    }

    public static partial class StitchingInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveNoSeamFinderCreate(ref IntPtr seamFinderPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveNoSeamFinderRelease(ref IntPtr seamFinderPtr);

//        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
//        internal static extern IntPtr cvePairwiseSeamFinderCreate(ref IntPtr seamFinderPtr);
//        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
//        internal static extern void cvePairwiseSeamFinderRelease(ref IntPtr seamFinderPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVoronoiSeamFinderCreate(ref IntPtr seamFinderPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVoronoiSeamFinderRelease(ref IntPtr seamFinderPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDpSeamFinderCreate(DpSeamFinder.CostFunction costFunc, ref IntPtr seamFinderPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDpSeamFinderRelease(ref IntPtr seamFinderPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGraphCutSeamFinderCreate(
            GraphCutSeamFinder.CostFunction costType,
            float terminalCost,
            float badRegionPenalty,
            ref IntPtr seamFinderPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGraphCutSeamFinderRelease(ref IntPtr seamFinderPtr);
    }
}
