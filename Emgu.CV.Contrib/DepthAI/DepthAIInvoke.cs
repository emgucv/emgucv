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
    /// Entry points for the DepthAI module.
    /// </summary>
    public static partial class DepthAIInvoke
    {
        private static readonly bool _haveDepthAI;

        static DepthAIInvoke()
        {
            CvInvoke.Init();
            _haveDepthAI = (CvInvoke.ConfigDict["HAVE_DEPTHAI"] == 1);
        }

        /// <summary>
        /// True if the native binary is built with DepthAI support.
        /// </summary>
        public static Boolean HaveDepthAI
        {
            get
            {
                return _haveDepthAI;
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr depthaiDeviceCreate(
            IntPtr usbDevice,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool usb2Mode);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiDeviceRelease(ref IntPtr usbDevice);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiDeviceGetAvailableStreams(IntPtr usbDevice, IntPtr availableStreams);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr depthaiDeviceCreatePipeline(IntPtr usb_device, IntPtr config_json_str, ref IntPtr hostedPipelineSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiCNNHostPipelineRelease(ref IntPtr hostedPipelineSharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr depthaiCNNHostPipelineGetAvailableNNetAndDataPackets(
            IntPtr cnnHostPipeline,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool blocking);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiNNetAndDataPacketsRelease(ref IntPtr nnetAndDataPackets);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int depthaiNNetAndDataPacketsGetNNetCount(IntPtr nnetAndDataPackets);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiNNetAndDataPacketsGetNNetArr(IntPtr nnetAndDataPackets, IntPtr packetArr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int depthaiNNetAndDataPacketsGetHostDataPacketCount(IntPtr nnetAndDataPackets);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiNNetAndDataPacketsGetHostDataPacketArr(IntPtr nnetAndDataPackets, IntPtr packetArr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiHostDataPacketGetDimensions(IntPtr packet, IntPtr dimensions);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool depthaiHostDataPacketGetMetadata(IntPtr packet, IntPtr metadata);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int depthaiNNetPacketGetDetectedObjectsCount(IntPtr packet);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiNNetPacketGetDetectedObjects(IntPtr packet, IntPtr detections);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool depthaiNNetPacketGetMetadata(IntPtr packet, IntPtr metadata);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr depthaiFrameMetadataCreate();
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void depthaiFrameMetadataRelease(ref IntPtr metadata);
    }
}