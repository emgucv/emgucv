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
    /// When Tesseract/LSTM is initialized we can choose to instantiate/load/run
    /// only the Tesseract part, only the Cube part or both along with the combiner.
    /// The preference of which engine to use is stored in tessedit_ocr_engine_mode.
    /// </summary>
    public enum OcrEngineMode
    {
        /// <summary>
        /// Run Tesseract only - fastest
        /// </summary>
        TesseractOnly,
        /// <summary>
        /// Run just the LSTM line recognizer.
        /// </summary>
        LstmOnly,
        /// <summary>
        /// Run the LSTM recognizer, but allow fallback to Tesseract when things get difficult.
        /// </summary>
        TesseractLstmCombined,
        /// <summary>
        /// Specify this mode when calling init_*(),
        /// to indicate that any of the above modes
        /// should be automatically inferred from the
        /// variables in the language-specific config,
        /// command-line configs, or if not specified
        /// in any of the above should be set to the
        /// default OEM_TESSERACT_ONLY.
        /// </summary>
        Default
    }
}

