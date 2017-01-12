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
    public partial class DISOpticalFlow : UnmanagedObject, IDenseOpticalFlow
    {
        public enum Preset
        {
            UltraFast = 0, 
            Fast = 1, 
            Medium = 2
        }

        private IntPtr _denseFlowPtr;
        private IntPtr _algorithmPtr;

        public DISOpticalFlow(Preset preset = Preset.Fast)
        {
            _ptr = CvInvoke.cveDISOpticalFlowCreate(preset, ref _denseFlowPtr, ref _algorithmPtr);
        }

        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                CvInvoke.cveDISOpticalFlowRelease(ref _ptr);
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
        public static extern IntPtr cveDISOpticalFlowCreate(DISOpticalFlow.Preset preset, ref IntPtr denseFlow, ref IntPtr algorithm);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void cveDISOpticalFlowRelease(ref IntPtr flow);
    }
}
