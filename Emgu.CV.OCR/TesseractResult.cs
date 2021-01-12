//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.OCR
{
    /// <summary>
    /// This structure is primary used for PInvoke
    /// </summary>
    public struct TesseractResult
    {
#pragma warning disable 0649
        /// <summary>
        /// The length
        /// </summary>
        public int Length;
        /// <summary>
        /// The cost
        /// </summary>
        public float Cost;
        /// <summary>
        /// The region
        /// </summary>
        public Rectangle Region;
#pragma warning restore 0649
    }

}
