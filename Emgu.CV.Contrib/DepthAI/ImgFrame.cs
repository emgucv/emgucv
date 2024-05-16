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
    /// DepthAI ImgFrame
    /// </summary>
    public partial class ImgFrame : SharedPtrObject
    {

        internal ImgFrame(IntPtr imgFramePtr, IntPtr imgFrameSharedPtr)
        {
            _ptr = imgFramePtr;
			_sharedPtr = imgFrameSharedPtr;
        }

        /// <summary>
        /// Retrieves the data pointer of the image frame.
        /// </summary>
        /// <returns>
        /// The IntPtr to the data of the image frame.
        /// </returns>
        public IntPtr GetDataPtr()
        {
            return DaiInvoke.daiImgFrameGetData(_ptr);
        }

        /// <summary>
        /// Release all unmanaged memory associated with the ImgFrame.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DaiInvoke.daiImgFrameRelease(ref _sharedPtr);
				_ptr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Entry points for the DepthAI module.
    /// </summary>
    public static partial class DaiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiImgFrameRelease(ref IntPtr imgFrameSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiImgFrameGetData(IntPtr imgFrame);
    }
}