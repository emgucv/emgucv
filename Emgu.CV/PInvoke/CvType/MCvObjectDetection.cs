//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Emgu.CV.Structure
{
    /// <summary>
    /// Structure contains the bounding box and confidence level for detected object
    /// </summary>
#if !(NETFX_CORE || NETSTANDARD1_4)
    [Serializable]
#endif
    [StructLayout(LayoutKind.Sequential)]
    public struct MCvObjectDetection
    {
        /// <summary>
        /// Bounding box for a detected object
        /// </summary>
        public Rectangle Rect;

        /// <summary>
        /// Confidence level 
        /// </summary>
        public float Score;

        /// <summary>
        /// The class identifier
        /// </summary>
        public int ClassId;
    }
}
