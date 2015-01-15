//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV
{
   public static partial class CvInvoke
   {
      public static void BalanceWhite(Mat src, Mat dst, CvEnum.WhiteBalanceMethod algorithmType, float inputMin = 0f, float inputMax = 255f, float outputMin = 0f, float outputMax = 255f)
      {
         cveBalanceWhite(src, dst, algorithmType, inputMin, inputMax, outputMin, outputMax);  
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveBalanceWhite(IntPtr src, IntPtr dst, CvEnum.WhiteBalanceMethod algorithmType, float inputMin, float inputMax, float outputMin, float outputMax);

      public static void cveDctDenoising(Mat src, Mat dst, double sigma, int psize = 16)
      {
         cveDctDenoising(src, dst, sigma, psize);
      }

      [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void cveDctDenoising(IntPtr src, IntPtr dst, double sigma, int psize);

   }
}
