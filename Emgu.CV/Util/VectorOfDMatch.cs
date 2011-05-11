//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;

namespace Emgu.CV.Util
{
   /// <summary>
   /// Wraped class of the C++ standard vector of DMatch.
   /// </summary>
   public class VectorOfDMatch : Emgu.Util.UnmanagedObject
   {
      #region PInvoke
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfDMatchCreate();

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfDMatchCreateSize(int size);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfDMatchRelease(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern int VectorOfDMatchGetSize(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfDMatchCopyData(IntPtr v, IntPtr data);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern IntPtr VectorOfDMatchGetStartAddress(IntPtr v);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfDMatchPushMulti(IntPtr v, IntPtr values, int count);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      private static extern void VectorOfDMatchClear(IntPtr v);
      #endregion

      /// <summary>
      /// Create an empty standard vector of DMatch
      /// </summary>
      public VectorOfDMatch()
      {
         _ptr = VectorOfDMatchCreate();
      }

      /// <summary>
      /// Create an standard vector of DMatch of the specific size
      /// </summary>
      /// <param name="size">The size of the vector</param>
      public VectorOfDMatch(int size)
      {
         _ptr = VectorOfDMatchCreateSize(size);
      }

      /// <summary>
      /// Push an array of value into the standard vector
      /// </summary>
      /// <param name="value">The value to be pushed to the vector</param>
      public void Push(MDMatch[] value)
      {
         if (value.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            VectorOfDMatchPushMulti(_ptr, handle.AddrOfPinnedObject(), value.Length);
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
            return VectorOfDMatchGetSize(_ptr);
         }
      }

      /// <summary>
      /// Clear the vector
      /// </summary>
      public void Clear()
      {
         VectorOfDMatchClear(_ptr);
      }

      /// <summary>
      /// The pointer to the first element on the vector. In case of an empty vector, IntPtr.Zero will be returned.
      /// </summary>
      public IntPtr StartAddress
      {
         get
         {
            return VectorOfDMatchGetStartAddress(_ptr);
         }
      }

      /// <summary>
      /// Convert the standard vector to an array of DMatch
      /// </summary>
      /// <returns>An array of DMatch</returns>
      public MDMatch[] ToArray()
      {
         MDMatch[] res = new MDMatch[Size];
         if (res.Length > 0)
         {
            GCHandle handle = GCHandle.Alloc(res, GCHandleType.Pinned);
            VectorOfDMatchCopyData(_ptr, handle.AddrOfPinnedObject());
            handle.Free();
         }
         return res;
      }

      /// <summary>
      /// Release the standard vector
      /// </summary>
      protected override void DisposeObject()
      {
         VectorOfDMatchRelease(_ptr);
      }
   }
}
