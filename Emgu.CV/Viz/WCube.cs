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
    /// This 3D Widget defines a cube.
    /// </summary>
    public class WCube : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs a WCube.
        /// </summary>
        /// <param name="minPoint">Specifies minimum point of the bounding box.</param>
        /// <param name="maxPoint">Specifies maximum point of the bounding box.</param>
        /// <param name="wireFrame">If true, cube is represented as wireframe.</param>
        /// <param name="color">Color of the cube.</param>
        public WCube(MCvPoint3D64f minPoint, MCvPoint3D64f maxPoint, bool wireFrame, MCvScalar color)
        {
            _ptr = CvInvoke.cveWCubeCreate(ref minPoint, ref maxPoint, wireFrame, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCube object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCubeRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCubeCreate(
           ref MCvPoint3D64f minPoint, ref MCvPoint3D64f maxPoint,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool wireFrame, ref MCvScalar color,
           ref IntPtr widget3d, ref IntPtr widget);
        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCubeRelease(ref IntPtr cube);

    }
}
#endif