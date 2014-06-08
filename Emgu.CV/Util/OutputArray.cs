//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   public class OutputArray : UnmanagedObject
   {
      private OutputArray()
      {
      }

      public OutputArray(IntPtr outputArrayPtr)
      {
         _ptr = outputArrayPtr;
      }

      public static OutputArray GetEmpty()
      {
         return  new OutputArray();
      }

      protected override void DisposeObject()
      {
         if (_ptr != IntPtr.Zero)
            CvInvoke.cveOutputArrayRelease(ref _ptr);
      }
   }

   public partial class CvInvoke
   {
      /// <summary>
      /// Release the input / output array
      /// </summary>
      /// <param name="arr">Pointer to the input / output array</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal extern static void cveOutputArrayRelease(ref IntPtr arr);
   }
}