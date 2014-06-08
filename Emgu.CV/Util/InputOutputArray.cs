//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   public class InputOutputArray : UnmanagedObject
   {
      private InputOutputArray()
      {
      }

      public InputOutputArray(IntPtr intputOutputArrayPtr)
      {
         _ptr = intputOutputArrayPtr;
      }

      public static InputOutputArray GetEmpty()
      {
         return new InputOutputArray();
      }

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