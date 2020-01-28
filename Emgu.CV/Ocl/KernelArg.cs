//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
   /// OpenCL kernel arg
   /// </summary>
   public class KernelArg : UnmanagedObject
   {
      /// <summary>
      /// KernelArg flags
      /// </summary>
      [Flags]
      public enum Flags
      {
         /// <summary>
         /// Local
         /// </summary>
         Local = 1,
         /// <summary>
         /// Read only
         /// </summary>
         ReadOnly = 2,
         /// <summary>
         /// Write only
         /// </summary>
         WriteOnly = 4,
         /// <summary>
         /// Read write
         /// </summary>
         ReadWrite = 6,
         /// <summary>
         /// Constant
         /// </summary>
         Constant = 8,
         /// <summary>
         /// Ptr only
         /// </summary>
         PtrOnly = 16,
         /// <summary>
         /// No size
         /// </summary>
         NoSize = 256
      }

      /// <summary>
      /// Create the OCL kernel arg
      /// </summary>
      /// <param name="flags">The flags</param>
      /// <param name="m">The UMat</param>
      /// <param name="wscale">wscale</param>
      /// <param name="iwscale">iwscale</param>
      /// <param name="obj">obj</param>
      /// <param name="sz">sz</param>
      public KernelArg(Flags flags, UMat m, int wscale = 1, int iwscale = 1, IntPtr obj = new IntPtr(), IntPtr sz = new IntPtr())
      {
         _ptr = OclInvoke.oclKernelArgCreate(flags, m, wscale, iwscale, obj, sz);
      }

      /// <summary>
      /// Release the unmanaged memory associated with this object
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            OclInvoke.oclKernelArgRelease(ref _ptr);
      }
   }

   /// <summary>
   /// Class that contains ocl functions.
   /// </summary>
   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclKernelArgCreate(KernelArg.Flags flags, IntPtr m, int wscale, int iwscale, IntPtr obj, IntPtr sz);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclKernelArgRelease(ref IntPtr k);
   }
}
