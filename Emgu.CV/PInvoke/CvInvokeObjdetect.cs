//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Util;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        /// <summary>
        /// Groups the object candidate rectangles.
        /// </summary>
        /// <param name="rectList">Input/output vector of rectangles. Output vector includes retained and grouped rectangles.</param>
        /// <param name="groupThreshold">Minimum possible number of rectangles minus 1. The threshold is used in a group of rectangles to retain it.</param>
        /// <param name="eps">Relative difference between sides of the rectangles to merge them into a group.</param>
        public static void GroupRectangles(VectorOfRect rectList, int groupThreshold, double eps = 0.2)
        {
            cveGroupRectangles1(rectList, groupThreshold, eps);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGroupRectangles1(IntPtr rectList, int groupThreshold, double eps = 0.2);

        /// <summary>
        /// Groups the object candidate rectangles.
        /// </summary>
        /// <param name="rectList">Input/output vector of rectangles. Output vector includes retained and grouped rectangles.</param>
        /// <param name="weights">Weights</param>
        /// <param name="groupThreshold">Minimum possible number of rectangles minus 1. The threshold is used in a group of rectangles to retain it.</param>
        /// <param name="eps">Relative difference between sides of the rectangles to merge them into a group.</param>
        public static void GroupRectangles(VectorOfRect rectList, VectorOfInt weights, int groupThreshold, double eps = 0.2)
        {
            cveGroupRectangles2(rectList, weights, groupThreshold, eps);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGroupRectangles2(IntPtr rectList, IntPtr weights, int groupThreshold, double eps = 0.2);

        /// <summary>
        /// Groups the object candidate rectangles.
        /// </summary>
        /// <param name="rectList">Input/output vector of rectangles. Output vector includes retained and grouped rectangles.</param>
        /// <param name="groupThreshold">Minimum possible number of rectangles minus 1. The threshold is used in a group of rectangles to retain it.</param>
        /// <param name="eps">Relative difference between sides of the rectangles to merge them into a group.</param>
        /// <param name="weights">weights</param>
        /// <param name="levelWeights">level weights</param>
        public static void GroupRectangles(VectorOfRect rectList, int groupThreshold, double eps, VectorOfInt weights, VectorOfDouble levelWeights)
        {
            cveGroupRectangles3(rectList, groupThreshold, eps, weights, levelWeights);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGroupRectangles3(IntPtr rectList, int groupThreshold, double eps, IntPtr weights, IntPtr levelWeights);

        /// <summary>
        /// Groups the object candidate rectangles.
        /// </summary>
        /// <param name="rectList">Input/output vector of rectangles. Output vector includes retained and grouped rectangles.</param>
        /// <param name="rejectLevels">reject levels</param>
        /// <param name="levelWeights">level weights</param>
        /// <param name="groupThreshold">Minimum possible number of rectangles minus 1. The threshold is used in a group of rectangles to retain it.</param>
        /// <param name="eps">Relative difference between sides of the rectangles to merge them into a group.</param>
        public static void GroupRectangles(VectorOfRect rectList, VectorOfInt rejectLevels, VectorOfDouble levelWeights, int groupThreshold, double eps = 0.2)
        {
            cveGroupRectangles4(rectList, rejectLevels, levelWeights, groupThreshold, eps);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGroupRectangles4(IntPtr rectList, IntPtr rejectLevels, IntPtr levelWeights, int groupThreshold, double eps);

        /// <summary>
        /// Groups the object candidate rectangles.
        /// </summary>
        /// <param name="rectList">Input/output vector of rectangles. Output vector includes retained and grouped rectangles.</param>
        /// <param name="foundWeights">found weights</param>
        /// <param name="foundScales">found scales</param>
        /// <param name="detectThreshold">detect threshold, use 0 for default</param>
        /// <param name="winDetSize">win det size, use (64, 128) for default</param>
        public static void GroupRectanglesMeanshift(VectorOfRect rectList, VectorOfDouble foundWeights, VectorOfDouble foundScales, double detectThreshold, Size winDetSize)
        {
            cveGroupRectanglesMeanshift(rectList, foundWeights, foundScales, detectThreshold, ref winDetSize);
        }
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGroupRectanglesMeanshift(IntPtr rectList, IntPtr foundWeights, IntPtr foundScales, double detectThreshold, ref Size winDetSize);

    }
}
