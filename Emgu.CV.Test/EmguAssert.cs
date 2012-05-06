//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV.Test
{
   public static class EmguAssert
   {
#if ANDROID
      public static Image<TColor, TDepth> LoadImage<TColor, TDepth>(String name)
         where TColor : struct, IColor
         where TDepth : new()
      {
         return AssetsUtil.LoadImage<TColor, TDepth>(name);
      }
#else
      public static Image<Bgr, Byte> LoadImage(String name)
      {
         return new Image<Bgr, Byte>(name);
      }
#endif

#if IOS || ANDROID
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
      public static void IsTrue(bool condition)
      {
         Assert.IsTrue(condition);
      }

      public static void IsTrue(bool condition, String message)
      {
         Assert.IsTrue(condition, message);
      }

      public static void AreEqual(object a, object b)
      {
         Assert.IsTrue(a.Equals(b));
      }

      public static void AreEqual(object a, object b, string message)
      {
         Assert.IsTrue(a.Equals(b), message);
      }

      public static void AreNotEqual(object a, object b, string message)
      {
         Assert.IsFalse(a.Equals(b), message);
      }

      public static void IsFalse(bool condition)
      {
         Assert.IsFalse(condition);
      }

      public static void WriteLine(String message)
      {
         Trace.WriteLine(message);
      }
#endif
   }
}