//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
        /// Specifies the encoding mode for the QR Code encoder.
        /// </summary>
        public enum EncodeMode
        {
            /// <summary>
            /// Automatically selects the most suitable encoding mode based on the input data.
            /// </summary>
            Auto = -1,
            /// <summary>
            /// Represents the numeric encoding mode for the QR Code encoder.
            /// This mode is used to encode numeric data (digits 0-9) efficiently,
            /// allowing up to 3 digits to be encoded per byte.
            /// </summary>
            Numeric = 1, // 0b0001
            /// <summary>
            /// Represents the alphanumeric encoding mode for the QR Code encoder.
            /// </summary>
            /// <remarks>
            /// This mode allows encoding of alphanumeric characters, including digits (0-9), 
            /// uppercase letters (A-Z), and a limited set of special characters 
            /// (space, $, %, *, +, -, ., /, :).
            /// </remarks>
            Alphanumeric = 2, // 0b0010
            /// <summary>
            /// Represents the Byte encoding mode for the QR Code encoder.
            /// This mode allows encoding data in 8-bit byte format, supporting a wide range of characters.
            /// </summary>
            Byte = 4, // 0b0100
            /// <summary>
            /// Specifies the Extended Channel Interpretation (ECI) encoding mode for the QR Code encoder.
            /// This mode allows the inclusion of character sets or encodings beyond the default QR Code character set.
            /// </summary>
            Eci = 7, // 0b0111
            /// <summary>
            /// Specifies the Kanji encoding mode for the QR Code encoder.
            /// </summary>
            /// <remarks>
            /// This mode is used to encode characters in the Shift JIS character set, 
            /// which is commonly used for Japanese text. It allows efficient encoding 
            /// of Kanji characters into QR codes.
            /// </remarks>
            Kanji = 8, // 0b1000
            /// <summary>
            /// Represents the Structured Append encoding mode for the QR Code encoder.
            /// </summary>
            /// <remarks>
            /// The Structured Append mode allows multiple QR codes to be combined into a single logical message. 
            /// This is useful when the data to be encoded exceeds the capacity of a single QR code.
            /// </remarks>
            StructuredAppend = 3  // 0b0011
        }

        /// <summary>
        /// Specifies the error correction levels for QR codes.
        /// </summary>
        public enum CorrectionLevel
        {
            /// <summary>
            /// Low error correction level. Recovers approximately 7% of data.
            /// </summary>
            L = 0,
            /// <summary>
            /// Medium error correction level. Recovers approximately 15% of data.
            /// </summary>
            M = 1,
            /// <summary>
            /// Quartile error correction level. Recovers approximately 25% of data.
            /// </summary>
            Q = 2,
            /// <summary>
            /// High error correction level. Recovers approximately 30% of data.
            /// </summary>
            H = 3
        };

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
            /// Represents the Shift JIS encoding, which is a character encoding for the Japanese language.
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
        /// <param name="version">
        /// The version of the QR code to generate. A value of 0 indicates automatic version selection.
        /// </param>
        /// <param name="correctionLevel">
        /// The error correction level for the QR code. This determines the level of error correction
        /// applied to the QR code. Possible values are <see cref="CorrectionLevel.L"/>, 
        /// <see cref="CorrectionLevel.M"/>, <see cref="CorrectionLevel.Q"/>, and <see cref="CorrectionLevel.H"/>.
        /// </param>
        /// <param name="mode">
        /// The encoding mode for the QR code. This specifies the type of data to encode, such as 
        /// numeric, alphanumeric, byte, or kanji. Possible values are defined in <see cref="EncodeMode"/>.
        /// </param>
        /// <param name="structureNumber">
        /// The structure number for structured append mode. This is used when the QR code is part of 
        /// a structured append sequence. Default is 1.
        /// </param>
        public QRCodeEncoder(
            int version = 0,
            QRCodeEncoder.CorrectionLevel correctionLevel = CorrectionLevel.L,
            QRCodeEncoder.EncodeMode mode = EncodeMode.Auto,
            int structureNumber = 1)
        {
            _ptr = ObjdetectInvoke.cveQRCodeEncoderCreate(
                ref _sharedPtr,
                version, 
                correctionLevel,
                mode,
                structureNumber);
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
        internal static extern IntPtr cveQRCodeEncoderCreate(
            ref IntPtr sharedPtr,
            int version,
            QRCodeEncoder.CorrectionLevel correctionLevel,
            QRCodeEncoder.EncodeMode mode,
            int structureNumber);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeEncoderRelease(ref IntPtr ptr, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveQRCodeEncoderEncode(IntPtr encoder, IntPtr encodedInfo, IntPtr qrcode);

    }

}