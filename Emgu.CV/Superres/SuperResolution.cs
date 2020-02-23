//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV.Superres
{
    /// <summary>
    /// Supper resolution
    /// </summary>
    public class SuperResolution : FrameSource
    {

        /// <summary>
        /// The pointer to the frame source
        /// </summary>
        protected IntPtr _frameSourcePtr;

        /// <summary>
        /// The type of optical flow algorithms used for super resolution
        /// </summary>
        public enum OpticalFlowType
        {
            /// <summary>
            /// BTVL
            /// </summary>
            Btvl = 0,
            /// <summary>
            /// BTVL using gpu
            /// </summary>
            Btvl1Gpu = 1,
        }

        /// <summary>
        /// Create a super resolution solver for the given frameSource
        /// </summary>
        /// <param name="type">The type of optical flow algorithm to use</param>
        /// <param name="frameSource">The frameSource</param>
        public SuperResolution(SuperResolution.OpticalFlowType type, FrameSource frameSource)
           : base()
        {
            _ptr = SuperresInvoke.cveSuperResolutionCreate(type, frameSource, ref _frameSourcePtr, ref _sharedPtr);
        }

        /// <summary>
        /// Get the pointer to the frame source
        /// </summary>
        protected override IntPtr FrameSourcePtr
        {
            get { return _frameSourcePtr; }
        }

        /// <summary>
        /// Release all the unmanaged memory associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                SuperresInvoke.cveSuperResolutionRelease(ref _sharedPtr);
                _frameSourcePtr = IntPtr.Zero;
                _ptr = IntPtr.Zero;
            }
        }
    }

    internal static partial class SuperresInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSuperResolutionCreate(SuperResolution.OpticalFlowType type, IntPtr frameSource, ref IntPtr frameSourceOut, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSuperResolutionRelease(ref IntPtr sharedPtr);
    }
}
