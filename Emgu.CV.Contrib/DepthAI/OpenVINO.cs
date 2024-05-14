//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
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
    /// DepthAI OpenVINO
    /// </summary>
    public partial class OpenVino
    {
        /// <summary>
        /// Specifies the version of OpenVINO used in the DepthAI pipeline.
        /// </summary>
        /// <remarks>
        /// Each version corresponds to a specific release of OpenVINO.
        /// </remarks>
        public enum Version
        {
            /// <summary>
            /// Represents OpenVINO version 2020.3.
            /// </summary>
            Version2020_3,
            /// <summary>
            /// Represents OpenVINO version 2020.4.
            /// </summary>
            Version2020_4,
            /// <summary>
            /// Represents OpenVINO version 2021.1.
            /// </summary>
            Version2021_1,
            /// <summary>
            /// Represents OpenVINO version 2021.2.
            /// </summary>
            Version2021_2,
            /// <summary>
            /// Represents OpenVINO version 2021.3.
            /// </summary>
            Version2021_3,
            /// <summary>
            /// Represents OpenVINO version 2021.4.
            /// </summary>
            Version2021_4
        }
    }

}