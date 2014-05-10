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
   public class VectorOfOclPlatformInfo : Emgu.Util.UnmanagedObject
   {
      /// <summary>
      /// Create an empty standard vector of OclInfo
      /// </summary>
      public VectorOfOclPlatformInfo()
      {
         _ptr = OclInvoke.VectorOfOclPlatformInfoCreate();
      }

      /// <summary>
      /// Create an standard vector of OclInfo of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfOclPlatformInfo(int size)
      {
         _ptr = OclInvoke.VectorOfOclPlatformInfoCreateSize(size);
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return OclInvoke.VectorOfOclPlatformInfoGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         OclInvoke.VectorOfOclPlatformInfoClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return OclInvoke.VectorOfOclPlatformInfoGetStartAddress(_ptr);
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
      public OclPlatformInfo this[int index]
      {
         get
         {
            return new OclPlatformInfo( OclInvoke.VectorOfOclPlatformInfoGetItem(_ptr, index) );
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.VectorOfOclPlatformInfoRelease(_ptr);
      }

   }


   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclPlatformInfoCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclPlatformInfoCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclPlatformInfoRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfOclPlatformInfoGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclPlatformInfoGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclPlatformInfoClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclPlatformInfoGetItem(IntPtr oclInfoVec, int index);
   }
}