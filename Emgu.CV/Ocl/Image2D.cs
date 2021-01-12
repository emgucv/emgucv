//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace Emgu.CV.Ocl
{
   /// <summary>
   /// cv::ocl::Image2D
   /// </summary>
   public class Image2D : UnmanagedObject
   {
      /// <summary>
      /// Create an OclImage2D object from UMat
      /// </summary>
      /// <param name="src">The UMat from which to get image properties and data</param>
      /// <param name="norm">Flag to enable the use of normalized channel data types</param>
      /// <param name="alias">Flag indicating that the image should alias the src UMat. If true, changes to the image or src will be reflected in both objects.</param>
      public Image2D(UMat src, bool norm = false, bool alias = false)
      {
         _ptr = OclInvoke.oclImage2DFromUMat(src, norm, alias);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this OclImage2D
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            OclInvoke.oclImage2DRelease(ref _ptr);
      }
   }

   /// <summary>
   /// Class that contains ocl functions.
   /// </summary>
   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclImage2DFromUMat(
         IntPtr src,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool norm,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool alias);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclImage2DRelease(ref IntPtr oclImage2d);
   }
}
