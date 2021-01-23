//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.DepthAI
{

    public partial class HostDataPacket
    {
        private IntPtr _ptr;

        public int[] Dimension
        {
            get
            {
                using (VectorOfInt vi = new VectorOfInt())
                {
                    DepthAIInvoke.depthaiHostDataPacketGetDimensions(_ptr, vi);
                    return vi.ToArray();
                }
            }
        }

        internal HostDataPacket(IntPtr ptr)
        {
            _ptr = ptr;
        }

    }
}