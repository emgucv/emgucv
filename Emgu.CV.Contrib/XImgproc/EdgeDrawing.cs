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
    /// Class implementing the ED (EdgeDrawing)
    /// </summary>
    public class EdgeDrawing : SharedPtrObject, IAlgorithm
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
        /// Create a new Edge Drawing object using default parameters.
        /// </summary>
        public EdgeDrawing()
		{
            _ptr = XImgprocInvoke.cveEdgeDrawingCreate(ref _algorithm, ref _sharedPtr);
		}
        

        /// <inheritdoc />
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XImgprocInvoke.cveEdgeDrawingRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
                _algorithm = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Detects edges and prepares them to detect lines and ellipses.
        /// </summary>
        /// <param name="src">Input image</param>
        public void DetectEdges(IInputArray src)
        {
            using (InputArray iaSrc = src.GetInputArray())
                XImgprocInvoke.cveEdgeDrawingDetectEdges(_ptr, iaSrc);
        }

        /// <summary>
        /// Get the edge image
        /// </summary>
        /// <param name="dst">The output edge image</param>
        public void GetEdgeImage(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingGetEdgeImage(_ptr, oaDst);
        }

        /// <summary>
        /// Get the gradient image
        /// </summary>
        /// <param name="dst">The output gradient image</param>
        public void GetGradientImage(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingGetGradientImage(_ptr, oaDst);
        }

        /// <summary>
        /// Detects lines.
        /// </summary>
        /// <param name="dst">Output Vec&lt;4f&gt; contains start point and end point of detected lines.</param>
        public void DetectLines(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingDetectLines(_ptr, oaDst);
        }

        /// <summary>
        /// Detects circles and ellipses.
        /// </summary>
        /// <param name="dst">Output Vec&lt;6d&gt; contains center point and perimeter for circles.</param>
        public void DetectEllipses(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingDetectEllipses(_ptr, oaDst);
        }
    }

    public static partial class XImgprocInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveEdgeDrawingCreate(
	        ref IntPtr algorithm,
	        ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEdgeDrawingDetectEdges(
            IntPtr edgeDrawing, 
            IntPtr src);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEdgeDrawingGetEdgeImage(IntPtr edgeDrawing, IntPtr dst);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEdgeDrawingGetGradientImage(IntPtr edgeDrawing, IntPtr dst);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEdgeDrawingDetectLines(IntPtr edgeDrawing, IntPtr lines);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEdgeDrawingDetectEllipses(IntPtr edgeDrawing, IntPtr ellipses);
        
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveEdgeDrawingRelease(ref IntPtr sharedPtr);
    }
}
