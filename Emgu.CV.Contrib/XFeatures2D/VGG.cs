//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{

    public class VGG : Feature2D
    {
        public VGG(
            int desc, float isigma,
            bool imgNormalize, bool useScaleOrientation,
            float scaleFactor, bool dscNormalize)
        {
            _ptr = XFeatures2DInvoke.cveVGGCreate(desc, isigma, imgNormalize, useScaleOrientation, scaleFactor, dscNormalize,
                ref _feature2D);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with VGG
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                XFeatures2DInvoke.cveVGGRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveVGGCreate(
            int desc, float isigma,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool imgNormalize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useScaleOrientation,
            float scaleFactor,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool dscNormalize, ref IntPtr feature2D);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveVGGRelease(ref IntPtr extractor);
    }
}

