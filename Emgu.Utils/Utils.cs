using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using System.Runtime.InteropServices;

namespace Emgu
{
    /// <summary>
    /// utilities functions for Emgu
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// A mothod that contains no input and returns nothing
        /// </summary>
        public delegate void Action();

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

        public delegate TOutput Func<TInput1, TInput2, TOutput>(TInput1 o1, TInput2 o2);

        public delegate TOutput Func<TInput1, TInput2, TInput3, TOutput>(TInput1 o1, TInput2 o2, TInput3 o3);

        public delegate TOutput Func<TInput1, TInput2, TInput3, TInput4, TOutput>(TInput1 o1, TInput2 o2, TInput3 o3, TInput4 o4);

        /// <summary>
        /// Convert an object to an xml document
        /// </summary>
        /// <typeparam name="T">The type of the object to be converted</typeparam>
        /// <param name="o">The object to be serialized</param>
        /// <returns>An xml document that represents the object</returns>
        public static XmlDocument XmlSerialize<T>(T o)
        {
            StringBuilder sb = new StringBuilder();
            (new XmlSerializer(typeof(T))).Serialize( new StringWriter(sb), o);
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
        /// Convert an xml string to an object
        /// </summary>
        /// <typeparam name="T">The type of the object to be converted to</typeparam>
        /// <param name="xmlString">The xml document as a string</param>
        /// <returns>The object representation as a result of the deserialization of the xml string</returns>
        public static T XmlStringDeserialize<T>(String xmlString)
        {
            return (T)(new XmlSerializer(typeof(T))).Deserialize(new StringReader(xmlString));
        }

        public static string VectorToString<T>(T[] a, string seperator)
        {
            return String.Join(
                seperator, 
                Array.ConvertAll<T, string>(a, delegate(T item) { return item.ToString(); }));
        }

        public static string MatrixToString<T>(T[][] mat, string columnToken, string rowToken)
        {
            return String.Join(
                rowToken, 
                Array.ConvertAll<T[], string>(
                    mat,
                    delegate(T[] a) { return VectorToString<T>(a, columnToken); }));
        }

        public static string FileTypeFromName(string fileName)
        {
            int startPos = fileName.LastIndexOf('.') + 1;
            return fileName.Substring(startPos, fileName.Length - startPos);
        }

        public static T[] StringToVector<T>(string input, string token) 
        {
            return Array.ConvertAll<String, T>(
                input.Split(token.ToCharArray()),
                delegate(String s) { return (T)Convert.ChangeType(s, typeof(T)); });
        }

        public static T[][] StringToMatrix<T>(string input, string columnToken, string rowToken)
        {
            return Array.ConvertAll<String, T[]>(
                input.Split(rowToken.ToCharArray()),
                delegate(string s) { return StringToVector<T>(s, columnToken); });
        }      

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

        public static TOutput[] Operate<TInput1, TInput2, TOutput>(TInput1[] param1, TInput2[] param2, Func<TInput1, TInput2, TOutput> func)
        {
            int size = param1.Length;
            if (size != param2.Length) 
                throw new Emgu.Exception(Emgu.ExceptionHeader.CriticalException, "Array size do not match");
            
            TOutput[] res = new TOutput[size];
            for (int i = 0; i < size; res[i] = func(param1[i], param2[i]), i++);
            
            return res;
        }

        public static T[] Evolve<T>(T start, Converter<T, bool> terminateCondition, Converter<T, T> evolveAction)
        {
            List<T> res = new List<T>();
            T current = start;
            while (terminateCondition(current))
            {
                res.Add(current);
                current = evolveAction(current);
            }
            
            return res.ToArray();
        }

        public class StringEventArgs : EventArgs
        {
            private string _message;
            public string Message { get { return _message; } }
            public StringEventArgs(string msg)
                : base()
            {
                _message = msg;
            }
        }

        /// <summary>
        /// Convert some generic vector to vector of Bytes
        /// </summary>
        /// <typeparam name="D">type of the input vector</typeparam>
        /// <param name="data">array of data</param>
        /// <returns>the byte vector</returns>
        public static Byte[] ToBytes<D>(D[] data)
        {
            int size = System.Runtime.InteropServices.Marshal.SizeOf(data[0]) * data.Length;
            Byte[] res = new Byte[size];
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.Copy(handle.AddrOfPinnedObject(), res, 0, size);
            handle.Free();
            return res;
        }

        public static void CopyVector<D>(D[] src, IntPtr dest)
        {
            int size = Marshal.SizeOf( typeof(D) ) * src.Length;
            GCHandle handle = GCHandle.Alloc(src, GCHandleType.Pinned);
            Emgu.Utils.memcpy(dest, handle.AddrOfPinnedObject(), size);
            handle.Free();
        }

        public static void CopyMatrix<D>(D[][] src, IntPtr dest)
        {
            int datasize = Marshal.SizeOf( typeof(D) );
            int columnsize = datasize * src[0].Length;
            int current = dest.ToInt32();
            
            for (int i = 0; i < src.Length; i++, current += columnsize)
            {
                GCHandle handle = GCHandle.Alloc(src[i], GCHandleType.Pinned);
                Emgu.Utils.memcpy(new IntPtr(current), handle.AddrOfPinnedObject(), columnsize);
                handle.Free();
            }
        }

        public static void CopyMatrix<D>(IntPtr src, D[][] dest)
        {
            int datasize = System.Runtime.InteropServices.Marshal.SizeOf( typeof(D) );
            int columnsize = datasize * dest[0].Length;
            int current = src.ToInt32();

            for (int i = 0; i < dest.Length; i++, current += columnsize)
            {
                GCHandle handle = GCHandle.Alloc(dest[i], GCHandleType.Pinned);
                Emgu.Utils.memcpy(handle.AddrOfPinnedObject(), new IntPtr(current), columnsize);
                handle.Free();
            }
        }

        /// <summary>
        /// memcpy function
        /// </summary>
        /// <param name="dest">the destination of memory copy</param>
        /// <param name="src">the source of memory copy</param>
        /// <param name="len">the number of bytes to be copied</param>
        [DllImport("kernel32.dll", EntryPoint="CopyMemory")]
        public static extern void memcpy(IntPtr dest, IntPtr src, int len);

    }
}

