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
    /// DepthAI DataOutputQueue
    /// </summary>
    public partial class DataOutputQueue : SharedPtrObject
    {

        internal DataOutputQueue(IntPtr dataOutputQueuePtr, IntPtr dataOutputQueueSharedPtr)
        {
            _ptr = dataOutputQueuePtr;
			_sharedPtr = dataOutputQueueSharedPtr;
        }

        /// <summary>
        /// Retrieves an image frame from the data output queue.
        /// </summary>
        /// <returns>
        /// An instance of the <see cref="ImgFrame"/> class representing the retrieved image frame.
        /// </returns>
        public ImgFrame GetImgFrame()
        {
            IntPtr imgFrameSharedPtr = IntPtr.Zero;
            
            IntPtr imgFramePtr = DaiInvoke.daiDataOutputQueueGetImgFrame(_ptr, ref imgFrameSharedPtr);
            return new ImgFrame(imgFramePtr, imgFrameSharedPtr);
        }


        /// <summary>
        /// Release all unmanaged memory associated with the DataOutputQueue.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DaiInvoke.daiDataOutputQueueRelease(ref _sharedPtr);
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
        internal static extern void daiDataOutputQueueRelease(ref IntPtr dataOutputQueueSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiDataOutputQueueGetImgFrame(IntPtr dataOutputQueue, ref IntPtr imgFrameSharedPtr);
    }
}