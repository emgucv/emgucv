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

      /// <summary>
      /// Get the default parameter grid for the specific SVM type
      /// </summary>
      /// <param name="type">The SVM type</param>
      /// <returns>The default parameter grid for the specific SVM type </returns>
      public static MCvParamGrid GetDefaultGrid(MlEnum.SVM_TYPE type)
      {
         MCvParamGrid grid = new MCvParamGrid();
         MlInvoke.CvSVMGetDefaultGrid(type, ref grid);
         return grid;
      }

      public bool TrainAuto(
         Matrix<float> trainData,
         Matrix<float> responses,
         Matrix<int> varIdx,
         Matrix<int> sampleIdx,
         MCvSVMParams parameters,
         int kFold,
         ref MCvParamGrid cGrid,
         ref MCvParamGrid gammaGrid,
         ref MCvParamGrid pGrid,
         ref MCvParamGrid nuGrid,
         ref MCvParamGrid coefGrid,
         ref MCvParamGrid degreeGrid)
      {
         return MlInvoke.CvSVMTrainAuto(
            Ptr,
            trainData.Ptr,
            responses.Ptr,
            varIdx == null ? IntPtr.Zero : varIdx.Ptr,
            sampleIdx == null ? IntPtr.Zero : sampleIdx.Ptr,
            parameters,
            kFold,
            cGrid,
            gammaGrid, 
            pGrid, 
            nuGrid,
            coefGrid,
            degreeGrid);
      }
   }
}
