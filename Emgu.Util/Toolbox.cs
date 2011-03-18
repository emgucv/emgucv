//----------------------------------------------------------------------------
//  Copyright (C) 2004-2011 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace Emgu.Util
{
   /// <summary>
   /// utilities functions for Emgu
   /// </summary>
   public static class Toolbox
   {
      #region xml serilization and deserialization
      /// <summary>
      /// Convert an object to an xml document
      /// </summary>
      /// <typeparam name="T">The type of the object to be converted</typeparam>
      /// <param name="o">The object to be serialized</param>
      /// <returns>An xml document that represents the object</returns>
      public static XmlDocument XmlSerialize<T>(T o)
      {
         StringBuilder sb = new StringBuilder();
         (new XmlSerializer(typeof(T))).Serialize(new StringWriter(sb), o);
         XmlDocument doc = new XmlDocument();
         doc.LoadXml(sb.ToString());
         return doc;
      }

      /// <summary>
      /// Convert an object to an xml document
      /// </summary>
      /// <typeparam name="T">The type of the object to be converted</typeparam>
      /// <param name="o">The object to be serialized</param>
      /// <param name="knownTypes">Other types that it must known ahead to serialize the object</param>
      /// <returns>An xml document that represents the object</returns>
      public static XmlDocument XmlSerialize<T>(T o, Type[] knownTypes)
      {
         StringBuilder sb = new StringBuilder();
         (new XmlSerializer(typeof(T), knownTypes)).Serialize(new StringWriter(sb), o);
         XmlDocument doc = new XmlDocument();
         doc.LoadXml(sb.ToString());
         return doc;
      }

      /// <summary>
      /// Convert an xml document to an object
      /// </summary>
      /// <typeparam name="T">The type of the object to be converted to</typeparam>
      /// <param name="xDoc">The xml document</param>
      /// <returns>The object representation as a result of the deserialization of the xml document</returns>
      public static T XmlDeserialize<T>(XmlDocument xDoc)
      {
         return (T)(new XmlSerializer(typeof(T))).Deserialize(new XmlNodeReader(xDoc));
      }

      /// <summary>
      /// Convert an xml document to an object
      /// </summary>
      /// <typeparam name="T">The type of the object to be converted to</typeparam>
      /// <param name="xDoc">The xml document</param>
      /// <param name="knownTypes">Other types that it must known ahead to deserialize the object</param>
      /// <returns>The object representation as a result of the deserialization of the xml document</returns>
      public static T XmlDeserialize<T>(XmlDocument xDoc, Type[] knownTypes)
      {
         return (T)(new XmlSerializer(typeof(T), knownTypes)).Deserialize(new XmlNodeReader(xDoc));
      }

      /// <summary>
      /// Convert an xml string to an object
      /// </summary>
      /// <typeparam name="T">The type of the object to be converted to</typeparam>
      /// <param name="xmlString">The xml document as a string</param>
      /// <returns>The object representation as a result of the deserialization of the xml string</returns>
      public static T XmlStringDeserialize<T>(String xmlString)
      {
         return (T)(new XmlSerializer(typeof(T))).Deserialize(new StringReader(xmlString));
      }
      #endregion

      /*
      /// <summary>
      /// Read a text file to an array of string, each row are seperated using by the input seperator
      /// </summary>
      /// <param name="fileName">The text file to read from</param>
      /// <param name="seperator">The row seperator</param>
      /// <returns></returns>
      public static string FileToString(string fileName, char seperator)
      {
         StringBuilder res = new StringBuilder();
         string input;
         using (StreamReader sr = File.OpenText(fileName))
            while ((input = sr.ReadLine()) != null)
               res.AppendFormat("{0}{1}", input, seperator);

         return res.ToString();
      }*/

      /// <summary>
      /// Merges two byte vector into one
      /// </summary>
      /// <param name="a">the first byte vector to be merged</param>
      /// <param name="b">the second byte vector to be merged</param>
      /// <returns>The bytes that is a concatenation of a and b</returns>
      public static Byte[] MergeBytes(Byte[] a, Byte[] b)
      {
         Byte[] c = new byte[a.Length + b.Length]; // just one array allocation
         Buffer.BlockCopy(a, 0, c, 0, a.Length);
         Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
         return c;
      }

      /// <summary>
      /// Call a command from command line
      /// </summary>
      /// <param name="execFileName">The name of the executable</param>
      /// <param name="arguments">The arguments to the executeable</param>
      /// <returns>The standard output</returns>
      public static string ExecuteCmd(string execFileName, string arguments)
      {
         using (Process processor = new Process())
         {
            processor.StartInfo.FileName = execFileName;
            processor.StartInfo.Arguments = arguments;
            processor.StartInfo.UseShellExecute = false;
            processor.StartInfo.RedirectStandardOutput = true;
            processor.StartInfo.RedirectStandardError = true;

            //string error = string.Empty;
            try
            {
               processor.Start();
            }
            catch (Exception)
            {
               //error = e.Message;
            }

            //processor.BeginErrorReadLine();
            //String error2 = processor.StandardError.ReadToEnd();
            string output = processor.StandardOutput.ReadToEnd();

            processor.WaitForExit();
            processor.Close();
            return output;
         }
      }

      /// <summary>
      /// Use reflection to find the base type. If such type do not exist, null is returned
      /// </summary>
      /// <param name="currentType">The type to search from</param>
      /// <param name="baseclassName">The name of the base class to search</param>
      /// <returns>The base type</returns>
      public static Type GetBaseType(Type currentType, String baseclassName)
      {
         if (currentType.Name.Equals(baseclassName))
            return currentType;
         Type baseType = currentType.BaseType;

         return (baseType == null) ?
            null : GetBaseType(baseType, baseclassName);
      }

      #region memory copy
      /// <summary>
      /// Convert some generic vector to vector of Bytes
      /// </summary>
      /// <typeparam name="D">type of the input vector</typeparam>
      /// <param name="data">array of data</param>
      /// <returns>the byte vector</returns>
      public static Byte[] ToBytes<D>(D[] data)
      {
         int size = Marshal.SizeOf(typeof(D)) * data.Length;
         Byte[] res = new Byte[size];
         GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
         Marshal.Copy(handle.AddrOfPinnedObject(), res, 0, size);
         handle.Free();
         return res;
      }

      /// <summary>
      /// Copy a generic vector to the unmanaged memory
      /// </summary>
      /// <typeparam name="D">The data type of the vector</typeparam>
      /// <param name="src">The source vector</param>
      /// <param name="dest">Pointer to the destination unmanaged memory</param>
      public static void CopyVector<D>(D[] src, IntPtr dest)
      {
         int size = Marshal.SizeOf(typeof(D)) * src.Length;
         GCHandle handle = GCHandle.Alloc(src, GCHandleType.Pinned);
         memcpy(dest, handle.AddrOfPinnedObject(), size);
         handle.Free();
      }

      /// <summary>
      /// Copy a jagged two dimensional array to the unmanaged memory
      /// </summary>
      /// <typeparam name="D">The data type of the jagged two dimensional</typeparam>
      /// <param name="src">The src array</param>
      /// <param name="dest">Pointer to the destination unmanaged memory</param>
      public static void CopyMatrix<D>(D[][] src, IntPtr dest)
      {
         int datasize = Marshal.SizeOf(typeof(D));
         int step = datasize * src[0].Length;
         int current = dest.ToInt32();

         for (int i = 0; i < src.Length; i++, current += step)
         {
            GCHandle handle = GCHandle.Alloc(src[i], GCHandleType.Pinned);
            memcpy(new IntPtr(current), handle.AddrOfPinnedObject(), step);
            handle.Free();
         }
      }

      /// <summary>
      /// Copy a jagged two dimensional array from the unmanaged memory
      /// </summary>
      /// <typeparam name="D">The data type of the jagged two dimensional</typeparam>
      /// <param name="src">The src array</param>
      /// <param name="dest">Pointer to the destination unmanaged memory</param>
      public static void CopyMatrix<D>(IntPtr src, D[][] dest)
      {
         int datasize = Marshal.SizeOf(typeof(D));
         int step = datasize * dest[0].Length;
         int current = src.ToInt32();

         for (int i = 0; i < dest.Length; i++, current += step)
         {
            GCHandle handle = GCHandle.Alloc(dest[i], GCHandleType.Pinned);
            memcpy(handle.AddrOfPinnedObject(), new IntPtr(current), step);
            handle.Free();
         }
      }

      /// <summary>
      /// Perform first degree interpolation give the sorted data <paramref name="src"/> and the interpolation <paramref name="indexes"/>
      /// </summary>
      /// <param name="src">The sorted data that will be interpolated from</param>
      /// <param name="indexes">The indexes of the interpolate result</param>
      /// <returns></returns>
      public static IEnumerable<T> LinearInterpolate<T> (IEnumerable<T> src, IEnumerable<double> indexes) where T:IInterpolatable<T>, new()
      {
         using (IEnumerator<T> sampleEnumerator = src.GetEnumerator())
         {
            if (!sampleEnumerator.MoveNext()) yield break;
            T old = sampleEnumerator.Current;
            if (!sampleEnumerator.MoveNext()) yield break;

            T current = sampleEnumerator.Current;

            foreach (double index in indexes)
            {
               while (index > current.InterpolationIndex && sampleEnumerator.MoveNext())
               {
                  old = current;
                  current = sampleEnumerator.Current;
               }
               //yield return LinearInterpolate(old, current, index);
               yield return old.LinearInterpolate(current, index);
            }
         }
      }

      /// <summary>
      /// Get subsamples with the specific rate
      /// </summary>
      /// <param name="src">The source which the subsamples will be derived from</param>
      /// <param name="subsampleRate">The subsample rate</param>
      /// <returns><paramref name="src"/> subsampled with the specific rate </returns>
      public static IEnumerable<T> LinearSubsample<T>(IEnumerable<T> src, double subsampleRate) where T : IInterpolatable<T>, new()
      {
         using (IEnumerator<T> sampleEnumerator = src.GetEnumerator())
         {
            if (!sampleEnumerator.MoveNext()) yield break;
            T old = sampleEnumerator.Current;
            yield return old;

            if (!sampleEnumerator.MoveNext()) yield break;
            T current = sampleEnumerator.Current;
            double currentIndex = old.InterpolationIndex + subsampleRate;

            bool endOfSubsample = false;
            while (!endOfSubsample)
            {
               while (currentIndex > current.InterpolationIndex )
               {
                  if (endOfSubsample = !sampleEnumerator.MoveNext()) break;

                  old = current;
                  current = sampleEnumerator.Current;
               }

               if (!endOfSubsample)
               {
                  yield return old.LinearInterpolate(current, currentIndex);
                  //yield return LinearInterpolate(old, current, currentIndex);
                  currentIndex += subsampleRate;
               }
            }
         }
      }

      /// <summary>
      /// Joining multiple index ascending IInterpolatables together as a single index ascending IInterpolatable. 
      /// </summary>
      /// <typeparam name="T">The type of objects that will be joined</typeparam>
      /// <param name="enums">The enumerables, each should be stored in index ascending order</param>
      /// <returns>A single enumerable sorted in index ascending order</returns>
      public static IEnumerable<T> JoinInterpolatables<T>(params IEnumerable<T>[] enums) where T : IInterpolatable<T>, new()
      {
         if (enums.Length == 0)
            yield break;
         else if (enums.Length == 1)
         {
            foreach (T sample in enums[0])
               yield return sample;
         }
         else if (enums.Length == 2)
         {
            foreach (T sample in JoinTwoInterpolatables(enums[0], enums[1]))
               yield return sample;
         }
         else
         {
            int middle = enums.Length / 2;
            IEnumerable<T>[] lower = new IEnumerable<T>[middle];
            IEnumerable<T>[] upper = new IEnumerable<T>[enums.Length - middle];
            Array.Copy(enums, lower, middle);
            Array.Copy(enums, middle, upper, 0, enums.Length - middle);

            foreach (T sample in JoinTwoInterpolatables<T>(JoinInterpolatables<T>(lower), JoinInterpolatables<T>(upper)))
               yield return sample;
         }
      }

      private static IEnumerable<T> JoinTwoInterpolatables<T>(IEnumerable<T> enum1, IEnumerable<T> enum2) where T : IInterpolatable<T>, new()
      {
         IEnumerator<T> l1 = enum1.GetEnumerator();
         IEnumerator<T> l2 = enum2.GetEnumerator();
         if (!l1.MoveNext())
         {
            while (l2.MoveNext())
               yield return l2.Current;
            yield break;
         }
         else if (!l2.MoveNext())
         {
            while (l1.MoveNext())
               yield return l1.Current;
            yield break;
         }

         T s1 = l1.Current;
         T s2 = l2.Current;

         while (true)
         {
            if (s1.InterpolationIndex < s2.InterpolationIndex)
            {
               yield return s1;
               if (l1.MoveNext())
                  s1 = l1.Current;
               else
               {
                  while (l2.MoveNext())
                     yield return l2.Current;
                  yield break;
               }
            }
            else
            {
               yield return s2;
               if (l2.MoveNext())
                  s2 = l2.Current;
               else
               {
                  while (l1.MoveNext())
                     yield return l1.Current;
                  yield break;
               }
            }
         }
      }

      /*
      /// <summary>
      /// Use the two IInterpolatable and the index to perform first degree interpolation
      /// </summary>
      /// <param name="i1">The first element to interpolate from</param>
      /// <param name="i2">The second eleemnt to interpolate from</param>
      /// <param name="index">The interpolation index</param>
      /// <returns>The interpolatation result</returns>
      private static T LinearInterpolate<T>(T i1, T i2, double index) where T : IInterpolatable<T>, new()
      {
         
         double f = (i2.InterpolationIndex - index) / (i2.InterpolationIndex - i1.InterpolationIndex);

         //compute result = i1 * f + i2 * (1.0 - f)
         T a = new T();
         a.Add(i1);
         a.Mul(f);
         T b = new T();
         b.Add(i2);
         b.Mul(1.0 - f);
         a.Add(b);
         return a;
      }*/


      /// <summary>
      /// memcpy function
      /// </summary>
      /// <param name="dest">the destination of memory copy</param>
      /// <param name="src">the source of memory copy</param>
      /// <param name="len">the number of bytes to be copied</param>
      [DllImport("kernel32.dll", EntryPoint = "CopyMemory")]
      public static extern void memcpy(IntPtr dest, IntPtr src, int len);
      #endregion

      /// <summary>
      /// Maps the specified executable module into the address space of the calling process.
      /// </summary>
      /// <param name="dllname">The name of the dll</param>
      /// <returns>The handle to the library</returns>
      [DllImport("kernel32.dll")]
      public static extern IntPtr LoadLibrary(String dllname);

      /// <summary>
      /// Decrements the reference count of the loaded dynamic-link library (DLL). When the reference count reaches zero, the module is unmapped from the address space of the calling process and the handle is no longer valid
      /// </summary>
      /// <param name="handle">The handle to the library</param>
      /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero.</returns>
      [DllImport("kernel32.dll")]
      public static extern bool FreeLibrary(IntPtr handle);

      /// <summary>
      /// Adds a directory to the search path used to locate DLLs for the application
      /// </summary>
      /// <param name="path">The directory to be searched for DLLs</param>
      /// <returns>True if success</returns>
      [DllImport("kernel32.dll")]
      public static extern bool SetDllDirectory(String path);

   }
}
