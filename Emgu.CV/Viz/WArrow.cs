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
    /// This 3D Widget defines an arrow.
    /// </summary>
    public class WArrow : UnmanagedObject, IWidget3D
    {
        private IntPtr _widgetPtr;
        private IntPtr _widget3dPtr;

        /// <summary>
        /// Constructs an WArrow.
        /// </summary>
        /// <param name="pt1">Start point of the arrow.</param>
        /// <param name="pt2">End point of the arrow.</param>
        /// <param name="thickness">Thickness of the arrow. Thickness of arrow head is also adjusted accordingly.</param>
        /// <param name="color">Color of the arrow.</param>
        public WArrow(MCvPoint3D64f pt1, MCvPoint3D64f pt2, double thickness, MCvScalar color)
        {
            _ptr = CvInvoke.cveWArrowCreate(ref pt1, ref pt2, thickness, ref color, ref _widget3dPtr, ref _widgetPtr);
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
        /// Release the unmanaged memory associated with this WArrow object
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_ptr))
                CvInvoke.cveWArrowRelease(ref _ptr);
            _widgetPtr = IntPtr.Zero;
            _widget3dPtr = IntPtr.Zero;
        }
    }

    public static partial class CvInvoke
    {

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveWArrowCreate(ref MCvPoint3D64f pt1, ref MCvPoint3D64f pt2, double thickness, ref MCvScalar color, ref IntPtr widget3d, ref IntPtr widget);

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveWArrowRelease(ref IntPtr arrow);
    }
}
#endif