//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;

namespace Emgu.CV
{
   /// <summary>
   /// The stereo matcher interface
   /// </summary>
   public interface IStereoMatcher
   {
      /// <summary>
      /// Pointer to the stereo matcher
      /// </summary>
      IntPtr StereoMatcherPtr { get; }
   }
}
