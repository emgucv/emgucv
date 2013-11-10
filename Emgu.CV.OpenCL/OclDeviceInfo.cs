using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// This class contains ocl runtime information
   /// </summary>
   public class OclDeviceInfo : UnmanagedObject
   {
      internal OclDeviceInfo(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Get the device profile
      /// </summary>
      public String Profile
      {
         get
         {
            int type = 0;
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return Marshal.PtrToStringAnsi(profile);
         }
      }

      /// <summary>
      /// Get the device version
      /// </summary>
      public String Version
      {
         get
         {
            int type = 0;
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return Marshal.PtrToStringAnsi(version);
         }
      }

      /// <summary>
      /// Get the device vendor
      /// </summary>
      public String Vendor
      {
         get
         {
            int type = 0;
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return Marshal.PtrToStringAnsi(vendor);
         }
      }

      /// <summary>
      /// Get the device vendor id
      /// </summary>
      public int VendorId
      {
         get
         {
            int type = 0;
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return vendorId;
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
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int deviceVersionMajor = 0 ;
            int deviceVersionMinor= 0;
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

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
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return deviceVersionMinor;
         }
      }

      /// <summary>
      /// True if the device has double support
      /// </summary>
      public bool HaveDoubleSupport
      {
         get
         {
            int type = 0;
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return haveDoubleSupport != 0;
         }
      }

      /// <summary>
      /// True if the device use unified memory
      /// </summary>
      public bool IsUnifiedMemory
      {
         get
         {
            int type = 0;
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return isUnifiedMemory != 0;
         }
      }

      /// <summary>
      /// Get the Compilation extra options
      /// </summary>
      public String CompilationExtraOptions
      {
         get
         {
            int type = 0;
            IntPtr profile = IntPtr.Zero;
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
            int haveDoubleSupport = 0;
            int isUnifiedMemory = 0;
            IntPtr compilationExtraOptions = IntPtr.Zero;

            OclInvoke.oclDeviceInfoGetProperty(_ptr, ref type, ref profile, ref version, ref name, ref vendor, ref vendorId, ref driverVersion, ref extensions,
               ref maxWorkGroupSize, ref maxComputeUnits, ref localMemorySize, ref maxMemAllocSize, ref deviceVersionMajor, ref deviceVersionMinor, ref haveDoubleSupport, ref isUnifiedMemory, ref compilationExtraOptions);

            return Marshal.PtrToStringAnsi(compilationExtraOptions);
         }
      }

      /// <summary>
      /// Get the OpenCL platform info
      /// </summary>
      public OclPlatformInfo Platform
      {
         get
         {
            return new OclPlatformInfo(OclInvoke.oclDeviceInfoGetPlatform(_ptr));
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this OclInfo
      /// </summary>
      protected override void DisposeObject()
      {
         //throw new NotImplementedException();
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

   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceInfoGetProperty(IntPtr oclDeviceInfo,
         ref int type,
         ref IntPtr profile,
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
         ref int haveDoubleSupport,
         ref int isUnifiedMemory,
         ref IntPtr compilationExtraOptions
         );

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclDeviceInfoGetPlatform(IntPtr oclDeviceInfo);
   }
}
