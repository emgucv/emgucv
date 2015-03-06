//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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
   public partial class OclDevice : UnmanagedObject
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
            using (CvString s = new CvString())
            {
               OclInvoke.oclDeviceGetVersion(_ptr, s);
               return s.ToString();
            }
         }
      }

      /// <summary>
      /// Get the device vendor name
      /// </summary>
      public String VendorName
      {
         get
         {
            using (CvString s = new CvString())
            {
               OclInvoke.oclDeviceGetVenderName(_ptr, s);
               return s.ToString();
            }
         }
      }

      /// <summary>
      /// Get the device driver version
      /// </summary>
      public String DriverVersion
      {
         get
         {
            using (CvString s = new CvString())
            {
               OclInvoke.oclDeviceGetDriverVersion(_ptr, s);
               return s.ToString();
            }
         }
      }

      /// <summary>
      /// Get the device extensions
      /// </summary>
      public String Extensions
      {
         get
         {
            using (CvString s = new CvString())
            {
               OclInvoke.oclDeviceGetExtensions(_ptr, s);
               return s.ToString();
            }
         }
      }

      /// <summary>
      /// Get the device name
      /// </summary>
      public String Name
      {
         get
         {
            using (CvString s = new CvString())
            {
               OclInvoke.oclDeviceGetName(_ptr, s);
               return s.ToString();
            }
         }
      }


      /// <summary>
      /// Get the opencl version
      /// </summary>
      public String OpenCLVersion
      {
         get
         {
            using (CvString s = new CvString())
            {
               OclInvoke.oclDeviceGetOpenCLVersion(_ptr, s);
               return s.ToString();
            }
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

      /// <summary>
      /// Get the string representation of this oclDevice
      /// </summary>
      /// <returns>A string representation of this oclDevice</returns>
      public override string ToString()
      {
         return String.Format("{0} {1}.{2} ({3}):Version - {4}; Global memory - {5}； Local memory - {6}; Max image size - {7}x{8}", Name, DeviceVersionMajor, DeviceVersionMinor, Type, Version, GlobalMemSize, LocalMemSize, Image2DMaxWidth, Image2DMaxHeight);
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
      /// DGpu
      /// </summary>
      DGpu = Gpu | (1 << 16),
      /// <summary>
      /// IGpu
      /// </summary>
      IGpu = Gpu | (1 << 17),
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
      internal static extern void oclDeviceGetVersion(IntPtr oclDeviceInfo, IntPtr version);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceGetName(IntPtr oclDeviceInfo, IntPtr name);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceGetVenderName(IntPtr oclDeviceInfo, IntPtr vender);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceGetDriverVersion(IntPtr oclDeviceInfo, IntPtr driverVersion);
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceGetExtensions(IntPtr oclDeviceInfo, IntPtr extensions);


      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceGetOpenCLVersion(IntPtr oclDeviceInfo, IntPtr extensions);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr oclDeviceCreate();

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclDeviceRelease(ref IntPtr oclDevice);
   }
}
