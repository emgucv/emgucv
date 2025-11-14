//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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

      private static async Task<Mat> ReadFile(String fileName, ImreadModes modes = ImreadModes.AnyColor | ImreadModes.AnyDepth)
      {
         StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///" + fileName));
         Mat m = new Mat();
         await file.ToArray(m);
         return m;
      }

      public static Mat LoadMat(String name, ImreadModes modes = ImreadModes.AnyColor | ImreadModes.AnyDepth)
      {
         return Task.Run(async () => await ReadFile(name, modes)).Result;
      }
#else
        public static String GetFile(String fileName)
        {
            return fileName;
        }

        public static Mat LoadMat(string name, ImreadModes modes = ImreadModes.AnyColor | ImreadModes.AnyDepth)
        {
            return CvInvoke.Imread(name, modes);
        }
#endif

#if __IOS__ || __ANDROID__
        public static void IsTrue(bool condition)
        {
            Assert.That(condition);
        }

        public static void IsTrue(bool condition, String message)
        {
            Assert.That(condition, message);
        }

        public static void AreEqual(object a, object b)
        {
            Assert.That(a.Equals(b));
        }

        public static void AreEqual(object a, object b, string message)
        {
            Assert.That(a.Equals(b), message);
        }

        public static void AreNotEqual(object a, object b, string message)
        {
            Assert.That(a.Equals(b), Is.False, message);
        }

        public static void IsFalse(bool condition)
        {
            Assert.That(condition, Is.False);
        }

        public static void WriteLine(String message)
        {
            Console.WriteLine(message);
        }
#else
        public static void IsTrue(bool condition)
        {
#if VS_TEST || NETFX_CORE
            Assert.IsTrue(condition);
#else
            Assert.That(condition);
#endif
        }

        public static void IsTrue(bool condition, String message)
        {
#if VS_TEST || NETFX_CORE
            Assert.IsTrue(condition, message);
#else
            Assert.That(condition, message);
#endif
        }

        public static void AreEqual(object a, object b)
        {
#if VS_TEST || NETFX_CORE
            Assert.AreEqual(a, b);
#else
            Assert.That(a.Equals(b));
#endif
        }

        public static void AreEqual(object a, object b, string message)
        {
#if VS_TEST || NETFX_CORE
            Assert.AreEqual(a, b, message);
#else
            Assert.That(a.Equals(b), message);
#endif
        }

        public static void AreNotEqual(object a, object b, string message)
        {
#if VS_TEST || NETFX_CORE
            Assert.AreNotEqual(a, b, message);
#else
            Assert.That(a.Equals(b), Is.False, message);
#endif
        }

        public static void IsFalse(bool condition)
        {
#if VS_TEST || NETFX_CORE
            Assert.IsFalse(condition);
#else
            Assert.That(condition, Is.False);
#endif
        }

        public static void WriteLine(String message)
        {
            Trace.WriteLine(message);
        }

#endif
        }
}