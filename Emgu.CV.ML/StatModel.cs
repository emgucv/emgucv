using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;

namespace Emgu.CV.ML
{
   public abstract class StatModel : UnmanagedObject
   {
      public void Save(String fileName)
      {
         MlInvoke.StatModelSave(_ptr, fileName);
      }

      public void Clear()
      {
         MlInvoke.StatModelClear(_ptr);
      }
   }
}
