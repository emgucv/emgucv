//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __MACOS__ || __MACCATALYST__
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using CoreGraphics;
using AppKit;

namespace Emgu.CV
{
    /// <summary>
    /// Provide extension method to convert IInputArray to and from NSImage
    /// </summary>
    public static class NSImageExtension
    {


        /// <summary>
        /// Convert this Image object to NSImage
        /// </summary>
        public static NSImage ToNSImage<TColor, TDepth>(this Image<TColor, TDepth> image)
           where TColor : struct, IColor
           where TDepth : new()
        {
            using (CGImage cgImage = image.ToCGImage())
            {
                return new NSImage(cgImage, new CGSize(cgImage.Width, cgImage.Height));
            }
        }




        /// <summary>
        /// Converts to NSImage.
        /// </summary>
        /// <returns>The NSImage.</returns>
        public static NSImage ToNSImage(this UMat umat)
        {
            using (CGImage cgImage = umat.ToCGImage())
            {
                return new NSImage(cgImage, new CGSize(cgImage.Width, cgImage.Height));
            }
        }



        /// <summary>
        /// Converts to NSImage.
        /// </summary>
        /// <returns>The NSImage.</returns>
        public static NSImage ToNSImage(this Mat mat)
        {
            using (CGImage cgImage = mat.ToCGImage())
            {
                return new NSImage(cgImage, new CGSize(cgImage.Width, cgImage.Height));
            }
        }

        /// <summary>
        /// Converts to NSImage.
        /// </summary>
        /// <returns>The NSImage.</returns>
        public static NSImage ToNSImage(this IInputArray inputArray)
        {
            using (InputArray array = inputArray.GetInputArray())
            using (Mat m = array.GetMat())
            {
                return m.ToNSImage();
            }
        }

#if __MACOS__
        /// <summary>
        /// Creating an Image from the NSImage
        /// </summary>
        public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this NSImage nsImage)
            where TColor : struct, IColor
            where TDepth : new()
            //: this( (int) uiImage.Size.Width, (int) uiImage.Size.Height)
        {
            using (CGImage cgImage = nsImage.CGImage)
            {
                return cgImage.ToImage<TColor, TDepth>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.UMat"/> class from UIImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="nsImage">The NSImage.</param>
        public static UMat ToUMat(this NSImage nsImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            //UMat umat = new UMat ();
            using (CGImage cgImage = nsImage.CGImage)
            {
                //ConvertCGImageToArray (cgImage, this, mode);
                return cgImage.ToUMat(mode);
            }
        }
        /// <summary>
        /// Convert a NSImage to a IOutputArray
        /// </summary>
        /// <param name="nsImage">The source NSImage</param>
        /// <param name="mat">The destination array</param>
        /// <param name="modes">The color format for the destination array</param>
        /// <exception cref="NotImplementedException">Exception will be thrown if the ImreadModes is not supported.</exception>
        public static void ToArray(this NSImage nsImage, IOutputArray mat, ImreadModes modes = ImreadModes.AnyColor)
        {
            using (CGImage cgImage = nsImage.CGImage)
            {
                cgImage.ToArray(mat, modes);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from UIImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="nsImage">The NSImage.</param>
        public static Mat ToMat(this NSImage nsImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            using (CGImage cgImage = nsImage.CGImage)
            {
                return cgImage.ToMat(mode);
            }
        }
#endif

    }
}

#endif