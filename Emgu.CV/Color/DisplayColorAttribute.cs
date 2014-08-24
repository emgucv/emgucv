//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

#if NETFX_CORE
using Windows.UI;
#elif ( UNITY_ANDROID || UNITY_IPHONE )
using UnityEngine;
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
#if NETFX_CORE
         _displayColor = Color.FromArgb(255, (byte)red, (byte)green, (byte) blue);
#elif ( UNITY_ANDROID || UNITY_IPHONE )
         _displayColor = new Color(red/255.0f, green/255.0f, blue/255.0f, 1.0f);
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
