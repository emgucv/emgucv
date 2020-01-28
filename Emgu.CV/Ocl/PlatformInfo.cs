//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using System.Runtime.InteropServices;

namespace Emgu.CV.Ocl
{
   /// <summary>
   /// This class contains ocl platform information
   /// </summary>
   public partial class PlatformInfo : UnmanagedObject
   {
      private bool _needDispose;

      internal PlatformInfo(IntPtr ptr, bool needDispose)
      {
         _ptr = ptr;
         _needDispose = needDispose;
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
      public Device GetDevice(int d)
      {
         Device device = new Device();
         OclInvoke.oclPlatformInfoGetDevice(Ptr, device, d);
         return device;
      }

      /// <summary>
      /// Get the string that represent this oclPlatformInfo object
      /// </summary>
      /// <returns>A string that represent this oclPlatformInfo object</returns>
      public override string ToString()
      {
         return Name;
      }
   }


   public static partial class OclInvoke
   {
      static OclInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclPlatformInfoRelease(ref IntPtr platformInfo);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclGetPlatformsInfo(IntPtr platformInfoVec);

      /// <summary>
      /// Get all the platform info as a vector
      /// </summary>
      /// <returns>The vector of Platfom info</returns>
      public static Util.VectorOfOclPlatformInfo GetPlatformsInfo()
      {
         Util.VectorOfOclPlatformInfo result = new Util.VectorOfOclPlatformInfo();
         OclInvoke.oclGetPlatformsInfo(result);
         return result;
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclPlatformInfoGetDevice(IntPtr platformInfo, IntPtr device, int d);
   }
}
