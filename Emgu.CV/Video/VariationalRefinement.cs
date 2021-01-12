//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// This class implements variational refinement of the input flow field.
    /// </summary>
    /// <remarks>See: Thomas Brox, Andres Bruhn, Nils Papenberg, and Joachim Weickert. High accuracy optical flow estimation based on a theory for warping. In Computer Vision-ECCV 2004, pages 25-36. Springer, 2004.</remarks>
    public partial class VariationalRefinement : UnmanagedObject, IDenseOpticalFlow
    {
        private IntPtr _sharedPtr;
        private IntPtr _denseFlowPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create an instance of Variational Refinement.
        /// </summary>
        public VariationalRefinement()
        {
            _ptr = CvInvoke.cveVariationalRefinementCreate(ref _denseFlowPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Optical flow algorithm.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveVariationalRefinementRelease(ref _ptr, ref _sharedPtr);
            }
            _algorithmPtr = IntPtr.Zero;
            _denseFlowPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Pointer to the unmanaged cv::Algorithm
        /// </summary>
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }

        /// <summary>
        /// Pointer to the unmanaged cv::DenseOpticalFlow
        /// </summary>
        public IntPtr DenseOpticalFlowPtr { get { return _denseFlowPtr; } }
    }

    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVariationalRefinementCreate(ref IntPtr denseFlow, ref IntPtr algorithm, ref IntPtr sharedPtr);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVariationalRefinementRelease(ref IntPtr flow, ref IntPtr sharedPtr);
    }
}
