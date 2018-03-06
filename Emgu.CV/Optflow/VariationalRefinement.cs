//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
    /// This class implements variational refinement of the input flow field, i.e. it uses input flow to initialize the minimization of the following functional: E(U)=∫ΩδΨ(EI)+γΨ(EG)+αΨ(ES), where EI,EG,ES are color constancy, gradient constancy and smoothness terms respectively. Ψ(s2)=sqrt(s^2+ϵ^2) is a robust penalizer to limit the influence of outliers.
    /// </summary>
    /// <remarks>See: Thomas Brox, Andres Bruhn, Nils Papenberg, and Joachim Weickert. High accuracy optical flow estimation based on a theory for warping. In Computer Vision-ECCV 2004, pages 25–36. Springer, 2004.</remarks>
    public partial class VariationalRefinement : UnmanagedObject, IDenseOpticalFlow
    {
        private IntPtr _denseFlowPtr;
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create an instance of Variational Refinement.
        /// </summary>
        public VariationalRefinement()
        {
            _ptr = CvInvoke.cveVariationalRefinementCreate(ref _denseFlowPtr, ref _algorithmPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Optical flow algorithm.
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveVariationalRefinementRelease(ref _ptr);
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
        internal static extern IntPtr cveVariationalRefinementCreate(ref IntPtr denseFlow, ref IntPtr algorithm);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVariationalRefinementRelease(ref IntPtr flow);
    }
}
