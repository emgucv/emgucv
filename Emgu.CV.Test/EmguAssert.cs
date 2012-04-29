using System;
using NUnit.Framework;
using System.IO;
using System.Diagnostics;

namespace Emgu.CV.Test
{
   public static class EmguAssert
   {
#if IOS
      public static void IsTrue(bool condition)
      {
         Assert.True(condition);
      }

      public static void IsTrue(bool condition, String message)
      {
         Assert.True(condition, message);
      }

      public static void AreEqual(object a, object b)
      {
         Assert.True(a.Equals(b));
      }

      public static void AreEqual(object a, object b, string message)
      {
         Assert.True(a.Equals(b), message);
      }

      public static void AreNotEqual(object a, object b, string message)
      {
         Assert.False(a.Equals(b), message);
      }

      public static void IsFalse(bool condition)
      {
         Assert.False(condition);
      }

      public static void WriteLine(String message)
      {
         Console.WriteLine(message);
      }
#else
#endif
   }
}