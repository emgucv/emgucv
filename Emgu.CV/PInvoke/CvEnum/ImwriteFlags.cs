//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
        Jpeg2000CompressionX1000 = 272,
        /// <summary>
        /// For AVIF, it can be a quality between 0 and 100 (the higher the better). Default is 95.
        /// </summary>
        AvifQuality = 512,
        /// <summary>
        /// For AVIF, it can be 8, 10 or 12. If >8, it is stored/read as CV_32F. Default is 8.
        /// </summary>
        AvifDepth = 513,
        /// <summary>
        /// For AVIF, it is between 0 (slowest) and (fastest). Default is 9.
        /// </summary>
        AvifSpeed = 514,
        /// <summary>
        /// For JPEG XL, it can be a quality from 0 to 100 (the higher is the better). Default value is 95. If set, distance parameter is re-calicurated from quality level automatically. This parameter request libjxl v0.10 or later.
        /// </summary>
        JpegxlQuality = 640,
        /// <summary>
        /// For JPEG XL, encoder effort/speed level without affecting decoding speed; it is between 1 (fastest) and 10 (slowest). Default is 7.
        /// </summary>
        JpegxlEffort = 641,
        /// <summary>
        /// For JPEG XL, distance level for lossy compression: target max butteraugli distance, lower = higher quality, 0 = lossless; range: 0 .. 25. Default is 1.
        /// </summary>
        JpegxlDistance = 642,
        /// <summary>
        /// For JPEG XL, decoding speed tier for the provided options; minimum is 0 (slowest to decode, best quality/density), and maximum is 4 (fastest to decode, at the cost of some quality/density). Default is 0.
        /// </summary>
        JpegxlDecodingSpeed = 643,
        /// <summary>
        /// For GIF, it can be a loop flag from 0 to 65535. Default is 0 - loop forever.
        /// </summary>
        GifLoop = 1024,
        /// <summary>
        /// For GIF, it is between 1 (slowest) and 100 (fastest). Default is 96.
        /// </summary>
        GifSpeed = 1025,
        /// <summary>
        /// For GIF, it can be a quality from 1 to 8. Default is 2. See cv::ImwriteGifCompressionFlags.
        /// </summary>
        GifQuality = 1026,
        /// <summary>
        /// For GIF, it can be a quality from -1(most dither) to 3(no dither). Default is 0.
        /// </summary>
        GifDither = 1027,
        /// <summary>
        /// For GIF, the alpha channel lower than this will be set to transparent. Default is 1.
        /// </summary>
        GifTransparency = 1028,
        /// <summary>
        /// For GIF, 0 means global color table is used, 1 means local color table is used. Default is 0.
        /// </summary>
        GifColortable = 1029  

    }

    /// <summary>
    /// Imwrite GIF specific values for IMWRITE_GIF_QUALITY parameter key, if larger than 3, then its related to the size of the color table.
    /// </summary>
    public enum ImwriteGIFCompressionFlags
    {
        /// <summary>
        /// Specifies a fast GIF compression mode without dithering.
        /// </summary>
        FastNoDither = 1,

        /// <summary>
        /// Specifies the use of Floyd-Steinberg dithering for GIF compression with a faster processing speed.
        /// </summary>
        FastFloydDither = 2,
        /// <summary>
        /// Specifies the GIF compression flag for a color table size of 8.
        /// </summary>
        ColortableSize8 = 3,
        /// <summary>
        /// Specifies a GIF compression flag indicating a color table size of 16.
        /// </summary>
        ColortableSize16 = 4,
        /// <summary>
        /// Specifies a GIF compression flag indicating a color table size of 32.
        /// </summary>
        ColortableSize32 = 5,
        /// <summary>
        /// Specifies a GIF compression flag indicating a color table size of 64.
        /// </summary>
        ColortableSize64 = 6,
        /// <summary>
        /// Specifies a GIF compression flag indicating a color table size of 128.
        /// </summary>
        ColortableSize128 = 7,
        /// <summary>
        /// Specifies the GIF compression flag for a color table size of 256.
        /// </summary>
        ColortableSize256 = 8
    };

}
