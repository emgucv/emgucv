//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
   /// <summary>
   /// The module brings implementation of the image processing algorithms based on fuzzy mathematics.
   /// </summary>
   public static partial class FuzzyInvoke
   {
      static FuzzyInvoke()
      {
         CvInvoke.Init();
      }

      /// <summary>
      /// Function type
      /// </summary>
      public enum Function
      {
         /// <summary>
         /// Linear
         /// </summary>
         Linear = 1,
         /// <summary>
         /// Sinus
         /// </summary>
         Sinus = 2
      }

      /// <summary>
      /// Inpaint algorithm
      /// </summary>
      public enum InpaintAlgorithm
      {
         /// <summary>
         /// One step algorithm.
         /// </summary>
         OneStep = 1,
         /// <summary>
         /// Algorithm automaticaly increasing radius of the basic function.
         /// </summary>
         MultiStep = 2,
         /// <summary>
         /// Iterative algorithm running in more steps using partial computations.
         /// </summary>
         Iterative = 3
      }

      /// <summary>
      /// Creates kernel from basic functions.
      /// </summary>
      /// <param name="A">Basic function used in axis x.</param>
      /// <param name="B">Basic function used in axis y.</param>
      /// <param name="kernel">Final 32-b kernel derived from A and B.</param>
      /// <param name="chn">Number of kernel channels.</param>
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
      internal static extern void cveFtCreateKernel(IntPtr A, IntPtr B, IntPtr kernel, int chn);

      /// <summary>
      /// Creates kernel from general functions.
      /// </summary>
      /// <param name="function">Function type</param>
      /// <param name="radius">Radius of the basic function.</param>
      /// <param name="kernel">Final 32-b kernel.</param>
      /// <param name="chn">Number of kernel channels.</param>
      public static void CreateKernel(Function function, int radius, IOutputArray kernel, int chn = 1)
      {
         using (OutputArray oaKernel = kernel.GetOutputArray())
            cveFtcreateKernelFromFunction(function, radius, oaKernel, chn);
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFtcreateKernelFromFunction(Function function, int radius, IntPtr kernel, int chn);

      /// <summary>
      /// Image inpainting.
      /// </summary>
      /// <param name="image">Input image.</param>
      /// <param name="mask">Mask used for unwanted area marking.</param>
      /// <param name="output">Output 32-bit image.</param>
      /// <param name="radius">Radius of the basic function.</param>
      /// <param name="function">Function type</param>
      /// <param name="algorithm">Algorithm type</param>
      public static void Inpaint(Mat image, Mat mask, Mat output, int radius = 2, Function function = Function.Linear, InpaintAlgorithm algorithm = InpaintAlgorithm.OneStep)
      {
         cveFtInpaint(image, mask, output, radius, function, algorithm);
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFtInpaint(IntPtr image, IntPtr mask, IntPtr output, int radius, Function function, InpaintAlgorithm algorithm);

      /// <summary>
      /// Image filtering.
      /// </summary>
      /// <param name="image">Input image.</param>
      /// <param name="kernel">Final 32-b kernel.</param>
      /// <param name="output">Output 32-bit image.</param>
      public static void Filter(Mat image, Mat kernel, Mat output)
      {
         cveFtFilter(image, kernel, output);
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveFtFilter(IntPtr image, IntPtr kernel, IntPtr output);

   }
}