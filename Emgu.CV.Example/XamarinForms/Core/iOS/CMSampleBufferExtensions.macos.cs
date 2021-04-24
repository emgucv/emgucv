//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __MACOS__

using System;
using CoreMedia;
using CoreVideo;
using CoreGraphics;
using CoreImage;
using System.Threading;

using AppKit;

namespace Emgu.Util
{
    public static class CMSampleBufferExtensions
    {
        public static NSImage ToNSImage(this CMSampleBuffer sampleBuffer)
        {
            NSImage image;
            using (CVPixelBuffer pixelBuffer = sampleBuffer.GetImageBuffer() as CVPixelBuffer)
            {
                // Lock the base address
                pixelBuffer.Lock(CVPixelBufferLock.ReadOnly);
                using (CIImage cIImage = new CIImage(pixelBuffer))
                {
                    image = null;

                    AutoResetEvent e = new AutoResetEvent(false);
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(delegate
                    {
                        NSCIImageRep rep = new NSCIImageRep(cIImage);
                        image = new NSImage(rep.Size);
                        image.AddRepresentation(rep);
                        e.Set();
                    });
                    e.WaitOne();

                }
                pixelBuffer.Unlock(CVPixelBufferLock.ReadOnly);
            }
            return image;
        }
    }
}
#endif