//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of Byte.
   /// </summary>
   public class VectorOfByte : Emgu.Util.UnmanagedObject, IInputArray, IOutputArray
   {
      private IntPtr _inputArrayPtr;
      private IntPtr _outputArrayPtr;

      /// <summary>
      /// Create an empty standard vector of Byte
      /// </summary>
      public VectorOfByte()
      {
         _ptr = VectorOfByteCreate();
      }

      /// <summary>
      /// Create an standard vector of Byte of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfByte(int size)
      {
         _ptr = VectorOfByteCreateSize(size);
      }

      /// <summary>
      /// Push an array of value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(float[] value)
      {
         if (value.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            VectorOfBytePushMulti(_ptr, handle.AddrOfPinnedObject(), value.Length);
            handle.Free();
         }
      }

      /// <summary>
      /// Get the size of the vector
      /// </summary>
      public int Size
      {
         get
         {
            return VectorOfByteGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfByteClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return VectorOfByteGetStartAddress(_ptr);
         }
      }

      /// <summary>
      /// Convert the standard vector to an array of Byte
      /// </summary>
      /// <returns>An array of Byte</returns>
      public Byte[] ToArray()
      {
         Byte[] res = new Byte[Size];
         if (res.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            VectorOfByteCopyData(_ptr, handle.AddrOfPinnedObject());
            handle.Free();
         }
         return res;
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            VectorOfByteRelease(_ptr);

         if (_inputArrayPtr != IntPtr.Zero)
            CvInvoke.cvInputArrayRelease(ref _inputArrayPtr);

         if (_outputArrayPtr != IntPtr.Zero)
            CvInvoke.cvOutputArrayRelease(ref _outputArrayPtr);
      }

      public IntPtr InputArrayPtr
      {
         get
         {
            if (_inputArrayPtr == IntPtr.Zero)
               _inputArrayPtr = cvInputArrayFromVectorOfByte(_ptr);
            return _inputArrayPtr;
         }
      }

      public IntPtr OutputArrayPtr
      {
         get
         {
            if (_outputArrayPtr == IntPtr.Zero)
               _outputArrayPtr = cvOutputArrayFromVectorOfByte(_ptr);
            return _outputArrayPtr;
         }
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfByteCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfByteCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfByteRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern int VectorOfByteGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfByteCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr VectorOfByteGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfBytePushMulti(IntPtr v, IntPtr values, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void VectorOfByteClear(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvInputArrayFromVectorOfByte(IntPtr vec);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cvOutputArrayFromVectorOfByte(IntPtr vec);
   }
}
