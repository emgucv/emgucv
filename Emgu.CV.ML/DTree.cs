//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Decision Tree 
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
      /// Train the decision tree using the specific traning data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="tflag">data layout type</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be null if not needed. When specified, identifies variables (features) of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="missingMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="param">The parameters for training the decision tree</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData,
         MlEnum.DATA_LAYOUT_TYPE tflag,
         Matrix<float> responses,
         Matrix<Byte> varIdx,
         Matrix<Byte> sampleIdx,
         Matrix<Byte> varType,
         Matrix<Byte> missingMask,
         MCvDTreeParams param)
      {
         return MlInvoke.CvDTreeTrain(
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
      /// The method takes the feature vector and the optional missing measurement mask on input, traverses the decision tree and returns the reached leaf node on output. The prediction result, either the class label or the estimated function value, may be retrieved as value field of the CvDTreeNode structure
      /// </summary>
      /// <param name="sample">The sample to be predicted</param>
      /// <param name="missingDataMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <i>trainData</i>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="rawMode">normally set to false that implies a regular input. If it is true, the method assumes that all the values of the discrete input variables have been already normalized to 0..num_of_categoriesi-1 ranges. (as the decision tree uses such normalized representation internally). It is useful for faster prediction with tree ensembles. For ordered input variables the flag is not used. </param>
      /// <returns>Pointer to the reached leaf node on output. The prediction result, either the class label or the estimated function value, may be retrieved as value field of the CvDTreeNode structure</returns>
      public MCvDTreeNode Predict(
         Matrix<float> sample,
         Matrix<Byte> missingDataMask,
         bool rawMode)
      {
         IntPtr node = MlInvoke.CvDTreePredict(_ptr, sample.Ptr, missingDataMask == null ? IntPtr.Zero : missingDataMask.Ptr, rawMode);
         return (MCvDTreeNode)Marshal.PtrToStructure(node, typeof(MCvDTreeNode));
      }

      /// <summary>
      /// Release the decision tree and all the memory associate with it
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvDTreeRelease(ref _ptr);
      }
   }
}
