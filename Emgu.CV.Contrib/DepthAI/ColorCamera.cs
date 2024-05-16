//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
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
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Dai
{

    /// <summary>
    /// DepthAI ColorCamera
    /// </summary>
    public partial class ColorCamera : SharedPtrObject, INode
    {
        private IntPtr _nodePtr;

        internal ColorCamera(IntPtr colorCameraPtr, IntPtr colorCameraSharedPtr, IntPtr nodePtr)
        {
            _ptr = colorCameraPtr;
            _sharedPtr = colorCameraSharedPtr;
            _nodePtr = nodePtr;
        }

        /// <summary>
        /// Release all unmanaged memory associated with the ColorCamera.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DaiInvoke.daiColorCameraRelease(ref _sharedPtr);
				_ptr = IntPtr.Zero;
                _nodePtr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the preview output from the color camera.
        /// </summary>
        /// <returns>
        /// A NodeOutput object representing the preview output of the color camera.
        /// </returns>
        public NodeOutput GetPreview()
        {
            return new NodeOutput(DaiInvoke.daiColorCameraGetPreview(_ptr), false);
        }

        /// <summary>
        /// The camera image orientation
        /// </summary>
        public CameraImageOrientation Orientation
        {
            get { return DaiInvoke.daiColorCameraGetImageOrientation(_ptr); }
            set { DaiInvoke.daiColorCameraSetImageOrientation(_ptr, value); }
        }

        /// <summary>
        /// Gets the pointer to the node associated with this ColorCamera instance.
        /// </summary>
        /// <value>
        /// The pointer to the node.
        /// </value>
        public IntPtr NodePtr
        {
            get { return _nodePtr; }
        }
    }

    /// <summary>
    /// Entry points for the DepthAI module.
    /// </summary>
    public static partial class DaiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiColorCameraRelease(ref IntPtr colorCameraSharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiColorCameraGetPreview(IntPtr colorCamera);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern CameraImageOrientation daiColorCameraGetImageOrientation(IntPtr colorCamera);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiColorCameraSetImageOrientation(
            IntPtr colorCamera,
            CameraImageOrientation val);

    }
}