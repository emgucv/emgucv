//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
         CvInvoke.CheckLibraryLoaded();
         
#if !IOS
         String[] modules = new String[] 
         {
            CvInvoke.ExternCudaLibrary
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
               } catch (Exception e)
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
      /// Create an empty GpuMat 
      /// </summary>
      /// <returns>Pointer to an empty GpuMat</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreateDefault")]
      internal static extern IntPtr GpuMatCreateDefault();

      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="type">The type of GpuMat</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreate")]
      public static extern IntPtr GpuMatCreate(int rows, int cols, int type);

      /// <summary>
      /// Create a GpuMat of the specified size. The allocated data is continuous within this GpuMat.
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="type">The type of GpuMat</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreateContinuous")]
      internal static extern IntPtr GpuMatCreateContinuous(int rows, int cols, int type);

      /// <summary>
      /// Returns true iff the GpuMatrix data is continuous
      /// (i.e. when there are no gaps between successive rows).
      /// </summary>
      /// <param name="gpuMat">The GpuMat to be checked</param>
      /// <returns>True if the GpuMat is continuous</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatIsContinuous")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool GpuMatIsContinuous(IntPtr gpuMat);

      /// <summary>
      /// Create a GpuMat from the specific region of <paramref name="gpuMat"/>. The data is shared between the two GpuMat.
      /// </summary>
      /// <param name="gpuMat">The gpuMat to extract regions from.</param>
      /// <param name="colRange">The column range. Use MCvSlice.WholeSeq for all columns.</param>
      /// <param name="rowRange">The row range. Use MCvSlice.WholeSeq for all rows.</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetRegion")]
      public static extern IntPtr GetRegion(IntPtr gpuMat, ref MCvSlice rowRange, ref MCvSlice colRange);

      /// <summary>
      /// Check if the GpuMat is empty
      /// </summary>
      /// <param name="gpuMat">The GpuMat</param>
      /// <returns>True if the GpuMat is empty</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatIsEmpty")]
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
      public static void GpuMatSetTo(GpuMat mat, MCvScalar value, IInputArray mask, Stream stream)
      {
         gpuMatSetTo(mat, ref value, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuMatSetTo(IntPtr mat, ref MCvScalar value, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Resize the GpuMat
      /// </summary>
      /// <param name="src">The input GpuMat</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="interpolation">The interpolation type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>     
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatResize")]
      public static extern void GpuMatResize(IntPtr src, IntPtr dst, CvEnum.Inter interpolation, IntPtr stream);

      /// <summary>
      /// Reshape the src GpuMat  
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The resulting GpuMat, as input it should be an empty GpuMat.</param>
      /// <param name="cn">The new number of channels</param>
      /// <param name="rows">The new number of rows</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatReshape")]
      public static extern void GpuMatReshape(IntPtr src, IntPtr dst, int cn, int rows);

      /// <summary>
      /// Release the GpuMat
      /// </summary>
      /// <param name="mat">Pointer to the GpuMat</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatRelease")]
      public static extern void GpuMatRelease(ref IntPtr mat);

      /// <summary>
      /// Convert a CvArr to a GpuMat
      /// </summary>
      /// <param name="arr">Pointer to a CvArr</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCreateFromArr")]
      public static extern IntPtr GpuMatCreateFromArr(IntPtr arr);

      /// <summary>
      /// Get the GpuMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      /// <param name="gpuMat">The GpuMat</param>
      /// <returns>The size of the matrix</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetSize")]
      public static extern Size GpuMatGetSize(IntPtr gpuMat);

      /// <summary>
      /// Get the number of channels in the GpuMat
      /// </summary>
      /// <param name="gpuMat">The GpuMat</param>
      /// <returns>The number of channels in the GpuMat</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetChannels")]
      public static extern int GpuMatGetChannels(IntPtr gpuMat);

      /// <summary>
      /// Get the GpuMat type
      /// </summary>
      /// <param name="gpuMat">The GpuMat</param>
      /// <returns>The GpuMat type</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatGetType")]
      public static extern int GpuMatGetType(IntPtr gpuMat);

      /// <summary>
      /// Pefroms blocking upload data to GpuMat.
      /// </summary>
      /// <param name="gpuMat">The destination gpuMat</param>
      /// <param name="arr">The CvArray to be uploaded to GPU</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatUpload")]
      public static extern void GpuMatUpload(IntPtr gpuMat, IntPtr arr);

      /// <summary>
      /// Downloads data from device to host memory. Blocking calls.
      /// </summary>
      /// <param name="gpuMat">The source GpuMat</param>
      /// <param name="arr">The CvArray where data will be downloaded to</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatDownload")]
      public static extern void GpuMatDownload(IntPtr gpuMat, IntPtr arr);

      /// <summary>
      /// Converts image from one color space to another
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="code">The color conversion code</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaCvtColor")]
      public static extern void CvtColor(IntPtr src, IntPtr dst, CvEnum.ColorConversion code, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSwapChannels(IntPtr src, IntPtr dstOrder, IntPtr stream);

      /// <summary>
      /// Swap channels.
      /// </summary>
      /// <param name="src">The image where the channels will be swapped</param>
      /// <param name="dstOrder">
      /// Integer array describing how channel values are permutated. The n-th entry
      /// of the array contains the number of the channel that is stored in the n-th channel of
      /// the output image. E.g. Given an RGBA image, aDstOrder = [3,2,1,0] converts this to ABGR
      /// channel order.
      /// </param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void SwapChannels(IntPtr src, int[] dstOrder, IntPtr stream)
      {
         if (dstOrder == null || dstOrder.Length < 4)
            throw new ArgumentException("dstOrder must be an int array of size 4");
         GCHandle handle = GCHandle.Alloc(dstOrder, GCHandleType.Pinned);
         cudaSwapChannels(src, handle.AddrOfPinnedObject(), stream);
         handle.Free();
      }

      /// <summary>
      /// Copy the source GpuMat to destination GpuMat, using an optional mask.
      /// </summary>
      /// <param name="src">The GpuMat to be copied from</param>
      /// <param name="dst">The GpuMat to be copied to</param>
      /// <param name="mask">The optional mask, use IntPtr.Zero if not needed.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatCopy")]
      public static extern void Copy(IntPtr src, IntPtr dst, IntPtr mask, IntPtr stream);

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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaLShift")]
      public static extern void LShift(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Shifts a matrix to the right (c = a >> scalar)
      /// </summary>
      /// <param name="a">The matrix to be shifted.</param>
      /// <param name="scalar">The scalar to shift by.</param>
      /// <param name="c">The result of the shift</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaRShift")]
      public static extern void RShift(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Adds one matrix to another (c = a + b).
      /// </summary>
      /// <param name="a">The first matrix to be added.</param>
      /// <param name="b">The second matrix to be added.</param>
      /// <param name="c">The sum of the two matrix</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Add(IInputArray a, IInputArray b, IOutputArray c, IInputArray mask, Stream stream)
      {
         cudaAdd(a.InputArrayPtr, b.InputArrayPtr, c.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaAdd(IntPtr a, IntPtr b, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Adds scalar to a matrix (c = a + scalar)
      /// </summary>
      /// <param name="a">The matrix to be added.</param>
      /// <param name="scalar">The scalar to be added.</param>
      /// <param name="c">The sum of the matrix and the scalar</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Add(IInputArray a, MCvScalar scalar, IOutputArray c, IInputArray mask, Stream stream)
      {
         cudaAddS(a.InputArrayPtr, ref scalar, c.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaAddS(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Subtracts one matrix from another (c = a - b).
      /// </summary>
      /// <param name="a">The matrix where subtraction take place</param>
      /// <param name="b">The matrix to be substracted</param>
      /// <param name="c">The result of a - b</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Subtract(IInputArray a, IInputArray b, IOutputArray c, IInputArray mask, Stream stream)
      {
         cudaSubtract(a.InputArrayPtr, b.InputArrayPtr, c.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSubtract(IntPtr a, IntPtr b, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Computes element-wise weighted product of the two arrays (c = scale * a * b) 
      /// Supports CV_32FC1 and CV_32FC2 type.
      /// </summary>
      /// <param name="a">The matrix to be substraced from</param>
      /// <param name="scalar">The scalar to be substracted</param>
      /// <param name="c">The matrix substraced by the scalar</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Subtract(IInputArray a, MCvScalar scalar, IOutputArray c, IInputArray mask, Stream stream)
      {
         cudaSubtractS(a.InputArrayPtr, ref scalar, c.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSubtractS(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Computes element-wise product of the two GpuMat: c = scale * a * b.
      /// </summary>
      /// <param name="a">The first GpuMat to be element-wise multiplied.</param>
      /// <param name="b">The second GpuMat to be element-wise multiplied.</param>
      /// <param name="c">The element-wise multiplication of the two GpuMat</param>
      /// <param name="scale">The scale</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Multiply(IInputArray a, IInputArray b, IOutputArray c, double scale, Stream stream)
      {
         cudaMultiply(a.InputArrayPtr, b.InputArrayPtr, c.OutputArrayPtr, scale, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMultiply(IntPtr a, IntPtr b, IntPtr c, double scale, IntPtr stream);

      /// <summary>
      /// Multiplies GpuMat to a scalar (c = a * scalar).
      /// </summary>
      /// <param name="a">The first GpuMat to be element-wise multiplied.</param>
      /// <param name="scalar">The scalar to be multiplied</param>
      /// <param name="c">The result of the GpuMat mutiplied by the scalar</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Multiply(IInputArray a, MCvScalar scalar, IOutputArray c, Stream stream)
      {
         cudaMultiplyS(a.InputArrayPtr, ref scalar, c.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMultiplyS(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes element-wise quotient of the two GpuMat (c = scale *  a / b).
      /// </summary>
      /// <param name="a">The first GpuMat</param>
      /// <param name="b">The second GpuMat</param>
      /// <param name="c">The element-wise quotient of the two GpuMat</param>
      /// <param name="scale">The scale</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Divide(IInputArray a, IInputArray b, IOutputArray c, double scale, Stream stream)
      {
         cudaDivide(a.InputArrayPtr, b.InputArrayPtr, c.OutputArrayPtr, scale, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaDivide(IntPtr a, IntPtr b, IntPtr c, double scale, IntPtr stream);

      /// <summary>
      /// Computes element-wise quotient of a GpuMat and scalar (c = a / scalar).
      /// </summary>
      /// <param name="a">The first GpuMat to be element-wise divided.</param>
      /// <param name="scalar">The scalar to be divided</param>
      /// <param name="c">The result of the GpuMat divided by the scalar</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Divide(IInputArray a, MCvScalar scalar, IOutputArray c, Stream stream)
      {
         cudaDivideSR(a.InputArrayPtr, ref scalar, c.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaDivideSR(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes element-wise weighted reciprocal of an array (c = scale/ b).
      /// </summary>
      /// <param name="b">The second GpuMat to be element-wise divided.</param>
      /// <param name="scalar">The first scalar to be divided</param>
      /// <param name="c">The result of the scalar dividing the GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Divide(double scalar, IInputArray b, IOutputArray c, Stream stream)
      {
         cudaDivideSL(scalar, b.InputArrayPtr, c.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaDivideSL(double scalar, IntPtr b, IntPtr c, IntPtr stream);

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
      public static void AddWeighted(IInputArray src1, double alpha, IInputArray src2, double beta, double gamma, IOutputArray dst, Stream stream)
      {
         cudaAddWeighted(src1.InputArrayPtr, alpha, src2.InputArrayPtr, beta, gamma, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaAddWeighted(IntPtr src1, double alpha, IntPtr src2, double beta, double gamma, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes element-wise absolute difference of two GpuMats (c = abs(a - b)).
      /// </summary>
      /// <param name="a">The first GpuMat</param>
      /// <param name="b">The second GpuMat</param>
      /// <param name="c">The result of the element-wise absolute difference.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Absdiff(IInputArray a, IInputArray b, IOutputArray c, Stream stream)
      {
         cudaAbsdiff(a.InputArrayPtr, b.InputArrayPtr, c.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaAbsdiff(IntPtr a, IntPtr b, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes element-wise absolute difference of GpuMat and scalar (c = abs(a - s)).
      /// </summary>
      /// <param name="a">A GpuMat</param>
      /// <param name="scalar">A scalar</param>
      /// <param name="c">The result of the element-wise absolute difference.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Absdiff(IInputArray a, MCvScalar scalar, IOutputArray c, Stream stream)
      {
         cudaAbsdiffS(a.InputArrayPtr, ref scalar, c.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaAbsdiffS(IntPtr a, ref MCvScalar scalar, IntPtr c, IntPtr stream);

      /// <summary>
      /// Computes absolute value of each pixel in an image
      /// </summary>
      /// <param name="src">The source GpuMat, support depth of Int16 and float.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Abs(IInputArray src, IOutputArray dst, Stream stream)
      {
         cudaAbs(src.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaAbs(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes square of each pixel in an image
      /// </summary>
      /// <param name="src">The source GpuMat, support depth of byte, UInt16, Int16 and float.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Sqr(IInputArray src, IOutputArray dst, Stream stream)
      {
         cudaSqr(src.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSqr(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes square root of each pixel in an image
      /// </summary>
      /// <param name="src">The source GpuMat, support depth of byte, UInt16, Int16 and float.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Sqrt(IInputArray src, IOutputArray dst, Stream stream)
      {
         cudaSqrt(src.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSqrt(IntPtr src, IntPtr dst, IntPtr stream);
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
      public static void Compare(IInputArray a, IInputArray b, IOutputArray c, CvEnum.CmpType cmpop, Stream stream)
      {
         cudaCompare(a.InputArrayPtr, b.InputArrayPtr, c.OutputArrayPtr, cmpop, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaCompare(IntPtr a, IntPtr b, IntPtr c, CvEnum.CmpType cmpop, IntPtr stream);

      /// <summary>
      /// Resizes the image.
      /// </summary>
      /// <param name="src">The source image. Has to be GpuMat&lt;Byte&gt;. If stream is used, the GpuMat has to be either single channel or 4 channels.</param>
      /// <param name="dst">The destination image.</param>
      /// <param name="interpolation">The interpolation type. Supports INTER_NEAREST, INTER_LINEAR.</param>
      /// <param name="stream">Use a stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Resize(IInputArray src, IOutputArray dst, CvEnum.Inter interpolation, Stream stream)
      {
         cudaResize(src.InputArrayPtr, dst.OutputArrayPtr, interpolation, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaResize(IntPtr src, IntPtr dst, CvEnum.Inter interpolation, IntPtr stream);

      /// <summary>
      /// Changes shape of GpuMat without copying data.
      /// </summary>
      /// <param name="src">The GpuMat to be reshaped.</param>
      /// <param name="newCn">New number of channels. newCn = 0 means that the number of channels remains unchanged.</param>
      /// <param name="newRows">New number of rows. newRows = 0 means that the number of rows remains unchanged unless it needs to be changed according to newCn value.</param>
      /// <returns>A GpuMat of different shape</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatReshape")]
      public static extern IntPtr Reshape(IntPtr src, int newCn, int newRows);

      /// <summary>
      /// Copies each plane of a multi-channel GpuMat to a dedicated GpuMat
      /// </summary>
      /// <param name="src">The multi-channel gpuMat</param>
      /// <param name="dstArray">Pointer to an array of single channel GpuMat pointers</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaSplit")]
      public static extern void Split(IntPtr src, IntPtr dstArray, IntPtr stream);

      /// <summary>
      /// Makes multi-channel GpuMat out of several single-channel GpuMats
      /// </summary>
      /// <param name="srcArr">Pointer to an array of single channel GpuMat pointers</param>
      /// <param name="dst">The multi-channel gpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaMerge")]
      public static extern void Merge(IntPtr srcArr, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes exponent of each matrix element (b = exp(a))
      /// </summary>
      /// <param name="src">The source GpuMat. Supports Byte, UInt16, Int16 and float type.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Exp(IInputArray src, IOutputArray dst, Stream stream)
      {
         cudaExp(src.InputArrayPtr, dst.OutputArrayPtr, stream);
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Pow(IInputArray src, double power, IOutputArray dst, Stream stream)
      {
         cudaPow(src.InputArrayPtr, power, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaPow(IntPtr src, double power, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes natural logarithm of absolute value of each matrix element: b = log(abs(a))
      /// </summary>
      /// <param name="src">The source GpuMat. Supports Byte, UInt16, Int16 and float type.</param>
      /// <param name="dst">The resulting GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Log(IInputArray src, IOutputArray dst, Stream stream)
      {
         cudaLog(src.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaLog(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes magnitude of each (x(i), y(i)) vector
      /// </summary>
      /// <param name="x">The source GpuMat. Supports only floating-point type</param>
      /// <param name="y">The source GpuMat. Supports only floating-point type</param>
      /// <param name="magnitude">The destination GpuMat. Supports only floating-point type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Magnitude(IInputArray x, IInputArray y, IOutputArray magnitude, Stream stream)
      {
         cudaMagnitude(x.InputArrayPtr, y.InputArrayPtr, magnitude.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMagnitude(IntPtr x, IntPtr y, IntPtr magnitude, IntPtr stream);

      /// <summary>
      /// Computes squared magnitude of each (x(i), y(i)) vector
      /// </summary>
      /// <param name="x">The source GpuMat. Supports only floating-point type</param>
      /// <param name="y">The source GpuMat. Supports only floating-point type</param>
      /// <param name="magnitude">The destination GpuMat. Supports only floating-point type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaMagnitudeSqr")]
      public static extern void MagnitudeSqr(IntPtr x, IntPtr y, IntPtr magnitude, IntPtr stream);

      /// <summary>
      /// Computes angle (angle(i)) of each (x(i), y(i)) vector
      /// </summary>
      /// <param name="x">The source GpuMat. Supports only floating-point type</param>
      /// <param name="y">The source GpuMat. Supports only floating-point type</param>
      /// <param name="angle">The destination GpuMat. Supports only floating-point type</param>
      /// <param name="angleInDegrees">If true, the output angle is in degrees, otherwise in radian</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void Phase(IInputArray x, IInputArray y, IOutputArray angle, bool angleInDegrees, Stream stream)
      {
         cudaPhase(x.InputArrayPtr, y.InputArrayPtr, angle.OutputArrayPtr, angleInDegrees, stream);
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void CartToPolar(IInputArray x, IInputArray y, IOutputArray magnitude, IOutputArray angle, bool angleInDegrees, Stream stream)
      {
         cudaCartToPolar(x.InputArrayPtr, y.InputArrayPtr, magnitude.OutputArrayPtr, angle.OutputArrayPtr, angleInDegrees, stream);
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void PolarToCart(IInputArray magnitude, IInputArray angle, IOutputArray x, IOutputArray y, bool angleInDegrees, Stream stream)
      {
         cudaPolarToCart(magnitude.InputArrayPtr, angle.InputArrayPtr, x.OutputArrayPtr, y.OutputArrayPtr, angleInDegrees, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaPolarToCart(
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
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "gpuMatConvertTo")]
      public static extern void ConvertTo(IntPtr src, IntPtr dst, double scale, double shift, IntPtr stream);

      /// <summary>
      /// Finds minimum and maximum element values and their positions. The extremums are searched over the whole GpuMat or, if mask is not IntPtr.Zero, in the specified GpuMat region.
      /// </summary>
      /// <param name="gpuMat">The source GpuMat, single-channel</param>
      /// <param name="minVal">Pointer to returned minimum value</param>
      /// <param name="maxVal">Pointer to returned maximum value</param>
      /// <param name="minLoc">Pointer to returned minimum location</param>
      /// <param name="maxLoc">Pointer to returned maximum location</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use null if not needed</param>
      public static void MinMaxLoc(IInputArray gpuMat, ref double minVal, ref double maxVal, ref Point minLoc, ref Point maxLoc, IInputArray mask)
      {
         cudaMinMaxLoc(gpuMat.InputArrayPtr, ref minVal, ref maxVal, ref minLoc, ref maxLoc, mask == null ? IntPtr.Zero : mask.InputArrayPtr);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMinMaxLoc(IntPtr gpuMat, ref double minVal, ref double maxVal, ref Point minLoc, ref Point maxLoc, IntPtr mask);

      /// <summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. 
      /// </summary>
      /// <param name="src">The source CudaImage.</param>
      /// <param name="dst">The destination CudaImage, should have 2x smaller width and height than the source.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      public static void PyrDown(IInputArray src, IOutputArray dst, Stream stream)
      {
         cudaPyrDown(src.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaPyrDown(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition.
      /// </summary>
      /// <param name="src">The source CudaImage.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      public static void PyrUp(IInputArray src, IOutputArray dst, Stream stream)
      {
         cudaPyrUp(src.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaPyrUp(IntPtr src, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes mean value and standard deviation
      /// </summary>
      /// <param name="mtx">The GpuMat. Supports only CV_8UC1 type</param>
      /// <param name="mean">The mean value</param>
      /// <param name="stddev">The standard deviation</param>
      /// <param name="buffer">The buffer for the processing. Can be null if you do not wants to specify such.</param>
      public static void MeanStdDev(IInputArray mtx, ref MCvScalar mean, ref MCvScalar stddev, GpuMat buffer)
      {
         cudaMeanStdDev(mtx.InputArrayPtr, ref mean, ref stddev, buffer);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMeanStdDev(IntPtr mtx, ref MCvScalar mean, ref MCvScalar stddev, IntPtr buffer);

      /// <summary>
      /// Computes norm of the difference between two GpuMats
      /// </summary>
      /// <param name="src1">The GpuMat. Supports only CV_8UC1 type</param>
      /// <param name="src2">If IntPtr.Zero, norm operation is apply to <paramref name="src1"/> only. Otherwise, this is the GpuMat of type CV_8UC1</param>
      /// <param name="normType">The norm type. Supports NORM_INF, NORM_L1, NORM_L2.</param>
      /// <returns>The norm of the <paramref name="src1"/> if <paramref name="src2"/> is IntPtr.Zero. Otherwise the norm of the difference between two GpuMats.</returns>
      public static double Norm(IInputArray src1, IInputArray src2, Emgu.CV.CvEnum.NormType normType)
      {
         return cudaNorm(src1.InputArrayPtr, src2 != null ? src2.InputArrayPtr : IntPtr.Zero, normType);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern double cudaNorm(IntPtr src1, IntPtr src2, Emgu.CV.CvEnum.NormType normType);

      /// <summary>
      /// Counts non-zero array elements
      /// </summary>
      /// <param name="src">The GpuMat</param>
      /// <returns>The number of non-zero GpuMat elements</returns>
      public static int CountNonZero(IOutputArray src)
      {
         return cudaCountNonZero(src.OutputArrayPtr);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int cudaCountNonZero(IntPtr src);

      /// <summary>
      /// Reduces GpuMat to a vector by treating the GpuMat rows/columns as a set of 1D vectors and performing the specified operation on the vectors until a single row/column is obtained. 
      /// </summary>
      /// <param name="mtx">The input GpuMat</param>
      /// <param name="vec">The destination GpuMat. Must be preallocated 1 x n matrix and have the same number of channels as the input GpuMat</param>
      /// <param name="dim">The dimension index along which the matrix is reduce.</param>
      /// <param name="reduceOp">The reduction operation type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>      
      public static void Reduce(IInputArray mtx, IOutputArray vec, CvEnum.ReduceDimension dim, CvEnum.ReduceType reduceOp, Stream stream)
      {
         cudaReduce(mtx.InputArrayPtr, vec.OutputArrayPtr, dim, reduceOp, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaReduce(IntPtr mtx, IntPtr vec, CvEnum.ReduceDimension dim, CvEnum.ReduceType reduceOp, IntPtr stream);

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
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaFlip(IntPtr src, IntPtr dst, int flipMode, IntPtr stream);

      /// <summary>
      /// Flips the GpuMat&lt;Byte&gt; in one of different 3 ways (row and column indices are 0-based). 
      /// </summary>
      /// <param name="src">The source GpuMat. supports 1, 3 and 4 channels GpuMat with Byte, UInt16, int or float depth</param>
      /// <param name="dst">Destination GpuMat. The same source and type as <paramref name="src"/></param>
      /// <param name="flipType">Specifies how to flip the GpuMat.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>      
      public static void Flip(IInputArray src, IOutputArray dst, CvEnum.FlipType flipType, Stream stream)
      {
         int flipMode =
            //-1 indicates vertical and horizontal flip
            flipType == (Emgu.CV.CvEnum.FlipType.Horizontal | Emgu.CV.CvEnum.FlipType.Vertical) ? -1 :
         //1 indicates horizontal flip only
            flipType == Emgu.CV.CvEnum.FlipType.Horizontal ? 1 :
         //0 indicates vertical flip only
            0;
         cudaFlip(src.InputArrayPtr, dst.OutputArrayPtr, flipMode, stream);
      }


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
      public static void BitwiseXor(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask, Stream stream)
      {
         cudaBitwiseXor(src1.InputArrayPtr, src2.InputArrayPtr, dst.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBitwiseXor(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of a GpuMat and a scalar:
      /// dst(I)=src1(I)^scalar
      /// In the case of a floating-point GpuMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void BitwiseXor(IInputArray src1, MCvScalar scalar, IOutputArray dst, Stream stream)
      {
         cudaBitwiseXorS(src1.InputArrayPtr, ref scalar, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBitwiseXorS(IntPtr src1, ref MCvScalar scalar, IntPtr dst, IntPtr stream);

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
      public static void BitwiseOr(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask, Stream stream)
      {
         cudaBitwiseOr(src1.InputArrayPtr, src2.InputArrayPtr, dst.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBitwiseOr(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical or a GpuMat and a scalar:
      /// dst(I)=src1(I) | scalar
      /// In the case of a floating-point GpuMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void BitwiseOr(IInputArray src1, MCvScalar scalar, IOutputArray dst, Stream stream)
      {
         cudaBitwiseOrS(src1.InputArrayPtr, ref scalar, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBitwiseOrS(IntPtr src1, ref MCvScalar scalar, IntPtr dst, IntPtr stream);

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
      public static void BitwiseAnd(IInputArray src1, IInputArray src2, IOutputArray dst, IInputArray mask, Stream stream)
      {
         cudaBitwiseAnd(src1.InputArrayPtr, src2.InputArrayPtr, dst.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBitwiseAnd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical and of a GpuMat and a scalar:
      /// dst(I)=src1(I) &amp; scalar
      /// In the case of a floating-point GpuMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source GpuMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void BitwiseAnd(IInputArray src1, MCvScalar scalar, IOutputArray dst, Stream stream)
      {
         cudaBitwiseAndS(src1.InputArrayPtr, ref scalar, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBitwiseAndS(IntPtr src1, ref MCvScalar scalar, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Calculates per-element bit-wise logical not
      /// dst(I)=~src(I) if mask(I)!=0
      /// In the case of floating-point GpuMats their bit representations are used for the operation. All the GpuMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="mask">Mask, 8-bit single channel GpuMat; specifies elements of destination GpuMat to be changed. Use IntPtr.Zero if not needed.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void BitwiseNot(IInputArray src, IOutputArray dst, IInputArray mask, Stream stream)
      {
         cudaBitwiseNot(src.InputArrayPtr, dst.OutputArrayPtr, mask == null ? IntPtr.Zero : mask.InputArrayPtr, stream);
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
      public static void Min(IInputArray src1, IInputArray src2, IOutputArray dst, Stream stream)
      {
         cudaMin(src1.InputArrayPtr, src2.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMin(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes per-element minimum of GpuMat and scalar (dst = min(src1, src2))
      /// </summary>
      /// <param name="src1">The first GpuMat</param>
      /// <param name="src2">The scalar</param>
      /// <param name="dst">The result GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Min(IInputArray src1, double src2, IOutputArray dst, Stream stream)
      {
         cudaMinS(src1.InputArrayPtr, src2, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMinS(IntPtr src1, double src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes per-element maximum of two GpuMats (dst = max(src1, src2))
      /// </summary>
      /// <param name="src1">The first GpuMat</param>
      /// <param name="src2">The second GpuMat</param>
      /// <param name="dst">The result GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Max(IInputArray src1, IInputArray src2, IOutputArray dst, Stream stream)
      {
         cudaMax(src1.InputArrayPtr, src2.InputArrayPtr, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMax(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Computes per-element maximum of GpuMat and scalar (dst = max(src1, src2))
      /// </summary>
      /// <param name="src1">The first GpuMat</param>
      /// <param name="src2">The scalar</param>
      /// <param name="dst">The result GpuMat</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Max(IInputArray src1, double src2, IOutputArray dst, Stream stream)
      {
         cudaMaxS(src1.InputArrayPtr, src2, dst.OutputArrayPtr, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaMaxS(IntPtr src1, double src2, IntPtr dst, IntPtr stream);

      /// <summary>
      /// Applies fixed-level thresholding to single-channel array. The function is typically used to get bi-level (binary) image out of grayscale image or for removing a noise, i.e. filtering out pixels with too small or too large values. There are several types of thresholding the function supports that are determined by thresholdType
      /// </summary>
      /// <param name="src">Source array (single-channel, 8-bit of 32-bit floating point). </param>
      /// <param name="dst">Destination array; must be either the same type as src or 8-bit. </param>
      /// <param name="threshold">Threshold value</param>
      /// <param name="maxValue">Maximum value to use with CV_THRESH_BINARY and CV_THRESH_BINARY_INV thresholding types</param>
      /// <param name="thresholdType">Thresholding type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static double Threshold(IInputArray src, IOutputArray dst, double threshold, double maxValue, CvEnum.ThresholdType thresholdType, Stream stream)
      {
         return cudaThreshold(src.InputArrayPtr, dst.OutputArrayPtr, threshold, maxValue, thresholdType, stream);
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaGemm")]
      public static extern void Gemm(
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>
      public static void WarpAffine(IInputArray src, IOutputArray dst, IntPtr M, CvEnum.Inter flags, CvEnum.BorderType borderMode, MCvScalar borderValue, Stream stream)
      {
         cudaWarpAffine(src.InputArrayPtr, dst.OutputArrayPtr, M, flags, borderMode, ref borderValue, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaWarpAffine(IntPtr src, IntPtr dst, IntPtr M, CvEnum.Inter flags, CvEnum.BorderType borderMode, ref MCvScalar borderValue, IntPtr stream);

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
      public static void WarpPerspective(IInputArray src, IOutputArray dst, IntPtr M, CvEnum.Inter flags, CvEnum.BorderType borderMode, MCvScalar borderValue, Stream stream)
      {
         cudaWarpPerspective(src.InputArrayPtr, dst.OutputArrayPtr, M, flags, borderMode, ref borderValue, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaWarpPerspective(IntPtr src, IntPtr dst, IntPtr M, CvEnum.Inter flags, CvEnum.BorderType borderMode, ref MCvScalar borderValue, IntPtr stream);

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
      public static void Remap(IInputArray src, IOutputArray dst, IInputArray xmap, IInputArray ymap, CvEnum.Inter interpolation, CvEnum.BorderType borderMode, MCvScalar borderValue, Stream stream)
      {
         cudaRemap(src.InputArrayPtr, dst.OutputArrayPtr, xmap.InputArrayPtr, ymap.InputArrayPtr, interpolation, borderMode, ref borderValue, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaRemap(IntPtr src, IntPtr dst, IntPtr xmap, IntPtr ymap, CvEnum.Inter interpolation, CvEnum.BorderType borderMode, ref MCvScalar borderValue, IntPtr stream);

      /// <summary>
      /// Performs mean-shift filtering for each point of the source image. It maps each point of the source
      /// image into another point, and as the result we have new color and new position of each point.
      /// </summary>
      /// <param name="src">Source CudaImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Destination CudaImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaMeanShiftFiltering")]
      public static extern void MeanShiftFiltering(IntPtr src, IntPtr dst, int sp, int sr, ref MCvTermCriteria criteria, IntPtr stream);

      /// <summary>
      /// Performs mean-shift procedure and stores information about processed points (i.e. their colors
      /// and positions) into two images.
      /// </summary>
      /// <param name="src">Source CudaImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dstr">Destination CudaImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="dstsp">Destination CudaImage, containing position of mapped points. Will have the same size as src and CV 16SC2 type.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaMeanShiftProc")]
      public static extern void MeanShiftProc(IntPtr src, IntPtr dstr, IntPtr dstsp, int sp, int sr, ref MCvTermCriteria criteria, IntPtr stream);

      /// <summary>
      /// Performs mean-shift segmentation of the source image and eleminates small segments.
      /// </summary>
      /// <param name="src">Source CudaImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Segmented Image. Will have the same size and type as src. Note that this is an Image type and not CudaImage type</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="minsize">Minimum segment size. Smaller segements will be merged.</param>
      /// <param name="criteria">Termination criteria.</param>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaMeanShiftSegmentation")]
      public static extern void MeanShiftSegmentation(IntPtr src, IntPtr dst, int sp, int sr, int minsize, ref MCvTermCriteria criteria);

      /// <summary>
      /// Rotates an image around the origin (0,0) and then shifts it.
      /// </summary>
      /// <param name="src">Source image. Supports 1, 3 or 4 channels images with Byte, UInt16 or float depth</param>
      /// <param name="dst">Destination image with the same type as src. Must be pre-allocated</param>
      /// <param name="angle">Angle of rotation in degrees</param>
      /// <param name="xShift">Shift along the horizontal axis</param>
      /// <param name="yShift">Shift along the verticle axis</param>
      /// <param name="interpolation">Interpolation method. Only INTER_NEAREST, INTER_LINEAR, and INTER_CUBIC are supported.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Rotate(IInputArray src, IOutputArray dst, double angle, double xShift, double yShift, CvEnum.InpaintType interpolation, Stream stream)
      {
         cudaRotate(src.InputArrayPtr, dst.OutputArrayPtr, angle, xShift, yShift, interpolation, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaRotate(IntPtr src, IntPtr dst, double angle, double xShift, double yShift, CvEnum.InpaintType interpolation, IntPtr stream);

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
      public static void CopyMakeBorder(IInputArray src, IOutputArray dst, int top, int bottom, int left, int right, CvEnum.BorderType borderType, MCvScalar value, Stream stream)
      {
         cudaCopyMakeBorder(src.InputArrayPtr, dst.OutputArrayPtr, top, bottom, left, right, borderType, ref value, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaCopyMakeBorder(IntPtr src, IntPtr dst, int top, int bottom, int left, int right, CvEnum.BorderType borderType, ref MCvScalar value, IntPtr stream);

      /// <summary>
      /// Computes the integral image and integral for the squared image
      /// </summary>
      /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
      /// <param name="sum">The sum GpuMat, supports only CV_32S source type, but will contain unsigned int values</param>
      /// <param name="buffer">The buffer GpuMat, supports only CV32F source type.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void Integral(IInputArray src, IOutputArray sum, GpuMat buffer, Stream stream)
      {
         cudaIntegral(src.InputArrayPtr, sum.OutputArrayPtr, buffer, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaIntegral(IntPtr src, IntPtr sum, IntPtr buffer, IntPtr stream);

      /// <summary>
      /// Computes the integral image
      /// </summary>
      /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
      /// <param name="sum">The sum GpuMat, supports only CV_32S source type, but will contain unsigned int values</param>
      public static void Integral(IInputArray src, IOutputArray sum)
      {
         Integral(src, sum, null, null);
      }

      /// <summary>
      /// Computes squared integral image 
      /// </summary>
      /// <param name="src">The source GpuMat, supports only CV_8UC1 source type</param>
      /// <param name="sqsum">The sqsum GpuMat, supports only CV32F source type.</param>
      /// <param name="buffer">The buffer GpuMat, supports only CV32F source type.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public static void SqrIntegral(IInputArray src, IOutputArray sqsum, GpuMat buffer, Stream stream)
      {
         cudaSqrIntegral(src.InputArrayPtr, sqsum.OutputArrayPtr, buffer, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaSqrIntegral(IntPtr src, IntPtr sqsum, IntPtr buffer, IntPtr stream);

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
      public static void Dft(IInputArray src, IOutputArray dst, CvEnum.DxtType flags, Stream stream)
      {
         cudaDft(src.InputArrayPtr, dst.OutputArrayPtr, flags, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaDft(IntPtr src, IntPtr dst, CvEnum.DxtType flags, IntPtr stream);

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
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint="cudaHistEven")]
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

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaCreateOpticalFlowNeedleMap")]
      private static extern void CreateOpticalFlowNeedleMap(IntPtr u, IntPtr v, IntPtr vertex, IntPtr colors);

      /// <summary>
      /// Draw the optical flow needle map
      /// </summary>
      /// <param name="u"></param>
      /// <param name="v"></param>
      /// <param name="vertex"></param>
      /// <param name="colors"></param>
      public static void CreateOpticalFlowNeedleMap(CudaImage<Gray, float> u, CudaImage<Gray, float> v, GpuMat<float> vertex, GpuMat<float> colors)
      {
         CreateOpticalFlowNeedleMap(u, v, vertex, colors);
      }

      /// <summary>
      /// Performs linear blending of two images.
      /// </summary>
      /// <param name="img1">First image. Supports only CV_8U and CV_32F depth.</param>
      /// <param name="img2">Second image. Must have the same size and the same type as img1 .</param>
      /// <param name="weights1">Weights for first image. Must have tha same size as img1. Supports only CV_32F type.</param>
      /// <param name="weights2">Weights for second image. Must have tha same size as img2. Supports only CV_32F type.</param>
      /// <param name="result">Destination image.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "cudaBlendLinear")]
      public static extern void BlendLinear(IntPtr img1, IntPtr img2, IntPtr weights1, IntPtr weights2, IntPtr result, IntPtr stream);

      /// <summary>
      /// Applies bilateral filter to the image.
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image; should have the same size and the same type as src</param>
      /// <param name="kernelSize">The diameter of each pixel neighborhood, that is used during filtering.</param>
      /// <param name="sigmaColor">Filter sigma in the color space. Larger value of the parameter means that farther colors within the pixel neighborhood (see sigmaSpace) will be mixed together, resulting in larger areas of semi-equal color</param>
      /// <param name="sigmaSpace">Filter sigma in the coordinate space. Larger value of the parameter means that farther pixels will influence each other (as long as their colors are close enough; see sigmaColor). Then d&gt;0, it specifies the neighborhood size regardless of sigmaSpace, otherwise d is proportional to sigmaSpace.</param>
      /// <param name="borderType">Pixel extrapolation method, use DEFAULT for default</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or IntPtr.Zero to call the function synchronously (blocking).</param>  
      public static void BilateralFilter(IInputArray src, IOutputArray dst, int kernelSize, float sigmaColor, float sigmaSpace, CvEnum.BorderType borderType, Stream stream)
      {
         cudaBilateralFilter(src.InputArrayPtr, dst.OutputArrayPtr, kernelSize, sigmaColor, sigmaSpace, borderType, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaBilateralFilter(IntPtr src, IntPtr dst, int kernelSize, float sigmaColor, float sigmaSpace, CvEnum.BorderType borderType, IntPtr stream);


      /// <summary>
      /// Routines for correcting image color gamma
      /// </summary>
      /// <param name="src">Source image (3- or 4-channel 8 bit).</param>
      /// <param name="dst">Destination image.</param>
      /// <param name="forward">True for forward gamma correction or false for inverse gamma correction.</param>
      /// <param name="stream">Stream for the asynchronous version.</param>
      public static void GammaCorrection(IInputArray src, IOutputArray dst, bool forward = true, Stream stream = null)
      {
         cudaGammaCorrection(src.InputArrayPtr, dst.OutputArrayPtr, forward, stream);
      }
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cudaGammaCorrection(
         IntPtr src, 
         IntPtr dst, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool forward, 
         IntPtr stream);
   }
}
