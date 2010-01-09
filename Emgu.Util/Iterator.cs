using System;
using System.Collections.Generic;
using System.Text;

namespace Emgu.Util
{
   /// <summary>
   /// An iterator is used to convert an enumerator to enumerable
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class Iterator<T> : IEnumerable<T>
   {
      IEnumerator<T> _iterator;

      /// <summary>
      /// Create an iterator from an enumerator
      /// </summary>
      /// <param name="iter"></param>
      public Iterator(IEnumerator<T> iter)
      {
         _iterator = iter;
      }

      /// <summary>
      /// Returns the enumerator
      /// </summary>
      /// <returns>The enumerator</returns>
      public IEnumerator<T> GetEnumerator()
      {
         return _iterator;
      }

      /// <summary>
      /// Returns the enumerator
      /// </summary>
      /// <returns>The enumerator</returns>
      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return _iterator;
      }
   } 
}
