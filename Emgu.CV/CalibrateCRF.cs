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
   public abstract class CalibrateCRF
   {
      protected IntPtr _calibrateCRFPtr;

      public void Process(IInputArray src, IOutputArray dst, IInputArray times)
      {
         using (InputArray iaSrc = src.GetInputArray())
         using (OutputArray oaDst = dst.GetOutputArray())
         using (InputArray iaTimes = times.GetInputArray())
         {
            CvInvoke.cveCalibrateCRFProcess(_calibrateCRFPtr, iaSrc, oaDst, iaTimes);
         }
      }
   }

   public static partial class CvInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveCalibrateCRFProcess(
         IntPtr calibrateCRF, IntPtr src, IntPtr dst, IntPtr times);
   }
}
