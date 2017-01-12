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
    public class OpticalFlowPCAFlow : UnmanagedObject, IDenseOpticalFlow
    {
        private IntPtr _algorithmPtr;

        public OpticalFlowPCAFlow()
        {
            _ptr = CvInvoke.cveOptFlowPCAFlowCreate(ref _algorithmPtr);
        }

        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveDenseOpticalFlowRelease(ref _ptr);
            }
            _algorithmPtr = IntPtr.Zero;
        }

        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }
        public IntPtr DenseOpticalFlowPtr { get { return _ptr; } }
    }


    public static partial class CvInvoke
    {
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern IntPtr cveOptFlowPCAFlowCreate(ref IntPtr algorithm);
    }
}
