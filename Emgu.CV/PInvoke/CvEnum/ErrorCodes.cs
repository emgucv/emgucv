//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    ///<summary>
    /// Error codes
    /// </summary>
    public enum ErrorCodes
    {
        /// <summary>
        /// Ok
        /// </summary>
        StsOk = 0,
        /// <summary>
        /// Back trace
        /// </summary>
        StsBacktrace = -1,
        /// <summary>
        /// Error
        /// </summary>
        StsError = -2,
        /// <summary>
        /// Internal
        /// </summary>
        StsInternal = -3,
        /// <summary>
        /// No memory
        /// </summary>
        StsNoMem = -4,
        /// <summary>
        /// Bad argument
        /// </summary>
        StsBadArg = -5,
        /// <summary>
        /// Bad function
        /// </summary>
        StsBadFunc = -6,
        /// <summary>
        /// No Conv
        /// </summary>
        StsNoConv = -7,
        /// <summary>
        /// Auto trace
        /// </summary>
        StsAutoTrace = -8,
        /// <summary>
        /// Header is Null
        /// </summary>
        HeaderIsNull = -9,
        /// <summary>
        /// Bad image size
        /// </summary>
        BadImageSize = -10,
        /// <summary>
        /// Bad Offset
        /// </summary>
        BadOffset = -11,
        /// <summary>
        /// Bad Data pointer
        /// </summary>
        BadDataPtr = -12,
        /// <summary>
        /// Bad step
        /// </summary>
        Badstep = -13,
        /// <summary>
        /// Bad model or chseq
        /// </summary>
        BadModelOrChseq = -14,
        /// <summary>
        /// Bad number of channels
        /// </summary>
        BadNumChannels = -15,
        /// <summary>
        /// Bad number of channels 1U
        /// </summary>
        BadNumChannel1U = -16,
        /// <summary>
        /// Bad depth
        /// </summary>
        BadDepth = -17,
        /// <summary>
        /// Bad Alpha channel
        /// </summary>
        BadAlphaChannel = -18,
        /// <summary>
        /// Bad Order
        /// </summary>
        BadOrder = -19,
        /// <summary>
        /// Bad origin
        /// </summary>
        BadOrigin = -20,
        /// <summary>
        /// Bad Align
        /// </summary>
        BadAlign = -21,
        /// <summary>
        /// Bad callback
        /// </summary>
        BadCallback = -22,
        /// <summary>
        /// Bad tile size
        /// </summary>
        BadTileSize = -23,
        /// <summary>
        /// Bad COI
        /// </summary>
        BadCoi = -24,
        /// <summary>
        /// Bad ROI size
        /// </summary>
        BadRoiSize = -25,
        /// <summary>
        /// Mask is tiled
        /// </summary>
        MaskIsTiled = -26,
        /// <summary>
        /// Null Pointer
        /// </summary>
        StsNullPtr = -27,
        /// <summary>
        /// Vec length error
        /// </summary>
        StsVecLengthErr = -28,
        /// <summary>
        /// Filter Structure Content Error
        /// </summary>
        StsFilterStructContenterr = -29,
        /// <summary>
        /// Kernel Structure Content Error
        /// </summary>
        StsKernelStructContenterr = -30,
        /// <summary>
        /// Filter Offset Error
        /// </summary>
        StsFilterOffSetErr = -31,
        /// <summary>
        /// Bad Size
        /// </summary>
        StsBadSize = -201,
        /// <summary>
        /// Division by zero
        /// </summary>
        StsDivByZero = -202,
        /// <summary>
        /// Inplace not supported
        /// </summary>
        StsInplaceNotSupported = -203,
        /// <summary>
        /// Object Not Found
        /// </summary>
        StsObjectNotFound = -204,
        /// <summary>
        /// Unmatched formats
        /// </summary>
        StsUnmatchedFormats = -205,
        /// <summary>
        /// Bad flag
        /// </summary>
        StsBadFlag = -206,
        /// <summary>
        /// Bad point
        /// </summary>
        StsBadPoint = -207,
        /// <summary>
        /// Bad mask
        /// </summary>
        StsBadMask = -208,
        /// <summary>
        /// Unmatched sizes
        /// </summary>
        StsUnmatchedSizes = -209,
        /// <summary>
        /// Unsupported format
        /// </summary>
        StsUnsupportedFormat = -210,
        /// <summary>
        /// Out of range
        /// </summary>
        StsOutOfRange = -211,
        /// <summary>
        /// Parse Error
        /// </summary>
        StsParseError = -212,
        /// <summary>
        /// Not Implemented
        /// </summary>
        StsNotImplemented = -213,
        /// <summary>
        /// Bad memory block
        /// </summary>
        StsBadMemBlock = -214
    }

}
