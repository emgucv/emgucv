//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Types of thresholding 
    /// </summary>
    public enum ThresholdType
    {
        ///<summary>
        ///value = value &gt; threshold ? max_value : 0
        ///</summary>
        Binary = 0,
        ///<summary>
        /// value = value &gt; threshold ? 0 : max_value       
        ///</summary>
        BinaryInv = 1,
        ///<summary>
        /// value = value &gt; threshold ? threshold : value   
        ///</summary>
        Trunc = 2,
        ///<summary>
        /// value = value &gt; threshold ? value : 0           
        ///</summary>
        ToZero = 3,
        ///<summary>
        /// value = value &gt; threshold ? 0 : value           
        ///</summary>
        ToZeroInv = 4,
        /// <summary>
        /// Mask
        /// </summary>
        Mask = 7,
        ///<summary>
        /// use Otsu algorithm to choose the optimal threshold value;
        /// combine the flag with one of the above CV_THRESH_* values 
        ///</summary>
        Otsu = 8,
        /// <summary>
        /// Use Triangle algorithm to choose the optimal threshold value
        /// </summary>
        Triangle = 16
    }
}
