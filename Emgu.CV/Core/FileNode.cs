//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// File Storage Node class.
    /// The node is used to store each and every element of the file storage opened for reading. When
    /// XML/YAML file is read, it is first parsed and stored in the memory as a hierarchical collection of
    /// nodes. Each node can be a "leaf" that is contain a single number or a string, or be a collection of
    /// other nodes. There can be named collections (mappings) where each element has a name and it is
    /// accessed by a name, and ordered collections (sequences) where elements do not have names but rather
    /// accessed by index. Type of the file node can be determined using FileNode::type method.
    /// Note that file nodes are only used for navigating file storages opened for reading. When a file
    /// storage is opened for writing, no data is stored in memory after it is written.
    /// </summary>
    public partial class FileNode : UnmanagedObject, IEnumerable<FileNode>
    {
        /// <summary>
        /// Type of the file storage node
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Empty node
            /// </summary>
            None = 0,
            ///<summary>
            ///  an integer
            ///</summary>
            Int = 1,
            /// <summary>
            /// Floating-point number
            /// </summary>
            Real = 2,
            /// <summary>
            /// Synonym or Real
            /// </summary>
            Float = Real,
            /// <summary>
            /// Text string in UTF-8 encoding
            /// </summary>
            Str = 3,
            /// <summary>
            /// Synonym for Str
            /// </summary>
            String = Str,
    
            /// <summary>
            /// The sequence
            /// </summary>
            Seq = 4,
            /// <summary>
            /// Mapping
            /// </summary>
            Map = 5,
            /// <summary>
            /// The type mask
            /// </summary>
            TypeMask = 7,
            /// <summary>
            /// Used only when writing. Compact representation of a sequence or mapping. Used only by YAML writer
            /// </summary>
            Flow = 8,
            /// <summary>
            /// Used only when reading FileStorage. If set, means that all the collection elements are numbers of the same type (real's or int's).
            /// </summary>
            Uniform = 8,
            /// <summary>
            /// Empty structure (sequence or mapping)
            /// </summary>
            Empty = 16,
            /// <summary>
            /// The node has a name (i.e. it is element of a mapping)
            /// </summary>
            Named = 32
        };

        internal FileNode(IntPtr ptr)
        {
            _ptr = ptr;
        }


        /// <summary>
        /// Reads a Mat from the node
        /// </summary>
        /// <param name="mat">The Mat where the result is read into</param>
        /// <param name="defaultMat">The default mat.</param>
        public void ReadMat(Mat mat, Mat defaultMat = null)
        {
            if (defaultMat == null)
            {
                using (Mat def = new Mat())
                    CvInvoke.cveFileNodeReadMat(_ptr, mat, def);
            }
            else
            {
                CvInvoke.cveFileNodeReadMat(_ptr, mat, defaultMat);
            }
        }

        /// <summary>
        /// Gets the type of the node.
        /// </summary>
        /// <value>
        /// The type of the node.
        /// </value>
        public Type NodeType
        {
            get { return (Type)CvInvoke.cveFileNodeGetType(_ptr); }
        }

        /// <summary>
        /// Get the node name or an empty string if the node is nameless
        /// </summary>
        public String Name
        {
            get
            {
                if (!IsNamed)
                    return String.Empty;
                using (CvString csName = new CvString())
                {
                    CvInvoke.cveFileNodeGetName(_ptr, csName);
                    return csName.ToString();
                }
            }
        }

        /*
        /// <summary>
        /// Get the keys of a mapping node.
        /// </summary>
        public String[] Keys
        {
            get
            {
                using (VectorOfCvString vsKeys = new VectorOfCvString())
                {
                    CvInvoke.cveFileNodeGetName(_ptr, vsKeys);
                    return vsKeys.ToArray();
                }
            }
        }*/

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveFileNodeRelease(ref _ptr);
        }

        /// <summary>
        /// Reads the string from the node
        /// </summary>
        /// <param name="defaultString">The default value if one is not found in the node.</param>
        /// <returns>The string from the node</returns>
        public String ReadString(String defaultString = null)
        {
            using (CvString s = new CvString())
            using (CvString ds = new CvString(defaultString))
            {
                CvInvoke.cveFileNodeReadString(_ptr, s, ds);
                return s.ToString();
            }
        }

        /// <summary>
        /// Reads the int from the node.
        /// </summary>
        /// <param name="defaultInt">The default value if one is not found in the node.</param>
        /// <returns>The int from the node.</returns>
        public int ReadInt(int defaultInt = int.MinValue)
        {
            return CvInvoke.cveFileNodeReadInt(_ptr, defaultInt);
        }

        /// <summary>
        /// Reads the float from the node.
        /// </summary>
        /// <param name="defaultFloat">The default value if one is not found in the node.</param>
        /// <returns>The float from the node.</returns>
        public float ReadFloat(float defaultFloat = float.MinValue)
        {
            return CvInvoke.cveFileNodeReadFloat(_ptr, defaultFloat);
        }

        /// <summary>
        /// Reads the double from the node.
        /// </summary>
        /// <param name="defaultDouble">The default value if one is not found in the node.</param>
        /// <returns>The double from the node.</returns>
        public double ReadDouble(double defaultDouble = Double.MinValue)
        {
            return CvInvoke.cveFileNodeReadDouble(_ptr, defaultDouble);
        }

        /// <summary>
        /// Get an enumerator of the file node children
        /// </summary>
        /// <returns>An enumerator of the file node children</returns>
        public IEnumerator<FileNode> GetEnumerator()
        {
            using (FileNodeIterator it = new FileNodeIterator(this, false))
            using (FileNodeIterator end = new FileNodeIterator(this, true))
            {
                while (!it.Equals(end))
                {
                    yield return it.GetFileNode();
                    it.Next();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFileNodeRelease(ref IntPtr node);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFileNodeReadMat(IntPtr node, IntPtr mat, IntPtr defaultMat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveFileNodeGetType(IntPtr node);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFileNodeGetName(IntPtr node, IntPtr name);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFileNodeGetKeys(IntPtr node, IntPtr keys);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFileNodeReadString(IntPtr node, IntPtr str, IntPtr defaultStr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveFileNodeReadInt(IntPtr node, int defaultInt);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern double cveFileNodeReadDouble(IntPtr node, double defaultDouble);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveFileNodeReadFloat(IntPtr node, float defaultFloat);
    }
}
