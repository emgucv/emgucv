//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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

namespace Emgu.CV.OpenCL
{
   public enum OclDeviceType
   {
      Default = (1 << 0),
      Cpu = (1 << 1),
      Gpu = (1 << 2),
      Accelerator = (1 << 3),
      All = -1 //0xFFFFFFFF
   }

   /// <summary>
   /// This class wraps the functional calls to the opencv_ocl module
   /// </summary>
   public static partial class OclInvoke
   {
      static OclInvoke()
      {
         //Dummy code to make sure the static constructor of CvInvoke has been called and the error handler has been registered.
         CvInvoke.CV_MAKETYPE(0, 0);
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclGetDevice")]
      public static extern int GetDevice(OclDeviceType deviceType);


      /// <summary>
      /// Create an empty OclMat 
      /// </summary>
      /// <returns>Pointer to an empty OclMat</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCreateDefault")]
      internal static extern IntPtr OclMatCreateDefault();

      /// <summary>
      /// Create a GpuMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="type">The type of GpuMat</param>
      /// <returns>Pointer to the GpuMat</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCreate")]
      public static extern IntPtr OclMatCreate(int rows, int cols, int type);

      /// <summary>
      /// Convert a CvArr to an OclMat
      /// </summary>
      /// <param name="arr">Pointer to a CvArr</param>
      /// <returns>Pointer to the OclMat</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCreateFromArr")]
      public static extern IntPtr OclMatCreateFromArr(IntPtr arr);

      /// <summary>
      /// Check if the OclMat is empty
      /// </summary>
      /// <param name="oclMat">The OclMat</param>
      /// <returns>True if the OclMat is empty</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatIsEmpty")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool OclMatIsEmpty(IntPtr oclMat);

      /// <summary>
      /// Returns true iff the OclMat data is continuous
      /// (i.e. when there are no gaps between successive rows).
      /// </summary>
      /// <param name="oclMat">The OclMat to be checked</param>
      /// <returns>True if the OclMat is continuous</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatIsContinuous")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool OclMatIsContinuous(IntPtr oclMat);

      /// <summary>
      /// Release the OclMat
      /// </summary>
      /// <param name="mat">Pointer to the OclMat</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatRelease")]
      public static extern void OclMatRelease(ref IntPtr mat);

      /// <summary>
      /// Get the OclMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      /// <param name="oclMat">The OclMat</param>
      /// <returns>The size of the matrix</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatGetSize")]
      public static extern Size OclMatGetSize(IntPtr oclMat);

      /// <summary>
      /// Get the number of channels in the OclMat
      /// </summary>
      /// <param name="oclMat">The OclMat</param>
      /// <returns>The number of channels in the OclMat</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatGetChannels")]
      public static extern int OclMatGetChannels(IntPtr oclMat);

      /// <summary>
      /// Pefroms blocking upload data to OclMat.
      /// </summary>
      /// <param name="oclMat">The destination oclMat</param>
      /// <param name="arr">The CvArray to be uploaded to GPU</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatUpload")]
      public static extern void OclMatUpload(IntPtr oclMat, IntPtr arr);

      /// <summary>
      /// Downloads data from device to host memory. Blocking calls.
      /// </summary>
      /// <param name="oclMat">The source OclMat</param>
      /// <param name="arr">The CvArray where data will be downloaded to</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatDownload")]
      public static extern void OclMatDownload(IntPtr oclMat, IntPtr arr);

      /// <summary>
      /// Counts non-zero array elements
      /// </summary>
      /// <param name="src">The OclMat</param>
      /// <returns>The number of non-zero OclMat elements</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclCountNonZero")]
      public static extern int CountNonZero(IntPtr src);

      /// <summary>
      /// Adds one matrix to another (c = a + b).
      /// </summary>
      /// <param name="a">The first matrix to be added.</param>
      /// <param name="b">The second matrix to be added.</param>
      /// <param name="c">The sum of the two matrix</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatAdd")]
      public static extern void Add(IntPtr a, IntPtr b, IntPtr c, IntPtr mask);

      /// <summary>
      /// Adds scalar to a matrix (c = a + scalar)
      /// </summary>
      /// <param name="a">The matrix to be added.</param>
      /// <param name="scalar">The scalar to be added.</param>
      /// <param name="c">The sum of the matrix and the scalar</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatAddS")]
      public static extern void Add(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr mask);

      /// <summary>
      /// Subtracts one matrix from another (c = a - b).
      /// </summary>
      /// <param name="a">The matrix where subtraction take place</param>
      /// <param name="b">The matrix to be substracted</param>
      /// <param name="c">The result of a - b</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatSubtract")]
      public static extern void Subtract(IntPtr a, IntPtr b, IntPtr c, IntPtr mask);

      /// <summary>
      /// Computes element-wise weighted product of the two arrays (c = scale * a * b) 
      /// </summary>
      /// <param name="a">The matrix to be substraced from</param>
      /// <param name="scalar">The scalar to be substracted</param>
      /// <param name="c">The matrix substraced by the scalar</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatSubtractS")]
      public static extern void Subtract(IntPtr a, MCvScalar scalar, IntPtr c, IntPtr mask);

      /// <summary>
      /// Computes element-wise product of the two GpuMat: c = scale * a * b.
      /// </summary>
      /// <param name="a">The first OclMat to be element-wise multiplied.</param>
      /// <param name="b">The second OclMat to be element-wise multiplied.</param>
      /// <param name="c">The element-wise multiplication of the two OclMat</param>
      /// <param name="scale">The scale</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMultiply")]
      public static extern void Multiply(IntPtr a, IntPtr b, IntPtr c, double scale);

      /// <summary>
      /// Computes element-wise quotient of the two GpuMat (c = scale *  a / b).
      /// </summary>
      /// <param name="a">The first OclMat</param>
      /// <param name="b">The second OclMat</param>
      /// <param name="c">The element-wise quotient of the two OclMat</param>
      /// <param name="scale">The scale</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatDivide")]
      public static extern void Divide(IntPtr a, IntPtr b, IntPtr c, double scale);

      /// <summary>
      /// Flips the OclMat in one of different 3 ways (row and column indices are 0-based):
      /// dst(i,j)=src(rows(src)-i-1,j) if flip_mode = 0
      /// dst(i,j)=src(i,cols(src1)-j-1) if flip_mode &gt; 0
      /// dst(i,j)=src(rows(src)-i-1,cols(src)-j-1) if flip_mode &lt; 0
      /// </summary>
      /// <param name="src">Source OclMat.</param>
      /// <param name="dst">Destination OclMat.</param>
      /// <param name="flipMode">
      /// Specifies how to flip the GpuMat.
      /// flip_mode = 0 means flipping around x-axis, 
      /// flip_mode &gt; 0 (e.g. 1) means flipping around y-axis and 
      /// flip_mode &lt; 0 (e.g. -1) means flipping around both axises. 
      ///</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void oclMatFlip(IntPtr src, IntPtr dst, int flipMode);

      /// <summary>
      /// Flips the OclMat in one of different 3 ways (row and column indices are 0-based). 
      /// </summary>
      /// <param name="src">The source OclMat. Supports all types</param>
      /// <param name="dst">Destination OclMat. The same source and type as <paramref name="src"/></param>
      /// <param name="flipType">Specifies how to flip the OclMat.</param>
      public static void Flip(IntPtr src, IntPtr dst, CvEnum.FLIP flipType)
      {
         int flipMode =
            //-1 indicates vertical and horizontal flip
            flipType == (Emgu.CV.CvEnum.FLIP.HORIZONTAL | Emgu.CV.CvEnum.FLIP.VERTICAL) ? -1 :
            //1 indicates horizontal flip only
            flipType == Emgu.CV.CvEnum.FLIP.HORIZONTAL ? 1 :
            //0 indicates vertical flip only
            0;
         oclMatFlip(src, dst, flipMode);
      }

      /// <summary>
      /// Converts image from one color space to another
      /// </summary>
      /// <param name="src">The source oclMat</param>
      /// <param name="dst">The destination oclMat</param>
      /// <param name="code">The color conversion code</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCvtColor")]
      public static extern void CvtColor(IntPtr src, IntPtr dst, CvEnum.COLOR_CONVERSION code);

      /// <summary>
      /// Copy the source OclMat to destination OclMat, using an optional mask.
      /// </summary>
      /// <param name="src">The OclMat to be copied from</param>
      /// <param name="dst">The OclMat to be copied to</param>
      /// <param name="mask">The optional mask, use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCopy")]
      public static extern void Copy(IntPtr src, IntPtr dst, IntPtr mask);


      /// <summary>
      /// Resizes the image.
      /// </summary>
      /// <param name="src">The source image.</param>
      /// <param name="dst">The destination image.</param>
      /// <param name="interpolation">The interpolation type. Supports INTER_NEAREST, INTER_LINEAR.</param>
      /// <param name="fx">Use 0 for default</param>
      /// <param name="fy">Use 0 for default</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatResize")]
      public static extern void Resize(IntPtr src, IntPtr dst, double fx, double fy, CvEnum.INTER interpolation);


      /// <summary>
      /// Finds minimum and maximum element values and their positions. The extremums are searched over the whole OclMat or, if mask is not IntPtr.Zero, in the specified OclMat region.
      /// </summary>
      /// <param name="oclMat">The source OclMat, single-channel</param>
      /// <param name="minVal">Pointer to returned minimum value</param>
      /// <param name="maxVal">Pointer to returned maximum value</param>
      /// <param name="minLoc">Pointer to returned minimum location</param>
      /// <param name="maxLoc">Pointer to returned maximum location</param>
      /// <param name="mask">The optional mask that is used to select a subarray. Use IntPtr.Zero if not needed</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMinMaxLoc")]
      public static extern void MinMaxLoc(IntPtr oclMat,
         ref double minVal, ref double maxVal,
         ref Point minLoc, ref Point maxLoc,
         IntPtr mask);

      /// <summary>
      /// Copies each plane of a multi-channel OclMat to a dedicated OclMat
      /// </summary>
      /// <param name="src">The multi-channel gpuMat</param>
      /// <param name="dstArray">Pointer to an array of single channel GpuMat pointers</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatSplit")]
      public static extern void Split(IntPtr src, IntPtr dstArray);

      /// <summary>
      /// Makes multi-channel OclMat out of several single-channel OclMats
      /// </summary>
      /// <param name="srcArr">Pointer to an array of single channel OclMat pointers</param>
      /// <param name="dst">The multi-channel oclMat</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMerge")]
      public static extern void Merge(IntPtr srcArr, IntPtr dst);

      /// <summary>
      /// This function has several different purposes and thus has several synonyms. It copies one OclMat to another with optional scaling, which is performed first, and/or optional type conversion, performed after:
      /// dst(I)=src(I)*scale + (shift,shift,...)
      /// All the channels of multi-channel OclMats are processed independently.
      /// The type conversion is done with rounding and saturation, that is if a result of scaling + conversion can not be represented exactly by a value of destination OclMat element type, it is set to the nearest representable value on the real axis.
      /// In case of scale=1, shift=0 no prescaling is done. This is a specially optimized case and it has the appropriate convertTo synonym.
      /// </summary>
      /// <param name="src">Source OclMat</param>
      /// <param name="dst">Destination OclMat</param>
      /// <param name="scale">Scale factor</param>
      /// <param name="shift">Value added to the scaled source OclMat elements</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatConvertTo")]
      public static extern void ConvertTo(IntPtr src, IntPtr dst, double scale, double shift);

      #region Logical operators
      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of two OclMats:
      /// dst(I)=src1(I)^src2(I) if mask(I)!=0
      /// In the case of floating-point GpuMats their bit representations are used for the operation. All the OclMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source OclMat</param>
      /// <param name="src2">The second source OclMat</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="mask">Mask, 8-bit single channel OclMat; specifies elements of destination OclMat to be changed. Use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBitwiseXor")]
      public static extern void BitwiseXor(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of a OclMat and a scalar:
      /// dst(I)=src1(I)^scalar
      /// In the case of a floating-point OclMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source OclMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="mask">Mask, 8-bit single channel OclMat; specifies elements of destination OclMat to be changed. Use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBitwiseXorS")]
      public static extern void BitwiseXor(IntPtr src1, MCvScalar scalar, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical or of two OclMats:
      /// dst(I)=src1(I) | src2(I) if mask(I)!=0
      /// In the case of floating-point OclMats their bit representations are used for the operation. All the OclMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source OclMat</param>
      /// <param name="src2">The second source OclMat</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="mask">Mask, 8-bit single channel OclMat; specifies elements of destination OclMat to be changed. Use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBitwiseOr")]
      public static extern void BitwiseOr(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical or a OclMat and a scalar:
      /// dst(I)=src1(I) | scalar
      /// In the case of a floating-point OclMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source OclMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="mask">Mask, 8-bit single channel OclMat; specifies elements of destination OclMat to be changed. Use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBitwiseOrS")]
      public static extern void BitwiseOr(IntPtr src1, MCvScalar scalar, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical and of two OclMats:
      /// dst(I)=src1(I) &amp; src2(I) if mask(I)!=0
      /// In the case of floating-point OclMats their bit representations are used for the operation. All the OclMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src1">The first source OclMat</param>
      /// <param name="src2">The second source OclMat</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="mask">Mask, 8-bit single channel OclMat; specifies elements of destination OclMat to be changed. Use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBitwiseAnd")]
      public static extern void BitwiseAnd(IntPtr src1, IntPtr src2, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical and of a OclMat and a scalar:
      /// dst(I)=src1(I) &amp; scalar
      /// In the case of a floating-point OclMat its bit representation is used for the operation.
      /// </summary>
      /// <param name="src1">The first source OclMat</param>
      /// <param name="scalar">The scalar</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="mask">Mask, 8-bit single channel OclMat; specifies elements of destination OclMat to be changed. Use IntPtr.Zero if not needed.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBitwiseAndS")]
      public static extern void BitwiseAnd(IntPtr src1, MCvScalar scalar, IntPtr dst, IntPtr mask);

      /// <summary>
      /// Calculates per-element bit-wise logical not
      /// dst(I)=~src(I) if mask(I)!=0
      /// In the case of floating-point OclMats their bit representations are used for the operation. All the OclMats must have the same type, except the mask, and the same size
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The destination OclMat</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBitwiseNot")]
      public static extern void BitwiseNot(IntPtr src, IntPtr dst);
      #endregion
   }
}
