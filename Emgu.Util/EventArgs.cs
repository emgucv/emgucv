using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.Util
{
   /// <summary>
   /// A generic EventArgs
   /// </summary>
   /// <typeparam name="T">The type of arguments</typeparam>
   public class EventArgs<T> : EventArgs
   {
      private T _value;

      public EventArgs(T value)
         : base()
      {
         _value = value;
      }

      public T Value
      {
         get { return _value; }
      }
   }

   /// <summary>
   /// A generic EventArgs
   /// </summary>
   /// <typeparam name="T">The type of arguments</typeparam>
   public class EventArgs<T1, T2> : EventArgs
   {
      private T1 _value1;
      private T2 _value2;

      public EventArgs(T1 value1, T2 value2)
         : base()
      {
         _value1 = value1;
         _value2 = value2;
      }

      public T1 Value1
      {
         get { return _value1; }
      }

      public T2 Value2
      {
         get { return _value2; }
      }
   }
}
