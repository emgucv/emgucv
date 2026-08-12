//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features;
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
        /// Deinterlacing mode used by decoder.
        /// </summary>
        public enum DeinterlaceMode
        {
            /// <summary>
            /// Weave both fields (no deinterlacing). For progressive content and for content that doesn't need deinterlacing.
            /// </summary>
            Weave = 0,
            /// <summary>
            /// Drop one field.
            /// </summary>
            Bob = 1,
            /// <summary>
            /// Adaptive deinterlacing needs more video memory than other deinterlacing modes.
            /// </summary>
            Adaptive = 2
        }

        /// <summary>
        /// Video surface formats output by the decoder.
        /// </summary>
        public enum SurfaceFormat
        {
            /// <summary>
            /// Semi-Planar YUV [Y plane followed by interleaved UV plane]
            /// </summary>
            Nv12 = 0,
            /// <summary>
            /// 16 bit Semi-Planar YUV [Y plane followed by interleaved UV plane]. Can be used for 10 bit(6LSB bits 0), 12 bit (4LSB bits 0)
            /// </summary>
            P016 = 1,
            /// <summary>
            /// Planar YUV [Y plane followed by U and V planes]
            /// </summary>
            Yuv444 = 2,
            /// <summary>
            /// 16 bit Planar YUV [Y plane followed by U and V planes]. Can be used for 10 bit(6LSB bits 0), 12 bit (4LSB bits 0)
            /// </summary>
            Yuv444_16Bit = 3
        }

        /// <summary>
        /// Video Signal Description Color Primaries
        /// </summary>
        public enum ColorSpaceStandard
        {
            /// <summary>
            /// ITU-R BT.709 standard for high-definition television.
            /// </summary>
            Bt709 = 1,
            /// <summary>
            /// Unspecified color space standard.
            /// </summary>
            Unspecified = 2,
            /// <summary>
            /// Reserved for future use.
            /// </summary>
            Reserved = 3,
            /// <summary>
            /// FCC color space standard.
            /// </summary>
            Fcc = 4,
            /// <summary>
            /// ITU-R BT.470, used for older analog television systems.
            /// </summary>
            Bt470 = 5,
            /// <summary>
            /// ITU-R BT.601, used for standard definition television.
            /// </summary>
            Bt601 = 6,
            /// <summary>
            /// SMPTE 240M, used for early HDTV systems.
            /// </summary>
            Smpte240M = 7,
            /// <summary>
            /// YCgCo color space, used in some video compression algorithms.
            /// </summary>
            YCgCo = 8,
            /// <summary>
            /// ITU-R BT.2020, used for ultra-high-definition television.
            /// </summary>
            Bt2020 = 9,
            /// <summary>
            /// ITU-R BT.2020 Constant Luminance, used for ultra-high-definition television.
            /// </summary>
            Bt2020C = 10
        }

        /// <summary>
        /// Struct providing information about video file format.
        /// </summary>
        public struct FormatInfo
        {
            /// <summary>
            /// The Codec
            /// </summary>
            public CudaCodec Codec;

            /// <summary>
            /// The chroma format
            /// </summary>
            public ChromaFormat ChromaFormat;

            /// <summary>
            /// Surface format of the decoded frame.
            /// </summary>
            public SurfaceFormat SurfaceFormat;

            /// <summary>
            /// Number of bit depth - 8
            /// </summary>
            public int NBitDepthMinus8;

            /// <summary>
            /// Number of chroma bit depth - 8
            /// </summary>
            public int NBitDepthChromaMinus8;

            /// <summary>
            /// Coded sequence width in pixels
            /// </summary>
            public int UlWidth;

            /// <summary>
            /// Coded sequence height in pixels
            /// </summary>
            public int UlHeight;

            /// <summary>
            /// Width of the decoded frame returned by nextFrame(frame).
            /// </summary>
            public int Width;

            /// <summary>
            /// Height of the decoded frame returned by nextFrame(frame).
            /// </summary>
            public int Height;

            /// <summary>
            /// Max coded sequence width in pixels
            /// </summary>
            public int UlMaxWidth;

            /// <summary>
            /// Max coded sequence height in pixels
            /// </summary>
            public int UlMaxHeight;

            /// <summary>
            /// ROI inside the decoded frame returned by nextFrame(frame), containing the useable video frame.
            /// </summary>
            public Rectangle DisplayArea;

            /// <summary>
            /// True if the format is valid
            /// </summary>
            [MarshalAs(CvInvoke.BoolMarshalType)]
            public bool Valid;

            /// <summary>
            /// Frames per second
            /// </summary>
            public double Fps;

            /// <summary>
            /// Maximum number of internal decode surfaces.
            /// </summary>
            public int UlNumDecodeSurfaces;

            /// <summary>
            /// De-interlace mode
            /// </summary>
            public DeinterlaceMode DeinterlaceMode;

            /// <summary>
            /// Post-processed size of the output frame.
            /// </summary>
            public Size TargetSz;

            /// <summary>
            /// Region of interest decoded from video source.
            /// </summary>
            public Rectangle SrcRoi;

            /// <summary>
            /// Region of interest in the output frame containing the decoded frame.
            /// </summary>
            public Rectangle TargetRoi;

            /// <summary>
            /// Output value indicating if the black level, luma and chroma of the source are represented using the
            /// full or limited range (AKA TV or "analogue" range) of values as defined in Annex E of the ITU-T Specification.
            /// </summary>
            [MarshalAs(CvInvoke.BoolMarshalType)]
            public bool VideoFullRangeFlag;

            /// <summary>
            /// Video Signal Description Color Primaries of the VideoReader source (section E.2.1 VUI parameters
            /// semantics of H265 spec file)
            /// </summary>
            public ColorSpaceStandard ColorSpaceStandard;

            /// <summary>
            /// Flag requesting histogram output if supported. Exception will be thrown when requested but not supported.
            /// </summary>
            [MarshalAs(CvInvoke.BoolMarshalType)]
            public bool EnableHistogram;

            /// <summary>
            /// Bit depth of histogram bins if histogram output is requested and supported.
            /// </summary>
            public int NCounterBitDepth;

            /// <summary>
            /// Max number of histogram bins if histogram output is requested and supported.
            /// </summary>
            public int NMaxHistogramBins;
        }

        /// <summary>
        /// Creates video reader.
        /// </summary>
        /// <param name="fileName">Name of the input video file.</param>
        public CudaVideoReader(String fileName)
        {
            using (CvString s = new CvString(fileName))
            {
                _ptr = CudaInvoke.cudaVideoReaderCreate(s, ref _sharedPtr);
                CvInvoke.CheckError();
            }
        }

        /// <summary>
        /// Grabs, decodes and returns the next video frame.
        /// </summary>
        /// <param name="frame">The frame</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <returns>If no frames has been grabbed (there are no more frames in video file), the methods return false . </returns>
        public bool NextFrame(GpuMat frame, Stream stream = null)
        {
            var result = CudaInvoke.cudaVideoReaderNextFrame(_ptr, frame, stream);
            CvInvoke.CheckError();
            return result;
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
                CvInvoke.CheckError();
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
