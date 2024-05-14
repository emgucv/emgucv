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
    /// Specifies the camera board socket options for the DepthAI module.
    /// </summary>
    /// <remarks>
    /// The DepthAI module uses this enum to determine which camera to use for image processing tasks.
    /// </remarks>
    public enum CameraBoardSocket
    {
        /// <summary>
        /// Represents an automatic selection of the camera board socket.
        /// </summary>
        /// <remarks>
        /// When this option is used, the system will automatically choose the most appropriate camera board socket.
        /// </remarks>
        Auto = -1,
        
        /// <summary>
        /// Represents the RGB camera board socket for the DepthAI module.
        /// </summary>
        /// <remarks>
        /// Use this value to specify the RGB camera for image processing tasks.
        /// </remarks>
        Rgb,

        /// <summary>
        /// Represents the left camera board socket on the DepthAI module.
        /// </summary>
        /// <remarks>
        /// Use this value to specify the left camera for image processing tasks.
        /// </remarks>
        Left,
        
        /// <summary>
        /// Represents the right camera board socket on the DepthAI module.
        /// </summary>
        /// <remarks>
        /// Use this value to specify the right camera for image processing tasks.
        /// </remarks>
        Right
    }

    
}