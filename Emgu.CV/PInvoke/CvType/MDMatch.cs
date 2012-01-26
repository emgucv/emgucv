//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

﻿using System;
using System.Text;

namespace Emgu.CV.Structure
{
   /// <summary>
   /// OpenCV's DMatch structure
   /// </summary>
   public struct MDMatch
   {
      /// <summary>
      /// Query descriptor index
      /// </summary>
      public int QueryIdx;

      /// <summary>
      /// Train descriptor index
      /// </summary>
      public int TrainIdx;

      /// <summary>
      /// Train image index
      /// </summary>
      public int ImgIdx;

      /// <summary>
      /// Distance
      /// </summary>
      public float distance;
   }
}
