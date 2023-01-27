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
    /// DepthAI device
    /// </summary>
    public partial class Device : UnmanagedObject
    {
        /// <summary>
        /// Create a new device
        /// </summary>
        public Device(Pipeline pipeline)
        {
            _ptr = DaiInvoke.daiDeviceCreate(pipeline);
        }

        
        public Device(Pipeline pipeline, bool usb2Mode)
        {
            _ptr = DaiInvoke.daiDeviceCreate2(pipeline, usb2Mode);
        }

        /*
        /// <summary>
        /// Get the available streams
        /// </summary>
        public String[] AvailableStreams
        {
            get
            {
                using (VectorOfCvString vs = new VectorOfCvString())
                {
                    DepthAIInvoke.depthaiDeviceGetAvailableStreams(_ptr, vs);
                    return vs.ToArray();
                }
            }
        }

        /// <summary>
        /// Create a pipeline
        /// </summary>
        /// <param name="jsonConfigStr">The Json configuration string</param>
        /// <returns>The CNNHostPipeline</returns>
        public CNNHostPipeline CreatePipeline(string jsonConfigStr)
        {
            using (CvString csJsonConfigStr = new CvString(jsonConfigStr))
            {
                IntPtr sharedPtr = IntPtr.Zero;

                IntPtr ptr = DepthAIInvoke.depthaiDeviceCreatePipeline(_ptr, csJsonConfigStr, ref sharedPtr);
                return new CNNHostPipeline(sharedPtr, ptr);
            }
        }*/

        public DataOutputQueue GetDataOutputQueue(String name)
        {
            using (CvString csName = new CvString(name))
            {
                IntPtr sharedPtr = IntPtr.Zero;
                IntPtr ptr = DaiInvoke.daiDeviceGetOutputQueue(_ptr, csName, ref sharedPtr);
                return new DataOutputQueue(ptr, sharedPtr);
            }
        }

        public String[] InputQueueNames
        {
            get
            {
                using (VectorOfCvString inputQueueNames = new VectorOfCvString())
                {
                    DaiInvoke.daiDeviceGetInputQueueNames(_ptr, inputQueueNames);
                    return inputQueueNames.ToArray();
                }
            }
        }

        public String[] OutputQueueNames
        {
            get
            {
                using (VectorOfCvString outputQueueNames = new VectorOfCvString())
                {
                    DaiInvoke.daiDeviceGetOutputQueueNames(_ptr, outputQueueNames);
                    return outputQueueNames.ToArray();
                }
            }
        }

        /// <summary>
        /// Release all unmanaged memory associated with the Device.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DaiInvoke.daiDeviceRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Entry points for the DepthAI module.
    /// </summary>
    public static partial class DaiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiDeviceCreate(IntPtr pipeline);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiDeviceCreate2(
            IntPtr pipeline,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool usb2Mode);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiDeviceRelease(ref IntPtr usbDevice);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr daiDeviceGetOutputQueue(IntPtr device, IntPtr name, ref IntPtr outputQueueSharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiDeviceGetInputQueueNames(IntPtr device, IntPtr names);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void daiDeviceGetOutputQueueNames(IntPtr device, IntPtr names);
    }
}