using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.ML
{
   /// <summary>
   /// CvParamGrid used by SVM
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
