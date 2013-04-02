//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;

#if NETFX_CORE
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
#if NETFX_CORE
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
