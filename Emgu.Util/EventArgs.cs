//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

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

      /// <summary>
      /// Create a generic EventArgs with the specific value
      /// </summary>
      /// <param name="value">The value</param>
      public EventArgs(T value)
         : base()
      {
         _value = value;
      }

      /// <summary>
      /// The value of the EventArgs
      /// </summary>
      public T Value
      {
         get { return _value; }
      }
   }

   /// <summary>
   /// A generic EventArgs
   /// </summary>
   /// <typeparam name="T1">The type of the first value</typeparam>
   /// <typeparam name="T2">The type of the second value</typeparam>
   public class EventArgs<T1, T2> : EventArgs
   {
      private T1 _value1;
      private T2 _value2;

      /// <summary>
      /// Create a generic EventArgs with two values
      /// </summary>
      /// <param name="value1">The first value</param>
      /// <param name="value2">The second value</param>
      public EventArgs(T1 value1, T2 value2)
         : base()
      {
         _value1 = value1;
         _value2 = value2;
      }

      /// <summary>
      /// The first value
      /// </summary>
      public T1 Value1
      {
         get { return _value1; }
      }


      /// <summary>
      /// The second value
      /// </summary>
      public T2 Value2
      {
         get { return _value2; }
      }
   }
}
