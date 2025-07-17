//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Shape of the Structuring Element
    /// </summary>
    public enum MorphShapes
    {
        /// <summary>
        /// A rectangular element.
        /// </summary>
        Rectangle = 0,
        /// <summary>
        /// A cross-shaped element.
        /// </summary>
        Cross = 1,
        /// <summary>
        /// An elliptic element.
        /// </summary>
        Ellipse = 2,
        /// <summary>
        /// Represents a diamond-shaped structuring element used in morphological operations.
        /// </summary>
        Diamond = 3,
        /// <summary>
        /// A user-defined element.
        /// </summary>
        Custom = 100
    }
}
