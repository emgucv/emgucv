//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.OCR
{
    /// <summary>
    /// Renders tesseract output into searchable PDF
    /// </summary>
    public class PDFRenderer : UnmanagedObject, ITessResultRenderer
    {
        private IntPtr _tessResultRendererPtr;

        /// <summary>
        /// Create a PDF renderer
        /// </summary>
        /// <param name="outputBase">Output base</param>
        /// <param name="dataDir">dataDir is the location of the TESSDATA. We need it because we load a custom PDF font from this location.</param>
        /// <param name="textOnly">Text only</param>
        public PDFRenderer(String outputBase, String dataDir, bool textOnly)
        {
            using (CvString csOutputBase = new CvString(outputBase))
            using (CvString csDataDir = new CvString(dataDir))
            {
                _ptr = OcrInvoke.TessPDFRendererCreate(csOutputBase, csDataDir, textOnly, ref _tessResultRendererPtr);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Renderer
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                OcrInvoke.TessPDFRendererRelease(ref _ptr);
            _tessResultRendererPtr = IntPtr.Zero;
        }

        /// <summary>
        /// Pointer to the unmanaged TessResultRendered
        /// </summary>
        public IntPtr TessResultRendererPtr
        {
            get
            {
                return _tessResultRendererPtr;
            }
        }
    }

    public static partial class OcrInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr TessPDFRendererCreate(
            IntPtr outputbase, 
            IntPtr datadir, 
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool textonly, 
            ref IntPtr resultRenderer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void TessPDFRendererRelease(ref IntPtr renderer);
    }
}
