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
    /// DepthAI device
    /// </summary>
    public partial class Device : UnmanagedObject
    {
        /// <summary>
        /// Create a new device
        /// </summary>
        /// <param name="usbDevice">Usb device</param>
        /// <param name="usb2Mode">If true, will use usb 2 mode</param>
        public Device(String usbDevice, bool usb2Mode=false)
        {
            using (CvString csUsbDevice = new CvString(usbDevice))
                _ptr = DepthAIInvoke.depthaiDeviceCreate(csUsbDevice, usb2Mode);
        }

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
        }

        /// <summary>
        /// Release all unmanaged memory associated with the Device.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DepthAIInvoke.depthaiDeviceRelease(ref _ptr);
            }
        }
    }

}