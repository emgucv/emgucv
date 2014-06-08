//----------------------------------------------------------------------------
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

#if NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Trace = System.Diagnostics.Debug;
using System.Threading.Tasks;
using Windows.Storage;
#else
using NUnit.Framework;
#endif

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

      public static String GetFile(String fileName)
      {
         return AssetsUtil.LoadFile(fileName);
      }  
#elif NETFX_CORE
      public static String GetFile(String fileName)
      {
         return fileName;
      }

      private static async Task<byte[]> ReadFile(String fileName)
      {
         StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + fileName));
         //StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
         IBuffer buffer = await FileIO.ReadBufferAsync(file);
         return buffer.ToArray();
      }

      public static Image<TColor, TDepth> LoadImage<TColor, TDepth>(String name)
         where TColor : struct, IColor
         where TDepth : new()
      {
         byte[] data = Task.Run(async () => await ReadFile(name)).Result;
         Mat m = new Mat();
         CvInvoke.Imdecode(data, LoadImageType.Unchanged, m);
         return m.ToImage<TColor, TDepth>();
      }
#else
      public static String GetFile(String fileName)
      {
         return fileName;
      }

      public static Image<TColor, TDepth> LoadImage<TColor, TDepth>(String name)
         where TColor : struct, IColor
         where TDepth : new()
      {
         return new Image<TColor, TDepth>(name);
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