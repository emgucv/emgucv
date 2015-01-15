//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
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
      Random = 0,
      /// <summary>
      /// 
      /// </summary>
      Gonzales = 1,
      /// <summary>
      /// 
      /// </summary>
      Kmeanspp = 2
   }
}
