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

    public static partial class GapiInvoke
    {
        static GapiInvoke()
        {
            CvInvoke.Init();
        }

        public static GMat Resize(GMat src, Size dsize, double fx = 0, double fy = 0, CvEnum.Inter interpolation = Inter.Linear)
        {
            return new GMat(cveGapiResize(src, ref dsize, fx, fy, interpolation), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiResize(IntPtr src, ref Size dsize, double fx, double fy, CvEnum.Inter interpolation);

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
        private extern static IntPtr cveGapiBitwiseNot(IntPtr src);

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
        private extern static IntPtr cveGapiAdd(IntPtr src1, IntPtr src2, CvEnum.DepthType ddepth);


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
        private extern static IntPtr cveGapiAddC(IntPtr src1, IntPtr c, CvEnum.DepthType ddepth);


        public static GMat Sub(GMat src1, GMat src2, CvEnum.DepthType ddepth)
        {
            return new GMat(cveGapiSub(src1, src2, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiSub(IntPtr src1, IntPtr src2, CvEnum.DepthType ddepth);

        public static GMat SubC(GMat src1, GScalar c, CvEnum.DepthType ddepth)
        {
            return new GMat(cveGapiSubC(src1, c, ddepth), true);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiSubC(IntPtr src1, IntPtr c, CvEnum.DepthType ddepth);

        public static GMat SubRC(GScalar c, GMat src1, CvEnum.DepthType ddepth)
        {
            return new GMat(cveGapiSubRC(c, src1, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiSubRC(IntPtr c, IntPtr src1, CvEnum.DepthType ddepth);


        public static GMat Mul(GMat src1, GMat src2, double scale, CvEnum.DepthType ddepth)
        {
            return new GMat(cveGapiMul(src1, src2, scale, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiMul(IntPtr src1, IntPtr src2, double scale, CvEnum.DepthType ddepth);

        public static GMat MulC(GMat src1, GScalar c, CvEnum.DepthType ddepth)
        {
            return new GMat(cveGapiMulC(src1, c, ddepth), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiMulC(IntPtr src, IntPtr scale, CvEnum.DepthType ddepth);


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
        private extern static IntPtr cveGapiDiv(IntPtr src1, IntPtr src2, double scale, CvEnum.DepthType ddepth);

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
        private extern static IntPtr cveGapiDivC(IntPtr src, IntPtr divisor, double scale, CvEnum.DepthType ddepth);


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
        private extern static IntPtr cveGapiDivRC(IntPtr divident, IntPtr src, double scale, CvEnum.DepthType ddepth);

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
        private extern static IntPtr cveGapiMean(IntPtr src);


        public static Tuple<GMat, GMat> PolarToCart(
            GMat magnitude,
            GMat angle,
            bool angleInDegrees)
        {
            GMat outX = new GMat();
            GMat outY = new GMat();
            cveGapiPolarToCart(magnitude, angle, angleInDegrees, outX, outY);
            return new Tuple<GMat, GMat>(outX, outY);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveGapiPolarToCart(
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
        private extern static void cveGapiCartToPolar(
            IntPtr x,
            IntPtr y,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool angleInDegrees,
            IntPtr outMagnitude,
            IntPtr outAngle);


        public static GMat Phase(GMat x, GMat y, bool angleInDegrees)
        {
            return new GMat(cveGapiPhase(x, y, angleInDegrees), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiPhase(
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
        private extern static IntPtr cveGapiSqrt(IntPtr src);

        public static GMat CmpGT(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpGT(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpGT(IntPtr src1, IntPtr src2);

        public static GMat CmpGT(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpGTS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpGTS(IntPtr src1, IntPtr src2);

        public static GMat CmpLT(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpLT(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpLT(IntPtr src1, IntPtr src2);

        public static GMat CmpLT(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpLTS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpLTS(IntPtr src1, IntPtr src2);

        public static GMat CmpGE(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpGE(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpGE(IntPtr src1, IntPtr src2);

        public static GMat CmpGE(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpGES(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpGES(IntPtr src1, IntPtr src2);

        public static GMat CmpLE(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpLE(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpLE(IntPtr src1, IntPtr src2);

        public static GMat CmpLE(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpLES(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpLES(IntPtr src1, IntPtr src2);


        public static GMat CmpEQ(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpEQ(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpEQ(IntPtr src1, IntPtr src2);

        public static GMat CmpEQ(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpEQS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpEQS(IntPtr src1, IntPtr src2);

        public static GMat CmpNE(GMat src1, GMat src2)
        {
            return new GMat(cveGapiCmpNE(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpNE(IntPtr src1, IntPtr src2);

        public static GMat CmpNE(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiCmpNES(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiCmpNES(IntPtr src1, IntPtr src2);

        /// <summary>
        /// Computes bitwise conjunction of the two matrixes (src1 &amp; src2) Calculates the per-element bit-wise logical conjunction of two matrices of the same size.
        /// </summary>
        /// <param name="src1">First input matrix.</param>
        /// <param name="src2">Second input matrix.</param>
        /// <returns>Bitwise conjunction of the two matrixes (src1 &amp; src2)</returns>
        public static GMat BitwiseAnd(GMat src1, GMat src2)
        {
            return new GMat(cveGapiBitwiseAnd(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBitwiseAnd(IntPtr src1, IntPtr src2);

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
        private extern static IntPtr cveGapiBitwiseAndS(IntPtr src1, IntPtr src2);

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
        private extern static IntPtr cveGapiBitwiseOr(IntPtr src1, IntPtr src2);

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
        private extern static IntPtr cveGapiBitwiseOrS(IntPtr src1, IntPtr src2);

        public static GMat BitwiseXor(GMat src1, GMat src2)
        {
            return new GMat(cveGapiBitwiseXor(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBitwiseXor(IntPtr src1, IntPtr src2);

        public static GMat BitwiseXor(GMat src1, GScalar src2)
        {
            return new GMat(cveGapiBitwiseXorS(src1, src2), true);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBitwiseXorS(IntPtr src1, IntPtr src2);

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
        private extern static IntPtr cveGapiMask(IntPtr src, IntPtr mask);


        public static GMat SepFilter(
            GMat src,
            CvEnum.DepthType ddepth,
            Mat kernelX,
            Mat kernelY,
            Point anchor,
            MCvScalar delta,
            CvEnum.BorderType borderType,
            MCvScalar borderValue)
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
        private extern static IntPtr cveGapiSepFilter(
            IntPtr src,
            CvEnum.DepthType ddepth,
            IntPtr kernelX,
            IntPtr kernelY,
            ref Point anchor,
            ref MCvScalar delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat Filter2D(
            GMat src,
            CvEnum.DepthType ddepth,
            Mat kernel,
            Point anchor,
            MCvScalar delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue
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
        private extern static IntPtr cveGapiFilter2D(
            IntPtr src,
            CvEnum.DepthType ddepth,
            IntPtr kernel,
            ref Point anchor,
            ref MCvScalar delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat BoxFilter(
            GMat src,
            CvEnum.DepthType dtype,
            Size ksize,
            Point anchor,
            bool normalize,
            CvEnum.BorderType borderType,
            MCvScalar borderValue)
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
        private extern static IntPtr cveGapiBoxFilter(
            IntPtr src,
            CvEnum.DepthType dtype,
            ref Size ksize,
            ref Point anchor,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool normalize,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat Blur(
                GMat src,
                Size ksize,
                Point anchor,
                CvEnum.BorderType borderType,
                MCvScalar borderValue)
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
        private extern static IntPtr cveGapiBlur(
            IntPtr src,
            ref Size ksize,
            ref Point anchor,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat GaussianBlur(
            GMat src,
            Size ksize,
            double sigmaX,
            double sigmaY,
            CvEnum.BorderType borderType,
            MCvScalar borderValue)
        {
            return new GMat(
                cveGapiGaussianBlur(
                    src,
                    ref ksize,
                    sigmaX,
                    sigmaY,
                    borderType,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiGaussianBlur(
            IntPtr src,
            ref Size ksize,
            double sigmaX,
            double sigmaY,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat MedianBlur(
            GMat src,
            int ksize)
        {
            return new GMat(
                cveGapiMedianBlur(
                    src,
                    ksize),
                true
            );

        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiMedianBlur(
            IntPtr src,
            int ksize);


        public static GMat Erode(
            GMat src,
            Mat kernel,
            Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            MCvScalar borderValue
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
        private extern static IntPtr cveGapiErode(
            IntPtr src,
            IntPtr kernel,
            ref Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat Erode3x3(
            GMat src,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue)
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
        private extern static IntPtr cveGapiErode3x3(
            IntPtr src,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);


        public static GMat Dilate(
            GMat src,
            Mat kernel,
            Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            MCvScalar borderValue
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
        private extern static IntPtr cveGapiDilate(
            IntPtr src,
            IntPtr kernel,
            ref Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat Dilate3x3(
            GMat src,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue)
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
        private extern static IntPtr cveGapiDilate3x3(
            IntPtr src,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat MorphologyEx(
            GMat src,
            CvEnum.MorphOp op,
            Mat kernel,
            Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            MCvScalar borderValue)
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
        private extern static IntPtr cveGapiMorphologyEx(
            IntPtr src,
            CvEnum.MorphOp op,
            IntPtr kernel,
            ref Point anchor,
            int iterations,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static GMat Sobel(
            GMat src,
            CvEnum.DepthType ddepth,
            int dx,
            int dy,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue)
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
        private extern static IntPtr cveGapiSobel(
            IntPtr src,
            CvEnum.DepthType ddepth,
            int dx,
            int dy,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType,
            ref MCvScalar borderValue);

        public static Tuple<GMat, GMat> SobelXY(
            GMat src,
            CvEnum.DepthType ddepth,
            int order,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType,
            MCvScalar borderValue)
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
        private extern static void cveGapiSobelXY(
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

        public static GMat Laplacian(
            GMat src,
            CvEnum.DepthType ddepth,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType)
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
        private extern static IntPtr cveGapiLaplacian(
            IntPtr src,
            CvEnum.DepthType ddepth,
            int ksize,
            double scale,
            double delta,
            CvEnum.BorderType borderType);

        public static GMat BilateralFilter(
            GMat src,
            int d,
            double sigmaColor,
            double sigmaSpace,
            CvEnum.BorderType borderType)
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
        private extern static IntPtr cveGapiBilateralFilter(
            IntPtr src,
            int d,
            double sigmaColor,
            double sigmaSpace,
            CvEnum.BorderType borderType);

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
        private extern static IntPtr cveGapiCanny(
            IntPtr image,
            double threshold1,
            double threshold2,
            int apertureSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool L2gradient);

        public static GMat EqualizeHist(GMat src)
        {
            return new GMat(
                cveGapiEqualizeHist(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiEqualizeHist(IntPtr src);

        public static GMat BGR2RGB(GMat src)
        {
            return new GMat(
                cveGapiBGR2RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBGR2RGB(IntPtr src);

        public static GMat RGB2Gray(GMat src)
        {
            return new GMat(
                cveGapiRGB2Gray1(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRGB2Gray1(IntPtr src);

        public static GMat RGB2Gray(GMat src, float rY, float gY, float bY)
        {
            return new GMat(
                cveGapiRGB2Gray2(src, rY, gY, bY),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRGB2Gray2(IntPtr src, float rY, float gY, float bY);

        public static GMat BGR2Gray(GMat src)
        {
            return new GMat(
                cveGapiBGR2Gray(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBGR2Gray(IntPtr src);


        public static GMat RGB2YUV(GMat src)
        {
            return new GMat(
                cveGapiRGB2YUV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRGB2YUV(IntPtr src);

        public static GMat BGR2I420(GMat src)
        {
            return new GMat(
                cveGapiBGR2I420(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBGR2I420(IntPtr src);


        public static GMat RGB2I420(GMat src)
        {
            return new GMat(
                cveGapiRGB2I420(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRGB2I420(IntPtr src);

        public static GMat I4202BGR(GMat src)
        {
            return new GMat(
                cveGapiI4202BGR(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiI4202BGR(IntPtr src);

        public static GMat I4202RGB(GMat src)
        {
            return new GMat(
                cveGapiI4202RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiI4202RGB(IntPtr src);

        public static GMat BGR2LUV(GMat src)
        {
            return new GMat(
                cveGapiBGR2LUV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBGR2LUV(IntPtr src);

        public static GMat LUV2BGR(GMat src)
        {
            return new GMat(
                cveGapiLUV2BGR(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiLUV2BGR(IntPtr src);

        public static GMat YUV2BGR(GMat src)
        {
            return new GMat(
                cveGapiYUV2BGR(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiYUV2BGR(IntPtr src);

        public static GMat BGR2YUV(GMat src)
        {
            return new GMat(
                cveGapiBGR2YUV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBGR2YUV(IntPtr src);

        public static GMat RGB2Lab(GMat src)
        {
            return new GMat(
                cveGapiRGB2Lab(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRGB2Lab(IntPtr src);

        public static GMat YUV2RGB(GMat src)
        {
            return new GMat(
                cveGapiYUV2RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiYUV2RGB(IntPtr src);

        public static GMat NV12toRGB(GMat srcY, GMat srcUV)
        {
            return new GMat(
                cveGapiNV12toRGB(srcY, srcUV),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiNV12toRGB(IntPtr srcY, IntPtr srcUV);

        public static GMat NV12toGray(GMat srcY, GMat srcUV)
        {
            return new GMat(
                cveGapiNV12toGray(srcY, srcUV),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiNV12toGray(IntPtr srcY, IntPtr srcUV);

        public static GMat NV12toBGR(GMat srcY, GMat srcUV)
        {
            return new GMat(
                cveGapiNV12toBGR(srcY, srcUV),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiNV12toBGR(IntPtr srcY, IntPtr srcUV);

        public static GMat BayerGR2RGB(GMat src)
        {
            return new GMat(
                cveGapiBayerGR2RGB(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiBayerGR2RGB(IntPtr srcGR);

        public static GMat RGB2HSV(GMat src)
        {
            return new GMat(
                cveGapiRGB2HSV(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRGB2HSV(IntPtr src);

        public static GMat RGB2YUV422(GMat src)
        {
            return new GMat(
                cveGapiRGB2YUV422(src),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRGB2YUV422(IntPtr src);

        public static GMat Select(GMat src1, GMat src2, GMat mask)
        {
            return new GMat(
                cveGapiSelect(src1, src2, mask),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiSelect(IntPtr src1, IntPtr src2, IntPtr mask);

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
        private extern static IntPtr cveGapiMin(IntPtr src1, IntPtr src2);

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
        private extern static IntPtr cveGapiMax(IntPtr src1, IntPtr src2);

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
        private extern static IntPtr cveGapiAbsDiff(IntPtr src1, IntPtr src2);

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
        private extern static IntPtr cveGapiAbsDiffC(IntPtr src, IntPtr c);

        public static GScalar Sum(GMat src)
        {
            return new GScalar(cveGapiSum(src));
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiSum(IntPtr src);

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
        private extern static IntPtr cveGapiAddWeighted(
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
        private extern static IntPtr cveGapiNormL1(IntPtr src);

        public static GScalar NormL2(GMat src)
        {
            return new GScalar(cveGapiNormL2(src));
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiNormL2(IntPtr src);

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
        private extern static IntPtr cveGapiNormInf(IntPtr src);

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
        private extern static void cveGapiIntegral(IntPtr src, CvEnum.DepthType sdepth, CvEnum.DepthType sqdepth, IntPtr dst1, IntPtr dst2);

        public static GMat Threshold(
            GMat src,
            GScalar thresh,
            GScalar maxval,
            int type)
        {
            return new GMat(
                cveGapiThreshold(src, thresh, maxval, type),
                true
            );
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiThreshold(IntPtr src, IntPtr thresh, IntPtr maxval, int type);

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
        private extern static IntPtr cveGapiInRange(IntPtr src, IntPtr threshLow, IntPtr threshUp);


        public static GMat Merge4(
            GMat src1, GMat src2, GMat src3, GMat src4)
        {
            return new GMat(
                cveGapiMerge4(src1, src2, src3, src4),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiMerge4(IntPtr src1, IntPtr src2, IntPtr src3, IntPtr src4);

        public static GMat Merge3(
            GMat src1, GMat src2, GMat src3)
        {
            return new GMat(
                cveGapiMerge3(src1, src2, src3),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiMerge3(IntPtr src1, IntPtr src2, IntPtr src3);

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
        private extern static void cveGapiSplit4(IntPtr src, IntPtr dst1, IntPtr dst2, IntPtr dst3, IntPtr dst4);

        public static Tuple<GMat, GMat, GMat> Split3(GMat src)
        {
            GMat dst1 = new GMat();
            GMat dst2 = new GMat();
            GMat dst3 = new GMat();

            cveGapiSplit3(src, dst1, dst2, dst3);
            return new Tuple<GMat, GMat, GMat>(dst1, dst2, dst3);
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static void cveGapiSplit3(IntPtr src, IntPtr dst1, IntPtr dst2, IntPtr dst3);

        public static GMat Remap(
            GMat src,
            Mat map1,
            Mat map2,
            int interpolation,
            int borderMode,
            MCvScalar borderValue)
        {
            return new GMat(
                cveGapiRemap(src, map1, map2, interpolation, borderMode, ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiRemap(
            IntPtr src,
            IntPtr map1,
            IntPtr map2,
            int interpolation,
            int borderMode,
            ref MCvScalar borderValue);

        public static GMat Flip(
            GMat src,
            int flipCode)
        {
            return new GMat(
                cveGapiFlip(src, flipCode),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiFlip(IntPtr src, int flipCode);

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
        private extern static IntPtr cveGapiCrop(IntPtr src, ref Rectangle rect);

        public static GMat ConcatHor(GMat src1, GMat src2)
        {
            return new GMat(
                cveGapiConcatHor(src1, src2),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiConcatHor(IntPtr src1, IntPtr src2);

        public static GMat ConcatHor(VectorOfGMat v)
        {
            return new GMat(
                cveGapiConcatHorV(v),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiConcatHorV(IntPtr v);

        public static GMat ConcatVert(GMat src1, GMat src2)
        {
            return new GMat(
                cveGapiConcatVert(src1, src2),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiConcatVert(IntPtr src1, IntPtr src2);

        public static GMat ConcatVert(VectorOfGMat v)
        {
            return new GMat(
                cveGapiConcatVertV(v),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiConcatVertV(IntPtr v);

        public static GMat LUT(GMat src, Mat lut)
        {
            return new GMat(
                cveGapiLUT(src, lut),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiLUT(IntPtr src, IntPtr lut);

        public static GMat ConvertTo(
            GMat src,
            int rdepth,
            double alpha,
            double beta)
        {
            return new GMat(
                cveGapiConvertTo(src, rdepth, alpha, beta),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiConvertTo(
            IntPtr src,
            int rdepth,
            double alpha,
            double beta);

        public static GMat Normalize(
            GMat src,
            double alpha,
            double beta,
            int normType,
            int ddepth)
        {
            return new GMat(
                cveGapiNormalize(src, alpha, beta, normType, ddepth),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiNormalize(
            IntPtr src,
            double alpha,
            double beta,
            int normType,
            int ddepth);

        public static GMat WarpPerspective(
            GMat src,
            Mat M,
            Size dsize,
            int flags,
            int borderMode,
            MCvScalar borderValue)
        {
            return new GMat(
                cveGapiWarpPerspective(
                    src,
                    M,
                    ref dsize,
                    flags,
                    borderMode,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiWarpPerspective(
            IntPtr src,
            IntPtr M,
            ref Size dsize,
            int flags,
            int borderMode,
            ref MCvScalar borderValue);

        public static GMat WarpAffine(
            GMat src,
            Mat M,
            Size dsize,
            int flags,
            int borderMode,
            MCvScalar borderValue)
        {
            return new GMat(
                cveGapiWarpAffine(
                    src,
                    M,
                    ref dsize,
                    flags,
                    borderMode,
                    ref borderValue),
                true
            );
        }
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private extern static IntPtr cveGapiWarpAffine(
            IntPtr src,
            IntPtr M,
            ref Size dsize,
            int flags,
            int borderMode,
            ref MCvScalar borderValue);
    }
}

