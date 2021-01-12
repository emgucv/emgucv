//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;


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
        /// <param name="sourceObject">The object to be serialized</param>
        /// <returns>An xml document that represents the object</returns>
        public static XDocument XmlSerialize<T>(T sourceObject)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
                (new XmlSerializer(typeof(T))).Serialize(sw, sourceObject);

            return XDocument.Parse(sb.ToString());
        }

        /// <summary>
        /// Convert an object to an xml document
        /// </summary>
        /// <typeparam name="T">The type of the object to be converted</typeparam>
        /// <param name="sourceObject">The object to be serialized</param>
        /// <param name="knownTypes">Other types that it must known ahead to serialize the object</param>
        /// <returns>An xml document that represents the object</returns>
        public static XDocument XmlSerialize<T>(T sourceObject, Type[] knownTypes)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
                (new XmlSerializer(typeof(T), knownTypes)).Serialize(sw, sourceObject);
            return XDocument.Parse(sb.ToString());
        }

        /// <summary>
        /// Convert an xml document to an object
        /// </summary>
        /// <typeparam name="T">The type of the object to be converted to</typeparam>
        /// <param name="document">The xml document</param>
        /// <returns>The object representation as a result of the deserialization of the xml document</returns>
        public static T XmlDeserialize<T>(XDocument document)
        {
            using (XmlReader reader = document.Root.CreateReader())
            {
                if (reader.CanReadBinaryContent)
                    return (T)(new XmlSerializer(typeof(T))).Deserialize(reader);
                else
                {
                    return XmlStringDeserialize<T>(document.ToString());
                }
            }
        }

        /// <summary>
        /// Convert an xml document to an object
        /// </summary>
        /// <typeparam name="T">The type of the object to be converted to</typeparam>
        /// <param name="xDoc">The xml document</param>
        /// <param name="knownTypes">Other types that it must known ahead to deserialize the object</param>
        /// <returns>The object representation as a result of the deserialization of the xml document</returns>
        public static T XmlDeserialize<T>(XDocument xDoc, Type[] knownTypes)
        {
            using (XmlReader reader = xDoc.Root.CreateReader())
            {
                if (reader.CanReadBinaryContent)
                    return (T)(new XmlSerializer(typeof(T), knownTypes)).Deserialize(reader);
                else
                {
                    using (StringReader stringReader = new StringReader(xDoc.ToString()))
                    {
                        return (T)(new XmlSerializer(typeof(T), knownTypes)).Deserialize(stringReader);
                    }
                }
            }
        }

        /// <summary>
        /// Convert an xml string to an object
        /// </summary>
        /// <typeparam name="T">The type of the object to be converted to</typeparam>
        /// <param name="xmlString">The xml document as a string</param>
        /// <returns>The object representation as a result of the deserialization of the xml string</returns>
        public static T XmlStringDeserialize<T>(String xmlString)
        {
            using (StringReader stringReader = new StringReader(xmlString))
                return (T)(new XmlSerializer(typeof(T))).Deserialize(stringReader);
        }
        #endregion


        /// <summary>
        /// Similar to Marshal.SizeOf function
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>The size of T in bytes</returns>
        public static int SizeOf<T>()
        {
            return Marshal.SizeOf<T>();
        }

        /*
        /// <summary>
        /// Read a text file to an array of string, each row are separated using by the input separator
        /// </summary>
        /// <param name="fileName">The text file to read from</param>
        /// <param name="seperator">The row separator</param>
        /// <returns></returns>
        public static string FileToString(string fileName, char separator)
        {
           StringBuilder res = new StringBuilder();
           string input;
           using (StreamReader sr = File.OpenText(fileName))
              while ((input = sr.ReadLine()) != null)
                 res.AppendFormat("{0}{1}", input, separator);

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
        /// <param name="arguments">The arguments to the executable</param>
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
        /// <param name="baseClassName">The name of the base class to search</param>
        /// <returns>The base type</returns>
        public static Type GetBaseType(Type currentType, String baseClassName)
        {
            if (currentType.Name.Equals(baseClassName))
                return currentType;
            Type baseType = currentType.BaseType;

            return (baseType == null) ?
               null : GetBaseType(baseType, baseClassName);
        }

        #region memory copy
        /// <summary>
        /// Convert some generic vector to vector of Bytes
        /// </summary>
        /// <typeparam name="TData">type of the input vector</typeparam>
        /// <param name="data">array of data</param>
        /// <returns>the byte vector</returns>
        public static Byte[] ToBytes<TData>(TData[] data)
        {
            int size = Marshal.SizeOf<TData>() * data.Length;
            Byte[] res = new Byte[size];
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.Copy(handle.AddrOfPinnedObject(), res, 0, size);
            handle.Free();
            return res;
        }

        /// <summary>
        /// Perform first degree interpolation give the sorted data <paramref name="src"/> and the interpolation <paramref name="indexes"/>
        /// </summary>
        /// <param name="src">The sorted data that will be interpolated from</param>
        /// <param name="indexes">The indexes of the interpolate result</param>
        /// <returns></returns>
        public static IEnumerable<T> LinearInterpolate<T>(IEnumerable<T> src, IEnumerable<double> indexes) where T : IInterpolatable<T>, new()
        {
            using (IEnumerator<T> sampleEnumerator = src.GetEnumerator())
            {
                if (!sampleEnumerator.MoveNext())
                    yield break;
                T old = sampleEnumerator.Current;
                if (!sampleEnumerator.MoveNext())
                    yield break;

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
        /// <typeparam name="T">The type of the object</typeparam>
        public static IEnumerable<T> LinearSubsample<T>(IEnumerable<T> src, double subsampleRate) where T : IInterpolatable<T>, new()
        {
            using (IEnumerator<T> sampleEnumerator = src.GetEnumerator())
            {
                if (!sampleEnumerator.MoveNext())
                    yield break;
                T old = sampleEnumerator.Current;
                yield return old;

                if (!sampleEnumerator.MoveNext())
                    yield break;
                T current = sampleEnumerator.Current;
                double currentIndex = old.InterpolationIndex + subsampleRate;

                bool endOfSubsample = false;
                while (!endOfSubsample)
                {
                    while (currentIndex > current.InterpolationIndex)
                    {
                        if (endOfSubsample = !sampleEnumerator.MoveNext())
                            break;

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


        #endregion

        private static System.Collections.Generic.Dictionary<String, AssemblyName> _assemblyNameDict = new Dictionary<String, AssemblyName>();

        /// <summary>
        /// Load all the assemblies.
        /// </summary>
        /// <returns></returns>
        public static System.Reflection.Assembly[] LoadAllDependentAssemblies()
        {
            try
            {
                lock (_assemblyNameDict)
                {
                    System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    bool dirty = false;
                    foreach (Assembly assembly in assemblies)
                    {
                        AssemblyName name = assembly.GetName();
                        if (!_assemblyNameDict.ContainsKey(name.ToString()))
                            _assemblyNameDict.Add(name.ToString(), name);
                    }

                    foreach (Assembly assembly in assemblies)
                    {
                        AssemblyName[] referencedAssemblyNames = assembly.GetReferencedAssemblies();
                        foreach (AssemblyName name in referencedAssemblyNames)
                        {
                            if (!_assemblyNameDict.ContainsKey(name.ToString()))
                            {
                                try
                                {
                                    Assembly.Load(name);
                                    _assemblyNameDict.Add(name.ToString(), name);
                                    dirty = true;
                                }
                                catch (Exception e)
                                {
                                    //if failed to load, it is ok, we will continue.
                                    Debug.WriteLine(e);
                                }
                            }
                        }
                    }

                    if (dirty)
                        return AppDomain.CurrentDomain.GetAssemblies();
                    else
                    {
                        return assemblies;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// Get the interface implementation from assemblies
        /// </summary>
        /// <typeparam name="T">The interface</typeparam>
        /// <returns>The types that implement the specific interface</returns>
        public static Type[] GetIntefaceImplementationFromAssembly<T>()
        {
            System.Reflection.Assembly[] assemblies = LoadAllDependentAssemblies();
            List<Type> types = new List<Type>();
            if (assemblies != null)
            {
                foreach (Assembly assembly in assemblies)
                {
                    foreach (Type t in assembly.GetTypes())
                    {
                        if (typeof(T).IsAssignableFrom(t) && typeof(T) != t)
                        {
                            types.Add(t);
                        }
                    }
                }
            }

            return types.ToArray();
        }

        /// <summary>
        /// Find the loaded assembly with the specific assembly name
        /// </summary>
        /// <param name="assembleName">The name of the assembly</param>
        /// <returns>The assembly.</returns>
        public static System.Reflection.Assembly FindAssembly(String assembleName)
        {
            try
            {
                System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
                foreach (System.Reflection.Assembly asm in asms)
                {
                    if (asm.ManifestModule.Name.Equals(assembleName))
                        return asm;
                }
            }
            catch
            {

            }
            return null;
        }

        /// <summary>
        /// Maps the specified executable module into the address space of the calling process.
        /// </summary>
        /// <param name="dllname">The name of the dll</param>
        /// <returns>The handle to the library</returns>
        public static IntPtr LoadLibrary(String dllname)
        {
#if UNITY_EDITOR_WIN
            const int loadLibrarySearchDllLoadDir = 0x00000100;
            const int loadLibrarySearchDefaultDirs = 0x00001000;
            return LoadLibraryEx(dllname, IntPtr.Zero, loadLibrarySearchDllLoadDir | loadLibrarySearchDefaultDirs);
#else
            if (Platform.OperationSystem == Emgu.Util.Platform.OS.Windows)
            {
                const int loadLibrarySearchDllLoadDir = 0x00000100;
                const int loadLibrarySearchDefaultDirs = 0x00001000;
                //const int loadLibrarySearchApplicationDir = 0x00000200;
                //const int loadLibrarySearchUserDirs = 0x00000400;
                IntPtr handler = LoadLibraryEx(dllname, IntPtr.Zero, loadLibrarySearchDllLoadDir | loadLibrarySearchDefaultDirs);
                //IntPtr handler = LoadLibraryEx(dllname, IntPtr.Zero, loadLibrarySearchUserDirs);
                if (handler == IntPtr.Zero)
                {
                    int error = Marshal.GetLastWin32Error();

                    System.ComponentModel.Win32Exception ex = new System.ComponentModel.Win32Exception(error);
                    System.Diagnostics.Trace.WriteLine(String.Format("LoadLibraryEx {0} failed with error code {1}: {2}", dllname, (uint)error, ex.Message));
                    if (error == 5)
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("Please check if the current user has execute permission for file: {0} ", dllname));
                    }

                    //Also try loadPackagedLibrary
                    IntPtr packagedLibraryHandler = LoadPackagedLibrary(dllname, 0);
                    if (packagedLibraryHandler == IntPtr.Zero)
                    {
                        error = Marshal.GetLastWin32Error();
                        ex = new System.ComponentModel.Win32Exception(error);
                        System.Diagnostics.Debug.WriteLine(String.Format("LoadPackagedLibrary {0} failed with error code {1}: {2}", dllname, (uint)error, ex.Message));
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine(String.Format("LoadPackagedLibrary loaded: {0}", dllname));
                        return packagedLibraryHandler;
                    }
                }
                return handler;
            }
            else
            {
                return Dlopen(dllname, 2); // 2 == RTLD_NOW
            }
#endif
        }

        /*
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

        private static bool? _kernel32HasLoadPackagedLibrary = null;
        private static IntPtr LoadPackagedLibrarySafe(String fileName, int dwFlags)
        {
            if (_kernel32HasLoadPackagedLibrary == null)
            {
                UIntPtr = GetProcAddress("")
            }
        }*/

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadPackagedLibrary(
            [MarshalAs(UnmanagedType.LPStr)]
            String fileName,
            int dwFlags);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(
            [MarshalAs(UnmanagedType.LPStr)]
            String fileName,
            IntPtr hFile,
            int dwFlags);

        [DllImport("dl", EntryPoint = "dlopen")]
        private static extern IntPtr Dlopen(
            [MarshalAs(UnmanagedType.LPStr)]
            String dllname, int mode);

        /*
        /// <summary>
        /// Decrements the reference count of the loaded dynamic-link library (DLL). When the reference count reaches zero, the module is unmapped from the address space of the calling process and the handle is no longer valid
        /// </summary>
        /// <param name="handle">The handle to the library</param>
        /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr handle);
        */
        
        /// <summary>
        /// Set the directory to the search path used to locate DLLs for the application
        /// </summary>
        /// <param name="path">The directory to be searched for DLLs</param>
        /// <returns>True if success</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(String path);
        

        /// <summary>
        /// Adds a directory to the search path used to locate DLLs for the application
        /// </summary>
        /// <param name="path">The directory to be searched for DLLs</param>
        /// <returns>True if success</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddDllDirectory(String path);

    }
}
