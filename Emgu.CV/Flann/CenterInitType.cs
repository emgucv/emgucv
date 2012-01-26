//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Flann
{
   /// <summary>
   /// The Kmeans center initiation types
   /// </summary>
   public enum CenterInitType
   {
      /// <summary>
      /// Random
      /// </summary>
      RANDOM = 0,
      /// <summary>
      /// 
      /// </summary>
      GONZALES = 1,
      /// <summary>
      /// 
      /// </summary>
      KMEANSPP = 2
   }
}
