//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Method for solving a PnP problem
    /// </summary>
    public enum SolvePnpMethod
    {
        /// <summary>
        /// Iterative
        /// </summary>
        Iterative = 0,
        /// <summary>
        /// F.Moreno-Noguer, V.Lepetit and P.Fua "EPnP: Efficient Perspective-n-Point Camera Pose Estimation"
        /// </summary>
        EPnP = 1,
        /// <summary>
        /// X.S. Gao, X.-R. Hou, J. Tang, H.-F. Chang; "Complete Solution Classification for the Perspective-Three-Point Problem"
        /// </summary>
        P3P = 2,
        /// <summary>
        /// A Direct Least-Squares (DLS) Method for PnP
        /// </summary>
        Dls = 3,
        /// <summary>
        /// Exhaustive Linearization for Robust Camera Pose and Focal Length Estimation
        /// </summary>
        UPnP = 4
    }
}
