//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

#if __UNIFIED__
using System;
using CoreVideo;
using Emgu.CV.CvEnum;

namespace Emgu.CV
{
    /// <summary>
    /// Provide extension method to convert a CVPixelBuffer to IOutputArray
    /// </summary>
    public static class CVPixelBufferExtension
    {
        /// <summary>
        /// Copy the pixels of a 32-bit BGRA CVPixelBuffer (e.g. an AVCaptureSession camera frame
        /// configured with CVPixelFormatType.CV32BGRA) into an IOutputArray, without going
        /// through CIImage / UIImage rendering.
        /// </summary>
        /// <param name="pixelBuffer">The source pixel buffer. Must be CVPixelFormatType.CV32BGRA.</param>
        /// <param name="mat">The destination array</param>
        /// <param name="modes">The color format for the destination array: ColorBgr for 3-channel BGR, Grayscale for 1 channel, AnyColor / Unchanged for 4-channel BGRA.</param>
        /// <exception cref="NotSupportedException">Thrown if the pixel format or the ImreadModes is not supported.</exception>
        public static void ToArray(this CVPixelBuffer pixelBuffer, IOutputArray mat, ImreadModes modes = ImreadModes.AnyColor)
        {
            if (pixelBuffer.PixelFormatType != CVPixelFormatType.CV32BGRA)
                throw new NotSupportedException(String.Format("Pixel format {0} is not supported, only CV32BGRA is.", pixelBuffer.PixelFormatType));

            pixelBuffer.Lock(CVPixelBufferLock.ReadOnly);
            try
            {
                using (Mat bgra = new Mat(
                    (int)pixelBuffer.Height,
                    (int)pixelBuffer.Width,
                    DepthType.Cv8U,
                    4,
                    pixelBuffer.BaseAddress,
                    (int)pixelBuffer.BytesPerRow))
                {
                    switch (modes)
                    {
                        case ImreadModes.ColorBgr:
                            CvInvoke.CvtColor(bgra, mat, ColorConversion.Bgra2Bgr);
                            break;
                        case ImreadModes.Grayscale:
                            CvInvoke.CvtColor(bgra, mat, ColorConversion.Bgra2Gray);
                            break;
                        case ImreadModes.AnyColor:
                        case ImreadModes.Unchanged:
                            bgra.CopyTo(mat);
                            break;
                        default:
                            throw new NotSupportedException(String.Format("ImreadModes {0} is not supported.", modes));
                    }
                }
            }
            finally
            {
                pixelBuffer.Unlock(CVPixelBufferLock.ReadOnly);
            }
        }
    }
}
#endif
