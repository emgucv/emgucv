//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// This type is very similar to InputArray except that it is used for output function parameters.
   /// </summary>
   public interface IOutputArray : IInputArray
   {
      /// <summary>
      /// The unmanaged pointer to the output array
      /// </summary>
      /// <returns>Get the output array</returns>
      OutputArray GetOutputArray();
   }

   /// <summary>
   /// OutputArrayOfArrays
   /// </summary>
   public interface IOutputArrayOfArrays : IOutputArray
   {

   }
}
