//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.DepthAI
{
    /// <summary>
    /// The CNNHostPipeline
    /// </summary>
    public partial class CNNHostPipeline : SharedPtrObject
    {
        internal CNNHostPipeline(IntPtr sharedPtr, IntPtr ptr)
        {
            _sharedPtr = sharedPtr;
            _ptr = ptr;
        }

        /// <summary>
        /// Get the available NNet and data packets
        /// </summary>
        /// <param name="blocking">If true, this will be a blocking function call</param>
        /// <returns>The available NNet and data packets</returns>
        public NNetAndDataPackets GetAvailableNNetAndDataPackets(bool blocking)
        {
            return new NNetAndDataPackets(
                DepthAIInvoke.depthaiCNNHostPipelineGetAvailableNNetAndDataPackets(_ptr, blocking));
        }

        /// <summary>
        /// Release all unmanaged memory associated with the CNNHostPipeline.
        /// </summary>
        protected override void DisposeObject()
        {
            
            if (_sharedPtr != IntPtr.Zero)
            {
                DepthAIInvoke.depthaiCNNHostPipelineRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }
}