//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// Wraped class of the C++ standard vector of OclInfo.
   /// </summary>
   public class VectorOfOclDeviceInfo : Emgu.Util.UnmanagedObject
   {
      private bool _requiresDispose;

      internal VectorOfOclDeviceInfo(IntPtr ptr, bool requiresDispose)
      {
         _ptr = ptr;
         _requiresDispose = requiresDispose;
      }

      /// <summary>
      /// Create an empty standard vector of OclInfo
      /// </summary>
      public VectorOfOclDeviceInfo()
         :this (OclInvoke.VectorOfOclDeviceInfoCreate(), true)
      {
      }

      /// <summary>
      /// Create an standard vector of OclInfo of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfOclDeviceInfo(int size)
         : this (OclInvoke.VectorOfOclDeviceInfoCreateSize(size), true)
      {
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return OclInvoke.VectorOfOclDeviceInfoGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         OclInvoke.VectorOfOclDeviceInfoClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return OclInvoke.VectorOfOclDeviceInfoGetStartAddress(_ptr);
         }
      }

      /*
      /// <summary>
      /// Convert the standard vector to an array of KeyPoint
      /// </summary>
      /// <returns>An array of KeyPoint</returns>
      public MKeyPoint[] ToArray()
      {
         MKeyPoint[] res = new MKeyPoint[Size];
         if (res.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            CvInvoke.VectorOfKeyPointCopyData(_ptr, handle.AddrOfPinnedObject());
            handle.Free();
         }
         return res;
      }*/

      /// <summary>
      /// Get the item in the specific index
      /// </summary>
      /// <param name="index">The index</param>
      /// <returns>The item in the specific index</returns>
      public OclDeviceInfo this[int index]
      {
         get
         {
            return new OclDeviceInfo( OclInvoke.VectorOfOclDeviceInfoGetItem(_ptr, index) );
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero && _requiresDispose)
         {
            OclInvoke.VectorOfOclDeviceInfoRelease(_ptr);
         }
      }

   }


   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclDeviceInfoCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclDeviceInfoCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclDeviceInfoRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfOclDeviceInfoGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclDeviceInfoGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclDeviceInfoClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclDeviceInfoGetItem(IntPtr oclInfoVec, int index);
   }
}