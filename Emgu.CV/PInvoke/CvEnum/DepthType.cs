//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// OpenCV depth type
    /// </summary>
    public enum DepthType
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = -1,
        /// <summary>
        /// Byte
        /// </summary>
        Cv8U = 0,
        /// <summary>
        /// SByte
        /// </summary>
        Cv8S = 1,
        /// <summary>
        /// UInt16
        /// </summary>
        Cv16U = 2,
        /// <summary>
        /// Int16
        /// </summary>
        Cv16S = 3,
        /// <summary>
        /// Int32
        /// </summary>
        Cv32S = 4,
        /// <summary>
        /// float
        /// </summary>
        Cv32F = 5,
        /// <summary>
        /// double
        /// </summary>
        Cv64F = 6,
        /// <summary>
        /// Represents a 16-bit floating-point depth type in OpenCV.
        /// </summary>
        Cv16F = 7,
        /// <summary>
        /// Represents a 16-bit floating-point depth type with a brain floating-point format (BF16).
        /// </summary>
        Cv16BF = 8,
        /// <summary>
        /// Represents a boolean data type in OpenCV.
        /// </summary>
        CvBool = 9,
        /// <summary>
        /// Represents a 64-bit unsigned integer depth type in OpenCV.
        /// </summary>
        Cv64U = 10,
        /// <summary>
        /// Represents a 64-bit signed integer depth type in OpenCV.
        /// </summary>
        Cv64S = 11,
        /// <summary>
        /// Represents a 32-bit unsigned integer depth type in OpenCV.
        /// </summary>
        Cv32U = 12,
        /// <summary>
        /// Represents an 8-bit floating-point depth type, OCP/ONNX FLOAT8E4M3FN (1 sign, 4 exponent, 3 mantissa bits, bias 7, no infinity, max 448). Available since OpenCV 5.1.
        /// </summary>
        Cv8FE4M3FN = 13,
        /// <summary>
        /// Represents an 8-bit floating-point depth type, ONNX FLOAT8E4M3FNUZ (1-4-3, bias 8, no infinity, single NaN, no negative zero). Available since OpenCV 5.1.
        /// </summary>
        Cv8FE4M3FNUZ = 14
    }
}
