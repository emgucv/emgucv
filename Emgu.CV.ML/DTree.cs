using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Wrapped CvDTree class in machine learning library
   /// </summary>
   public class DTree : Emgu.CV.ML.StatModel
   {
      /// <summary>
      /// Create a default decision tree
      /// </summary>
      public DTree()
      {
         _ptr = MlInvoke.CvDTreeCreate();
      }

      /// <summary>
      /// Release the decision tree and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvDTreeRelease(_ptr);
      }
   }
}
