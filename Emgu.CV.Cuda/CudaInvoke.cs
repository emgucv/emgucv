//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.IO;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// This class wraps the functional calls to the opencv_gpu module
    /// </summary>
    public static partial class CudaInvoke
    {
        static CudaInvoke()
        {
            //Dummy code to make sure the static constructor of CvInvoke has been called and the error handler has been registered.
            CvInvoke.Init();
        }

        #region device info
        #region HasCuda
        private static bool _testedCuda = false;
        private static bool _hasCuda = false;
        /// <summary>
        /// Return true if Cuda is found on the system
        /// </summary>
        public static bool HasCuda
        {
            get
            {
#if IOS
            return _hasCuda;
#else
                if (_testedCuda)
                    return _hasCuda;
                else
                {
                    _testedCuda = true;
                    try
                    {
                        _hasCuda = GetCudaEnabledDeviceCount() > 0;
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(String.Format("unable to retrieve cuda device count: {0}", e.Message));
                    }

                    return _hasCuda;
                }
#endif
            }
        }

        #endregion

        /// <summary>
        /// Get the opencl platform summary as a string
        /// </summary>
        /// <returns>An opencl platfor summary</returns>
        public static String GetCudaDevicesSummary()
        {

            StringBuilder builder = new StringBuilder();
            if (HasCuda)
            {
                builder.Append(String.Format("Has cuda: true{0}", Environment.NewLine));

                int deviceCount = GetCudaEnabledDeviceCount();
                builder.Append(String.Format("Cuda devices: {0}{1}", deviceCount, Environment.NewLine));

                for (int i = 0; i < deviceCount; i++)
                {
                    using (CudaDeviceInfo deviceInfo = new CudaDeviceInfo(i))
                    {
                        builder.Append(String.Format("  Device {0}: {1}{2}", i, deviceInfo.Name, Environment.NewLine));
                    }
                }

                return builder.ToString();
            }
            else
            {
                return "Has cuda: false";
            }
        }

        /// <summary>
        /// Get the number of Cuda enabled devices
        /// </summary>
        /// <returns>The number of Cuda enabled devices</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaGetCudaEnabledDeviceCount")]
        public static extern int GetCudaEnabledDeviceCount();

        /// <summary>
        /// Set the current Gpu Device
        /// </summary>
        /// <param name="deviceId">The id of the device to be setted as current</param>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaSetDevice")]
        public static extern void SetDevice(int deviceId);

        /// <summary>
        /// Get the current Cuda device id
        /// </summary>
        /// <returns>The current Cuda device id</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaGetDevice")]
        public static extern int GetDevice();
        #endregion

        /// <summary>
        /// Create a GpuMat from the specific region of <paramref name="gpuMat"/>. The data is shared between the two GpuMat.
        /// </summary>
        /// <param name="gpuMat">The gpuMat to extract regions from.</param>
        /// <param name="colRange">The column range. Use MCvSlice.WholeSeq for all columns.</param>
        /// <param name="rowRange">The row range. Use MCvSlice.WholeSeq for all rows.</param>
        /// <returns>Pointer to the GpuMat</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetRegion")]
        public static extern IntPtr GetRegion(IntPtr gpuMat, ref Emgu.CV.Structure.Range rowRange, ref Emgu.CV.Structure.Range colRange);

        /*
        /// <summary>
        /// Resize the GpuMat
        /// </summary>
        /// <param name="src">The input GpuMat</param>
        /// <param name="dst">The resulting GpuMat</param>
        /// <param name="interpolation">The interpolation type</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>     
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatResize")]
        public static extern void GpuMatResize(IntPtr src, IntPtr dst, CvEnum.Inter interpolation, IntPtr stream);
        */

        /// <summary>
        /// gpuMatReshape the src GpuMat  
        /// </summary>
        /// <param name="src">The source GpuMat</param>
        /// <param name="dst">The resulting GpuMat, as input it should be an empty GpuMat.</param>
        /// <param name="cn">The new number of channels</param>
        /// <param name="rows">The new number of rows</param>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatReshape")]
        public static extern void GpuMatReshape(IntPtr src, IntPtr dst, int cn, int rows);

        /// <summary>
        /// Returns header, corresponding to a specified rectangle of the input GpuMat. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array.
        /// </summary>
        /// <param name="mat">Input GpuMat</param>
        /// <param name="rect">Zero-based coordinates of the rectangle of interest.</param>
        /// <returns>Pointer to the resultant sub-array header.</returns>
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetSubRect")]
        public static extern IntPtr GetSubRect(IntPtr mat, ref Rectangle rect);

        #region arithmatic

        /// <summary>
        /// Shifts a matrix to the left (c = a &lt;&lt; scalar)
        /// </summary>
        /// <param name="a">The matrix to be shifted.</param>
        /// <param name="scalar">The scalar to shift by.</param>
        /// <param name="c">The result of the shift</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void LShift(IInputArray a, MCvScalar scalar, IOutputArray c, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
                cudaLShift(iaA, ref scalar, oaC, stream);
        }

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaLShift(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr stream);

        /// <summary>
        /// Shifts a matrix to the right (c = a >> scalar)
        /// </summary>
        /// <param name="a">The matrix to be shifted.</param>
        /// <param name="scalar">The scalar to shift by.</param>
        /// <param name="c">The result of the shift</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void RShift(IInputArray a, MCvScalar scalar, IOutputArray c, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
                cudaRShift(iaA, ref scalar, oaC, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaRShift(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr stream);

        /// <summary>
        /// Adds one matrix to another (c = a + b).
        /// </summary>
        /// <param name="a">The first matrix to be added.</param>
        /// <param name="b">The second matrix to be added.</param>
        /// <param name="c">The sum of the two matrix</param>
        /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="depthType">Optional depth of the output array.</param>
        public static void Add(IInputArray a, IInputArray b, IOutputArray c, IInputArray mask = null, DepthType depthType = DepthType.Default, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (InputArray iaB = b.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaAdd(iaA, iaB, oaC, iaMask, depthType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaAdd(IntPtr a, IntPtr b, IntPtr c, IntPtr mask, DepthType depthType, IntPtr stream);

        /// <summary>
        /// Subtracts one matrix from another (c = a - b).
        /// </summary>
        /// <param name="a">The matrix where subtraction take place</param>
        /// <param name="b">The matrix to be substracted</param>
        /// <param name="c">The result of a - b</param>
        /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="depthType">Optional depth of the output array.</param>
        public static void Subtract(IInputArray a, IInputArray b, IOutputArray c, IInputArray mask = null, DepthType depthType = DepthType.Default, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (InputArray iaB = b.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaSubtract(iaA, iaB, oaC, iaMask, depthType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaSubtract(IntPtr a, IntPtr b, IntPtr c, IntPtr mask, DepthType depthType, IntPtr stream);

        /// <summary>
        /// Computes element-wise product of the two GpuMat: c = scale * a * b.
        /// </summary>
        /// <param name="a">The first GpuMat to be element-wise multiplied.</param>
        /// <param name="b">The second GpuMat to be element-wise multiplied.</param>
        /// <param name="c">The element-wise multiplication of the two GpuMat</param>
        /// <param name="scale">The scale</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="depthType">Optional depth of the output array.</param>
        public static void Multiply(IInputArray a, IInputArray b, IOutputArray c, double scale = 1.0, DepthType depthType = DepthType.Default, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (InputArray iaB = b.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
                cudaMultiply(iaA, iaB, oaC, scale, depthType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMultiply(IntPtr a, IntPtr b, IntPtr c, double scale, DepthType depthType, IntPtr stream);

        /// <summary>
        /// Computes element-wise quotient of the two GpuMat (c = scale *  a / b).
        /// </summary>
        /// <param name="a">The first GpuMat</param>
        /// <param name="b">The second GpuMat</param>
        /// <param name="c">The element-wise quotient of the two GpuMat</param>
        /// <param name="scale">The scale</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="depthType">Optional depth of the output array.</param>
        public static void Divide(IInputArray a, IInputArray b, IOutputArray c, double scale = 1.0, DepthType depthType = DepthType.Default, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (InputArray iaB = b.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
                cudaDivide(iaA, iaB, oaC, scale, depthType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaDivide(IntPtr a, IntPtr b, IntPtr c, double scale, DepthType depthType, IntPtr stream);

        /// <summary>
        /// Computes the weighted sum of two arrays (dst = alpha*src1 + beta*src2 + gamma)
        /// </summary>
        /// <param name="src1">The first source GpuMat</param>
        /// <param name="alpha">The weight for <paramref name="src1"/></param>
        /// <param name="src2">The second source GpuMat</param>
        /// <param name="beta">The weight for <paramref name="src2"/></param>
        /// <param name="gamma">The constant to be added</param>
        /// <param name="dst">The result</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="depthType">Optional depth of the output array.</param>
        public static void AddWeighted(IInputArray src1, double alpha, IInputArray src2, double beta, double gamma, IOutputArray dst, DepthType depthType = DepthType.Default, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaAddWeighted(iaSrc1, alpha, iaSrc2, beta, gamma, oaDst, depthType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaAddWeighted(IntPtr src1, double alpha, IntPtr src2, double beta, double gamma, IntPtr dst, DepthType depthType, IntPtr stream);

        /// <summary>
        /// Computes element-wise absolute difference of two GpuMats (c = abs(a - b)).
        /// </summary>
        /// <param name="a">The first GpuMat</param>
        /// <param name="b">The second GpuMat</param>
        /// <param name="c">The result of the element-wise absolute difference.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Absdiff(IInputArray a, IInputArray b, IOutputArray c, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (InputArray iaB = b.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
                cudaAbsdiff(iaA, iaB, oaC, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaAbsdiff(IntPtr a, IntPtr b, IntPtr c, IntPtr stream);

        /// <summary>
        /// Computes absolute value of each pixel in an image
        /// </summary>
        /// <param name="src">The source GpuMat, support depth of Int16 and float.</param>
        /// <param name="dst">The resulting GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Abs(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaAbs(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaAbs(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes square of each pixel in an image
        /// </summary>
        /// <param name="src">The source GpuMat, support depth of byte, UInt16, Int16 and float.</param>
        /// <param name="dst">The resulting GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Sqr(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaSqr(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaSqr(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes square root of each pixel in an image
        /// </summary>
        /// <param name="src">The source GpuMat, support depth of byte, UInt16, Int16 and float.</param>
        /// <param name="dst">The resulting GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Sqrt(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaSqrt(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaSqrt(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Transposes a matrix.
        /// </summary>
        /// <param name="src">Source matrix. 1-, 4-, 8-byte element sizes are supported for now.</param>
        /// <param name="dst">Destination matrix.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Transpose(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaTranspose(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaTranspose(IntPtr src, IntPtr dst, IntPtr stream);
        #endregion

        /// <summary>
        /// Compares elements of two GpuMats (c = a &lt;cmpop&gt; b).
        /// Supports CV_8UC4, CV_32FC1 types
        /// </summary>
        /// <param name="a">The first GpuMat</param>
        /// <param name="b">The second GpuMat</param>
        /// <param name="c">The result of the comparison.</param>
        /// <param name="cmpop">The type of comparison</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Compare(IInputArray a, IInputArray b, IOutputArray c, CvEnum.CmpType cmpop, Stream stream = null)
        {
            using (InputArray iaA = a.GetInputArray())
            using (InputArray iaB = b.GetInputArray())
            using (OutputArray oaC = c.GetOutputArray())
                cudaCompare(iaA, iaB, oaC, cmpop, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCompare(IntPtr a, IntPtr b, IntPtr c, CvEnum.CmpType cmpop, IntPtr stream);

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="src">The source image. Has to be GpuMat&lt;Byte&gt;. If stream is used, the GpuMat has to be either single channel or 4 channels.</param>
        /// <param name="dst">The destination image.</param>
        /// <param name="interpolation">The interpolation type. Supports INTER_NEAREST, INTER_LINEAR.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="fx">Scale factor along the horizontal axis. If it is zero, it is computed as: (double)dsize.width/src.cols</param>
        /// <param name="fy">Scale factor along the vertical axis. If it is zero, it is computed as: (double)dsize.height/src.rows</param>
        /// <param name="dsize">Destination image size. If it is zero, it is computed as: dsize = Size(round(fx* src.cols), round(fy* src.rows)). Either dsize or both fx and fy must be non-zero.</param>
        public static void Resize(IInputArray src, IOutputArray dst, Size dsize, double fx = 0, double fy = 0, CvEnum.Inter interpolation = Inter.Linear, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaResize(iaSrc, oaDst, ref dsize, fx, fy, interpolation, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaResize(IntPtr src, IntPtr dst, ref Size dsize, double fx, double fy, CvEnum.Inter interpolation, IntPtr stream);

        /// <summary>
        /// Copies each plane of a multi-channel GpuMat to a dedicated GpuMat
        /// </summary>
        /// <param name="src">The multi-channel gpuMat</param>
        /// <param name="dstArray">Pointer to an array of single channel GpuMat pointers</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
        public static void Split(IInputArray src, VectorOfGpuMat dstArray, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
                cudaSplit(iaSrc, dstArray, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaSplit(IntPtr src, IntPtr dstArray, IntPtr stream);

        /// <summary>
        /// Makes multi-channel GpuMat out of several single-channel GpuMats
        /// </summary>
        /// <param name="srcArr">Pointer to an array of single channel GpuMat pointers</param>
        /// <param name="dst">The multi-channel gpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
        public static void Merge(VectorOfGpuMat srcArr, IOutputArray dst, Stream stream = null)
        {
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaMerge(srcArr, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMerge(IntPtr srcArr, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes exponent of each matrix element (b = exp(a))
        /// </summary>
        /// <param name="src">The source GpuMat. Supports Byte, UInt16, Int16 and float type.</param>
        /// <param name="dst">The resulting GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Exp(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaExp(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaExp(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes power of each matrix element:
        ///   (dst(i,j) = pow(     src(i,j) , power), if src.type() is integer;
        ///   (dst(i,j) = pow(fabs(src(i,j)), power), otherwise.
        /// supports all, except depth == CV_64F
        /// </summary>
        /// <param name="src">The source GpuMat</param>
        /// <param name="power">The power</param>
        /// <param name="dst">The resulting GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Pow(IInputArray src, double power, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaPow(iaSrc, power, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaPow(IntPtr src, double power, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes natural logarithm of absolute value of each matrix element: b = log(abs(a))
        /// </summary>
        /// <param name="src">The source GpuMat. Supports Byte, UInt16, Int16 and float type.</param>
        /// <param name="dst">The resulting GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Log(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaLog(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaLog(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes magnitude of each (x(i), y(i)) vector
        /// </summary>
        /// <param name="x">The source GpuMat. Supports only floating-point type</param>
        /// <param name="y">The source GpuMat. Supports only floating-point type</param>
        /// <param name="magnitude">The destination GpuMat. Supports only floating-point type</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Magnitude(IInputArray x, IInputArray y, IOutputArray magnitude, Stream stream = null)
        {
            using (InputArray iaX = x.GetInputArray())
            using (InputArray iaY = y.GetInputArray())
            using (OutputArray oaMagnitude = magnitude.GetOutputArray())
                cudaMagnitude(iaX, iaY, oaMagnitude, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMagnitude(IntPtr x, IntPtr y, IntPtr magnitude, IntPtr stream);

        /// <summary>
        /// Computes squared magnitude of each (x(i), y(i)) vector
        /// </summary>
        /// <param name="x">The source GpuMat. Supports only floating-point type</param>
        /// <param name="y">The source GpuMat. Supports only floating-point type</param>
        /// <param name="magnitude">The destination GpuMat. Supports only floating-point type</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void MagnitudeSqr(IInputArray x, IInputArray y, IOutputArray magnitude, Stream stream = null)
        {
            using (InputArray iaX = x.GetInputArray())
            using (InputArray iaY = y.GetInputArray())
            using (OutputArray oaMagnitude = magnitude.GetOutputArray())
                cudaMagnitudeSqr(iaX, iaY, oaMagnitude, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMagnitudeSqr(IntPtr x, IntPtr y, IntPtr magnitude, IntPtr stream);

        /// <summary>
        /// Computes angle (angle(i)) of each (x(i), y(i)) vector
        /// </summary>
        /// <param name="x">The source GpuMat. Supports only floating-point type</param>
        /// <param name="y">The source GpuMat. Supports only floating-point type</param>
        /// <param name="angle">The destination GpuMat. Supports only floating-point type</param>
        /// <param name="angleInDegrees">If true, the output angle is in degrees, otherwise in radian</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Phase(IInputArray x, IInputArray y, IOutputArray angle, bool angleInDegrees = false, Stream stream = null)
        {
            using (InputArray iaX = x.GetInputArray())
            using (InputArray iaY = y.GetInputArray())
            using (OutputArray oaAngle = angle.GetOutputArray())
                cudaPhase(iaX, iaY, oaAngle, angleInDegrees, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaPhase(
           IntPtr x, IntPtr y, IntPtr angle,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool angleInDegrees, IntPtr stream);

        /// <summary>
        /// Converts Cartesian coordinates to polar
        /// </summary>
        /// <param name="x">The source GpuMat. Supports only floating-point type</param>
        /// <param name="y">The source GpuMat. Supports only floating-point type</param>
        /// <param name="magnitude">The destination GpuMat. Supports only floating-point type</param>
        /// <param name="angle">The destination GpuMat. Supports only floating-point type</param>
        /// <param name="angleInDegrees">If true, the output angle is in degrees, otherwise in radian</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void CartToPolar(IInputArray x, IInputArray y, IOutputArray magnitude, IOutputArray angle, bool angleInDegrees = false, Stream stream = null)
        {
            using (InputArray iaX = x.GetInputArray())
            using (InputArray iaY = y.GetInputArray())
            using (OutputArray oaMagnitude = magnitude.GetOutputArray())
            using (OutputArray oaAngle = angle.GetOutputArray())
                cudaCartToPolar(iaX, iaY, oaMagnitude, oaAngle, angleInDegrees, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCartToPolar(
           IntPtr x, IntPtr y, IntPtr magnitude, IntPtr angle,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool angleInDegrees, IntPtr stream);

        /// <summary>
        /// Converts polar coordinates to Cartesian
        /// </summary>
        /// <param name="magnitude">The source GpuMat. Supports only floating-point type</param>
        /// <param name="angle">The source GpuMat. Supports only floating-point type</param>
        /// <param name="x">The destination GpuMat. Supports only floating-point type</param>
        /// <param name="y">The destination GpuMat. Supports only floating-point type</param>
        /// <param name="angleInDegrees">If true, the input angle is in degrees, otherwise in radian</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void PolarToCart(IInputArray magnitude, IInputArray angle, IOutputArray x, IOutputArray y, bool angleInDegrees = false, Stream stream = null)
        {
            using (InputArray iaMagnitude = magnitude.GetInputArray())
            using (InputArray iaAngle = angle.GetInputArray())
            using (OutputArray oaX = x.GetOutputArray())
            using (OutputArray oaY = y.GetOutputArray())
                cudaPolarToCart(iaMagnitude, iaAngle, oaX, oaY, angleInDegrees, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaPolarToCart(
           IntPtr magnitude, IntPtr angle, IntPtr x, IntPtr y,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool angleInDegrees, IntPtr stream);

        /// <summary>
        /// Finds minimum and maximum element values and their positions. The extremums are searched over the whole GpuMat or, if mask is not IntPtr.Zero, in the specified GpuMat region.
        /// </summary>
        /// <param name="gpuMat">The source GpuMat, single-channel</param>
        /// <param name="minVal">Pointer to returned minimum value</param>
        /// <param name="maxVal">Pointer to returned maximum value</param>
        /// <param name="minLoc">Pointer to returned minimum location</param>
        /// <param name="maxLoc">Pointer to returned maximum location</param>
        /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
        public static void MinMaxLoc(IInputArray gpuMat, ref double minVal, ref double maxVal, ref Point minLoc, ref Point maxLoc, IInputArray mask = null)
        {
            using (InputArray iaMat = gpuMat.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaMinMaxLoc(iaMat, ref minVal, ref maxVal, ref minLoc, ref maxLoc, iaMask);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMinMaxLoc(IntPtr gpuMat, ref double minVal, ref double maxVal, ref Point minLoc, ref Point maxLoc, IntPtr mask);

        /// <summary>
        /// Finds global minimum and maximum matrix elements and returns their values with locations.
        /// </summary>
        /// <param name="src">Single-channel source image.</param>
        /// <param name="minMaxVals">The output min and max values</param>
        /// <param name="loc">The ouput min and max locations</param>
        /// <param name="mask">Optional mask to select a sub-matrix.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void FindMinMaxLoc(IInputArray src, IOutputArray minMaxVals, IOutputArray loc,
           IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaMinMaxVals = minMaxVals.GetOutputArray())
            using (OutputArray oaLoc = loc.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                cudaFindMinMaxLoc(iaSrc, oaMinMaxVals, oaLoc, iaMask, stream);
            }
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaFindMinMaxLoc(IntPtr src, IntPtr minMaxVals, IntPtr loc, IntPtr mask, IntPtr stream);


        /// <summary>
        /// Performs downsampling step of Gaussian pyramid decomposition. 
        /// </summary>
        /// <param name="src">The source CudaImage.</param>
        /// <param name="dst">The destination CudaImage, should have 2x smaller width and height than the source.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void PyrDown(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaPyrDown(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaPyrDown(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Performs up-sampling step of Gaussian pyramid decomposition.
        /// </summary>
        /// <param name="src">The source CudaImage.</param>
        /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void PyrUp(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaPyrUp(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaPyrUp(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes mean value and standard deviation
        /// </summary>
        /// <param name="mtx">The GpuMat. Supports only CV_8UC1 type</param>
        /// <param name="mean">The mean value</param>
        /// <param name="stddev">The standard deviation</param>
        public static void MeanStdDev(IInputArray mtx, ref MCvScalar mean, ref MCvScalar stddev)
        {
            using (InputArray iaMtx = mtx.GetInputArray())
                cudaMeanStdDev(iaMtx, ref mean, ref stddev);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMeanStdDev(IntPtr mtx, ref MCvScalar mean, ref MCvScalar stddev);

        /// <summary>
        /// Computes norm of the difference between two GpuMats
        /// </summary>
        /// <param name="src1">The GpuMat. Supports only CV_8UC1 type</param>
        /// <param name="src2">If IntPtr.Zero, norm operation is apply to <paramref name="src1"/> only. Otherwise, this is the GpuMat of type CV_8UC1</param>
        /// <param name="normType">The norm type. Supports NORM_INF, NORM_L1, NORM_L2.</param>
        /// <returns>The norm of the <paramref name="src1"/> if <paramref name="src2"/> is IntPtr.Zero. Otherwise the norm of the difference between two GpuMats.</returns>
        public static double Norm(IInputArray src1, IInputArray src2, Emgu.CV.CvEnum.NormType normType = NormType.L2)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2 == null ? InputArray.GetEmpty() : src2.GetInputArray())
                return cudaNorm2(iaSrc1, iaSrc2, normType);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cudaNorm2(IntPtr src1, IntPtr src2, Emgu.CV.CvEnum.NormType normType);

        /// <summary>
        /// Returns the norm of a matrix.
        /// </summary>
        /// <param name="src">Source matrix. Any matrices except 64F are supported.</param>
        /// <param name="normType">Norm type. NORM_L1 , NORM_L2 , and NORM_INF are supported for now.</param>
        /// <param name="mask">optional operation mask; it must have the same size as src1 and CV_8UC1 type.</param>
        /// <returns>The norm of a matrix</returns>
        public static double Norm(IInputArray src, Emgu.CV.CvEnum.NormType normType, IInputArray mask = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                return cudaNorm1(iaSrc, normType, iaMask);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cudaNorm1(IntPtr src1, Emgu.CV.CvEnum.NormType normType, IntPtr mask);

        /// <summary>
        /// Returns the norm of a matrix.
        /// </summary>
        /// <param name="src">Source matrix. Any matrices except 64F are supported.</param>
        /// <param name="dst">The GpuMat to store the result</param>
        /// <param name="normType">Norm type. NORM_L1 , NORM_L2 , and NORM_INF are supported for now.</param>
        /// <param name="mask">optional operation mask; it must have the same size as src1 and CV_8UC1 type.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void CalcNorm(IInputArray src, IOutputArray dst, NormType normType = NormType.L2, IInputArray mask = null,
           Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaCalcNorm(iaSrc, oaDst, normType, iaMask, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCalcNorm(IntPtr src, IntPtr dst, NormType normType, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Returns the difference of two matrices.
        /// </summary>
        /// <param name="src1">Source matrix. Any matrices except 64F are supported.</param>
        /// <param name="src2">Second source matrix (if any) with the same size and type as src1.</param>
        /// <param name="dst">The GpuMat where the result will be stored in</param>
        /// <param name="normType">Norm type. NORM_L1 , NORM_L2 , and NORM_INF are supported for now.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void CalcNormDiff(IInputArray src1, IInputArray src2, IOutputArray dst, NormType normType = NormType.L2,
           Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaCalcNormDiff(iaSrc1, iaSrc2, oaDst, normType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCalcNormDiff(IntPtr src1, IntPtr src2, IntPtr dst, NormType normType, IntPtr stream);

        /// <summary>
        /// Returns the sum of absolute values for matrix elements.
        /// </summary>
        /// <param name="src">Source image of any depth except for CV_64F.</param>
        /// <param name="mask">optional operation mask; it must have the same size as src and CV_8UC1 type.</param>
        /// <returns>The sum of absolute values for matrix elements.</returns>
        public static MCvScalar AbsSum(IInputArray src, IInputArray mask = null)
        {
            MCvScalar result = new MCvScalar();
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaAbsSum(iaSrc, ref result, iaMask);
            return result;
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaAbsSum(IntPtr src, ref MCvScalar sum, IntPtr mask);

        /// <summary>
        /// Returns the sum of absolute values for matrix elements.
        /// </summary>
        /// <param name="src">Source image of any depth except for CV_64F.</param>
        /// <param name="dst">The GpuMat where the result will be stored.</param>
        /// <param name="mask">optional operation mask; it must have the same size as src1 and CV_8UC1 type.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void CalcAbsSum(IInputArray src, IOutputArray dst, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaCalcAbsSum(iaSrc, oaDst, iaMask, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCalcAbsSum(IntPtr src, IntPtr dst, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Returns the squared sum of matrix elements.
        /// </summary>
        /// <param name="src">Source image of any depth except for CV_64F.</param>
        /// <param name="mask">optional operation mask; it must have the same size as src1 and CV_8UC1 type.</param>
        /// <returns>The squared sum of matrix elements.</returns>
        public static MCvScalar SqrSum(IInputArray src, IInputArray mask = null)
        {
            MCvScalar result = new MCvScalar();
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaSqrSum(iaSrc, ref result, iaMask);
            return result;
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaSqrSum(IntPtr src, ref MCvScalar sqrSum, IntPtr mask);

        /// <summary>
        /// Returns the squared sum of matrix elements.
        /// </summary>
        /// <param name="src">Source image of any depth except for CV_64F.</param>
        /// <param name="dst">The GpuMat where the result will be stored</param>
        /// <param name="mask">optional operation mask; it must have the same size as src1 and CV_8UC1 type.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void CalcSqrSum(IInputArray src, IOutputArray dst, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaCalcSqrSum(iaSrc, oaDst, iaMask, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCalcSqrSum(IntPtr src, IntPtr dst, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Counts non-zero array elements
        /// </summary>
        /// <param name="src">Single-channel source image.</param>
        /// <returns>The number of non-zero GpuMat elements</returns>
        public static int CountNonZero(IInputArray src)
        {
            using (InputArray iaSrc = src.GetInputArray())
                return cudaCountNonZero1(iaSrc);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern int cudaCountNonZero1(IntPtr src);

        /// <summary>
        /// Counts non-zero array elements
        /// </summary>
        /// <param name="src">Single-channel source image.</param>
        /// <param name="dst">A Gpu mat to hold the result</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void CountNonZero(IInputArray src, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaCountNonZero2(iaSrc, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCountNonZero2(IntPtr src, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Normalizes the norm or value range of an array.
        /// </summary>
        /// <param name="src">Input array.</param>
        /// <param name="dst">Output array of the same size as src .</param>
        /// <param name="alpha">Norm value to normalize to or the lower range boundary in case of the range normalization.</param>
        /// <param name="beta">Upper range boundary in case of the range normalization; it is not used for the norm normalization.</param>
        /// <param name="normType">Normalization type ( NORM_MINMAX , NORM_L2 , NORM_L1 or NORM_INF ).</param>
        /// <param name="depthType">Optional depth of the output array.</param>
        /// <param name="mask">Optional operation mask.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void Normalize(IInputArray src, IOutputArray dst, double alpha, double beta, NormType normType,
           CvEnum.DepthType depthType, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                cudaNormalize(iaSrc, oaDst, alpha, beta, normType, depthType, iaMask, stream);
            }
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaNormalize(IntPtr src, IntPtr dst, double alpha, double beta,
           NormType normType, CvEnum.DepthType dtype, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Reduces GpuMat to a vector by treating the GpuMat rows/columns as a set of 1D vectors and performing the specified operation on the vectors until a single row/column is obtained. 
        /// </summary>
        /// <param name="mtx">The input GpuMat</param>
        /// <param name="vec">Destination vector. Its size and type is defined by dim and dtype parameters</param>
        /// <param name="dim">Dimension index along which the matrix is reduced. 0 means that the matrix is reduced to a single row. 1 means that the matrix is reduced to a single column.</param>
        /// <param name="reduceOp">The reduction operation type</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>      
        /// <param name="dType">Optional depth of the output array.</param>
        public static void Reduce(IInputArray mtx, IOutputArray vec, CvEnum.ReduceDimension dim, CvEnum.ReduceType reduceOp, DepthType dType = DepthType.Default, Stream stream = null)
        {
            using (InputArray iaMtx = mtx.GetInputArray())
            using (OutputArray oaVec = vec.GetOutputArray())
                cudaReduce(iaMtx, oaVec, dim, reduceOp, dType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaReduce(IntPtr mtx, IntPtr vec, CvEnum.ReduceDimension dim, CvEnum.ReduceType reduceOp, DepthType dType, IntPtr stream);
        
        /// <summary>
        /// Flips the GpuMat&lt;Byte&gt; in one of different 3 ways (row and column indices are 0-based). 
        /// </summary>
        /// <param name="src">The source GpuMat. supports 1, 3 and 4 channels GpuMat with Byte, UInt16, int or float depth</param>
        /// <param name="dst">Destination GpuMat. The same source and type as <paramref name="src"/></param>
        /// <param name="flipType">Specifies how to flip the GpuMat.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>      
        public static void Flip(IInputArray src, IOutputArray dst, CvEnum.FlipType flipType, Stream stream = null)
        {
            int flipMode =
               //-1 indicates vertical and horizontal flip
               flipType == (Emgu.CV.CvEnum.FlipType.Horizontal | Emgu.CV.CvEnum.FlipType.Vertical) ? -1 :
               //1 indicates horizontal flip only
               flipType == Emgu.CV.CvEnum.FlipType.Horizontal ? 1 :
               //0 indicates vertical flip only
               0;
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaFlip(iaSrc, oaDst, flipMode, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaFlip(IntPtr src, IntPtr dst, int flipMode, IntPtr stream);


        #region Logical operators
        /// <summary>
        /// Calculates per-element bit-wise logical conjunction of two GpuMats:
        /// dst(I)=src1(I)^src2(I) if mask(I)!=0
        /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
        /// </summary>
        /// <param name="src1">The first source GpuMat</param>
        /// <param name="src2">The second source GpuMat</param>
        /// <param name="dst">The destination GpuMat</param>
        /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void BitwiseXor(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaBitwiseXor(iaSrc1, iaSrc2, oaDst, iaMask, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaBitwiseXor(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Calculates per-element bit-wise logical or of two GpuMats:
        /// dst(I)=src1(I) | src2(I) if mask(I)!=0
        /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
        /// </summary>
        /// <param name="src1">The first source GpuMat</param>
        /// <param name="src2">The second source GpuMat</param>
        /// <param name="dst">The destination GpuMat</param>
        /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void BitwiseOr(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaBitwiseOr(iaSrc1, iaSrc2, oaDst, iaMask, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaBitwiseOr(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Calculates per-element bit-wise logical and of two GpuMats:
        /// dst(I)=src1(I) &amp; src2(I) if mask(I)!=0
        /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
        /// </summary>
        /// <param name="src1">The first source GpuMat</param>
        /// <param name="src2">The second source GpuMat</param>
        /// <param name="dst">The destination GpuMat</param>
        /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void BitwiseAnd(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaBitwiseAnd(iaSrc1, iaSrc2, oaDst, iaMask, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaBitwiseAnd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

        /// <summary>
        /// Calculates per-element bit-wise logical not
        /// dst(I)=~src(I) if mask(I)!=0
        /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
        /// </summary>
        /// <param name="src">The source GpuMat</param>
        /// <param name="dst">The destination GpuMat</param>
        /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void BitwiseNot(IInputArray src, IOutputArray dst, IInputArray mask = null, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
                cudaBitwiseNot(iaSrc, oaDst, iaMask, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaBitwiseNot(IntPtr src, IntPtr dst, IntPtr mask, IntPtr stream);
        #endregion

        /// <summary>
        /// Computes per-element minimum of two GpuMats (dst = min(src1, src2))
        /// </summary>
        /// <param name="src1">The first GpuMat</param>
        /// <param name="src2">The second GpuMat</param>
        /// <param name="dst">The result GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Min(IInputArray src1, IInputArray src2, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaMin(iaSrc1, iaSrc2, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMin(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Computes per-element maximum of two GpuMats (dst = max(src1, src2))
        /// </summary>
        /// <param name="src1">The first GpuMat</param>
        /// <param name="src2">The second GpuMat</param>
        /// <param name="dst">The result GpuMat</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Max(IInputArray src1, IInputArray src2, IOutputArray dst, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaMax(iaSrc1, iaSrc2, oaDst, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMax(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr stream);

        /// <summary>
        /// Applies fixed-level thresholding to single-channel array. The function is typically used to get bi-level (binary) image out of grayscale image or for removing a noise, i.e. filtering out pixels with too small or too large values. There are several types of thresholding the function supports that are determined by thresholdType
        /// </summary>
        /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
        /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
        /// <param name="threshold">Threshold value</param>
        /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
        /// <param name="thresholdType">Thresholding type</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <returns>The computed threshold value if Otsu's or Triangle methods used.</returns>
        public static double Threshold(IInputArray src, IOutputArray dst, double threshold, double maxValue, CvEnum.ThresholdType thresholdType, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                return cudaThreshold(iaSrc, oaDst, threshold, maxValue, thresholdType, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cudaThreshold(IntPtr src, IntPtr dst, double threshold, double maxValue, CvEnum.ThresholdType thresholdType, IntPtr stream);

        /// <summary>
        /// Performs generalized matrix multiplication:
        /// dst = alpha*op(src1)*op(src2) + beta*op(src3), where op(X) is X or XT
        /// </summary>
        /// <param name="src1">The first source array. </param>
        /// <param name="src2">The second source array. </param>
        /// <param name="alpha">The scalar</param>
        /// <param name="src3">The third source array (shift). Can be IntPtr.Zero, if there is no shift.</param>
        /// <param name="beta">The scalar</param>
        /// <param name="dst">The destination array.</param>
        /// <param name="tABC">The gemm operation type</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Gemm(
           IInputArray src1,
           IInputArray src2,
           double alpha,
           IInputArray src3,
           double beta,
           IOutputArray dst,
           CvEnum.GemmType tABC = GemmType.Default,
           Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (InputArray iaSrc3 = src3 == null ? InputArray.GetEmpty() : src3.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaGemm(iaSrc1, iaSrc2, alpha, iaSrc3, beta, oaDst, tABC, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaGemm(
           IntPtr src1,
           IntPtr src2,
           double alpha,
           IntPtr src3,
           double beta,
           IntPtr dst,
           CvEnum.GemmType tABC,
           IntPtr stream);

        /// <summary>
        /// Warps the image using affine transformation
        /// </summary>
        /// <param name="src">The source GpuMat</param>
        /// <param name="dst">The destination GpuMat</param>
        /// <param name="M">The 2x3 transformation matrix (pointer to CvArr)</param>
        /// <param name="flags">Supports NN, LINEAR, CUBIC</param>
        /// <param name="borderMode">The border mode, use BORDER_TYPE.CONSTANT for default.</param>
        /// <param name="borderValue">The border value, use new MCvScalar() for default.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="dSize">The size of the destination image</param>
        public static void WarpAffine(IInputArray src, IOutputArray dst, IInputArray M, Size dSize, CvEnum.Inter flags = Inter.Linear, CvEnum.BorderType borderMode = BorderType.Constant, MCvScalar borderValue = new MCvScalar(), Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaM = M.GetInputArray())
                cudaWarpAffine(iaSrc, oaDst, iaM, ref dSize, flags, borderMode, ref borderValue, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaWarpAffine(IntPtr src, IntPtr dst, IntPtr M, ref Size dSize, CvEnum.Inter flags, CvEnum.BorderType borderMode, ref MCvScalar borderValue, IntPtr stream);

        /// <summary>
        /// Warps the image using perspective transformation
        /// </summary>
        /// <param name="src">The source GpuMat</param>
        /// <param name="dst">The destination GpuMat</param>
        /// <param name="M">The 2x3 transformation matrix (pointer to CvArr)</param>
        /// <param name="flags">Supports NN, LINEAR, CUBIC</param>
        /// <param name="borderMode">The border mode, use BORDER_TYPE.CONSTANT for default.</param>
        /// <param name="borderValue">The border value, use new MCvScalar() for default.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        /// <param name="dSize">The size of the destination image</param>
        public static void WarpPerspective(IInputArray src, IOutputArray dst, IInputArray M, Size dSize, CvEnum.Inter flags = Inter.Linear, CvEnum.BorderType borderMode = BorderType.Constant, MCvScalar borderValue = new MCvScalar(), Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaM = M.GetInputArray())
                cudaWarpPerspective(iaSrc, oaDst, iaM, ref dSize, flags, borderMode, ref borderValue, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaWarpPerspective(IntPtr src, IntPtr dst, IntPtr M, ref Size dSize, CvEnum.Inter flags, CvEnum.BorderType borderMode, ref MCvScalar borderValue, IntPtr stream);

        /// <summary>
        /// DST[x,y] = SRC[xmap[x,y],ymap[x,y]] with bilinear interpolation.
        /// </summary>
        /// <param name="src">The source GpuMat. Supports CV_8UC1, CV_8UC3 source types. </param>
        /// <param name="dst">The dstination GpuMat. Supports CV_8UC1, CV_8UC3 source types. </param>
        /// <param name="xmap">The xmap. Supports CV_32FC1 map type.</param>
        /// <param name="ymap">The ymap. Supports CV_32FC1 map type.</param>
        /// <param name="interpolation">Interpolation type.</param>
        /// <param name="borderMode">Border mode. Use BORDER_CONSTANT for default.</param>
        /// <param name="borderValue">The value of the border.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Remap(IInputArray src, IOutputArray dst, IInputArray xmap, IInputArray ymap, CvEnum.Inter interpolation, CvEnum.BorderType borderMode = BorderType.Constant, MCvScalar borderValue = new MCvScalar(), Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            using (InputArray iaXmap = xmap.GetInputArray())
            using (InputArray iaYmap = ymap.GetInputArray())
                cudaRemap(iaSrc, oaDst, iaXmap, iaYmap, interpolation, borderMode, ref borderValue, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaRemap(IntPtr src, IntPtr dst, IntPtr xmap, IntPtr ymap, CvEnum.Inter interpolation, CvEnum.BorderType borderMode, ref MCvScalar borderValue, IntPtr stream);

        /// <summary>
        /// Rotates an image around the origin (0,0) and then shifts it.
        /// </summary>
        /// <param name="src">Source image. Supports 1, 3 or 4 channels images with Byte, UInt16 or float depth</param>
        /// <param name="dst">Destination image with the same type as src. Must be pre-allocated</param>
        /// <param name="angle">Angle of rotation in degrees</param>
        /// <param name="xShift">Shift along the horizontal axis</param>
        /// <param name="yShift">Shift along the verticle axis</param>
        /// <param name="dSize">The size of the destination image</param>
        /// <param name="interpolation">Interpolation method. Only INTER_NEAREST, INTER_LINEAR, and INTER_CUBIC are supported.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Rotate(IInputArray src, IOutputArray dst, Size dSize, double angle, double xShift = 0, double yShift = 0, CvEnum.Inter interpolation = Inter.Linear, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaRotate(iaSrc, oaDst, ref dSize, angle, xShift, yShift, interpolation, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaRotate(IntPtr src, IntPtr dst, ref Size dSize, double angle, double xShift, double yShift, CvEnum.Inter interpolation, IntPtr stream);

        /// <summary>
        /// Copies a 2D array to a larger destination array and pads borders with the given constant.
        /// </summary>
        /// <param name="src">Source image.</param>
        /// <param name="dst">Destination image with the same type as src. The size is Size(src.cols+left+right, src.rows+top+bottom).</param>
        /// <param name="top">Number of pixels in each direction from the source image rectangle to extrapolate.</param>
        /// <param name="bottom">Number of pixels in each direction from the source image rectangle to extrapolate.</param>
        /// <param name="left">Number of pixels in each direction from the source image rectangle to extrapolate.</param>
        /// <param name="right">Number of pixels in each direction from the source image rectangle to extrapolate.</param>
        /// <param name="borderType">Border Type</param>
        /// <param name="value">Border value.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void CopyMakeBorder(IInputArray src, IOutputArray dst, int top, int bottom, int left, int right, CvEnum.BorderType borderType, MCvScalar value = new MCvScalar(), Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaCopyMakeBorder(iaSrc, oaDst, top, bottom, left, right, borderType, ref value, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCopyMakeBorder(IntPtr src, IntPtr dst, int top, int bottom, int left, int right, CvEnum.BorderType borderType, ref MCvScalar value, IntPtr stream);

        /// <summary>
        /// Computes the integral image and integral for the squared image
        /// </summary>
        /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
        /// <param name="sum">The sum GpuMat, supports only CV_32S source type, but will contain unsigned int values</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void Integral(IInputArray src, IOutputArray sum, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaSum = sum.GetOutputArray())
                cudaIntegral(iaSrc, oaSum, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaIntegral(IntPtr src, IntPtr sum, IntPtr stream);

        /// <summary>
        /// Computes squared integral image 
        /// </summary>
        /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
        /// <param name="sqsum">The sqsum GpuMat, supports only CV32F source type.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
        public static void SqrIntegral(IInputArray src, IOutputArray sqsum, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaSqsum = sqsum.GetOutputArray())
                cudaSqrIntegral(iaSrc, oaSqsum, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaSqrIntegral(IntPtr src, IntPtr sqsum, IntPtr stream);

        /// <summary>
        /// Performs a forward or inverse discrete Fourier transform (1D or 2D) of floating point matrix.
        /// Param dft_size is the size of DFT transform.
        /// 
        /// If the source matrix is not continous, then additional copy will be done,
        /// so to avoid copying ensure the source matrix is continous one. If you want to use
        /// preallocated output ensure it is continuous too, otherwise it will be reallocated.
        ///
        /// Being implemented via CUFFT real-to-complex transform result contains only non-redundant values
        /// in CUFFT's format. Result as full complex matrix for such kind of transform cannot be retrieved.
        ///
        /// For complex-to-real transform it is assumed that the source matrix is packed in CUFFT's format.
        /// </summary>
        /// <param name="src">The source GpuMat</param>
        /// <param name="dst">The resulting GpuMat of the DST, must be pre-allocated and continious. If single channel, the result is real. If double channel, the result is complex</param>
        /// <param name="dftSize">Size of a discrete Fourier transform.</param>
        /// <param name="flags">DFT flags</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void Dft(IInputArray src, IOutputArray dst, Size dftSize, CvEnum.DxtType flags = DxtType.Forward, Stream stream = null)
        {
            using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaDft(iaSrc, oaDst, ref dftSize, flags, stream);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaDft(IntPtr src, IntPtr dst, ref Size dftSize, CvEnum.DxtType flags, IntPtr stream);

        /// <summary>
        /// Performs a per-element multiplication of two Fourier spectrums and scales the result.
        /// </summary>
        /// <param name="src1">First spectrum.</param>
        /// <param name="src2">Second spectrum with the same size and type.</param>
        /// <param name="dst">Destination spectrum.</param>
        /// <param name="flags">Mock parameter used for CPU/CUDA interfaces similarity, simply add a 0 value.</param>
        /// <param name="scale">Scale constant.</param>
        /// <param name="conjB">Optional flag to specify if the second spectrum needs to be conjugated before the multiplication.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void MulAndScaleSpectrums(IInputArray src1, IInputArray src2, IOutputArray dst, int flags, float scale, bool conjB = false, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaMulAndScaleSpectrums(iaSrc1, iaSrc2, oaDst, flags, scale, conjB, stream);
        }

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMulAndScaleSpectrums(
           IntPtr src1, IntPtr src2, IntPtr dst, int flags, float scale,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool conjB,
           IntPtr stream);

        /// <summary>
        /// Performs a per-element multiplication of two Fourier spectrums.
        /// </summary>
        /// <param name="src1">First spectrum.</param>
        /// <param name="src2">Second spectrum with the same size and type.</param>
        /// <param name="dst">Destination spectrum.</param>
        /// <param name="flags">Mock parameter used for CPU/CUDA interfaces similarity.</param>
        /// <param name="conjB">Optional flag to specify if the second spectrum needs to be conjugated before the multiplication.</param>
        /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>  
        public static void MulSpectrums(IInputArray src1, IInputArray src2, IOutputArray dst, int flags,
           bool conjB = false, Stream stream = null)
        {
            using (InputArray iaSrc1 = src1.GetInputArray())
            using (InputArray iaSrc2 = src2.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
                cudaMulSpectrums(iaSrc1, iaSrc2, oaDst, flags, conjB, stream);
        }

        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaMulSpectrums(IntPtr src1, IntPtr src2, IntPtr dst, int flags,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool conjB,
           IntPtr stream);

        /// <summary>
        /// Sets a CUDA device and initializes it for the current thread with OpenGL interoperability.
        /// This function should be explicitly called after OpenGL context creation and before any CUDA calls.
        /// </summary>
        /// <param name="device">System index of a CUDA device starting with 0.</param>
        public static void SetGlDevice(int device = 0)
        {
            cudaSetGlDevice(device);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaSetGlDevice(int device);

        /// <summary>
        /// Colors a disparity image.
        /// </summary>
        /// <param name="srcDisp">Input single-channel 8-bit unsigned, 16-bit signed, 32-bit signed or 32-bit floating-point disparity image. If 16-bit signed format is used, the values are assumed to have no fractional bits.</param>
        /// <param name="dstDisp">Output disparity image. It has the same size as src_disp. The type is CV_8UC4 in BGRA format (alpha = 255).</param>
        /// <param name="ndisp">Number of disparities.</param>
        /// <param name="stream">Stream for the asynchronous version.</param>
        public static void DrawColorDisp(IInputArray srcDisp, IOutputArray dstDisp, int ndisp, Stream stream = null)
        {
            using (InputArray iaSrcDisp = srcDisp.GetInputArray())
            using (OutputArray oaDstDisp = dstDisp.GetOutputArray())
            {
                cudaDrawColorDisp(iaSrcDisp, oaDstDisp, ndisp, stream);
            }

        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaDrawColorDisp(
            IntPtr srcDisp, 
            IntPtr dstDisp, 
            int ndisp, 
            IntPtr stream);

        /*
        /// <summary>
        /// Draw the optical flow needle map
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="vertex"></param>
        /// <param name="colors"></param>
        public static void CreateOpticalFlowNeedleMap(GpuMat u, GpuMat v, GpuMat vertex, GpuMat colors)
        {
           cudaCreateOpticalFlowNeedleMap(u, v, vertex, colors);
        }
        [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cudaCreateOpticalFlowNeedleMap(IntPtr u, IntPtr v, IntPtr vertex, IntPtr colors);
        */

    }
}
