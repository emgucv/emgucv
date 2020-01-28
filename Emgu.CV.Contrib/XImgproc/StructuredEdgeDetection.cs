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
    /// Class implementing edge detection algorithm from Piotr Dollar and C Lawrence Zitnick. Structured forests for fast edge detection. In Computer Vision (ICCV), 2013 IEEE International Conference on, pages 1841-1848. IEEE, 2013.
    /// </summary>
    public class StructuredEdgeDetection : UnmanagedObject
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create an edge detection algorithm.
        /// </summary>
        /// <param name="model">name of the file where the model is stored</param>
        /// <param name="howToGetFeatures">optional object inheriting from RFFeatureGetter. You need it only if you would like to train your own forest, pass NULL otherwise</param>
        public StructuredEdgeDetection(String model, RFFeatureGetter howToGetFeatures)
        {
            using (CvString sModel = new CvString(model))
                _ptr = XImgprocInvoke.cveStructuredEdgeDetectionCreate(sModel, howToGetFeatures, ref _sharedPtr);
        }

        /// <summary>
        /// The function detects edges in src and draw them to dst. The algorithm underlies this function is much more robust to texture presence, than common approaches, e.g. Sobel
        /// </summary>
        /// <param name="src">source image (RGB, float, in [0;1]) to detect edges</param>
        /// <param name="dst">destination image (grayscale, float, in [0;1]) where edges are drawn</param>
        public void DetectEdges(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveStructuredEdgeDetectionDetectEdges(_ptr, iaSrc, oaDst);
        }

        /// <summary>
        /// The function computes orientation from edge image.
        /// </summary>
        /// <param name="src">Edge image.</param>
        /// <param name="dst">Orientation image.</param>
        public void ComputeOrientation(IInputArray src, IOutputArray dst)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveStructuredEdgeDetectionComputeOrientation(_ptr, iaSrc, oaDst);
        }

        /// <summary>
        /// The function edgenms in edge image and suppress edges where edge is stronger in orthogonal direction.
        /// </summary>
        /// <param name="edgeImage">edge image from DetectEdges function.</param>
        /// <param name="orientationImage">orientation image from ComputeOrientation function.</param>
        /// <param name="dst">Suppressed image (grayscale, float, in [0;1])</param>
        /// <param name="r">Radius for NMS suppression.</param>
        /// <param name="s">Radius for boundary suppression.</param>
        /// <param name="m">Multiplier for conservative suppression.</param>
        /// <param name="isParallel">Enables/disables parallel computing.</param>
        public void EdgesNms(IInputArray edgeImage, IInputArray orientationImage, IOutputArray dst, int r = 2, int s = 0, float m = 1, bool isParallel = true)
        {
            using (InputArray iaEdgeImage = edgeImage.GetInputArray())
            using (InputArray iaOrientationImage = orientationImage.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                XImgprocInvoke.cveStructuredEdgeDetectionEdgesNms(_ptr, iaEdgeImage, iaOrientationImage, oaDst, r, s, m, isParallel);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XImgprocInvoke.cveStructuredEdgeDetectionRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }

    /// <summary>
    /// Helper class for training part of [P. Dollar and C. L. Zitnick. Structured Forests for Fast Edge Detection, 2013].
    /// </summary>
    public class RFFeatureGetter : UnmanagedObject
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a default RFFeatureGetter
        /// </summary>
        public RFFeatureGetter()
        {
            _ptr = XImgprocInvoke.cveRFFeatureGetterCreate(ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this RFFeatureGetter.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XImgprocInvoke.cveRFFeatureGetterRelease(ref _ptr, ref _sharedPtr);
            }
        }
    }

    /// <summary>
    /// Library to invoke XImgproc functions
    /// </summary>
    public static partial class XImgprocInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStructuredEdgeDetectionCreate(IntPtr model, IntPtr howToGetFeatures, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStructuredEdgeDetectionDetectEdges(IntPtr detection, IntPtr src, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStructuredEdgeDetectionRelease(ref IntPtr detection, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRFFeatureGetterCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRFFeatureGetterRelease(ref IntPtr getter, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStructuredEdgeDetectionComputeOrientation(IntPtr detection, IntPtr src, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStructuredEdgeDetectionEdgesNms(
            IntPtr detection, 
            IntPtr edgeImage, 
            IntPtr orientationImage, 
            IntPtr dst, 
            int r, 
            int s, 
            float m, 
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool isParallel);
    }
}
