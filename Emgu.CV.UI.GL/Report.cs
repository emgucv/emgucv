using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emgu.CV.UI.GLView
{
   public class Report
   {
#if ANDROID
      public static void Error(String packageName, String message)
      {
         Android.Util.Log.Error(packageName, message);
         /*
#if DEBUG
         throw new Exception(message);
#endif
          */
      }

      public static void Debug(String packageName, String message)
      {
#if DEBUG
         Android.Util.Log.Debug(packageName, message);
#endif
      }

      public static void Warn(String packageName, String message)
      {
         Android.Util.Log.Warn(packageName, message);
      }
#else
      public static void Error(String packageName, String message)
      {
         System.Diagnostics.Debug.WriteLine(String.Format("Error in {0}: {1}", packageName, message));
         throw new Exception(message);
      }

      public static void Debug(String packageName, String message)
      {
         System.Diagnostics.Debug.WriteLine(String.Format( "Debug in {0}: {1}", packageName, message));
      }

      public static void Warn(String packageName, String message)
      {
         System.Diagnostics.Debug.WriteLine(String.Format("Warning in {0}: {1}", packageName, message));
      }
#endif
   }
}
