//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Flags for Imwrite function
    /// </summary>
    [Flags]
    public enum ImwriteFlags
    {
        /// <summary>
        /// For JPEG, it can be a quality from 0 to 100 (the higher is the better). Default value is 95.
        /// </summary>
        JpegQuality = 1,
        /// <summary>
        /// Enable JPEG features, 0 or 1, default is False.
        /// </summary>
        JpegProgressive = 2,
        /// <summary>
        /// Enable JPEG features, 0 or 1, default is False.
        /// </summary>
        JpegOptimize = 3,
        /// <summary>
        /// JPEG restart interval, 0 - 65535, default is 0 - no restart.
        /// </summary>
        JpegRstInterval = 4,
        /// <summary>
        /// Separate luma quality level, 0 - 100, default is 0 - don't use.
        /// </summary>
        JpegLumaQuality = 5,
        /// <summary>
        /// Separate chroma quality level, 0 - 100, default is 0 - don't use.
        /// </summary>
        JpegChromaQuality = 6,
        /// <summary>
        /// For PNG, it can be the compression level from 0 to 9. A higher value means a smaller size and longer compression time. Default value is 3.
        /// </summary>
        PngCompression = 16,
        /// <summary>
        /// One of cv::ImwritePNGFlags, default is IMWRITE_PNG_STRATEGY_DEFAULT.
        /// </summary>
        PngStrategy = 17,
        /// <summary>
        /// Binary level PNG, 0 or 1, default is 0.
        /// </summary>
        PngBilevel = 18,
        /// <summary>
        /// For PPM, PGM, or PBM, it can be a binary format flag, 0 or 1. Default value is 1.
        /// </summary>
        PxmBinary = 32,
        /// <summary>
        /// Override EXR storage type (FLOAT (FP32) is default)
        /// </summary>
        ExrType = 48,
        /// <summary>
        /// Override EXR compression type (ZIP_COMPRESSION = 3 is default)
        /// </summary>
        ExrCompression = 49,
        /// <summary>
        /// For WEBP, it can be a quality from 1 to 100 (the higher is the better). By default (without any parameter) and for quality above 100 the lossless compression is used.
        /// </summary>
        WebpQuality = 64,
        /// <summary>
        /// For PAM, sets the TUPLETYPE field to the corresponding string value that is defined for the format
        /// </summary>
        PamTupletype = 128,
        /// <summary>
        /// For TIFF, use to specify which DPI resolution unit to set; see libtiff documentation for valid values
        /// </summary>
        TiffResunit = 256,
        /// <summary>
        /// For TIFF, use to specify the X direction DPI
        /// </summary>
        TiffXdpi = 257,
        /// <summary>
        /// For TIFF, use to specify the Y direction DPI
        /// </summary>
        TiffYdpi = 258,
        /// <summary>
        /// For TIFF, use to specify the image compression scheme. See libtiff for integer constants corresponding to compression formats. Note, for images whose depth is CV_32F, only libtiff's SGILOG compression scheme is used. For other supported depths, the compression scheme can be specified by this flag; LZW compression is the default.
        /// </summary>
        TiffCompression = 259,
        /// <summary>
        /// For JPEG2000, use to specify the target compression rate (multiplied by 1000). The value can be from 0 to 1000. Default is 1000.
        /// </summary>
        Jpeg2000CompressionX1000 = 272 
    }
}
