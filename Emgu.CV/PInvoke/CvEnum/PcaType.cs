//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// PCA Type
    /// </summary>
    [Flags]
    public enum PcaType
    {
        /// <summary>
        /// the vectors are stored as rows (i.e. all the components of a certain vector are stored continously)
        /// </summary>
        DataAsRow = 0,
        /// <summary>
        ///  the vectors are stored as columns (i.e. values of a certain vector component are stored continuously)
        /// </summary>
        DataAsCol = 1,
        /// <summary>
        /// use pre-computed average vector
        /// </summary>
        UseAvg = 2
    }

}
