//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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

    }

    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveQRCodeDetectorCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQRCodeDetectorRelease(ref IntPtr descriptor);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return:MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveQRCodeDetectorDetect(IntPtr detector, IntPtr input, IntPtr points);
    }

}