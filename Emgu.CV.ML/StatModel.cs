//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.ML.MlEnum;
using Emgu.Util;

namespace Emgu.CV.ML
{
   public interface IStatModel : IAlgorithm
   {
      /// <summary>
      /// Return the pointer to the algorithm object
      /// </summary>
      /// <returns>The pointer to the algorithm object</returns>
      IntPtr StatModelPtr { get; }
   }

   /// <summary>
   /// A statistic model
   /// </summary>
   public static class StatModelExtensions
   {
      /// <summary>
      /// Save the statistic model to file
      /// </summary>
      /// <param name="fileName">The file name where this StatModel will be saved</param>
      public static void Save(this IStatModel model, String fileName)
      {
         using (CvString fs = new CvString(fileName))
            MlInvoke.StatModelSave(model.StatModelPtr, fs);
      }

      public static bool Train(this IStatModel model, IInputArray samples, DataLayoutType layoutType, IInputArray responses)
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (InputArray iaResponses = responses.GetInputArray())
         {
            return MlInvoke.StatModelTrain(model.StatModelPtr, iaSamples, layoutType, iaResponses);
         }
      }

      public static bool Train(this IStatModel model, TrainData trainData, int flags = 0)
      {
         return MlInvoke.StatModelTrainWithData(model.StatModelPtr, trainData, flags);
      }

      public static float Predict(this IStatModel model, IInputArray samples, IOutputArray results = null, int flags = 0)
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (OutputArray oaResults = results == null ? OutputArray.GetEmpty() : results.GetOutputArray())
         {
            return MlInvoke.StatModelPredict(model.StatModelPtr, iaSamples, oaResults, flags);
         }
      }

      /*
      /// <summary>
      /// Load the statistic model from file
      /// </summary>
      /// <param name="fileName">The file to load the model from</param>
      public void Load(String fileName)
      {
         using (CvString fs = new CvString(fileName))
         MlInvoke.StatModelLoad(_ptr, fs);
      }*/

      /// <summary>
      /// Clear the statistic model
      /// </summary>
      public static void Clear(this IStatModel model)
      {
         MlInvoke.StatModelClear(model.StatModelPtr);
      }
   }
}
