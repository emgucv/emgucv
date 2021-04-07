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
            }
        }

        public void DetectEdges(IInputArray src)
        {
            using (InputArray iaSrc = src.GetInputArray())
                XImgprocInvoke.cveEdgeDrawingDetectEdges(_ptr, iaSrc);
        }

        public void GetEdgeImage(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingGetEdgeImage(_ptr, oaDst);
        }

        public void GetGradientImage(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingGetGradientImage(_ptr, oaDst);
        }

        public void DetectLines(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingGetGradientImage(_ptr, oaDst);
        }

        public void DetectDetectEllipses(IOutputArray dst)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                XImgprocInvoke.cveEdgeDrawingGetGradientImage(_ptr, oaDst);
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
