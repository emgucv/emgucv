//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.Structure
{
   /// <summary>
   /// Wrapped CvParamGrid structure used by SVM
   /// </summary>
   public struct MCvParamGrid
   {
      /// <summary>
      /// Minimum value
      /// </summary>
      public double min_val;
      /// <summary>
      /// Maximum value
      /// </summary>
      public double max_val;
      /// <summary>
      /// step
      /// </summary>
      public double step;
   }
}
