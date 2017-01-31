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

    public class BoostDesc : Feature2D
    {
        public BoostDesc(int desc, bool useScaleOrientation, float scalefactor)
        {
            _ptr = XFeatures2DInvoke.cveBoostDescCreate(desc, useScaleOrientation, scalefactor, ref _feature2D);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with BRIEF
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                XFeatures2DInvoke.cveBoostDescRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        static XFeatures2DInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveBoostDescCreate(
            int desc,
            [MarshalAs(CvInvoke.BoolMarshalType)]
          bool useScaleOrientation,
            float scalefactor,
            ref IntPtr feature2D);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveBoostDescRelease(ref IntPtr extractor);
    }
}

