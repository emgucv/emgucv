//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Fuzzy
{

   public static partial class FuzzyInvoke
   {
      static FuzzyInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      public enum Function
      {
         Linear = 1,
         Sinus = 2
      }

      public enum InpaintAlgorithm
      {
         OneStep = 1,
         MultiStep = 2,
         Iterative = 3
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFtCreateKernel(IntPtr A, IntPtr B, IntPtr kernel, int chn);
      public static void CreateKernel(IInputArray A, IInputArray B, IOutputArray kernel, int chn = 1)
      {
         using (InputArray iaA = A.GetInputArray())
         using (InputArray iaB = B.GetInputArray())
         using (OutputArray oaKernel = kernel.GetOutputArray())
         {
            cveFtCreateKernel(iaA, iaB, oaKernel, chn);
         }
      }


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFtcreateKernelFromFunction(Function function, int radius, IntPtr kernel, int chn);
      public static void CreateKernel(Function function, int radius, IOutputArray kernel, int chn = 1)
      {
         using (OutputArray oaKernel = kernel.GetOutputArray())
            cveFtcreateKernelFromFunction(function, radius, oaKernel, chn);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFtInpaint(IntPtr image, IntPtr mask, IntPtr output, int radius, Function function, InpaintAlgorithm algorithm);
      public static void Inpaint(Mat image, Mat mask, Mat output, int radius = 2, Function function = Function.Linear, InpaintAlgorithm algorithm = InpaintAlgorithm.OneStep)
      {
         cveFtInpaint(image, mask, output, radius, function, algorithm);
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFtFilter(IntPtr image, IntPtr kernel, IntPtr output);

      public static void Filter(Mat image, Mat kernel, Mat output)
      {
         cveFtFilter(image, kernel, output);
      }
   }
}