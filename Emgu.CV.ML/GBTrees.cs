/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Gradient Boosting Tree 
   /// </summary>
   public class GBTrees : Emgu.CV.ML.StatModel
   {
      /// <summary>
      /// Create a default Gradient Boosting Trees 
      /// </summary>
      public GBTrees()
      {
         _ptr = MlInvoke.CvGBTreesCreate();
      }

      /// <summary>
      /// Train the gradient boost trees using the specific traning data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="tflag">data layout type</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be null if not needed. When specified, the elements in the matrix contains the index to the variables (features) of interest. It is a Matrix&gt;int&lt; of nx1 where n %lt;= number of rows in <paramref name="trainData"/></param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, the elements in the matrix contains the index to the samples of interest. It is a Matrix&gt;int&lt; of nx1 where n %lt;= number of rows in <paramref name="trainData"/></param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="missingMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="param">The parameters for training the Gradient Boosting tree</param>
      /// <param name="update">specifies whether the classifier needs to be updated (i.e. the new weak tree classifiers added to the existing ensemble), or the classifier needs to be rebuilt from scratch</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData,
         MlEnum.DataLayoutType tflag,
         Matrix<float> responses,
         Matrix<int> varIdx,
         Matrix<int> sampleIdx,
         Matrix<Byte> varType,
         Matrix<Byte> missingMask,
         MCvGBTreesParams param,
         bool update)
      {
         return MlInvoke.CvGBTreesTrain(
            _ptr,
            trainData.Ptr,
            tflag,
            responses.Ptr,
            varIdx == null ? IntPtr.Zero : varIdx.Ptr,
            sampleIdx == null ? IntPtr.Zero : sampleIdx.Ptr,
            varType == null ? IntPtr.Zero : varType.Ptr,
            missingMask == null ? IntPtr.Zero : missingMask.Ptr,
            ref param,
            update);
      }

      /// <summary>
      /// Train the gradient boost trees using the specific traning data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="tflag">data layout type</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varMask">Can be null if not needed. When specified, it is a mask that identifies variables (features) of interest. It must be a Matrix&gt;Byte&lt; of n x 1 where n is the number of rows in <paramref name="trainData"/></param>
      /// <param name="sampleMask">Can be null if not needed. When specified, it is a mask identifies samples of interest. It must be a Matrix&gt;Byte&lt; of nx1, where n is the number of rows in <paramref name="trainData"/></param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="missingMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="param">The parameters for training the Gradient Boosting tree</param>
      /// <param name="update">specifies whether the classifier needs to be updated (i.e. the new weak tree classifiers added to the existing ensemble), or the classifier needs to be rebuilt from scratch</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData,
         MlEnum.DataLayoutType tflag,
         Matrix<float> responses,
         Matrix<Byte> varMask,
         Matrix<Byte> sampleMask,
         Matrix<Byte> varType,
         Matrix<Byte> missingMask,
         MCvGBTreesParams param,
         bool update)
      {
         return MlInvoke.CvGBTreesTrain(
            _ptr,
            trainData.Ptr,
            tflag,
            responses.Ptr,
            varMask == null ? IntPtr.Zero : varMask.Ptr,
            sampleMask == null ? IntPtr.Zero : sampleMask.Ptr,
            varType == null ? IntPtr.Zero : varType.Ptr,
            missingMask == null ? IntPtr.Zero : missingMask.Ptr,
            ref param,
            update);
      }

      /// <summary>
      /// Train the gradient boost trees using the specific traning data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="tflag">data layout type</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="missingMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="param">The parameters for training the Gradient Boosting tree</param>
      /// <param name="update">specifies whether the classifier needs to be updated (i.e. the new weak tree classifiers added to the existing ensemble), or the classifier needs to be rebuilt from scratch</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData,
         MlEnum.DataLayoutType tflag,
         Matrix<float> responses,
         Matrix<Byte> varType,
         Matrix<Byte> missingMask,
         MCvGBTreesParams param,
         bool update)
      {
         return MlInvoke.CvGBTreesTrain(
            _ptr,
            trainData.Ptr,
            tflag,
            responses.Ptr,
            IntPtr.Zero,
            IntPtr.Zero,
            varType == null ? IntPtr.Zero : varType.Ptr,
            missingMask == null ? IntPtr.Zero : missingMask.Ptr,
            ref param,
            update);
      }


      /// <summary>
      /// The method takes the feature vector and the optional missing measurement mask on input, traverses the random tree and returns the cumulative result from all the trees in the forest (the class that receives the majority of voices, or the mean of the regression function estimates)
      /// </summary>
      /// <param name="sample">The sample to be predicted</param>
      /// <param name="missingDataMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <i>trainData</i>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="weakResponses">Can be null if not needed. a floating-point vector, of responses from each individual weak classifier. The number of elements in the vector must be equal to the slice length.</param>
      /// <param name="slice">The continuous subset of the sequence of weak classifiers to be used for prediction</param>
      /// <param name="rawMode">Normally set to false that implies a regular input. If it is true, the method assumes that all the values of the discrete input variables have been already normalized to 0..num_of_categoriesi-1 ranges. (as the decision tree uses such normalized representation internally). It is useful for faster prediction with tree ensembles. For ordered input variables the flag is not used. </param>    
      /// <returns>The cumulative result from all the trees in the forest (the class that receives the majority of voices, or the mean of the regression function estimates)</returns>
      public float Predict(
         Matrix<float> sample,
         Matrix<Byte> missingDataMask,
         Matrix<float> weakResponses,
         MCvSlice slice,
         bool rawMode
         )
      {
         return MlInvoke.CvGBTreesPredict(
            _ptr,
            sample.Ptr,
            missingDataMask == null ? IntPtr.Zero : missingDataMask.Ptr,
            weakResponses == null ? IntPtr.Zero : weakResponses.Ptr,
            ref slice,
            rawMode);
      }

      /// <summary>
      /// Release the Gradient Boosting Trees classifier and all the unmanaged memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvGBTreesRelease(ref _ptr);
      }
   }
}
*/