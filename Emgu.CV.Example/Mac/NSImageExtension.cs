//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __MACOS__

using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using CoreGraphics;
using Emgu.CV.CvEnum;
using AppKit;

namespace Emgu.CV
{
    public static class NSImageExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from NSImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="nsImage">The NSImage.</param>
        public static Mat ToMat(this NSImage nsImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            using (CGImage cgImage = nsImage.CGImage)
            {
                return cgImage.ToMat();
            }
        }

        /// <summary>
        /// Converts to NSImage.
        /// </summary>
        /// <returns>The NSImage.</returns>
        public static NSImage ToNSImage(this Mat mat)
        {
            using (CGImage tmp = mat.ToCGImage())
            {
                return new NSImage(tmp, new CGSize(tmp.Width, tmp.Height));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from NSImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="nsImage">The NSImage.</param>
        public static UMat ToUMat(this NSImage nsImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            using (CGImage cgImage = nsImage.CGImage)
            {
                return cgImage.ToUMat();
            }
        }

        /// <summary>
        /// Converts to NSImage.
        /// </summary>
        /// <returns>The NSImage.</returns>
        public static NSImage ToNSImage(this UMat umat)
        {
            using (CGImage tmp = umat.ToCGImage())
            {
                return new NSImage(tmp, new CGSize(tmp.Width, tmp.Height));
            }
        }
    }
}

#endif