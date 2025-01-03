//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
        /// Initializes a new instance of the <see cref="Device"/> class using the specified DepthAI pipeline.
        /// </summary>
        /// <param name="pipeline">
        /// The <see cref="Pipeline"/> object that defines the configuration and operations for the DepthAI device.
        /// </param>
        public Device(Pipeline pipeline)
        {
            _ptr = DaiInvoke.daiDeviceCreate(pipeline);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        /// <param name="pipeline">The pipeline to be used by the device.</param>
        /// <param name="usb2Mode">If set to <c>true</c>, the device will operate in USB 2.0 mode.</param>
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

        /// <summary>
        /// Retrieves the DataOutputQueue associated with the specified name.
        /// </summary>
        /// <param name="name">The name of the DataOutputQueue to retrieve.</param>
        /// <returns>A DataOutputQueue object associated with the specified name.</returns>
        public DataOutputQueue GetDataOutputQueue(String name)
        {
            using (CvString csName = new CvString(name))
            {
                IntPtr sharedPtr = IntPtr.Zero;
                IntPtr ptr = DaiInvoke.daiDeviceGetOutputQueue(_ptr, csName, ref sharedPtr);
                return new DataOutputQueue(ptr, sharedPtr);
            }
        }

        /// <summary>
        /// Gets the names of the input queues associated with the device.
        /// </summary>
        /// <value>
        /// An array of strings, where each string is the name of an input queue.
        /// </value>
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

        /// <summary>
        /// Gets the names of the output queues available in the device.
        /// </summary>
        /// <value>
        /// An array of strings, where each string is the name of an output queue.
        /// </value>
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