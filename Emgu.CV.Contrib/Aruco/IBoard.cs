//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV.Aruco
{
    /// <summary>
    /// Board of markers
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// Pointer to native IBoard
        /// </summary>
        IntPtr BoardPtr { get; }
    }

}