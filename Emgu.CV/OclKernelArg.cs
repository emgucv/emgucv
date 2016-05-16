//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;


namespace Emgu.CV
{
    public class OclKernelArg : UnmanagedObject
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

       public OclKernelArg(Flags flags, UMat m, int wscale = 1, int iwscale = 1, IntPtr obj = new IntPtr(), IntPtr sz = new IntPtr())
       {
          _ptr = OclInvoke.oclKernelArgCreate(flags, m, wscale, iwscale, obj, sz);
       }

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
      internal static extern IntPtr oclKernelArgCreate(OclKernelArg.Flags flags, IntPtr m, int wscale, int iwscale, IntPtr obj, IntPtr sz);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclKernelArgRelease(ref IntPtr k);
   }
}
