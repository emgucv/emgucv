//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Features
{
   /// <summary>
   /// Library to invoke Features functions
   /// </summary>
   public static partial class FeaturesInvoke
   {
      static FeaturesInvoke()
      {
         CvInvoke.Init();
      }
   }
}
