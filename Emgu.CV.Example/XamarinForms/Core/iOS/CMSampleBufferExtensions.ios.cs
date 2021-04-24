//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__
using System;
using CoreMedia;
using CoreVideo;
using CoreGraphics;
using CoreImage;

using UIKit;

namespace Emgu.Util
{
    public static class CMSampleBufferExtensions
    {
        public static UIImage ToUIImage(this CMSampleBuffer sampleBuffer)
        {
            UIImage image;
            using (CVPixelBuffer pixelBuffer = sampleBuffer.GetImageBuffer() as CVPixelBuffer)
            {
                // Lock the base address
                pixelBuffer.Lock(CVPixelBufferLock.ReadOnly);
                using (CIImage cIImage = new CIImage(pixelBuffer))
                {

                    image = new UIImage(cIImage);

                }
                pixelBuffer.Unlock(CVPixelBufferLock.ReadOnly);
            }
            //Debug.WriteLine(String.Format("({2}) Received NSImage: {0}x{1}", image.Size.Width, image.Size.Height, flag));
            return image;
        }
    }
}
#endif