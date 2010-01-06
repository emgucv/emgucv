using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   internal class VectorOfFloat : Emgu.Util.UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern IntPtr VectorOfFloatCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern IntPtr VectorOfFloatCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void VectorOfFloatRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern int VectorOfFloatGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void VectorOfFloatCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern IntPtr VectorOfFloatGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void VectorOfFloatPushMulti(IntPtr v, IntPtr values, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY)]
      private static extern void VectorOfFloatClear(IntPtr v);
      #endregion

      public VectorOfFloat()
      {
         _ptr = VectorOfFloatCreate();
      }

      public VectorOfFloat(int size)
      {
         _ptr = VectorOfFloatCreateSize(size);
      }

      public void Push(float[] value)
      {
         GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
         VectorOfFloatPushMulti(_ptr, handle.AddrOfPinnedObject(), value.Length);
         handle.Free();
      }

      public int Size
      {
         get
         {
            return VectorOfFloatGetSize(_ptr);
         }
      }

      public void Clear()
      {
         VectorOfFloatClear(_ptr);
      }

      public IntPtr StartAddress
      {
         get
         {
            return VectorOfFloatGetStartAddress(_ptr);
         }
      }

      public float[] ToArray()
      {
         float[] res = new float[Size];
         GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
         VectorOfFloatCopyData(_ptr, handle.AddrOfPinnedObject());
         handle.Free();
         return res;
      }

      protected override void DisposeObject()
      {
         VectorOfFloatRelease(_ptr);
      }
   }
}
