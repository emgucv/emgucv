//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Features
{
   /// <summary>
   /// Library to invoke Features2D functions
   /// </summary>
   public static partial class Features2DInvoke
   {
      static Features2DInvoke()
      {
         CvInvoke.Init();
      }
   }
}
