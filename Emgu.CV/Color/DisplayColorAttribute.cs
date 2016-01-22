//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO || UNITY_EDITOR
using UnityEngine;
#elif NETFX_CORE
using Windows.UI;
#else
using System.Drawing;
#endif

namespace Emgu.CV
{
   [AttributeUsage(AttributeTargets.Property)]
   internal sealed class DisplayColorAttribute : Attribute
   {
      public DisplayColorAttribute(int blue, int green, int red)
      {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO || UNITY_EDITOR
         _displayColor = new Color(red/255.0f, green/255.0f, blue/255.0f, 1.0f);
#elif NETFX_CORE
         _displayColor = Color.FromArgb(255, (byte)red, (byte)green, (byte) blue);
#else
         _displayColor = Color.FromArgb(red, green, blue);
#endif
      }

      private Color _displayColor;

      public Color DisplayColor
      {
         get { return _displayColor; }
         set { _displayColor = value; }
      }
   }
}
