//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// This is used store and set up the parameters of the robust local optical flow (RLOF) algorithm.
    /// </summary>
    public partial class RLOFOpticalFlowParameter : UnmanagedObject
    {
        /// <summary>
        /// The solver type
        /// </summary>
        public enum SolverType
        {
            /// <summary>
            /// Apply standard iterative refinement
            /// </summary>
            Standard = 0,
            /// <summary>
            /// Apply optimized iterative refinement based bilinear equation solutions
            /// </summary>
            Bilinear = 1
        }

        /// <summary>
        /// The support region type
        /// </summary>
        public enum SupportRegionType
        {
            /// <summary>
            /// Apply a constant support region
            /// </summary>
            Fixed = 0,

            /// <summary>
            /// Apply a adaptive support region obtained by cross-based segmentation
            /// </summary>
            Cross = 1
        }

        /// <summary>
        /// Create a RLOF Optical Flow Parameter with default parameters.
        /// </summary>
        public RLOFOpticalFlowParameter()
        {
            _ptr = CvInvoke.cveRLOFOpticalFlowParameterCreate();
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveRLOFOpticalFlowParameterRelease(ref _ptr);
            }
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRLOFOpticalFlowParameterCreate();
		
		[DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRLOFOpticalFlowParameterRelease(ref IntPtr p);
    }
}
