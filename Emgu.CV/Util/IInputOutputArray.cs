//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
   /// This type is very similar to InputArray except that it is used for input/output function parameters.
   /// </summary>
   public interface IInputOutputArray : IInputArray, IOutputArray, IInputArrayOfArrays
   {
      /// <summary>
      /// The unmanaged pointer to the input/output array
      /// </summary>
      InputOutputArray GetInputOutputArray();
   }


}
