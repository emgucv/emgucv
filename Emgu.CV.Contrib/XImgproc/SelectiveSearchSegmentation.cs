//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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

    public class SelectiveSearchSegmentation : UnmanagedObject
    {
        public SelectiveSearchSegmentation()
        {
            _ptr = XImgprocInvoke.cveSelectiveSearchSegmentationCreate();
        }

        public void SetBaseImage(IInputArray image)
        {
            using (InputArray iaImage = image.GetInputArray())
                XImgprocInvoke.cveSelectiveSearchSegmentationSetBaseImage(_ptr, iaImage);
        }

        public void SwitchToSingleStrategy(int k, float sigma)
        {
            XImgprocInvoke.cveSelectiveSearchSegmentationSwitchToSingleStrategy(_ptr, k, sigma);
        }

        public void SwitchToSelectiveSearchFast(int baseK, int incK, float sigma)
        {
            XImgprocInvoke.cveSelectiveSearchSegmentationSwitchToSelectiveSearchFast(_ptr, baseK, incK, sigma);
        }

        public void SwitchToSelectiveSearchQuality(int baseK, int incK, float sigma)
        {
            XImgprocInvoke.cveSelectiveSearchSegmentationSwitchToSelectiveSearchQuality(_ptr, baseK, incK, sigma);
        }

        public void AddImage(IInputArray img)
        {
            using (InputArray iaImg = img.GetInputArray())
            XImgprocInvoke.cveSelectiveSearchSegmentationAddImage(_ptr, iaImg);
        }

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
                XImgprocInvoke.cveSelectiveSearchSegmentationRelease(ref _ptr);
            }
        }
    }


    public static partial class XImgprocInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSelectiveSearchSegmentationCreate();

        


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
        internal static extern void cveSelectiveSearchSegmentationRelease(ref IntPtr segmentation);
    }
}
