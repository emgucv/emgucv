//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr DataLoggerCreate(int logLevel, int loggerId);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void DataLoggerRelease(ref IntPtr logger);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void DataLoggerRegisterCallback(
           IntPtr logger,
           Util.DataLoggerHelper.DataCallback messageCallback);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void DataLoggerLog(
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
        internal static extern int zlib_compress_bound(int length);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void zlib_compress2(IntPtr dataCompressed, ref int sizeDataCompressed, IntPtr dataOriginal, int sizeDataOriginal, int compressionLevel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void zlib_uncompress(IntPtr dataUncompressed, ref int sizeDataUncompressed, IntPtr compressedData, int sizeDataCompressed);
    }
}