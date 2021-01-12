//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// HandEyeCalibration Method
    /// </summary>
    public enum HandEyeCalibrationMethod
    {
        /// <summary>
        /// A New Technique for Fully Autonomous and Efficient 3D Robotics Hand/Eye Calibration
        /// </summary>
        Tsai = 0,
        /// <summary>
        /// Robot Sensor Calibration: Solving AX = XB on the Euclidean Group
        /// </summary>
        Park = 1,
        /// <summary>
        /// Hand-eye Calibration
        /// </summary>
        Horaud = 2,
        /// <summary>
        /// On-line Hand-Eye Calibration
        /// </summary>
        Andreff = 3,
        /// <summary>
        /// Hand-Eye Calibration Using Dual Quaternions
        /// </summary>
        Daniilidis = 4
    }
}