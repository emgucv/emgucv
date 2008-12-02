using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.ML
{
   /// <summary>
   /// The KNearest classifier
   /// </summary>
   public class KNearest : StatModel
   {
      /// <summary>
      /// Create a default KNearest classifier
      /// </summary>
      public KNearest()
      {
         _ptr = MlInvoke.CvKNearestDefaultCreate();
      }

      /// <summary>
      /// Creaet a KNearest classifier using the specific traing data
      /// </summary>
      /// <param name="trainData"></param>
      /// <param name="responses"></param>
      /// <param name="sampleIdx"></param>
      /// <param name="isRegression"></param>
      /// <param name="maxK"></param>
      public KNearest(Matrix<float> trainData, Matrix<float> responses, Matrix<int> sampleIdx, bool isRegression, int maxK)
      {
         _ptr = MlInvoke.CvKNearestCreate(trainData, responses, sampleIdx == null? IntPtr.Zero : sampleIdx.Ptr, isRegression, maxK);
      }

      /// <summary>
      /// Release the classifer and all the memory associated with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvKNearestRelease(_ptr);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="samples">The sample matrix where each row is a sample</param>
      /// <param name="k">The number of nearest neighbor to find</param>
      /// <param name="results">
      /// If regression, return a mean value of the particular vector's neighbor responses;
      /// If classification, return the class determined by voting.
      /// </param>
      /// <param name="neighborResponses"></param>
      /// <param name="dist"></param>
      /// <returns></returns>
      public float FindNearest(
         Matrix<float> samples, 
         int k, 
         Matrix<float> results, 
         Matrix<float> neighborResponses, 
         Matrix<float> dist)
      {
         IntPtr[] neighbors = new IntPtr[samples.Rows * k];
         return MlInvoke.CvKNearestFindNearest(_ptr, samples.Ptr, k, results.Ptr, neighbors, neighborResponses, dist);
      }
   }
}
