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
    /// Marr-Hildreth Operator Based Hash, slowest but more discriminative.
    /// </summary>
    public class MarrHildrethHash : ImgHashBase
    {
        /// <summary>
        /// Create a Marr-Hildreth operator based hash.
        /// </summary>
        /// <param name="alpha">Scale factor for marr wavelet.</param>
        /// <param name="scale">Level of scale factor</param>
        public MarrHildrethHash(float alpha = 2.0f, float scale = 1.0f)
        {
            _ptr = ImgHashInvoke.cveMarrHildrethHashCreate(ref _imgHashBase, alpha, scale);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with MarrHildrethHash
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ImgHashInvoke.cveMarrHildrethHashRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    internal static partial class ImgHashInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveMarrHildrethHashCreate(ref IntPtr imgHash, float alpha, float scale);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMarrHildrethHashRelease(ref IntPtr hash);
    }
}

