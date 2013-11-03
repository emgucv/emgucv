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
   public class OclPlatformInfo : UnmanagedObject
   {
      internal OclPlatformInfo(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Get the platform profile
      /// </summary>
      public String Profile
      {
         get
         {
            IntPtr profile = IntPtr.Zero;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;
            int versionMajor = 0;
            int versionMinor = 0;
            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref profile, ref version, ref name, ref vendor, ref extensions, ref versionMajor, ref versionMinor);
            return Marshal.PtrToStringAnsi(profile);
         }
      }

      /// <summary>
      /// Get the platform version
      /// </summary>
      public String Version
      {
         get
         {
            IntPtr profile = IntPtr.Zero;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;
            int versionMajor = 0;
            int versionMinor = 0;
            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref profile, ref version, ref name, ref vendor, ref extensions, ref versionMajor, ref versionMinor);
            return Marshal.PtrToStringAnsi(version);
         }
      }

      /// <summary>
      /// Get the platform name
      /// </summary>
      public String Name
      {
         get
         {
            IntPtr profile = IntPtr.Zero;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;
            int versionMajor = 0;
            int versionMinor = 0;
            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref profile, ref version, ref name, ref vendor, ref extensions, ref versionMajor, ref versionMinor);
            return Marshal.PtrToStringAnsi(name);
         }
      }

      /// <summary>
      /// Get the platform vendor
      /// </summary>
      public String Vendor
      {
         get
         {
            IntPtr profile = IntPtr.Zero;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;
            int versionMajor = 0;
            int versionMinor = 0;
            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref profile, ref version, ref name, ref vendor, ref extensions, ref versionMajor, ref versionMinor);
            return Marshal.PtrToStringAnsi(vendor);
         }
      }

      /// <summary>
      /// Get the platform extensions
      /// </summary>
      public String Extensions
      {
         get
         {
            IntPtr profile = IntPtr.Zero;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;
            int versionMajor = 0;
            int versionMinor = 0;
            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref profile, ref version, ref name, ref vendor, ref extensions, ref versionMajor, ref versionMinor);
            return Marshal.PtrToStringAnsi(extensions);
         }
      }

      /// <summary>
      /// Get the platform version major
      /// </summary>
      public int VersionMajor
      {
         get
         {
            IntPtr profile = IntPtr.Zero;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;
            int versionMajor = 0;
            int versionMinor = 0;
            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref profile, ref version, ref name, ref vendor, ref extensions, ref versionMajor, ref versionMinor);
            return versionMajor;
         }
      }

      /// <summary>
      /// Get the platform version minor
      /// </summary>
      public int VersionMinor
      {
         get
         {
            IntPtr profile = IntPtr.Zero;
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;
            IntPtr extensions = IntPtr.Zero;
            int versionMajor = 0;
            int versionMinor = 0;
            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref profile, ref version, ref name, ref vendor, ref extensions, ref versionMajor, ref versionMinor);
            return versionMinor;
         }
      }
      
      /// <summary>
      /// Get the devices
      /// </summary>
      public VectorOfOclDeviceInfo Devices
      {
         get
         {
            return new VectorOfOclDeviceInfo(OclInvoke.oclPlatformInfoGetDevices(_ptr), false);
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


   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclPlatformInfoGetProperties(
         IntPtr oclPlatformInfo,
         ref IntPtr platformProfile,
         ref IntPtr platformVersion,
         ref IntPtr platformName,
         ref IntPtr platformVendor,
         ref IntPtr platformExtensions,

         ref int platformVersionMajor,
         ref int platformVersionMinor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclPlatformInfoGetDevices(IntPtr oclPlatformInfo);
   }
}
