//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

#if !(IOS || UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || NETFX_CORE)
using Emgu.CV.Cuda;
#endif
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
         using (InputArray iaImages = images.GetInputArray())
         using (OutputArray oaPano = pano.GetOutputArray())
         return StitchingInvoke.CvStitcherStitch(_ptr, iaImages, oaPano);
      }

      public void SetFeaturesFinder(FeaturesFinder finder)
      {
         StitchingInvoke.CvStitcherSetFeaturesFinder(_ptr, finder.Ptr);
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
#if !(IOS || UNITY_IPHONE || UNITY_ANDROID || UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || NETFX_CORE)
         //Dummy code to make sure the static constructor of GpuInvoke has been called
         bool hasCuda = CudaInvoke.HasCuda;
#else		 
		 CvInvoke.CheckLibraryLoaded();
#endif		 
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr CvStitcherCreateDefault(
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool tryUseGpu
         );

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool CvStitcherStitch(IntPtr stitcherWrapper, IntPtr images, IntPtr pano);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvStitcherSetFeaturesFinder(IntPtr stitcherWrapper, IntPtr finder);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void CvStitcherRelease(ref IntPtr stitcherWrapper);
   }
}
