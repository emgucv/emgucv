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
   /// <summary>
   /// An opencl kernel
   /// </summary>
   public partial class OclKernel : UnmanagedObject
   {
      /// <summary>
      /// Create an opencl kernel
      /// </summary>
      public OclKernel()
      {

         _ptr = OclInvoke.oclKernelCreateDefault();
      }

      /// <summary>
      /// Create an opencl kernel
      /// </summary>
      /// <param name="kernelName">The name of the kernel</param>
      /// <param name="programSource">The program source code</param>
      /// <param name="buildOps">The build options</param>
      /// <param name="errMsg">Option error message container that can be passed to this function</param>
      /// <returns>True if the kernel can be created</returns>
      public bool Create(String kernelName, OclProgramSource programSource, String buildOps = null, CvString errMsg = null)
      {
         using (CvString cs = new CvString(kernelName))
         using (CvString buildOptStr = new CvString(buildOps))
         {
            return OclInvoke.oclKernelCreate(_ptr, cs, programSource, buildOptStr, errMsg);
         }
      }

      /// <summary>
      /// Release the opencl kernel
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
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
