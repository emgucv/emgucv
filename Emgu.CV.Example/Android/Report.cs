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
      }

      public static void Debug(String packageName, String message)
      {
         Android.Util.Log.Debug(packageName, message);
      }

   }
}
