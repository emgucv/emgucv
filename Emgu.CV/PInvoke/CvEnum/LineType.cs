//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The type of line for drawing
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// Filled
        /// </summary>
        Filled = -1,
        /// <summary>
        /// 8-connected
        /// </summary>
        EightConnected = 8,
        /// <summary>
        /// 4-connected
        /// </summary>
        FourConnected = 4,
        /// <summary>
        /// Anti-alias
        /// </summary>
        AntiAlias = 16
    }
}
