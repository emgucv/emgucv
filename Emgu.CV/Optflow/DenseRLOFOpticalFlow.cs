//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    /// Fast dense optical flow computation based on robust local optical flow (RLOF) algorithms and sparse-to-dense interpolation scheme.
    /// </summary>
    public partial class DenseRLOFOpticalFlow : SharedPtrObject, IDenseOpticalFlow
    {
        /// <summary>
        /// Interpolation type used to compute the dense optical flow.
        /// </summary>
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

        /// <summary>
        /// Creates instance of DenseRLOFOpticalFlow
        /// </summary>
        /// <param name="parameter">The RLOF optical flow parameters</param>
        /// <param name="forwardBackwardThreshold">Threshold for the forward backward confidence check. Use 1.0f for default</param>
        /// <param name="gridStep">Size of the grid to spawn the motion vectors. Use (6, 6) for default</param>
        /// <param name="interpType">Interpolation used to compute the dense optical flow.</param>
        /// <param name="epicK">See Ximgproc.EdgeAwareInterpolator() K value.</param>
        /// <param name="epicSigma">See Ximgproc.EdgeAwareInterpolator() sigma value.</param>
        /// <param name="epicLambda">See Ximgproc.EdgeAwareInterpolator() lambda value.</param>
        /// <param name="usePostProc">Enables Ximgproc.fastGlobalSmootherFilter</param>
        /// <param name="fgsLambda">See Ximgproc.EdgeAwareInterpolator().</param>
        /// <param name="fgsSigma">See Ximgproc.EdgeAwareInterpolator().</param>
        public DenseRLOFOpticalFlow(
            RLOFOpticalFlowParameter parameter,
            float forwardBackwardThreshold,
            Size gridStep, 
            InterpolationType interpType = InterpolationType.Epic,
            int epicK = 128,
            float epicSigma = 0.05f,
            float epicLambda = 999.0f,
            bool usePostProc = true,
            float fgsLambda = 500.0f,
            float fgsSigma = 1.5f
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
