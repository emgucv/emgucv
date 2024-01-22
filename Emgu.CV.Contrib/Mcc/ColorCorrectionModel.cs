//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Ccm
{
    /// <summary>
    /// Core class of ccm model.
    /// </summary>
    public partial class ColorCorrectionModel : UnmanagedObject
    {
        /// <summary>
        /// The color checker type
        /// </summary>
        public enum ColorChecker
        {
            /// <summary>
            /// Macbeth ColorChecker
            /// </summary>
            Macbeth,
            /// <summary>
            /// DKK ColorChecker
            /// </summary>
            Vinyl,
            /// <summary>
            /// DigitalSG ColorChecker with 140 squares
            /// </summary>
            DigitalSG,
        }

        /// <summary>
        /// The color correction model type
        /// </summary>
        public enum CcmType
        {
            /// <summary>
            /// The CCM with the shape 3x3 performs linear transformation on color values.
            /// </summary>
            Ccm3x3,
            /// <summary>
            /// The CCM with the shape 4x3 performs affine transformation.
            /// </summary>
            Ccm4x3
        }

        /// <summary>
        /// Possible functions to calculate the distance between colors.
        /// </summary>
        public enum DistanceType
        {
            /// <summary>
            /// The 1976 formula is the first formula that related a measured color difference to a known set of CIELAB coordinates.
            /// </summary>
            Cie76,
            /// <summary>
            /// The 1976 definition was extended to address perceptual non-uniformities.
            /// </summary>
            Cie94GraphicArts,
            /// <summary>
            /// Textiles
            /// </summary>
            Cie94Textiles,
            /// <summary>
            /// CIE 2000
            /// </summary>
            Cie2000,
            /// <summary>
            /// In 1984, the Colour Measurement Committee of the Society of Dyers and Colourists defined a difference measure, also based on the L*C*h color model.
            /// </summary>
            Cmc1To1,
            /// <summary>
            /// In 1984, the Colour Measurement Committee of the Society of Dyers and Colourists defined a difference measure, also based on the L*C*h color model.
            /// </summary>
            Cmc2To1,
            /// <summary>
            /// Euclidean distance of rgb color space
            /// </summary>
            Rgb,
            /// <summary>
            /// Euclidean distance of rgbl color space
            /// </summary>
            Rgbl
        };

        /// <summary>
        /// Linearization transformation type.
        /// </summary>
        public enum LinearType
        {
            /// <summary>
            /// No change is made
            /// </summary>
            Identity,
            /// <summary>
            /// Gamma correction; Need assign a value to gamma simultaneously
            /// </summary>
            Gamma,
            /// <summary>
            /// Polynomial fitting channels respectively; Need assign a value to deg simultaneously
            /// </summary>
            ColorPolyFit,
            /// <summary>
            /// Logarithmic polynomial fitting channels respectively; Need assign a value to deg simultaneously
            /// </summary>
            ColorLogPolyFit,
            /// <summary>
            /// Grayscale polynomial fitting; Need assign a value to deg and dst_whites simultaneously
            /// </summary>
            GrayPolyFit,
            /// <summary>
            /// Grayscale Logarithmic polynomial fitting;  Need assign a value to deg and dst_whites simultaneously
            /// </summary>
            GrayLogPolyFit
        };

        /// <summary>
        /// The color space
        /// </summary>
        public enum ColorSpace
        {
            /// <summary>
            /// https://en.wikipedia.org/wiki/SRGB , RGB color space
            /// </summary>
            SRgb,
            /// <summary>
            /// https://en.wikipedia.org/wiki/SRGB , linear RGB color space
            /// </summary>
            SRgbL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/Adobe_RGB_color_space , RGB color space
            /// </summary>
            AdobeRGB,
            /// <summary>
            /// https://en.wikipedia.org/wiki/Adobe_RGB_color_space , linear RGB color space
            /// </summary>
            AdobeRGBL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/Wide-gamut_RGB_color_space , RGB color space
            /// </summary>
            WideGamutRGB,
            /// <summary>
            /// https://en.wikipedia.org/wiki/Wide-gamut_RGB_color_space , linear RGB color space
            /// </summary>
            WideGamutRGBL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/ProPhoto_RGB_color_space , RGB color space
            /// </summary>
            ProPhotoRGB,
            /// <summary>
            /// https://en.wikipedia.org/wiki/ProPhoto_RGB_color_space , linear RGB color space
            /// </summary>
            ProPhotoRGBL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/DCI-P3 , RGB color space
            /// </summary>
            DciP3Rgb,
            /// <summary>
            /// https://en.wikipedia.org/wiki/DCI-P3 , linear RGB color space
            /// </summary>
            DciP3RgbL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/RGB_color_space , RGB color space
            /// </summary>
            AppleRGB,
            /// <summary>
            /// https://en.wikipedia.org/wiki/RGB_color_space , linear RGB color space
            /// </summary>
            AppleRGBL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/Rec._709 , RGB color space
            /// </summary>
            Rec709Rgb,
            /// <summary>
            /// https://en.wikipedia.org/wiki/Rec._709 , linear RGB color space
            /// </summary>
            Rec709RgbL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/Rec._2020 , RGB color space
            /// </summary>
            Rec2020Rgb,
            /// <summary>
            ///  https://en.wikipedia.org/wiki/Rec._2020 , linear RGB color space
            /// </summary>
            Rec2020RgbL,
            /// <summary>
            /// https://en.wikipedia.org/wiki/CIE_1931_color_space , non-RGB color space
            /// </summary>
            XyzD65_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzD65_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzD50_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzD50_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzA2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzA10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzD55_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzD55_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzD75_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzD75_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzE_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            XyzE_10,
            /// <summary>
            /// https://en.wikipedia.org/wiki/CIELAB_color_space , non-RGB color space
            /// </summary>
            LabD65_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>                                       
            LabD65_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabD50_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabD50_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabA_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabA_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabD55_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabD55_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabD75_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabD75_10,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabE_2,
            /// <summary>
            /// non-RGB color space
            /// </summary>
            LabE_10,
        };


        private bool _needDispose;

        internal ColorCorrectionModel(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Color Correction Model.
        /// </summary>
        /// <param name="src">Detected colors of ColorChecker patches; the color type is RGB not BGR, and the color values are in [0, 1];</param>
        /// <param name="color">The Built-in color card</param>
        public ColorCorrectionModel(Mat src, ColorChecker color)
        {
            _ptr = CcmInvoke.cveColorCorrectionModelCreate1(src, color);
            _needDispose = true;
        }

        /// <summary>
        /// Color Correction Model.
        /// </summary>
        /// <param name="src">Detected colors of ColorChecker patches; the color type is RGB not BGR, and the color values are in [0, 1];</param>
        /// <param name="colors">the reference color values, the color values are in [0, 1].</param>
        /// <param name="refCs">The corresponding color space. If the color type is some RGB, the format is RGB not BGR</param>
        public ColorCorrectionModel(Mat src, Mat colors, ColorSpace refCs)
        {
            _ptr = CcmInvoke.cveColorCorrectionModelCreate2(src, colors, refCs);
            _needDispose = true;
        }

        /// <summary>
        /// Color Correction Model.
        /// </summary>
        /// <param name="src">Detected colors of ColorChecker patches; the color type is RGB not BGR, and the color values are in [0, 1];</param>
        /// <param name="colors">the reference color values, the color values are in [0, 1].</param>
        /// <param name="refCs">The corresponding color space. If the color type is some RGB, the format is RGB not BGR</param>
        /// <param name="colored">Mask of colored color</param>
        public ColorCorrectionModel(Mat src, Mat colors, ColorSpace refCs, Mat colored)
        {
            _ptr = CcmInvoke.cveColorCorrectionModelCreate3(src, colors, refCs, colored);
            _needDispose = true;
        }

        /// <summary>
        /// Make color correction
        /// </summary>
        public void Run()
        {
            CcmInvoke.cveColorCorrectionModelRun(_ptr);
        }

        /// <summary>
        /// Get the CCM
        /// </summary>
        /// <returns>The CCM matrix</returns>
        public Mat GetCCM()
        {
            Mat m = new Mat();
            using (OutputArray oaM = m.GetOutputArray())
                CcmInvoke.cveColorCorrectionModelGetCCM(_ptr, oaM);
            return m;
        }

        /// <summary>
        /// Infer using fitting ccm.
        /// </summary>
        /// <param name="img">The input image.</param>
        /// <param name="isLinear">Default false</param>
        /// <returns>The output array.</returns>
        public Mat Infer(Mat img, bool isLinear = false)
        {
            Mat m = new Mat();
            using (OutputArray oaM = m.GetOutputArray())
            {
                CcmInvoke.cveColorCorrectionModelInfer(_ptr, img, oaM, isLinear);
            }
            return m;
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero && _needDispose)
            {
                CcmInvoke.cveColorCorrectionModelRelease(ref _ptr);
            }
        }
    }


    /// <summary>
    /// Class that contains entry points for the Ccm module.
    /// </summary>
    public static partial class CcmInvoke
    {
        static CcmInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveColorCorrectionModelCreate1(IntPtr src, ColorCorrectionModel.ColorChecker constColor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveColorCorrectionModelCreate2(IntPtr src, IntPtr colors, ColorCorrectionModel.ColorSpace refCs);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveColorCorrectionModelCreate3(IntPtr src, IntPtr colors, ColorCorrectionModel.ColorSpace refCs, IntPtr colored);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveColorCorrectionModelRelease(ref IntPtr ccm);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveColorCorrectionModelRun(IntPtr ccm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveColorCorrectionModelGetCCM(IntPtr ccm, IntPtr result);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveColorCorrectionModelInfer(
            IntPtr ccm,
            IntPtr img,
            IntPtr result,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool isLinear);
    }
}
