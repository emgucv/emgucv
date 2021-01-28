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

        public FrameMetadata GetFrameMetadata()
        {
            FrameMetadata metaData = new FrameMetadata();
            bool success = DepthAIInvoke.depthaiHostDataPacketGetMetadata(_ptr, metaData.Ptr);
            if (success)
            {
                return metaData;
            }
            else
            {
                metaData.Dispose();
                return null;
            }
        }

        public bool GetPreviewOut(Mat preview)
        {
            if (StreamName.Contains("previewout"))
            {
                int[] dim = Dimension;
                if (dim[0] == 3)
                {
                    int step = dim[2] * ElemSize;
                    using (Mat blue = new Mat(new Size(dim[2], dim[1]), DepthType.Cv8U, 1, Data, step))
                    using (Mat green = new Mat(new Size(dim[2], dim[1]), DepthType.Cv8U, 1, new IntPtr(Data.ToInt64() + dim[1] * step), step))
                    using (Mat red = new Mat(new Size(dim[2], dim[1]), DepthType.Cv8U, 1, new IntPtr(Data.ToInt64() + dim[1] * step * 2), step))
                    using (VectorOfMat vm = new VectorOfMat(blue, green, red))
                    {
                        CvInvoke.Merge(vm, preview);
                        return true;
                    }
                }
            }

            return false;
        }

        internal HostDataPacket(IntPtr ptr)
        {
            _ptr = ptr;
        }

    }
}