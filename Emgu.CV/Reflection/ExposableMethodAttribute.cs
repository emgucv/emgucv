using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.CV.Reflection
{
   /// <summary>
   /// Attribute used by ImageBox to generate Operation Menu
   /// </summary>
   [AttributeUsage(AttributeTargets.Method)]
   public sealed class ExposableMethodAttribute : System.Attribute
   {
      private bool _exposable;
      private string _category;

      /// <summary>
      /// Get or Set the Exposable value, if true, this function will be displayed in Operation Menu of ImageBox
      /// </summary>
      public bool Exposable
      {
         get
         {
            return _exposable;
         }
         set
         {
            _exposable = value;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      public String Category
      {
         get
         {
            return _category;
         }
         set
         {
            _category = value;
         }
      }

      private String _genericParametesOptions;

      /// <summary>
      /// The options for generic parameters
      /// </summary>
      public String GenericParametersOptions
      {
         get { return _genericParametesOptions; }
         set { _genericParametesOptions = value; }
      }

      /// <summary>
      /// Constructor
      /// </summary>
      public ExposableMethodAttribute()
      {
         //_exposable = false;
         _category = "Various Tools";
         _genericParametesOptions = string.Empty;
      }
   }
}
