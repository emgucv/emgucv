//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.LineDescriptor
{
    /// <summary>
    /// Class implements both functionality for detection of lines and computation of their binary descriptor.
    /// </summary>
    public class BinaryDescriptor : SharedPtrObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BinaryDescriptor()
        {
            _ptr = LineDescriptorInvoke.cveLineDescriptorBinaryDescriptorCreate(ref _sharedPtr);
        }

        /// <summary>
        /// Line detection.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="keylines">Vector that will store extracted lines for one or more images</param>
        /// <param name="mask">Mask matrix to detect only KeyLines of interest</param>
        public void Detect(Mat image, VectorOfKeyLine keylines, Mat mask = null)
        {
            LineDescriptorInvoke.cveLineDescriptorBinaryDescriptorDetect(_ptr, image, keylines, mask);
        }

        /// <summary>
        /// Descriptors computation.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="keylines">Vector containing lines for which descriptors must be computed</param>
        /// <param name="descriptors">Computed descriptors will be stored here</param>
        /// <param name="returnFloatDescr">When true, original non-binary descriptors are returned</param>
        public void Compute(Mat image, VectorOfKeyLine keylines, Mat descriptors, bool returnFloatDescr = false)
        {
            LineDescriptorInvoke.cveLineDescriptorBinaryDescriptorCompute(_ptr, image, keylines, descriptors, returnFloatDescr);
        }

        /// <summary>
        /// Release unmanaged memory associated with this binary descriptor
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                LineDescriptorInvoke.cveLineDescriptorBinaryDescriptorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Entry points for LineDescriptor module
    /// </summary>
    public static partial class LineDescriptorInvoke
    {
        static LineDescriptorInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLineDescriptorBinaryDescriptorCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineDescriptorBinaryDescriptorDetect(
            IntPtr descriptor,
            IntPtr image,
            IntPtr keypoints,
            IntPtr mask);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineDescriptorBinaryDescriptorCompute(
            IntPtr descriptor,
            IntPtr image,
            IntPtr keylines,
            IntPtr descriptors,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool returnFloatDescr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineDescriptorBinaryDescriptorRelease(ref IntPtr sharedPtr);
    }
}
