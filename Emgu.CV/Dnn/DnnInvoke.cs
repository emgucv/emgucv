//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Dnn
{
    public static partial class DnnInvoke
    {
        static DnnInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        public static Mat BlobFromImage(Mat image, double scaleFactor = 1.0, Size size = new Size(), MCvScalar mean = new MCvScalar(), bool swapRB = true)
        {
            Mat blob = new Mat();
            cveDnnBlobFromImage(image, scaleFactor, ref size, ref mean, swapRB, blob);
            return blob;
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnBlobFromImage(
            IntPtr image,
            double scalefactor,
            ref Size size,
            ref MCvScalar mean,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool swapRB,
            IntPtr blob);

        public static Mat BlobFromImages(Mat[] images, double scaleFactor = 1.0, Size size = new Size(), MCvScalar mean = new MCvScalar(), bool swapRB = true)
        {
            Mat blob = new Mat();
            using (VectorOfMat vm = new VectorOfMat(images))
            {
                cveDnnBlobFromImages(vm, scaleFactor, ref size, ref mean, swapRB, blob);
            }
            return blob;
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnBlobFromImages(
            IntPtr images,
            double scalefactor,
            ref Size size,
            ref MCvScalar mean,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool swapRB,
            IntPtr blob);
    }
}

#endif