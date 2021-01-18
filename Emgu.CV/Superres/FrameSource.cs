//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;
using Emgu.CV.Util;

namespace Emgu.CV.Superres
{
    /// <summary>
    /// Create a video frame source
    /// </summary>
    public class FrameSource : SharedPtrObject
    {

        /// <summary>
        /// Create video frame source from video file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <param name="tryUseGpu">If true, it will try to create video frame source using gpu</param>
        public FrameSource(String fileName, bool tryUseGpu)
        {
            using (CvString s = new CvString(fileName))
                if (tryUseGpu)
                {
                    try
                    {
                        _ptr = SuperresInvoke.cveSuperresCreateFrameSourceVideo(s, true, ref _sharedPtr);
                    }
                    catch
                    {
                        _ptr = SuperresInvoke.cveSuperresCreateFrameSourceVideo(s, false, ref _sharedPtr);
                    }
                }
                else
                {
                    _ptr = SuperresInvoke.cveSuperresCreateFrameSourceVideo(s, false, ref _sharedPtr);
                }

            //_frameSourcePtr = _ptr;
        }

        /// <summary> Create a framesource using the specific camera</summary>
        /// <param name="camIndex"> The index of the camera to create capture from, starting from 0</param>
        public FrameSource(int camIndex)
        {
            _ptr = SuperresInvoke.cveSuperresCreateFrameSourceCamera(camIndex, ref _sharedPtr);
            //_frameSourcePtr = _ptr;
        }

        
        internal FrameSource()
        {
        }

        /// <summary>
        /// Get the next frame
        /// </summary>
        /// <param name="frame">The output array to put the frame to</param>
        public void NextFrame(IOutputArray frame)
        {
            using (OutputArray oaFrame = frame.GetOutputArray())
                SuperresInvoke.cveSuperresFrameSourceNextFrame(FrameSourcePtr, oaFrame);

        }

        /// <summary>
        /// Get the pointer to the frame source
        /// </summary>
        protected virtual IntPtr FrameSourcePtr
        {
            get { return _ptr; }
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this framesource
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {

                SuperresInvoke.cveSuperresFrameSourceRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    internal static partial class SuperresInvoke
    {
        static SuperresInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSuperresCreateFrameSourceVideo(
           IntPtr fileName,
           [MarshalAs(CvInvoke.BoolMarshalType)]
         bool useGpu,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSuperresCreateFrameSourceCamera(int deviceId, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSuperresFrameSourceRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSuperresFrameSourceNextFrame(IntPtr frameSource, IntPtr frame);
    }

}
