//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Hfs
{
    /// <summary>
    /// Entry points to the Open CV HFS module
    /// </summary>
    public static partial class HfsInvoke
    {
        static HfsInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveHfsSegmentCreate(
          int height,
          int width,
          float segEgbThresholdI,
          int minRegionSizeI,
          float segEgbThresholdII,
          int minRegionSizeII,
          float spatialWeight,
          int slicSpixelSize,
          int numSlicIter,
          ref IntPtr algorithmPtr,
          ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHfsSegmentRelease(ref IntPtr hfsSegmentPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHfsPerformSegment(IntPtr hfsSegment, IntPtr src, IntPtr dst, bool ifDraw, bool useGpu);

    }
}