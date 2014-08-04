//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Util;

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
      InputArray GetInputArray();
   }

   /// <summary>
   /// InputArrayOfArrays
   /// </summary>
   public interface IInputArrayOfArrays : IInputArray
   {
      
   }
}
