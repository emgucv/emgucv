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
        public enum Version
        {
            VERSION_2020_3, 
            VERSION_2020_4, 
            VERSION_2021_1, 
            VERSION_2021_2, 
            VERSION_2021_3, 
            VERSION_2021_4
        };
    }

}