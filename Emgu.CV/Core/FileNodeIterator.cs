//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
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
    /// Used to iterate through sequences and mappings.
    /// </summary>
    public partial class FileNodeIterator : UnmanagedObject, IEquatable<FileNodeIterator>
    {
        /// <summary>
        /// Create a blank file node iterator
        /// </summary>
        internal FileNodeIterator()
        {
            _ptr = CvInvoke.cveFileNodeIteratorCreate();
        }

        /// <summary>
        /// Create a FileNodeIterator from a specific node.
        /// </summary>
        /// <param name="node">the collection to iterate over</param>
        /// <param name="seekEnd">True if iterator needs to be set after the last element of the node</param>
        public FileNodeIterator(FileNode node, bool seekEnd)
        {
            _ptr = CvInvoke.cveFileNodeIteratorCreateFromNode(node, seekEnd);
        }

        /// <summary>
        /// Check if the current iterator equals to the other.
        /// </summary>
        /// <param name="iterator">The other iterator to compares with.</param>
        /// <returns>True if the current iterator equals to the other</returns>
        public bool Equals(FileNodeIterator iterator)
        {
            return CvInvoke.cveFileNodeIteratorEqualTo(_ptr, iterator);
        }

        /// <summary>
        /// moves iterator to the next node
        /// </summary>
        public void Next()
        {
            CvInvoke.cveFileNodeIteratorNext(_ptr);
        }

        /// <summary>
        /// Get the currently observed element
        /// </summary>
        /// <returns>The currently observed element</returns>
        public FileNode GetFileNode()
        {
            return new FileNode(CvInvoke.cveFileNodeIteratorGetFileNode(_ptr));
        }

        /// <summary>
        /// Release the unmanaged resources
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                CvInvoke.cveFileNodeIteratorRelease(ref _ptr);
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFileNodeIteratorCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFileNodeIteratorRelease(ref IntPtr iterator);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFileNodeIteratorCreateFromNode(
            IntPtr node, 
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool seekEnd);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveFileNodeIteratorEqualTo(IntPtr iterator, IntPtr otherIterator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFileNodeIteratorNext(IntPtr iterator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFileNodeIteratorGetFileNode(IntPtr iterator);

    }
}
