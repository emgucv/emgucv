//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace Emgu.CV
{
   public partial class OclKernel : UnmanagedObject
   {
      public OclKernel()
      {

         _ptr = OclInvoke.oclKernelCreateDefault();
      }


      public bool Create(String kernelName, OclProgramSource programSource, String buildOps = null, CvString errMsg = null)
      {
         using (CvString cs = new CvString(kernelName))
         using (CvString buildOptStr = new CvString(buildOps))
         {
            return OclInvoke.oclKernelCreate(_ptr, cs, programSource, buildOptStr, errMsg);
         }
      }

      protected override void DisposeObject()
      {
         OclInvoke.oclKernelRelease(ref _ptr);
      }
   }

   /// <summary>
   /// Class that contains ocl functions.
   /// </summary>
   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclKernelCreateDefault();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool oclKernelCreate(IntPtr kernel, IntPtr kname, IntPtr programSource, IntPtr buildOps, IntPtr errorMsg);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclKernelRelease(ref IntPtr oclKernel);
   }
}
