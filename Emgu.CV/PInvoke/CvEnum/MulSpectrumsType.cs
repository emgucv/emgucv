//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The types for MulSpectrums
    /// </summary>
    [Flags]
    public enum MulSpectrumsType
    {
        /// <summary>
        /// The default type
        /// </summary>
        Default = 0,
        /// <summary>
        /// Do forward or inverse transform of every individual row of the input matrix. This flag allows user to transform multiple vectors simultaneously and can be used to decrease the overhead (which is sometimes several times larger than the processing itself), to do 3D and higher-dimensional transforms etc
        /// </summary>
        DxtRows = 4,
        /// <summary>
        /// Conjugate the second argument of cvMulSpectrums
        /// </summary>
        DxtMulConj = 8
    }

}
