//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Video Acceleration Type
    /// </summary>
    public enum VideoAccelerationType
    {
        /// <summary>
        /// Do not require any specific H/W acceleration, prefer software processing.
        /// </summary>
        /// <remarks>Reading of this value means that special H/W accelerated handling is not added or not detected by OpenCV.</remarks>
        None = 0,

        /// <summary>
        /// Prefer to use H/W acceleration. If no one supported, then fallback to software processing.
        /// </summary>
        /// <remarks>H/W acceleration may require special configuration of used environment. Results in encoding scenario may differ between software and hardware accelerated encoders.</remarks>
        Any = 1,

        /// <summary>
        /// DirectX 11
        /// </summary>
        D3D11 = 2, 

        /// <summary>
        /// VAAPI
        /// </summary>
        VaAPI = 3,  

        /// <summary>
        /// libmfx (Intel MediaSDK/oneVPL)
        /// </summary>
        Mfx = 4,  
    }

}
