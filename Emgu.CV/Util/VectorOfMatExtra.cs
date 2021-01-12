//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Emgu.CV.Util
{
   public partial class VectorOfMat : Emgu.Util.UnmanagedObject, IInputArray, IOutputArray, IInputOutputArray
   {
      /// <summary>
      /// Convert a CvArray to Mat and push it into the vector
      /// </summary>
      /// <typeparam name="TDepth">The type of depth of the cvArray</typeparam>
      /// <param name="cvArray">The cvArray to be pushed into the vector</param>
      public void Push<TDepth>(CvArray<TDepth> cvArray) where TDepth : new()
      {
            Push(cvArray.Mat);
      }

      /// <summary>
      /// Convert a group of CvArray to Mat and push them into the vector
      /// </summary>
      /// <typeparam name="TDepth">The type of depth of the cvArray</typeparam>
      /// <param name="values">The values to be pushed to the vector</param>
      public void Push<TDepth>(CvArray<TDepth>[] values) where TDepth : new()
      {
         foreach (CvArray<TDepth> value in values)
            Push(value);
      }
   }
}
