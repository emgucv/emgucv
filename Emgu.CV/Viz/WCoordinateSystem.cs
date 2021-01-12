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
    /// This 3D Widget represents a coordinate system.
    /// </summary>
    public class WCoordinateSystem : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs a WCoordinateSystem.
        /// </summary>
        /// <param name="scale">Determines the size of the axes.</param>
        public WCoordinateSystem(double scale = 1.0)
        {
            _ptr = CvInvoke.cveWCoordinateSystemCreate(scale, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WCoordinateSysyem object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWCoordinateSystemRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWCoordinateSystemCreate(double scale, ref IntPtr widget3d, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWCoordinateSystemRelease(ref IntPtr system);
    }
}
#endif