//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV
{
    /// <summary>
    /// Barcode detector
    /// </summary>
    public class BarcodeDetector : UnmanagedObject
    {
        /// <summary>
        /// Initialize the BarcodeDetector.
        /// </summary>
        /// <param name="prototxtPath">prototxt file path for the super resolution model</param>
        /// <param name="modelPath">model file path for the super resolution model</param>
        public BarcodeDetector(
            String prototxtPath,
            String modelPath)
        {
            using (CvString csPrototxtPath = new CvString(prototxtPath))
            using (CvString csModelPath = new CvString(modelPath))
                _ptr = BarcodeInvoke.cveBarcodeDetectorCreate(
                    csPrototxtPath,
                    csModelPath);
        }


        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                BarcodeInvoke.cveBarcodeDetectorRelease(ref _ptr);
            }
        }

        /// <summary>
        /// Detects Barcode in image and returns the rectangle(s) containing the code.
        /// </summary>
        /// <param name="image">grayscale or color (BGR) image containing (or not) Barcode.</param>
        /// <param name="points">
        /// Output vector of vector of vertices of the minimum-area rotated rectangle containing the codes.
        /// For N detected barcodes, the dimensions of this array should be [N][4].
        /// Order of four points in VectorOfPointF is bottomLeft, topLeft, topRight, bottomRight.
        /// </param>
        /// <returns>True of barcode found</returns>
        public bool Detect(IInputArray image, IOutputArray points)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
                return BarcodeInvoke.cveBarcodeDetectorDetect(_ptr, iaImage, oaPoints);
        }

        /// <summary>
        /// Decodes barcode in image once it's found by the detect() method.
        /// </summary>
        /// <param name="image">grayscale or color (BGR) image containing bar code.</param>
        /// <param name="points">vector of rotated rectangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="decodedInfo">UTF8-encoded output vector of string or empty vector of string if the codes cannot be decoded.</param>
        /// <param name="decodedType">vector of BarcodeType, specifies the type of these barcodes</param>
        /// <returns>True if decode is successful</returns>
        public bool Decode(IInputArray image, IInputArray points, VectorOfCvString decodedInfo, VectorOfInt decodedType)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaPoints = points.GetInputArray())
            {
                return BarcodeInvoke.cveBarcodeDetectorDecode(_ptr, iaImage, iaPoints, decodedInfo, decodedType);
            }
        }

        /// <summary>
        /// Both detects and decodes barcode
        /// </summary>
        /// <param name="image">grayscale or color (BGR) image containing barcode.</param>
        /// <param name="decodedInfo">UTF8-encoded output vector of string(s) or empty vector of string if the codes cannot be decoded.</param>
        /// <param name="decodedType">vector of BarcodeType, specifies the type of these barcodes</param>
        /// <param name="points">Optional output vector of vertices of the found  barcode rectangle. Will be empty if not found.</param>
        /// <returns>True if barcode is detected and decoded.</returns>
        public bool DetectAndDecode(
            IInputArray image,
            VectorOfCvString decodedInfo,
            VectorOfInt decodedType,
            IOutputArray points = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaPoints = points == null ? OutputArray.GetEmpty() : points.GetOutputArray())
                return BarcodeInvoke.cveBarcodeDetectorDetectAndDecode(
                    _ptr,
                    iaImage,
                    decodedInfo,
                    decodedType,
                    oaPoints);
        }

        /// <summary>
        /// Barcode type
        /// </summary>
        public enum BarcodeType
        {
            /// <summary>
            /// None
            /// </summary>
            None,
            /// <summary>
            /// EAN-8
            /// </summary>
            EAN_8,
            /// <summary>
            /// EAN-13
            /// </summary>
            EAN_13,
            /// <summary>
            /// UPC-A
            /// </summary>
            UPC_A,
            /// <summary>
            /// UPC-E
            /// </summary>
            UPC_E,
            /// <summary>
            /// UPC-EAN-EXTENSION
            /// </summary>
            UPC_EAN_Extension
        };
    }


    /// <summary>
    /// Class that contains entry points for the Barcode module.
    /// </summary>
    internal static class BarcodeInvoke
    {
        static BarcodeInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBarcodeDetectorCreate(
            IntPtr prototxtPath,
            IntPtr modelPath);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBarcodeDetectorRelease(ref IntPtr detector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveBarcodeDetectorDetect(
            IntPtr detector,
            IntPtr img,
            IntPtr points);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveBarcodeDetectorDecode(
            IntPtr detector,
            IntPtr img,
            IntPtr points,
            IntPtr decodedInfo,
            IntPtr decodedType);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveBarcodeDetectorDetectAndDecode(
            IntPtr detector,
            IntPtr img,
            IntPtr decodedInfo,
            IntPtr decodedType,
            IntPtr points);

    }

}