//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Various camera calibration flags
    /// </summary>
    [Flags]
    public enum CalibType
    {
        /// <summary>
        /// The default value
        /// </summary>
        Default = 0,
        /// <summary>
        /// intrinsic_matrix contains valid initial values of fx, fy, cx, cy that are optimized further. Otherwise, (cx, cy) is initially set to the image center (image_size is used here), and focal distances are computed in some least-squares fashion
        /// </summary>
        UseIntrinsicGuess = 0x00001,
        /// <summary>
        /// The optimization procedure consider only one of fx and fy as independent variable and keeps the aspect ratio fx/fy the same as it was set initially in intrinsic_matrix. In this case the actual initial values of (fx, fy) are either taken from the matrix (when CV_CALIB_USE_INTRINSIC_GUESS is set) or estimated somehow (in the latter case fx, fy may be set to arbitrary values, only their ratio is used)
        /// </summary>
        FixAspectRatio = 0x00002,
        /// <summary>
        /// The principal point is not changed during the global optimization, it stays at the center and at the other location specified (when CV_CALIB_FIX_FOCAL_LENGTH - Both fx and fy are fixed.
        /// CV_CALIB_USE_INTRINSIC_GUESS is set as well)
        /// </summary>
        FixPrincipalPoint = 0x00004,
        /// <summary>
        /// Tangential distortion coefficients are set to zeros and do not change during the optimization
        /// </summary>
        ZeroTangentDist = 0x00008,
        /// <summary>
        /// The focal length is fixed
        /// </summary>
        FixFocalLength = 0x00010,
        /// <summary>
        /// The 1st distortion coefficient (k1) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
        /// </summary>
        FixK1 = 0x00020,
        /// <summary>
        /// The 2nd distortion coefficient (k2) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
        /// </summary>
        FixK2 = 0x00040,
        /// <summary>
        /// The 3rd distortion coefficient (k3) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
        /// </summary>
        FixK3 = 0x00080,
        /// <summary>
        /// The 4th distortion coefficient (k4) is fixed (see above)
        /// </summary>
        FixK4 = 0x00800,
        /// <summary>
        /// The 5th distortion coefficient (k5) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
        /// </summary>
        FixK5 = 0x01000,
        /// <summary>
        /// The 6th distortion coefficient (k6) is fixed to 0 or to the initial passed value if CV_CALIB_USE_INTRINSIC_GUESS is passed
        /// </summary>
        FixK6 = 0x02000,
        /// <summary>
        /// Rational model
        /// </summary>
        RationalModel = 0x04000,
        /// <summary>
        /// Thin prism model
        /// </summary>
        ThinPrismModel = 0x08000,
        /// <summary>
        /// Fix S1, S2, S3, S4
        /// </summary>
        FixS1S2S3S4 = 0x10000,
        /// <summary>
        /// Tilted model
        /// </summary>
        TiltedModel = 0x40000,
        /// <summary>
        /// Fix Taux Tauy
        /// </summary>
        FixTauxTauy = 0x80000,

        /// <summary>
        /// Use QR instead of SVD decomposition for solving. Faster but potentially less precise
        /// </summary>
        UseQR = 0x100000,

        /// <summary>
        /// Only for stereo: Fix intrinsic
        /// </summary>
        FixIntrinsic = 0x00100,
        /// <summary>
        /// Only for stereo: Same focal length
        /// </summary>
        SameFocalLength = 0x00200,


        /// <summary>
        /// For stereo rectification: Zero disparity
        /// </summary>
        ZeroDisparity = 0x00400,
        /// <summary>
        /// For stereo rectification: use LU instead of SVD decomposition for solving. much faster but potentially less precise
        /// </summary>
        UseLU = (1 << 17),

    }
}
