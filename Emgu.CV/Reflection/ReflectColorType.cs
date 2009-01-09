using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Reflection
{
   /// <summary>
   /// A collection of reflection function that can be applied to ColorType object
   /// </summary>
   public static class ReflectColorType
   {
      /// <summary>
      /// Get the display color for each channel
      /// </summary>
      public static System.Drawing.Color[] GetDisplayColorOfChannels(IColor color)
      {
         List<System.Drawing.Color> channelColor = new List<System.Drawing.Color>();
         foreach (System.Reflection.PropertyInfo pInfo in color.GetType().GetProperties())
         {
            Object[] displayAtts = pInfo.GetCustomAttributes(typeof(DisplayColorAttribute), true);
            if (displayAtts.Length > 0)
            {
               channelColor.Add(((DisplayColorAttribute)displayAtts[0]).DisplayColor);
            }
         }
         if (channelColor.Count > 0) return channelColor.ToArray();

         System.Drawing.Color[] res = new System.Drawing.Color[color.Dimension];
         for (int i = 0; i < color.Dimension; i++)
         {
            res[i] = System.Drawing.Color.Gray;
         }
         return res;
      }

      /// <summary>
      /// Get the names for each channel
      /// </summary>
      public static String[] GetNamesOfChannels(IColor t)
      {
         List<String> channelNames = new List<string>();
         foreach (System.Reflection.PropertyInfo pInfo in t.GetType().GetProperties())
         {
            Object[] displayAtts = pInfo.GetCustomAttributes(typeof(DisplayColorAttribute), true);
            if (displayAtts.Length > 0)
            {
               channelNames.Add(pInfo.Name);
            }
         }
         if (channelNames.Count > 0) return channelNames.ToArray();

         String[] res = new string[t.Dimension];
         for (int i = 0; i < t.Dimension; i++)
         {
            res[i] = "Channel " + i;
         }
         return res;
      }
   }
}
