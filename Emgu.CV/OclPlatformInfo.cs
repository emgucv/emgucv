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
   public class OclPlatformInfo : UnmanagedObject
   {
      private bool _needDispose;

      internal OclPlatformInfo(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
      }

      /// <summary>
      /// Get the platform version
      /// </summary>
      public String Version
      {
         get
         {
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;

            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref version, ref name, ref vendor);
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
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;

            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref version, ref name, ref vendor);
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
            IntPtr version = IntPtr.Zero;
            IntPtr name = IntPtr.Zero;
            IntPtr vendor = IntPtr.Zero;

            OclInvoke.oclPlatformInfoGetProperties(_ptr, ref version, ref name, ref vendor);
            return Marshal.PtrToStringAnsi(vendor);
         }
      }

      /// <summary>
      /// Get the number of device that belongs to the 
      /// </summary>
      public int DeviceNumber
      {
         get
         {
            return OclInvoke.oclPlatformInfoDeviceNumber(_ptr);
         }
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this OclInfo
      /// </summary>
      protected override void DisposeObject()
      {
         if (_needDispose)
            OclInvoke.oclPlatformInfoRelease(ref _ptr);
      }

      /// <summary>
      /// Get the OclDevice with the specific index
      /// </summary>
      /// <param name="d">The index of the ocl device</param>
      /// <returns>The ocl device with the specific index</returns>
      public OclDevice GetDevice(int d)
      {
         OclDevice device = new OclDevice();
         OclInvoke.oclPlatformInfoGetDevice(Ptr, device, d);
         return device;
      }
   }


   public static partial class OclInvoke
   {
      static OclInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclPlatformInfoGetProperties(
         IntPtr oclPlatformInfo,
         ref IntPtr platformVersion,
         ref IntPtr platformName,
         ref IntPtr platformVendor);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclPlatformInfoRelease(ref IntPtr platformInfo);

      //[DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      //internal static extern IntPtr oclPlatformInfoGetDevices(IntPtr oclPlatformInfo);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclPlatformInfoDeviceNumber(IntPtr platformInfo);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclGetPlatfomsInfo(IntPtr platformInfoVec);

      /// <summary>
      /// Get all the platform info as a vector
      /// </summary>
      /// <returns>The vector of Platfom info</returns>
      public static Util.VectorOfOclPlatformInfo GetPlatformInfo()
      {
         Util.VectorOfOclPlatformInfo result = new Util.VectorOfOclPlatformInfo();
         OclInvoke.oclGetPlatfomsInfo(result);
         return result;
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclPlatformInfoGetDevice(IntPtr platformInfo, IntPtr device, int d);
   }
}
