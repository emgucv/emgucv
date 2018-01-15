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
    public interface IFacemark 
    {

        /// <summary>
        /// Return the pointer to the Facemark object
        /// </summary>
        /// <returns>The pointer to the Facemark object</returns>
        IntPtr FacemarkPtr { get; }
    }
}

namespace Emgu.CV
{

    public static partial class ContribInvoke
    {
        public static bool SetFaceDetector(Emgu.CV.Face.IFacemark facemark, FaceDetectNative faceDetect)
        {
            return cveFacemarkSetFaceDetector(facemark.FacemarkPtr, faceDetect);
        }

        [UnmanagedFunctionPointer(CvInvoke.CvCallingConvention)]
        public delegate int FaceDetectNative(IntPtr input, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveFacemarkSetFaceDetector(IntPtr facemark, FaceDetectNative detector);
    }
}