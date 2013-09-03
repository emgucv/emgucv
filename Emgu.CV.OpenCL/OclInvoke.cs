//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
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
      public static extern int GetDevice(IntPtr oclInfoVector, OclDeviceType deviceType);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclSetDevice")]
      public static extern void SetDevice(IntPtr oclInfo, int deviceNum);

      /// <summary>
      /// Create an empty OclMat 
      /// </summary>
      /// <returns>Pointer to an empty OclMat</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCreateDefault")]
      internal static extern IntPtr OclMatCreateDefault();

      /// <summary>
      /// Create a OclMat of the specified size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="type">The type of OclMat</param>
      /// <returns>Pointer to the OclMat</returns>
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
      /// Copies scalar value to every selected element of the destination OclMat:
      /// arr(I)=value if mask(I)!=0
      /// </summary>
      /// <param name="mat">The destination OclMat</param>
      /// <param name="value">Fill value</param>
      /// <param name="mask">Operation mask, 8-bit single channel OclMat; specifies elements of destination OclMat to be changed. Can be IntPtr.Zero if not used</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatSetTo")]
      public static extern void OclMatSetTo(IntPtr mat, MCvScalar value, IntPtr mask);

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
      /// Get the OclMat type.
      /// </summary>
      /// <param name="oclMat">The OclMat</param>
      /// <returns>The type of the matrix</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatGetType")]
      public static extern int OclMatGetType(IntPtr oclMat);

      /// <summary>
      /// Get the OclMat size:
      /// width == number of columns, height == number of rows
      /// </summary>
      /// <param name="oclMat">The OclMat</param>
      /// <returns>The size of the matrix</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatGetSize")]
      public static extern Size OclMatGetSize(IntPtr oclMat);

      /// <summary>
      /// Get the OclMat wholeSize:
      /// width == wholecols, height == wholerows
      /// </summary>
      /// <param name="oclMat">The OclMat</param>
      /// <returns>The whole size of the matrix</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatGetWholeSize")]
      public static extern Size OclMatGetWholeSize(IntPtr oclMat);

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
      /// Computes element-wise product of the two OclMat: c = scale * a * b.
      /// </summary>
      /// <param name="a">The first OclMat to be element-wise multiplied.</param>
      /// <param name="b">The second OclMat to be element-wise multiplied.</param>
      /// <param name="c">The element-wise multiplication of the two OclMat</param>
      /// <param name="scale">The scale</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMultiply")]
      public static extern void Multiply(IntPtr a, IntPtr b, IntPtr c, double scale);

      /// <summary>
      /// Multiplies OclMat to a scalar (c = a * scalar).
      /// </summary>
      /// <param name="a">The first OclMat to be element-wise multiplied.</param>
      /// <param name="scalar">The scalar to be multiplied</param>
      /// <param name="c">The result of the OclMat mutiplied by the scalar</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMultiplyS")]
      public static extern void Multiply(IntPtr a, double scalar, IntPtr c);

      /// <summary>
      /// Computes element-wise quotient of the two OclMat (c = scale *  a / b).
      /// </summary>
      /// <param name="a">The first OclMat</param>
      /// <param name="b">The second OclMat</param>
      /// <param name="c">The element-wise quotient of the two OclMat</param>
      /// <param name="scale">The scale</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatDivide")]
      public static extern void Divide(IntPtr a, IntPtr b, IntPtr c, double scale);

      /// <summary>
      /// Computes element-wise weighted reciprocal of an array (c = scale/ b).
      /// </summary>
      /// <param name="b">The second OclMat to be element-wise divided.</param>
      /// <param name="scalar">The first scalar to be divided</param>
      /// <param name="c">The result of the scalar dividing the OclMat</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatDivideSL")]
      public static extern void Divide(double scalar, IntPtr b, IntPtr c);

      /// <summary>
      /// Computes the weighted sum of two arrays (dst = alpha*src1 + beta*src2 + gamma)
      /// </summary>
      /// <param name="src1">The first source OclMat</param>
      /// <param name="alpha">The weight for <paramref name="src1"/></param>
      /// <param name="src2">The second source OclMat</param>
      /// <param name="beta">The weight for <paramref name="src2"/></param>
      /// <param name="gamma">The constant to be added</param>
      /// <param name="dst">The result</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatAddWeighted")]
      public static extern void AddWeighted(IntPtr src1, double alpha, IntPtr src2, double beta, double gamma, IntPtr dst);

      /// <summary>
      /// Computes element-wise absolute difference of two OclMats (c = abs(a - b)).
      /// </summary>
      /// <param name="a">The first OclMat</param>
      /// <param name="b">The second OclMat</param>
      /// <param name="c">The result of the element-wise absolute difference.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatAbsdiff")]
      public static extern void Absdiff(IntPtr a, IntPtr b, IntPtr c);

      /// <summary>
      /// Computes element-wise absolute difference of OclMat and scalar (c = abs(a - s)).
      /// </summary>
      /// <param name="a">An OclMat</param>
      /// <param name="scalar">A scalar</param>
      /// <param name="c">The result of the element-wise absolute difference.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatAbsdiffS")]
      public static extern void Absdiff(IntPtr a, MCvScalar scalar, IntPtr c);

      /// <summary>
      /// Flips the OclMat in one of different 3 ways (row and column indices are 0-based):
      /// dst(i,j)=src(rows(src)-i-1,j) if flip_mode = 0
      /// dst(i,j)=src(i,cols(src1)-j-1) if flip_mode &gt; 0
      /// dst(i,j)=src(rows(src)-i-1,cols(src)-j-1) if flip_mode &lt; 0
      /// </summary>
      /// <param name="src">Source OclMat.</param>
      /// <param name="dst">Destination OclMat.</param>
      /// <param name="flipMode">
      /// Specifies how to flip the OclMat.
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
      /// Compares elements of two OclMats (c = a &lt;cmpop&gt; b).
      /// Supports all types except CV_8SC1,CV_8SC2,CV8SC3,CV_8SC4 types
      /// </summary>
      /// <param name="a">The first OclMat</param>
      /// <param name="b">The second OclMat</param>
      /// <param name="c">The result of the comparison.</param>
      /// <param name="cmpop">The type of comparison</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCompare")]
      public static extern void Compare(IntPtr a, IntPtr b, IntPtr c, CvEnum.CMP_TYPE cmpop);

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
      /// This function is similiar to cvCalcBackProjectPatch. It slids through image, compares overlapped patches of size wxh with templ using the specified method and stores the comparison results to result
      /// </summary>
      /// <param name="image">Image where the search is running. It should be 8-bit or 32-bit floating-point</param>
      /// <param name="templ">Searched template; must be not greater than the source image and the same data type as the image</param>
      /// <param name="result">A map of comparison results; single-channel 32-bit floating-point. If image is WxH and templ is wxh then result must be W-w+1xH-h+1.</param>
      /// <param name="method">Specifies the way the template must be compared with image regions </param>
      /// <param name="oclMatchTemplateBuf">Pointer to OclMatchTemplateBuf</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMatchTemplate")]
      public static extern void MatchTemplate(IntPtr image, IntPtr templ, IntPtr result, CvEnum.TM_TYPE method, IntPtr oclMatchTemplateBuf);

      /// <summary>
      /// Performs downsampling step of Gaussian pyramid decomposition. 
      /// </summary>
      /// <param name="src">The source GpuImage.</param>
      /// <param name="dst">The destination GpuImage, should have 2x smaller width and height than the source.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatPyrDown")]
      public static extern void PyrDown(IntPtr src, IntPtr dst);

      /// <summary>
      /// Performs up-sampling step of Gaussian pyramid decomposition.
      /// </summary>
      /// <param name="src">The source GpuImage.</param>
      /// <param name="dst">The destination image, should have 2x smaller width and height than the source.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatPyrUp")]
      public static extern void PyrUp(IntPtr src, IntPtr dst);

      /// <summary>
      /// Computes mean value and standard deviation
      /// </summary>
      /// <param name="mtx">The OclMat. Supports only CV_8UC1 type</param>
      /// <param name="mean">The mean value</param>
      /// <param name="stddev">The standard deviation</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMeanStdDev")]
      public static extern void MeanStdDev(IntPtr mtx, ref MCvScalar mean, ref MCvScalar stddev);

      /// <summary>
      /// Computes norm of the difference between two OclMats
      /// </summary>
      /// <param name="src1">The OclMat. Supports only CV_8UC1 type</param>
      /// <param name="src2">If IntPtr.Zero, norm operation is apply to <paramref name="src1"/> only. Otherwise, this is the OclMat of type CV_8UC1</param>
      /// <param name="normType">The norm type. Supports NORM_INF, NORM_L1, NORM_L2.</param>
      /// <returns>The norm of the <paramref name="src1"/> if <paramref name="src2"/> is IntPtr.Zero. Otherwise the norm of the difference between two OclMats.</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatNorm")]
      public static extern double Norm(IntPtr src1, IntPtr src2, Emgu.CV.CvEnum.NORM_TYPE normType);

      /// <summary>
      /// Transforms 8-bit unsigned integers using lookup table: dst(i)=lut(src(i)).
      /// Destination OclMat will have the depth type as lut and the same channels number as source.
      /// Supports CV_8UC1, CV_8UC3 types.
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="lut">The OclMat that contains the look up table</param>
      /// <param name="dst">The destination OclMat</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatLUT")]
      public static extern void LUT(IntPtr src, IntPtr lut, IntPtr dst);

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
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCopyMakeBorder")]
      public static extern void CopyMakeBorder(IntPtr src, IntPtr dst, int top, int bottom, int left, int right, CvEnum.BORDER_TYPE borderType, MCvScalar value);

      /// <summary>
      /// Computes the integral image and integral for the squared image
      /// </summary>
      /// <param name="src">The source OclMat, supports only CV_8UC1 source type</param>
      /// <param name="sum">The sum OclMat, supports only CV_32S source type, but will contain unsigned int values.</param>
      /// <param name="sqrSum">The sqsum OclMat, supports only CV32F source type. Use IntPtr.Zero if you don't want the sqrSum to be computed.</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatIntegral")]
      public static extern void Integral(IntPtr src, IntPtr sum, IntPtr sqrSum);

      /// <summary>
      /// Runs the Harris edge detector on image. Similarly to cvCornerMinEigenVal and cvCornerEigenValsAndVecs, for each pixel it calculates 2x2 gradient covariation matrix M over block_size x block_size neighborhood. Then, it stores
      /// det(M) - k*trace(M)^2
      /// to the destination image. Corners in the image can be found as local maxima of the destination image.
      /// </summary>
      /// <param name="image">Input OclMat</param>
      /// <param name="harrisResponce">OclMat to store the Harris detector responces. Should have the same size as <paramref name="image"/>. </param>
      /// <param name="blockSize">Neighborhood size </param>
      /// <param name="kSize"></param>
      /// <param name="k">Harris detector free parameter.</param>
      /// <param name="borderType">Boreder type, use REFLECT101 for default</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCornerHarris")]
      public static extern void CornerHarris(IntPtr image, IntPtr harrisResponce, int blockSize, int kSize, double k, CvEnum.BORDER_TYPE borderType);


      /// <summary>
      /// Applies bilateral filter to the image. Supports 8UC1 8UC4 data types.
      /// </summary>
      /// <param name="src">The source image</param>
      /// <param name="dst">The destination image; will have the same size and the same type as src</param>
      /// <param name="d">The diameter of each pixel neighborhood, that is used during filtering. If it is non-positive, it’s computed from sigmaSpace</param>
      /// <param name="sigmaColor">Filter sigma in the color space. Larger value of the parameter means that farther colors within the pixel neighborhood (see sigmaSpace) will be mixed together, resulting in larger areas of semi-equal color</param>
      /// <param name="sigmaSpave">Filter sigma in the coordinate space. Larger value of the parameter means that farther pixels will influence each other (as long as their colors are close enough; see sigmaColor). Then d&gt;0, it specifies the neighborhood size regardless of sigmaSpace, otherwise d is proportional to sigmaSpace.</param>
      /// <param name="borderType">Pixel extrapolation method, use DEFAULT for default</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatBilateralFilter")]
      public static extern void BilateralFilter(IntPtr src, IntPtr dst, int d, double sigmaColor, double sigmaSpave, CvEnum.BORDER_TYPE borderType);

      /// <summary>
      /// Computes exponent of each matrix element (b = exp(a))
      /// </summary>
      /// <param name="src">The source OclMat. Supports single channel float type.</param>
      /// <param name="dst">The resulting OclMat</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatExp")]
      public static extern void Exp(IntPtr src, IntPtr dst);

      /// <summary>
      /// Computes natural logarithm of absolute value of each matrix element: b = log(abs(a))
      /// </summary>
      /// <param name="src">The source OclMat. Supports single channel float type.</param>
      /// <param name="dst">The resulting OclMat</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatLog")]
      public static extern void Log(IntPtr src, IntPtr dst);

      /// <summary>
      /// The function pow raises every element of the input array to <paramref name="power"/>. Supports only CV_32FC1 and CV_64FC1 data type.
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="power">The power</param>
      /// <param name="dst">The resulting OclMat, should be the same type as the source</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatPow")]
      public static extern void Pow(IntPtr src, double power, IntPtr dst);

      /// <summary>
      /// Calculates the magnitude and angle of 2d vectors. Supports only CV_32F and CV_64F data types.
      /// </summary>
      /// <param name="x">The source OclMat of x-coordinates; must be single-precision or double-precision floating-point array</param>
      /// <param name="y">The source OclMat of y-coordinates; it must have the same size and same type as x</param>
      /// <param name="magnitude">The destination array of magnitudes of the same size and same type as x</param>
      /// <param name="angle">The destination array of angles of the same size and same type as x. The angles are measured in radians (0 to 2pi ) or in degrees (0 to 360 degrees).</param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians, which is default mode, or in degrees</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCartToPolar")]
      public static extern void CartToPolar(
         IntPtr x, IntPtr y, IntPtr magnitude, IntPtr angle,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool angleInDegrees);

      /// <summary>
      /// The function polarToCart computes the cartesian coordinates of each 2D vector represented by the corresponding elements of magnitude and angle. Supports only CV_32F and CV_64F data types.
      /// </summary>
      /// <param name="magnitude">he source floating-point array of magnitudes of 2D vectors. It can be an empty matrix (=Mat()) - in this case the function assumes that all the magnitudes are =1. If it’s not empty, it must have the same size and same type as angle</param>
      /// <param name="angle">The source floating-point array of angles of the 2D vectors</param>
      /// <param name="x">The destination array of x-coordinates of 2D vectors; will have the same size and the same type as angle</param>
      /// <param name="y">The destination array of y-coordinates of 2D vectors; will have the same size and the same type as angle</param>
      /// <param name="angleInDegrees">The flag indicating whether the angles are measured in radians, which is default mode, or in degrees</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatPolarToCart")]
      public static extern void PolarToCart(
         IntPtr magnitude, IntPtr angle, IntPtr x, IntPtr y,
         [MarshalAs(CvInvoke.BoolMarshalType)] 
         bool angleInDegrees);

      /// <summary>
      /// Calculates histogram of one or more arrays. 
      /// </summary>
      /// <param name="src">Source historgram array. Supports only 8UC1 data type.</param>
      /// <param name="hist">The output histogram, only 256 bins is supported now</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCalcHist")]
      public static extern void CalcHist(IntPtr src, IntPtr hist);

      /// <summary>
      /// The algorithm normalizes brightness and increases contrast of the image
      /// </summary>
      /// <param name="src">The source OclMat. only 8UC1 is supported now</param>
      /// <param name="dst">The destination OclMat, only 256 bins is supported now</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatEqualizeHist")]
      public static extern void EqualizeHist(IntPtr src, IntPtr dst);

      /// <summary>
      /// Copies each plane of a multi-channel OclMat to a dedicated OclMat
      /// </summary>
      /// <param name="src">The multi-channel OclMat</param>
      /// <param name="dstArray">Pointer to an array of single channel OclMat pointers</param>
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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatConvertTo")]
      public static extern void ConvertTo(IntPtr src, IntPtr dst, double scale, double shift);

      #region Logical operators
      /// <summary>
      /// Calculates per-element bit-wise logical conjunction of two OclMats:
      /// dst(I)=src1(I)^src2(I) if mask(I)!=0
      /// In the case of floating-point OclMats their bit representations are used for the operation. All the OclMats must have the same type, except the mask, and the same size
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

      #region filters
      /// <summary>
      /// Applies arbitrary linear filter to the image. In-place operation is supported. When the aperture is partially outside the image, the function interpolates outlier pixel values from the nearest pixels that is inside the image
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The destination OclImage</param>
      /// <param name="kernel">Convolution kernel, single-channel floating point matrix (e.g. Emgu.CV.Matrix). If you want to apply different kernels to different channels, split the ocl image into separate color planes and process them individually</param>
      /// <param name="anchor">The anchor of the kernel that indicates the relative position of a filtered point within the kernel. The anchor shoud lie within the kernel. The special default value (-1,-1) means that it is at the kernel center</param>
      /// <param name="borderType">Border type. Use REFLECT101 for default.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatFilter2D")]
      public static extern void Filter2D(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, CvEnum.BORDER_TYPE borderType);

      /// <summary>
      /// Applies generalized Sobel operator to the image
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The resulting OclMat</param>
      /// <param name="dx">Order of the derivative x</param>
      /// <param name="dy">Order of the derivative y</param>
      /// <param name="ksize">Size of the extended Sobel kernel</param>
      /// <param name="scale">Optional scale, use 1 for default.</param>
      /// <param name="borderType">The border type.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatSobel")]
      public static extern void Sobel(IntPtr src, IntPtr dst, int dx, int dy, IntPtr buffer, int ksize, double scale, CvEnum.BORDER_TYPE borderType);

      /// <summary>
      /// Applies Laplacian operator to the OclMat
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The resulting OclMat</param>
      /// <param name="ksize">Either 1 or 3</param>
      /// <param name="scale">Optional scale. Use 1.0 for default</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatLaplacian")]
      public static extern void Laplacian(IntPtr src, IntPtr dst, int ksize, double scale);

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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatGemm")]
      public static extern void Gemm(
         IntPtr src1,
         IntPtr src2,
         double alpha,
         IntPtr src3,
         double beta,
         IntPtr dst,
         CvEnum.GEMM_TYPE tABC);

      /// <summary>
      /// Smooths the OclMat using Gaussian filter.
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The smoothed OclMat</param>
      /// <param name="ksize">The size of the kernel</param>
      /// <param name="sigma1">This parameter may specify Gaussian sigma (standard deviation). If it is zero, it is calculated from the kernel size.</param>
      /// <param name="sigma2">In case of non-square Gaussian kernel the parameter may be used to specify a different (from param3) sigma in the vertical direction. Use 0 for default</param>
      /// <param name="borderType">The border type.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatGaussianBlur")]
      public static extern void GaussianBlur(IntPtr src, IntPtr dst, Size ksize, double sigma1, double sigma2, CvEnum.BORDER_TYPE borderType);

      /// <summary>
      /// Finds the edges on the input <paramref name="image"/> and marks them in the output image edges using the Canny algorithm. The smallest of threshold1 and threshold2 is used for edge linking, the largest - to find initial segments of strong edges.
      /// </summary>
      /// <param name="image">Input image</param>
      /// <param name="edges">Image to store the edges found by the function</param>
      /// <param name="lowThreshold">The first threshold</param>
      /// <param name="highThreshold">The second threshold</param>
      /// <param name="apertureSize">Aperture parameter for Sobel operator, use 3 for default</param>
      /// <param name="L2gradient">Use false for default</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatCanny")]
      public static extern void Canny(
         IntPtr image,
         IntPtr edges,
         double lowThreshold,
         double highThreshold,
         int apertureSize,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool L2gradient);
      #endregion

      /// <summary>
      /// Reshape the src OclMat  
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The resulting OclMat, as input it should be an empty OclMat.</param>
      /// <param name="cn">The new number of channels</param>
      /// <param name="rows">The new number of rows</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatReshape")]
      public static extern void OclMatReshape(IntPtr src, IntPtr dst, int cn, int rows);

      /// <summary>
      /// Finds circles in a grayscale image using the Hough transform.
      /// </summary>
      /// <param name="src"> 8-bit, single-channel grayscale input image.</param>
      /// <param name="circles">Output vector of found circles. Each vector is encoded as a 3-element floating-point vector (x, y, radius)</param>
      /// <param name="method">Detection method to use. Currently, the only implemented method is CV_HOUGH_GRADIENT , which is basically 21HT</param>
      /// <param name="dp">Inverse ratio of the accumulator resolution to the image resolution. For example, if dp=1 , the accumulator has the same resolution as the input image. If dp=2 , the accumulator has half as big width and height.</param>
      /// <param name="minDist">Minimum distance between the centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed.</param>
      /// <param name="cannyThreshold">The higher threshold of the two passed to the ocl::Canny() edge detector (the lower one is twice smaller).</param>
      /// <param name="votesThreshold"> The accumulator threshold for the circle centers at the detection stage. The smaller it is, the more false circles may be detected.</param>
      /// <param name="minRadius">Minimum circle radius.</param>
      /// <param name="maxRadius">Maximum circle radius.</param>
      /// <param name="maxCircles">Maximum number of output circles.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatHoughCircles")]
      public static extern void HoughCircles(IntPtr src, IntPtr circles, CvEnum.HOUGH_TYPE method, float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles);

      /// <summary>
      /// Finds circles in a grayscale image using the Hough transform.
      /// </summary>
      /// <param name="src">8-bit, single-channel grayscale input image.</param>
      /// <param name="dp">Inverse ratio of the accumulator resolution to the image resolution. For example, if dp=1 , the accumulator has the same resolution as the input image. If dp=2 , the accumulator has half as big width and height.</param>
      /// <param name="minDist">Minimum distance between the centers of the detected circles. If the parameter is too small, multiple neighbor circles may be falsely detected in addition to a true one. If it is too large, some circles may be missed.</param>
      /// <param name="cannyThreshold">The higher threshold of the two passed to the ocl::Canny() edge detector (the lower one is twice smaller).</param>
      /// <param name="votesThreshold"> The accumulator threshold for the circle centers at the detection stage. The smaller it is, the more false circles may be detected.</param>
      /// <param name="minRadius">Minimum circle radius.</param>
      /// <param name="maxRadius">Maximum circle radius.</param>
      /// <param name="maxCircles">Maximum number of output circles.</param>
      public static CircleF[] HoughCircles(OclImage<Gray, Byte> src, float dp, float minDist, int cannyThreshold, int votesThreshold, int minRadius, int maxRadius, int maxCircles)
      {
         OclMat<float> oclCircles = new OclMat<float>();

         OclInvoke.HoughCircles(src, oclCircles, CvEnum.HOUGH_TYPE.CV_HOUGH_GRADIENT, 1, 10, 100, 50, 1, 30, 1000);

         if (oclCircles.IsEmpty || oclCircles.Size.Width == 0)
         {
            return new CircleF[0];
         }

         int wholeCount = oclCircles.WholeSize.Width;

         CircleF[] circles = new CircleF[wholeCount];
         GCHandle handle = GCHandle.Alloc(circles, GCHandleType.Pinned);

         using (Matrix<float> circleMat = new Matrix<float>(1, wholeCount, 3, handle.AddrOfPinnedObject(), wholeCount * 3 * sizeof(float)))
         {
            oclCircles.Download(circleMat);
         }
         handle.Free();
         Array.Resize(ref circles, oclCircles.Size.Width);
         return circles;
      }

      /*
      /// <summary>
      /// Download hugh circles 
      /// </summary>
      /// <param name="detectedCirclesOclMat">The oclMat that contains the hough circles</param>
      /// <param name="houghCirclesCvMat">The cv::Mat where the circles will be downloaded to</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatHoughCirclesDownload")]
      public static extern void HoughCirclesDownload(IntPtr detectedCirclesOclMat, IntPtr houghCirclesCvMat);
      */

      #region morphology operation
      /// <summary>
      /// Erodes the image (applies the local minimum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      public static void Erode(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, int iterations)
      {
         Erode(src, dst, kernel, anchor, iterations, CvEnum.BORDER_TYPE.CONSTANT, new MCvScalar(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue));
      }

      /// <summary>
      /// Erodes the image (applies the local minimum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source OclMat</param>
      /// <param name="dst">The destination OclMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="bordertype">Type of the border to create around the copied source image rectangle</param>
      /// <param name="borderValue">Value of the border pixels if bordertype=CONSTANT</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatErode")]
      private static extern void Erode(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, int iterations, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue);

      /// <summary>
      /// Dilate the image (applies the local maximum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      public static void Dilate(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, int iterations)
      {
         Dilate(src, dst, kernel, anchor, iterations, CvEnum.BORDER_TYPE.CONSTANT, new MCvScalar(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue));
      }

      /// <summary>
      /// Dilate the image (applies the local maximum operator).
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. If it is IntPtr.Zero, a 3x3 rectangular structuring element is used.</param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="bordertype">Type of the border to create around the copied source image rectangle</param>
      /// <param name="borderValue">Value of the border pixels if bordertype=CONSTANT</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatDilate")]
      private static extern void Dilate(IntPtr src, IntPtr dst, IntPtr kernel, Point anchor, int iterations, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue);

      /// <summary>
      /// Applies an advanced morphological operation to the image
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="op">The type of morphological operation</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. </param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      public static void MorphologyEx(IntPtr src, IntPtr dst, CvEnum.CV_MORPH_OP op, IntPtr kernel, Point anchor, int iterations)
      {
         MorphologyEx(src, dst, op, kernel, anchor, iterations, CvEnum.BORDER_TYPE.CONSTANT, new MCvScalar(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue));
      }

      /// <summary>
      /// Applies an advanced morphological operation to the image
      /// Supports CV_8UC1, CV_8UC4 type.
      /// </summary>
      /// <param name="src">The source GpuMat</param>
      /// <param name="dst">The destination GpuMat</param>
      /// <param name="op">The type of morphological operation</param>
      /// <param name="kernel">The morphology kernel, pointer to an CvArr. </param>
      /// <param name="anchor">The center of the kernel. User (-1, -1) for the default kernel center.</param>
      /// <param name="iterations">The number of iterations morphology is applied</param>
      /// <param name="bordertype">Type of the border to create around the copied source image rectangle</param>
      /// <param name="borderValue">Value of the border pixels if bordertype=CONSTANT</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMorphologyEx")]
      public static extern void MorphologyEx(IntPtr src, IntPtr dst, CvEnum.CV_MORPH_OP op, IntPtr kernel, Point anchor, int iterations, CvEnum.BORDER_TYPE borderType, MCvScalar borderValue);
      #endregion

      #region meanshift
      /// <summary>
      /// Performs mean-shift filtering for each point of the source image. It maps each point of the source
      /// image into another point, and as the result we have new color and new position of each point.
      /// </summary>
      /// <param name="src">Source OclImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Destination OclImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      public static void MeanShiftFiltering(OclImage<Bgra, Byte> src, OclImage<Bgra, Byte> dst, int sp, int sr, MCvTermCriteria criteria)
      {
         Debug.Assert(src.Size.Equals(dst.Size));
         oclMatMeanShiftFiltering(src, dst, sp, sr, ref criteria);
      }

      /// <summary>
      /// Performs mean-shift filtering for each point of the source image. It maps each point of the source
      /// image into another point, and as the result we have new color and new position of each point.
      /// </summary>
      /// <param name="src">Source OclImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Destination OclImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMeanShiftFiltering")]
      private static extern void oclMatMeanShiftFiltering(IntPtr src, IntPtr dst, int sp, int sr, ref MCvTermCriteria criteria);

      /// <summary>
      /// Performs mean-shift procedure and stores information about processed points (i.e. their colors
      /// and positions) into two images.
      /// </summary>
      /// <param name="src">Source OclImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dstr">Destination OclImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="dstsp">Destination OclImage, containing position of mapped points. Will have the same size as src and CV 16SC2 type.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      public static void MeanShiftProc(OclImage<Bgra, Byte> src, OclImage<Bgra, Byte> dstr, OclMat<Int16> dstsp, int sp, int sr, MCvTermCriteria criteria)
      {
         Debug.Assert(src.Size.Equals(dstr.Size) && dstr.Size.Equals(dstsp.Size) && dstsp.NumberOfChannels == 2);
         oclMatMeanShiftProc(src, dstr, dstsp, sp, sr, ref criteria);
      }

      /// <summary>
      /// Performs mean-shift procedure and stores information about processed points (i.e. their colors
      /// and positions) into two images.
      /// </summary>
      /// <param name="src">Source OclImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dstr">Destination OclImage, containing color of mapped points. Will have the same size and type as src.</param>
      /// <param name="dstsp">Destination OclImage, containing position of mapped points. Will have the same size as src and CV 16SC2 type.</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="criteria">Termination criteria.</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMeanShiftProc")]
      private static extern void oclMatMeanShiftProc(IntPtr src, IntPtr dstr, IntPtr dstsp, int sp, int sr, ref MCvTermCriteria criteria);

      /// <summary>
      /// Performs mean-shift segmentation of the source image and eleminates small segments.
      /// </summary>
      /// <param name="src">Source OclImage. </param>
      /// <param name="dst">Segmented Image. Will have the same size and type as src. Note that this is an Image type and not OclImage type</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="minsize">Minimum segment size. Smaller segements will be merged.</param>
      /// <param name="criteria">Termination criteria.</param>
      public static void MeanShiftSegmentation(OclImage<Bgra, Byte> src, Image<Bgra, Byte> dst, int sp, int sr, int minsize, MCvTermCriteria criteria)
      {
         Debug.Assert(src.Size.Equals(dst.Size), "size of Image does not match that of OclImage");
         oclMatMeanShiftSegmentation(src, dst, sp, sr, minsize, ref criteria);
         CvInvoke.cvOrS(dst, new MCvScalar(0, 0, 0, 255), dst, IntPtr.Zero);
      }

      /// <summary>
      /// Performs mean-shift segmentation of the source image and eleminates small segments.
      /// </summary>
      /// <param name="src">Source OclImage. Only CV 8UC4 images are supported for now.</param>
      /// <param name="dst">Segmented Image. Will have the same size and type as src. Note that this is an Image type and not OclImage type</param>
      /// <param name="sp">Spatial window radius.</param>
      /// <param name="sr">Color window radius.</param>
      /// <param name="minsize">Minimum segment size. Smaller segements will be merged.</param>
      /// <param name="criteria">Termination criteria.</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "oclMatMeanShiftSegmentation")]
      private static extern void oclMatMeanShiftSegmentation(IntPtr src, IntPtr dst, int sp, int sr, int minsize, ref MCvTermCriteria criteria);
      #endregion
   }
}
