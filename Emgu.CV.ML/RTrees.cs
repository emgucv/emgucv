//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;
using System.Diagnostics;

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
         MlInvoke.CvRTreesRelease(ref _ptr);
      }

      /// <summary>
      /// Train the random tree using the specific traning data
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="tflag">data layout type</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="varIdx">Can be null if not needed. When specified, identifies variables (features) of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&lt;int&gt; of nx1</param>
      /// <param name="varType">The types of input variables</param>
      /// <param name="missingMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <paramref name="trainData"/>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <param name="param">The parameters for training the random tree</param>
      /// <returns></returns>
      public bool Train(
         Matrix<float> trainData,
         MlEnum.DATA_LAYOUT_TYPE tflag,
         Matrix<float> responses,
         Matrix<Byte> varIdx,
         Matrix<Byte> sampleIdx,
         Matrix<Byte> varType,
         Matrix<Byte> missingMask,
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
      /// <param name="missingDataMask">Can be null if not needed. When specified, it is an 8-bit matrix of the same size as <i>trainData</i>, is used to mark the missed values (non-zero elements of the mask)</param>
      /// <returns>The cumulative result from all the trees in the forest (the class that receives the majority of voices, or the mean of the regression function estimates)</returns>
      public float Predict(
         Matrix<float> sample,
         Matrix<Byte> missingDataMask)
      {
         return MlInvoke.CvRTreesPredict(_ptr, sample.Ptr, missingDataMask == null ? IntPtr.Zero : missingDataMask.Ptr);
      }

      /// <summary>
      /// Get the number of Trees in the Random tree
      /// </summary>
      public int TreeCount
      {
         get
         {
            return MlInvoke.CvRTreesGetTreeCount(Ptr);
         }
      }

      /// <summary>
      /// Get the variable importance matrix
      /// </summary>
      public Matrix<float> VarImportance
      {
         get
         {
            IntPtr matPtr = MlInvoke.CvRTreesGetVarImportance(Ptr);
            if (matPtr == IntPtr.Zero) return null;
            MCvMat mat = (MCvMat)Marshal.PtrToStructure(matPtr, typeof(MCvMat));

            Matrix<float> res = new Matrix<float>(mat.rows, mat.cols, 1, mat.data, mat.step);
            Debug.Assert(mat.type == res.MCvMat.type, "Matrix type is not float");
            return res;
         }
      }
   }
}
