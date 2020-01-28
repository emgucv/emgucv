//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Inpaint type
    /// </summary>
    public enum InpaintType
    {
        /// <summary>
        /// Navier-Stokes based method.
        /// </summary>
        NS = 0,
        /// <summary>
        /// The method by Alexandru Telea 
        /// </summary>
        Telea = 1
    }

}
