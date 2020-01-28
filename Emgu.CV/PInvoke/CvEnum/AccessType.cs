//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Access type
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// Read
        /// </summary>
        Read = 1 << 24,
        /// <summary>
        /// Write
        /// </summary>
        Write = 1 << 25,
        /// <summary>
        /// Read and write
        /// </summary>
        ReadWrite = 3 << 24,
        /// <summary>
        /// Mask
        /// </summary>
        Mask = ReadWrite,
        /// <summary>
        /// Fast
        /// </summary>
        Fast = 1 << 26
    }

}
