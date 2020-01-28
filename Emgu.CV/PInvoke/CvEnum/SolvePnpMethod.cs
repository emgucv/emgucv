//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
        /// EPnP: Efficient Perspective-n-Point Camera Pose Estimation
        /// F.Moreno-Noguer, V.Lepetit and P.Fua "EPnP: Efficient Perspective-n-Point Camera Pose Estimation"
        /// </summary>
        EPnP = 1,
        /// <summary>
        /// Complete Solution Classification for the Perspective-Three-Point Problem
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
        UPnP = 4,
        /// <summary>
        /// An Efficient Algebraic Solution to the Perspective-Three-Point Problem
        /// </summary>
        AP3P = 5,
        /// <summary>
        /// Infinitesimal Plane-Based Pose Estimation. Object points must be coplanar.
        /// </summary>
        IPPE = 6,
        /// <summary>
        /// Infinitesimal Plane-Based Pose Estimation. This is a special case suitable for marker pose estimation.
        ///  4 coplanar object points must be defined in the following order:
        ///   - point 0: [-squareLength / 2,  squareLength / 2, 0]
        ///   - point 1: [ squareLength / 2,  squareLength / 2, 0]
        ///   - point 2: [ squareLength / 2, -squareLength / 2, 0]
        ///   - point 3: [-squareLength / 2, -squareLength / 2, 0]
        /// </summary>
        IPPESquare = 7

    }
}
