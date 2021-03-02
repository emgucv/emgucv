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
    /// <summary>
    /// The NNet packet
    /// </summary>
    public class NNetPacket
    {
        private IntPtr _ptr;

        internal NNetPacket(IntPtr ptr)
        {
            _ptr = ptr;
        }

        /// <summary>
        /// Get the detections from the NNet packet
        /// </summary>
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

        /// <summary>
        /// Get the frame meta data
        /// </summary>
        /// <returns>The frame meta data</returns>
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

        /// <summary>
        /// The detection result
        /// </summary>
        public struct Detection
        {
            /// <summary>
            /// The label
            /// </summary>
            public UInt32 Label;
            /// <summary>
            /// The confidence
            /// </summary>
            public float Confidence;
            /// <summary>
            /// Min x value
            /// </summary>
            public float XMin;
            /// <summary>
            /// Min y value
            /// </summary>
            public float YMin;
            /// <summary>
            /// Max x value
            /// </summary>
            public float XMax;
            /// <summary>
            /// Max y value
            /// </summary>
            public float YMax;
            /// <summary>
            /// The x value of the 3d location
            /// </summary>
            public float DepthX;
            /// <summary>
            /// The y value of the 3d location
            /// </summary>
            public float DepthY;
            /// <summary>
            /// The z value of the 3d location
            /// </summary>
            public float DepthZ;
        };
    }
}