//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// Gives information about what GPU archs this OpenCV GPU module was compiled for
   /// </summary>
   public static class TargetArchs
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsBuildWith")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool BuildWith(GpuDeviceInfo.GpuFeature featureSet);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHas")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool Has(int major, int minor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasPtx")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasPtx(int major, int minor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasBin")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasBin(int major, int minor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrLessPtx")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrLessPtx(int major, int minor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrGreater")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrGreater(int major, int minor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrGreaterPtx")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrGreaterPtx(int major, int minor);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention, EntryPoint = "targetArchsHasEqualOrGreaterBin")]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      public static extern bool HasEqualOrGreaterBin(int major, int minor);
      #endregion
   }
}
