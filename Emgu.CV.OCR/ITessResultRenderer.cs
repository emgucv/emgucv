//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    /// Interface to the TesseractResultRender
    /// </summary>
    public interface ITessResultRenderer
    {
        /// <summary>
        /// Pointer to the unmanaged TessResultRendered
        /// </summary>
        IntPtr TessResultRendererPtr { get; }
    }
}
