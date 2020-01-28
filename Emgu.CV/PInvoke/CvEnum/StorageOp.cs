//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The file storage operation type
    /// </summary>
    public enum StorageOp
    {
        /// <summary>
        /// The storage is open for reading
        /// </summary>
        Read = 0,
        /// <summary>
        /// The storage is open for writing
        /// </summary>
        Write = 1,
        /// <summary>
        /// The storage is open for append
        /// </summary>
        Append = 2
    }
}
