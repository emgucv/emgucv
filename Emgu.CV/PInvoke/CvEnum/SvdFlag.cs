//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type for cvSVD
    /// </summary>
    [Flags]
    public enum SvdFlag
    {
        /// <summary>
        /// The default type
        /// </summary>
        Default = 0,
        /// <summary>
        /// Enables modification of matrix src1 during the operation. It speeds up the processing. 
        /// </summary>
        ModifyA = 1,
        /// <summary>
        /// indicates that only a vector of singular values 'w' is to be processed, while u and vt will be set to empty matrices
        /// </summary>
        NoUV = 2,
        /// <summary>
        /// when the matrix is not square, by default the algorithm produces u and vt matrices of
        /// sufficiently large size for the further A reconstruction; if, however, FULL_UV flag is
        /// specified, u and vt will be full-size square orthogonal matrices.
        /// </summary>
        FullUV = 4
    }

}
