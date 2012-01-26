//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Drawing;

namespace Emgu.CV
{
   [AttributeUsage(AttributeTargets.Property)]
   internal sealed class DisplayColorAttribute : Attribute
   {
      public DisplayColorAttribute(int blue, int green, int red)
      {
         _displayColor = Color.FromArgb(red, green, blue);
      }

      private Color _displayColor;

      public Color DisplayColor
      {
         get { return _displayColor; }
         set { _displayColor = value; }
      }
   }
}
