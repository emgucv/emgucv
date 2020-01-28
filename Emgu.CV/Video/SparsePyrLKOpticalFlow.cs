//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// The class can calculate an optical flow for a sparse feature set using the iterative Lucas-Kanade method with pyramids.
    /// </summary>
    public partial class SparsePyrLKOpticalFlow : UnmanagedObject, ISparseOpticalFlow
    {
        private IntPtr _sharedPtr;

        private IntPtr _algorithm;
        private IntPtr _sparseOpticalFlow;

        /// <summary>
        /// Create a SparsePyrLKOpticalFlow object
        /// </summary>
        /// <param name="winSize">size of the search window at each pyramid level.</param>
        /// <param name="maxLevel">0-based maximal pyramid level number; if set to 0, pyramids are not used (single level), if set to 1, two levels are used, and so on; if pyramids are passed to input then algorithm will use as many levels as pyramids have but no more than maxLevel.</param>
        /// <param name="crit">specifying the termination criteria of the iterative search algorithm (after the specified maximum number of iterations criteria.maxCount or when the search window moves by less than criteria.epsilon.</param>
        /// <param name="flags">operation flags</param>
        /// <param name="minEigThreshold">the algorithm calculates the minimum eigen value of a 2x2 normal matrix of optical flow equations, divided by number of pixels in a window; if this value is less than minEigThreshold, then a corresponding feature is filtered out and its flow is not processed, so it allows to remove bad points and get a performance boost.</param>
        public SparsePyrLKOpticalFlow(
          Size winSize,
          int maxLevel,
          MCvTermCriteria crit,
          CvEnum.LKFlowFlag flags,
          double minEigThreshold)
        {
            _ptr = CvInvoke.cveSparsePyrLKOpticalFlowCreate(
                ref winSize,
                maxLevel,
                ref crit,
                flags,
                minEigThreshold,
                ref _sparseOpticalFlow,
                ref _algorithm, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveSparsePyrLKOpticalFlowRelease(ref _ptr, ref _sharedPtr);
                _algorithm = IntPtr.Zero;
                _sparseOpticalFlow = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Pointer to the unmanaged SparseOpticalFlow object
        /// </summary>
        public IntPtr SparseOpticalFlowPtr
        {
            get { return _sparseOpticalFlow; }
        }

        /// <summary>
        /// Return the pointer to the algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSparsePyrLKOpticalFlowCreate(
            ref Size winSize,
            int maxLevel,
            ref MCvTermCriteria crit,
            CvEnum.LKFlowFlag flags,
            double minEigThreshold,
            ref IntPtr sparseOpticalFlow,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSparsePyrLKOpticalFlowRelease(ref IntPtr flow, ref IntPtr sharedPtr);
    }
}
