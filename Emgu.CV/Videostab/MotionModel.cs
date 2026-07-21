//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.VideoStab
{
    /// <summary>
    /// Describes the motion model used by video stabilization estimators.
    /// </summary>
    public enum MotionModel
    {
        /// <summary>Pure translation (2 DOF)</summary>
        Translation = 0,
        /// <summary>Translation and uniform scale (3 DOF)</summary>
        TranslationAndScale = 1,
        /// <summary>Rotation only (1 DOF)</summary>
        Rotation = 2,
        /// <summary>Rotation and translation (3 DOF)</summary>
        Rigid = 3,
        /// <summary>Rotation, translation, and uniform scale (4 DOF)</summary>
        Similarity = 4,
        /// <summary>Full affine (6 DOF)</summary>
        Affine = 5,
        /// <summary>Perspective homography (8 DOF)</summary>
        Homography = 6,
        /// <summary>Unknown motion model</summary>
        Unknown = 7
    }
}
