//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.IO;

namespace Emgu.CV
{
    /// <summary>
    /// The equivalent of cv::GScalar
    /// </summary>
    public partial class GScalar : UnmanagedObject
    {
        internal GScalar(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Create a GScalar from a scalar value.
        /// </summary>
        /// <param name="value">The scalar value</param>
        public GScalar(MCvScalar value)
        {
            _ptr = GapiInvoke.cveGScalarCreate(ref value);
        }

        /// <summary>
        /// Create a GScalar from a double value
        /// </summary>
        /// <param name="value">The double value</param>
        public GScalar(double value)
            : this(new MCvScalar(value))
        {
        }

        /// <summary>
        /// Release all the unmanaged memory associated with the GScalar
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                GapiInvoke.cveGScalarRelease(ref _ptr);
            }
        }
    }

    public static partial class GapiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGScalarCreate(ref MCvScalar value);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGScalarRelease(ref IntPtr gscalar);

    }
}

