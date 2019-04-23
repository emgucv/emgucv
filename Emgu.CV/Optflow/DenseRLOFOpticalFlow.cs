//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
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
    public partial class DenseRLOFOpticalFlow : SharedPtrObject, IDenseOpticalFlow
    {
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

        public enum InterpolationType
        {
            /// <summary>
            /// Fast geodesic interpolation
            /// </summary>
            Geo = 0,
            /// <summary>
            /// Edge-preserving interpolation
            /// </summary>
            Epic = 1,    
        };

        private IntPtr _algorithm;
        private IntPtr _denseOpticalFlow;

        public DenseRLOFOpticalFlow(
            RLOFOpticalFlowParameter parameter,
            float forwardBackwardThreshold,
            Size gridStep,
            InterpolationType interpType,
            int epicK,
            float epicSigma,
            float epicLambda,
            bool usePostProc,
            float fgsLambda,
            float fgsSigma
            )
        {
            _ptr = CvInvoke.cveDenseRLOFOpticalFlowCreate(
                parameter,
                forwardBackwardThreshold,
                ref gridStep,
                interpType,
                epicK,
                epicSigma,
                epicLambda,
                usePostProc,
                fgsLambda,
                fgsSigma,
                ref _denseOpticalFlow, 
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
                CvInvoke.cveDenseRLOFOpticalFlowRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
                _denseOpticalFlow = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the dense optical flow pointer.
        /// </summary>
        /// <value>
        /// The pointer to the dense optical flow object.
        /// </value>
        public IntPtr DenseOpticalFlowPtr
        {
            get { return _denseOpticalFlow; }
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
        internal static extern void cveDenseRLOFOpticalFlowRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDenseRLOFOpticalFlowCreate(
			IntPtr rlofParameter, 
			float forwardBackwardThreshold,
			ref Size gridStep,
            Emgu.CV.DenseRLOFOpticalFlow.InterpolationType interpType,
			int epicK,
			float epicSigma,
			float epicLambda,
            [MarshalAs(CvInvoke.BoolMarshalType)]
			bool usePostProc,
			float fgsLambda,
			float fgsSigma,
			ref IntPtr denseOpticalFlow, 
			ref IntPtr algorithm, 
			ref IntPtr sharedPtr);
    }
}
