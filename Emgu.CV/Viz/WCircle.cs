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
    /// This 3D Widget defines a circle.
    /// </summary>
    public class WCircle : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs default planar circle centred at origin with plane normal along z-axis.
        /// </summary>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="thickness">Thickness of the circle.</param>
        /// <param name="color">Color of the circle.</param>
        public WCircle(double radius, double thickness, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCircleCreateAtOrigin(radius, thickness, ref color, ref _widget3dPtr, ref _widgetPtr);
        }

        /// <summary>
        /// Constructs repositioned planar circle.
        /// </summary>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="center">Center of the circle.</param>
        /// <param name="normal">Normal of the plane in which the circle lies.</param>
        /// <param name="thickness">Thickness of the circle.</param>
        /// <param name="color">Color of the circle.</param>
        public WCircle(double radius, MCvPoint3D64f center, MCvPoint3D64f normal, double thickness, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCircleCreate(radius, ref center, ref normal, thickness, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCircle object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCircleRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCircleCreateAtOrigin(double radius, double thickness, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCircleCreate(double radius, ref MCvPoint3D64f center, ref MCvPoint3D64f normal, double thickness, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCircleRelease(ref IntPtr circle);

    }
}
#endif