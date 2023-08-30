//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
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
    public partial class QRCodeDetector : UnmanagedObject, IGraphicalCodeDetector
    {
        private IntPtr _graphicalCodeDetectorPtr;
        public IntPtr GraphicalCodeDetectorPtr
        {
            get { return _graphicalCodeDetectorPtr; }
        }

        /// <summary>
        /// Create a new QR code detector
        /// </summary>
        public QRCodeDetector()
        {
            _ptr = ObjdetectInvoke.cveQRCodeDetectorCreate(ref _graphicalCodeDetectorPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this HOGDescriptor
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                ObjdetectInvoke.cveQRCodeDetectorRelease(ref _ptr);
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
                ObjdetectInvoke.cveQRCodeDetectorDecodeCurved(_ptr, iaImage, iaPoints, decodedInfo, oaStraightQrcode);
                return decodedInfo.ToString();
            }
        }

    }

    public static partial class ObjdetectInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveQRCodeDetectorCreate(ref IntPtr graphicalCodeDetector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeDetectorRelease(ref IntPtr descriptor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeDetectorDecodeCurved(IntPtr detector, IntPtr img, IntPtr points, IntPtr decodedInfo, IntPtr straightQrcode);

    }

}