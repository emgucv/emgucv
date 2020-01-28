//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Emgu.CV;
using Emgu.Util;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Ocl
{
   /// <summary>
   /// An opencl kernel
   /// </summary>
   public partial class Kernel : UnmanagedObject
   {
      /// <summary>
      /// Create an opencl kernel
      /// </summary>
      public Kernel()
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
      public bool Create(String kernelName, ProgramSource programSource, String buildOps = null, CvString errMsg = null)
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

      /// <summary>
      /// Set the parameters for the kernel
      /// </summary>
      /// <param name="i">The index of the parameter</param>
      /// <param name="image2d">The ocl image</param>
      /// <returns>The next index value to be set</returns>
      public int Set(int i, Image2D image2d)
      {
         return OclInvoke.oclKernelSetImage2D(_ptr, i, image2d);
      }

      /// <summary>
      /// Set the parameters for the kernel
      /// </summary>
      /// <param name="i">The index of the parameter</param>
      /// <param name="umat">The umat</param>
      /// <returns>The next index value to be set</returns>
      public int Set(int i, UMat umat)
      {
         return OclInvoke.oclKernelSetUMat(_ptr, i, umat);
      }

      /// <summary>
      /// Set the parameters for the kernel
      /// </summary>
      /// <param name="i">The index of the parameter</param>
      /// <param name="value">The value</param>
      /// <returns>The next index value to be set</returns>
      public int Set(int i, ref int value)
      {
         return OclInvoke.oclKernelSetInt(_ptr, i, ref value, _sizeOfInt);
      }
      private static int _sizeOfInt = Toolbox.SizeOf<int>();

      /// <summary>
      /// Set the parameters for the kernel
      /// </summary>
      /// <param name="i">The index of the parameter</param>
      /// <param name="value">The value</param>
      /// <returns>The next index value to be set</returns>
      public int Set(int i, ref float value)
      {
         return OclInvoke.oclKernelSetFloat(_ptr, i, ref value, _sizeOfFloat);
      }
      private static int _sizeOfFloat = Toolbox.SizeOf<float>();

      /// <summary>
      /// Set the parameters for the kernel
      /// </summary>
      /// <param name="i">The index of the parameter</param>
      /// <param name="value">The value</param>
      /// <returns>The next index value to be set</returns>
      public int Set(int i, ref double value)
      {
         return OclInvoke.oclKernelSetDouble(_ptr, i, ref value, _sizeOfDouble);
      }
      private static int _sizeOfDouble = Toolbox.SizeOf<double>();


      /// <summary>
      /// Set the parameters for the kernel
      /// </summary>
      /// <param name="i">The index of the parameter</param>
      /// <param name="kernelArg">The kernel arg</param>
      /// <returns>The next index value to be set</returns>
      public int Set(int i, KernelArg kernelArg)
      {
         return OclInvoke.oclKernelSetKernelArg(_ptr, i, kernelArg);
      }

      /// <summary>
      /// Set the parameters for the kernel
      /// </summary>
      /// <param name="i">The index of the parameter</param>
      /// <param name="data">The data</param>
      /// <param name="size">The size of the data in number of bytes</param>
      /// <returns>The next index value to be set</returns>
      public int Set(int i, IntPtr data, int size)
      {
         return OclInvoke.oclKernelSet(_ptr, i, data, size);
      }

      /// <summary>
      /// Execute the kernel
      /// </summary>
      /// <param name="globalsize">The global size</param>
      /// <param name="localsize">The local size</param>
      /// <param name="sync">If true, the code is run synchronously (blocking)</param>
      /// <param name="q">Optional Opencl queue</param>
      /// <returns>True if the execution is sucessful</returns>
      public bool Run(IntPtr[] globalsize, IntPtr[] localsize, bool sync, Queue q = null)
      {
         Debug.Assert(localsize == null || globalsize.Length == localsize.Length, "The dimension of global size do not match the dimension of local size.");
         GCHandle gHandle = GCHandle.Alloc(globalsize, GCHandleType.Pinned);
         GCHandle lHandle;
         
         if (localsize != null)
            lHandle = GCHandle.Alloc(localsize, GCHandleType.Pinned);
         else
         {
            lHandle = new GCHandle();
         }
         try
         {
            return OclInvoke.oclKernelRun(
               _ptr, globalsize.Length, gHandle.AddrOfPinnedObject(),     
               localsize == null ? IntPtr.Zero : lHandle.AddrOfPinnedObject(), sync, q);
         }
         finally
         {
            gHandle.Free();
            if (localsize != null)
               lHandle.Free();
         }
         
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

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclKernelSetImage2D(IntPtr kernel, int i, IntPtr image2D);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclKernelSetUMat(IntPtr kernel, int i, IntPtr umat);

      [DllImport(CvInvoke.ExternLibrary, EntryPoint = "oclKernelSet", CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclKernelSetFloat(IntPtr kernel, int i, ref float value, int size);
      [DllImport(CvInvoke.ExternLibrary, EntryPoint = "oclKernelSet", CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclKernelSetInt(IntPtr kernel, int i, ref int value, int size);
      [DllImport(CvInvoke.ExternLibrary, EntryPoint = "oclKernelSet", CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclKernelSetDouble(IntPtr kernel, int i, ref double value, int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclKernelSet(IntPtr kernel, int i, IntPtr value, int size);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclKernelSetKernelArg(IntPtr kernel, int i, IntPtr kernelArg);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      internal static extern bool oclKernelRun(
         IntPtr kernel, int dims, IntPtr globalsize, IntPtr localsize, 
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool sync, 
         IntPtr q);

      /// <summary>
      /// Convert the DepthType to a string that represent the OpenCL value type.
      /// </summary>
      /// <param name="depthType">The depth type</param>
      /// <param name="channels">The number of channels</param>
      /// <returns>A string the repsent the OpenCL value type</returns>
      public static String TypeToString(DepthType depthType, int channels = 1)
      {
         using (CvString str = new CvString())
         {
            oclTypeToString(CvInvoke.MakeType(depthType, channels), str);
            return str.ToString();
         }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclTypeToString(int type, IntPtr str);
   }
}
