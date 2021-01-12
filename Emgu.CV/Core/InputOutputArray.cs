//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// This type is very similar to InputArray except that it is used for input/output function parameters.
   /// </summary>
   public class InputOutputArray : OutputArray
   {
      internal InputOutputArray()
         : base()
      {
      }

      /// <summary>
      /// Create an InputOutputArray from an existing unmanaged inputOutputArray pointer
      /// </summary>
      /// <param name="inputOutputArrayPtr">The pointer to the existing inputOutputArray</param>
      /// <param name="parent">The parent object to keep reference to</param>
      public InputOutputArray(IntPtr inputOutputArrayPtr, object parent)
         : base(inputOutputArrayPtr, parent)
      {
      }

      private static InputOutputArray _empty = new InputOutputArray();

      /// <summary>
      /// Get an empty InputOutputArray
      /// </summary>
      /// <returns>An empty InputOutputArray</returns>
      public static new InputOutputArray GetEmpty()
      {
         return _empty;
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