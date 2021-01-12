//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// Parameters for the FacemarkAAM model
    /// </summary>
    public partial class FacemarkAAMParams : UnmanagedObject
    {
        /// <summary>
        /// Create the paramaters with the default values.
        /// </summary>
        public FacemarkAAMParams()
        {
            _ptr = FaceInvoke.cveFacemarkAAMParamsCreate();
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                FaceInvoke.cveFacemarkAAMParamsRelease(ref _ptr);
            }
        }
    }


    /// <summary>
    /// The Facemark AMM model
    /// </summary>
    public class FacemarkAAM : UnmanagedObject, IFacemark
    {
        private IntPtr _sharedPtr;

        private IntPtr _facemarkPtr;

        /// <summary>
        /// Pointer to the unmanaged Facemark object
        /// </summary>
        public IntPtr FacemarkPtr { get { return _facemarkPtr; } }

        private IntPtr _algorithmPtr;

        /// <summary>
        /// Pointer to the unmanaged Algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr { get { return _algorithmPtr; } }

        /// <summary>
        /// Create an instance of FacemarkAAM model
        /// </summary>
        /// <param name="parameters">The model parameters</param>
        public FacemarkAAM(FacemarkAAMParams parameters)
        {
            _ptr = FaceInvoke.cveFacemarkAAMCreate(parameters, ref _facemarkPtr, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this Facemark
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                FaceInvoke.cveFacemarkAAMRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }

    public static partial class FaceInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkAAMCreate(IntPtr parameters, ref IntPtr facemark, ref IntPtr algorithm, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkAAMRelease(ref IntPtr facemark, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveFacemarkAAMParamsCreate();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveFacemarkAAMParamsRelease(ref IntPtr parameters);
    }
}