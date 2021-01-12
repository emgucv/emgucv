//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Type used for cvCmp function
    /// </summary>
    public enum CmpType
    {
        /// <summary>
        /// src1(I) "equal to" src2(I)
        /// </summary>
        Equal = 0,
        /// <summary>
        /// src1(I) "greater than" src2(I)
        /// </summary>
        GreaterThan = 1,
        /// <summary>
        /// src1(I) "greater or equal" src2(I)
        /// </summary>
        GreaterEqual = 2,
        /// <summary>
        /// src1(I) "less than" src2(I)
        /// </summary>
        LessThan = 3,
        /// <summary>
        /// src1(I) "less or equal" src2(I)
        /// </summary>
        LessEqual = 4,
        /// <summary>
        /// src1(I) "not equal to" src2(I)
        /// </summary>
        NotEqual = 5
    }

}
