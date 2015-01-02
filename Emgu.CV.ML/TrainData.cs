//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.CV.ML.MlEnum;
using Emgu.Util;
using Emgu.CV.ML.Structure;
using System.Runtime.InteropServices;

namespace Emgu.CV.ML
{
   /// <summary>
   /// Train data
   /// </summary>
   public class TrainData : UnmanagedObject
   {
      public TrainData(
         IInputArray samples, DataLayoutType layoutType, IInputArray response,
         IInputArray varIdx = null, IInputArray sampleIdx = null,
         IInputArray sampleWeight = null, IInputArray varType = null
         )
      {
         using (InputArray iaSamples = samples.GetInputArray())
         using (InputArray iaResponse = response.GetInputArray())
         using (InputArray iaVarIdx = varIdx == null ? InputArray.GetEmpty() : varIdx.GetInputArray())
         using (InputArray iaSampleIdx = sampleIdx == null ? InputArray.GetEmpty() : sampleIdx.GetInputArray())
         using (InputArray iaSampleWeight = sampleWeight == null ? InputArray.GetEmpty() : sampleWeight.GetInputArray())
         using (InputArray iaVarType = varType == null ? InputArray.GetEmpty() : varType.GetInputArray())
         {
            _ptr = MlInvoke.cveTrainDataCreate(iaSamples, layoutType, iaResponse, iaVarIdx, iaSampleIdx, iaSampleWeight,
               iaVarType);
         }
      }

      protected override void DisposeObject()
      {
         MlInvoke.cveTrainDataRelease(ref _ptr);
      }
   }
}
