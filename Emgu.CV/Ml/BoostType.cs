//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.MlEnum
{
    /// <summary>
    /// Boosting type
    /// </summary>
    public enum BoostType
    {
        /// <summary>
        /// Discrete AdaBoost
        /// </summary>
        Discrete = 0,
        /// <summary>
        /// Real AdaBoost
        /// </summary>
        Real = 1,
        /// <summary>
        /// LogitBoost
        /// </summary>
        Logit = 2,
        /// <summary>
        /// Gentle AdaBoost
        /// </summary>
        Gentle = 3
    }
}
