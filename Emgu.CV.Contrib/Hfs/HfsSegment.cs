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
    /// Hierarchical Feature Selection for Efficient Image Segmentation
    /// </summary>
    public partial class HfsSegment : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create a hfs object
        /// </summary>
        /// <param name="height">The height of the input image</param>
        /// <param name="width">The width of the input image</param>
        /// <param name="segEgbThresholdI">segEgbThresholdI</param>
        /// <param name="minRegionSizeI">minRegionSizeI</param>
        /// <param name="segEgbThresholdII">segEgbThresholdII</param>
        /// <param name="minRegionSizeII">minRegionSizeII</param>
        /// <param name="spatialWeight">spatialWeight</param>
        /// <param name="slicSpixelSize">slicSpixelSize</param>
        /// <param name="numSlicIter">numSlicIter</param>
        public HfsSegment(
            int height,
            int width,
            float segEgbThresholdI = 0.08f,
            int minRegionSizeI = 100,
            float segEgbThresholdII = 0.28f,
            int minRegionSizeII = 200,
            float spatialWeight = 0.6f,
            int slicSpixelSize = 8,
            int numSlicIter = 5)
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

        /// <summary>
        /// Segmentation with gpu
        /// </summary>
        /// <param name="src">The input image</param>
        /// <param name="ifDraw">if draw the image in the returned Mat. if this parameter is false, then the content of the returned Mat is a matrix of index, describing the region each pixel belongs to. And it's data type is CV_16U. If this parameter is true, then the returned Mat is a segmented picture, and color of each region is the average color of all pixels in that region. And it's data type is the same as the input image</param>
        /// <returns>Segmentation result</returns>
        public Mat PerformSegmentGpu(IInputArray src, bool ifDraw = true)
        {
            using (InputArray iaSrc = src.GetInputArray())
            {
                Mat m = new Mat();
                HfsInvoke.cveHfsPerformSegment(_ptr, iaSrc, m, ifDraw, true);
                return m;
            }
        }

        /// <summary>
        /// Segmentation with cpu. This method is only implemented for reference. It is highly NOT recommended to use it.
        /// </summary>
        /// <param name="src">The input image</param>
        /// <param name="ifDraw">if draw the image in the returned Mat. if this parameter is false, then the content of the returned Mat is a matrix of index, describing the region each pixel belongs to. And it's data type is CV_16U. If this parameter is true, then the returned Mat is a segmented picture, and color of each region is the average color of all pixels in that region. And it's data type is the same as the input image</param>
        /// <returns>Segmentation result</returns>
        public Mat PerformSegmentCpu(IInputArray src, bool ifDraw = true)
        {
            using (InputArray iaSrc = src.GetInputArray())
            {
                Mat m = new Mat();
                HfsInvoke.cveHfsPerformSegment(_ptr, iaSrc, m, ifDraw, false);
                return m;
            }
        }
    }

}