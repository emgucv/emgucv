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
        public Device(String usbDevice, bool usb2Mode=false)
        {
            using (CvString csUsbDevice = new CvString(usbDevice))
                _ptr = DepthAIInvoke.depthaiDeviceCreate(csUsbDevice, usb2Mode);
        }

        public String[] GetAvailableStreams()
        {
            using (VectorOfCvString vs = new VectorOfCvString())
            {
                DepthAIInvoke.depthaiDeviceGetAvailableStreams(_ptr, vs);
                return vs.ToArray();
            }
        }

        public CNNHostPipeline CreatePipeline(string jsonConfigStr)
        {
            using (CvString csJsonConfigStr = new CvString(jsonConfigStr))
            {
                IntPtr sharedPtr = IntPtr.Zero;

                IntPtr ptr = DepthAIInvoke.depthaiDeviceCreatePipeline(_ptr, csJsonConfigStr, ref sharedPtr);
                return new CNNHostPipeline(sharedPtr, ptr);
            }
        }

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DepthAIInvoke.depthaiDeviceRelease(ref _ptr);
            }
        }
    }

}