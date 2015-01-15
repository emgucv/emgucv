//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
//using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// The base class for camera response calibration algorithms.
   /// </summary>
   public abstract class CalibrateCRF
   {
      /// <summary>
      /// The pointer the the calibrateCRF object
      /// </summary>
      protected IntPtr _calibrateCRFPtr;

      /// <summary>
      /// Recovers inverse camera response.
      /// </summary>
      /// <param name="src">Vector of input images</param>
      /// <param name="dst">256x1 matrix with inverse camera response function</param>
      /// <param name="times">Vector of exposure time values for each image</param>
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
