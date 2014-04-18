using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// This class contains ocl runtime information
   /// </summary>
   public class OclDevice : UnmanagedObject
   {
      private bool _needDispose;

      /// <summary>
      /// Create a empty OclDevice object
      /// </summary>
      public OclDevice()
         : this(OclInvoke.oclDeviceCreate(), true)
      {
      }

      internal OclDevice(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Get the device version
      /// </summary>
      public String Version
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return Marshal.PtrToStringAnsi(version);
         }
      }

      /// <summary>
      /// Get the device vendor name
      /// </summary>
      public String VendorName
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return Marshal.PtrToStringAnsi(vendor);
         }
      }

      /// <summary>
      /// Get the device driver version
      /// </summary>
      public String DriverVersion
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return Marshal.PtrToStringAnsi(driverVersion);
         }
      }

      /// <summary>
      /// Get the device extensions
      /// </summary>
      public String Extensions
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return Marshal.PtrToStringAnsi(extensions);
         }
      }

      /// <summary>
      /// Get the device name
      /// </summary>
      public String Name
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return Marshal.PtrToStringAnsi(name);
         }
      }

      /// <summary>
      /// Get the ocl device type
      /// </summary>
      public OclDeviceType Type
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return (OclDeviceType) type;
         }
      }

      /// <summary>
      /// The maximum work group size
      /// </summary>
      public int MaxWorkGroupSize
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return maxWorkGroupSize;
         }
      }

      /// <summary>
      /// The max compute unit
      /// </summary>
      public int MaxComputeUnits
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return maxComputeUnits;
         }
      }

      /// <summary>
      /// The local memory size
      /// </summary>
      public int LocalMemorySize
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return localMemorySize;
         }
      }

      /// <summary>
      /// The maximum memory allocation size
      /// </summary>
      public int MaxMemAllocSize
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return maxMemAllocSize;
         }
      }

      /// <summary>
      /// The device major version number
      /// </summary>
      public int DeviceVersionMajor
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return deviceVersionMajor;
         }
      }

      /// <summary>
      /// The Device minor version number
      /// </summary>
      public int DeviceVersionMinor
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return deviceVersionMinor;
         }
      }

      /// <summary>
      /// The device double float point configuration
      /// </summary>
      public int DoubleFPConfig
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return doubleFPConfig;
         }
      }

      /// <summary>
      /// True if the device use unified memory
      /// </summary>
      public int HostUnifiedMemory
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return hostUnifiedMemory;
         }
      }

      /// <summary>
      /// Get the opencl version
      /// </summary>
      public String OpenCLVersion
      {
         get
         {
            int type = 0;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            int vendorId = 0;
            IntPtr driverVersion = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;

            int maxWorkGroupSize = 0;
            int maxComputeUnits = 0;
            int localMemorySize = 0;
            int maxMemAllocSize = 0;
            int deviceVersionMajor = 0;
            int deviceVersionMinor = 0;
            int doubleFPConfig = 0;
            int hostUnifiedMemory = 0;
            IntPtr openCLVersion = IntPtr.Zero;

            OclInvoke.oclDeviceGetProperty(_ptr, ref type, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref doubleFPConfig, ref hostUnifiedMemory, ref openCLVersion);

            return Marshal.PtrToStringAnsi(openCLVersion);
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this OclInfo
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose)
         {
            if (_ptr != IntPtr.Zero)
            {
               OclInvoke.oclDeviceRelease(ref _ptr);
            }
         }
      }
   }


   /// <summary>
   /// Ocl Device Type
   /// </summary>
   public enum OclDeviceType
   {
      /// <summary>
      /// Default
      /// </summary>
      Default = (1 << 0),
      /// <summary>
      /// Cpu
      /// </summary>
      Cpu = (1 << 1),
      /// <summary>
      /// Gpu
      /// </summary>
      Gpu = (1 << 2),
      /// <summary>
      /// Accerlerator
      /// </summary>
      Accelerator = (1 << 3),
      /// <summary>
      /// All
      /// </summary>
      All = -1 //0xFFFFFFFF
   }

   /// <summary>
   /// Class that contains ocl functions.
   /// </summary>
   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceGetProperty(
         IntPtr oclDeviceInfo,
         ref int type,
         
         ref IntPtr version,
         ref IntPtr name,
         ref IntPtr vendor,
         ref int vendorId,
         ref IntPtr driverVersion,
         ref IntPtr extensions,

         ref int maxWorkGroupSize,
         ref int maxComputeUnits,
         ref int localMemorySize,
         ref int maxMemAllocSize,
         ref int deviceVersionMajor,
         ref int deviceVersionMinor,
         ref int doubleFPConfig,
         ref int hostUnifiedMemory,
         ref IntPtr openCLVersion
         );

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclDeviceCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceRelease(ref IntPtr oclDevice);
   }
}
