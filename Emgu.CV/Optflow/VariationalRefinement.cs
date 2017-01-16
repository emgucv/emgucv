//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public partial class VariationalRefinement : UnmanagedObject, IDenseOpticalFlow
    {
        private IntPtr _denseFlowPtr;
        private IntPtr _algorithmPtr;

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

        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }
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
