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

    public class FacemarkLBF : UnmanagedObject, IFacemark
    {
        private IntPtr _facemarkPtr;
        public IntPtr FacemarkPtr { get { return _facemarkPtr; } }

        public class Params : UnmanagedObject
        {
            public Params()
            {
                _ptr = Emgu.CV.ContribInvoke.cveFacemarkLBFParamsCreate();
            }

            protected override void DisposeObject()
            {
                if (_ptr != IntPtr.Zero)
                {
                    Emgu.CV.ContribInvoke.cveFacemarkLBFParamsRelease(ref _ptr);
                }
            }
        }

        public FacemarkLBF(Params parameters)
        {
            _ptr = Emgu.CV.ContribInvoke.cveFacemarkLBFCreate(parameters, ref _facemarkPtr);
        }

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                Emgu.CV.ContribInvoke.cveFacemarkLBFRelease(ref _ptr);
            }
        }
    }
}

namespace Emgu.CV
{
    public static partial class ContribInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkLBFCreate(IntPtr parameters, ref IntPtr facemark);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkLBFRelease(ref IntPtr facemark);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkLBFParamsCreate();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkLBFParamsRelease(ref IntPtr parameters);
    }
}