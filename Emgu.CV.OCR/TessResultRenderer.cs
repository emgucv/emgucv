//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
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
                _ptr = OcrInvoke.cveTessPDFRendererCreate(csOutputBase, csDataDir, textOnly, ref _tessResultRendererPtr);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this Renderer
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                OcrInvoke.cveTessPDFRendererRelease(ref _ptr);
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
        internal static extern IntPtr cveTessPDFRendererCreate(
            IntPtr outputbase,
            IntPtr datadir,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool textonly,
            ref IntPtr resultRenderer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessPDFRendererRelease(ref IntPtr renderer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTessResultRendererBeginDocument(IntPtr resultRenderer, IntPtr title);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTessResultRendererAddImage(IntPtr resultRenderer, IntPtr api);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTessResultRendererEndDocument(IntPtr resultRenderer);

        /// <summary>
        /// Starts a new document with the given title.
        /// This clears the contents of the output data.
        /// </summary>
        /// <param name="renderer">The renderer</param>
        /// <param name="title">The title</param>
        /// <returns>True if successful</returns>
        public static bool BeginDocument(this ITessResultRenderer renderer, String title)
        {
            using (CvString csTitle = new CvString(title))
            {
                return cveTessResultRendererBeginDocument(renderer.TessResultRendererPtr, csTitle);
            }
        }

        /// <summary>
        /// Adds the recognized text from the source image to the current document. Invalid if BeginDocument not yet called.
        /// </summary>
        /// <param name="renderer">The result rendered</param>
        /// <param name="api">The Tesseract obj</param>
        /// <returns>True if successful</returns>
        public static bool AddImage(this ITessResultRenderer renderer, Tesseract api)
        {
            return cveTessResultRendererAddImage(renderer.TessResultRendererPtr, api.Ptr);
        }

        /// <summary>
        /// Finishes the document and finalizes the output data. Invalid if BeginDocument not yet called.
        /// </summary>
        /// <param name="renderer">The renderer</param>
        /// <returns>True if successful</returns>
        public static bool EndDocument(this ITessResultRenderer renderer)
        {
            return cveTessResultRendererEndDocument(renderer.TessResultRendererPtr);
        }
    }
}
