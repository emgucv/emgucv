//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.IO;
using System.Security.Cryptography;

namespace Emgu.CV
{
    /// <summary>
    /// Class that provide access to native GAPI functions from OpenCV
    /// </summary>
    public static partial class GapiInvoke
    {
        static GapiInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Resizes an image.
        /// </summary>
        /// <param name="src">Input image.</param>
        /// <param name="dsize">Output image size</param>
        /// <param name="fx">Scale factor along the horizontal axis</param>
        /// <param name="fy">Scale factor along the vertical axis</param>
        /// <param name="interpolation">Interpolation method</param>
        /// <returns>The resized image</returns>
        public static GMat Resize(
            GMat src, 
            Size dsize, 
            double fx = 0, 
            double fy = 0, 
            CvEnum.Inter interpolation = Inter.Linear)
        {
            return new GMat(cveGapiResize(src, ref dsize, fx, fy, interpolation), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiResize(IntPtr src, ref Size dsize, double fx, double fy, CvEnum.Inter interpolation);

        /// <summary>
        /// Calculates per-element bit-wise inversion of the input matrix
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <returns>Per-element bit-wise inversion of the input matrix</returns>
        public static GMat BitwiseNot(GMat src)
        {
            return new GMat(cveGapiBitwiseNot(src), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBitwiseNot(IntPtr src);

        /// <summary>
        /// The function add calculates sum of two matrices of the same size and the same number of channels
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix.</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>Per-element sum of two matrices.</returns>
        public static GMat Add(GMat src1, GMat src2, CvEnum.DepthType ddepth = CvEnum.DepthType.Default)
        {
            return new GMat(cveGapiAdd(src1, src2, ddepth), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiAdd(IntPtr src1, IntPtr src2, CvEnum.DepthType ddepth);


        /// <summary>
        /// The function addC adds a given scalar value to each element of given matrix.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="c">Scalar value to be added.</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>Per-element sum of matrix and given scalar.</returns>
        public static GMat AddC(GMat src1, GScalar c, CvEnum.DepthType ddepth)
        {
            return new GMat(cveGapiAddC(src1, c, ddepth), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiAddC(IntPtr src1, IntPtr c, CvEnum.DepthType ddepth);


        /// <summary>
        /// Calculates the per-element difference between two matrices.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix.</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>The per-element difference between two matrices.</returns>
        public static GMat Sub(
            GMat src1, 
            GMat src2, 
            CvEnum.DepthType ddepth = DepthType.Default)
        {
            return new GMat(cveGapiSub(src1, src2, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSub(IntPtr src1, IntPtr src2, CvEnum.DepthType ddepth);

        /// <summary>
        /// Calculates the per-element difference between matrix and given scalar.
        /// </summary>
        /// <param name="src">First input matrix.</param>
        /// <param name="c">Scalar value to subtracted.</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>The per-element difference between matrix and given scalar.</returns>
        public static GMat SubC(
            GMat src, 
            GScalar c, 
            CvEnum.DepthType ddepth = DepthType.Default)
        {
            return new GMat(cveGapiSubC(src, c, ddepth), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSubC(IntPtr src1, IntPtr c, CvEnum.DepthType ddepth);

        /// <summary>
        /// Calculates the per-element difference between given scalar and the matrix.
        /// </summary>
        /// <param name="c">Scalar value to subtract from</param>
        /// <param name="src">Input matrix to be subtracted.</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>Per-element difference between given scalar and the matrix.</returns>
        public static GMat SubRC(
            GScalar c, 
            GMat src, 
            CvEnum.DepthType ddepth = DepthType.Default)
        {
            return new GMat(cveGapiSubRC(c, src, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSubRC(IntPtr c, IntPtr src1, CvEnum.DepthType ddepth);

        /// <summary>
        /// Calculates the per-element scaled product of two matrices.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same size and the same depth as src1.</param>
        /// <param name="scale">Optional scale factor.</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>The per-element scaled product of two matrices.</returns>
        public static GMat Mul(GMat src1, GMat src2, double scale, CvEnum.DepthType ddepth)
        {
            return new GMat(cveGapiMul(src1, src2, scale, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMul(IntPtr src1, IntPtr src2, double scale, CvEnum.DepthType ddepth);

        /// <summary>
        /// Multiplies matrix by scalar.
        /// </summary>
        /// <param name="src1">Input matrix.</param>
        /// <param name="c">Factor to be multiplied</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>The per-element scaled product of the matrix and the scale.</returns>
        public static GMat MulC(GMat src1, GScalar c, CvEnum.DepthType ddepth = DepthType.Default)
        {
            return new GMat(cveGapiMulC(src1, c, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMulC(IntPtr src, IntPtr scale, CvEnum.DepthType ddepth);


        /// <summary>
        /// Performs per-element division of two matrices.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same size and depth as src1.</param>
        /// <param name="scale">Scalar factor.</param>
        /// <param name="ddepth">Optional depth of the output matrix</param>
        /// <returns>Per-element division of two matrices.</returns>
        public static GMat Div(GMat src1, GMat src2, double scale, CvEnum.DepthType ddepth = DepthType.Default)
        {
            return new GMat(cveGapiDiv(src1, src2, scale, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiDiv(IntPtr src1, IntPtr src2, double scale, CvEnum.DepthType ddepth);

        /// <summary>
        /// Divides each element of matrix src by given scalar value
        /// </summary>
        /// <param name="src1">Input matrix.</param>
        /// <param name="divisor">Number to be divided by.</param>
        /// <param name="scale">Optional depth of the output matrix.</param>
        /// <param name="ddepth">Scale factor.</param>
        /// <returns>Result of the divide operation</returns>
        public static GMat DivC(GMat src1, GScalar divisor, double scale, CvEnum.DepthType ddepth = DepthType.Default)
        {
            return new GMat(cveGapiDivC(src1, divisor, scale, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiDivC(IntPtr src, IntPtr divisor, double scale, CvEnum.DepthType ddepth);


        /// <summary>
        /// Divides given scalar by each element of matrix src and keep the division result in new matrix of the same size and type as src
        /// </summary>
        /// <param name="divisor">Input matrix.</param>
        /// <param name="src">Number to be divided.</param>
        /// <param name="scale">Optional depth of the output matrix.</param>
        /// <param name="ddepth">Scale factor</param>
        /// <returns>Result of the divide operation</returns>
        public static GMat DivRC(GScalar divisor, GMat src, double scale, CvEnum.DepthType ddepth = DepthType.Default)
        {
            return new GMat(cveGapiDivRC(divisor, src, scale, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiDivRC(IntPtr divident, IntPtr src, double scale, CvEnum.DepthType ddepth);

        /// <summary>
        /// The function mean calculates the mean value M of matrix elements, independently for each channel, and return it.
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <returns>The mean value of matrix elements, independently for each channel, and return it. </returns>
        public static GScalar Mean(GMat src)
        {
            return new GScalar(cveGapiMean(src));
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMean(IntPtr src);

        /// <summary>
        /// Calculates x and y coordinates of 2D vectors from their magnitude and angle.
        /// </summary>
        /// <param name="magnitude">Input floating-point CV_32FC1 matrix (1xN) of magnitudes of 2D vectors</param>
        /// <param name="angle">Input floating-point CV_32FC1 matrix (1xN) of angles of 2D vectors.</param>
        /// <param name="angleInDegrees">When true, the input angles are measured in degrees, otherwise, they are measured in radians.</param>
        /// <returns>The first GMat contains the X coordinates, the second GMat contains the Y coordinates.</returns>
        public static Tuple<GMat, GMat> PolarToCart(
            GMat magnitude,
            GMat angle,
            bool angleInDegrees = false)
        {
            GMat outX = new GMat();
            GMat outY = new GMat();
            cveGapiPolarToCart(magnitude, angle, angleInDegrees, outX, outY);
            return new Tuple<GMat, GMat>(outX, outY);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGapiPolarToCart(
            IntPtr magnitude,
            IntPtr angle,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool angleInDegrees,
            IntPtr outX,
            IntPtr outY);

        /// <summary>
        /// Calculates the magnitude and angle of 2D vectors.
        /// </summary>
        /// <param name="x">Matrix of CV_32FC1 x-coordinates.</param>
        /// <param name="y">Array of CV_32FC1 y-coordinates.</param>
        /// <param name="angleInDegrees">Indicating whether the angles are measured in radians (which is by default), or in degrees.</param>
        /// <returns>First output is a matrix of magnitudes of the same size and depth as input x. Second output is a matrix of angles that has the same size and depth as x; the angles are measured in radians (from 0 to 2*Pi) or in degrees (0 to 360 degrees).</returns>
        public static Tuple<GMat, GMat> CartToPolar(
            GMat x,
            GMat y,
            bool angleInDegrees = false)
        {
            GMat outMagnitude = new GMat();
            GMat outAngle = new GMat();
            cveGapiCartToPolar(x, y, angleInDegrees, outMagnitude, outAngle);
            return new Tuple<GMat, GMat>(outMagnitude, outAngle);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGapiCartToPolar(
            IntPtr x,
            IntPtr y,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool angleInDegrees,
            IntPtr outMagnitude,
            IntPtr outAngle);

        /// <summary>
        /// Calculates the rotation angle of 2D vectors.
        /// </summary>
        /// <param name="x">Input floating-point array of x-coordinates of 2D vectors.</param>
        /// <param name="y">Input array of y-coordinates of 2D vectors; it must have the same size and the same type as x.</param>
        /// <param name="angleInDegrees">When true, the function calculates the angle in degrees, otherwise, they are measured in radians.</param>
        /// <returns>Array of vector angles; it has the same size and same type as x.</returns>
        public static GMat Phase(GMat x, GMat y, bool angleInDegrees)
        {
            return new GMat(cveGapiPhase(x, y, angleInDegrees), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiPhase(
            IntPtr x,
            IntPtr y,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool angleInDegrees);

        /// <summary>
        /// Calculates a square root of each input array element. In case of multi-channel arrays, each channel is processed independently.
        /// </summary>
        /// <param name="src">Input floating-point array.</param>
        /// <returns>Output array of the same size and type as src.</returns>
        public static GMat Sqrt(GMat src)
        {
            return new GMat(cveGapiSqrt(src), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSqrt(IntPtr src);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are greater compare to elements in second.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same depth as first input matrix.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpGT(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpGT(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpGT(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are greater compare to the scalar value.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input scalar.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpGT(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpGTS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpGTS(IntPtr src1, IntPtr src2);

        /// <summary>
        ///Performs the per-element comparison of two matrices checking if elements from first matrix are less than elements in second.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same depth as first input matrix.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpLT(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpLT(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpLT(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are less than the scalar value.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input scalar.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpLT(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpLTS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpLTS(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are greater or equal compare to elements in second.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same depth as first input matrix.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpGE(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpGE(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpGE(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are greater or equal compare to the scalar value.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input scalar.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpGE(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpGES(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpGES(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are less or equal compare to elements in second.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same depth as first input matrix.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpLE(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpLE(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpLE(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are less or equal compare to the scalar value.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input scalar.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpLE(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpLES(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpLES(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are equal to elements in second.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same depth as first input matrix.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpEQ(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpEQ(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpEQ(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of a matrix and a scalar, checking if elements from first matrix are equal to the scalar value.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input scalar.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0 </returns>
        public static GMat CmpEQ(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpEQS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpEQS(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of two matrices checking if elements from first matrix are not equal to elements in second.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same depth as first input matrix.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpNE(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpNE(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpNE(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Performs the per-element comparison of of a matrix and a scalar, checking if elements from first matrix are not equal to elements in second.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input scalar.</param>
        /// <returns>When the comparison result is true, the corresponding element of output array is set to 255. Otherwise it is set to 0.</returns>
        public static GMat CmpNE(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpNES(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCmpNES(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Computes bitwise conjunction of the two matrices (src1 &amp; src2) Calculates the per-element bit-wise logical conjunction of two matrices of the same size.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix.</param>
        /// <returns>Bitwise conjunction of the two matrices (src1 &amp; src2)</returns>
        public static GMat BitwiseAnd(GMat src1, GMat src2)
        {
            return new GMat(cveGapiBitwiseAnd(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBitwiseAnd(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Computes bitwise conjunction of a matrix and a scalar. Calculates the per-element bit-wise logical conjunction of a matrix and a scalar.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Scalar value</param>
        /// <returns>Bitwise conjunction of a matrix and a scalar</returns>
        public static GMat BitwiseAnd(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiBitwiseAndS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBitwiseAndS(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Calculates the per-element bit-wise logical disjunction of two matrices of the same size.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix.</param>
        /// <returns>Per-element bit-wise logical disjunction of two matrices of the same size.</returns>
        public static GMat BitwiseOr(GMat src1, GMat src2)
        {
            return new GMat(cveGapiBitwiseOr(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBitwiseOr(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Computes bitwise logical disjunction of a matrix and a scalar. Calculates the per-element bit-wise logical disjunction of a matrix and a scalar.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Scalar value</param>
        /// <returns>Bitwise logical disjunction of a matrix and a scalar</returns>
        public static GMat BitwiseOr(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiBitwiseOrS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBitwiseOrS(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Calculates the per-element bit-wise logical "exclusive or" of two matrices of the same size.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix.</param>
        /// <returns>The per-element bit-wise logical "exclusive or" of two matrices of the same size.</returns>
        public static GMat BitwiseXor(GMat src1, GMat src2)
        {
            return new GMat(cveGapiBitwiseXor(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBitwiseXor(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Calculates the per-element bit-wise logical "exclusive or" of a matrix and a scalar.
        /// </summary>
        /// <param name="src1">First input matrix</param>
        /// <param name="src2">Scalar, for which per-lemenet "logical or" operation on elements of src1 will be performed.</param>
        /// <returns>The per-element bit-wise logical "exclusive or" of a matrix and a scalar.</returns>
        public static GMat BitwiseXor(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiBitwiseXorS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBitwiseXorS(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Set value from given matrix if the corresponding pixel value in mask matrix set to true, and set the matrix value to 0 otherwise.
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <param name="mask">Input mask matrix.</param>
        /// <returns>Result of the mask operation</returns>
        public static GMat Mask(GMat src, GMat mask)
        {
            return new GMat(cveGapiMask(src, mask), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMask(IntPtr src, IntPtr mask);

        /// <summary>
        /// Applies a separable linear filter to a matrix(image).
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <param name="ddepth">Desired depth of the destination image</param>
        /// <param name="kernelX">Coefficients for filtering each row.</param>
        /// <param name="kernelY">Coefficients for filtering each column.</param>
        /// <param name="anchor">Anchor position within the kernel. The default value (−1,−1) means that the anchor is at the kernel center.</param>
        /// <param name="delta">Value added to the filtered results before storing them.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of constant border type</param>
        /// <returns>Result of applying the filter</returns>
        public static GMat SepFilter(
            GMat src,
            CvEnum.DepthType ddepth,
            Mat kernelX,
            Mat kernelY,
            Point anchor,
            MCvScalar delta,
            CvEnum.BorderType borderType = BorderType.Default,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiSepFilter(
                    src,
                    ddepth,
                    kernelX,
                    kernelY,
                    ref anchor,
                    ref delta,
                    borderType,
                    ref borderValue),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSepFilter(
            IntPtr src,
            CvEnum.DepthType ddepth,
            IntPtr kernelX,
            IntPtr kernelY,
            ref Point anchor,
            ref MCvScalar delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Convolve an image with the kernel.
        /// </summary>
        /// <param name="src">Input image.</param>
        /// <param name="ddepth">Desired depth of the destination image</param>
        /// <param name="kernel">Convolution kernel (or rather a correlation kernel), a single-channel floating point matrix; if you want to apply different kernels to different channels, split the image into separate color planes using split and process them individually.</param>
        /// <param name="anchor">Anchor of the kernel that indicates the relative position of a filtered point within the kernel; the anchor should lie within the kernel; default value (-1,-1) means that the anchor is at the kernel center.</param>
        /// <param name="delta">Optional value added to the filtered pixels before storing them in dst.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of constant border type</param>
        /// <returns>Resulting image from the convolution filter.</returns>
        public static GMat Filter2D(
            GMat src,
            CvEnum.DepthType ddepth,
            Mat kernel,
            Point anchor,
            MCvScalar delta = new MCvScalar(),
            CvEnum.BorderType borderType = BorderType.Default,
            MCvScalar borderValue = new MCvScalar()
        )
        {
            return new GMat(
                cveGapiFilter2D(
                    src,
                    ddepth,
                    kernel,
                    ref anchor,
                    ref delta,
                    borderType,
                    ref borderValue),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiFilter2D(
            IntPtr src,
            CvEnum.DepthType ddepth,
            IntPtr kernel,
            ref Point anchor,
            ref MCvScalar delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Blurs an image using the box filter.
        /// </summary>
        /// <param name="src">Source image</param>
        /// <param name="dtype">The output image depth</param>
        /// <param name="ksize">Blurring kernel size.</param>
        /// <param name="anchor">Anchor position within the kernel. The value (−1,−1) means that the anchor is at the kernel center.</param>
        /// <param name="normalize">Specifying whether the kernel is normalized by its area or not.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of constant border type</param>
        /// <returns>Blurred image</returns>
        public static GMat BoxFilter(
            GMat src,
            CvEnum.DepthType dtype,
            Size ksize,
            Point anchor,
            bool normalize = true,
            CvEnum.BorderType borderType = BorderType.Default,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiBoxFilter(
                    src,
                    dtype,
                    ref ksize,
                    ref anchor,
                    normalize,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBoxFilter(
            IntPtr src,
            CvEnum.DepthType dtype,
            ref Size ksize,
            ref Point anchor,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool normalize,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Blurs an image using the normalized box filter.
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <param name="ksize">Blurring kernel size.</param>
        /// <param name="anchor">Anchor point; default value Point(-1,-1) means that the anchor is at the kernel center.</param>
        /// <param name="borderType">Border mode used to extrapolate pixels outside of the image</param>
        /// <param name="borderValue">Border value in case of constant border type</param>
        /// <returns>Blurred image</returns>
        public static GMat Blur(
                GMat src,
                Size ksize,
                Point anchor,
                CvEnum.BorderType borderType = BorderType.Default,
                MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiBlur(
                    src,
                    ref ksize,
                    ref anchor,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBlur(
            IntPtr src,
            ref Size ksize,
            ref Point anchor,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Blurs an image using a Gaussian filter.
        /// </summary>
        /// <param name="src">Input image</param>
        /// <param name="kSize">Gaussian kernel size. ksize.width and ksize.height can differ but they both must be positive and odd. Or, they can be zero's and then they are computed from sigma.</param>
        /// <param name="sigmaX">Gaussian kernel standard deviation in X direction.</param>
        /// <param name="sigmaY">Gaussian kernel standard deviation in Y direction; if sigmaY is zero, it is set to be equal to sigmaX, if both sigmas are zeros, they are computed from ksize.width and ksize.height, respectively.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of constant border type</param>
        /// <returns>The blurred image</returns>
        public static GMat GaussianBlur(
            GMat src,
            Size kSize,
            double sigmaX,
            double sigmaY = 0,
            CvEnum.BorderType borderType = BorderType.Default,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiGaussianBlur(
                    src,
                    ref kSize,
                    sigmaX,
                    sigmaY,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiGaussianBlur(
            IntPtr src,
            ref Size kSize,
            double sigmaX,
            double sigmaY,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Blurs an image using the median filter.
        /// </summary>
        /// <param name="src">input matrix (image)</param>
        /// <param name="kSize">aperture linear size; it must be odd and greater than 1, for example: 3, 5, 7 ...</param>
        /// <returns>The blurred image</returns>
        public static GMat MedianBlur(
            GMat src,
            int kSize)
        {
            return new GMat(
                cveGapiMedianBlur(
                    src,
                    kSize),
                true
            );

        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMedianBlur(
            IntPtr src,
            int kSize);


        /// <summary>
        /// Erodes an image by using a specific structuring element
        /// </summary>
        /// <param name="src">Input image</param>
        /// <param name="kernel">Structuring element used for erosion;</param>
        /// <param name="anchor">Position of the anchor within the element; default value (-1, -1) means that the anchor is at the element center.</param>
        /// <param name="iterations">Number of times erosion is applied.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of a constant border</param>
        /// <returns>The eroded image.</returns>
        public static GMat Erode(
            GMat src,
            Mat kernel,
            Point anchor,
            int iterations = 1,
            CvEnum.BorderType borderType = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar()
            )
        {
            return new GMat(
                cveGapiErode(
                    src,
                    kernel,
                    ref anchor,
                    iterations,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiErode(
            IntPtr src,
            IntPtr kernel,
            ref Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Erodes an image by using 3 by 3 rectangular structuring element.
        /// </summary>
        /// <param name="src">Input image</param>
        /// <param name="iterations">Number of times erosion is applied.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of a constant border</param>
        /// <returns>The eroded image.</returns>
        public static GMat Erode3x3(
            GMat src,
            int iterations = 1,
            CvEnum.BorderType borderType = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiErode3x3(
                    src,
                    iterations,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiErode3x3(
            IntPtr src,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Dilates an image by using a specific structuring element.
        /// </summary>
        /// <param name="src">Input image.</param>
        /// <param name="kernel">Structuring element used for dilation; if element=Mat(), a 3 x 3 rectangular structuring element is used. Kernel can be created using getStructuringElement.</param>
        /// <param name="anchor">Position of the anchor within the element; default value (-1, -1) means that the anchor is at the element center.</param>
        /// <param name="iterations">Number of times dilation is applied.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of a constant border</param>
        /// <returns>Result of the dilation</returns>
        public static GMat Dilate(
            GMat src,
            Mat kernel,
            Point anchor,
            int iterations = 1,
            CvEnum.BorderType borderType = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar()
        )
        {
            return new GMat(
                cveGapiDilate(
                    src,
                    kernel,
                    ref anchor,
                    iterations,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiDilate(
            IntPtr src,
            IntPtr kernel,
            ref Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Dilates an image by using 3 by 3 rectangular structuring element.
        /// </summary>
        /// <param name="src">Input image.</param>
        /// <param name="iterations">Number of times dilation is applied.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of a constant border</param>
        /// <returns>Result of the dilation</returns>
        public static GMat Dilate3x3(
            GMat src,
            int iterations =1,
            CvEnum.BorderType borderType = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiDilate3x3(
                    src,
                    iterations,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiDilate3x3(
            IntPtr src,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Performs advanced morphological transformations.
        /// </summary>
        /// <param name="src">Input image.</param>
        /// <param name="op">Type of a morphological operation</param>
        /// <param name="kernel">Structuring element</param>
        /// <param name="anchor">Anchor position within the element. Both negative values mean that the anchor is at the kernel center.</param>
        /// <param name="iterations">Number of times erosion and dilation are applied.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of a constant border.</param>
        /// <returns>Result of the morphological transformation.</returns>
        public static GMat MorphologyEx(
            GMat src,
            CvEnum.MorphOp op,
            Mat kernel,
            Point anchor,
            int iterations =1,
            CvEnum.BorderType borderType = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiMorphologyEx(
                    src,
                    op,
                    kernel,
                    ref anchor,
                    iterations,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMorphologyEx(
            IntPtr src,
            CvEnum.MorphOp op,
            IntPtr kernel,
            ref Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Calculates the first, second, third, or mixed image derivatives using an extended Sobel operator.
        /// </summary>
        /// <param name="src">Input image.</param>
        /// <param name="ddepth">Output image depth。</param>
        /// <param name="dx">Order of the derivative x.</param>
        /// <param name="dy">Order of the derivative y.</param>
        /// <param name="ksize">Size of the extended Sobel kernel; it must be odd.</param>
        /// <param name="scale">Optional scale factor for the computed derivative values; by default, no scaling is applied </param>
        /// <param name="delta">Optional delta value that is added to the results prior to storing them in dst.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of constant border type</param>
        /// <returns>The sobel filtered image</returns>
        public static GMat Sobel(
            GMat src,
            CvEnum.DepthType ddepth,
            int dx,
            int dy,
            int ksize = 3,
            double scale = 1,
            double delta =0,
            CvEnum.BorderType borderType = BorderType.Default,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiSobel(
                    src,
                    ddepth,
                    dx,
                    dy,
                    ksize,
                    scale,
                    delta,
                    borderType,
                    ref borderValue),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSobel(
            IntPtr src,
            CvEnum.DepthType ddepth,
            int dx,
            int dy,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        /// <summary>
        /// Calculates the first, second, third, or mixed image derivatives using an extended Sobel operator.
        /// </summary>
        /// <param name="src">Input image</param>
        /// <param name="ddepth">Output image depth</param>
        /// <param name="order">Order of the derivatives</param>
        /// <param name="ksize">Size of the extended Sobel kernel; it must be odd.</param>
        /// <param name="scale">Optional scale factor for the computed derivative values; by default, no scaling is applied</param>
        /// <param name="delta">Optional delta value that is added to the results prior to storing them in dst.</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <param name="borderValue">Border value in case of constant border type</param>
        /// <returns>First returned matrix correspond to dx derivative while the second one to dy</returns>
        public static Tuple<GMat, GMat> SobelXY(
            GMat src,
            CvEnum.DepthType ddepth,
            int order,
            int ksize = 3,
            double scale = 1,
            double delta = 0,
            CvEnum.BorderType borderType = BorderType.Default,
            MCvScalar borderValue = new MCvScalar())
        {
            GMat sobelX = new GMat();
            GMat sobelY = new GMat();
            cveGapiSobelXY(
                src,
                ddepth,
                order,
                ksize,
                scale,
                delta,
                borderType,
                ref borderValue,
                sobelX,
                sobelY);
            return new Tuple<GMat, GMat>(sobelX, sobelY);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGapiSobelXY(
            IntPtr src,
            CvEnum.DepthType ddepth,
            int order,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue,
            IntPtr sobelX,
            IntPtr sobelY);

        /// <summary>
        /// Calculates the Laplacian of an image.
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <param name="ddepth">Desired depth of the destination image.</param>
        /// <param name="ksize">Aperture size used to compute the second-derivative filters.</param>
        /// <param name="scale">Optional scale factor for the computed Laplacian values. By default, no scaling is applied.</param>
        /// <param name="delta">Optional delta value that is added to the results prior to storing them in dst .</param>
        /// <param name="borderType">Pixel extrapolation method</param>
        /// <returns>Destination image of the same size and the same number of channels as src.</returns>
        public static GMat Laplacian(
            GMat src,
            CvEnum.DepthType ddepth,
            int ksize = 1,
            double scale = 1,
            double delta = 0,
            CvEnum.BorderType borderType = BorderType.Default)
        {
            return new GMat(
                cveGapiLaplacian(
                    src,
                    ddepth,
                    ksize,
                    scale,
                    delta,
                    borderType),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiLaplacian(
            IntPtr src,
            CvEnum.DepthType ddepth,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType);

        /// <summary>
        /// Applies the bilateral filter to an image. BilateralFilter can reduce unwanted noise very well while keeping edges fairly sharp. However, it is very slow compared to most filters.
        /// </summary>
        /// <param name="src">Source 8-bit or floating-point, 1-channel or 3-channel image.</param>
        /// <param name="d">Diameter of each pixel neighborhood that is used during filtering. If it is non-positive, it is computed from sigmaSpace.</param>
        /// <param name="sigmaColor">Filter sigma in the color space. A larger value of the parameter means that farther colors within the pixel neighborhood (see sigmaSpace) will be mixed together, resulting in larger areas of semi-equal color.</param>
        /// <param name="sigmaSpace">Filter sigma in the coordinate space. A larger value of the parameter means that farther pixels will influence each other as long as their colors are close enough (see sigmaColor ). When d&gt;0, it specifies the neighborhood size regardless of sigmaSpace. Otherwise, d is proportional to sigmaSpace.</param>
        /// <param name="borderType">border mode used to extrapolate pixels outside of the image</param>
        /// <returns>Destination image of the same size and type as src.</returns>
        public static GMat BilateralFilter(
            GMat src,
            int d,
            double sigmaColor,
            double sigmaSpace,
            CvEnum.BorderType borderType = BorderType.Default)
        {
            return new GMat(
                cveGapiBilateralFilter(
                    src,
                    d,
                    sigmaColor,
                    sigmaColor,
                    borderType),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBilateralFilter(
            IntPtr src,
            int d,
            double sigmaColor,
            double sigmaSpace,
            CvEnum.BorderType borderType);

        /// <summary>
        /// Finds edges in an image using the Canny algorithm.
        /// </summary>
        /// <param name="image">8-bit input image</param>
        /// <param name="threshold1">first threshold for the hysteresis procedure.</param>
        /// <param name="threshold2">second threshold for the hysteresis procedure.</param>
        /// <param name="apertureSize">aperture size for the Sobel operator.</param>
        /// <param name="L2gradient">If true, a more accurate L2 norm should be used to calculate the image gradient magnitude.</param>
        /// <returns>The canny edges</returns>
        public static GMat Canny(
            GMat image,
            double threshold1,
            double threshold2,
            int apertureSize,
            bool L2gradient)
        {
            return new GMat(
                cveGapiCanny(
                    image,
                    threshold1,
                    threshold2,
                    apertureSize,
                    L2gradient),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCanny(
            IntPtr image,
            double threshold1,
            double threshold2,
            int apertureSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool L2gradient);

        /// <summary>
        /// The function equalizes the histogram of the input image
        /// </summary>
        /// <param name="src">Source 8-bit single channel image.</param>
        /// <returns>The normalized image</returns>
        public static GMat EqualizeHist(GMat src)
        {
            return new GMat(
                cveGapiEqualizeHist(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiEqualizeHist(IntPtr src);

        /// <summary>
        /// Converts an image from BGR color space to RGB color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel image.</param>
        /// <returns>Output image, 8-bit unsigned 3-channel image.</returns>
        public static GMat BGR2RGB(GMat src)
        {
            return new GMat(
                cveGapiBGR2RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBGR2RGB(IntPtr src);

        /// <summary>
        /// Converts an image from RGB color space to gray-scaled.
        /// </summary>
        /// <param name="src">input image: 8-bit unsigned 3-channel image.</param>
        /// <returns>Output image, 8-bit unsigned 1-channel image.</returns>
        public static GMat RGB2Gray(GMat src)
        {
            return new GMat(
                cveGapiRGB2Gray1(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRGB2Gray1(IntPtr src);

        /// <summary>
        /// Converts an image from RGB color space to gray-scaled.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel image.</param>
        /// <param name="rY">Float multiplier for R channel.</param>
        /// <param name="gY">Float multiplier for G channel.</param>
        /// <param name="bY">Float multiplier for B channel.</param>
        /// <returns>Output image, 8-bit unsigned 1-channel image.</returns>
        public static GMat RGB2Gray(GMat src, float rY, float gY, float bY)
        {
            return new GMat(
                cveGapiRGB2Gray2(src, rY, gY, bY),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRGB2Gray2(IntPtr src, float rY, float gY, float bY);

        /// <summary>
        /// Converts an image from BGR color space to gray-scaled.
        /// </summary>
        /// <param name="src">input image: 8-bit unsigned 3-channel image.</param>
        /// <returns>Output image, 8-bit unsigned 1-channel image.</returns>
        public static GMat BGR2Gray(GMat src)
        {
            return new GMat(
                cveGapiBGR2Gray(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBGR2Gray(IntPtr src);

        /// <summary>
        /// Converts an image from RGB color space to YUV color space.
        /// </summary>
        /// <param name="src">input image: 8-bit unsigned 3-channel image.</param>
        /// <returns>Output image, 8-bit unsigned 3-channel image</returns>

        public static GMat RGB2YUV(GMat src)
        {
            return new GMat(
                cveGapiRGB2YUV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRGB2YUV(IntPtr src);

        /// <summary>
        /// Converts an image from BGR color space to I420 color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel image</param>
        /// <returns>Output image, 8-bit unsigned 1-channel image. Width of I420 output image must be the same as width of input image. Height of I420 output image must be equal 3/2 from height of input image.</returns>
        public static GMat BGR2I420(GMat src)
        {
            return new GMat(
                cveGapiBGR2I420(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBGR2I420(IntPtr src);


        /// <summary>
        /// Converts an image from RGB color space to I420 color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel</param>
        /// <returns>Output image, 8-bit unsigned 1-channel</returns>
        public static GMat RGB2I420(GMat src)
        {
            return new GMat(
                cveGapiRGB2I420(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRGB2I420(IntPtr src);

        /// <summary>
        /// Converts an image from I420 color space to BGR color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 1-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat I4202BGR(GMat src)
        {
            return new GMat(
                cveGapiI4202BGR(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiI4202BGR(IntPtr src);

        /// <summary>
        /// Converts an image from I420 color space to BGR color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 1-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat I4202RGB(GMat src)
        {
            return new GMat(
                cveGapiI4202RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiI4202RGB(IntPtr src);

        /// <summary>
        /// Converts an image from BGR color space to LUV color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel image</param>
        /// <returns>Output image, 8-bit unsigned 3-channel image</returns>
        public static GMat BGR2LUV(GMat src)
        {
            return new GMat(
                cveGapiBGR2LUV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBGR2LUV(IntPtr src);

        /// <summary>
        /// Converts an image from LUV color space to BGR color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel.</returns>
        public static GMat LUV2BGR(GMat src)
        {
            return new GMat(
                cveGapiLUV2BGR(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiLUV2BGR(IntPtr src);

        /// <summary>
        /// Converts an image from YUV color space to BGR color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel.</param>
        /// <returns>Output image, 8-bit unsigned 3-channel.</returns>
        public static GMat YUV2BGR(GMat src)
        {
            return new GMat(
                cveGapiYUV2BGR(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiYUV2BGR(IntPtr src);

        /// <summary>
        /// Converts an image from BGR color space to YUV color space.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat BGR2YUV(GMat src)
        {
            return new GMat(
                cveGapiBGR2YUV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBGR2YUV(IntPtr src);

        /// <summary>
        /// Converts an image from RGB color space to Lab color space.
        /// </summary>
        /// <param name="src">input image: 8-bit unsigned 3-channel.</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat RGB2Lab(GMat src)
        {
            return new GMat(
                cveGapiRGB2Lab(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRGB2Lab(IntPtr src);

        /// <summary>
        /// Converts an image from YUV color space to RGB.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat YUV2RGB(GMat src)
        {
            return new GMat(
                cveGapiYUV2RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiYUV2RGB(IntPtr src);

        /// <summary>
        /// Converts an image from NV12 (YUV420p) color space to RGB.
        /// </summary>
        /// <param name="srcY">Input image: 8-bit unsigned 1-channel</param>
        /// <param name="srcUV">Input image: 8-bit unsigned 2-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat NV12toRGB(GMat srcY, GMat srcUV)
        {
            return new GMat(
                cveGapiNV12toRGB(srcY, srcUV),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiNV12toRGB(IntPtr srcY, IntPtr srcUV);

        /// <summary>
        /// Converts an image from NV12 (YUV420p) color space to gray-scaled.
        /// </summary>
        /// <param name="srcY">Input image: 8-bit unsigned 1-channel</param>
        /// <param name="srcUV">Input image: 8-bit unsigned 2-channel</param>
        /// <returns>Output image, 8-bit unsigned 1-channel</returns>
        public static GMat NV12toGray(GMat srcY, GMat srcUV)
        {
            return new GMat(
                cveGapiNV12toGray(srcY, srcUV),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiNV12toGray(IntPtr srcY, IntPtr srcUV);

        /// <summary>
        /// Converts an image from NV12 (YUV420p) color space to BGR.
        /// </summary>
        /// <param name="srcY">Input image: 8-bit unsigned 1-channel</param>
        /// <param name="srcUV">Input image: 8-bit unsigned 2-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat NV12toBGR(GMat srcY, GMat srcUV)
        {
            return new GMat(
                cveGapiNV12toBGR(srcY, srcUV),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiNV12toBGR(IntPtr srcY, IntPtr srcUV);

        /// <summary>
        /// Converts an image from BayerGR color space to RGB. The function converts an input image from BayerGR color space to RGB. The conventional ranges for G, R, and B channel values are 0 to 255.
        /// </summary>
        /// <param name="src">input image: 8-bit unsigned 1-channel image</param>
        /// <returns>Output 8-bit unsigned 3-channel image</returns>
        public static GMat BayerGR2RGB(GMat src)
        {
            return new GMat(
                cveGapiBayerGR2RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiBayerGR2RGB(IntPtr srcGR);

        /// <summary>
        /// Converts an image from RGB color space to HSV.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel</param>
        /// <returns>Output image, 8-bit unsigned 3-channel</returns>
        public static GMat RGB2HSV(GMat src)
        {
            return new GMat(
                cveGapiRGB2HSV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRGB2HSV(IntPtr src);

        /// <summary>
        /// Converts an image from RGB color space to YUV422.
        /// </summary>
        /// <param name="src">Input image: 8-bit unsigned 3-channel</param>
        /// <returns>Output image, 8-bit unsigned 2-channel</returns>
        public static GMat RGB2YUV422(GMat src)
        {
            return new GMat(
                cveGapiRGB2YUV422(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRGB2YUV422(IntPtr src);

        /// <summary>
        /// Select values from either first or second of input matrices by given mask. The function set to the output matrix either the value from the first input matrix if corresponding value of mask matrix is 255, or value from the second input matrix (if value of mask matrix set to 0).
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix.</param>
        /// <param name="mask">Mask input matrix.</param>
        /// <returns>Select result from either first or second of input matrices by given mask.</returns>
        public static GMat Select(GMat src1, GMat src2, GMat mask)
        {
            return new GMat(
                cveGapiSelect(src1, src2, mask),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSelect(IntPtr src1, IntPtr src2, IntPtr mask);

        /// <summary>
        /// Calculates the per-element minimum of two matrices of the same size, number of channels and depth
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same size and depth as src1.</param>
        /// <returns>The per-element minimum of two matrices of the same size, number of channels and depth</returns>
        public static GMat Min(GMat src1, GMat src2)
        {
            return new GMat(
                cveGapiMin(src1, src2),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMin(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Calculates the per-element maximum of two matrices of the same size, number of channels and depth
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix of the same size and depth as src1.</param>
        /// <returns>The per-element maximum of two matrices of the same size, number of channels and depth</returns>
        public static GMat Max(GMat src1, GMat src2)
        {
            return new GMat(
                cveGapiMax(src1, src2),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMax(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Calculates absolute difference between two matrices of the same size and depth
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix</param>
        /// <returns>Absolute difference between two matrices of the same size and depth</returns>
        public static GMat AbsDiff(GMat src1, GMat src2)
        {
            return new GMat(
                cveGapiAbsDiff(src1, src2),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiAbsDiff(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Calculates absolute difference between matrix elements and given scalar value
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <param name="c">Scalar to be subtracted.</param>
        /// <returns>Absolute difference between matrix elements and given scalar value</returns>
        public static GMat AbsDiffC(GMat src, GScalar c)
        {
            return new GMat(
                cveGapiAbsDiffC(src, c),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiAbsDiffC(IntPtr src, IntPtr c);

        /// <summary>
        /// Calculates sum of all matrix elements.
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <returns>Sum of all matrix elements, independently for each channel.</returns>
        public static GScalar Sum(GMat src)
        {
            return new GScalar(cveGapiSum(src));
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiSum(IntPtr src);

        /// <summary>
        /// Calculates the weighted sum of two matrices
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="alpha">Weight of the first matrix elements.</param>
        /// <param name="src2">Second input matrix of the same size and channel number as src1</param>
        /// <param name="beta">Weight of the second matrix elements.</param>
        /// <param name="gamma">Scalar added to each sum.</param>
        /// <param name="ddepth">Optional depth of the output matrix.</param>
        /// <returns>The weighted sum of two matrices</returns>
        public static GMat AddWeighted(
            GMat src1,
            double alpha,
            GMat src2,
            double beta,
            double gamma,
            CvEnum.DepthType ddepth)
        {
            return new GMat(
                cveGapiAddWeighted(
                    src1,
                    alpha,
                    src2,
                    beta,
                    gamma,
                    ddepth),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiAddWeighted(
            IntPtr src1,
            double alpha,
            IntPtr src2,
            double beta,
            double gamma,
            CvEnum.DepthType ddepth);

        /// <summary>
        /// Calculates the absolute L1 norm of a matrix.
        /// </summary>
        /// <param name="src">Tnput matrix.</param>
        /// <returns>The absolute L1 norm of a matrix.</returns>
        public static GScalar NormL1(GMat src)
        {
            return new GScalar(cveGapiNormL1(src));
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiNormL1(IntPtr src);

        /// <summary>
        /// Calculates the absolute L2 norm of a matrix.
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <returns>The absolute L2 norm of a matrix.</returns>
        public static GScalar NormL2(GMat src)
        {
            return new GScalar(cveGapiNormL2(src));
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiNormL2(IntPtr src);

        /// <summary>
        /// Calculates the absolute infinite norm of a matrix.
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <returns>The absolute infinite norm of a matrix.</returns>
        public static GScalar NormInf(GMat src)
        {
            return new GScalar(cveGapiNormInf(src));
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiNormInf(IntPtr src);

        /// <summary>
        /// Calculates the integral of an image.
        /// </summary>
        /// <param name="src">Input image.</param>
        /// <param name="sdepth">Desired depth of the integral and the tilted integral images, CV_32S, CV_32F, or CV_64F.</param>
        /// <param name="sqdepth">Desired depth of the integral image of squared pixel values, CV_32F or CV_64F.</param>
        /// <returns>Two GMats, first one is the tilted integral image, second one is the integral image of squared pixel values.</returns>
        public static Tuple<GMat, GMat> Integral(GMat src, CvEnum.DepthType sdepth = DepthType.Default, CvEnum.DepthType sqdepth = DepthType.Default)
        {
            GMat dst1 = new GMat();
            GMat dst2 = new GMat();
            cveGapiIntegral(src, sdepth, sqdepth, dst1, dst2);
            return new Tuple<GMat, GMat>(dst1, dst2);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGapiIntegral(IntPtr src, CvEnum.DepthType sdepth, CvEnum.DepthType sqdepth, IntPtr dst1, IntPtr dst2);

        /// <summary>
        /// Applies a fixed-level threshold to each matrix element.
        /// </summary>
        /// <param name="src">Input matrix</param>
        /// <param name="thresh">Threshold value.</param>
        /// <param name="maxval">Maximum value to use </param>
        /// <param name="type">Thresholding type </param>
        /// <returns>The thresholded image</returns>
        public static GMat Threshold(
            GMat src,
            GScalar thresh,
            GScalar maxval,
            CvEnum.ThresholdType type)
        {
            return new GMat(
                cveGapiThreshold(src, thresh, maxval, type),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiThreshold(IntPtr src, IntPtr thresh, IntPtr maxval, CvEnum.ThresholdType type);

        /// <summary>
        /// Applies range-level thresholding to a single- or multiple-channel matrix. It sets output pixel value to OxFF if the corresponding pixel value of input matrix is in specified range,or 0 otherwise.
        /// </summary>
        /// <param name="src">Input matrix (CV_8UC1).</param>
        /// <param name="threshLow">Lower boundary value.</param>
        /// <param name="threshUp">Upper boundary value.</param>
        /// <returns>Range-level thresholding to a single- or multiple-channel matrix. It sets output pixel value to OxFF if the corresponding pixel value of input matrix is in specified range,or 0 otherwise.</returns>
        public static GMat InRange(
            GMat src,
            GScalar threshLow,
            GScalar threshUp)
        {
            return new GMat(
                cveGapiInRange(src, threshLow, threshUp),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiInRange(IntPtr src, IntPtr threshLow, IntPtr threshUp);

        /// <summary>
        /// Creates one 4-channel matrix out of 4 single-channel ones.
        /// </summary>
        /// <param name="src1">First input CV_8UC1 matrix to be merged.</param>
        /// <param name="src2">Second input CV_8UC1 matrix to be merged.</param>
        /// <param name="src3">Third input CV_8UC1 matrix to be merged.</param>
        /// <param name="src4">Fourth input CV_8UC1 matrix to be merged.</param>
        /// <returns>A single multi-channel matrix</returns>
        public static GMat Merge4(
            GMat src1, GMat src2, GMat src3, GMat src4)
        {
            return new GMat(
                cveGapiMerge4(src1, src2, src3, src4),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMerge4(IntPtr src1, IntPtr src2, IntPtr src3, IntPtr src4);

        /// <summary>
        /// Creates one 3-channel matrix out of 3 single-channel ones.
        /// </summary>
        /// <param name="src1">First input CV_8UC1 matrix to be merged.</param>
        /// <param name="src2">Second input CV_8UC1 matrix to be merged.</param>
        /// <param name="src3">Third input CV_8UC1 matrix to be merged.</param>
        /// <returns>A single multi-channel matrix</returns>
        public static GMat Merge3(
            GMat src1, GMat src2, GMat src3)
        {
            return new GMat(
                cveGapiMerge3(src1, src2, src3),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiMerge3(IntPtr src1, IntPtr src2, IntPtr src3);

        /// <summary>
        /// Divides a 4-channel matrix into 4 single-channel matrices.
        /// </summary>
        /// <param name="src">Input CV_8UC4 matrix.</param>
        /// <returns>4 single-channel matrices.</returns>
        public static Tuple<GMat, GMat, GMat, GMat> Split4(GMat src)
        {
            GMat dst1 = new GMat();
            GMat dst2 = new GMat();
            GMat dst3 = new GMat();
            GMat dst4 = new GMat();
            cveGapiSplit4(src, dst1, dst2, dst3, dst4);
            return new Tuple<GMat, GMat, GMat, GMat>(dst1, dst2, dst3, dst4);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGapiSplit4(IntPtr src, IntPtr dst1, IntPtr dst2, IntPtr dst3, IntPtr dst4);

        /// <summary>
        /// Divides a 3-channel matrix into 3 single-channel matrices.
        /// </summary>
        /// <param name="src">Input CV_8UC3 matrix.</param>
        /// <returns>3 single-channel matrices.</returns>
        public static Tuple<GMat, GMat, GMat> Split3(GMat src)
        {
            GMat dst1 = new GMat();
            GMat dst2 = new GMat();
            GMat dst3 = new GMat();

            cveGapiSplit3(src, dst1, dst2, dst3);
            return new Tuple<GMat, GMat, GMat>(dst1, dst2, dst3);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveGapiSplit3(IntPtr src, IntPtr dst1, IntPtr dst2, IntPtr dst3);

        /// <summary>
        /// Applies a generic geometrical transformation to an image.
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <param name="map1">The first map of either (x,y) points or just x values having the type CV_16SC2, CV_32FC1, or CV_32FC2.</param>
        /// <param name="map2">The second map of y values having the type CV_16UC1, CV_32FC1, or none (empty map if map1 is (x,y) points), respectively.</param>
        /// <param name="interpolation">Interpolation method</param>
        /// <param name="borderMode">Pixel extrapolation method</param>
        /// <param name="borderValue">Value used in case of a constant border. By default, it is 0.</param>
        /// <returns>The transformed image.</returns>
        public static GMat Remap(
            GMat src,
            Mat map1,
            Mat map2,
            CvEnum.Inter interpolation,
            CvEnum.BorderType borderMode = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiRemap(src, map1, map2, interpolation, borderMode, ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiRemap(
            IntPtr src,
            IntPtr map1,
            IntPtr map2,
            CvEnum.Inter interpolation,
            CvEnum.BorderType borderMode,
            ref MCvScalar borderValue);

        /// <summary>
        /// Flips a 2D matrix around vertical, horizontal, or both axes.
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <param name="flipCode">A flag to specify how to flip the array.</param>
        /// <returns>The flipped GMat.</returns>
        public static GMat Flip(
            GMat src,
            CvEnum.FlipType flipCode)
        {
            return new GMat(
                cveGapiFlip(src, flipCode),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiFlip(IntPtr src, CvEnum.FlipType flipCode);

        /// <summary>
        /// Crops a 2D matrix.
        /// </summary>
        /// <param name="src">Input matrix.</param>
        /// <param name="rect">A rect to crop a matrix to</param>
        /// <returns>The cropped matrix</returns>
        public static GMat Crop(
            GMat src,
            Rectangle rect)
        {
            return new GMat(
                cveGapiCrop(src, ref rect),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiCrop(IntPtr src, ref Rectangle rect);

        /// <summary>
        /// Applies horizontal concatenation to given matrices.
        /// </summary>
        /// <param name="src1">First input matrix to be considered for horizontal concatenation.</param>
        /// <param name="src2">Second input matrix to be considered for horizontal concatenation.</param>
        /// <returns>The horizontally concatenated matrix</returns>
        public static GMat ConcatHor(GMat src1, GMat src2)
        {
            return new GMat(
                cveGapiConcatHor(src1, src2),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiConcatHor(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Applies horizontal concatenation to given matrices.
        /// </summary>
        /// <param name="v">vector of input matrices to be concatenated vertically.</param>
        /// <returns>The horizontally concatenated matrix</returns>
        public static GMat ConcatHor(VectorOfGMat v)
        {
            return new GMat(
                cveGapiConcatHorV(v),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiConcatHorV(IntPtr v);

        /// <summary>
        /// Applies vertical concatenation to given matrices.
        /// </summary>
        /// <param name="src1">First input matrix to be considered for vertical concatenation.</param>
        /// <param name="src2">Second input matrix to be considered for vertical concatenation.</param>
        /// <returns>The vertically concatenated matrix</returns>
        public static GMat ConcatVert(GMat src1, GMat src2)
        {
            return new GMat(
                cveGapiConcatVert(src1, src2),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiConcatVert(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Applies vertical concatenation to given matrices.
        /// </summary>
        /// <param name="v">vector of input matrices to be concatenated vertically.</param>
        /// <returns>The vertically concatenated matrix</returns>
        public static GMat ConcatVert(VectorOfGMat v)
        {
            return new GMat(
                cveGapiConcatVertV(v),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiConcatVertV(IntPtr v);

        /// <summary>
        /// Fills the output matrix with values from the look-up table. Indices of the entries are taken from the input matrix.
        /// </summary>
        /// <param name="src">Input matrix of 8-bit elements.</param>
        /// <param name="lut">Look-up table of 256 elements; in case of multi-channel input array, the table should either have a single channel (in this case the same table is used for all channels) or the same number of channels as in the input matrix.</param>
        /// <returns>A look-up table transform of a matrix.</returns>
        public static GMat LUT(GMat src, Mat lut)
        {
            return new GMat(
                cveGapiLUT(src, lut),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiLUT(IntPtr src, IntPtr lut);

        /// <summary>
        /// Converts a matrix to another data depth with optional scaling.
        /// </summary>
        /// <param name="src">input matrix to be converted from.</param>
        /// <param name="rdepth">Desired output matrix depth.</param>
        /// <param name="alpha">Optional scale factor.</param>
        /// <param name="beta">Optional delta added to the scaled values.</param>
        /// <returns>A matrix of the specific data depth with optional scaling.</returns>
        public static GMat ConvertTo(
            GMat src,
            CvEnum.DepthType rdepth,
            double alpha = 1,
            double beta = 0)
        {
            return new GMat(
                cveGapiConvertTo(src, rdepth, alpha, beta),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiConvertTo(
            IntPtr src,
            CvEnum.DepthType rdepth,
            double alpha,
            double beta);

        /// <summary>
        /// Normalizes the norm or value range of an array
        /// </summary>
        /// <param name="src">The input array</param>
        /// <param name="alpha">Norm value to normalize to or the lower range boundary in case of the range normalization.</param>
        /// <param name="beta">Upper range boundary in case of the range normalization; it is not used for the norm normalization.</param>
        /// <param name="normType">The normalization type</param>
        /// <param name="dType">Optional depth type for the returned array</param>
        /// <returns>The normalized output array</returns>
        public static GMat Normalize(
            GMat src,
            double alpha,
            double beta,
            CvEnum.NormType normType = CvEnum.NormType.L2,
            CvEnum.DepthType dType = CvEnum.DepthType.Default)
        {
            return new GMat(
                cveGapiNormalize(src, alpha, beta, normType, dType),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiNormalize(
            IntPtr src,
            double alpha,
            double beta,
            CvEnum.NormType normType,
            CvEnum.DepthType ddepth);

        /// <summary>
        /// Applies a perspective transformation to an image.
        /// </summary>
        /// <param name="src">Source image</param>
        /// <param name="m">3x3 transformation matrix</param>
        /// <param name="dsize">Size of the output image.</param>
        /// <param name="interMethod">Interpolation method</param>
        /// <param name="warpMethod">Warp method</param>
        /// <param name="borderMode">Pixel extrapolation method</param>
        /// <param name="borderValue">A value used to fill outliers</param>
        /// <returns>The transformed image.</returns>
        public static GMat WarpPerspective(
            GMat src,
            Mat m,
            Size dsize,
            CvEnum.Inter interMethod = CvEnum.Inter.Linear,
            CvEnum.Warp warpMethod = CvEnum.Warp.Default,
            CvEnum.BorderType borderMode = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiWarpPerspective(
                    src,
                    m,
                    ref dsize,
                    (int)interMethod | (int)warpMethod,
                    borderMode,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiWarpPerspective(
            IntPtr src,
            IntPtr m,
            ref Size dsize,
            int flags,
            CvEnum.BorderType borderMode,
            ref MCvScalar borderValue);

        /// <summary>
        /// Applies an affine transformation to an image.
        /// </summary>
        /// <param name="src">Source image</param>
        /// <param name="m">2x3 transformation matrix</param>
        /// <param name="dsize">Size of the output image.</param>
        /// <param name="interMethod">Interpolation method</param>
        /// <param name="warpMethod">Warp method</param>
        /// <param name="borderMode">Pixel extrapolation method</param>
        /// <param name="borderValue">A value used to fill outliers</param>
        /// <returns>The transformed image.</returns>
        public static GMat WarpAffine(
            GMat src,
            Mat m,
            Size dsize,
            CvEnum.Inter interMethod = CvEnum.Inter.Linear, 
            CvEnum.Warp warpMethod = CvEnum.Warp.Default,
            CvEnum.BorderType borderMode = BorderType.Constant,
            MCvScalar borderValue = new MCvScalar())
        {
            return new GMat(
                cveGapiWarpAffine(
                    src,
                    m,
                    ref dsize,
                    (int)interMethod | (int)warpMethod,
                    borderMode,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiWarpAffine(
            IntPtr src,
            IntPtr m,
            ref Size dsize,
            int flags,
            CvEnum.BorderType borderMode,
            ref MCvScalar borderValue);


        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <param name="src">Source matrix</param>
        /// <returns>The transposed matrix.</returns>
        public static GMat Transpose(GMat src)
        {
            return new GMat(cveGapiTranspose(src), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiTranspose(IntPtr src);

        /// <summary>
        /// Computes disparity/depth map for the specified stereo-pair. The function computes disparity or depth map depending on passed StereoOutputFormat argument.
        /// </summary>
        /// <param name="left">8-bit single-channel left image of CV_8UC1 type.</param>
        /// <param name="right">8-bit single-channel right image of CV_8UC1 type.</param>
        /// <param name="of">enum to specified output kind: depth or disparity and corresponding type</param>
        /// <returns>disparity/depth map for the specified stereo-pair</returns>
        public static GMat Stereo(GMat left, GMat right, StereoOutputFormat of)
        {
            return new GMat(cveGapiStereo(left, right, of), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern IntPtr cveGapiStereo(IntPtr left, IntPtr right, StereoOutputFormat of);
    }
}

