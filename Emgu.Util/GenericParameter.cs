using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.Util
{
   public class GenericParameter
   {
      private Type _selectedType;

      public Type SelectedType
      {
         get { return _selectedType; }
         set { _selectedType = value; }
      }

      private Type[] _availableTypes;

      public Type[] AvailableTypes
      {
         get { return _availableTypes; }
         set { _availableTypes = value; }
      }

      public GenericParameter(Type selectedType, Type[] availableType)
      {
         _selectedType = selectedType;
         _availableTypes = availableType;
      }
   }
}
