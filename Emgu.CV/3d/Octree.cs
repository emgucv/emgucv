//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV
{
    /// <summary>
    /// Represents a spatial partitioning data structure that organizes 3D point clouds using an octree for efficient
    /// spatial queries and operations.
    /// </summary>
    /// <remarks>An octree recursively subdivides 3D space into eight octants, enabling efficient management
    /// and querying of large point cloud datasets. This class manages unmanaged resources and should be disposed of
    /// properly to release native memory. Thread safety is not guaranteed; synchronize access if used from multiple
    /// threads.</remarks>
    public class Octree : UnmanagedObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Octree"/> class.
        /// </summary>
        public Octree()
        {
            _ptr = CvInvoke.cveOctreeCreate();
        }

        /// <summary>
        /// Creates an octree structure from a given point cloud.
        /// </summary>
        /// <param name="pointCloud">
        /// A <see cref="VectorOfPoint3D32F"/> object representing the input point cloud.
        /// </param>
        /// <param name="maxDepth">
        /// The maximum depth of the octree. This determines the level of detail in the octree structure.
        /// </param>
        /// <returns>
        /// A boolean value indicating whether the octree creation was successful.
        /// </returns>
        public bool Create(VectorOfPoint3D32F pointCloud, int maxDepth)
        {
            return CvInvoke.cveOctreeCreate2(_ptr, pointCloud, maxDepth);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveOctreeRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV functions
    /// </summary>
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOctreeCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOctreeRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveOctreeCreate2(IntPtr octree, IntPtr pointCloud, int maxDepth);
    }
}
