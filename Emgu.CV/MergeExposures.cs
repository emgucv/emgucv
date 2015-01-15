//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// The base class algorithms that can merge exposure sequence to a single image.
   /// </summary>
   public abstract class MergeExposures
   {
      /// <summary>
      /// The pointer to the unmanaged MergeExposure object
      /// </summary>
      protected IntPtr _mergeExposuresPtr;

      /// <summary>
      /// Merges images.
      /// </summary>
      /// <param name="src">Vector of input images</param>
      /// <param name="dst">Result image</param>
      /// <param name="times">Vector of exposure time values for each image</param>
      /// <param name="response">256x1 matrix with inverse camera response function for each pixel value, it should have the same number of channels as images.</param>
      public void Process(IInputArray src, IOutputArray dst, IInputArray times, IInputArray response)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         using (InputArray iaTimes = times.GetInputArray())
         using (InputArray iaResponse = response.GetInputArray())
         {
            CvInvoke.cveMergeExposuresProcess(_mergeExposuresPtr, iaSrc, oaDst, iaTimes, iaResponse);
         }
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveMergeExposuresProcess(
         IntPtr mergeExposures,
         IntPtr src, IntPtr dst,
         IntPtr times, IntPtr response);
   }
}
