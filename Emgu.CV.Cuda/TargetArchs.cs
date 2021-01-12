//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Gives information about what GPU archs this OpenCV GPU module was compiled for
   /// </summary>
   public static class TargetArchs
   {
      static TargetArchs()
      {
         //dummy code to make sure that the static constructor of GpuInvoke is called.
         bool hasCuda = CudaInvoke.HasCuda;
      }

      #region PInvoke
      /// <summary>
      /// Check if the GPU module is build with the specific feature set.
      /// </summary>
      /// <param name="featureSet">The feature set to be checked.</param>
      /// <returns>True if the GPU module is build with the specific feature set.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsBuildWith")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool BuildWith(CudaDeviceInfo.GpuFeature featureSet);

      /// <summary>
      /// Check if the GPU module is targeted for the specific device version
      /// </summary>
      /// <param name="major">The major version</param>
      /// <param name="minor">The minor version</param>
      /// <returns>True if the GPU module is targeted for the specific device version.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHas")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool Has(int major, int minor);

      /// <summary>
      /// Check if the GPU module is targeted for the specific PTX version
      /// </summary>
      /// <param name="major">The major version</param>
      /// <param name="minor">The minor version</param>
      /// <returns>True if the GPU module is targeted for the specific PTX version.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasPtx")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasPtx(int major, int minor);

      /// <summary>
      /// Check if the GPU module is targeted for the specific BIN version
      /// </summary>
      /// <param name="major">The major version</param>
      /// <param name="minor">The minor version</param>
      /// <returns>True if the GPU module is targeted for the specific BIN version.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasBin")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasBin(int major, int minor);

      /// <summary>
      /// Check if the GPU module is targeted for equal or less PTX version
      /// </summary>
      /// <param name="major">The major version</param>
      /// <param name="minor">The minor version</param>
      /// <returns>True if the GPU module is targeted for equal or less PTX version.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrLessPtx")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrLessPtx(int major, int minor);

      /// <summary>
      /// Check if the GPU module is targeted for equal or greater device version
      /// </summary>
      /// <param name="major">The major version</param>
      /// <param name="minor">The minor version</param>
      /// <returns>True if the GPU module is targeted for equal or greater device version.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrGreater")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrGreater(int major, int minor);

      /// <summary>
      /// Check if the GPU module is targeted for equal or greater PTX version
      /// </summary>
      /// <param name="major">The major version</param>
      /// <param name="minor">The minor version</param>
      /// <returns>True if the GPU module is targeted for equal or greater PTX version.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrGreaterPtx")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrGreaterPtx(int major, int minor);

      /// <summary>
      /// Check if the GPU module is targeted for equal or greater BIN version
      /// </summary>
      /// <param name="major">The major version</param>
      /// <param name="minor">The minor version</param>
      /// <returns>True if the GPU module is targeted for equal or greater BIN version.</returns>
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrGreaterBin")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrGreaterBin(int major, int minor);
      #endregion
   }
}
