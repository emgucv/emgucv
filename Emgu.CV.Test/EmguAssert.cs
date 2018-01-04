//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

#if VS_TEST
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestAttribute = Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute;
#elif NETFX_CORE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Trace = System.Diagnostics.Debug;
using System.Threading.Tasks;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
#else
using NUnit.Framework;
#endif

namespace Emgu.CV.Test
{
   public static class EmguAssert
   {
      #if NETFX_CORE
      public async static Task<string[]> ReadAllLinesAsync(String fileName)
      {
         StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + fileName));
         var lines = await FileIO.ReadLinesAsync(sf);
         return lines.ToArray();
      }
      #endif

      public static string[] ReadAllLines(String fileName)
      {
         string f = GetFile(fileName);
#if NETFX_CORE
         var t = ReadAllLinesAsync(f);
         return t.Result;
#else
         return System.IO.File.ReadAllLines(f);
#endif
      }

#if __ANDROID__
      public static Image<TColor, TDepth> LoadImage<TColor, TDepth>(String name)
         where TColor : struct, IColor
         where TDepth : new()
      {
         return AssetsUtil.LoadImage<TColor, TDepth>(name);
      }

      public static Mat LoadMat(String name)
      {
         return AssetsUtil.LoadMat(name);
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

      private static async Task<Mat> ReadFile(String fileName)
      {
         StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + fileName));
         Mat m = await Mat.FromStorageFile(file);
         return m;
      }

      public static Image<TColor, TDepth> LoadImage<TColor, TDepth>(String name)
         where TColor : struct, IColor
         where TDepth : new()
      {
         using (Mat m = Task.Run(async () => await ReadFile(name)).Result)
         {
            return m.ToImage<TColor, TDepth>();
         }
      }

      public static Mat LoadMat(String name)
      {
         return Task.Run(async () => await ReadFile(name)).Result;
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

      public static Mat LoadMat(string name)
      {
         return CvInvoke.Imread(name, ImreadModes.AnyColor | ImreadModes.AnyDepth);
      }
#endif

#if __IOS__ || __ANDROID__
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