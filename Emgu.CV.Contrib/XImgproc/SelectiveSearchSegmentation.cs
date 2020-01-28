//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Selective search segmentation algorithm The class implements the algorithm described in:
    /// Jasper RR Uijlings, Koen EA van de Sande, Theo Gevers, and Arnold WM Smeulders. Selective search for object recognition. International journal of computer vision, 104(2):154–171, 2013.
    /// </summary>
    public class SelectiveSearchSegmentation : UnmanagedObject
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Selective search segmentation algorithm
        /// </summary>
        public SelectiveSearchSegmentation()
        {
            _ptr = XImgprocInvoke.cveSelectiveSearchSegmentationCreate(ref _sharedPtr);
        }

        /// <summary>
        /// Set a image used by switch* functions to initialize the class.
        /// </summary>
        /// <param name="image">The image</param>
        public void SetBaseImage(IInputArray image)
        {
            using (InputArray iaImage = image.GetInputArray())
                XImgprocInvoke.cveSelectiveSearchSegmentationSetBaseImage(_ptr, iaImage);
        }

        /// <summary>
        /// Initialize the class with the 'Single stragegy' parameters
        /// </summary>
        /// <param name="k">The k parameter for the graph segmentation</param>
        /// <param name="sigma">The sigma parameter for the graph segmentation</param>
        public void SwitchToSingleStrategy(int k, float sigma)
        {
            XImgprocInvoke.cveSelectiveSearchSegmentationSwitchToSingleStrategy(_ptr, k, sigma);
        }

        /// <summary>
        /// Initialize the class with the 'Selective search fast' parameters
        /// </summary>
        /// <param name="baseK">The k parameter for the first graph segmentation</param>
        /// <param name="incK">The increment of the k parameter for all graph segmentations</param>
        /// <param name="sigma">The sigma parameter for the graph segmentation</param>
        public void SwitchToSelectiveSearchFast(int baseK = 150, int incK = 150, float sigma = 0.8f)
        {
            XImgprocInvoke.cveSelectiveSearchSegmentationSwitchToSelectiveSearchFast(_ptr, baseK, incK, sigma);
        }

        /// <summary>
        /// Initialize the class with the 'Selective search quality' parameters
        /// </summary>
        /// <param name="baseK">The k parameter for the first graph segmentation</param>
        /// <param name="incK">The increment of the k parameter for all graph segmentations</param>
        /// <param name="sigma">The sigma parameter for the graph segmentation</param>
        public void SwitchToSelectiveSearchQuality(int baseK = 150, int incK = 150, float sigma = 0.8f)
        {
            XImgprocInvoke.cveSelectiveSearchSegmentationSwitchToSelectiveSearchQuality(_ptr, baseK, incK, sigma);
        }

        /// <summary>
        /// Add a new image in the list of images to process.
        /// </summary>
        /// <param name="img">	The image</param>
        public void AddImage(IInputArray img)
        {
            using (InputArray iaImg = img.GetInputArray())
                XImgprocInvoke.cveSelectiveSearchSegmentationAddImage(_ptr, iaImg);
        }

        /// <summary>
        /// Based on all images, graph segmentations and stragies, computes all possible rects and return them.
        /// </summary>
        /// <returns>	The list of rects. The first ones are more relevents than the lasts ones.</returns>
        public Rectangle[] Process()
        {
            using (VectorOfRect vr = new VectorOfRect())
            {
                XImgprocInvoke.cveSelectiveSearchSegmentationProcess(_ptr, vr);
                return vr.ToArray();
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XImgprocInvoke.cveSelectiveSearchSegmentationRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }


    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSelectiveSearchSegmentationCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSelectiveSearchSegmentationSetBaseImage(IntPtr segmentation, IntPtr image);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSelectiveSearchSegmentationSwitchToSingleStrategy(IntPtr segmentation, int k, float sigma);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSelectiveSearchSegmentationSwitchToSelectiveSearchFast(IntPtr segmentation, int baseK, int incK, float sigma);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSelectiveSearchSegmentationSwitchToSelectiveSearchQuality(IntPtr segmentation, int baseK, int incK, float sigma);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSelectiveSearchSegmentationAddImage(IntPtr segmentation, IntPtr img);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSelectiveSearchSegmentationProcess(IntPtr segmentation, IntPtr rects);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSelectiveSearchSegmentationRelease(ref IntPtr segmentation, ref IntPtr sharedPtr);
    }
}
