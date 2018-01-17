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

    public class FacemarkAAM : UnmanagedObject, IFacemark
    {
        private IntPtr _facemarkPtr;
        public IntPtr FacemarkPtr { get { return _facemarkPtr; } }

        private IntPtr _algorithmPtr;
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }

        public class Params : UnmanagedObject
        {
            public Params()
            {
                _ptr = Emgu.CV.ContribInvoke.cveFacemarkAAMParamsCreate();
            }

            protected override void DisposeObject()
            {
                if (_ptr != IntPtr.Zero)
                {
                    Emgu.CV.ContribInvoke.cveFacemarkAAMParamsRelease(ref _ptr);
                }
            }
        }

        public FacemarkAAM(Params parameters)
        {
            _ptr = Emgu.CV.ContribInvoke.cveFacemarkAAMCreate(parameters, ref _facemarkPtr, ref _algorithmPtr);
        }

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                Emgu.CV.ContribInvoke.cveFacemarkAAMRelease(ref _ptr);
            }
        }
    }
}

namespace Emgu.CV
{
    public static partial class ContribInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkAAMCreate(IntPtr parameters, ref IntPtr facemark, ref IntPtr algorithm);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkAAMRelease(ref IntPtr facemark);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkAAMParamsCreate();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkAAMParamsRelease(ref IntPtr parameters);
    }
}