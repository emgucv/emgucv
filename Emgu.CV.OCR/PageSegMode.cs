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
    /// Tesseract page segmentation mode
    /// </summary>
    public enum PageSegMode
    {
        /// <summary>
        /// PageOrientation and script detection only.
        /// </summary>
        OsdOnly,
        /// <summary>
        /// Automatic page segmentation with orientation and script detection. (OSD)
        /// </summary>
        AutoOsd,
        /// <summary>
        /// Automatic page segmentation, but no OSD, or OCR.
        /// </summary>
        AutoOnly,
        /// <summary>
        /// Fully automatic page segmentation, but no OSD.
        /// </summary>
        Auto,
        /// <summary>
        /// Assume a single column of text of variable sizes.
        /// </summary>
        SingleColumn,
        /// <summary>
        /// Assume a single uniform block of vertically aligned text.
        /// </summary>
        SingleBlockVertText,


        /// <summary>
        /// Assume a single uniform block of text. (Default.)
        /// </summary>
        SingleBlock,
        /// <summary>
        /// Treat the image as a single text line.
        /// </summary>
        SingleLine,
        /// <summary>
        /// Treat the image as a single word.
        /// </summary>
        SingleWord,
        /// <summary>
        /// Treat the image as a single word in a circle.
        /// </summary>
        CircleWord,
        /// <summary>
        /// Treat the image as a single character.
        /// </summary>
        SingleChar,
        /// <summary>
        /// Find as much text as possible in no particular order.
        /// </summary>
        SparseText,
        /// <summary>
        /// Sparse text with orientation and script det.
        /// </summary>
        SparseTextOsd,
        /// <summary>
        /// Treat the image as a single text line, bypassing hacks that are Tesseract-specific.
        /// </summary>
        RawLine,

        /// <summary>
        /// Number of enum entries.
        /// </summary>
        Count
    }
}

