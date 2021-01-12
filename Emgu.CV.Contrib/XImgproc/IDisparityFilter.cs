//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Main interface for all disparity map filters.
    /// </summary>
    public interface IDisparityFilter
    {
        /// <summary>
        /// Pointer to the native diaprty filter object
        /// </summary>
        IntPtr DisparityFilterPtr { get; }
    }


    public static partial class XImgprocInvoke
    {
        /// <summary>
        /// Apply filtering to the disparity map.
        /// </summary>
        /// <param name="filter">The disparity filter</param>
        /// <param name="disparityMapLeft">Disparity map of the left view, 1 channel, CV_16S type. Implicitly assumes that disparity values are scaled by 16 (one-pixel disparity corresponds to the value of 16 in the disparity map). Disparity map can have any resolution, it will be automatically resized to fit left_view resolution.</param>
        /// <param name="leftView">Left view of the original stereo-pair to guide the filtering process, 8-bit single-channel or three-channel image.</param>
        /// <param name="filteredDisparityMap">Output disparity map.</param>
        /// <param name="disparityMapRight">Optional argument, some implementations might also use the disparity map of the right view to compute confidence maps, for instance.</param>
        /// <param name="roi">Region of the disparity map to filter. Optional, usually it should be set automatically.</param>
        /// <param name="rightView">Optional argument, some implementations might also use the right view of the original stereo-pair.</param>
        public static void Filter(
            this IDisparityFilter filter,
            IInputArray disparityMapLeft,
            IInputArray leftView,
            IOutputArray filteredDisparityMap,
            IInputArray disparityMapRight = null,
            Rectangle roi = new Rectangle(),
            IInputArray rightView = null)
        {
            using (InputArray iaDisparityMapLeft = disparityMapLeft.GetInputArray())
            using (InputArray oaLeftView = leftView.GetInputArray())
            using (OutputArray oaFilteredDisparityMap = filteredDisparityMap.GetOutputArray())
            using (InputArray iaDisparityMapRight = disparityMapRight == null ? InputArray.GetEmpty() : disparityMapRight.GetInputArray())
            using (InputArray iaRightView = rightView == null ? InputArray.GetEmpty() : rightView.GetInputArray())
            {
                cveDisparityFilterFilter(
                    filter.DisparityFilterPtr,
                    iaDisparityMapLeft,
                    oaLeftView,
                    oaFilteredDisparityMap,
                    iaDisparityMapRight,
                    ref roi,
                    iaRightView);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDisparityFilterFilter(
            IntPtr disparityFilter,
            IntPtr disparityMapLeft, IntPtr leftView, IntPtr filteredDisparityMap,
            IntPtr disparityMapRight, ref Rectangle ROI, IntPtr rightView);

    }
}
