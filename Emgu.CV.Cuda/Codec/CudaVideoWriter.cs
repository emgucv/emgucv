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
    /// Works only under Windows, Supports only H264 video codec and AVI files.
    /// </summary>
    public class CudaVideoWriter : SharedPtrObject
    {
        /// <summary>
        /// Surface format
        /// </summary>
        public enum SurfaceFormat
        {
            /// <summary>
            /// UYVY
            /// </summary>
            UYVY = 0,
            /// <summary>
            /// YUY2
            /// </summary>
            YUY2,
            /// <summary>
            /// YV12
            /// </summary>
            YV12,
            /// <summary>
            /// NV12
            /// </summary>
            NV12,
            /// <summary>
            /// IYUV
            /// </summary>
            IYUV,
            /// <summary>
            /// BGR
            /// </summary>
            BGR,
            /// <summary>
            /// GRAY
            /// </summary>
            GRAY = BGR
        }

        /// <summary>
        /// The constructors initialize video writer.
        /// </summary>
        /// <param name="fileName">Name of the output video file. Only AVI file format is supported.</param>
        /// <param name="frameSize">Size of the input video frames.</param>
        /// <param name="fps">Framerate of the created video stream.</param>
        /// <param name="format">Surface format of input frames. BGR or gray frames will be converted to YV12 format before encoding, frames with other formats will be used as is.</param>
        public CudaVideoWriter(String fileName, Size frameSize, double fps, SurfaceFormat format = SurfaceFormat.BGR)
        {
            using (CvString s = new CvString(fileName))
            {
                _ptr = CudaInvoke.cudaVideoWriterCreate(s, ref frameSize, fps, format, ref _sharedPtr);
            }
        }

        /// <summary>
        /// Release all the unmanaged memory assocuated with this VideoWriter
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _sharedPtr)
            {
                CudaInvoke.cudaVideoWriterRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// The method write the specified image to video file. The image must have the same size and the same surface format as has been specified when opening the video writer.
        /// </summary>
        /// <param name="frame">The written frame.</param>
        /// <param name="lastFrame">Indicates that it is end of stream. The parameter can be ignored.</param>
        public void Write(IInputArray frame, bool lastFrame = false)
        {
            using (InputArray iaFrame = frame.GetInputArray())
                CudaInvoke.cudaVideoWriterWrite(_ptr, iaFrame, lastFrame);
        }
    }

    public static partial class CudaInvoke
    {
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cudaVideoWriterCreate(
            IntPtr fileName,
            ref Size frameSize,
            double fps,
            CudaVideoWriter.SurfaceFormat format,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaVideoWriterRelease(ref IntPtr writer);

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cudaVideoWriterWrite(
            IntPtr writer,
            IntPtr frame,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool lastFrame);
    }
}
