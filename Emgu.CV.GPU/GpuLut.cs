//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   public class GpuLookUpTable : UnmanagedObject
   {
      /// <summary>
      /// Create the look up table
      /// </summary>
      /// <param name="lookUpTable">It should be either 1 or 3 channel matrix of 1x256</param>
      public GpuLookUpTable(Matrix<Byte> lookUpTable)
      {
         _ptr = GpuInvoke.gpuLookUpTableCreate(lookUpTable);
      }

      /// <summary>
      /// Transform the image using the lookup table
      /// </summary>
      /// <typeparam name="TColor">The type of color, should be either 3 channel or 1 channel</typeparam>
      /// <param name="image">The image to be transformed</param>
      /// <param name="dst">The transformation result</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Transform<TColor>(GpuImage<TColor, byte> image, GpuImage<TColor, byte> dst, Stream stream)
         where TColor : struct, IColor
      {
         GpuInvoke.gpuLookUpTableTransform(_ptr, image, dst, stream);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this look up table
      /// </summary>
      protected override void DisposeObject()
      {
         GpuInvoke.gpuLookUpTableRelease(ref _ptr);
      }
   }

   public static partial class GpuInvoke
   {
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr gpuLookUpTableCreate(IntPtr lut);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuLookUpTableTransform(IntPtr lut, IntPtr image, IntPtr dst, IntPtr stream);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void gpuLookUpTableRelease(ref IntPtr lut);
   }

   /*
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
*/
}
