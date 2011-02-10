using System;
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
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static IntPtr gpuDeviceInfoCreate(ref int deviceId);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private extern static void gpuDeviceInfoRelease(ref IntPtr di);

      /// <summary>
      /// Get the compute capability of the device
      /// </summary>
      /// <param name="device">The device</param>
      /// <param name="major">The major version of the compute capability</param>
      /// <param name="minor">The minor version of the compute capability</param>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoComputeCapability(IntPtr device, ref int major, ref int minor);

      /// <summary>
      /// Get the number of multiprocessors on device
      /// </summary>
      /// <param name="device">The device</param>
      /// <returns>The number of multiprocessors on device</returns>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public static extern int gpuDeviceInfoMultiProcessorCount(IntPtr device);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoFreeMemInfo(IntPtr device, ref UIntPtr free);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoTotalMemInfo(IntPtr device, ref UIntPtr total);

      /// <summary>
      /// Get the device name
      /// </summary>
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void gpuDeviceInfoDeviceName(
         IntPtr device,
         [MarshalAs(CvInvoke.StringMarshalType)]
         StringBuilder buffer,
         int maxSizeInBytes);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
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
      public ComputeCapability CudaComputeCapability
      {
         get
         {
            ComputeCapability result = new ComputeCapability();
            gpuDeviceInfoComputeCapability(_ptr, ref result.Major, ref result.Minor);
            return result;
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
      /// The compute capability
      /// </summary>
      public struct ComputeCapability
      {
         /// <summary>
         /// The major version
         /// </summary>
         public int Major;
         /// <summary>
         /// The minor version
         /// </summary>
         public int Minor;
      }

      /// <summary>
      /// GPU feature
      /// </summary>
      public enum GpuFeature
      {
         /// <summary>
         /// Native double
         /// </summary>
         NativeDouble,
         /// <summary>
         /// Atomic
         /// </summary>
         Atomics
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
