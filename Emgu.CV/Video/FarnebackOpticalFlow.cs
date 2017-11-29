//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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

    public partial class FarnebackOpticalFlow : UnmanagedObject, IDenseOpticalFlow
    {
        private IntPtr _algorithm;
        private IntPtr _denseOpticalFlow;


        public FarnebackOpticalFlow(
            int numLevels,
            double pyrScale,
            bool fastPyramids,
            int winSize,
            int numIters,
            int polyN,
            double polySigma,
            CvEnum.OpticalflowFarnebackFlag flags)
        {
            _ptr = CvInvoke.cveFarnebackOpticalFlowCreate(
                numLevels,
                pyrScale, fastPyramids, winSize, numIters, polyN, polySigma,
                flags,
                ref _denseOpticalFlow, ref _algorithm);
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveFarnebackOpticalFlowRelease(ref _ptr);
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
        internal static extern void cveFarnebackOpticalFlowRelease(ref IntPtr flow);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFarnebackOpticalFlowCreate(
            int numLevels,
            double pyrScale,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool fastPyramids,
            int winSize,
            int numIters,
            int polyN,
            double polySigma,
            CvEnum.OpticalflowFarnebackFlag flags,
            ref IntPtr denseOpticalFlow,
            ref IntPtr algorithm);
    }
}
