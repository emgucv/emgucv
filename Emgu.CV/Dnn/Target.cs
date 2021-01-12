//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// Target devices for computations.
    /// </summary>
    public enum Target
    {
        /// <summary>
        /// CPU
        /// </summary>
        Cpu,
        /// <summary>
        /// OpenCL
        /// </summary>
        OpenCL,
        /// <summary>
        /// Will fall back to OPENCL if the hardware does not support FP16
        /// </summary>
        OpenCLFp16,
        /// <summary>
        /// Myraid
        /// </summary>
        Myriad,
		/// <summary>
        /// Vulkan
        /// </summary>
		Vulkan,
		/// <summary>
        /// FPGA device with CPU fallbacks using Inference Engine's Heterogeneous plugin.
        /// </summary>
        FPGA,
		/// <summary>
        /// Cuda
        /// </summary>
        Cuda,
		/// <summary>
        /// Cuda fp16
        /// </summary>
        CudaFp16, 
        /// <summary>
        /// HDDL
        /// </summary>
        HDDL
    }
}
