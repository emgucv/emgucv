//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// OpenCV depth type
    /// </summary>
    public enum DepthType
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = -1,
        /// <summary>
        /// Byte
        /// </summary>
        Cv8U = 0,
        /// <summary>
        /// SByte
        /// </summary>
        Cv8S = 1,
        /// <summary>
        /// UInt16
        /// </summary>
        Cv16U = 2,
        /// <summary>
        /// Int16
        /// </summary>
        Cv16S = 3,
        /// <summary>
        /// Int32
        /// </summary>
        Cv32S = 4,
        /// <summary>
        /// float
        /// </summary>
        Cv32F = 5,
        /// <summary>
        /// double
        /// </summary>
        Cv64F = 6
    }
}
