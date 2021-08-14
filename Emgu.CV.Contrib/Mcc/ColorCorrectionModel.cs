//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
            /// The CCM with the shape 3×3 performs linear transformation on color values.
            /// </summary>
            Ccm3x3,
            /// <summary>
            /// The CCM with the shape 4×3 performs affine transformation.
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
            _ptr = CcmInvoke.cveColorCorrectionModelCreate(src, color);
            _needDispose = true;
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
		internal static extern IntPtr cveColorCorrectionModelCreate(IntPtr src, ColorCorrectionModel.ColorChecker constcolor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
		internal static extern IntPtr cveColorCorrectionModelRelease(ref IntPtr ccm);
    }
}
