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
   public class OclInfo : UnmanagedObject
   {
      internal OclInfo(IntPtr ptr)
      {
         _ptr = ptr;
      }

      /// <summary>
      /// Get the platform name
      /// </summary>
      public String PlatformName
      {
         get
         {
            IntPtr ptr = OclInvoke.oclInfoGetPlatformName(_ptr);
            return Marshal.PtrToStringAnsi(ptr);
         }
      }

      /// <summary>
      /// Get the device names
      /// </summary>
      public String[] DeviceNames
      {
         get
         {
            String[] names = new String[OclInvoke.oclInfoGetDeviceCount(_ptr)];
            for (int i = 0; i < names.Length; i++)
            {
               names[i] = Marshal.PtrToStringAnsi(OclInvoke.oclInfoGetDeviceName(_ptr, i));
            }
            return names;
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
      internal static extern IntPtr oclInfoGetPlatformName(IntPtr oclInfo);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int oclInfoGetDeviceCount(IntPtr oclInfo);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclInfoGetDeviceName(IntPtr oclInfo, int index);
   }
}
