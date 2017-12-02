//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   public class CudaMinEigenValCorner : CudaCornernessCriteria
   {     
      public CudaMinEigenValCorner(DepthType srcDepth, int srcChannels, int blockSize, int kSize, CvEnum.BorderType borderType = BorderType.Default)
      {
         _ptr = CudaInvoke.cudaCreateMinEigenValCorner(CvInvoke.MakeType(srcDepth, srcChannels), blockSize, kSize, borderType);
      }
   }


   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaCreateMinEigenValCorner(int srcType, int blockSize, int ksize, CvEnum.BorderType borderType);
   }
}
