//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// The enum specified format of result that you get from GapiInvoke.Stereo
    /// </summary>
    public enum StereoOutputFormat
    {
        /// <summary>
        /// Floating point 16 bit value, CV_16FC1.
        /// </summary>
        /// <remarks>This identifier is deprecated, use DEPTH_16F instead.</remarks>
        DepthFloat16,
        /// <summary>
        /// Floating point 32 bit value, CV_32FC1.
        /// </summary>
        /// <remarks>This identifier is deprecated, use DEPTH_16F instead.</remarks>
        DepthFloat32,
        /// <summary>
        /// 16 bit signed: first bit for sign, 10 bits for integer part, 5 bits for fractional part.
        /// </summary>
        /// <remarks>This identifier is deprecated, use DISPARITY_16Q_10_5 instead.</remarks>
        DisparityFixed_16_11_5,
        /// <summary>
        /// 16 bit signed: first bit for sign, 11 bits for integer part, 4 bits for fractional part.
        /// </summary>
        /// <remarks>This identifier is deprecated, use DISPARITY_16Q_11_4 instead.</remarks>
        DisparityFixed16_12_4,
        /// <summary>
        /// Same as DepthFloat16
        /// </summary>
        Depth16F = DepthFloat16,
        /// <summary>
        /// Same as DepthFloat32
        /// </summary>
        Depth32F = DepthFloat32,
        /// <summary>
        /// Same as DisparityFixed_16_11_5
        /// </summary>
        Disparity_16Q_10_5 = DisparityFixed_16_11_5,
        /// <summary>
        /// Same as DisparityFixed16_12_4
        /// </summary>
        Disparity_16Q_11_4 = DisparityFixed16_12_4  
    }
}
