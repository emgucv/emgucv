//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;

namespace Emgu.CV.ML
{
   /// <summary>
   /// A statistic model
   /// </summary>
   public abstract class StatModel : UnmanagedObject
   {
      /// <summary>
      /// Save the statistic model to file
      /// </summary>
      /// <param name="fileName">The file name where this StatModel will be saved</param>
      public void Save(String fileName)
      {
         MlInvoke.StatModelSave(_ptr, fileName, IntPtr.Zero);
      }

      /// <summary>
      /// Load the statistic model from file
      /// </summary>
      /// <param name="fileName">The file to load the model from</param>
      public void Load(String fileName)
      {
         MlInvoke.StatModelLoad(_ptr, fileName, IntPtr.Zero);
      }

      /// <summary>
      /// Clear the statistic model
      /// </summary>
      public void Clear()
      {
         MlInvoke.StatModelClear(_ptr);
      }
   }
}
