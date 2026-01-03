//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Aruco
{
    /// <summary>
    /// RefineParameters is used by ArucoDetector
    /// </summary>
    public struct RefineParameters
    {
        /// <summary>
        /// Minimum distance between the corners of the rejected candidate and 
        /// the reprojected marker to consider it a correspondence.
        /// </summary>
        public float MinRepDistance { get; set; }

        /// <summary>
        /// Rate of allowed erroneous bits w.r.t. dictionary error correction capability.
        /// Use -1 to ignore error correction.
        /// </summary>
        public float ErrorCorrectionRate { get; set; }

        /// <summary>
        /// Consider the four possible corner orders in rejectedCorners array.
        /// If false, only the provided order is considered.
        /// </summary>
        public bool CheckAllOrders { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefineParameters"/> class with the specified parameters.
        /// </summary>
        /// <param name="minRepDistance">
        /// The minimum distance between the corners of the marker and the projection of the marker in the image.
        /// Default value is 10.0f.
        /// </param>
        /// <param name="errorCorrectionRate">
        /// The rate of error correction applied during marker detection.
        /// Default value is 3.0f.
        /// </param>
        /// <param name="checkAllOrders">
        /// A boolean value indicating whether all possible orders of marker corners should be checked.
        /// Default value is true.
        /// </param>
        public RefineParameters(float minRepDistance=10.0f, float errorCorrectionRate=3.0f, bool checkAllOrders=true)
        {
            MinRepDistance = minRepDistance;
            ErrorCorrectionRate = errorCorrectionRate;
            CheckAllOrders = checkAllOrders;
        }

    }
}