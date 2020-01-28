//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE || UNITY_METRO || UNITY_EDITOR
using UnityEngine;
#elif NETFX_CORE
using Windows.UI;
#else
using System.Drawing;
#endif

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
      /// <param name="color">The color</param>
      /// <returns>The display color for each channel</returns>
      public static Color[] GetDisplayColorOfChannels(IColor color)
      {
         List<Color> channelColor = new List<Color>();
         foreach (System.Reflection.PropertyInfo pInfo in color.GetType().GetProperties())
         {
            object[] displayAtts = pInfo.GetCustomAttributes(typeof(DisplayColorAttribute), true);
            if (displayAtts.Length > 0)
               channelColor.Add(((DisplayColorAttribute)displayAtts[0]).DisplayColor);
         }
         if (channelColor.Count > 0) return channelColor.ToArray();

         //create default color
         Color[] res = new Color[color.Dimension];
         for (int i = 0; i < res.Length; i++)
            //res[i] = Color.FromArgb(255, 125, 125, 125);
#if ( UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE || UNITY_METRO )
            res[i] = Color.gray;
#else
            res[i] = Color.Gray;
#endif
         return res;
      }

      /// <summary>
      /// Get the names of the channels
      /// </summary>
      /// <param name="color">The color</param>
      /// <returns>The names of the channels</returns>
      public static String[] GetNamesOfChannels(IColor color)
      {
         List<String> channelNames = new List<string>();
         foreach (System.Reflection.PropertyInfo pInfo in color.GetType().GetProperties())
         {
            if (pInfo.GetCustomAttributes(typeof(DisplayColorAttribute), true).Length > 0)
               channelNames.Add(pInfo.Name);
         }
         if (channelNames.Count > 0) return channelNames.ToArray();

         //Create default channel names
         String[] res = new string[color.Dimension];
         for (int i = 0; i < res.Length; i++)
            res[i] = String.Format("Channel {0}", i);
         return res;
      }

   }
}
