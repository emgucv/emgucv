//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.Util;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Applies Ridge Detection Filter to an input image.
    /// Implements Ridge detection similar to the one in [Mathematica] (http://reference.wolfram.com/language/ref/RidgeFilter.html)
    /// using the eigen values from the Hessian Matrix of the input image using Sobel Derivatives.
    /// Additional refinement can be done using Skeletonization and Binarization.
    /// </summary>
    public class RidgeDetectionFilter : SharedPtrObject, IAlgorithm
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
        /// Create a Ridge detection filter.
        /// </summary>
        /// <param name="dDepthType">Specifies output image depth.</param>
        /// <param name="dChannels">Specifies output image channel.</param>
        /// <param name="dx">Order of derivative x</param>
        /// <param name="dy">Order of derivative y</param>
        /// <param name="ksize">Sobel kernel size</param>
        /// <param name="outDepthType">Converted format for output</param>
        /// <param name="outChannels">Converted format for output</param>
        /// <param name="scale">Optional scale value for derivative values</param>
        /// <param name="delta">Optional bias added to output</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        public RidgeDetectionFilter (
            CvEnum.DepthType dDepthType = CvEnum.DepthType.Cv32F,
            int dChannels = 1,
            int dx = 1,
            int dy = 1,
            int ksize = 3,
            CvEnum.DepthType outDepthType = CvEnum.DepthType.Cv8U, 
            int outChannels = 1,
            double scale = 1,
            double delta = 0,
            Emgu.CV.CvEnum.BorderType borderType = Emgu.CV.CvEnum.BorderType.Default)
		{
            _ptr = XImgprocInvoke.cveRidgeDetectionFilterCreate(
                CvInvoke.MakeType(dDepthType, dChannels), 
                dx, 
                dy, 
                ksize, 
                CvInvoke.MakeType(outDepthType, outChannels), 
                scale, 
                delta, 
                borderType, 
                ref _algorithm, 
                ref _sharedPtr);
		}

        /// <summary>
        /// Apply Ridge detection filter on input image.
        /// </summary>
        /// <param name="img">InputArray as supported by Sobel. img can be 1-Channel or 3-Channels.</param>
        /// <param name="output">Output image with ridges.</param>
        public void GetRidgeFilteredImage(IInputArray img, IOutputArray output)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (OutputArray oaOutput = output.GetOutputArray())
                XImgprocInvoke.cveRidgeDetectionFilterGetRidgeFilteredImage(_ptr, iaImg, oaOutput);
        }

        /// <inheritdoc />
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XImgprocInvoke.cveRidgeDetectionFilterRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRidgeDetectionFilterCreate(
	        int ddepth,
	        int dx,
	        int dy,
	        int ksize,
	        int outDtype,
	        double scale,
	        double delta,
            Emgu.CV.CvEnum.BorderType borderType,
	        ref IntPtr algorithm,
	        ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRidgeDetectionFilterGetRidgeFilteredImage(IntPtr ridgeDetection, IntPtr img, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRidgeDetectionFilterRelease(ref IntPtr sharedPtr);
    }
}
