//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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
    /// Library to invoke Tesseract OCR functions
    /// </summary>
    public static partial class OcrInvoke
    {
        static OcrInvoke()
        {
            //dummy code that is used to involve the static constructor of CvInvoke, if it has not already been called.
            CvInvoke.Init();
        }

        #region Tesseract
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTessBaseAPICreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveTessBaseAPIInit(
            IntPtr ocr,
            IntPtr dataPath,
            IntPtr language,
            OcrEngineMode mode);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIRelease(ref IntPtr ocr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPISetImage(IntPtr ocr, IntPtr image);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPISetImagePix(IntPtr ocr, IntPtr pix);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIGetUTF8Text(
           IntPtr ocr,
           IntPtr text);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIGetHOCRText(IntPtr ocr, int pageNumber, IntPtr vectorOfByte);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIGetTSVText(IntPtr ocr, int pageNumber, IntPtr vectorOfByte);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIGetBoxText(IntPtr ocr, int pageNumber, IntPtr vectorOfByte);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIGetUNLVText(IntPtr ocr, IntPtr vectorOfByte);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIGetOsdText(IntPtr ocr, int pageNumber, IntPtr vectorOfByte);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPIExtractResult(IntPtr ocr, IntPtr charSeq, IntPtr resultSeq);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTessBaseAPIProcessPage(
            IntPtr ocr,
            IntPtr pix,
            int pageIndex,
            IntPtr filename,
            IntPtr retryConfig,
            int timeoutMillisec,
            IntPtr renderer);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTessBaseAPISetVariable(
            IntPtr ocr,
            [MarshalAs(CvInvoke.StringMarshalType)]
            String varName,
            [MarshalAs(CvInvoke.StringMarshalType)]
            String value);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTesseractGetVersion();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessBaseAPISetPageSegMode(IntPtr ocr, PageSegMode mode);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern PageSegMode cveTessBaseAPIGetPageSegMode(IntPtr ocr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveTessBaseAPIGetOpenCLDevice(IntPtr ocr, ref IntPtr device);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTessBaseAPIAnalyseLayout(
            IntPtr ocr,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool mergeSimilarWords);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveTessBaseAPIIsValidWord(
            IntPtr ocr,
            [MarshalAs(CvInvoke.StringMarshalType)]
            String word);
        #endregion

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessPageIteratorRelease(ref IntPtr iterator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTessPageIteratorGetOrientation(IntPtr iterator, ref PageOrientation orientation, ref WritingDirection writingDirection, ref TextlineOrder order, ref float deskewAngle);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveTessPageIteratorGetBaseLine(
           IntPtr iterator,
           PageIteratorLevel level,
           ref int x1, ref int y1, ref int x2, ref int y2);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern OcrEngineMode cveTessBaseAPIGetOem(IntPtr ocr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveTessBaseAPIRecognize(IntPtr ocr);

    }
}

