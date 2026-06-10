//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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
    public class Octree : SharedPtrObject
    {
        /// <summary>
        /// Creates an octree from a point cloud up to the specified depth.
        /// </summary>
        /// <param name="pointCloud">The input point cloud.</param>
        /// <param name="maxDepth">The maximum depth of the octree.</param>
        public Octree(VectorOfPoint3D32F pointCloud, int maxDepth)
        {
            _ptr = CvInvoke.cveOctreeCreate(pointCloud, maxDepth, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                CvInvoke.cveOctreeRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV functions
    /// </summary>
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOctreeCreate(IntPtr pointCloud, int maxDepth, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOctreeRelease(ref IntPtr sharedPtr);
    }
}
