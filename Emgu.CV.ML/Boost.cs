using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Wrapped CvDTree class in machine learning library
   /// </summary>
   public class Boost : Emgu.CV.ML.StatModel
   {
      /// <summary>
      /// Create a default Boost classifier
      /// </summary>
      public Boost()
      {
         _ptr = MlInvoke.CvBoostCreate();
      }

      /// <summary>
      /// Release the Boost classifier and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvBoostRelease(_ptr);
      }
   }
}
