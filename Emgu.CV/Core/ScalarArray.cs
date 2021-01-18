//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
   /// <summary>
   /// An implementation of IInputArray intented to convert data to IInputArray
   /// </summary>
   public class ScalarArray : UnmanagedObject, IInputArray
   {
      static ScalarArray()
      {
         CvInvoke.Init();
      }

      private enum DataType
      {
         Scalar, 
         Double
      }
      
      private DataType _dataType;
      
      /// <summary>
      /// Create an InputArray from MCvScalar
      /// </summary>
      /// <param name="scalar">The MCvScalar to be converted to InputArray</param>
      public ScalarArray(MCvScalar scalar)
      {
         _ptr = cveScalarCreate(ref scalar);
         _dataType = DataType.Scalar;
         
      }

      /// <summary>
      /// Create an InputArray from a double value
      /// </summary>
      /// <param name="scalar">The double value to be converted to InputArray</param>
      public ScalarArray(double scalar)
      {
         _ptr = Marshal.AllocHGlobal(sizeof(double));
         _dataType = DataType.Double;
         Marshal.Copy(new double[] { scalar }, 0, _ptr, 1);
      }

      /// <summary>
      /// Convert double scalar to InputArray
      /// </summary>
      /// <param name="scalar">The double scalar</param>
      /// <returns>The InputArray</returns>
      public static explicit operator ScalarArray(double scalar)
      {
         return new ScalarArray(scalar);
      }

      /// <summary>
      /// Convert MCvScalar to InputArray
      /// </summary>
      /// <param name="scalar">The MCvScalar</param>
      /// <returns>The InputArray</returns>
      public static explicit operator ScalarArray(MCvScalar scalar)
      {
         return new ScalarArray(scalar);
      }

      /// <summary>
      /// Release all the unmanaged memory associated with this InputArray
      /// </summary>
      protected override void DisposeObject()
      {
         
         if (_ptr != IntPtr.Zero)
         {
            if (_dataType == DataType.Scalar)
               cveScalarRelease(ref _ptr);
            else if (_dataType == DataType.Double)
            {
               Marshal.FreeHGlobal(_ptr);
               _ptr = IntPtr.Zero;
            }

            Debug.Assert(_ptr == IntPtr.Zero);
         }
      }

      /// <summary>
      /// The pointer to the input array
      /// </summary>
      /// <returns>The input array</returns>
      public InputArray GetInputArray()
      {
         if (_dataType == DataType.Scalar)
            return new InputArray(cveInputArrayFromScalar(_ptr), this);
         else if (_dataType == DataType.Double)
         {
            return new InputArray(cveInputArrayFromDouble(_ptr), this);
         }
         else
         {
            throw new NotImplementedException("Not implemented");
         }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveScalarCreate(ref MCvScalar scalar);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveScalarRelease(ref IntPtr scalar);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputArrayFromScalar(IntPtr scalar);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern IntPtr cveInputArrayFromDouble(IntPtr scalar);

   }
}