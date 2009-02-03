using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;

namespace Emgu.Util
{
   /// <summary>
   /// utilities functions for Emgu
   /// </summary>
   public static class Toolbox
   {
      /// <summary>
      /// Convert on enumeration to another using the specific convertor
      /// </summary>
      /// <typeparam name="Tin">The input enumerator type</typeparam>
      /// <typeparam name="Tout">The output enumerator type</typeparam>
      /// <param name="inputs">the input enumerator</param>
      /// <param name="convertor">the convertor that convert one enumeration to another</param>
      /// <returns>An enumerator of <paramref name="Tout"/></returns>
      public static IEnumerable<Tout> IEnumConvertor<Tin, Tout>(IEnumerable<Tin> inputs, System.Converter<Tin, Tout> convertor)
      {
         foreach (Tin obj in inputs)
            yield return convertor(obj);
      }

      #region delegates
      /// <summary>
      /// An Action that accepts two input and returns nothing
      /// </summary>
      /// <typeparam name="TInput1"></typeparam>
      /// <typeparam name="TInput2"></typeparam>
      /// <param name="o1">The first input parameter</param>
      /// <param name="o2">The second input parameter</param>
      public delegate void Action<TInput1, TInput2>(TInput1 o1, TInput2 o2);

      /// <summary>
      /// An Action that accepts three input and returns nothing
      /// </summary>
      /// <typeparam name="TInput1"></typeparam>
      /// <typeparam name="TInput2"></typeparam>
      /// <typeparam name="TInput3"></typeparam>
      /// <param name="o1">The first input parameter</param>
      /// <param name="o2">The second input parameter</param>
      /// <param name="o3">The third input parameter</param>
      public delegate void Action<TInput1, TInput2, TInput3>(TInput1 o1, TInput2 o2, TInput3 o3);

      /// <summary>
      /// An Action that accepts fourth input and returns nothing
      /// </summary>
      /// <typeparam name="TInput1"></typeparam>
      /// <typeparam name="TInput2"></typeparam>
      /// <typeparam name="TInput3"></typeparam>
      /// <typeparam name="TInput4"></typeparam>
      /// <param name="o1">The first input parameter</param>
      /// <param name="o2">The second input parameter</param>
      /// <param name="o3">The third input parameter</param>
      /// <param name="o4">The fourth input parameter</param>
      public delegate void Action<TInput1, TInput2, TInput3, TInput4>(TInput1 o1, TInput2 o2, TInput3 o3, TInput4 o4);

      /// <summary>
      /// Delegate similar to that in .Net 3.5
      /// </summary>
      /// <typeparam name="TInput1"></typeparam>
      /// <typeparam name="TInput2"></typeparam>
      /// <typeparam name="TOutput"></typeparam>
      /// <param name="o1"></param>
      /// <param name="o2"></param>
      /// <returns></returns>
      public delegate TOutput Func<TInput1, TInput2, TOutput>(TInput1 o1, TInput2 o2);

      /// <summary>
      /// Delegate similar to that in .Net 3.5
      /// </summary>
      /// <typeparam name="TInput1"></typeparam>
      /// <typeparam name="TInput2"></typeparam>
      /// <typeparam name="TInput3"></typeparam>
      /// <typeparam name="TOutput"></typeparam>
      /// <param name="o1"></param>
      /// <param name="o2"></param>
      /// <param name="o3"></param>
      /// <returns></returns>
      public delegate TOutput Func<TInput1, TInput2, TInput3, TOutput>(TInput1 o1, TInput2 o2, TInput3 o3);

      /// <summary>
      /// Delegate similar to that in .Net 3.5
      /// </summary>
      /// <typeparam name="TInput1"></typeparam>
      /// <typeparam name="TInput2"></typeparam>
      /// <typeparam name="TInput3"></typeparam>
      /// <typeparam name="TInput4"></typeparam>
      /// <typeparam name="TOutput"></typeparam>
      /// <param name="o1"></param>
      /// <param name="o2"></param>
      /// <param name="o3"></param>
      /// <param name="o4"></param>
      /// <returns></returns>
      public delegate TOutput Func<TInput1, TInput2, TInput3, TInput4, TOutput>(TInput1 o1, TInput2 o2, TInput3 o3, TInput4 o4);
      #endregion

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
      }

      /// <summary>
      /// Merges two byte vector into one
      /// </summary>
      /// <param name="a">the first byte vector to be merged</param>
      /// <param name="b">the second byte vector to be merged</param>
      /// <returns></returns>
      public static Byte[] MergeBytes(Byte[] a, Byte[] b)
      {
         Byte[] c = new byte[a.Length + b.Length]; // just one array allocation
         Buffer.BlockCopy(a, 0, c, 0, a.Length);
         Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
         return c;
      }

      /*
      /// <summary>
      /// Event argument that returns a string
      /// </summary>
      public class StringEventArgs : EventArgs
      {
         private string _message;
         /// <summary>
         /// The message for this EventArgs
         /// </summary>
         public string Message
         {
            get { return _message; }
         }

         /// <summary>
         /// Constructor
         /// </summary>
         /// <param name="msg">the message for this event</param>
         public StringEventArgs(string msg)
            : base()
         {
            _message = msg;
         }
      }*/

      /// <summary>
      /// Call a command from command line
      /// </summary>
      /// <param name="execFileName">The name of the executable</param>
      /// <param name="arguments">The arguments to the executeable</param>
      /// <returns>The standard output</returns>
      public static string ExecuteCmd(string execFileName, string arguments)
      {
         Process processor = new Process();

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
         int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(D)) * data.Length;
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
         int datasize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(D));
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
