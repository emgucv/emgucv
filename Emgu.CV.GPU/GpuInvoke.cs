//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// This class wraps the functional calls to the opencv_gpu module
   /// </summary>
   public static partial class GpuInvoke
   {
      static GpuInvoke()
      {
         //Dummy code to make sure the static constructor of CvInvoke has been called and the error handler has been registered.
         CvInvoke.CV_MAKETYPE(0, 0);
         
#if !IOS
         String[] modules = new String[] 
         {
            CvInvoke.EXTERN_GPU_LIBRARY
         };

         String formatString = CvInvoke.GetModuleFormatString();
         for (int i = 0; i < modules.Length; ++i)
         {
            modules[i] = String.Format(formatString, modules[i]);
         }

         CvInvoke.LoadUnmanagedModules(null, modules);
#endif
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
#if IOS || ANDROID
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
               } catch (Exception)
               {
               }

               return _hasCuda;
            }
#endif
         }
      }

      #endregion

      /// <summary>
      /// Get the number of Cuda enabled devices
      /// </summary>
      /// <returns>The number of Cuda enabled devices</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuGetCudaEnabledDeviceCount")]
      public static extern int GetCudaEnabledDeviceCount();

      /// <summary>
      /// Set the current Gpu Device
      /// </summary>
      /// <param name="deviceId">The id of the device to be setted as current</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuSetDevice")]
      public static extern void SetDevice(int deviceId);

      /// <summary>
      /// Get the current Cuda device id
      /// </summary>
      /// <returns>The current Cuda device id</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuGetDevice")]
      public static extern int GetDevice();
      #endregion

      /// <summary>
      /// Create an empty GpuMat 
      /// </summary>
      /// <returns>Pointer to an empty GpuMat</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreateDefault")]
      internal static extern IntPtr GpuMatCreateDefault();

      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="type">The type of GpuMat</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreate")]
      public static extern IntPtr GpuMatCreate(int rows, int cols, int type);

      /// <summary>
      /// Create a GpuMat of the specified size. The allocated data is continuous within this GpuMat.
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="type">The type of GpuMat</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreateContinuous")]
      internal static extern IntPtr GpuMatCreateContinuous(int rows, int cols, int type);

      /// <summary>
      /// Returns true iff the GpuMatrix data is continuous
      /// (i.e. when there are no gaps between successive rows).
      /// </summary>
      /// <param name="gpuMat">The GpuMat to be checked</param>
      /// <returns>True if the GpuMat is continuous</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatIsContinuous")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool GpuMatIsContinuous(IntPtr gpuMat);

      /// <summary>
      /// Create a GpuMat from the specific region of <paramref name="gpuMat"/>. The data is shared between the two GpuMat.
      /// </summary>
      /// <param name="gpuMat">The gpuMat to extract regions from.</param>
      /// <param name="colRange">The column range. Use MCvSlice.WholeSeq for all columns.</param>
      /// <param name="rowRange">The row range. Use MCvSlice.WholeSeq for all rows.</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetRegion")]
      public static extern IntPtr GpuMatGetRegion(IntPtr gpuMat, MCvSlice rowRange, MCvSlice colRange);

      /// <summary>
      /// Check if the GpuMat is empty
      /// </summary>
      /// <param name="gpuMat">The GpuMat</param>
      /// <returns>True if the GpuMat is empty</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatIsEmpty")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool GpuMatIsEmpty(IntPtr gpuMat);

      /// <summary>
      /// Copies scalar value to every selected element of the destination GpuMat:
      /// arr(I)=value if mask(I)!=0
      /// </summary>
      /// <param name="mat">The destination GpuMat</param>
      /// <param name="value">Fill value</param>
      /// <param name="mask">Operation mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Can be IntPtr.Zero if not used</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>     
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatSetTo")]
      public static extern void GpuMatSetTo(IntPtr mat, MCvScalar value, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Resize the GpuMat
      /// </summary>
      /// <param name="src">The input GpuMat</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="interpolation">The interpolation type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>     
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatResize")]
      public static extern void GpuMatResize(IntPtr src, IntPtr dst, CvEnum.INTER interpolation, IntPtr stream);

      /// <summary>
      /// Reshape the src GpuMat  
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The resulting GpuMat, as input it should be an empty GpuMat.</param>
      /// <param name="cn">The new number of channels</param>
      /// <param name="rows">The new number of rows</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatReshape")]
      public static extern void GpuMatReshape(IntPtr src, IntPtr dst, int cn, int rows);

      /// <summary>
      /// Release the GpuMat
      /// </summary>
      /// <param name="mat">Pointer to the GpuMat</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatRelease")]
      public static extern void GpuMatRelease(ref IntPtr mat);

      /// <summary>
      /// Convert a CvArr to a GpuMat
      /// </summary>
      /// <param name="arr">Pointer to a CvArr</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreateFromArr")]
      public static extern IntPtr GpuMatCreateFromArr(IntPtr arr);

      /// <summary>
      /// Get the GpuMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      /// <param name="gpuMat">The GpuMat</param>
      /// <returns>The size of the matrix</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetSize")]
      public static extern Size GpuMatGetSize(IntPtr gpuMat);

      /// <summary>
      /// Get the number of channels in the GpuMat
      /// </summary>
      /// <param name="gpuMat">The GpuMat</param>
      /// <returns>The number of channels in the GpuMat</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetChannels")]
      public static extern int GpuMatGetChannels(IntPtr gpuMat);

      /// <summary>
      /// Pefroms blocking upload data to GpuMat.
      /// </summary>
      /// <param name="gpuMat">The destination gpuMat</param>
      /// <param name="arr">The CvArray to be uploaded to GPU</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatUpload")]
      public static extern void GpuMatUpload(IntPtr gpuMat, IntPtr arr);

      /// <summary>
      /// Downloads data from device to host memory. Blocking calls.
      /// </summary>
      /// <param name="gpuMat">The source GpuMat</param>
      /// <param name="arr">The CvArray where data will be downloaded to</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDownload")]
      public static extern void GpuMatDownload(IntPtr gpuMat, IntPtr arr);

      /// <summary>
      /// Converts image from one color space to another
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="code">The color conversion code</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCvtColor")]
      public static extern void CvtColor(IntPtr src, IntPtr dst, CvEnum.COLOR_CONVERSION code, IntPtr stream);

      /// <summary>
      /// Copy the source GpuMat to destination GpuMat, using an optional mask.
      /// </summary>
      /// <param name="src">The GpuMat to be copied from</param>
      /// <param name="dst">The GpuMat to be copied to</param>
      /// <param name="mask">The optional mask, use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCopy")]
      public static extern void Copy(IntPtr src, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Returns header, corresponding to a specified rectangle of the input GpuMat. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array.
      /// </summary>
      /// <param name="mat">Input GpuMat</param>
      /// <param name="rect">Zero-based coordinates of the rectangle of interest.</param>
      /// <returns>Pointer to the resultant sub-array header.</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetSubRect")]
      public static extern IntPtr GetSubRect(IntPtr mat, Rectangle rect);

      #region arithmatic
      /// <summary>
      /// Shifts a matrix to the left (c = a &lt;&lt; scalar)
      /// </summary>
      /// <param name="a">The matrix to be shifted.</param>
      /// <param name="scalar">The scalar to shift by.</param>
      /// <param name="c">The result of the shift</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatLShift")]
      public static extern void LShift(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Shifts a matrix to the right (c = a >> scalar)
      /// </summary>
      /// <param name="a">The matrix to be shifted.</param>
      /// <param name="scalar">The scalar to shift by.</param>
      /// <param name="c">The result of the shift</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatRShift")]
      public static extern void RShift(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Adds one matrix to another (c = a + b).
      /// </summary>
      /// <param name="a">The first matrix to be added.</param>
      /// <param name="b">The second matrix to be added.</param>
      /// <param name="c">The sum of the two matrix</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatAdd")]
      public static extern void Add(IntPtr a, IntPtr b, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Adds scalar to a matrix (c = a + scalar)
      /// </summary>
      /// <param name="a">The matrix to be added.</param>
      /// <param name="scalar">The scalar to be added.</param>
      /// <param name="c">The sum of the matrix and the scalar</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatAddS")]
      public static extern void Add(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Subtracts one matrix from another (c = a - b).
      /// </summary>
      /// <param name="a">The matrix where subtraction take place</param>
      /// <param name="b">The matrix to be substracted</param>
      /// <param name="c">The result of a - b</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatSubtract")]
      public static extern void Subtract(IntPtr a, IntPtr b, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Computes element-wise weighted product of the two arrays (c = scale * a * b) 
      /// Supports CV_32FC1 and CV_32FC2 type.
      /// </summary>
      /// <param name="a">The matrix to be substraced from</param>
      /// <param name="scalar">The scalar to be substracted</param>
      /// <param name="c">The matrix substraced by the scalar</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatSubtractS")]
      public static extern void Subtract(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Computes element-wise product of the two GpuMat: c = scale * a * b.
      /// </summary>
      /// <param name="a">The first GpuMat to be element-wise multiplied.</param>
      /// <param name="b">The second GpuMat to be element-wise multiplied.</param>
      /// <param name="c">The element-wise multiplication of the two GpuMat</param>
      /// <param name="scale">The scale</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMultiply")]
      public static extern void Multiply(IntPtr a, IntPtr b, IntPtr c, double scale, IntPtr stream);

      /// <summary>
      /// Multiplies GpuMat to a scalar (c = a * scalar).
      /// </summary>
      /// <param name="a">The first GpuMat to be element-wise multiplied.</param>
      /// <param name="scalar">The scalar to be multiplied</param>
      /// <param name="c">The result of the GpuMat mutiplied by the scalar</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMultiplyS")]
      public static extern void Multiply(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes element-wise quotient of the two GpuMat (c = scale *  a / b).
      /// </summary>
      /// <param name="a">The first GpuMat</param>
      /// <param name="b">The second GpuMat</param>
      /// <param name="c">The element-wise quotient of the two GpuMat</param>
      /// <param name="scale">The scale</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDivide")]
      public static extern void Divide(IntPtr a, IntPtr b, IntPtr c, double scale, IntPtr stream);

      /// <summary>
      /// Computes element-wise quotient of a GpuMat and scalar (c = a / scalar).
      /// </summary>
      /// <param name="a">The first GpuMat to be element-wise divided.</param>
      /// <param name="scalar">The scalar to be divided</param>
      /// <param name="c">The result of the GpuMat divided by the scalar</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDivideSR")]
      public static extern void Divide(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes element-wise weighted reciprocal of an array (c = scale/ b).
      /// </summary>
      /// <param name="b">The second GpuMat to be element-wise divided.</param>
      /// <param name="scalar">The dirst scalar to be divided</param>
      /// <param name="c">The result of the scalar dividing the GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDivideSL")]
      public static extern void Divide(double scalar, IntPtr b, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes the weighted sum of two arrays (dst = alpha*src1 + beta*src2 + gamma)
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="alpha">The weight for <paramref name="src1"/></param>
      /// <param name="src2">The second source GpuMat</param>
      /// <param name="beta">The weight for <paramref name="src2"/></param>
      /// <param name="gamma">The constant to be added</param>
      /// <param name="dst">The result</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatAddWeighted")]
      public static extern void AddWeighted(IntPtr src1, double alpha, IntPtr src2, double beta, double gamma, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes element-wise absolute difference of two GpuMats (c = abs(a - b)).
      /// </summary>
      /// <param name="a">The first GpuMat</param>
      /// <param name="b">The second GpuMat</param>
      /// <param name="c">The result of the element-wise absolute difference.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatAbsdiff")]
      public static extern void Absdiff(IntPtr a, IntPtr b, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes element-wise absolute difference of GpuMat and scalar (c = abs(a - s)).
      /// </summary>
      /// <param name="a">A GpuMat</param>
      /// <param name="scalar">A scalar</param>
      /// <param name="c">The result of the element-wise absolute difference.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatAbsdiffS")]
      public static extern void Absdiff(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes absolute value of each pixel in an image
      /// </summary>
      /// <param name="src">The source GpuMat, support depth of Int16 and float.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatAbs")]
      public static extern void Abs(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes square of each pixel in an image
      /// </summary>
      /// <param name="src">The source GpuMat, support depth of byte, UInt16, Int16 and float.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatSqr")]
      public static extern void Sqr(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes square root of each pixel in an image
      /// </summary>
      /// <param name="src">The source GpuMat, support depth of byte, UInt16, Int16 and float.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatSqrt")]
      public static extern void Sqrt(IntPtr src, IntPtr dst, IntPtr stream);
      #endregion

      /// <summary>
      /// Compares elements of two GpuMats (c = a &lt;cmpop&gt; b).
      /// Supports CV_8UC4, CV_32FC1 types
      /// </summary>
      /// <param name="a">The first GpuMat</param>
      /// <param name="b">The second GpuMat</param>
      /// <param name="c">The result of the comparison.</param>
      /// <param name="cmpop">The type of comparison</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCompare")]
      public static extern void Compare(IntPtr a, IntPtr b, IntPtr c, CvEnum.CMP_TYPE cmpop, IntPtr stream);

      /// <summary>
      /// Transforms 8-bit unsigned integers using lookup table: dst(i)=lut(src(i)).
      /// Destination GpuMat will have the depth type as lut and the same channels number as source.
      /// Supports CV_8UC1, CV_8UC3 types.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="lut">Pointer to a CvArr (e.g. Emgu.CV.Matrix).</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatLUT")]
      public static extern void LUT(IntPtr src, IntPtr lut, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Resizes the image.
      /// </summary>
      /// <param name="src">The source image. Has to be GpuMat&lt;Byte&gt;. If stream is used, the GpuMat has to be either single channel or 4 channels.</param>
      /// <param name="dst">The destination image.</param>
      /// <param name="interpolation">The interpolation type. Supports INTER_NEAREST, INTER_LINEAR.</param>
      /// <param name="stream">Use a stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatResize")]
      public static extern void Resize(IntPtr src, IntPtr dst, CvEnum.INTER interpolation, IntPtr stream);

      /// <summary>
      /// Changes shape of GpuMat without copying data.
      /// </summary>
      /// <param name="src">The GpuMat to be reshaped.</param>
      /// <param name="newCn">New number of channels. newCn = 0 means that the number of channels remains unchanged.</param>
      /// <param name="newRows">New number of rows. newRows = 0 means that the number of rows remains unchanged unless it needs to be changed according to newCn value.</param>
      /// <returns>A GpuMat of different shape</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatReshape")]
      public static extern IntPtr Reshape(IntPtr src, int newCn, int newRows);

      /// <summary>
      /// Copies each plane of a multi-channel GpuMat to a dedicated GpuMat
      /// </summary>
      /// <param name="src">The multi-channel gpuMat</param>
      /// <param name="dstArray">Pointer to an array of single channel GpuMat pointers</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatSplit")]
      public static extern void Split(IntPtr src, IntPtr dstArray, IntPtr stream);

      /// <summary>
      /// Makes multi-channel GpuMat out of several single-channel GpuMats
      /// </summary>
      /// <param name="srcArr">Pointer to an array of single channel GpuMat pointers</param>
      /// <param name="dst">The multi-channel gpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMerge")]
      public static extern void Merge(IntPtr srcArr, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes exponent of each matrix element (b = exp(a))
      /// </summary>
      /// <param name="src">The source GpuMat. Supports Byte, UInt16, Int16 and float type.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatExp")]
      public static extern void Exp(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes power of each matrix element:
      ///   (dst(i,j) = pow(     src(i,j) , power), if src.type() is integer;
      ///   (dst(i,j) = pow(fabs(src(i,j)), power), otherwise.
      /// supports all, except depth == CV_64F
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="power">The power</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatPow")]
      public static extern void Pow(IntPtr src, double power, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes natural logarithm of absolute value of each matrix element: b = log(abs(a))
      /// </summary>
      /// <param name="src">The source GpuMat. Supports Byte, UInt16, Int16 and float type.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatLog")]
      public static extern void Log(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes magnitude of each (x(i), y(i)) vector
      /// </summary>
      /// <param name="x">The source GpuMat. Supports only floating-point type</param>
      /// <param name="y">The source GpuMat. Supports only floating-point type</param>
      /// <param name="magnitude">The destination GpuMat. Supports only floating-point type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMagnitude")]
      public static extern void Magnitude(IntPtr x, IntPtr y, IntPtr magnitude, IntPtr stream);

      /// <summary>
      /// Computes squared magnitude of each (x(i), y(i)) vector
      /// </summary>
      /// <param name="x">The source GpuMat. Supports only floating-point type</param>
      /// <param name="y">The source GpuMat. Supports only floating-point type</param>
      /// <param name="magnitude">The destination GpuMat. Supports only floating-point type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMagnitudeSqr")]
      public static extern void MagnitudeSqr(IntPtr x, IntPtr y, IntPtr magnitude, IntPtr stream);

      /// <summary>
      /// Computes angle (angle(i)) of each (x(i), y(i)) vector
      /// </summary>
      /// <param name="x">The source GpuMat. Supports only floating-point type</param>
      /// <param name="y">The source GpuMat. Supports only floating-point type</param>
      /// <param name="angle">The destination GpuMat. Supports only floating-point type</param>
      /// <param name="angleInDegrees">If true, the output angle is in degrees, otherwise in radian</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatPhase")]
      public static extern void Phase(
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCartToPolar")]
      public static extern void CartToPolar(
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatPolarToCart")]
      public static extern void PolarToCart(
         IntPtr magnitude, IntPtr angle, IntPtr x, IntPtr y,
         [MarshalAs(CvInvoke.BoolMarshalType)] 
         bool angleInDegrees, IntPtr stream);

      /// <summary>
      /// This function has several different purposes and thus has several synonyms. It copies one GpuMat to another with optional scaling, which is performed first, and/or optional type conversion, performed after:
      /// dst(I)=src(I)*scale + (shift,shift,...)
      /// All the channels of multi-channel GpuMats are processed independently.
      /// The type conversion is done with rounding and saturation, that is if a result of scaling + conversion can not be represented exactly by a value of destination GpuMat element type, it is set to the nearest representable value on the real axis.
      /// In case of scale=1, shift=0 no prescaling is done. This is a specially optimized case and it has the appropriate convertTo synonym.
      /// </summary>
      /// <param name="src">Source GpuMat</param>
      /// <param name="dst">Destination GpuMat</param>
      /// <param name="scale">Scale factor</param>
      /// <param name="shift">Value added to the scaled source GpuMat elements</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>      
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatConvertTo")]
      public static extern void ConvertTo(IntPtr src, IntPtr dst, double scale, double shift, IntPtr stream);

      /// <summary>
      /// Finds minimum and maximum element values and their positions. The extremums are searched over the whole GpuMat or, if mask is not IntPtr.Zero, in the specified GpuMat region.
      /// </summary>
      /// <param name="gpuMat">The source GpuMat, single-channel</param>
      /// <param name="minVal">Pointer to returned minimum value</param>
      /// <param name="maxVal">Pointer to returned maximum value</param>
      /// <param name="minLoc">Pointer to returned minimum location</param>
      /// <param name="maxLoc">Pointer to returned maximum location</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMinMaxLoc")]
      public static extern void MinMaxLoc(IntPtr gpuMat,
         ref double minVal, ref double maxVal,
         ref Point minLoc, ref Point maxLoc,
         IntPtr mask);

      /// <summary>
      /// This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMatchTemplate")]
      public static extern void MatchTemplate(IntPtr image, IntPtr templ, IntPtr result, CvEnum.TM_TYPE method, IntPtr stream);

      /// <summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. 
      /// </summary>
      /// <param name="src">The source GpuImage.</param>
      /// <param name="dst">The destination GpuImage, should have 2x smaller width and height than the source.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatPyrDown")]
      public static extern void PyrDown(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition.
      /// </summary>
      /// <param name="src">The source GpuImage.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatPyrUp")]
      public static extern void PyrUp(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes mean value and standard deviation
      /// </summary>
      /// <param name="mtx">The GpuMat. Supports only CV_8UC1 type</param>
      /// <param name="mean">The mean value</param>
      /// <param name="stddev">The standard deviation</param>
      /// <param name="buffer">The buffer for the processing. Can be IntPtr.Zero if you do not wants to specify such.</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMeanStdDev")]
      public static extern void MeanStdDev(IntPtr mtx, ref MCvScalar mean, ref MCvScalar stddev, IntPtr buffer);

      /// <summary>
      /// Computes norm of the difference between two GpuMats
      /// </summary>
      /// <param name="src1">The GpuMat. Supports only CV_8UC1 type</param>
      /// <param name="src2">If IntPtr.Zero, norm operation is apply to <paramref name="src1"/> only. Otherwise, this is the GpuMat of type CV_8UC1</param>
      /// <param name="normType">The norm type. Supports NORM_INF, NORM_L1, NORM_L2.</param>
      /// <returns>The norm of the <paramref name="src1"/> if <paramref name="src2"/> is IntPtr.Zero. Otherwise the norm of the difference between two GpuMats.</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatNorm")]
      public static extern double Norm(IntPtr src1, IntPtr src2, Emgu.CV.CvEnum.NORM_TYPE normType);

      /// <summary>
      /// Counts non-zero array elements
      /// </summary>
      /// <param name="src">The GpuMat</param>
      /// <returns>The number of non-zero GpuMat elements</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCountNonZero")]
      public static extern int CountNonZero(IntPtr src);

      /// <summary>
      /// Reduces GpuMat to a vector by treating the GpuMat rows/columns as a set of 1D vectors and performing the specified operation on the vectors until a single row/column is obtained. 
      /// </summary>
      /// <param name="mtx">The input GpuMat</param>
      /// <param name="vec">The destination GpuMat. Must be preallocated 1 x n matrix and have the same number of channels as the input GpuMat</param>
      /// <param name="dim">The dimension index along which the matrix is reduce.</param>
      /// <param name="reduceOp">The reduction operation type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>      
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatReduce")]
      public static extern void Reduce(IntPtr mtx, IntPtr vec, CvEnum.REDUCE_DIMENSION dim, CvEnum.REDUCE_TYPE reduceOp, IntPtr stream);

      /// <summary>
      /// Flips the GpuMat in one of different 3 ways (row and column indices are 0-based):
      /// dst(i,j)=src(rows(src)-i-1,j) if flip_mode = 0
      /// dst(i,j)=src(i,cols(src1)-j-1) if flip_mode &gt; 0
      /// dst(i,j)=src(rows(src)-i-1,cols(src)-j-1) if flip_mode &lt; 0
      /// </summary>
      /// <param name="src">Source GpuMat.</param>
      /// <param name="dst">Destination GpuMat.</param>
      /// <param name="flipMode">
      /// Specifies how to flip the GpuMat.
      /// flip_mode = 0 means flipping around x-axis, 
      /// flip_mode &gt; 0 (e.g. 1) means flipping around y-axis and 
      /// flip_mode &lt; 0 (e.g. -1) means flipping around both axises. 
      ///</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>      
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuMatFlip(IntPtr src, IntPtr dst, int flipMode, IntPtr stream);

      /// <summary>
      /// Flips the GpuMat&lt;Byte&gt; in one of different 3 ways (row and column indices are 0-based). 
      /// </summary>
      /// <param name="src">The source GpuMat. supports 1, 3 and 4 channels GpuMat with Byte, UInt16, int or float depth</param>
      /// <param name="dst">Destination GpuMat. The same source and type as <paramref name="src"/></param>
      /// <param name="flipType">Specifies how to flip the GpuMat.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>      
      public static void Flip(IntPtr src, IntPtr dst, CvEnum.FLIP flipType, Stream stream)
      {
         int flipMode =
            //-1 indicates vertical and horizontal flip
            flipType == (Emgu.CV.CvEnum.FLIP.HORIZONTAL | Emgu.CV.CvEnum.FLIP.VERTICAL) ? -1 :
         //1 indicates horizontal flip only
            flipType == Emgu.CV.CvEnum.FLIP.HORIZONTAL ? 1 :
         //0 indicates vertical flip only
            0;
         gpuMatFlip(src, dst, flipMode, stream);
      }

      #region morphology operation
      /// <summary>
      /// Erodes the image (applies the local minimum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="buffer">Temperary buffer. Should be the same size and type as the <paramref name="src"/> GpuMat. </param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatErode")]
      public static extern void Erode(IntPtr src, IntPtr dst, IntPtr kernel, IntPtr buffer, Point anchor, int iterations, IntPtr stream);

      /// <summary>
      /// Dilate the image (applies the local maximum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="buffer">Temperary buffer. Should be the same size and type as the <paramref name="src"/> GpuMat. </param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDilate")]
      public static extern void Dilate(IntPtr src, IntPtr dst, IntPtr kernel, IntPtr buffer, Point anchor, int iterations, IntPtr stream);

      /// <summary>
      /// Applies an advanced morphological operation to the image
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="op">The type of morphological operation</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. </param>
      /// <param name="buffer1">Temperary buffer. Should be the same size and type as the <paramref name="src"/> GpuMat. </param>
      /// <param name="buffer2">Temperary buffer. Should be the same size and type as the <paramref name="src"/> GpuMat. Required for morphology operations other than erode or dilate. For erode and dilate operation, you can use IntPtr.Zero here.</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMorphologyEx")]
      public static extern void MorphologyEx(IntPtr src, IntPtr dst, CvEnum.CV_MORPH_OP op, IntPtr kernel, IntPtr buffer1, IntPtr buffer2, Point anchor, int iterations, IntPtr stream);

      #endregion

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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatBitwiseXor")]
      public static extern void BitwiseXor(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of a GpuMat and a scalar:
      /// dst(I)=src1(I)^scalar
      /// In the case of a floating-point GpuMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatBitwiseXorS")]
      public static extern void BitwiseXor(IntPtr src1, MCvScalar scalar, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical or of two GpuMats:
      /// dst(I)=src1(I) | src2(I) if mask(I)!=0
      /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="src2">The second source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatBitwiseOr")]
      public static extern void BitwiseOr(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical or a GpuMat and a scalar:
      /// dst(I)=src1(I) | scalar
      /// In the case of a floating-point GpuMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatBitwiseOrS")]
      public static extern void BitwiseOr(IntPtr src1, MCvScalar scalar, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical and of two GpuMats:
      /// dst(I)=src1(I) &amp; src2(I) if mask(I)!=0
      /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="src2">The second source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatBitwiseAnd")]
      public static extern void BitwiseAnd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical and of a GpuMat and a scalar:
      /// dst(I)=src1(I) &amp; scalar
      /// In the case of a floating-point GpuMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatBitwiseAndS")]
      public static extern void BitwiseAnd(IntPtr src1, MCvScalar scalar, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical not
      /// dst(I)=~src(I) if mask(I)!=0
      /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatBitwiseNot")]
      public static extern void BitwiseNot(IntPtr src, IntPtr dst, IntPtr mask, IntPtr stream);
      #endregion

      /// <summary>
      /// Computes per-element minimum of two GpuMats (dst = min(src1, src2))
      /// </summary>
      /// <param name="src1">The first GpuMat</param>
      /// <param name="src2">The second GpuMat</param>
      /// <param name="dst">The result GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMin")]
      public static extern void Min(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes per-element minimum of GpuMat and scalar (dst = min(src1, src2))
      /// </summary>
      /// <param name="src1">The first GpuMat</param>
      /// <param name="src2">The scalar</param>
      /// <param name="dst">The result GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMinS")]
      public static extern void Min(IntPtr src1, double src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes per-element maximum of two GpuMats (dst = max(src1, src2))
      /// </summary>
      /// <param name="src1">The first GpuMat</param>
      /// <param name="src2">The second GpuMat</param>
      /// <param name="dst">The result GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMax")]
      public static extern void Max(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes per-element maximum of GpuMat and scalar (dst = max(src1, src2))
      /// </summary>
      /// <param name="src1">The first GpuMat</param>
      /// <param name="src2">The scalar</param>
      /// <param name="dst">The result GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMaxS")]
      public static extern void Max(IntPtr src1, double src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Applies fixed-level thresholding to single-channel array. The function is typically used to get bi-level (binary) image out of grayscale image or for removing a noise, i.e. filtering out pixels with too small or too large values. There are several types of thresholding the function supports that are determined by thresholdType
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="threshold">Threshold value</param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="thresholdType">Thresholding type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatThreshold")]
      public static extern double Threshold(IntPtr src, IntPtr dst, double threshold, double maxValue, CvEnum.THRESH thresholdType, IntPtr stream);

      #region filters
      /// <summary>
      /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMmage</param>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix (e.g. Emgu.CV.Matrix). If you want to apply different kernels to different channels, split the gpu image into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      /// <param name="borderType">Border type. </param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatFilter2D")]
      public static extern void Filter2D(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, CvEnum.BORDER_TYPE borderType, IntPtr stream);

      /// <summary>
      /// Applies generalized Sobel operator to the image
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="dx">Order of the derivative x</param>
      /// <param name="dy">Order of the derivative y</param>
      /// <param name="buffer">A GpuMat that is used for buffer. Use IntPtr.Zero for default.</param>
      /// <param name="ksize">Size of the extended Sobel kernel</param>
      /// <param name="scale">Optional scale, use 1 for default.</param>
      /// <param name="rowBorderType">The row border type.</param>
      /// <param name="columnBorderType">The column border type.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatSobel")]
      public static extern void Sobel(IntPtr src, IntPtr dst, int dx, int dy, IntPtr buffer, int ksize, double scale, CvEnum.BORDER_TYPE rowBorderType, CvEnum.BORDER_TYPE columnBorderType, IntPtr stream);

      /// <summary>
      /// Applies Laplacian operator to the GpuMat
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="ksize">Either 1 or 3</param>
      /// <param name="scale">Optional scale. Use 1.0 for default</param>
      /// <param name="borderType">The border type.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatLaplacian")]
      public static extern void Laplacian(IntPtr src, IntPtr dst, int ksize, double scale, CvEnum.BORDER_TYPE borderType, IntPtr stream);

      /// <summary>
      /// Smooths the GpuMat using Gaussian filter.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The smoothed GpuMat</param>
      /// <param name="ksize">The size of the kernel</param>
      /// <param name="buffer">A GpuMat that is used for buffer. Use IntPtr.Zero for default.</param>
      /// <param name="sigma1">This parameter may specify Gaussian sigma (standard deviation). If it is zero, it is calculated from the kernel size.</param>
      /// <param name="sigma2">In case of non-square Gaussian kernel the parameter may be used to specify a different (from param3) sigma in the vertical direction. Use 0 for default</param>
      /// <param name="rowBorderType">The row border type.</param>
      /// <param name="columnBorderType">The column border type.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGaussianBlur")]
      public static extern void GaussianBlur(IntPtr src, IntPtr dst, Size ksize, IntPtr buffer, double sigma1, double sigma2, CvEnum.BORDER_TYPE rowBorderType, CvEnum.BORDER_TYPE columnBorderType, IntPtr stream);
      #endregion

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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGemm")]
      public static extern void Gemm(
         IntPtr src1,
         IntPtr src2,
         double alpha,
         IntPtr src3,
         double beta,
         IntPtr dst,
         CvEnum.GEMM_TYPE tABC,
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatWarpAffine")]
      public static extern void WarpAffine(IntPtr src, IntPtr dst, IntPtr M, CvEnum.INTER flags, CvEnum.BORDER_TYPE borderMode, MCvScalar borderValue, IntPtr stream);

      /// <summary>
      /// Warps the image using perspective transformation
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="M">The 2x3 transformation matrix (pointer to CvArr)</param>
      /// <param name="flags">Supports NN, LINEAR, CUBIC</param>
      /// <param name="borderMode">The border mode, use BORDER_TYPE.CONSTANT for default.</param>
      /// <param name="borderValue">The border value, use new MCvScalar() for default.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatWarpPerspective")]
      public static extern void WarpPerspective(IntPtr src, IntPtr dst, IntPtr M, CvEnum.INTER flags, CvEnum.BORDER_TYPE borderMode, MCvScalar borderValue, IntPtr stream);

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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatRemap")]
      public static extern void Remap(IntPtr src, IntPtr dst, IntPtr xmap, IntPtr ymap, CvEnum.INTER interpolation, CvEnum.BORDER_TYPE borderMode, MCvScalar borderValue, IntPtr stream);

      /// <summary>
      /// Performs mean-shift filtering for each point of the source image. It maps each point of the source
      /// image into another point, and as the result we have new color and new position of each point.
      /// </summary>
      /// <param name="src">Source GpuImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Destination GpuImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMeanShiftFiltering")]
      public static extern void MeanShiftFiltering(IntPtr src, IntPtr dst, int sp, int sr, MCvTermCriteria criteria, IntPtr stream);

      /// <summary>
      /// Performs mean-shift procedure and stores information about processed points (i.e. their colors
      /// and positions) into two images.
      /// </summary>
      /// <param name="src">Source GpuImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dstr">Destination GpuImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="dstsp">Destination GpuImage, containing position of mapped points. Will have the same size as src and CV 16SC2 type.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMeanShiftProc")]
      public static extern void MeanShiftProc(IntPtr src, IntPtr dstr, IntPtr dstsp, int sp, int sr, MCvTermCriteria criteria, IntPtr stream);

      /// <summary>
      /// Performs mean-shift segmentation of the source image and eleminates small segments.
      /// </summary>
      /// <param name="src">Source GpuImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Segmented Image. Will have the same size and type as src. Note that this is an Image type and not GpuImage type</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="minsize">Minimum segment size. Smaller segements will be merged.</param>
      /// <param name="criteria">Termination criteria.</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatMeanShiftSegmentation")]
      public static extern void MeanShiftSegmentation(IntPtr src, IntPtr dst, int sp, int sr, int minsize, MCvTermCriteria criteria);

      /// <summary>
      /// Rotates an image around the origin (0,0) and then shifts it.
      /// </summary>
      /// <param name="src">Source image. Supports 1, 3 or 4 channels images with Byte, UInt16 or float depth</param>
      /// <param name="dst">Destination image with the same type as src. Must be pre-allocated</param>
      /// <param name="angle">Angle of rotation in degrees</param>
      /// <param name="xShift">Shift along the horizontal axis</param>
      /// <param name="yShift">Shift along the verticle axis</param>
      /// <param name="interpolation">Interpolation method. Only INTER_NEAREST, INTER_LINEAR, and INTER_CUBIC are supported.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatRotate")]
      public static extern void Rotate(IntPtr src, IntPtr dst, double angle, double xShift, double yShift, CvEnum.INPAINT_TYPE interpolation, IntPtr stream);

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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCopyMakeBorder")]
      public static extern void CopyMakeBorder(IntPtr src, IntPtr dst, int top, int bottom, int left, int right, CvEnum.BORDER_TYPE borderType, MCvScalar value, IntPtr stream);

      /// <summary>
      /// Computes the integral image and integral for the squared image
      /// </summary>
      /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
      /// <param name="sum">The sum GpuMat, supports only CV_32S source type, but will contain unsigned int values</param>
      /// <param name="buffer">The buffer GpuMat, supports only CV32F source type.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatIntegral")]
      public static extern void IntegralBuffered(IntPtr src, IntPtr sum, IntPtr buffer, IntPtr stream);

      /// <summary>
      /// Computes the integral image
      /// </summary>
      /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
      /// <param name="sum">The sum GpuMat, supports only CV_32S source type, but will contain unsigned int values</param>
      public static void Integral(IntPtr src, IntPtr sum)
      {
         IntegralBuffered(src, sum, IntPtr.Zero, IntPtr.Zero);
      }

      /// <summary>
      /// Computes squared integral image 
      /// </summary>
      /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
      /// <param name="sqsum">The sqsum GpuMat, supports only CV32F source type.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void SqrIntegral(IntPtr src, IntPtr sqsum, IntPtr stream)
      {
         IntegralBuffered(src, IntPtr.Zero, sqsum, stream);
      }

      /// <summary>
      /// Runs the Harris edge detector on image. Similarly to cvCornerMinEigenVal and cvCornerEigenValsAndVecs, for each pixel it calculates 2x2 gradient covariation matrix M over block_size x block_size neighborhood. Then, it stores
      /// det(M) - k*trace(M)^2
      /// to the destination image. Corners in the image can be found as local maxima of the destination image.
      /// </summary>
      /// <param name="image">Input GpuMat</param>
      /// <param name="harrisResponce">GpuMat to store the Harris detector responces. Should have the same size as <paramref name="image"/>. </param>
      /// <param name="blockSize">Neighborhood size </param>
      /// <param name="kSize"></param>
      /// <param name="k">Harris detector free parameter.</param>
      /// <param name="borderType">Boreder type, use REFLECT101 for default</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCornerHarris")]
      public static extern void CornerHarris(IntPtr image, IntPtr harrisResponce, int blockSize, int kSize, double k, CvEnum.BORDER_TYPE borderType);

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
      /// <param name="flags">DFT flags</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDft")]
      public static extern void Dft(IntPtr src, IntPtr dst, CvEnum.CV_DXT flags, IntPtr stream);

      /// <summary>
      /// Calculates histogram with evenly distributed bins for signle channel source.
      /// </summary>
      /// <param name="src">The source GpuMat. Supports CV_8UC1, CV_16UC1 and CV_16SC1 types.</param>
      /// <param name="buffer">The GpuMat Buffer</param>
      /// <param name="histSize">The size of histogram (number of levels)</param>
      /// <param name="lowerLevel">The lower level</param>
      /// <param name="upperLevel">The upper level</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      /// <param name="hist">Histogram with evenly distributed bins. A GpuMat&lt;int&gt; type.</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="gpuMatHistEven")]
      public static extern void HistEven(IntPtr src, IntPtr hist, IntPtr buffer, int histSize, int lowerLevel, int upperLevel, IntPtr stream);

      /// <summary>
      /// Calculates histogram with evenly distributed bins for signle channel source.
      /// </summary>
      /// <param name="src">The source GpuMat. Supports CV_8UC1, CV_16UC1 and CV_16SC1 types.</param>
      /// <param name="buffer">The GpuMat Buffer</param>
      /// <param name="histSize">The size of histogram (number of levels)</param>
      /// <param name="lowerLevel">The lower level</param>
      /// <param name="upperLevel">The upper level</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      /// <returns>Histogram with evenly distributed bins</returns>
      public static GpuMat<Int32> HistEven(IntPtr src, IntPtr buffer, int histSize, int lowerLevel, int upperLevel, IntPtr stream)
      {
         GpuMat<int> hist = new GpuMat<int>();
         HistEven(src, hist, buffer, histSize, lowerLevel, upperLevel, stream);
         return hist;
      }

      /// <summary>
      /// Finds the edges on the input <paramref name="image"/> and marks them in the output image edges using the Canny algorithm. The smallest of threshold1 and threshold2 is used for edge linking, the largest - to find initial segments of strong edges.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      /// <param name="lowThreshold">The first threshold</param>
      /// <param name="highThreshold">The second threshold</param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator, use 3 for default</param>
      /// <param name="L2gradient">Use false for default</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCanny")]
      public static extern void Canny(
         IntPtr image,
         IntPtr edges,
         double lowThreshold,
         double highThreshold,
         int apertureSize,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool L2gradient);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuCreateOpticalFlowNeedleMap")]
      private static extern void CreateOpticalFlowNeedleMap(IntPtr u, IntPtr v, IntPtr vertex, IntPtr colors);

      public static void CreateOpticalFlowNeedleMap(GpuImage<Gray, float> u, GpuImage<Gray, float> v, GpuMat<float> vertex, GpuMat<float> colors)
      {
         CreateOpticalFlowNeedleMap(u, v, vertex, colors);
      }

   }
}
