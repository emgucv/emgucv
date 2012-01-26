//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// Structure contains the bounding box and confidence level for detected object
   /// </summary>
   [StructLayout(LayoutKind.Sequential)]
   public struct MCvObjectDetection
   {
      /// <summary>
      /// Bounding box for a detected object
      /// </summary>
      public Rectangle Rect;

      /// <summary>
      /// Confidence level 
      /// </summary>
      public float score;
   }
}
