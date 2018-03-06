//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// Result of cvHaarDetectObjects
    /// </summary>
#if !(NETFX_CORE || NETSTANDARD1_4)
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvAvgComp
    {
        /// <summary>
        /// Bounding rectangle for the object (average rectangle of a group)
        /// </summary>
        public Rectangle Rect;

        /// <summary>
        /// Number of neighbor rectangles in the group
        /// </summary>
        public int Neighbors;
    }
}
