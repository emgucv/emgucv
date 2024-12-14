//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDataLoggerCreate(int logLevel, int loggerId);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDataLoggerRelease(ref IntPtr logger);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDataLoggerRegisterCallback(
           IntPtr logger,
           Util.DataLoggerHelper.DataCallback messageCallback);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDataLoggerLog(
           IntPtr logger,
           IntPtr data,
           int logLevel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cvGetImageSubRect(IntPtr imagePtr, ref Rectangle rect);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMemcpy(IntPtr dst, IntPtr src, int length);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr tbbTaskSchedulerInit();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void tbbTaskSchedulerRelease(ref IntPtr scheduler);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern int zlib_compress_bound(int length);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void zlib_compress2(IntPtr dataCompressed, ref int sizeDataCompressed, IntPtr dataOriginal, int sizeDataOriginal, int compressionLevel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        public static extern void zlib_uncompress(IntPtr dataUncompressed, ref int sizeDataUncompressed, IntPtr compressedData, int sizeDataCompressed);
    }
}