using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of float.
   /// </summary>
   public class VectorOfFloat : Emgu.Util.UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfFloatCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfFloatCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfFloatRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int VectorOfFloatGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfFloatCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfFloatGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfFloatPushMulti(IntPtr v, IntPtr values, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfFloatClear(IntPtr v);
      #endregion

      /// <summary>
      /// Create an empty standard vector of float
      /// </summary>
      public VectorOfFloat()
      {
         _ptr = VectorOfFloatCreate();
      }

      /// <summary>
      /// Create an standard vector of float of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfFloat(int size)
      {
         _ptr = VectorOfFloatCreateSize(size);
      }

      /// <summary>
      /// Push an array of value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(float[] value)
      {
         GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
         VectorOfFloatPushMulti(_ptr, handle.AddrOfPinnedObject(), value.Length);
         handle.Free();
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return VectorOfFloatGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfFloatClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return VectorOfFloatGetStartAddress(_ptr);
         }
      }

      /// <summary>
      /// Convert the standard vector to an array of float
      /// </summary>
      /// <returns>An array of float</returns>
      public float[] ToArray()
      {
         float[] res = new float[Size];
         GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
         VectorOfFloatCopyData(_ptr, handle.AddrOfPinnedObject());
         handle.Free();
         return res;
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         VectorOfFloatRelease(_ptr);
      }
   }
}
