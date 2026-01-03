//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Geodetic;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Tiff
{
    internal static partial class TIFFInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr tiffWriterOpen(
           [MarshalAs(CvInvoke.StringMarshalType)]
         string fileSpec);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void tiffWriterClose(ref IntPtr pTiff);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void tiffWriteGeoTag(IntPtr pTiff, IntPtr modelTiepoint, IntPtr ModelPixelScale);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void tiffWriteImage(IntPtr pTiff, IntPtr image);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void tiffWriteImageInfo(IntPtr pTiff, int bitsPerSample, int samplesPerPixel);
    }

    /// <summary>
    /// A class that can be used for writing geotiff
    /// </summary>
    public class TiffWriter : UnmanagedObject
    {
        static TiffWriter()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Indicates whether the image information has been written.
        /// </summary>
        /// <remarks>This field is intended for use by derived classes to track whether image metadata or related
        /// information has already been output. It should be set to <see langword="true"/> after the image information is
        /// written to prevent duplicate operations.</remarks>
        protected bool _imageInfoWritten = false;

        /// <summary>
        /// Create a tiff writer to save an image
        /// </summary>
        /// <param name="fileName">The file name to be saved</param>
        public TiffWriter(String fileName)
        {
            _ptr = TIFFInvoke.tiffWriterOpen(fileName);

        }

        /// <summary>
        /// Write the image to the tiff file
        /// </summary>
        /// <param name="image">The image to be written. If it has 3 channels, it needs to be RGB. If it has 4 channels, it needs to be RGBA</param>
        public virtual void WriteImage(Mat image)
        {
            if (!_imageInfoWritten)
            {
                TIFFInvoke.tiffWriteImageInfo(_ptr, image.ElementSize * 8, image.NumberOfChannels);
                _imageInfoWritten = true;
            }

            if (image.NumberOfChannels == 3 && image.Depth == DepthType.Cv8U)
            {
                TIFFInvoke.tiffWriteImage(_ptr, image);
            }
            else if (image.NumberOfChannels == 4 && image.Depth == DepthType.Cv8U)
            {
                TIFFInvoke.tiffWriteImage(_ptr, image);
            }
            else
            {
                throw new NotImplementedException(String.Format(
                    "The specific image type ({0} channels, {1} depth) is not supported",
                    image.NumberOfChannels,
                    image.Depth));
            }
        }

        /// <summary>
        /// Write the geo information into the tiff file
        /// </summary>
        /// <param name="modelTiepoint">Model Tie Point, an array of size 6</param>
        /// <param name="modelPixelScale">Model pixel scale, an array of size 3</param>
        public void WriteGeoTag(double[] modelTiepoint, double[] modelPixelScale)
        {
            Debug.Assert(modelTiepoint.Length == 6, "Model Tiepoint should have length of 6");
            Debug.Assert(modelPixelScale.Length == 3, "Model Pixel Scale should have length of 3");

            GCHandle tiepointHandle = GCHandle.Alloc(modelTiepoint, GCHandleType.Pinned);
            GCHandle pixelScaleHandle = GCHandle.Alloc(modelPixelScale, GCHandleType.Pinned);

            TIFFInvoke.tiffWriteGeoTag(_ptr, tiepointHandle.AddrOfPinnedObject(), pixelScaleHandle.AddrOfPinnedObject());

            tiepointHandle.Free();
            pixelScaleHandle.Free();
        }

        /// <summary>
        /// Release the writer and write all data on to disk.
        /// </summary>
        protected override void DisposeObject()
        {
            TIFFInvoke.tiffWriterClose(ref _ptr);
        }

    }
}
