//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.IO;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    public static partial class CudaInvoke
    {
        /// <summary>
        /// Performs pure non local means denoising without any simplification, and thus it is not fast.
        /// </summary>
        /// <param name="src">Source image. Supports only CV_8UC1, CV_8UC2 and CV_8UC3.</param>
        /// <param name="dst">Destination image.</param>
        /// <param name="h">Filter sigma regulating filter strength for color.</param>
        /// <param name="searchWindow">Size of search window.</param>
        /// <param name="blockSize">Size of block used for computing weights.</param>
        /// <param name="borderMode">Border type. REFLECT101 , REPLICATE, CONSTANT, REFLECT and WRAP are supported for now.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void NonLocalMeans(
            IInputArray src,
            IOutputArray dst,
            float h,
            int searchWindow = 21,
            int blockSize = 7,
            CvEnum.BorderType borderMode = BorderType.Default,
            Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaNonLocalMeans(iaSrc, oaDst, h, searchWindow, blockSize, borderMode, stream);
        }


        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaNonLocalMeans(
          IntPtr src,
          IntPtr dst,
          float h,
          int searchWindow,
          int blockSize,
          CvEnum.BorderType borderMode,
          IntPtr stream);

        /// <summary>
        /// Perform image denoising using Non-local Means Denoising algorithm http://www.ipol.im/pub/algo/bcm_non_local_means_denoising with several computational optimizations. Noise expected to be a gaussian white noise.
        /// </summary>
        /// <param name="src">Input 8-bit 1-channel, 2-channel or 3-channel image.</param>
        /// <param name="dst">Output image with the same size and type as src.</param>
        /// <param name="h">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise</param>
        /// <param name="searchWindow">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater search_window - greater denoising time. Recommended value 21 pixels</param>
        /// <param name="blockSize">Size in pixels of the template patch that is used to compute weights. Should be odd. Recommended value 7 pixels</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void FastNlMeansDenoising(
            IInputArray src,
            IOutputArray dst,
            float h,
            int searchWindow = 21,
            int blockSize = 7,
            Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cudaFastNlMeansDenoising(iaSrc, oaDst, h, searchWindow, blockSize, stream);
            }
        }

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaFastNlMeansDenoising(
            IntPtr src,
            IntPtr dst,
            float h,
            int searchWindow,
            int blockSize,
            IntPtr stream);

        /// <summary>
        /// Modification of fastNlMeansDenoising function for colored images.
        /// </summary>
        /// <param name="src">Input 8-bit 3-channel image.</param>
        /// <param name="dst">Output image with the same size and type as src.</param>
        /// <param name="hLuminance">Parameter regulating filter strength. Big h value perfectly removes noise but also removes image details, smaller h value preserves details but also preserves some noise</param>
        /// <param name="photoRender">The same as h but for color components. For most images value equals 10 will be enough to remove colored noise and do not distort colors</param>
        /// <param name="searchWindow">Size in pixels of the window that is used to compute weighted average for given pixel. Should be odd. Affect performance linearly: greater search_window - greater denoising time. Recommended value 21 pixels</param>
        /// <param name="blockSize">Size in pixels of the template patch that is used to compute weights. Should be odd. Recommended value 7 pixels</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void FastNlMeansDenoisingColored(
            IInputArray src,
            IOutputArray dst,
            float hLuminance,
            float photoRender,
            int searchWindow = 21,
            int blockSize = 7,
            Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                cudaFastNlMeansDenoisingColored(iaSrc, oaDst, hLuminance, photoRender, searchWindow, blockSize, stream);
            }
        }

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaFastNlMeansDenoisingColored(
            IntPtr src,
            IntPtr dst,
            float hLuminance,
            float photoRender,
            int searchWindow,
            int blockSize,
            IntPtr stream);

    }
}
