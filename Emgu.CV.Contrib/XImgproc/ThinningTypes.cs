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
    /// Thinning type
    /// </summary>
    public enum ThinningTypes
    {
        /// <summary>
        /// Thinning technique of Zhang-Suen
        /// </summary>
        ZhangSuen = 0,
        /// <summary>
        /// Thinning technique of Guo-Hall
        /// </summary>
        GuoHall = 1  
    }
}
