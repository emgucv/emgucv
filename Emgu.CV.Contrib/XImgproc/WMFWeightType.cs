//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.XImgproc
{
    /// <summary>
    /// Weight type
    /// </summary>
    public enum WMFWeightType
    {
        /// <summary>
        /// exp(-|I1-I2|^2/(2*sigma^2))
        /// </summary>
        Exp,
        /// <summary>
        /// (|I1-I2|+sigma)^-1
        /// </summary>
        Iv1,
        /// <summary>
        /// (|I1-I2|^2+sigma^2)^-1
        /// </summary>
        Iv2,
        /// <summary>
        /// dot(I1,I2)/(|I1|*|I2|)
        /// </summary>
        Cos,
        /// <summary>
        /// (min(r1,r2)+min(g1,g2)+min(b1,b2))/(max(r1,r2)+max(g1,g2)+max(b1,b2))
        /// </summary>
        Jac,
        /// <summary>
        /// unweighted
        /// </summary>
        Off
    }
}
