//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
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
    /// Works only under Windows, Supports only H264 video codec and AVI files.
    /// </summary>
    public class CudaVideoWriter : SharedPtrObject
    {

        /// <summary>
        /// Color format
        /// </summary>
        public enum ColorFormat
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Undefined = 0,
            /// <summary>
            /// OpenCV color format, can be used with both VideoReader and VideoWriter.
            /// </summary>
            Bgra = 1,
            /// <summary>
            /// OpenCV color format, can be used with both VideoReader and VideoWriter.
            /// </summary>
            Bgr = 2,
            /// <summary>
            /// OpenCV color format, can be used with both VideoReader and VideoWriter.
            /// </summary>
            Gray = 3,
            /// <summary>
            /// Nvidia color format - equivalent to YUV - Semi-Planar YUV [Y plane followed by interleaved UV plane], can be used with both VideoReader and VideoWriter.
            /// </summary>
            NvNv12 = 4,
            /// <summary>
            /// OpenCV color format, can only be used with VideoWriter.
            /// </summary>
            Rgb = 5,
            /// <summary>
            /// OpenCV color format, can only be used with VideoWriter.
            /// </summary>
            Rgba = 6,
            /// <summary>
            /// Nvidia Buffer Format - Planar YUV [Y plane followed by V and U planes], use with VideoReader, can only be used with VideoWriter.
            /// </summary>
            NvYv12 = 8,
            /// <summary>
            /// Nvidia Buffer Format - Planar YUV [Y plane followed by U and V planes], use with VideoReader, can only be used with VideoWriter.
            /// </summary>
            NvIyuv = 9,
            /// <summary>
            /// Nvidia Buffer Format - Planar YUV [Y plane followed by U and V planes], use with VideoReader, can only be used with VideoWriter.
            /// </summary>
            NvYuv444 = 10,
            /// <summary>
            /// Nvidia Buffer Format - 8 bit Packed A8Y8U8V8. This is a word-ordered format where a pixel is represented by a 32-bit word with V in the lowest 8 bits, U in the next 8 bits, Y in the 8 bits after that and A in the highest 8 bits, can only be used with VideoWriter.
            /// </summary>
            NvAyuv = 11, 

        }

        /// <summary>
        /// The constructors initialize video writer.
        /// </summary>
        /// <param name="fileName">Name of the output video file. Only AVI file format is supported.</param>
        /// <param name="frameSize">Size of the input video frames.</param>
        /// <param name="codec">Video codec</param>
        /// <param name="fps">Framerate of the created video stream.</param>
        /// <param name="format">Surface format of input frames. BGR or gray frames will be converted to YV12 format before encoding, frames with other formats will be used as is.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public CudaVideoWriter(String fileName, Size frameSize, CudaCodec codec, double fps, ColorFormat format = ColorFormat.Bgr, Stream stream = null)
        {
            using (CvString s = new CvString(fileName))
            {
                _ptr = CudaInvoke.cudaVideoWriterCreate(s, ref frameSize, codec, fps, format, stream, ref _sharedPtr);
            }
        }

        /// <summary>
        /// Release all the unmanaged memory assocuated with this VideoWriter
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cudaVideoWriterDelete(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// The method write the specified image to video file. The image must have the same size and the same surface format as has been specified when opening the video writer.
        /// </summary>
        /// <param name="frame">The written frame.</param>
        public void Write(IInputArray frame)
        {
            using (InputArray iaFrame = frame.GetInputArray())
                CudaInvoke.cudaVideoWriterWrite(_ptr, iaFrame);
        }

        /// <summary>
        /// Waits until the encoding process has finished
        /// </summary>
        public void Release()
        {
            CudaInvoke.cudaVideoWriterRelease(_ptr);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaVideoWriterCreate(
            IntPtr fileName,
            ref Size frameSize,
            CudaCodec codec,
            double fps,
            CudaVideoWriter.ColorFormat format,
            IntPtr stream,
            ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaVideoWriterRelease(IntPtr writer);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaVideoWriterDelete(ref IntPtr writer);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaVideoWriterWrite(
            IntPtr writer,
            IntPtr frame);
    }
}
