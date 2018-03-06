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
    /// DeepFlow optical flow algorithm implementation.
    /// </summary>
    public class OptFlowDeepFlow : UnmanagedObject, IDenseOpticalFlow
    {
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create an instance of DeepFlow optical flow algorithm.
        /// </summary>
        public OptFlowDeepFlow()
        {
            _ptr = CvInvoke.cveOptFlowDeepFlowCreate(ref _algorithmPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Object
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveDenseOpticalFlowRelease(ref _ptr);
            }
            _algorithmPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Pointer to the unmanaged cv::Algorithm
        /// </summary>
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }
        /// <summary>
        /// Pointer to the unmanaged cv::DenseOpticalFlow
        /// </summary>
        public IntPtr DenseOpticalFlowPtr { get { return _ptr; } }
    }

    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOptFlowDeepFlowCreate(ref IntPtr algorithm);
    }
}
