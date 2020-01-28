//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    }

    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveQRCodeDetectorCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQRCodeDetectorRelease(ref IntPtr descriptor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveQRCodeDetectorDetect(IntPtr detector, IntPtr input, IntPtr points);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQRCodeDetectorDecode(IntPtr detector, IntPtr img, IntPtr points, IntPtr decodedInfo, IntPtr straightQrcode);

        /*
        /// <summary>
        /// Detect QR code in image and return minimum area of quadrangle that describes QR code.
        /// </summary>
        /// <param name="input">Matrix of the type CV_8U containing an image where QR code are detected.</param>
        /// <param name="points">Output vector of vertices of a quadrangle of minimal area that describes QR code.</param>
        /// <param name="epsX">Epsilon neighborhood, which allows you to determine the horizontal pattern of the scheme 1:1:3:1:1 according to QR code standard.</param>
        /// <param name="epsY">Epsilon neighborhood, which allows you to determine the vertical pattern of the scheme 1:1:3:1:1 according to QR code standard.</param>
        /// <returns>True if QR code is found</returns>
        public static bool DetectQRCode(IInputArray input, VectorOfPoint points, double epsX, double epsY)
        {
            using (InputArray iaInput = input.GetInputArray())
                return cveDetectQRCode(iaInput, points, epsX, epsY);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveDetectQRCode(IntPtr input, IntPtr points, double epsX, double epsY);

        /// <summary>
        /// Decode QR code in image and return text that is encrypted in QR code.
        /// </summary>
        /// <param name="input">Matrix of the type CV_8UC1 containing an image where QR code are detected.</param>
        /// <param name="points">Input vector of vertices of a quadrangle of minimal area that describes QR code.</param>
        /// <param name="decodeInfo">String information that is encrypted in QR code.</param>
        /// <param name="straightQRCode">Matrix of the type CV_8UC1 containing an binary straight QR code.</param>
        /// <returns>True if the QR code is found.</returns>
        public static bool DecodeQRCode(IInputArray input, IInputArray points, CvString decodeInfo, IOutputArray straightQRCode = null)
        {
            using (InputArray iaInput = input.GetInputArray())
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaStraightQRCode = straightQRCode == null ? OutputArray.GetEmpty() : straightQRCode.GetOutputArray())
            {
                return cveDecodeQRCode(iaInput, iaPoints, decodeInfo, oaStraightQRCode);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveDecodeQRCode(IntPtr input, IntPtr points, IntPtr decodedInfo, IntPtr straightQrcode);
        */
    }

}