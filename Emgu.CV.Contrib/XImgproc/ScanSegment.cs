//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV.XImgproc
{

    public class ScanSegment : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithm;

        /// <summary>
        /// Pointer to cv::Algorithm
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }


        public ScanSegment(
            int imageWidth,
            int imageHeight,
            int numSuperpixels,
            int slices = 8,
            bool mergeSmall = true)
        {
            _ptr = XImgprocInvoke.cveScanSegmentCreate(
                imageWidth,
                imageHeight,
                numSuperpixels,
                slices,
                mergeSmall,
                ref _algorithm,
                ref _sharedPtr);
        }

        public void Iterate(IInputArray img)
        {
            using(InputArray iaImg = img.GetInputArray())
                XImgprocInvoke.cveScanSegmentIterate(_ptr, iaImg);
        }

        public void GetLabels(IOutputArray labelsOut)
        {
            using (OutputArray oaLabelsOut = labelsOut.GetOutputArray())
                XImgprocInvoke.cveScanSegmentGetLabels(_ptr, oaLabelsOut);
        }

        public void GetLabelContourMask(IOutputArray image, bool thickLine=false)
        {
            using (OutputArray oaImage = image.GetOutputArray())
                XImgprocInvoke.cveScanSegmentGetLabelContourMask(_ptr, oaImage, thickLine);
        }


        /// <inheritdoc />
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XImgprocInvoke.cveScanSegmentRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveScanSegmentCreate(
            int imageWidth,
            int imageHeight,
            int numSuperpixels,
            int slices,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool mergeSmall,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveScanSegmentIterate(IntPtr scanSegment, IntPtr img);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveScanSegmentGetLabels(IntPtr scanSegment, IntPtr labelsOut);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveScanSegmentGetLabelContourMask(
            IntPtr scanSegment, 
            IntPtr image,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool thickLine);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveScanSegmentRelease(ref IntPtr sharedPtr);
    }
}
