//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Features2D
{
   public static partial class Features2DInvoke
   {
      static Features2DInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }
   }
}
