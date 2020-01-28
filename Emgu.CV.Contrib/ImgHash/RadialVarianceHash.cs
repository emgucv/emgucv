//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create an image hash based on Radon transform
        /// </summary>
        /// <param name="sigma">Sigma</param>
        /// <param name="numOfAngleLine">Number of angle line</param>
        public RadialVarianceHash(double sigma = 1, int numOfAngleLine = 180)
        {
            _ptr = ImgHashInvoke.cveRadialVarianceHashCreate(ref _imgHashBase, sigma, numOfAngleLine, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with RadialVarianceHash
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ImgHashInvoke.cveRadialVarianceHashRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

    internal static partial class ImgHashInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveRadialVarianceHashCreate(ref IntPtr imgHash, double sigma, int numOfAngleLine, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveRadialVarianceHashRelease(ref IntPtr hash, ref IntPtr sharedPtr);
    }
}

