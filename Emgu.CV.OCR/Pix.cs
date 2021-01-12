//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.OCR
{
    /// <summary>
    /// Leptonica Pix image structure
    /// </summary>
    public class Pix : UnmanagedObject
    {
        /// <summary>
        /// Create a Pix object by coping data from Mat
        /// </summary>
        /// <param name="mat">The Mat to create the Pix object from</param>
        public Pix(Mat mat)
        {
            _ptr = OcrInvoke.leptCreatePixFromMat(mat);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this Pix
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                OcrInvoke.leptPixDestroy(ref _ptr);
            }
        }
    }

    public static partial class OcrInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr leptCreatePixFromMat(IntPtr m);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void leptPixDestroy(ref IntPtr pix);
    }
}
