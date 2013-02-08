//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Emgu.CV.GPU;
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
      /// <param name="images">The input images</param>
      /// <returns>The panoramic image</returns>
      public Image<Bgr, Byte> Stitch(Image<Bgr, Byte>[] images)
      {
         IntPtr[] ptrs = new IntPtr[images.Length];
         for (int i = 0; i < images.Length; ++i)
            ptrs[i] = images[i].Ptr;

         GCHandle handle = GCHandle.Alloc(ptrs, GCHandleType.Pinned);
         IntPtr resultIplImage = StitchingInvoke.CvStitcherStitch(_ptr, handle.AddrOfPinnedObject(), images.Length);
         handle.Free();

         if (resultIplImage == IntPtr.Zero)
            throw new ArgumentException("Requires more images");

         MIplImage tmp = (MIplImage) Marshal.PtrToStructure(resultIplImage, typeof(MIplImage));
         Image<Bgr, Byte> result = new Image<Bgr, byte>(tmp.width, tmp.height);
         CvInvoke.cvCopy(resultIplImage, result, IntPtr.Zero);
         CvInvoke.cvReleaseImage(ref resultIplImage);
         return result;
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
         bool hasCuda = GpuInvoke.HasCuda;
      }

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvStitcherCreateDefault(
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool tryUseGpu
         );

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvStitcherStitch(IntPtr stitcherWrapper, IntPtr images, int imgCount);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvStitcherRelease(ref IntPtr stitcherWrapper);
   }
}
