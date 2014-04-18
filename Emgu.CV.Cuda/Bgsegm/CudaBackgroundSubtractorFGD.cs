//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   /// <typeparam name="TColor">The color type of the CudaImage to be processed</typeparam>
   public class CudaBackgroundSubtractorFGD<TColor> : UnmanagedObject
       where TColor : struct, IColor
   {
      private CudaImage<Gray, Byte> _forgroundMask;

      /// <summary>
      /// The forground mask
      /// </summary>
      public CudaImage<Gray, Byte> ForgroundMask
      {
         get
         {
            return _forgroundMask;
         }
      }

      /// <summary>
      /// Create a Background/Foreground Segmentation model
      /// </summary>
      public CudaBackgroundSubtractorFGD(
         int Lc,
         int N1c,
         int N2c,
         int Lcc,
         int N1cc,
         int N2cc,
         bool isObjWithoutHoles,
         int performMorphing,
         float alpha1,
         float alpha2,
         float alpha3,
         float delta,
         float T,
         float minArea)
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
      public void Update(CudaImage<TColor, Byte> frame, float learningRate)
      {
         if (_forgroundMask == null)
         {
            _forgroundMask = new CudaImage<Gray, byte>(frame.Size);
         }
         CudaInvoke.cudaBackgroundSubtractorFGDApply(_ptr, frame, learningRate, _forgroundMask);
      }

      /// <summary>
      /// Release all the managed resource associated with this object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (_forgroundMask != null)
         {
            _forgroundMask.Dispose();
         }
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
      internal static extern void cudaBackgroundSubtractorFGDApply(IntPtr fgd, IntPtr frame, float learningRate, IntPtr fgMask);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorFGDRelease(ref IntPtr fgd);
   }
}
