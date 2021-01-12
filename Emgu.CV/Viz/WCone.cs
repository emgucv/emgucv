//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
#if ! ( UNITY_IOS || UNITY_ANDROID )

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Emgu.CV
{
    /// <summary>
    /// This 3D Widget defines a cone.
    /// </summary>
    public class WCone : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs default cone oriented along x-axis with center of its base located at origin.
        /// </summary>
        /// <param name="length">Length of the cone.</param>
        /// <param name="radius">Radius of the cone.</param>
        /// <param name="resolution">Resolution of the cone.</param>
        /// <param name="color">Color of the cone.</param>
        public WCone(double length, double radius, int resolution, MCvScalar color)
        {
            _ptr = CvInvoke.cveWConeCreateAtOrigin(length, radius, resolution, ref color, ref _widget3dPtr, ref _widgetPtr);
        }

        /// <summary>
        /// Constructs repositioned planar cone.
        /// </summary>
        /// <param name="radius">Radius of the cone.</param>
        /// <param name="center">Center of the cone base.</param>
        /// <param name="tip">Tip of the cone.</param>
        /// <param name="resolution">Resolution of the cone.</param>
        /// <param name="color">Color of the cone.</param>
        public WCone(double radius, MCvPoint3D64f center, MCvPoint3D64f tip, int resolution, MCvScalar color)
        {
            _ptr = CvInvoke.cveWConeCreate(radius, ref center, ref tip, resolution, ref color, ref _widget3dPtr, ref _widgetPtr);
        }

        /// <summary>
        /// Get the pointer to the Widget3D obj
        /// </summary>
        public IntPtr GetWidget3D
        {
            get { return _widget3dPtr; }
        }

        /// <summary>
        /// Get the pointer to the Widget obj
        /// </summary>
        public IntPtr GetWidget
        {
            get { return _widgetPtr; }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this WCone object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWConeRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }


    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWConeCreateAtOrigin(double length, double radius, int resolution, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWConeCreate(double radius, ref MCvPoint3D64f center, ref MCvPoint3D64f tip, int resolution, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWConeRelease(ref IntPtr cone);

    }
}
#endif