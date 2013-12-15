//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
   public interface IInputArray
   {
      IntPtr InputArrayPtr { get; }
   }

   public partial class CvInvoke
   {
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static IntPtr cvInputArrayRelease(ref IntPtr arr);
   }

   public class InputArray : UnmanagedObject, IInputArray
   {
      private IntPtr _cvScalarPtr;
      private IntPtr _doublePtr;
      
      public InputArray(MCvScalar scalar)
      {
         _cvScalarPtr = cvScalarCreate(ref scalar);
         _ptr = cvInputArrayFromScalar(_cvScalarPtr);
      }
      public InputArray(double scalar)
      {
         _doublePtr = Marshal.AllocHGlobal(sizeof(double));
         Marshal.Copy(new double[] { scalar }, 0, _doublePtr, 1);
         _ptr = cvInputArrayFromDouble(_doublePtr);
      }

      public static explicit operator InputArray(double scalar)
      {
         return new InputArray(scalar);
      }

      public static explicit operator InputArray(MCvScalar scalar)
      {
         return new InputArray(scalar);
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cvInputArrayRelease(ref _ptr);

         if (_cvScalarPtr != IntPtr.Zero)
         {
            cvScalarRelease(ref _cvScalarPtr);
         }

         if (_doublePtr != IntPtr.Zero)
         {
            Marshal.FreeHGlobal(_doublePtr);
            _doublePtr = IntPtr.Zero;
         }
      }

      public IntPtr InputArrayPtr
      {
         get { return _ptr; }
      }

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvScalarCreate(ref MCvScalar scalar);
      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cvScalarRelease(ref IntPtr scalar);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvInputArrayFromScalar(IntPtr scalar);

      [DllImport(CvInvoke.EXTERN_LIBRARY, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cvInputArrayFromDouble(IntPtr scalar);
   }
}
