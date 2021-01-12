//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Class used for calculation sparse optical flow and feature tracking with robust local optical flow (RLOF) algorithms.
    /// </summary>
    public partial class SparseRLOFOpticalFlow : SharedPtrObject, ISparseOpticalFlow
    {
        private IntPtr _algorithm;
        private IntPtr _sparseOpticalFlow;

        /// <summary>
        /// Creates instance of SparseRLOFOpticalFlow
        /// </summary>
        /// <param name="parameter">The RLOF optical flow parameters</param>
        /// <param name="forwardBackwardThreshold">Threshold for the forward backward confidence check. Use 1.0f for default</param>
        public SparseRLOFOpticalFlow(
            RLOFOpticalFlowParameter parameter,
            float forwardBackwardThreshold)
        {
            _ptr = CvInvoke.cveSparseRLOFOpticalFlowCreate(
                parameter,
                forwardBackwardThreshold,
                ref _sparseOpticalFlow, 
                ref _algorithm, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CvInvoke.cveSparseRLOFOpticalFlowRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
                _sparseOpticalFlow = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the sparse optical flow pointer.
        /// </summary>
        /// <value>
        /// The pointer to the sparse optical flow object.
        /// </value>
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
        internal static extern void cveSparseRLOFOpticalFlowRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSparseRLOFOpticalFlowCreate(
			IntPtr rlofParameter, 
			float forwardBackwardThreshold,
			ref IntPtr sparseOpticalFlow, 
			ref IntPtr algorithm, 
			ref IntPtr sharedPtr);
    }
}
