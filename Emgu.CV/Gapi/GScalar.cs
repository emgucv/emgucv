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

        public GScalar(MCvScalar value)
        {
            _ptr = GapiInvoke.cveGScalarCreate(ref value);
        }

        public GScalar(double value)
            : this(new MCvScalar(value))
        {
        }

        protected override void DisposeObject()
        {
            if (IntPtr.Zero == _ptr)
            {
                GapiInvoke.cveGScalarRelease(ref _ptr);
            }
        }
    }

    public static partial class GapiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveGScalarCreate(ref MCvScalar value);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGScalarRelease(ref IntPtr gscalar);

    }
}

