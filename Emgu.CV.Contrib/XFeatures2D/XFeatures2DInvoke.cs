//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;


namespace Emgu.CV.XFeatures2D
{
    /// <summary>
    /// This class wraps the functional calls to the OpenCV XFeatures2D modules
    /// </summary>
    public static partial class XFeatures2DInvoke
    {
        static XFeatures2DInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        /// <summary>
        /// GMS (Grid-based Motion Statistics) feature matching strategy
        /// </summary>
        /// <param name="size1">Input size of image1.</param>
        /// <param name="size2">Input size of image2.</param>
        /// <param name="keypoints1">Input keypoints of image1.</param>
        /// <param name="keypoints2">Input keypoints of image2.</param>
        /// <param name="matches1to2">Input 1-nearest neighbor matches.</param>
        /// <param name="matchesGMS">Matches returned by the GMS matching strategy.</param>
        /// <param name="withRotation">Take rotation transformation into account.</param>
        /// <param name="withScale">Take scale transformation into account.</param>
        /// <param name="thresholdFactor">The higher, the less matches.</param>
        public static void MatchGMS(
            Size size1, Size size2,
            VectorOfKeyPoint keypoints1, VectorOfKeyPoint keypoints2,
            VectorOfDMatch matches1to2, VectorOfDMatch matchesGMS,
            bool withRotation = false, 
            bool withScale = false, 
            double thresholdFactor = 6.0)
        {
            cveMatchGMS(ref size1, ref size2, keypoints1, keypoints2, matches1to2, matchesGMS, withRotation, withScale, thresholdFactor);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMatchGMS(
            ref Size size1, ref Size size2,
            IntPtr keypoints1, IntPtr keypoints2,
            IntPtr matches1to2, IntPtr matchesGMS,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool withRotation,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool withScale, 
            double thresholdFactor);
    }
}

