//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.Reflection
{
   /// <summary>
   /// Attribute used by ImageBox to generate Operation Menu
   /// </summary>
   [AttributeUsage(AttributeTargets.Method)]
   public sealed class ExposableMethodAttribute : Attribute
   {
      private bool _exposable;
      private string _category;
      private Type[] _genericParametesOptions;

      /// <summary>
      /// Get or Set the exposable value, if true, this function will be displayed in Operation Menu of ImageBox
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
      /// The catefory of this function
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

      private int[] _genericParametersOptionSizes;

      /// <summary>
      /// The size for each generic parameter Options
      /// </summary>
      public int[] GenericParametersOptionSizes
      {
         get { return _genericParametersOptionSizes; }
         set { _genericParametersOptionSizes = value; }
      }


      /// <summary>
      /// The options for generic parameters
      /// </summary>
      public Type[] GenericParametersOptions
      {
         get { return _genericParametesOptions; }
         set { _genericParametesOptions = value; }
      }

      /// <summary>
      /// Constructor
      /// </summary>
      public ExposableMethodAttribute()
      {
         _category = "Various";
      }
   }
}
