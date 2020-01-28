//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
    /// <summary>
    /// Simple Blob detector
    /// </summary>
    public class SimpleBlobDetector : Feature2D
    {

        /// <summary>
        /// Create a simple blob detector
        /// </summary>
        /// <param name="parameters">The parameters for creating a simple blob detector</param>
        public SimpleBlobDetector(SimpleBlobDetectorParams parameters = null)
        {
            if (parameters == null)
                _ptr = Features2DInvoke.cveSimpleBlobDetectorCreate(ref _feature2D, ref _sharedPtr);
            else
            {
                _ptr = Features2DInvoke.cveSimpleBlobDetectorCreateWithParams(ref _feature2D, parameters, ref _sharedPtr);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                Features2DInvoke.cveSimpleBlobDetectorRelease(ref _sharedPtr);

            base.DisposeObject();
        }
    }

    /// <summary>
    /// Parameters for the simple blob detector
    /// </summary>
    public partial class SimpleBlobDetectorParams : UnmanagedObject
    {
        /// <summary>
        /// Create parameters for simple blob detector and use default values.
        /// </summary>
        public SimpleBlobDetectorParams()
        {
            _ptr = Features2DInvoke.cveSimpleBlobDetectorParamsCreate();
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this simple blob detector parameter.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                Features2DInvoke.cveSimpleBlobDetectorParamsRelease(ref _ptr);
            }
        }
    }

    public static partial class Features2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveSimpleBlobDetectorCreate(ref IntPtr feature2DPtr, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveSimpleBlobDetectorCreateWithParams(ref IntPtr feature2DPtr, IntPtr parameters, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveSimpleBlobDetectorRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveSimpleBlobDetectorParamsCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveSimpleBlobDetectorParamsRelease(ref IntPtr parameters);

    }
}