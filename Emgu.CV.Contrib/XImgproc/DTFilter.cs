//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Ximgproc
{
   public class DTFilter : UnmanagedObject
   {
      public DTFilter(IInputArray guide, double sigmaSpatial, double sigmaColor, int mode, int numIters)
      {
         using (InputArray iaGuide = guide.GetInputArray())
            _ptr = XimgprocInvoke.cveDTFilterCreate( iaGuide, sigmaSpatial, sigmaColor, mode, numIters);
      }

      public void Filter(IInputArray src, IOutputArray dst, int dDepth)
      {
         using (InputArray iaSrc = src.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
               XimgprocInvoke.cveDTFilterFilter(_ptr, iaSrc, oaDst, dDepth);
            }
         
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
         {
            XimgprocInvoke.cveDTFilterRelease(ref _ptr);
         }
      }
   }


   public static partial class XimgprocInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveDTFilterCreate(IntPtr guide, double sigmaSpatial, double sigmaColor, int mode, int numIters);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDTFilterFilter(IntPtr filter, IntPtr src, IntPtr dst, int dDepth);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveDTFilterRelease(ref IntPtr filter);
   }
}
