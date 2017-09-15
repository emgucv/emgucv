//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if !(__IOS__ || UNITY_IPHONE || NETFX_CORE || NETSTANDARD1_4)
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// Entry points to the Open CV bioinspired module
    /// </summary>
    public static partial class DnnInvoke
    {
        static DnnInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        /// <summary>
        /// Creates 4-dimensional blob from image. Optionally resizes and crops image from center, subtract mean values, scales values by scalefactor, swap Blue and Red channels.
        /// </summary>
        /// <param name="image">Input image (with 1- or 3-channels).</param>
        /// <param name="scaleFactor">Multiplier for image values.</param>
        /// <param name="size">Spatial size for output image</param>
        /// <param name="mean">Scalar with mean values which are subtracted from channels. Values are intended to be in (mean-R, mean-G, mean-B) order if image has BGR ordering and swapRB is true.</param>
        /// <param name="swapRB">Flag which indicates that swap first and last channels in 3-channel image is necessary.</param>
        /// <returns>4-dimansional Mat with NCHW dimensions order.</returns>
        public static Mat BlobFromImage(Mat image, double scaleFactor = 1.0, Size size = new Size(), MCvScalar mean = new MCvScalar(), bool swapRB = true)
        {
            Mat blob = new Mat();
            cveDnnBlobFromImage(image, scaleFactor, ref size, ref mean, swapRB, blob);
            return blob;
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnBlobFromImage(
            IntPtr image,
            double scalefactor,
            ref Size size,
            ref MCvScalar mean,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool swapRB,
            IntPtr blob);

        /// <summary>
        /// Creates 4-dimensional blob from series of images. Optionally resizes and crops images from center, subtract mean values, scales values by scalefactor, swap Blue and Red channels.
        /// </summary>
        /// <param name="images">Input images (all with 1- or 3-channels).</param>
        /// <param name="scaleFactor">Multiplier for images values.</param>
        /// <param name="size">Spatial size for output image</param>
        /// <param name="mean">Scalar with mean values which are subtracted from channels. Values are intended to be in (mean-R, mean-G, mean-B) order if image has BGR ordering and swapRB is true.</param>
        /// <param name="swapRB">	flag which indicates that swap first and last channels in 3-channel image is necessary.</param>
        /// <returns>Input image is resized so one side after resize is equal to corresponding dimension in size and another one is equal or larger. Then, crop from the center is performed.</returns>
        public static Mat BlobFromImages(Mat[] images, double scaleFactor = 1.0, Size size = new Size(), MCvScalar mean = new MCvScalar(), bool swapRB = true)
        {
            Mat blob = new Mat();
            using (VectorOfMat vm = new VectorOfMat(images))
            {
                cveDnnBlobFromImages(vm, scaleFactor, ref size, ref mean, swapRB, blob);
            }
            return blob;
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDnnBlobFromImages(
            IntPtr images,
            double scalefactor,
            ref Size size,
            ref MCvScalar mean,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool swapRB,
            IntPtr blob);
    }
}

#endif