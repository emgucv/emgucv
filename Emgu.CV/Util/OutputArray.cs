//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
   /// <summary>
   /// This type is very similar to InputArray except that it is used for output function parameters. 
   /// </summary>
   public partial class OutputArray : InputArray
   {
      internal OutputArray()
         : base()
      {
      }

      /// <summary>
      /// Create an OutputArray from an existing unmanaged outputArray pointer
      /// </summary>
      /// <param name="outputArrayPtr">The pointer to the unmanaged outputArray</param>
      public OutputArray(IntPtr outputArrayPtr)
         : base(outputArrayPtr)
      {
         
      }

      private static OutputArray _empty = new OutputArray();

      /// <summary>
      /// Get an empty output array
      /// </summary>
      /// <returns>An empty output array</returns>
      public static new OutputArray GetEmpty()
      {
         return _empty;
      }

      /// <summary>
      /// Release the unmanaged memory associated with this output array.
      /// </summary>
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