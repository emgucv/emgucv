//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{

    /// <summary>
    /// Flag used for cvDFT
    /// </summary>
    [Flags]
    public enum DxtType
    {
        /// <summary>
        /// Do forward 1D or 2D transform. The result is not scaled
        /// </summary>
        Forward = 0,
        /// <summary>
        /// Do inverse 1D or 2D transform. The result is not scaled. CV_DXT_FORWARD and CV_DXT_INVERSE are mutually exclusive, of course
        /// </summary>
        Inverse = 1,
        /// <summary>
        /// Scale the result: divide it by the number of array elements. Usually, it is combined with CV_DXT_INVERSE, and one may use a shortcut 
        /// </summary>
        Scale = 2,
        /// <summary>
        /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
        /// </summary>
        Rows = 4,
        /// <summary>
        /// Inverse and scale
        /// </summary>
        InvScale = (Scale | Inverse)
    }

}
