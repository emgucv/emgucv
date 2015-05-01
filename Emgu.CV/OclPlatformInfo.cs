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
   /// This class contains ocl platform information
   /// </summary>
   public partial class OclPlatformInfo : UnmanagedObject
   {
      private bool _needDispose;

      internal OclPlatformInfo(IntPtr ptr, bool needDispose)
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
      public OclDevice GetDevice(int d)
      {
         OclDevice device = new OclDevice();
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
      public static Util.VectorOfOclPlatformInfo GetPlatformInfo()
      {
         Util.VectorOfOclPlatformInfo result = new Util.VectorOfOclPlatformInfo();
         OclInvoke.oclGetPlatformsInfo(result);
         return result;
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void oclPlatformInfoGetDevice(IntPtr platformInfo, IntPtr device, int d);
   }
}
