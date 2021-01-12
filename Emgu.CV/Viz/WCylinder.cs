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
    /// This 3D Widget defines a cylinder.
    /// </summary>
    public class WCylinder : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs a WCylinder.
        /// </summary>
        /// <param name="axisPoint1">A point1 on the axis of the cylinder.</param>
        /// <param name="axisPoint2">A point2 on the axis of the cylinder.</param>
        /// <param name="radius">Radius of the cylinder.</param>
        /// <param name="numsides">Resolution of the cylinder.</param>
        /// <param name="color">Color of the cylinder.</param>
        public WCylinder(ref MCvPoint3D64f axisPoint1, MCvPoint3D64f axisPoint2, double radius, int numsides, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCylinderCreate(ref axisPoint1, ref axisPoint2, radius, numsides, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCylinder object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCylinderRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCylinderCreate(ref MCvPoint3D64f axisPoint1, ref MCvPoint3D64f axisPoint2, double radius, int numsides, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCylinderRelease(ref IntPtr cylinder);

    }
}
#endif