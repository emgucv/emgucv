//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
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
   /// This type is very similar to InputArray except that it is used for input/output function parameters.
   /// </summary>
   public interface IInputOutputArray
   {
      /// <summary>
      /// The unmanaged pointer to the input/output array
      /// </summary>
      IntPtr InputOutputArrayPtr { get; }
   }

   public partial class CvInvoke
   {
      /// <summary>
      /// Release the input / output array
      /// </summary>
      /// <param name="arr">Pointer to the input output array</param>
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      public extern static void cveInputOutputArrayRelease(ref IntPtr arr);

   }
}
