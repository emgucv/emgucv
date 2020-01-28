//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV
{
   /// <summary>
   /// Attribute used to specify color information
   /// </summary>
   [AttributeUsage(AttributeTargets.Struct)]
   public sealed class ColorInfoAttribute : Attribute
   {
      /// <summary>
      /// The code which is used for color conversion
      /// </summary>
      private String _conversionCodename;

      /// <summary>
      /// The code which is used for color conversion
      /// </summary>
      public String ConversionCodename
      {
         get { return _conversionCodename; }
         set { _conversionCodename = value; }
      }

      /// <summary>
      /// The code which is used for color conversion
      /// </summary>
      public ColorInfoAttribute()
      {
         _conversionCodename = String.Empty;
      }
   }
}
