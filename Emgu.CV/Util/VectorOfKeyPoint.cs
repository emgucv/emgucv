using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of MKeyPoint.
   /// </summary>
   public class VectorOfKeyPoint : Emgu.Util.UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfKeyPointCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfKeyPointCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfKeyPointRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int VectorOfKeyPointGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfKeyPointCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfKeyPointGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfKeyPointPushMulti(IntPtr v, IntPtr values, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfKeyPointClear(IntPtr v);
      #endregion

      /// <summary>
      /// Create an empty standard vector of float
      /// </summary>
      public VectorOfKeyPoint()
      {
         _ptr = VectorOfKeyPointCreate();
      }

      /// <summary>
      /// Create an standard vector of float of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfKeyPoint(int size)
      {
         _ptr = VectorOfKeyPointCreateSize(size);
      }

      /// <summary>
      /// Push an array of value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(MKeyPoint[] value)
      {
         GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
         VectorOfKeyPointPushMulti(_ptr, handle.AddrOfPinnedObject(), value.Length);
         handle.Free();
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return VectorOfKeyPointGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfKeyPointClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return VectorOfKeyPointGetStartAddress(_ptr);
         }
      }

      /// <summary>
      /// Convert the standard vector to an array of float
      /// </summary>
      /// <returns>An array of float</returns>
      public MKeyPoint[] ToArray()
      {
         MKeyPoint[] res = new MKeyPoint[Size];
         GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
         VectorOfKeyPointCopyData(_ptr, handle.AddrOfPinnedObject());
         handle.Free();
         return res;
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         VectorOfKeyPointRelease(_ptr);
      }
   }
}
