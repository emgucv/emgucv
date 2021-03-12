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

    internal static partial class GapiInvoke
    {
        static GapiInvoke()
        {
            CvInvoke.Init();
        }

        public static GMat Resize(GMat src, Size dsize, double fx, double fy, int interpolation)
        {
            return new GMat(cveGapiResize(src, ref dsize, fx, fy, interpolation), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiResize(IntPtr src, ref Size dsize, double fx, double fy, int interpolation);

        public static GMat BitwiseNot(GMat src)
        {
            return new GMat(cveGapiBitwiseNot(src), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBitwiseNot(IntPtr src);

    }
}

