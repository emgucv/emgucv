//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
    /// DepthAI MonoCamera
    /// </summary>
    public partial class MonoCamera : SharedPtrObject, INode
    {
        /// <summary>
        /// Enum representing the resolution of the MonoCamera sensor.
        /// </summary>
        public enum SensorResolution
        {
            /// <summary>
            /// Represents a resolution of 720p.
            /// </summary>
            The720P,
            /// <summary>
            /// Represents a resolution of 800p.
            /// </summary>
            The800P,
            /// <summary>
            /// Represents a resolution of 400p.
            /// </summary>
            The400P,
            /// <summary>
            /// Represents a resolution of 480p.
            /// </summary>
            The480P
        };

        private IntPtr _nodePtr;

        internal MonoCamera(IntPtr monoCameraPtr, IntPtr monoCameraSharedPtr, IntPtr nodePtr)
        {
            _ptr = monoCameraPtr;
            _sharedPtr = monoCameraSharedPtr;
            _nodePtr = nodePtr;
        }

        /// <summary>
        /// Release all unmanaged memory associated with the MonoCamera.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DaiInvoke.daiMonoCameraRelease(ref _sharedPtr);
				_ptr = IntPtr.Zero;
                _nodePtr = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the output of the MonoCamera.
        /// </summary>
        /// <returns>
        /// A NodeOutput object representing the output of the MonoCamera.
        /// </returns>
        public NodeOutput GetOutput()
        {
            return new NodeOutput(DaiInvoke.daiMonoCameraGetOutput(_ptr), false);
        }

        /// <summary>
        /// The camera image orientation
        /// </summary>
        public CameraImageOrientation Orientation
        {
            get { return DaiInvoke.daiMonoCameraGetImageOrientation(_ptr); }
            set { DaiInvoke.daiMonoCameraSetImageOrientation(_ptr, value); }
        }

        /// <summary>
        /// Gets or sets the camera board socket for the DepthAI module.
        /// </summary>
        /// <value>
        /// The camera board socket.
        /// </value>
        /// <remarks>
        /// The DepthAI module uses this property to determine which camera to use for image processing tasks.
        /// </remarks>
        public CameraBoardSocket BoardSocket
        {
            get
            {
                return DaiInvoke.daiMonoCameraGetBoardSocket(_ptr);
            }
            set
            {
                DaiInvoke.daiMonoCameraSetBoardSocket(_ptr, value);
            }
        }

        /// <summary>
        /// Gets or sets the resolution of the MonoCamera sensor.
        /// </summary>
        /// <value>
        /// The resolution of the MonoCamera sensor.
        /// </value>
        /// <remarks>
        /// The resolution is represented by the SensorResolution enum.
        /// </remarks>
        public SensorResolution Resolution
        {
            get
            {
                return DaiInvoke.daiMonoCameraGetResolution(_ptr);
            }
            set
            {
                DaiInvoke.daiMonoCameraSetResolution(_ptr, value);
            }
        }

        /// <summary>
        /// Gets the pointer to the node of the MonoCamera.
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
        internal static extern void daiMonoCameraRelease(ref IntPtr monoCameraSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiMonoCameraGetOutput(IntPtr monoCameraPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiMonoCameraSetBoardSocket(IntPtr monoCamera, CameraBoardSocket boardSocket);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern CameraBoardSocket daiMonoCameraGetBoardSocket(IntPtr monoCamera);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern CameraImageOrientation daiMonoCameraGetImageOrientation(IntPtr monoCamera);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiMonoCameraSetImageOrientation(
            IntPtr monoCamera,
            CameraImageOrientation val);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern MonoCamera.SensorResolution daiMonoCameraGetResolution(IntPtr monoCamera);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiMonoCameraSetResolution(
            IntPtr monoCamera,
            MonoCamera.SensorResolution val);

    }
}