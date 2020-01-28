//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.ML.MlEnum
{
    /// <summary>
    /// Splitting criteria, used to choose optimal splits during a weak tree construction
    /// </summary>
    public enum BoostSplitCreiteria
    {
        /// <summary>
        /// Use the default criteria for the particular boosting method
        /// </summary>
        Default = 0,
        /// <summary>
        /// Use Gini index. This is default option for Real AdaBoost; may be also used for Discrete AdaBoost
        /// </summary>
        Gini = 1,
        /// <summary>
        /// Use misclassification rate. This is default option for Discrete AdaBoost; may be also used for Real AdaBoost
        /// </summary>
        Misclass = 3,
        /// <summary>
        /// Use least squares criteria. This is default and the only option for LogitBoost and Gentle AdaBoost
        /// </summary>
        Sqerr = 4
    }

}
