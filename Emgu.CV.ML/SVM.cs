using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Support Vector Machine 
   /// </summary>
   public class SVM : StatModel
   {
      /// <summary>
      /// Create a support Vector Machine
      /// </summary>
      public SVM()
      {
         _ptr = MlInvoke.CvSVMDefaultCreate();
      }

      /// <summary>
      /// Release all the memory associated with the SVM
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvSVMRelease(_ptr);
      }

      public bool Train(
         Matrix<float> trainData,
         Matrix<float> responses,
         Matrix<int> varIdx,
         Matrix<int> sampleIdx,
         SVMParams parameters)
      {
         return MlInvoke.CvSVMTrain(
            _ptr, 
            trainData.Ptr, 
            responses.Ptr, 
            varIdx == null ? IntPtr.Zero: varIdx.Ptr, 
            sampleIdx == null ? IntPtr.Zero : varIdx.Ptr, 
            parameters.MCvSVMParams);
      }
   }
}
