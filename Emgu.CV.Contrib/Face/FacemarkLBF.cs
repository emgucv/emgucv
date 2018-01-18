//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Face
{

    public partial class FacemarkLBFParams : UnmanagedObject
    {
        public FacemarkLBFParams()
        {
            _ptr = FaceInvoke.cveFacemarkLBFParamsCreate();
        }

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                FaceInvoke.cveFacemarkLBFParamsRelease(ref _ptr);
            }
        }
    }

    public class FacemarkLBF : UnmanagedObject, IFacemark
    {
        private IntPtr _facemarkPtr;
        public IntPtr FacemarkPtr { get { return _facemarkPtr; } }

        private IntPtr _algorithmPtr;
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }

        
        public FacemarkLBF(FacemarkLBFParams parameters)
        {
            _ptr = FaceInvoke.cveFacemarkLBFCreate(parameters, ref _facemarkPtr, ref _algorithmPtr);
        }

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                FaceInvoke.cveFacemarkLBFRelease(ref _ptr);
            }
        }
    }

    public static partial class FaceInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkLBFCreate(IntPtr parameters, ref IntPtr facemark, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkLBFRelease(ref IntPtr facemark);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkLBFParamsCreate();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkLBFParamsRelease(ref IntPtr parameters);
    }
}