//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
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
    /// DepthAI StereoDepth
    /// </summary>
    public partial class StereoDepth : SharedPtrObject, INode
    {
        private IntPtr _nodePtr;

        internal StereoDepth(IntPtr stereoDepthPtr, IntPtr stereoDepthSharedPtr, IntPtr nodePtr)
        {
            _ptr = stereoDepthPtr;
            _sharedPtr = stereoDepthSharedPtr;
            _nodePtr = nodePtr;
        }

        /// <summary>
        /// Release all unmanaged memory associated with the NeuralNetwork.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                DaiInvoke.daiStereoDepthRelease(ref _sharedPtr);
				_ptr = IntPtr.Zero;
                _nodePtr = IntPtr.Zero;
            }
        }

        public NodeInput GetLeft()
        {
            return new NodeInput(DaiInvoke.daiStereoDepthGetLeft(_ptr), false);
        }

        public NodeInput GetRight()
        {
            return new NodeInput(DaiInvoke.daiStereoDepthGetRight(_ptr), false);
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
        internal static extern void daiStereoDepthRelease(ref IntPtr stereoDepthSharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiStereoDepthGetLeft(IntPtr stereoDepth);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiStereoDepthGetRight(IntPtr stereoDepth);
    }
}