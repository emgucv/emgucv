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
    /// Class implementing the Tree Based Morse Regions
    /// </summary>
    public class TBMR : Feature2D
    {
        /// <summary>
        /// Creat a new Tree Based Morse Regions
        /// </summary>
        /// <param name="minArea">Prune areas smaller than minArea</param>
        /// <param name="maxAreaRelative">Prune areas bigger than maxArea = max_area_relative * input_image_size</param>
        /// <param name="scaleFactor">Scale factor for scaled extraction.</param>
        /// <param name="nScales">Number of applications of the scale factor (octaves)</param>
        public TBMR(
            int minArea = 60,
            float maxAreaRelative = 0.01f,
            float scaleFactor = 1.25f,
            int nScales = -1)
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
        internal static extern IntPtr cveTBMRCreate(
            int minArea,
            float maxAreaRelative,
            float scaleFactor,
            int nScales,
            ref IntPtr tmbr,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTBMRRelease(ref IntPtr shared);
    }
}