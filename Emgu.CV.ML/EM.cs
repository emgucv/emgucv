using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Expectation Maximization model
   /// </summary>
   public class EM : StatModel
   {
      /// <summary>
      /// Create a Expectation Maximization model
      /// </summary>
      public EM()
      {
         _ptr = MlInvoke.CvEMDefaultCreate();
      }

      /// <summary>
      /// Train the EM model using the specific training data
      /// </summary>
      /// <param name="samples"></param>
      /// <param name="sampleIdx"></param>
      /// <param name="parameters"></param>
      /// <param name="labels"></param>
      /// <returns></returns>
      public bool Train(Matrix<float> samples, Matrix<float> sampleIdx, EMParams parameters, Matrix<Int32> labels)
      {
         return MlInvoke.CvEMTrain(
            _ptr, 
            samples.Ptr, 
            sampleIdx == null? IntPtr.Zero : sampleIdx.Ptr, 
            parameters.MCvEMParams, 
            labels.Ptr);
      }

      public float Predict(Matrix<float> samples, Matrix<float> probs)
      {
         return MlInvoke.CvEMPredict(
            _ptr, 
            samples.Ptr, 
            probs == null ? IntPtr.Zero : probs.Ptr);
      }

      /// <summary>
      /// Release the memory associated with this EM model
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvEMRelease(_ptr);
      }
   }
}
