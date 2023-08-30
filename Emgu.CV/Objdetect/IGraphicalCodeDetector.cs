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

    public interface IGraphicalCodeDetector
    {

        /// <summary>
        /// Pointer to the graphical code detector
        /// </summary>
        IntPtr GraphicalCodeDetectorPtr { get; }
    }

    /// <summary>
    /// The detected barcode
    /// </summary>
    public class GraphicalCode
    {
        /// <summary>
        /// The string that the barcode represents
        /// </summary>
        public String DecodedInfo { get; set; }

        /*
        /// <summary>
        /// Barcode type
        /// </summary>
        public BarcodeType Type { get; set; }
        */

        /// <summary>
        /// The barcode region
        /// </summary>
        public PointF[] Points { get; set; }
    }

    public static partial class ObjdetectInvoke
    {
        static ObjdetectInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Detects graphical code in image and returns the quadrangle containing the code.
        /// </summary>
        /// <param name="img">Grayscale or color (BGR) image containing (or not) graphical code.</param>
        /// <param name="points">Output vector of vertices of the minimum-area quadrangle containing the code.</param>
        /// <returns>True if a graphical code is found.</returns>
        public static bool Detect(this IGraphicalCodeDetector detector, IInputArray img, IOutputArray points)
        {
            using (InputArray iaInput = img.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
                return cveGraphicalCodeDetectorDetect(detector.GraphicalCodeDetectorPtr, iaInput, oaPoints);
        }

        /// <summary>
        /// Detects graphical codes in image and returns the vector of the quadrangles containing the codes.
        /// </summary>
        /// <param name="img">Grayscale or color (BGR) image containing (or not) graphical codes.</param>
        /// <param name="points">Output vector of vector of vertices of the minimum-area quadrangle containing the codes.</param>
        /// <returns>True if a QRCode is found.</returns>
        public static bool DetectMulti(this IGraphicalCodeDetector detector, IInputArray img, IOutputArray points)
        {
            using (InputArray iaInput = img.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
                return cveGraphicalCodeDetectorDetectMulti(detector.GraphicalCodeDetectorPtr, iaInput, oaPoints);
        }

        /// <summary>
        /// Decodes graphical code in image once it's found by the detect() method.
        /// </summary>
        /// <param name="image">Grayscale or color (BGR) image containing graphical codes.</param>
        /// <param name="points">Quadrangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="straightCode">The optional output image containing binarized code, will be empty if not found.</param>
        /// <returns>UTF8-encoded output string or empty string if the code cannot be decoded.</returns>
        public static String Decode(this IGraphicalCodeDetector detector, IInputArray image, IInputArray points, IOutputArray straightCode = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaStraightCode = straightCode == null ? OutputArray.GetEmpty() : straightCode.GetOutputArray())
            using (CvString decodedInfo = new CvString())
            {
                cveGraphicalCodeDetectorDecode(detector.GraphicalCodeDetectorPtr, iaImage, iaPoints, oaStraightCode, decodedInfo);
                return decodedInfo.ToString();
            }
        }

        /// <summary>
        /// Decodes graphical codes in image once it's found by the detect() method.
        /// </summary>
        /// <param name="detector">The graphical code detector</param>
        /// <param name="img">Grayscale or color (BGR) image containing graphical codes.</param>
        /// <param name="points">Vector of Quadrangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="straightCode">The optional output vector of images containing binarized codes</param>
        /// <returns>UTF8-encoded output vector of string or empty vector of string if the codes cannot be decoded.</returns>
        public static String[] DecodeMulti(
            this IGraphicalCodeDetector detector,
            IInputArray img,
            IInputArray points,
            IOutputArrayOfArrays straightCode = null)
        {
            using (VectorOfCvString decodedInfo = new VectorOfCvString())
            using (InputArray iaImg = img.GetInputArray())
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaStraightCode =
                (straightCode == null ? OutputArray.GetEmpty() : straightCode.GetOutputArray()))
            {
                bool success = cveGraphicalCodeDetectorDecodeMulti(
                    detector.GraphicalCodeDetectorPtr,
                    iaImg,
                    iaPoints,
                    decodedInfo,
                    oaStraightCode);
                if (!success)
                    return new String[0];
                return decodedInfo.ToArray();
            }
        }

        /// <summary>
        /// Both detects and decodes graphical codes
        /// </summary>
        /// <param name="detector">The graphical code detector</param>
        /// <param name="img">Grayscale or color (BGR) image containing graphical codes.</param>
        /// <param name="points">Vector of Quadrangle vertices found by detect() method (or some other algorithm).</param>
        /// <param name="straightCode">The optional vector of images containing binarized codes</param>
        /// <returns>UTF8-encoded output vector of string or empty vector of string if the codes cannot be decoded</returns>
        public static GraphicalCode[] DetectAndDecodeMulti(
            this IGraphicalCodeDetector detector,
            IInputArray img,
            IOutputArrayOfArrays straightCode = null)
        {
            using (VectorOfPointF points = new VectorOfPointF())
            using (VectorOfCvString decodedInfo = new VectorOfCvString())
            using (InputArray iaImg = img.GetInputArray())
            using (OutputArray oaPoints = 
                   (points == null? OutputArray.GetEmpty() : points.GetOutputArray()))
            using (OutputArray oaStraightCode =
                   (straightCode == null ? OutputArray.GetEmpty() : straightCode.GetOutputArray()))
            {
                bool success = cveGraphicalCodeDetectorDetectAndDecodeMulti(
                    detector.GraphicalCodeDetectorPtr,
                    iaImg,
                    decodedInfo,
                    oaPoints,
                    oaStraightCode);
                if (!success)
                    return new GraphicalCode[0];

                String[] labels = decodedInfo.ToArray();
                PointF[] pts = points.ToArray();
                GraphicalCode[] results = new GraphicalCode[labels.Length];
                for (int i = 0; i < results.Length; i++)
                {
                    GraphicalCode c = new GraphicalCode();
                    c.DecodedInfo = labels[i];
                    PointF[] currentPoints = new PointF[4];
                    Array.Copy(pts, i*4, currentPoints, 0, 4);
                    c.Points = currentPoints;
                    results[i] = c;
                }

                return results;
            }
        }


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveGraphicalCodeDetectorDetect(IntPtr detector, IntPtr img, IntPtr points);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveGraphicalCodeDetectorDetectMulti(IntPtr detector, IntPtr img, IntPtr points);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGraphicalCodeDetectorDecode(IntPtr detector, IntPtr img, IntPtr points, IntPtr straightCode, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveGraphicalCodeDetectorDecodeMulti(
            IntPtr detector,
            IntPtr img,
            IntPtr points,
            IntPtr decodedInfo,
            IntPtr straightCode);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveGraphicalCodeDetectorDetectAndDecodeMulti(
            IntPtr detector,
            IntPtr img,
            IntPtr decodedInfo,
            IntPtr points,
            IntPtr straightCode);

    }

}