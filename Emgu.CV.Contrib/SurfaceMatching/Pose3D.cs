//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.PpfMatch3d
{
    /// <summary>
    /// Class, allowing the storage of a pose. The data structure stores both the quaternions and the matrix forms. It supports IO functionality together with various helper methods to work with poses.
    /// </summary>
    public partial class Pose3D : UnmanagedObject
    {
        private bool _needDispose = true;
        
        /// <summary>
        /// Create a new Pose3D
        /// </summary>
        public Pose3D()
        {
            _ptr = PpfMatch3dInvoke.cvePose3DCreate();
            _needDispose = true;
        }

        internal Pose3D(IntPtr pose3dPtr, bool needDispose)
        {
            _ptr = pose3dPtr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Updates the pose with the new one.
        /// </summary>
        /// <param name="pose">New pose to overwrite</param>
        public void UpdatePose(Mat pose)
        {
            PpfMatch3dInvoke.cvePose3DUpdatePose(_ptr, pose);
        }

        /// <summary>
        /// Release the unmanaged resources associated with the Pose3D
        /// </summary>
        protected override void DisposeObject()
        {
            if (_needDispose && IntPtr.Zero != _ptr)
                PpfMatch3dInvoke.cvePose3DRelease(ref _ptr);
        }

        /// <summary>
        /// The translation vector
        /// </summary>
        public MCvPoint3D64f T
        {
            get
            {
                MCvPoint3D64f translation = new MCvPoint3D64f();
                PpfMatch3dInvoke.cvePose3DGetT(_ptr, ref translation);
                return translation;
            }
            set
            {
                PpfMatch3dInvoke.cvePose3DSetT(_ptr, ref value);
            }
        }

        /// <summary>
        /// Get or set the quaternion value
        /// </summary>
        public MCvScalar Q
        {
            get
            {
                MCvScalar q = new MCvScalar();
                PpfMatch3dInvoke.cvePose3DGetQ(_ptr, ref q);
                return q;
            }
            set
            {
                PpfMatch3dInvoke.cvePose3DSetQ(_ptr, ref value);
            }
        }
    }

    /// <summary>
    /// Entry points to the Open CV Surface Matching module
    /// </summary>
    public static partial class PpfMatch3dInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cvePose3DCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePose3DUpdatePose(IntPtr pose3d, IntPtr pose);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePose3DRelease(ref IntPtr icp);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePose3DGetT(IntPtr pose3d, ref MCvPoint3D64f t);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePose3DSetT(IntPtr pose3d, ref MCvPoint3D64f t);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePose3DGetQ(IntPtr pose3d, ref MCvScalar q);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePose3DSetQ(IntPtr pose3d, ref MCvScalar q);
    }
}