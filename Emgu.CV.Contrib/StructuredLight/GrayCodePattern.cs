//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
using System.Drawing;
using Emgu.CV.Quality;

namespace Emgu.CV.StructuredLight
{
    /// <summary>
    /// Class implementing the Gray-code pattern, based on 
    /// Kyriakos Herakleous and Charalambos Poullis. 3DUNDERWORLD-SLS: An Open-Source Structured-Light Scanning System for Rapid Geometry Acquisition. arXiv preprint arXiv:1406.6595, 2014.
    /// </summary>
    public partial class GrayCodePattern : SharedPtrObject, IStructuredLightPattern
    {
        private IntPtr _structuredLightPatternPtr;
        private IntPtr _algorithmPtr;


        /// <summary>
        /// Create a new GrayCodePattern
        /// </summary>
        /// <param name="width">The width of the projector.</param>
        /// <param name="height">The height of the projector.</param>
        public GrayCodePattern(
            int width = 1024,
            int height = 768)
        {
            _ptr = StructuredLightInvoke.cveGrayCodePatternCreate(
                width,
                height,
                ref _sharedPtr,
                ref _structuredLightPatternPtr,
                ref _algorithmPtr);
        }

        /// <inheritdoc/>        
        public IntPtr StructuredLightPatternPtr
        {
            get
            {
                return _structuredLightPatternPtr;
            }
        }

        /// <inheritdoc/>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Generates the all-black and all-white images needed for shadowMasks computation. To identify shadow regions, the regions of two images where the pixels are not lit by projector's light and thus where there is not coded information, the 3DUNDERWORLD algorithm computes a shadow mask for the two cameras views, starting from a white and a black images captured by each camera. This method generates these two additional images to project.
        /// </summary>
        /// <param name="blackImage">The generated all-black CV_8U image, at projector's resolution.</param>
        /// <param name="whiteImage">The generated all-white CV_8U image, at projector's resolution.</param>
        public void GetImagesForShadowMasks(IInputOutputArray blackImage, IInputOutputArray whiteImage)
        {
            using (InputOutputArray ioaBlackImage = blackImage.GetInputOutputArray())
            using (InputOutputArray ioaWhiteImage = whiteImage.GetInputOutputArray())
            {
                StructuredLightInvoke.cveGrayCodePatternGetImagesForShadowMasks(_ptr, ioaBlackImage, ioaWhiteImage);
            }
        }

        /// <summary>
        /// For a (x,y) pixel of a camera returns the corresponding projector pixel. The function decodes each pixel in the pattern images acquired by a camera into their corresponding decimal numbers representing the projector's column and row, providing a mapping between camera's and projector's pixel.
        /// </summary>
        /// <param name="patternImages">The pattern images acquired by the camera, stored in a grayscale VectorOfMat.</param>
        /// <param name="x">x coordinate of the image pixel.</param>
        /// <param name="y">y coordinate of the image pixel.</param>
        /// <returns>Projector's pixel corresponding to the camera's pixel: projPix.x and projPix.y are the image coordinates of the projector's pixel corresponding to the pixel being decoded in a camera. If failed to calculate the project, null will be returned.</returns>
        public Point? GetProjPixel(IInputArray patternImages, int x, int y)
        {
            using (InputArray iaPatternImages = patternImages.GetInputArray())
            {
                Point projPix = new Point();
                bool found =
                    StructuredLightInvoke.cveGrayCodePatternGetProjPixel(_ptr, iaPatternImages, x, y, ref projPix);
                if (!found)
                    return null;
                return projPix;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                StructuredLightInvoke.cveGrayCodePatternRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _structuredLightPatternPtr = IntPtr.Zero;
                _algorithmPtr = IntPtr.Zero;
            }
        }
    }


    /// <summary>
    /// Provide interfaces to the Open CV StructuredLight functions
    /// </summary>
    public static partial class StructuredLightInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGrayCodePatternCreate(
            int width,
            int height,
            ref IntPtr sharedPtr,
            ref IntPtr structuredLightPattern,
            ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGrayCodePatternRelease(ref IntPtr pattern);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGrayCodePatternGetImagesForShadowMasks(IntPtr grayCodePattern, IntPtr blackImage, IntPtr whiteImage);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveGrayCodePatternGetProjPixel(IntPtr grayCodePattern, IntPtr patternImages, int x, int y, ref Point projPix);

    }
}