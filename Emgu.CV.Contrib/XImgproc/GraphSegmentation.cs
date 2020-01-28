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


namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Graph Based Segmentation Algorithm. The class implements the algorithm described in Pedro F Felzenszwalb and Daniel P Huttenlocher. Efficient graph-based image segmentation. volume 59, pages 167 - 181. Springer, 2004.
    /// </summary>
    public class GraphSegmentation : UnmanagedObject
    {
        private IntPtr _sharedPtr;
        /// <summary>
        /// Creates a graph based segmentor.
        /// </summary>
        /// <param name="sigma">The sigma parameter, used to smooth image</param>
        /// <param name="k">The k parameter of the algorithm</param>
        /// <param name="minSize">The minimum size of segments</param>
        public GraphSegmentation(double sigma = 0.5, float k = 300, int minSize = 100)
        {
            _ptr = XImgprocInvoke.cveGraphSegmentationCreate(sigma, k, minSize, ref _sharedPtr);
        }

        /// <summary>
        /// Segment an image and store output in dst.
        /// </summary>
        /// <param name="src">The input image. Any number of channel (1 (Eg: Gray), 3 (Eg: RGB), 4 (Eg: RGB-D)) can be provided</param>
        /// <param name="dst">The output segmentation. It's a CV_32SC1 Mat with the same number of cols and rows as input image, with an unique, sequential, id for each pixel.</param>
        public void ProcessImage(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveGraphSegmentationProcessImage(_ptr, iaSrc, oaDst);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XImgprocInvoke.cveGraphSegmentationRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }


    public static partial class XImgprocInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGraphSegmentationCreate(
            double sigma, float k, int minSize,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGraphSegmentationProcessImage(IntPtr segmentation, IntPtr src, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGraphSegmentationRelease(ref IntPtr segmentation, ref IntPtr sharedPtr);
    }
}
