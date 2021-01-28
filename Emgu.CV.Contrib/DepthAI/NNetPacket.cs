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

    public class NNetPacket
    {
        private IntPtr _ptr;

        internal NNetPacket(IntPtr ptr)
        {
            _ptr = ptr;
        }

        public Detection[] Detections
        {
            get
            {
                int size = DepthAIInvoke.depthaiNNetPacketGetDetectedObjectsCount(_ptr);
                Detection[] detections = new Detection[size];
                GCHandle detectionsHandler = GCHandle.Alloc(detections, GCHandleType.Pinned);
                DepthAIInvoke.depthaiNNetPacketGetDetectedObjects(_ptr, detectionsHandler.AddrOfPinnedObject());
                detectionsHandler.Free();
                return detections;
            }
        }

        public FrameMetadata GetFrameMetadata()
        {
            FrameMetadata metaData = new FrameMetadata();
            bool success = DepthAIInvoke.depthaiNNetPacketGetMetadata(_ptr, metaData.Ptr);
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

        public struct Detection
        {
            public UInt32 Label;
            public float Confidence;
            public float XMin;
            public float YMin;
            public float XMax;
            public float YMax;
            public float DepthX;
            public float DepthY;
            public float DepthZ;
        };
    }
}