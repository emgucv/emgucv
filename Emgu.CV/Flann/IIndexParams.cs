//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Flann
{
   /// <summary>
   /// The index parameters interface
   /// </summary>
   public interface IIndexParams
   {
      /// <summary>
      /// Gets the pointer to the index parameter.
      /// </summary>
      /// <value>
      /// The index parameter pointer.
      /// </value>
      IntPtr IndexParamPtr { get; }
   }
}
