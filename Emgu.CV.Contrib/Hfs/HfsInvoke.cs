//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
    public partial class HfsSegment : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create instance to draw UTF-8 strings.
        /// </summary>
        public HfsSegment(
            int height,
            int width,
            float segEgbThresholdI,
            int minRegionSizeI,
            float segEgbThresholdII,
            int minRegionSizeII,
            float spatialWeight,
            int slicSpixelSize,
            int numSlicIter)
        {
            _ptr = HfsInvoke.cveHfsSegmentCreate(
                height,
                width,
                segEgbThresholdI,
                minRegionSizeI,
                segEgbThresholdII,
                minRegionSizeII,
                spatialWeight,
                slicSpixelSize,
                numSlicIter,
                ref _algorithmPtr,
                ref _sharedPtr);
        }

        /// <summary>
        /// Native algorithm pointer
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Release all the unmanaged memory associate with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                HfsInvoke.cveHfsSegmentRelease(ref _sharedPtr);
                _algorithmPtr = IntPtr.Zero;
                _ptr = IntPtr.Zero;
            }
        }

        public Mat PerformSegmentGpu(IInputArray src, bool ifDraw)
        {
            using (InputArray iaSrc = src.GetInputArray())
            {
                Mat m = new Mat();
                HfsInvoke.cveHfsPerformSegment(_ptr, iaSrc, m, ifDraw, true);
                return m;
            }
        }

        public Mat PerformSegmentCpu(IInputArray src, bool ifDraw)
        {
            using (InputArray iaSrc = src.GetInputArray())
            {
                Mat m = new Mat();
                HfsInvoke.cveHfsPerformSegment(_ptr, iaSrc, m, ifDraw, false);
                return m;
            }
        }
    }

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