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
   /// <summary>
   /// This is the proxy class for passing read-only input arrays into OpenCV functions.
   /// </summary>
   public interface IInputArray
   {
      /// <summary>
      /// The unmanaged pointer to the input array.
      /// </summary>
      IntPtr InputArrayPtr { get; }
   }

   public partial class CvInvoke
   {
      /// <summary>
      /// Release the InputArray
      /// </summary>
      /// <param name="arr">Pointer to the input array</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cveInputArrayRelease(ref IntPtr arr);
   }

   /// <summary>
   /// An implementation of IInputArray intented to convert data to IInputArray
   /// </summary>
   public class ScalarArray : UnmanagedObject, IInputArray
   {
      static ScalarArray()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      private enum DataType
      {
         Scalar, 
         Double
      }
      private IntPtr _dataPtr;
      private DataType _dataType;
      
      /// <summary>
      /// Create an InputArray from MCvScalar
      /// </summary>
      /// <param name="scalar">The MCvScalar to be converted to InputArray</param>
      public ScalarArray(MCvScalar scalar)
      {
         _dataPtr = cveScalarCreate(ref scalar);
         _dataType = DataType.Scalar;
         _ptr = cveInputArrayFromScalar(_dataPtr);
      }

      /// <summary>
      /// Create an InputArray from a double value
      /// </summary>
      /// <param name="scalar">The double value to be converted to InputArray</param>
      public ScalarArray(double scalar)
      {
         _dataPtr = Marshal.AllocHGlobal(sizeof(double));
         _dataType = DataType.Double;
         Marshal.Copy(new double[] { scalar }, 0, _dataPtr, 1);
         _ptr = cveInputArrayFromDouble(_dataPtr);
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
      /// Convert MCvSalar to InputArray
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
            CvInvoke.cveInputArrayRelease(ref _ptr);

         if (_dataPtr != IntPtr.Zero)
         {
            if (_dataType == DataType.Scalar)
               cveScalarRelease(ref _dataPtr);
            else if (_dataType == DataType.Double)
            {
               Marshal.FreeHGlobal(_dataPtr);
               _dataPtr = IntPtr.Zero;
            }

            Debug.Assert(_dataPtr == IntPtr.Zero);
         }
      }

      /// <summary>
      /// The pointer to the input array
      /// </summary>
      public IntPtr InputArrayPtr
      {
         get { return _ptr; }
      }

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveScalarCreate(ref MCvScalar scalar);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveScalarRelease(ref IntPtr scalar);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputArrayFromScalar(IntPtr scalar);

      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static IntPtr cveInputArrayFromDouble(IntPtr scalar);

   }
}
