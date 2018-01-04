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
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{

    public class HarrisLaplaceFeatureDetector : Feature2D
    {

        public HarrisLaplaceFeatureDetector(
            int numOctaves,
            float cornThresh,
            float DOGThresh,
            int maxCorners,
            int numLayers)
        {
            _ptr = XFeatures2DInvoke.cveHarrisLaplaceFeatureDetectorCreate(
                numOctaves,
                cornThresh,
                DOGThresh,
                maxCorners,
                numLayers);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with FREAK
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                XFeatures2DInvoke.cveHarrisLaplaceFeatureDetectorRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveHarrisLaplaceFeatureDetectorCreate(
            int numOctaves,
            float cornThresh,
            float DOGThresh,
            int maxCorners,
            int numLayers);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveHarrisLaplaceFeatureDetectorRelease(ref IntPtr detector);
    }
}

