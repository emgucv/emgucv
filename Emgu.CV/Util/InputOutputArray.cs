//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// This type is very similar to InputArray except that it is used for input/output function parameters.
   /// </summary>
   public class InputOutputArray : UnmanagedObject
   {
      private InputOutputArray()
      {
      }

      /// <summary>
      /// Create an InputOutputArray from an existing unmanaged inputOutputArray pointer
      /// </summary>
      /// <param name="inputOutputArrayPtr">The pointer to the existing inputOutputArray</param>
      public InputOutputArray(IntPtr inputOutputArrayPtr)
      {
         _ptr = inputOutputArrayPtr;
      }

      /// <summary>
      /// Get an empty InputOutputArray
      /// </summary>
      /// <returns>An empty InputOutputArray</returns>
      public static InputOutputArray GetEmpty()
      {
         return new InputOutputArray();
      }

      /// <summary>
      /// Release all the memory associated with this InputOutputArry
      /// </summary>
      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveInputOutputArrayRelease(ref _ptr);
      }
   }

   public partial class CvInvoke
   {
      /// <summary>
      /// Release the input / output array
      /// </summary>
      /// <param name="arr">Pointer to the input output array</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveInputOutputArrayRelease(ref IntPtr arr);

   }
}