//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Sequence constants
    /// </summary>
    internal static class SeqConst
    {
        /// <summary>
        /// The bit to shift for SEQ_ELTYPE
        /// </summary>
        public const int EltypeBits = 12;

        /// <summary>
        /// The mask of CV_SEQ_ELTYPE
        /// </summary>
        public const int EltypeMask = ((1 << EltypeBits) - 1);

        /// <summary>
        /// The bits to shift for SEQ_KIND
        /// </summary>
        public const int KindBits = 2;
        /// <summary>
        /// The bits to shift for SEQ_FLAG
        /// </summary>
        public const int Shift = KindBits + EltypeBits;
    }

}
