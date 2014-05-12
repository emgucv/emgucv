//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
﻿using System.Drawing;
﻿using System.Runtime.InteropServices;
using System.Text;
﻿using Emgu.CV.CvEnum;
﻿using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// Gaussian Mixture-based Background/Foreground Segmentation Algorithm.
   /// </summary>
   public class CudaBackgroundSubtractorMOG : UnmanagedObject
   {
      /// <summary>
      /// Create a Gaussian Mixture-based Background/Foreground Segmentation model
      /// </summary>
      public CudaBackgroundSubtractorMOG(int history = 200, int nMixtures = 4, double backgroundRatio = 0.7, double noiseSigma = 0)
      {
         _ptr = CudaInvoke.cudaBackgroundSubtractorMOGCreate(history, nMixtures, backgroundRatio, noiseSigma);
      }

      /// <summary>
      /// Updates the background model
      /// </summary>
      /// <param name="frame">Next video frame.</param>
      /// <param name="learningRate">The learning rate, use -1.0f for default value.</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      public void Update(IInputArray frame, float learningRate, IOutputArray forgroundMask, Stream stream = null)
      {

         CudaInvoke.cudaBackgroundSubtractorMOGApply(_ptr, frame.InputArrayPtr, learningRate, forgroundMask.OutputArrayPtr, stream);
      }

      /// <summary>
      /// Release all the unmanaged resource associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         CudaInvoke.cudaBackgroundSubtractorMOGRelease(ref _ptr);
      }
   }

   public static partial class CudaInvoke
   {
      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cudaBackgroundSubtractorMOGCreate(int history, int nMixtures, double backgroundRatio, double noiseSigma);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorMOGApply(IntPtr mog, IntPtr frame, float learningRate, IntPtr fgMask, IntPtr stream);

      [DllImport(CvInvoke.ExternCudaLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cudaBackgroundSubtractorMOGRelease(ref IntPtr mog);
   }
}
