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
    /// Class implementing the SEEDS (Superpixels Extracted via Energy-Driven Sampling) superpixels algorithm described in Michael Van den Bergh, Xavier Boix, Gemma Roig, Benjamin de Capitani, and Luc Van Gool. Seeds: Superpixels extracted via energy-driven sampling. In Computer Vision-ECCV 2012, pages 13-26. Springer, 2012.
    /// </summary>
    public class SupperpixelSEEDS : UnmanagedObject
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// The function initializes a SuperpixelSEEDS object for the input image.
        /// </summary>
        /// <param name="imageWidth">Image width</param>
        /// <param name="imageHeight">Image height</param>
        /// <param name="imageChannels">Number of channels of the image.</param>
        /// <param name="numSuperpixels">Desired number of superpixels. Note that the actual number may be smaller due to restrictions (depending on the image size and num_levels). Use getNumberOfSuperpixels() to get the actual number.</param>
        /// <param name="numLevels">Number of block levels. The more levels, the more accurate is the segmentation, but needs more memory and CPU time.</param>
        /// <param name="prior">Enable 3x3 shape smoothing term if >0. A larger value leads to smoother shapes. prior must be in the range [0, 5].</param>
        /// <param name="histogramBins">Number of histogram bins.</param>
        /// <param name="doubleStep">If true, iterate each block level twice for higher accuracy.</param>
        public SupperpixelSEEDS(int imageWidth, int imageHeight, int imageChannels,
           int numSuperpixels, int numLevels, int prior,
           int histogramBins,
           bool doubleStep)
        {
            _ptr = XImgprocInvoke.cveSuperpixelSEEDSCreate(
               imageWidth, imageHeight, imageChannels,
               numSuperpixels, numLevels, prior,
               histogramBins, doubleStep, ref _sharedPtr);
        }

        /// <summary>
        /// The function computes the superpixels segmentation of an image with the parameters initialized with the function createSuperpixelSEEDS().
        /// </summary>
        public int NumberOfSuperpixels
        {
            get { return XImgprocInvoke.cveSuperpixelSEEDSGetNumberOfSuperpixels(_ptr); }
        }

        /// <summary>
        /// Returns the segmentation labeling of the image.
        /// Each label represents a superpixel, and each pixel is assigned to one superpixel label.
        /// </summary>
        /// <param name="labels">Return: A CV_32UC1 integer array containing the labels of the superpixel segmentation. The labels are in the range [0, NumberOfSuperpixels].</param>
        public void GetLabels(IOutputArray labels)
        {
            using (OutputArray oaLabels = labels.GetOutputArray())
                XImgprocInvoke.cveSuperpixelSEEDSGetLabels(_ptr, oaLabels);
        }

        /// <summary>
        /// Returns the mask of the superpixel segmentation stored in SuperpixelSEEDS object.
        /// </summary>
        /// <param name="image">Return: CV_8UC1 image mask where -1 indicates that the pixel is a superpixel border, and 0 otherwise.</param>
        /// <param name="thickLine">If false, the border is only one pixel wide, otherwise all pixels at the border are masked.</param>
        public void GetLabelContourMask(IOutputArray image, bool thickLine = false)
        {
            using (OutputArray oaImage = image.GetOutputArray())
                XImgprocInvoke.cveSuperpixelSEEDSGetLabelContourMask(_ptr, oaImage, thickLine);
        }

        /// <summary>
        /// Calculates the superpixel segmentation on a given image with the initialized parameters in the SuperpixelSEEDS object.
        /// </summary>
        /// <remarks>This function can be called again for other images without the need of initializing the algorithm with createSuperpixelSEEDS(). This save the computational cost of allocating memory for all the structures of the algorithm.</remarks>
        /// <param name="img">Input image. Supported formats: CV_8U, CV_16U, CV_32F. Image size &amp; number of channels must match with the initialized image size &amp; channels with the function createSuperpixelSEEDS(). It should be in HSV or Lab color space. Lab is a bit better, but also slower.</param>
        /// <param name="numIterations">Number of pixel level iterations. Higher number improves the result.</param>
        public void Iterate(IInputArray img, int numIterations = 4)
        {
            using (InputArray iaImg = img.GetInputArray())
                XImgprocInvoke.cveSuperpixelSEEDSIterate(_ptr, iaImg, numIterations);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XImgprocInvoke.cveSuperpixelSEEDSRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }


    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSuperpixelSEEDSCreate(
           int imageWidth, int imageHeight, int imageChannels,
           int numSuperpixels, int numLevels, int prior,
           int histogramBins,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool doubleStep,
           ref IntPtr sharedPtr
           );

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveSuperpixelSEEDSGetNumberOfSuperpixels(IntPtr seeds);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSuperpixelSEEDSGetLabels(IntPtr seeds, IntPtr labelsOut);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSuperpixelSEEDSGetLabelContourMask(
           IntPtr seeds,
           IntPtr image,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool thickLine);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSuperpixelSEEDSRelease(ref IntPtr seeds, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSuperpixelSEEDSIterate(IntPtr seeds, IntPtr img, int numIterations);
    }
}
