//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// Dnn backend. 
    /// </summary>
    public enum Backend
    {
        /// <summary>
        /// Default equals to InferenceEngine if
        /// OpenCV is built with Intel's Inference Engine library or
        /// Opencv otherwise.
        /// </summary>
        Default,
        /// <summary>
        /// Halide backend
        /// </summary>
        Halide,
        /// <summary>
        /// Intel's Inference Engine library
        /// </summary>
        InferenceEngine, 
        /// <summary>
        /// OpenCV's implementation
        /// </summary>
        OpenCV,
		/// <summary>
        /// Vulkan based backend
        /// </summary>
		VkCom,
		/// <summary>
        /// Cuda backend
        /// </summary>
		Cuda
    }
}
