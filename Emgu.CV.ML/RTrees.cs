using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Random tree
   /// </summary>
   public class RTrees : Emgu.CV.ML.StatModel
   {
      /// <summary>
      /// Create a random tree
      /// </summary>
      public RTrees()
      {
         _ptr = MlInvoke.CvRTreesCreate();
      }

      /// <summary>
      /// Release the random tree and all memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvRTreesRelease(_ptr);
      }

      /// <summary>
      /// Train the random tree using the specific traning data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="tflag">data layout type</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be null if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="missingMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="param">The parameters for training the random tree</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData,
         MlEnum.DATA_LAYOUT_TYPE tflag,
         Matrix<float> responses,
         Matrix<int> varIdx,
         Matrix<int> sampleIdx,
         Matrix<int> varType,
         Matrix<int> missingMask,
         MCvRTParams param)
      {
         return MlInvoke.CvRTreesTrain(
            _ptr,
            trainData.Ptr,
            tflag,
            responses.Ptr,
            varIdx == null ? IntPtr.Zero : varIdx.Ptr,
            sampleIdx == null ? IntPtr.Zero : sampleIdx.Ptr,
            varType == null ? IntPtr.Zero : varType.Ptr,
            missingMask == null ? IntPtr.Zero : missingMask.Ptr,
            param);
      }

      /// <summary>
      /// The method takes the feature vector and the optional missing measurement mask on input, traverses the random tree and returns the cumulative result from all the trees in the forest (the class that receives the majority of voices, or the mean of the regression function estimates)
      /// </summary>
      /// <param name="sample">The sample to be predicted</param>
      /// <param name="missingDataMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <returns>The cumulative result from all the trees in the forest (the class that receives the majority of voices, or the mean of the regression function estimates)</returns>
      public float Predict(
         Matrix<float> sample,
         Matrix<int> missingDataMask)
      {
         return MlInvoke.CvRTreesPredict(_ptr, sample.Ptr, missingDataMask == null ? IntPtr.Zero : missingDataMask.Ptr);
      }
   }
}
