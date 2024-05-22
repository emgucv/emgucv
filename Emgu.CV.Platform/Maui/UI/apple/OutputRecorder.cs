//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__ || __MACCATALYST__

using System;
using CoreGraphics;
//using Xamarin.Forms;

using System.Threading;
using Foundation;
using AVFoundation;
using CoreVideo;
using CoreMedia;
using CoreImage;
using CoreFoundation;

namespace Emgu.Util
{
    /// <summary>
    /// The OutputRecorder class is a delegate for handling the output of a sample buffer from a capture output.
    /// </summary>
    /// <remarks>
    /// This class is used to handle the output of a sample buffer from a capture output. It provides an event that is triggered when a buffer is received, and a method for handling the output of a sample buffer.
    /// </remarks>
    public class OutputRecorder : AVCaptureVideoDataOutputSampleBufferDelegate
    {
        /// <summary>
        /// Represents the arguments for the BufferReceived event in the OutputRecorder class.
        /// </summary>
        /// <remarks>
        /// This class contains a CMSampleBuffer that is outputted from a capture output.
        /// </remarks>
        public class BufferReceivedEventArgs : EventArgs
        {
            /// <summary>
            /// Gets the CMSampleBuffer instance associated with the BufferReceived event.
            /// </summary>
            /// <value>
            /// The CMSampleBuffer instance.
            /// </value>
            public CMSampleBuffer Buffer { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="BufferReceivedEventArgs"/> class.
            /// </summary>
            /// <param name="buffer">The CMSampleBuffer that was outputted.</param>
            public BufferReceivedEventArgs(CMSampleBuffer buffer)
            {
                this.Buffer = buffer;
            }
        }

        /// <summary>
        /// Occurs when a new buffer is received from the AVCaptureVideoDataOutputSampleBufferDelegate.
        /// </summary>
        /// <remarks>
        /// This event is triggered in the DidOutputSampleBuffer method when a new sample buffer is available.
        /// The BufferReceivedEventArgs contains the received CMSampleBuffer.
        /// </remarks>
        public event EventHandler<BufferReceivedEventArgs> BufferReceived;

        /// <summary>
        /// Handles the output of a sample buffer from a capture output.
        /// </summary>
        /// <param name="captureOutput">The capture output that outputted the sample buffer.</param>
        /// <param name="sampleBuffer">The sample buffer that was outputted.</param>
        /// <param name="connection">The connection from which the sample buffer was outputted.</param>
        /// <remarks>
        /// This method raises the BufferReceived event with the sample buffer as argument. After the event is raised, the sample buffer is disposed.
        /// </remarks>
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