//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Background/Foreground Segmentation Algorithm.
   /// </summary>
   public class CudaBackgroundSubtractorFGD : UnmanagedObject
   {

      /// <summary>
      /// Create a Background/Foreground Segmentation model
      /// </summary>
      /// <param name="Lc"></param>
      /// <param name="N1c"></param>
      /// <param name="N2c"></param>
      /// <param name="Lcc"></param>
      /// <param name="N1cc"></param>
      /// <param name="N2cc"></param>
      /// <param name="isObjWithoutHoles"></param>
      /// <param name="performMorphing"></param>
      /// <param name="alpha1">Background reference image update parameter</param>
      /// <param name="alpha2">Stat model update parameter. 0.002f ~ 1K frame(~45sec), 0.005 ~ 18sec (if 25fps and absolutely static BG)</param>
      /// <param name="alpha3">start value for alpha parameter (to fast initiate statistic model)</param>
      /// <param name="delta"></param>
      /// <param name="T"></param>
      /// <param name="minArea"></param>
      public CudaBackgroundSubtractorFGD(
         int Lc = 128,
         int N1c = 15,
         int N2c =25,
         int Lcc = 64,
         int N1cc = 25,
         int N2cc = 40,
         bool isObjWithoutHoles = true,
         int performMorphing = 1,
         float alpha1 = 0.1f,
         float alpha2 = 0.005f,
         float alpha3 = 0.1f,
         float delta = 2.0f,
         float T = 0.9f,
         float minArea = 15.0f)
      {
         _ptr = CudaInvoke.cudaBackgroundSubtractorFGDCreate(
            Lc,
            N1c,
            N2c,
            Lcc,
            N1cc,
            N2cc,
            isObjWithoutHoles,
            performMorphing,
            alpha1,
            alpha2,
            alpha3,
            delta,
            T,
            minArea);
      }

      /// <summary>
      /// Updates the background model
      /// </summary>
      /// <param name="frame">Next video frame.</param>
      /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
      public void Apply(IInputArray frame, IOutputArray forgroundMask, double learningRate =-1.0)
      {
         using (InputArray iaFrame = frame.GetInputArray())
         using (OutputArray oaForgroundMask = forgroundMask.GetOutputArray())
            CudaInvoke.cudaBackgroundSubtractorFGDApply(_ptr, iaFrame, oaForgroundMask, learningRate);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaBackgroundSubtractorFGDRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaBackgroundSubtractorFGDCreate(
         int Lc,
         int N1c,
         int N2c,
         int Lcc,
         int N1cc,
         int N2cc,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool isObjWithoutHoles,
         int performMorphing,
         float alpha1,
         float alpha2,
         float alpha3,
         float delta,
         float T,
         float minArea);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorFGDApply(IntPtr fgd, IntPtr frame, IntPtr fgMask, double learningRate);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorFGDRelease(ref IntPtr fgd);
   }
}
