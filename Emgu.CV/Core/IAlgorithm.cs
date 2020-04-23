//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------


using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Interface to the algorithm class
    /// </summary>
    public interface IAlgorithm
    {
        /// <summary>
        /// Return the pointer to the algorithm object
        /// </summary>
        /// <returns>The pointer to the algorithm object</returns>
        IntPtr AlgorithmPtr { get; }
    }

    /// <summary>
    /// Extension methods to the IAlgorithm interface
    /// </summary>
    public static class AlgorithmExtensions
    {
        /// <summary>
        /// Reads algorithm parameters from a file storage.
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="node">The node from file storage.</param>
        public static void Read(this IAlgorithm algorithm, FileNode node)
        {
            CvInvoke.cveAlgorithmRead(algorithm.AlgorithmPtr, node);
        }

        /// <summary>
        /// Stores algorithm parameters in a file storage
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="storage">The storage.</param>
        public static void Write(this IAlgorithm algorithm, FileStorage storage)
        {
            CvInvoke.cveAlgorithmWrite(algorithm.AlgorithmPtr, storage);
        }

        /// <summary>
        /// Stores algorithm parameters in a file storage
        /// </summary>
        /// <param name="algorithm">The algorithm.</param>
        /// <param name="storage">The storage.</param>
        /// <param name="name">The name of the node</param>
        public static void Write(this IAlgorithm algorithm, FileStorage storage, String name)
        {
            using (CvString csName = new CvString(name))
                CvInvoke.cveAlgorithmWrite2(algorithm.AlgorithmPtr, storage, csName);
        }

        /// <summary>
        /// Save the algorithm to file
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <param name="fileName">The file name where this algorithm will be saved to</param>
        public static void Save(this IAlgorithm algorithm, String fileName)
        {
            using (CvString fs = new CvString(fileName))
                CvInvoke.cveAlgorithmSave(algorithm.AlgorithmPtr, fs);
        }

        /// <summary>
        /// Save the algorithm to a string
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <param name="format">file format, can be .xml or .yml</param>
        /// <returns>The algorithm as an yml string</returns>
        public static String SaveToString(this IAlgorithm algorithm, String format=".xml")
        {
            using (FileStorage fs = new FileStorage(format, FileStorage.Mode.Write | FileStorage.Mode.Memory))
            {
                fs.Insert(algorithm.GetDefaultName());
                fs.Insert("{");
                algorithm.Write(fs);
                fs.Insert("}");
                return fs.ReleaseAndGetString();
            }
        }

        /// <summary>
        /// Clear the algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        public static void Clear(this IAlgorithm algorithm)
        {
            CvInvoke.cveAlgorithmClear(algorithm.AlgorithmPtr);
        }

        /// <summary>
        /// Returns true if the Algorithm is empty. e.g. in the very beginning or after unsuccessful read.
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <returns>Returns true if the Algorithm is empty. e.g. in the very beginning or after unsuccessful read.</returns>
        public static bool IsEmpty(this IAlgorithm algorithm)
        {
            return CvInvoke.cveAlgorithmEmpty(algorithm.AlgorithmPtr);
        }

        /// <summary>
        /// Loads algorithm from the file
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <param name="fileName">Name of the file to read.</param>
        /// <param name="objName">The optional name of the node to read (if empty, the first top-level node will be used)</param>
        /// <param name="encoding">Encoding of the file. Note that UTF-16 XML encoding is not supported currently and
        /// you should use 8-bit encoding instead of it.</param>
        public static void Load(this IAlgorithm algorithm, String fileName, String objName = null, String encoding = null)
        {
            using (FileStorage fs = new FileStorage(fileName, FileStorage.Mode.Read, encoding))
            using (FileNode fn = objName == null ? fs.GetFirstTopLevelNode() : fs[objName])
            {
                algorithm.Read(fn);
            }
        }

        /// <summary>
        /// Loads algorithm from a String
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <param name="strModel">The string variable containing the model you want to load.</param>
        /// <param name="objName">The optional name of the node to read (if empty, the first top-level node will be used)</param>
        /// <param name="encoding">Encoding of the file. Note that UTF-16 XML encoding is not supported currently and
        /// you should use 8-bit encoding instead of it.</param>
        public static void LoadFromString(this IAlgorithm algorithm, String strModel, String objName = null, String encoding = null)
        {
            using (FileStorage fs = new FileStorage(strModel, FileStorage.Mode.Read | FileStorage.Mode.Memory, encoding))
            using (FileNode fn = objName == null ? fs.GetFirstTopLevelNode() : fs[objName])
            {
                algorithm.Read(fn);
            }
        }

        /// <summary>
        /// Returns the algorithm string identifier.
        /// This string is used as top level xml/yml node tag when the object is saved to a file or string.
        /// </summary>
        /// <param name="algorithm">The algorithm</param>
        /// <returns>
        /// Returns the algorithm string identifier.
        /// This string is used as top level xml/yml node tag when the object is saved to a file or string.
        /// </returns>
        public static String GetDefaultName(this IAlgorithm algorithm)
        {
            using (CvString s = new CvString())
            {
                CvInvoke.cveAlgorithmGetDefaultName(algorithm.AlgorithmPtr, s);
                return s.ToString();
            }
        }
    }

    public partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAlgorithmRead(IntPtr algorithm, IntPtr node);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAlgorithmWrite(IntPtr algorithm, IntPtr storage);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAlgorithmWrite2(IntPtr algorithm, IntPtr storage, IntPtr name);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAlgorithmSave(IntPtr algorithm, IntPtr filename);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAlgorithmClear(IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal extern static bool cveAlgorithmEmpty(IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAlgorithmGetDefaultName(IntPtr algorithm, IntPtr defaultName);
    }
}
