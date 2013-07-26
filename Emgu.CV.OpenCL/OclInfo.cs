using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.OpenCL
{
   public class OclInfo
   {
      private IntPtr _ptr;
      internal OclInfo(IntPtr ptr)
      {
         _ptr = ptr;
      }

      public String PlatformName
      {
         get
         {
            IntPtr ptr = OclInvoke.oclInfoGetPlatformName(_ptr);
            return Marshal.PtrToStringAnsi(ptr);
         }
      }

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
