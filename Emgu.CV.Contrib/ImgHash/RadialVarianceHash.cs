//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.ImgHash
{
    /// <summary>
    /// Image hash based on Radon transform
    /// </summary>
    public class RadialVarianceHash : ImgHashBase
    {
        /// <summary>
        /// Create an image hash based on Radon transform
        /// </summary>
        /// <param name="sigma">Sigma</param>
        /// <param name="numOfAngleLine">Number of angle line</param>
        public RadialVarianceHash(double sigma = 1, int numOfAngleLine = 180)
        {
            _ptr = ImgHashInvoke.cveRadialVarianceHashCreate(ref _imgHashBase, sigma, numOfAngleLine);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with RadialVarianceHash
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ImgHashInvoke.cveRadialVarianceHashRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    internal static partial class ImgHashInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveRadialVarianceHashCreate(ref IntPtr imgHash, double sigma, int numOfAngleLine);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveRadialVarianceHashRelease(ref IntPtr hash);
    }
}

