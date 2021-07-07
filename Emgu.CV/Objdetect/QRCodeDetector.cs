//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// A QR code detector
    /// </summary>
    public partial class QRCodeDetector : UnmanagedObject
    {
        /// <summary>
        /// Create a new QR code detector
        /// </summary>
        public QRCodeDetector()
        {
            _ptr = CvInvoke.cveQRCodeDetectorCreate();
        }

        /// <summary>
        /// Release the unmanaged memory associated with this HOGDescriptor
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveQRCodeDetectorRelease(ref _ptr);
        }

        /// <summary>
        /// Detector the location of the QR code
        /// </summary>
        /// <param name="input">The input image</param>
        /// <param name="points">The location of the QR code in the image</param>
        /// <returns>True if a QRCode is found.</returns>
        public bool Detect(IInputArray input, IOutputArray points)
        {
            using (InputArray iaInput = input.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
                return CvInvoke.cveQRCodeDetectorDetect(_ptr, iaInput, oaPoints);
        }

        /// <summary>
        /// Detects QR codes in image and returns the vector of the quadrangles containing the codes.
        /// </summary>
        /// <param name="img">Grayscale or color (BGR) image containing (or not) QR codes</param>
        /// <param name="points">Output vector of vector of vertices of the minimum-area quadrangle containing the codes.</param>
        /// <returns>True if a QRCode is found.</returns>
        public bool DetectMulti(IInputArray img, IOutputArray points)
        {
            using (InputArray iaInput = img.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
                return CvInvoke.cveQRCodeDetectorDetectMulti(_ptr, iaInput, oaPoints);
        }

        /// <summary>
        /// Decodes QR code in image once it's found by the detect() method.
        /// </summary>
        /// <param name="image">grayscale or color (BGR) image containing QR code.</param>
        /// <param name="points">Quadrangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="straightQrcode">The optional output image containing rectified and binarized QR code</param>
        /// <returns>UTF8-encoded output string or empty string if the code cannot be decoded.</returns>
        public String Decode(IInputArray image, IInputArray points, IOutputArray straightQrcode = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaStraightQrcode = straightQrcode == null ? OutputArray.GetEmpty() : straightQrcode.GetOutputArray())
            using (CvString decodedInfo = new CvString())
            {
                CvInvoke.cveQRCodeDetectorDecode(_ptr, iaImage, iaPoints, decodedInfo, oaStraightQrcode);
                return decodedInfo.ToString();
            }
        }

        /// <summary>
        /// Decodes QR code on a curved surface in image once it's found by the detect() method.
        /// </summary>
        /// <param name="img">grayscale or color (BGR) image containing QR code.</param>
        /// <param name="points">Quadrangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="straightQrcode">The optional output image containing rectified and binarized QR code</param>
        /// <returns>UTF8-encoded output string or empty string if the code cannot be decoded.</returns>
        public String DecodeCurved(IInputArray img, IInputArray points, IOutputArray straightQrcode = null)
        {
            using (InputArray iaImage = img.GetInputArray())
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaStraightQrcode = straightQrcode == null ? OutputArray.GetEmpty() : straightQrcode.GetOutputArray())
            using (CvString decodedInfo = new CvString())
            {
                CvInvoke.cveQRCodeDetectorDecodeCurved(_ptr, iaImage, iaPoints, decodedInfo, oaStraightQrcode);
                return decodedInfo.ToString();
            }
        }

        /// <summary>
        /// Decodes QR codes in image once it's found by the detect() method.
        /// </summary>
        /// <param name="img">Grayscale or color (BGR) image containing QR codes.</param>
        /// <param name="points">Vector of Quadrangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="decodedInfo">UTF8-encoded output vector of string or empty vector of string if the codes cannot be decoded.</param>
        /// <param name="straightQrcode">The optional output vector of images containing rectified and binarized QR codes</param>
        /// <returns>True if decoding is successful.</returns>
        public bool DecodeMulti(
            IInputArray img, 
            IInputArray points, 
            VectorOfCvString decodedInfo,
            IOutputArray straightQrcode = null)
        {
            using (InputArray iaImg = img.GetInputArray())
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaStraightQrcode =
                (straightQrcode == null ? OutputArray.GetEmpty() : straightQrcode.GetOutputArray()))
            {
                return CvInvoke.cveQRCodeDetectorDecodeMulti(_ptr, iaImg, iaPoints, decodedInfo, oaStraightQrcode);
            }
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveQRCodeDetectorCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeDetectorRelease(ref IntPtr descriptor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveQRCodeDetectorDetect(IntPtr detector, IntPtr input, IntPtr points);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveQRCodeDetectorDetectMulti(IntPtr detector, IntPtr input, IntPtr points);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeDetectorDecode(IntPtr detector, IntPtr img, IntPtr points, IntPtr decodedInfo, IntPtr straightQrcode);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeDetectorDecodeCurved(IntPtr detector, IntPtr img, IntPtr points, IntPtr decodedInfo, IntPtr straightQrcode);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveQRCodeDetectorDecodeMulti(
            IntPtr detector,
            IntPtr img,
            IntPtr points,
            IntPtr decodedInfo,
            IntPtr straightQrcode);
        
    }

}