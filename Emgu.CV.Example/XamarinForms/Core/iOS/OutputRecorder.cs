//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__ || __MACOS__

using System;
using CoreGraphics;
using Xamarin.Forms;

using System.Threading;
using Foundation;
using AVFoundation;
using CoreVideo;
using CoreMedia;
using CoreImage;
using CoreFoundation;

namespace Emgu.Util
{
    public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        public class BufferReceivedEventArgs : EventArgs
        {
            public CMSampleBuffer Buffer { get; private set; }
            public BufferReceivedEventArgs(CMSampleBuffer buffer)
            {
                this.Buffer = buffer;
            }
        }

        public event EventHandler<BufferReceivedEventArgs> BufferReceived;

        public override void DidOutputSampleBuffer(AVCaptureOutput captureOutput, CMSampleBuffer sampleBuffer, AVCaptureConnection connection)
        {
            if (BufferReceived != null)
            {
                BufferReceived(this, new BufferReceivedEventArgs(sampleBuffer));
            }
            sampleBuffer.Dispose();
        }
    }
}

#endif