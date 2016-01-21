//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Emgu.CV
{
   public class Report
   {
      public static void Error(String packageName, String message)
      {
         Android.Util.Log.Error(packageName, message);
         throw new Exception(message);
      }

      public static void Debug(String packageName, String message)
      {
         Android.Util.Log.Debug(packageName, message);
      }

      public static void Warn(String packageName, String message)
      {
         Android.Util.Log.Warn(packageName, message);
      }

   }
}
