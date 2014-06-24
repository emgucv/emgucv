//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   public abstract class MergeExposures
   {
      protected IntPtr _mergeExposuresPtr;

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
