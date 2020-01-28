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
    /// Image hash based on color moments.
    /// </summary>
    public class ColorMomentHash : ImgHashBase
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a Color Moment Hash object
        /// </summary>
        public ColorMomentHash()
        {
            _ptr = ImgHashInvoke.cveColorMomentHashCreate(ref _imgHashBase, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with ColorMomentHash
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ImgHashInvoke.cveColorMomentHashRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

    internal static partial class ImgHashInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveColorMomentHashCreate(ref IntPtr imgHash, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveColorMomentHashRelease(ref IntPtr hash, ref IntPtr sharedPtr);
    }
}

