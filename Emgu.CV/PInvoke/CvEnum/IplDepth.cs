//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// IPL_DEPTH
    /// </summary>
    public enum IplDepth : uint
    {
        /// <summary>
        /// Indicates if the value is signed
        /// </summary>
        IplDepthSign = 0x80000000,
        /// <summary>
        /// 1bit unsigned
        /// </summary>
        IplDepth_1U = 1,
        /// <summary>
        /// 8bit unsigned (Byte)
        /// </summary>
        IplDepth_8U = 8,
        /// <summary>
        /// 16bit unsigned
        /// </summary>
        IplDepth16U = 16,
        /// <summary>
        /// 32bit float (Single)
        /// </summary>
        IplDepth32F = 32,
        /// <summary>
        /// 8bit signed
        /// </summary>
        IplDepth_8S = (IplDepthSign | 8),
        /// <summary>
        /// 16bit signed
        /// </summary>
        IplDepth16S = (IplDepthSign | 16),
        /// <summary>
        /// 32bit signed 
        /// </summary>
        IplDepth32S = (IplDepthSign | 32),
        /// <summary>
        /// double
        /// </summary>
        IplDepth64F = 64
    }
}
