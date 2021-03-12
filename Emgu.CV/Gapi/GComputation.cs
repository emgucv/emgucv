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
    /// The equivalent of cv::GComputation
    /// </summary>

    public partial class GComputation : UnmanagedObject
    {
        public GComputation(GMat input, GMat output)
        {
            _ptr = GapiInvoke.cveGComputationCreate(input, output);
        }

        protected override void DisposeObject()
        {
            if (IntPtr.Zero == _ptr)
            {
                GapiInvoke.cveGComputationRelease(ref _ptr);
            }
        }
    }

    internal static partial class GapiInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveGComputationCreate(IntPtr input, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGComputationRelease(ref IntPtr computation);

    }
}

