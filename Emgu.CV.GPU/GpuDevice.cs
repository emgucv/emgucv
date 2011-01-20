using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// The Gpu device information
   /// </summary>
   public class GpuDevice
   {
      /// <summary>
      /// Query the information of the gpu device with the specific id.
      /// </summary>
      /// <param name="deviceId">The device id</param>
      public GpuDevice(int deviceId)
      {
         ID = deviceId;
         Name = GpuInvoke.GetDeviceName(deviceId);
         GpuInvoke.GetComputeCapability(deviceId, ref CudaComputeCapability.Major, ref CudaComputeCapability.Minor);
         NumberOfSMs = GpuInvoke.GetNumberOfSMs(deviceId);
         HasNativeDoubleSupport = GpuInvoke.HasNativeDoubleSupport(deviceId);
         HasAtomicSupport = GpuInvoke.HasAtomicsSupport(deviceId);
      }

      /// <summary>
      /// The id of the device
      /// </summary>
      public int ID;
      /// <summary>
      /// The name of the device
      /// </summary>
      public String Name;
      /// <summary>
      /// The compute capability
      /// </summary>
      public ComputeCapability CudaComputeCapability;

      /// <summary>
      /// The number of single multi processors
      /// </summary>
      public int NumberOfSMs;

      /// <summary>
      /// Indicates if the decive has native double support
      /// </summary>
      public bool HasNativeDoubleSupport;

      /// <summary>
      /// Indicates if the device has atomic support
      /// </summary>
      public bool HasAtomicSupport;

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

   }
}
