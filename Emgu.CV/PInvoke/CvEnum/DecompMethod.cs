//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// cvInvert method
    /// </summary>
    public enum DecompMethod
    {
        /// <summary>
        /// Gaussian elimination with optimal pivot element chose
        /// In case of LU method the function returns src1 determinant (src1 must be square). If it is 0, the matrix is not inverted and src2 is filled with zeros.
        /// </summary>
        LU = 0,
        /// <summary>
        /// Singular value decomposition (SVD) method
        /// In case of SVD methods the function returns the inversed condition number of src1 (ratio of the smallest singular value to the largest singular value) and 0 if src1 is all zeros. The SVD methods calculate a pseudo-inverse matrix if src1 is singular
        /// </summary>
        Svd = 1,
        /// <summary>
        /// Eig
        /// </summary>
        Eig = 2,
        /// <summary>
        /// method for a symmetric positively-defined matrix
        /// </summary>
        Cholesky = 3,
        /// <summary>
        /// QR decomposition
        /// </summary>
        QR = 4,
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 16
    }

}
