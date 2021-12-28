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
    /// <summary>
    /// Class implementing the F-DBSCAN (Accelerated superpixel image segmentation with a parallelized DBSCAN algorithm) superpixels algorithm by Loke SC, et al.
    /// </summary>
    /// <remarks>The algorithm uses a parallelised DBSCAN cluster search that is resistant to noise, competitive in segmentation quality, and faster than existing superpixel segmentation methods. When tested on the Berkeley Segmentation Dataset, the average processing speed is 175 frames/s with a Boundary Recall of 0.797 and an Achievable Segmentation Accuracy of 0.944. The computational complexity is quadratic O(n2) and more suited to smaller images, but can still process a 2MP colour image faster than the SEEDS algorithm in OpenCV. The output is deterministic when the number of processing threads is fixed, and requires the source image to be in Lab colour format.</remarks>
    public partial class ScanSegment : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithm;

        /// <summary>
        /// Pointer to cv::Algorithm
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithm; }
        }

        /// <summary>
        /// Initializes a ScanSegment object.
        /// </summary>
        /// <param name="imageWidth">Image width.</param>
        /// <param name="imageHeight">Image height.</param>
        /// <param name="numSuperpixels">Desired number of superpixels. Note that the actual number may be smaller due to restrictions (depending on the image size). Use NumberOfSuperpixels to get the actual number.</param>
        /// <param name="slices">Number of processing threads for parallelisation. Setting -1 uses the maximum number of threads. In practice, four threads is enough for smaller images and eight threads for larger ones.</param>
        /// <param name="mergeSmall">Merge small segments to give the desired number of superpixels. Processing is much faster without merging, but many small segments will be left in the image.</param>
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

        /// <summary>
        /// Calculates the superpixel segmentation on a given image with the initialized parameters in the ScanSegment object. This function can be called again for other images without the need of initializing the algorithm with createScanSegment(). This save the computational cost of allocating memory for all the structures of the algorithm.
        /// </summary>
        /// <param name="img">Input image. Supported format: CV_8UC3. Image size must match with the initialized image size with the function createScanSegment(). It MUST be in Lab color space.</param>
        public void Iterate(IInputArray img)
        {
            using(InputArray iaImg = img.GetInputArray())
                XImgprocInvoke.cveScanSegmentIterate(_ptr, iaImg);
        }

        /// <summary>
        /// Returns the segmentation labeling of the image. Each label represents a superpixel, and each pixel is assigned to one superpixel label.
        /// </summary>
        /// <param name="labelsOut">A CV_32UC1 integer array containing the labels of the superpixel segmentation. The labels are in the range [0, NumberOfSuperpixels].</param>
        public void GetLabels(IOutputArray labelsOut)
        {
            using (OutputArray oaLabelsOut = labelsOut.GetOutputArray())
                XImgprocInvoke.cveScanSegmentGetLabels(_ptr, oaLabelsOut);
        }

        /// <summary>
        /// Returns the mask of the superpixel segmentation stored in the ScanSegment object.
        /// </summary>
        /// <param name="image">CV_8UC1 image mask where 255 indicates that the pixel is a superpixel border, and 0 otherwise.</param>
        /// <param name="thickLine">If false, the border is only one pixel wide, otherwise all pixels at the border are masked.</param>
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
                _algorithm = IntPtr.Zero;
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
