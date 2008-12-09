using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Neural network
   /// </summary>
   public class ANN_MLP : StatModel
   {
      /// <summary>
      /// Create a neural network using the specific parameters
      /// </summary>
      /// <param name="layerSize">The size of the layer</param>
      /// <param name="activeFunction">Activation function</param>
      /// <param name="fParam1"></param>
      /// <param name="fParam2"></param>
      public ANN_MLP(Matrix<int> layerSize, MlEnum.ANN_MLP_ACTIVATION_FUNCTION activeFunction, double fParam1, double fParam2)
      {
         _ptr = MlInvoke.CvANN_MLPCreate(layerSize.Ptr, activeFunction, fParam1, fParam2);
      }

      /// <summary>
      /// Release the memory associated with this neural network
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvANN_MLPRelease(_ptr);
      }

      public int Train(
         Matrix<float> inputs,
         Matrix<float> outputs,
         Matrix<float> sampleWeights,
         Matrix<int> sampleIdx,
         MCvANN_MLP_TrainParams parameters,
         MlEnum.ANN_MLP_TRAINING_FLAG flag)
      {
         return 
            MlInvoke.CvANN_MLPTrain(
               _ptr,
               inputs.Ptr,
               outputs.Ptr,
               sampleWeights == null? IntPtr.Zero : sampleWeights.Ptr,
               sampleIdx == null ? IntPtr.Zero : sampleIdx.Ptr,
               parameters,
               flag);
      }

      public float Predict(Matrix<float> inputs, Matrix<float> outputs)
      {
         return MlInvoke.CvANN_MLPPredict(_ptr, inputs, outputs);
      }
   }
}
