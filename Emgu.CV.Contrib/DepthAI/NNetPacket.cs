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


        public struct Detection
        {
            UInt32 Label;
            float Confidence;
            float XMin;
            float YMin;
            float XMax;
            float YMax;
            float DepthX;
            float DepthY;
            float DepthZ;
        };
    }
}