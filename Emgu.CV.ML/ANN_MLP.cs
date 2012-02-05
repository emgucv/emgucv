//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.ML.Structure;

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
      /// <param name="fParam1">Free parameters of the activation function, alpha</param>
      /// <param name="fParam2">Free parameters of the activation function, beta</param>
      public ANN_MLP(Matrix<int> layerSize, MlEnum.ANN_MLP_ACTIVATION_FUNCTION activeFunction, double fParam1, double fParam2)
      {
         _ptr = MlInvoke.CvANN_MLPCreate(layerSize.Ptr, activeFunction, fParam1, fParam2);
      }

      /// <summary>
      /// Release the memory associated with this neural network
      /// </summary>
      protected override void DisposeObject()
      {
         MlInvoke.CvANN_MLPRelease(ref _ptr);
      }

      /// <summary>
      /// Train the ANN_MLP model with the specific paramters
      /// </summary>
      /// <param name="trainData">The training data. A 32-bit floating-point, single-channel matrix, one vector per row</param>
      /// <param name="responses">A floating-point matrix of the corresponding output vectors, one vector per row. </param>
      /// <param name="sampleWeights">It is not null only for RPROP. The optional floating-point vector of weights for each sample. Some samples may be more important than others for training, e.g. user may want to gain the weight of certain classes to find the right balance between hit-rate and false-alarm rate etc</param>
      /// <param name="sampleIdx">Can be null if not needed. When specified, identifies samples of interest. It is a Matrix&gt;int&lt; of nx1</param>
      /// <param name="parameters">The parameters for ANN_MLP</param>
      /// <param name="flag">The traning flag</param>
      /// <returns>The number of done iterations</returns>
      public int Train(
         Matrix<float> trainData,
         Matrix<float> responses,
         Matrix<float> sampleWeights,
         Matrix<Byte> sampleIdx,
         MCvANN_MLP_TrainParams parameters,
         MlEnum.ANN_MLP_TRAINING_FLAG flag)
      {
         return 
            MlInvoke.CvANN_MLPTrain(
               _ptr,
               trainData.Ptr,
               responses.Ptr,
               sampleWeights == null? IntPtr.Zero : sampleWeights.Ptr,
               sampleIdx == null ? IntPtr.Zero : sampleIdx.Ptr,
               ref parameters,
               flag);
      }

      /// <summary>
      /// Predit the response of the <paramref name="samples"/>
      /// </summary>
      /// <param name="samples">The input samples</param>
      /// <param name="outputs">The prediction results, should have the same # of rows as the <paramref name="samples"/></param>
      /// <returns>In case of classification the method returns the class label, in case of regression - the output function value</returns>
      public float Predict(Matrix<float> samples, Matrix<float> outputs)
      {
         return MlInvoke.CvANN_MLPPredict(_ptr, samples, outputs);
      }

      /// <summary>
      /// Get the number of layer in the neural network.
      /// </summary>
      public int LayerCount
      {
         get
         {
            return MlInvoke.CvANN_MLPGetLayerCount(_ptr);
         }
      }
   }
}
