//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Dai
{
    /// <summary>
    /// Specifies the orientation of the camera image.
    /// </summary>
    /// <remarks>
    /// This enum is used to control the orientation of the image produced by the camera.
    /// </remarks>
    public enum CameraImageOrientation
    {
        /// <summary>
        /// Represents the automatic orientation of the camera image.
        /// </summary>
        /// <remarks>
        /// When set to Auto, the orientation of the image is determined automatically by the system.
        /// </remarks>
        Auto = -1,
        /// <summary>
        /// The image is in its normal orientation, without any rotation or flipping.
        /// </summary>
        Normal,
        /// <summary>
        /// Represents the horizontal mirror orientation of the camera image.
        /// </summary>
        /// <remarks>
        /// When this orientation is set, the camera image is mirrored along the horizontal axis.
        /// </remarks>
        HorizontalMirror,
        /// <summary>
        /// Represents the vertical flip orientation of the camera image.
        /// </summary>
        /// <remarks>
        /// When this orientation is set, the output image from the camera will be flipped vertically.
        /// </remarks>
        VerticalFlip,
        /// <summary>
        /// Represents a 180 degree rotation of the camera image.
        /// </summary>
        /// <remarks>
        /// When this value is used, the camera image is rotated by 180 degrees.
        /// </remarks>
        Rotate180Deg
    }
}