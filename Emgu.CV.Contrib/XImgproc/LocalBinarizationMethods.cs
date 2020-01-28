//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// LocalBinarizationMethods type
    /// </summary>
    public enum LocalBinarizationMethods
    {
        /// <summary>
        /// Classic Niblack binarization.
        /// </summary>
        Niblack = 0,
        /// <summary>
        /// Sauvola's technique.
        /// </summary>
        Sauvola = 1,
        /// <summary>
        /// Wolf's technique.
        /// </summary>
        Wolf = 2,
        /// <summary>
        /// NICK's technique.
        /// </summary>
        NICK = 3  
    }
}
