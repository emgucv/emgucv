using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV
{
   /// <summary>
   /// Attributes used to specify color information
   /// </summary>
   [AttributeUsage(AttributeTargets.Struct)]
   public sealed class ColorInfoAttribute : System.Attribute
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
