//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
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
   public class VectorOfOclInfo : Emgu.Util.UnmanagedObject
   {
      /// <summary>
      /// Create an empty standard vector of OclInfo
      /// </summary>
      public VectorOfOclInfo()
      {
         _ptr = OclInvoke.VectorOfOclInfoCreate();
      }

      /// <summary>
      /// Create an standard vector of OclInfo of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfOclInfo(int size)
      {
         _ptr = OclInvoke.VectorOfOclInfoCreateSize(size);
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return OclInvoke.VectorOfOclInfoGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         OclInvoke.VectorOfOclInfoClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return OclInvoke.VectorOfOclInfoGetStartAddress(_ptr);
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
      public OclInfo this[int index]
      {
         get
         {
            return new OclInfo( OclInvoke.VectorOfOclInfoGetItem(_ptr, index) );
         }
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         OclInvoke.VectorOfOclInfoRelease(_ptr);
      }

   }


   public static partial class OclInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclInfoCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclInfoCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclInfoRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfOclInfoGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclInfoGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfOclInfoClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfOclInfoGetItem(IntPtr oclInfoVec, int index);
   }
}