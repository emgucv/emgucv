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
    /// DepthAI MonoCamera
    /// </summary>
    public partial class MonoCamera : SharedPtrObject, INode
    {
        public enum SensorResolution
        {
            The720P, 
            The800P, 
            The400P, 
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