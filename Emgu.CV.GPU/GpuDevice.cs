//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// The Gpu device information
   /// </summary>
   public class GpuDevice : UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr gpuDeviceInfoCreate(ref int deviceId);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuDeviceInfoRelease(ref IntPtr di);

      /// <summary>
      /// Get the compute capability of the device
      /// </summary>
      /// <param name="device">The device</param>
      /// <param name="major">The major version of the compute capability</param>
      /// <param name="minor">The minor version of the compute capability</param>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoComputeCapability(IntPtr device, ref int major, ref int minor);

      /// <summary>
      /// Get the number of multiprocessors on device
      /// </summary>
      /// <param name="device">The device</param>
      /// <returns>The number of multiprocessors on device</returns>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int gpuDeviceInfoMultiProcessorCount(IntPtr device);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoFreeMemInfo(IntPtr device, ref UIntPtr free);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoTotalMemInfo(IntPtr device, ref UIntPtr total);

      /// <summary>
      /// Get the device name
      /// </summary>
      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoDeviceName(
         IntPtr device,
         [MarshalAs(CvInvoke.StringMarshalType)]
         StringBuilder buffer,
         int maxSizeInBytes);

      [DllImport(CvInvoke.EXTERN_GPU_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      [return: MarshalAs(CvInvoke.BoolMarshalType)]
      private static extern bool gpuDeviceInfoSupports(IntPtr device, GpuFeature feature);
      #endregion

      private int _deviceID;

      /// <summary>
      /// Query the information of the gpu device that is currently in use.
      /// </summary>
      public GpuDevice()
         : this(-1)
      {
      }

      /// <summary>
      /// Query the information of the gpu device with the specific id.
      /// </summary>
      /// <param name="deviceId">The device id</param>
      public GpuDevice(int deviceId)
      {
         _ptr = gpuDeviceInfoCreate(ref deviceId);
         _deviceID = deviceId;
      }

      /// <summary>
      /// The id of the device
      /// </summary>
      public int ID
      {
         get { return _deviceID; }
      }

      /// <summary>
      /// The name of the device
      /// </summary>
      public String Name
      {
         get
         {
            StringBuilder buffer = new StringBuilder(1024);
            gpuDeviceInfoDeviceName(_ptr, buffer, 1024);
            return buffer.ToString();
         }
      }

      /// <summary>
      /// The compute capability
      /// </summary>
      public Version CudaComputeCapability
      {
         get
         {
            int major = 0, minor = 0;
            gpuDeviceInfoComputeCapability(_ptr, ref major, ref minor);
            return new Version(major, minor);
         }
      }

      /// <summary>
      /// The number of single multi processors
      /// </summary>
      public int MultiProcessorCount
      {
         get
         {
            return gpuDeviceInfoMultiProcessorCount(_ptr);
         }
      }

      /// <summary>
      /// Get the amount of free memory at the moment
      /// </summary>
      public ulong FreeMemory
      {
         get
         {
            UIntPtr f = new UIntPtr();
            gpuDeviceInfoFreeMemInfo(_ptr, ref f);
            return f.ToUInt64();
         }
      }

      /// <summary>
      /// Get the amount of total memory
      /// </summary>
      public ulong TotalMemory
      {
         get
         {
            UIntPtr t = new UIntPtr();
            gpuDeviceInfoTotalMemInfo(_ptr, ref t);
            return t.ToUInt64();
         }
      }

      /// <summary>
      /// Indicates if the decive has the specific feature
      /// </summary>
      public bool Supports(GpuFeature feature)
      {
         return gpuDeviceInfoSupports(_ptr, feature);
      }

      /// <summary>
      /// GPU feature
      /// </summary>
      public enum GpuFeature
      {
         /// <summary>
         /// Cuda compute 1.0
         /// </summary>
         Compute10 = 10,
         /// <summary>
         /// Cuda compute 1.1
         /// </summary>
         Compute11 = 11,
         /// <summary>
         /// Cuda compute 1.2
         /// </summary>
         Compute12 = 12,
         /// <summary>
         /// Cuda compute 1.3
         /// </summary>
         Compute13 = 13,
         /// <summary>
         /// Cuda compute 2.0
         /// </summary>
         Compute20 = 20,
         /// <summary>
         /// Cuda compute 2.1
         /// </summary>
         Compute21 = 21,

         /// <summary>
         /// Native double
         /// </summary>
         NativeDouble = Compute11,
         /// <summary>
         /// Atomic
         /// </summary>
         Atomics = Compute13
      }

      /// <summary>
      /// Release the unmanaged resource related to the GpuDevice
      /// </summary>
      protected override void DisposeObject()
      {
         gpuDeviceInfoRelease(ref _ptr);
      }
   }
}
