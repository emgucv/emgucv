//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// TBMR descriptor.
    /// </summary>
    public class TBMR : Feature2D
    {
        public TBMR(
            int minArea,
            float maxAreaRelative,
            float scaleFactor,
            int nScales)
        {          
            _ptr = XFeatures2DInvoke.cveTBMRCreate(
                minArea,
                maxAreaRelative,
                scaleFactor,
                nScales,
                ref _feature2D, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with TBMR
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XFeatures2DInvoke.cveTBMRRelease(ref _sharedPtr);
            }
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveTBMRCreate(
            int minArea,
            float maxAreaRelative,
            float scaleFactor,
            int nScales,
            ref IntPtr tmbr,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveTBMRRelease(ref IntPtr shared);
    }
}