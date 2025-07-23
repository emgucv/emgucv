//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
    /// Represents a QR Code encoder that provides functionality to encode information into QR codes.
    /// </summary>
    public partial class QRCodeEncoder : SharedPtrObject
    {
        /// <summary>
        /// Specifies the Extended Channel Interpretations (ECI) encodings supported by the QR Code encoder.
        /// </summary>
        /// <summary>
        /// Represents the Shift JIS encoding, commonly used for Japanese characters.
        /// </summary>
        /// <summary>
        /// Represents the UTF-8 encoding, a widely used encoding for Unicode characters.
        /// </summary>
        public enum ECIEncodings
        {
            /// <summary>
            /// 
            /// </summary>
            ShiftJis = 20,
            /// <summary>
            /// Represents the UTF-8 encoding, a widely used character encoding for Unicode text.
            /// </summary>
            Utf8 = 26
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeEncoder"/> class.
        /// </summary>
        public QRCodeEncoder()
        {
            _ptr = ObjdetectInvoke.cveQRCodeEncoderCreate(ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this QRCodeEncoder
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                ObjdetectInvoke.cveQRCodeEncoderRelease(ref _ptr, ref _sharedPtr);
        }

        /// <summary>
        /// Encodes the specified input string into a QR code and outputs the result.
        /// </summary>
        /// <param name="encodedInfo">
        /// The string containing the information to be encoded into the QR code.
        /// </param>
        /// <param name="qrcode">
        /// An <see cref="IOutputArray"/> object where the generated QR code will be stored.
        /// </param>
        public void Encode(String encodedInfo, IOutputArray qrcode)
        {
            using (CvString csEncodedInfo = new CvString(encodedInfo))
            using (OutputArray oaQrcode = qrcode.GetOutputArray())
            {
                ObjdetectInvoke.cveQRCodeEncoderEncode(_ptr, csEncodedInfo, oaQrcode);
            }
        }

    }

    public static partial class ObjdetectInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveQRCodeEncoderCreate(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeEncoderRelease(ref IntPtr ptr, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeEncoderEncode(IntPtr encoder, IntPtr encodedInfo, IntPtr qrcode);

    }

}