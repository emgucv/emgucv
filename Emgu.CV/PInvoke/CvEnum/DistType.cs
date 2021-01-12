//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Defines for Distance Transform
    /// </summary>
    public enum DistType
    {
        ///<summary>
        ///  User defined distance 
        ///</summary>
        User = -1,

        ///<summary>
        ///  distance = |x1-x2| + |y1-y2| 
        ///</summary>
        L1 = 1,
        ///<summary>
        ///  Simple euclidean distance 
        ///</summary>
        L2 = 2,
        ///<summary>
        ///  distance = max(|x1-x2|,|y1-y2|) 
        ///</summary>
        C = 3,
        ///<summary>
        ///  L1-L2 metric: distance = 2(sqrt(1+x*x/2) - 1)) 
        ///</summary>
        L12 = 4,
        ///<summary>
        ///  distance = c^2(|x|/c-log(1+|x|/c)), c = 1.3998 
        ///</summary>
        Fair = 5,
        ///<summary>
        ///  distance = c^2/2(1-exp(-(x/c)^2)), c = 2.9846 
        ///</summary>
        Welsch = 6,
        ///<summary>
        ///  distance = |x|&lt;c ? x^2/2 : c(|x|-c/2), c=1.345 
        ///</summary>
        Huber = 7,
    }
}
