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
    /// DepthAI XLinkOut
    /// </summary>
    public partial class XLinkOut : SharedPtrObject
    {

        internal XLinkOut(IntPtr xLinkOutPtr, IntPtr xLinkOutSharedPtr)
        {
            _ptr = xLinkOutPtr;
			_sharedPtr = xLinkOutSharedPtr;
        }

        /// <summary>
        /// Retrieves the input node for the XLinkOut instance.
        /// </summary>
        /// <returns>
        /// A <see cref="NodeInput"/> object representing the input node of the XLinkOut instance.
        /// </returns>
        public NodeInput GetInput()
        {
            return new NodeInput(DaiInvoke.daiXLinkOutGetInput(_ptr), false);
        }

        /// <summary>
        /// Release all unmanaged memory associated with the XLinkOut.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DaiInvoke.daiXLinkOutRelease(ref _sharedPtr);
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
        internal static extern void daiXLinkOutRelease(ref IntPtr xLinkOutSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiXLinkOutGetInput(IntPtr xlinkOut);
        /*
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiXLinkOutSetStreamName(IntPtr xlinkOut, IntPtr streamName);*/
    }
}