using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// A Normal Bayes Classifier
   /// </summary>
   public class NormalBayesClassifier : StatModel
   {
      /// <summary>
      /// Create a normal Bayes classifier
      /// </summary>
      public NormalBayesClassifier()
      {
         _ptr = MlInvoke.CvNormalBayesClassifierDefaultCreate();
      }

      /// <summary>
      /// Create a normal bayes classifier using the specific training data
      /// </summary>
      /// <param name="trainData"></param>
      /// <param name="responses"></param>
      /// <param name="varIdx"></param>
      /// <param name="sampleIdx"></param>
      public NormalBayesClassifier(Matrix<float> trainData, Matrix<float> responses, Matrix<int> varIdx, Matrix<int> sampleIdx)
      {
         MlInvoke.CvNormalBayesClassifierCreate(trainData.Ptr, responses.Ptr, varIdx.Ptr, sampleIdx.Ptr);
      }

      /// <summary>
      /// Release the memory associated with this classifier
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvNormalBayesClassifierRelease(_ptr);
      }

      /// <summary>
      /// Train the classifier using the specific data
      /// </summary>
      /// <param name="trainData"></param>
      /// <param name="responses"></param>
      /// <param name="varIdx"></param>
      /// <param name="sampleIdx"></param>
      /// <param name="update"></param>
      /// <returns></returns>
      public bool Train(Matrix<float> trainData, Matrix<float> responses, Matrix<int> varIdx, Matrix<int> sampleIdx, bool update)
      {
         return MlInvoke.CvNormalBayesClassifierTrain(_ptr, trainData.Ptr, responses.Ptr, varIdx.Ptr, sampleIdx.Ptr, update);
      }

      public float Predict(Matrix<float> samples, out Matrix<float> results)
      {
         results = new Matrix<float>(samples.Rows, 1);
         return MlInvoke.CvNormalBayesClassifierPredict(_ptr, samples.Ptr, results.Ptr);
      }

   }
}
