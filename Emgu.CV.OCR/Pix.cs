//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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
    public class Pix : UnmanagedObject
    {
        public Pix(Mat mat)
        {
            _ptr = OcrInvoke.leptCreatePixFromMat(mat);
        }

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
