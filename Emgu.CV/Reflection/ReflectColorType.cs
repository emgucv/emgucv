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
      public static System.Drawing.Color[] GetChannelDisplayColor(ColorType color)
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
   }
}
