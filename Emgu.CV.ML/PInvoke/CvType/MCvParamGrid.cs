//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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
      public double MinVal;
      /// <summary>
      /// Maximum value
      /// </summary>
      public double MaxVal;
      /// <summary>
      /// step
      /// </summary>
      public double Step;
   }
}
