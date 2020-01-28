//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.Reflection
{
   /// <summary>
   /// A generic parameter for the Operation class
   /// </summary>
   public class GenericParameter
   {
      private Type _selectedType;

      /// <summary>
      /// The selected generic parameter type
      /// </summary>
      public Type SelectedType
      {
         get { return _selectedType; }
         set { _selectedType = value; }
      }

      private Type[] _availableTypes;

      /// <summary>
      /// The types that can be used
      /// </summary>
      public Type[] AvailableTypes
      {
         get { return _availableTypes; }
         set { _availableTypes = value; }
      }
      
      /// <summary>
      /// Create a generic parameter for the Operation class
      /// </summary>
      /// <param name="selectedType">The selected generic parameter typ</param>
      /// <param name="availableType">The types that can be used</param>
      public GenericParameter(Type selectedType, Type[] availableType)
      {
         _selectedType = selectedType;
         _availableTypes = availableType;
      }
   }
}
