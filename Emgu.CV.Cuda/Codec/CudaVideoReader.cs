//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    public class CudaVideoReader : SharedPtrObject
    {
        public enum Codec
        {
            MPEG1 = 0,
            MPEG2,
            MPEG4,
            VC1,
            H264,
            JPEG,
            H264_SVC,
            H264_MVC,
            HEVC,
            VP8,
            VP9,
            NumCodecs,
            Uncompressed_YUV420 = (('I' << 24) | ('Y' << 16) | ('U' << 8) | ('V')),   //!< Y,U,V (4:2:0)
            Uncompressed_YV12 = (('Y' << 24) | ('V' << 16) | ('1' << 8) | ('2')),   //!< Y,V,U (4:2:0)
            Uncompressed_NV12 = (('N' << 24) | ('V' << 16) | ('1' << 8) | ('2')),   //!< Y,UV  (4:2:0)
            Uncompressed_YUYV = (('Y' << 24) | ('U' << 16) | ('Y' << 8) | ('V')),   //!< YUYV/YUY2 (4:2:2)
            Uncompressed_UYVY = (('U' << 24) | ('Y' << 16) | ('V' << 8) | ('Y'))    //!< UYVY (4:2:2)
        }

        public enum ChromaFormat
        {
            Monochrome = 0,
            YUV420,
            YUV422,
            YUV444,
            NumFormats
        }

        public struct FormatInfo
        {
            Codec codec;
            ChromaFormat chromaFormat;
            int width;
            int height;
        }

        public CudaVideoReader(String fileName)
        {
            using (CvString s = new CvString(fileName))
                _ptr = CudaInvoke.cudaVideoReaderCreate(s, ref _sharedPtr);
        }

        public bool NextFrame(GpuMat frame, Stream stream = null)
        {
            return CudaInvoke.cudaVideoReaderNextFrame(_ptr, frame, stream);
        }

        public FormatInfo Format
        {
            get
            {
                FormatInfo fi = new FormatInfo();
                CudaInvoke.cudaVideoReaderFormat(_ptr, ref fi);
                return fi;
            }
        }

        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CudaInvoke.cudaVideoReaderRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaVideoReaderCreate(IntPtr fileName, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaVideoReaderRelease(ref IntPtr reader);


        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cudaVideoReaderNextFrame(IntPtr reader, IntPtr frame, IntPtr stream);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaVideoReaderFormat(IntPtr reader, ref CudaVideoReader.FormatInfo formatInfo);
    }
}
