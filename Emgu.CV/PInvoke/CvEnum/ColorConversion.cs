//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Color Conversion code
    /// </summary>
    public enum ColorConversion
    {
        ///<summary>
        ///Convert BGR color to BGRA color
        ///</summary>
        Bgr2Bgra = 0,
        /// <summary>
        /// Convert RGB color to RGBA color
        /// </summary>
        Rgb2Rgba = Bgr2Bgra,

        ///<summary>
        ///Convert BGRA color to BGR color
        ///</summary>
        Bgra2Bgr = 1,
        /// <summary>
        /// Convert RGBA color to RGB color
        /// </summary>
        Rgba2Rgb = Bgra2Bgr,
        /// <summary>
        /// Convert BGR color to RGBA color
        /// </summary>
        Bgr2Rgba = 2,
        /// <summary>
        /// Convert RGB color to BGRA color
        /// </summary>
        Rgb2Bgra = Bgr2Rgba,
        /// <summary>
        /// Convert RGBA color to BGR color
        /// </summary>
        Rgba2Bgr = 3,
        /// <summary>
        /// Convert BGRA color to RGB color
        /// </summary>
        Bgra2Rgb = Rgba2Bgr,
        /// <summary>
        /// Convert BGR color to RGB color
        /// </summary>
        Bgr2Rgb = 4,
        /// <summary>
        /// Convert RGB color to BGR color
        /// </summary>
        Rgb2Bgr = Bgr2Rgb,
        /// <summary>
        /// Convert BGRA color to RGBA color
        /// </summary>
        Bgra2Rgba = 5,
        /// <summary>
        /// Convert RGBA color to BGRA color
        /// </summary>
        Rgba2Bgra = Bgra2Rgba,
        ///<summary>
        ///Convert BGR color to GRAY color
        ///</summary>
        Bgr2Gray = 6,
        /// <summary>
        /// Convert RGB color to GRAY color
        /// </summary>
        Rgb2Gray = 7,
        ///<summary>
        ///Convert GRAY color to BGR color
        ///</summary>
        Gray2Bgr = 8,
        /// <summary>
        /// Convert GRAY color to RGB color
        /// </summary>
        Gray2Rgb = Gray2Bgr,
        ///<summary>
        ///Convert GRAY color to BGRA color
        ///</summary>
        Gray2Bgra = 9,
        /// <summary>
        /// Convert GRAY color to RGBA color
        /// </summary>
        Gray2Rgba = Gray2Bgra,
        ///<summary>
        ///Convert BGRA color to GRAY color
        ///</summary>
        Bgra2Gray = 10,
        /// <summary>
        /// Convert RGBA color to GRAY color
        /// </summary>
        Rgba2Gray = 11,
        ///<summary>
        ///Convert BGR color to BGR565 color
        ///</summary>
        Bgr2Bgr565 = 12,
        /// <summary>
        /// Convert RGB color to BGR565 color
        /// </summary>
        Rgb2Bgr565 = 13,
        ///<summary>
        ///Convert BGR565 color to BGR color
        ///</summary>
        Bgr5652Bgr = 14,
        /// <summary>
        /// Convert BGR565 color to RGB color
        /// </summary>
        Bgr5652Rgb = 15,
        ///<summary>
        ///Convert BGRA color to BGR565 color
        ///</summary>
        Bgra2Bgr565 = 16,
        /// <summary>
        /// Convert RGBA color to BGR565 color
        /// </summary>
        Rgba2Bgr565 = 17,
        ///<summary>
        ///Convert BGR565 color to BGRA color
        ///</summary>
        Bgr5652Bgra = 18,
        /// <summary>
        /// Convert BGR565 color to RGBA color
        /// </summary>
        Bgr5652Rgba = 19,
        ///<summary>
        ///Convert GRAY color to BGR565 color
        ///</summary>
        Gray2Bgr565 = 20,
        ///<summary>
        ///Convert BGR565 color to GRAY color
        ///</summary>
        Bgr5652Gray = 21,
        ///<summary>
        ///Convert BGR color to BGR555 color
        ///</summary>
        Bgr2Bgr555 = 22,
        /// <summary>
        /// Convert RGB color to BGR555 color
        /// </summary>
        Rgb2Bgr555 = 23,
        ///<summary>
        ///Convert BGR555 color to BGR color
        ///</summary>
        Bgr5552Bgr = 24,
        /// <summary>
        /// Convert BGR555 color to RGB color
        /// </summary>
        Bgr5552Rgb = 25,
        ///<summary>
        ///Convert BGRA color to BGR555 color
        ///</summary>
        Bgra2Bgr555 = 26,
        /// <summary>
        /// Convert RGBA color to BGR555 color
        /// </summary>
        Rgba2Bgr555 = 27,
        ///<summary>
        ///Convert BGR555 color to BGRA color
        ///</summary>
        Bgr5552Bgra = 28,
        /// <summary>
        /// Convert BGR555 color to RGBA color
        /// </summary>
        Bgr5552Rgba = 29,
        ///<summary>
        ///Convert GRAY color to BGR555 color
        ///</summary>
        Gray2Bgr555 = 30,
        ///<summary>
        ///Convert BGR555 color to GRAY color
        ///</summary>
        Bgr5552Gray = 31,
        ///<summary>
        ///Convert BGR color to XYZ color
        ///</summary>
        Bgr2Xyz = 32,
        /// <summary>
        /// Convert RGB color to XYZ color
        /// </summary>
        Rgb2Xyz = 33,
        ///<summary>
        ///Convert XYZ color to BGR color
        ///</summary>
        Xyz2Bgr = 34,
        /// <summary>
        /// Convert XYZ color to RGB color
        /// </summary>
        Xyz2Rgb = 35,
        ///<summary>
        ///Convert BGR color to YCrCb color
        ///</summary>
        Bgr2YCrCb = 36,
        /// <summary>
        /// Convert RGB color to YCrCb color
        /// </summary>
        Rgb2YCrCb = 37,
        ///<summary>
        ///Convert YCrCb color to BGR color
        ///</summary>
        YCrCb2Bgr = 38,
        /// <summary>
        /// Convert YCrCb color to RGB color
        /// </summary>
        YCrCb2Rgb = 39,
        ///<summary>
        ///Convert BGR color to HSV color
        ///</summary>
        Bgr2Hsv = 40,
        /// <summary>
        /// Convert RGB colot to HSV color
        /// </summary>
        Rgb2Hsv = 41,
        ///<summary>
        ///Convert BGR color to Lab color
        ///</summary>
        Bgr2Lab = 44,
        /// <summary>
        /// Convert RGB color to Lab color
        /// </summary>
        Rgb2Lab = 45,
        ///<summary>
        ///Convert BayerBG color to BGR color
        ///</summary>
        BayerBg2Bgr = 46,
        ///<summary>
        ///Convert BayerGB color to BGR color
        ///</summary>
        BayerGb2Bgr = 47,
        ///<summary>
        ///Convert BayerRG color to BGR color
        ///</summary>
        BayerRg2Bgr = 48,
        ///<summary>
        ///Convert BayerGR color to BGR color
        ///</summary>
        BayerGr2Bgr = 49,
        /// <summary>
        /// Convert BayerBG color to BGR color
        /// </summary>
        BayerBg2Rgb = BayerRg2Bgr,
        /// <summary>
        /// Convert BayerRG color to BGR color
        /// </summary>
        BayerGb2Rgb = BayerGr2Bgr,
        /// <summary>
        /// Convert BayerRG color to RGB color
        /// </summary>
        BayerRg2Rgb = BayerBg2Bgr,
        /// <summary>
        /// Convert BayerGR color to RGB color
        /// </summary>
        BayerGr2Rgb = BayerGb2Bgr,
        ///<summary>
        ///Convert BGR color to Luv color
        ///</summary>
        Bgr2Luv = 50,
        /// <summary>
        /// Convert RGB color to Luv color
        /// </summary>
        Rgb2Luv = 51,
        ///<summary>
        ///Convert BGR color to HLS color
        ///</summary>
        Bgr2Hls = 52,
        /// <summary>
        /// Convert RGB color to HLS color
        /// </summary>
        Rgb2Hls = 53,
        ///<summary>
        ///Convert HSV color to BGR color
        ///</summary>
        Hsv2Bgr = 54,
        /// <summary>
        /// Convert HSV color to RGB color
        /// </summary>
        Hsv2Rgb = 55,
        ///<summary>
        ///Convert Lab color to BGR color
        ///</summary>
        Lab2Bgr = 56,
        /// <summary>
        /// Convert Lab color to RGB color
        /// </summary>
        Lab2Rgb = 57,
        ///<summary>
        ///Convert Luv color to BGR color
        ///</summary>
        Luv2Bgr = 58,
        /// <summary>
        /// Convert Luv color to RGB color
        /// </summary>
        Luv2Rgb = 59,
        ///<summary>
        ///Convert HLS color to BGR color
        ///</summary>
        Hls2Bgr = 60,
        /// <summary>
        /// Convert HLS color to RGB color
        /// </summary>
        Hls2Rgb = 61,
        /// <summary>
        /// Convert BayerBG pattern to BGR color using VNG
        /// </summary>
        BayerBg2BgrVng = 62,
        /// <summary>
        /// Convert BayerGB pattern to BGR color using VNG
        /// </summary>
        BayerGb2BgrVng = 63,
        /// <summary>
        /// Convert BayerRG pattern to BGR color using VNG
        /// </summary>
        BayerRg2BgrVng = 64,
        /// <summary>
        /// Convert BayerGR pattern to BGR color using VNG
        /// </summary>
        BayerGr2BgrVng = 65,
        /// <summary>
        /// Convert BayerBG pattern to RGB color using VNG
        /// </summary>
        BayerBg2RgbVng = BayerRg2BgrVng,
        /// <summary>
        /// Convert BayerGB pattern to RGB color using VNG
        /// </summary>
        BayerGb2RgbVng = BayerGr2BgrVng,
        /// <summary>
        /// Convert BayerRG pattern to RGB color using VNG
        /// </summary>
        BayerRg2RgbVng = BayerBg2BgrVng,
        /// <summary>
        /// Convert BayerGR pattern to RGB color using VNG
        /// </summary>
        BayerGr2RgbVng = BayerGb2BgrVng,

        /// <summary>
        /// Convert BGR to HSV
        /// </summary>
        Bgr2HsvFull = 66,
        /// <summary>
        /// Convert RGB to HSV
        /// </summary>
        Rgb2HsvFull = 67,
        /// <summary>
        /// Convert BGR to HLS
        /// </summary>
        Bgr2HlsFull = 68,
        /// <summary>
        /// Convert RGB to HLS
        /// </summary>
        Rgb2HlsFull = 69,

        /// <summary>
        /// Convert HSV color to BGR color
        /// </summary>
        Hsv2BgrFull = 70,
        /// <summary>
        /// Convert HSV color to RGB color
        /// </summary>
        Hsv2RgbFull = 71,
        /// <summary>
        /// Convert HLS color to BGR color
        /// </summary>
        Hls2BgrFull = 72,
        /// <summary>
        /// Convert HLS color to RGB color
        /// </summary>
        Hls2RgbFull = 73,

        /// <summary>
        /// Convert sBGR color to Lab color
        /// </summary>
        Lbgr2Lab = 74,
        /// <summary>
        /// Convert sRGB color to Lab color
        /// </summary>
        Lrgb2Lab = 75,
        /// <summary>
        /// Convert sBGR color to Luv color
        /// </summary>
        Lbgr2Luv = 76,
        /// <summary>
        /// Convert sRGB color to Luv color
        /// </summary>
        Lrgb2Luv = 77,

        /// <summary>
        /// Convert Lab color to sBGR color
        /// </summary>
        Lab2Lbgr = 78,
        /// <summary>
        /// Convert Lab color to sRGB color
        /// </summary>
        Lab2Lrgb = 79,
        /// <summary>
        /// Convert Luv color to sBGR color
        /// </summary>
        Luv2Lbgr = 80,
        /// <summary>
        /// Convert Luv color to sRGB color
        /// </summary>
        Luv2Lrgb = 81,

        /// <summary>
        /// Convert BGR color to YUV
        /// </summary>
        Bgr2Yuv = 82,
        /// <summary>
        /// Convert RGB color to YUV
        /// </summary>
        Rgb2Yuv = 83,
        /// <summary>
        /// Convert YUV color to BGR
        /// </summary>
        Yuv2Bgr = 84,
        /// <summary>
        /// Convert YUV color to RGB
        /// </summary>
        Yuv2Rgb = 85,

        /// <summary>
        /// Convert BayerBG to GRAY
        /// </summary>
        BayerBg2Gray = 86,
        /// <summary>
        /// Convert BayerGB to GRAY
        /// </summary>
        BayerGb2Gray = 87,
        /// <summary>
        /// Convert BayerRG to GRAY
        /// </summary>
        BayerRg2Gray = 88,
        /// <summary>
        /// Convert BayerGR to GRAY
        /// </summary>
        BayerGr2Gray = 89,
        /// <summary>
        /// Convert YUV420i to RGB
        /// </summary>
        Yuv420I2Rgb = 90,
        /// <summary>
        /// Convert YUV420i to BGR
        /// </summary>
        Yuv420I2Bgr = 91,
        /// <summary>
        /// Convert YUV420sp to RGB
        /// </summary>
        Yuv420Sp2Rgb = 92,
        /// <summary>
        /// Convert YUV320sp to BGR
        /// </summary>
        Yuv420Sp2Bgr = 93,
        /// <summary>
        /// Convert YUV320i to RGBA
        /// </summary>
        Yuv420I2Rgba = 94,
        /// <summary>
        /// Convert YUV420i to BGRA
        /// </summary>
        Yuv420I2Bgra = 95,
        /// <summary>
        /// Convert YUV420sp to RGBA
        /// </summary>
        Yuv420Sp2Rgba = 96,
        /// <summary>
        /// Convert YUV420sp to BGRA
        /// </summary>
        Yuv420Sp2Bgra = 97,

        /// <summary>
        /// Convert YUV (YV12) to RGB
        /// </summary>
        Yuv2RgbYv12 = 98,
        /// <summary>
        /// Convert YUV (YV12) to BGR
        /// </summary>
        Yuv2BgrYv12 = 99,
        /// <summary>
        /// Convert YUV (iYUV) to RGB
        /// </summary>
        Yuv2RgbIyuv = 100,
        /// <summary>
        /// Convert YUV (iYUV) to BGR
        /// </summary>
        Yuv2BgrIyuv = 101,
        /// <summary>
        /// Convert YUV (i420) to RGB
        /// </summary>
        Yuv2RgbI420 = Yuv2RgbIyuv,
        /// <summary>
        /// Convert YUV (i420) to BGR
        /// </summary>
        Yuv2BgrI420 = Yuv2BgrIyuv,
        /// <summary>
        /// Convert YUV (420p) to RGB
        /// </summary>
        Yuv420P2Rgb = Yuv2RgbYv12,
        /// <summary>
        /// Convert YUV (420p) to BGR
        /// </summary>
        Yuv420P2Bgr = Yuv2BgrYv12,

        /// <summary>
        /// Convert YUV (YV12) to RGBA
        /// </summary>
        Yuv2RgbaYv12 = 102,
        /// <summary>
        /// Convert YUV (YV12) to BGRA
        /// </summary>
        Yuv2BgraYv12 = 103,
        /// <summary>
        /// Convert YUV (iYUV) to RGBA
        /// </summary>
        Yuv2RgbaIyuv = 104,
        /// <summary>
        /// Convert YUV (iYUV) to BGRA
        /// </summary>
        Yuv2BgraIyuv = 105,
        /// <summary>
        /// Convert YUV (i420) to RGBA
        /// </summary>
        Yuv2RgbaI420 = Yuv2RgbaIyuv,
        /// <summary>
        /// Convert YUV (i420) to BGRA
        /// </summary>
        Yuv2BgraI420 = Yuv2BgraIyuv,
        /// <summary>
        /// Convert YUV (420p) to RGBA
        /// </summary>
        Yuv420P2Rgba = Yuv2RgbaYv12,
        /// <summary>
        /// Convert YUV (420p) to BGRA
        /// </summary>
        Yuv420P2Bgra = Yuv2BgraYv12,

        /// <summary>
        /// Convert YUV 420 to Gray
        /// </summary>
        Yuv2Gray420 = 106,
        /// <summary>
        /// Convert YUV NV21 to Gray
        /// </summary>
        Yuv2GrayNv21 = Yuv2Gray420,
        /// <summary>
        /// Convert YUV NV12 to Gray
        /// </summary>
        Yuv2GrayNv12 = Yuv2Gray420,
        /// <summary>
        /// Convert YUV YV12 to Gray
        /// </summary>
        Yuv2GrayYv12 = Yuv2Gray420,
        /// <summary>
        /// Convert YUV (iYUV) to Gray
        /// </summary>
        Yuv2GrayIyuv = Yuv2Gray420,
        /// <summary>
        /// Convert YUV (i420) to Gray
        /// </summary>
        Yuv2GrayI420 = Yuv2Gray420,
        /// <summary>
        /// Convert YUV (420sp) to Gray
        /// </summary>
        Yuv420Sp2Gray = Yuv2Gray420,
        /// <summary>
        /// Convert YUV (420p) to Gray
        /// </summary>
        Yuv420P2Gray = Yuv2Gray420,

        //YUV 4:2:2 formats family
        /// <summary>
        /// Convert YUV (UYVY) to RGB
        /// </summary>
        Yuv2RgbUyvy = 107,
        /// <summary>
        /// Convert YUV (UYVY) to BGR
        /// </summary>
        Yuv2BgrUyvy = 108,
        //YUV2RGB_VYUY = 109,
        //YUV2BGR_VYUY = 110,
        /// <summary>
        /// Convert YUV (Y422) to RGB
        /// </summary>
        Yuv2RgbY422 = Yuv2RgbUyvy,
        /// <summary>
        /// Convert YUV (Y422) to BGR
        /// </summary>
        Yuv2BgrY422 = Yuv2BgrUyvy,
        /// <summary>
        /// Convert YUV (UYNY) to RGB
        /// </summary>
        Yuv2RgbUynv = Yuv2RgbUyvy,
        /// <summary>
        /// Convert YUV (UYNV) to BGR
        /// </summary>
        Yuv2BgrUynv = Yuv2BgrUyvy,

        /// <summary>
        /// Convert YUV (UYVY) to RGBA
        /// </summary>
        Yuv2RgbaUyvy = 111,
        /// <summary>
        /// Convert YUV (VYUY) to BGRA
        /// </summary>
        Yuv2BgraUyvy = 112,
        //YUV2RGBA_VYUY = 113,
        //YUV2BGRA_VYUY = 114,
        /// <summary>
        /// Convert YUV (Y422) to RGBA
        /// </summary>
        Yuv2RgbaY422 = Yuv2RgbaUyvy,
        /// <summary>
        /// Convert YUV (Y422) to BGRA
        /// </summary>
        Yuv2BgraY422 = Yuv2BgraUyvy,
        /// <summary>
        /// Convert YUV (UYNV) to RGBA 
        /// </summary>
        Yuv2RgbaUynv = Yuv2RgbaUyvy,
        /// <summary>
        /// Convert YUV (UYNV) to BGRA
        /// </summary>
        Yuv2BgraUynv = Yuv2BgraUyvy,

        /// <summary>
        /// Convert YUV (YUY2) to RGB
        /// </summary>
        Yuv2RgbYuy2 = 115,
        /// <summary>
        /// Convert YUV (YUY2) to BGR
        /// </summary>
        Yuv2BgrYuy2 = 116,
        /// <summary>
        /// Convert YUV (YVYU) to RGB
        /// </summary>
        Yuv2RgbYvyu = 117,
        /// <summary>
        /// Convert YUV (YVYU) to BGR
        /// </summary>
        Yuv2BgrYvyu = 118,
        /// <summary>
        /// Convert YUV (YUYV) to RGB
        /// </summary>
        Yuv2RgbYuyv = Yuv2RgbYuy2,
        /// <summary>
        /// Convert YUV (YUYV) to BGR 
        /// </summary>
        Yuv2BgrYuyv = Yuv2BgrYuy2,
        /// <summary>
        /// Convert YUV (YUNV) to RGB
        /// </summary>
        Yuv2RgbYunv = Yuv2RgbYuy2,
        /// <summary>
        /// Convert YUV (YUNV) to BGR
        /// </summary>
        Yuv2BgrYunv = Yuv2BgrYuy2,

        /// <summary>
        /// Convert YUV (YUY2) to RGBA
        /// </summary>
        Yuv2RgbaYuy2 = 119,
        /// <summary>
        /// Convert YUV (YUY2) to BGRA
        /// </summary>
        Yuv2BgraYuy2 = 120,
        /// <summary>
        /// Convert YUV (YVYU) to RGBA
        /// </summary>
        Yuv2RgbaYvyu = 121,
        /// <summary>
        /// Convert YUV (YVYU) to BGRA
        /// </summary>
        Yuv2BgraYvyu = 122,
        /// <summary>
        /// Convert YUV (YUYV) to RGBA
        /// </summary>
        Yuv2RgbaYuyv = Yuv2RgbaYuy2,
        /// <summary>
        /// Convert YUV (YUYV) to BGRA
        /// </summary>
        Yuv2BgraYuyv = Yuv2BgraYuy2,
        /// <summary>
        /// Convert YUV (YUNV) to RGBA
        /// </summary>
        Yuv2RgbaYunv = Yuv2RgbaYuy2,
        /// <summary>
        /// Convert YUV (YUNV) to BGRA
        /// </summary>
        Yuv2BgraYunv = Yuv2BgraYuy2,

        /// <summary>
        /// Convert YUV (UYVY) to Gray
        /// </summary>
        Yuv2GrayUyvy = 123,
        /// <summary>
        /// Convert YUV (YUY2) to Gray
        /// </summary>
        Yuv2GrayYuy2 = 124,
        //YUV2GRAY_VYUY = YUV2GRAY_UYVY,
        /// <summary>
        /// Convert YUV (Y422) to Gray
        /// </summary>
        Yuv2GrayY422 = Yuv2GrayUyvy,
        /// <summary>
        /// Convert YUV (UYNV) to Gray
        /// </summary>
        Yuv2GrayUynv = Yuv2GrayUyvy,
        /// <summary>
        /// Convert YUV (YVYU) to Gray
        /// </summary>
        Yuv2GrayYvyu = Yuv2GrayYuy2,
        /// <summary>
        /// Convert YUV (YUYV) to Gray
        /// </summary>
        Yuv2GrayYuyv = Yuv2GrayYuy2,
        /// <summary>
        /// Convert YUV (YUNV) to Gray
        /// </summary>
        Yuv2GrayYunv = Yuv2GrayYuy2,

        /// <summary>
        /// Alpha premultiplication
        /// </summary>
        Rgba2MRgba = 125,
        /// <summary>
        /// Alpha premultiplication
        /// </summary>
        MRgba2Rgba = 126,

        // RGB to YUV 4:2:0 family

        /// <summary>
        /// Convert RGB to YUV_I420
        /// </summary>
        Rgb2YuvI420 = 127,
        /// <summary>
        /// Convert BGR to YUV_I420
        /// </summary>
        Bgr2YuvI420 = 128,
        /// <summary>
        /// Convert RGB to YUV_IYUV
        /// </summary>
        Rgb2YuvIyuv = Rgb2YuvI420,
        /// <summary>
        /// Convert BGR to YUV_IYUV
        /// </summary>
        Bgr2YuvIyuv = Bgr2YuvI420,

        /// <summary>
        /// Convert RGBA to YUV_I420
        /// </summary>
        Rgba2YuvI420 = 129,
        /// <summary>
        /// Convert BGRA to YUV_I420
        /// </summary>
        Bgra2YuvI420 = 130,
        /// <summary>
        /// Convert RGBA to YUV_IYUV
        /// </summary>
        Rgba2YuvIyuv = Rgba2YuvI420,
        /// <summary>
        /// Convert BGRA to YUV_IYUV
        /// </summary>
        Bgra2YuvIyuv = Bgra2YuvI420,
        /// <summary>
        /// Convert RGB to YUV_YV12
        /// </summary>
        Rgb2YuvYv12 = 131,
        /// <summary>
        /// Convert BGR to YUV_YV12
        /// </summary>
        Bgr2YuvYv12 = 132,
        /// <summary>
        /// Convert RGBA to YUV_YV12
        /// </summary>
        Rgba2YuvYv12 = 133,
        /// <summary>
        /// Convert BGRA to YUV_YV12
        /// </summary>
        Bgra2YuvYv12 = 134,

        /// <summary>
        /// Convert BayerBG to BGR (Edge-Aware Demosaicing)
        /// </summary>
        BayerBg2BgrEa = 135,
        /// <summary>
        /// Convert BayerGB to BGR (Edge-Aware Demosaicing)
        /// </summary>
        BayerGb2BgrEa = 136,
        /// <summary>
        /// Convert BayerRG to BGR (Edge-Aware Demosaicing)
        /// </summary>
        BayerRg2BgrEa = 137,
        /// <summary>
        /// Convert BayerGR to BGR (Edge-Aware Demosaicing)
        /// </summary>
        BayerGr2BgrEa = 138,

        /// <summary>
        /// Convert BayerBG to RGB (Edge-Aware Demosaicing)
        /// </summary>
        BayerBg2RgbEa = BayerRg2BgrEa,
        /// <summary>
        /// Convert BayerGB to RGB (Edge-Aware Demosaicing)
        /// </summary>
        BayerGb2RgbEa = BayerGr2BgrEa,
        /// <summary>
        /// Convert BayerRG to RGB (Edge-Aware Demosaicing)
        /// </summary>
        BayerRg2RgbEa = BayerBg2BgrEa,
        /// <summary>
        /// Convert BayerGR to RGB (Edge-Aware Demosaicing)
        /// </summary>
        BayerGr2RgbEa = BayerGb2BgrEa,

        /// <summary>
        /// The max number, do not use
        /// </summary>
        ColorcvtMax = 139
    }

}
