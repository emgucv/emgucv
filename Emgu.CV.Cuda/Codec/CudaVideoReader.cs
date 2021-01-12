//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// Cuda video reader
    /// </summary>
    public class CudaVideoReader : SharedPtrObject
    {
        /// <summary>
        /// Video codecs supported by VideoReader
        /// </summary>
        public enum Codec
        {
            /// <summary>
            /// MPEG1
            /// </summary>
            MPEG1 = 0,
            /// <summary>
            /// MPEG2
            /// </summary>
            MPEG2,
            /// <summary>
            /// MPEG4
            /// </summary>
            MPEG4,
            /// <summary>
            /// VC1
            /// </summary>
            VC1,
            /// <summary>
            /// H264
            /// </summary>
            H264,
            /// <summary>
            /// JPEG
            /// </summary>
            JPEG,
            /// <summary>
            /// H264_SVC
            /// </summary>
            H264_SVC,
            /// <summary>
            /// H264_MVC
            /// </summary>
            H264_MVC,
            /// <summary>
            /// HEVC
            /// </summary>
            HEVC,
            /// <summary>
            /// VP8
            /// </summary>
            VP8,
            /// <summary>
            /// VP9
            /// </summary>
            VP9,
            /// <summary>
            /// Number of codecs
            /// </summary>
            NumCodecs,
            /// <summary>
            /// Y,U,V (4:2:0)
            /// </summary>
            Uncompressed_YUV420 = (('I' << 24) | ('Y' << 16) | ('U' << 8) | ('V')),
            /// <summary>
            /// Y,V,U (4:2:0)
            /// </summary>
            Uncompressed_YV12 = (('Y' << 24) | ('V' << 16) | ('1' << 8) | ('2')),
            /// <summary>
            /// Y,UV  (4:2:0)
            /// </summary>
            Uncompressed_NV12 = (('N' << 24) | ('V' << 16) | ('1' << 8) | ('2')),
            /// <summary>
            /// YUYV/YUY2 (4:2:2)
            /// </summary>
            Uncompressed_YUYV = (('Y' << 24) | ('U' << 16) | ('Y' << 8) | ('V')),
            /// <summary>
            /// UYVY (4:2:2)
            /// </summary>
            Uncompressed_UYVY = (('U' << 24) | ('Y' << 16) | ('V' << 8) | ('Y'))
        }

        /// <summary>
        /// Chroma formats supported by VideoReader.
        /// </summary>
        public enum ChromaFormat
        {
            /// <summary>
            /// Monochrome
            /// </summary>
            Monochrome = 0,
            /// <summary>
            /// YUV420
            /// </summary>
            YUV420,
            /// <summary>
            /// YUV422
            /// </summary>
            YUV422,
            /// <summary>
            /// YUV444
            /// </summary>
            YUV444,
            /// <summary>
            /// Number of formats
            /// </summary>
            NumFormats
        }

        /// <summary>
        /// Struct providing information about video file format.
        /// </summary>
        public struct FormatInfo
        {
            /// <summary>
            /// The Codec
            /// </summary>
            public Codec Codec;
            /// <summary>
            /// The chroma format
            /// </summary>
            public ChromaFormat ChromaFormat;
            /// <summary>
            /// The Width
            /// </summary>
            public int Width;
            /// <summary>
            /// The Height
            /// </summary>
            public int Height;
        }

        /// <summary>
        /// Creates video reader.
        /// </summary>
        /// <param name="fileName">Name of the input video file.</param>
        public CudaVideoReader(String fileName)
        {
            using (CvString s = new CvString(fileName))
                _ptr = CudaInvoke.cudaVideoReaderCreate(s, ref _sharedPtr);
        }

        /// <summary>
        /// Grabs, decodes and returns the next video frame.
        /// </summary>
        /// <param name="frame">The frame</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <returns>If no frames has been grabbed (there are no more frames in video file), the methods return false . </returns>
        public bool NextFrame(GpuMat frame, Stream stream = null)
        {
            return CudaInvoke.cudaVideoReaderNextFrame(_ptr, frame, stream);
        }

        /// <summary>
        /// Get the information about video file format.
        /// </summary>
        public FormatInfo Format
        {
            get
            {
                FormatInfo fi = new FormatInfo();
                CudaInvoke.cudaVideoReaderFormat(_ptr, ref fi);
                return fi;
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this VideoReader
        /// </summary>
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
