//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
   /// <summary>
   /// Image Stitching.
   /// </summary>
   public class Stitcher : UnmanagedObject
   {
      /// <summary>
      /// Creates a stitcher with the default parameters.
      /// </summary>
      /// <param name="tryUseGpu">If true, the stitcher will try to use GPU for processing when available</param>
      public Stitcher(bool tryUseGpu)
      {
         _ptr = StitchingInvoke.CvStitcherCreateDefault(tryUseGpu);
      }

      /// <summary>
      /// Compute the panoramic images given the images
      /// </summary>
      /// <param name="images">The input images. This can be, for example, a VectorOfMat</param>
      /// <param name="pano">The panoramic image</param>
      /// <returns>true if successful</returns>
      public bool Stitch(IInputArray images, IOutputArray pano)
      {
         return StitchingInvoke.CvStitcherStitch(_ptr, images.InputArrayPtr, pano.OutputArrayPtr);
      }

      /// <summary>
      /// Release memory associated with this stitcher
      /// </summary>
      protected override void DisposeObject()
      {
         StitchingInvoke.CvStitcherRelease(ref _ptr);
      }
   }

   internal static partial class StitchingInvoke
   {
      static StitchingInvoke()
      {
         //Dummy code to make sure the static constructor of GpuInvoke has been called
         bool hasCuda = CudaInvoke.HasCuda;
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvStitcherCreateDefault(
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool tryUseGpu
         );

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool CvStitcherStitch(IntPtr stitcherWrapper, IntPtr images, IntPtr pano);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvStitcherRelease(ref IntPtr stitcherWrapper);
   }
}
