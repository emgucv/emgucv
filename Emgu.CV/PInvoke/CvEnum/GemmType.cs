//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Flags used for GEMM function
    /// </summary>
    [Flags]
    public enum GemmType
    {
        /// <summary>
        /// Do not apply transpose to neither matrices
        /// </summary>
        Default = 0,
        /// <summary>
        /// transpose src1
        /// </summary>
        Src1Transpose = 1,
        /// <summary>
        /// transpose src2
        /// </summary>
        Src2Transpose = 2,
        /// <summary>
        /// transpose src3
        /// </summary>
        Src3Transpose = 4
    }
}
