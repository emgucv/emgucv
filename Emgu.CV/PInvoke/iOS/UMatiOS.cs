//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __UNIFIED__
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using CoreGraphics;
using Emgu.CV.CvEnum;
#if __IOS__
using UIKit;
#else
using AppKit;
#endif

namespace Emgu.CV
{
    public partial class UMat
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from CGImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="cgImage">The CGImage.</param>
        public UMat(CGImage cgImage, ImreadModes mode = ImreadModes.AnyColor)
         : this()
        {
            CvInvoke.ConvertCGImageToArray(cgImage, this, mode);
        }

        /*
      private void ConvertFromCGImage(CGImage cgImage)
      {
         Size sz = new Size((int) cgImage.Width, (int) cgImage.Height);
         using (Mat m = new Mat(sz, DepthType.Cv8U, 4))
         {
            RectangleF rect = new RectangleF(PointF.Empty, new SizeF(cgImage.Width, cgImage.Height));
            using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
            using (CGBitmapContext context = new CGBitmapContext(
             m.DataPointer,             
             sz.Width, sz.Height,
             8,
             sz.Width * 4,
             cspace,
             CGImageAlphaInfo.PremultipliedLast))
               context.DrawImage(rect, cgImage);
            CvInvoke.CvtColor(m, this, ColorConversion.Rgba2Bgr);
         } 
      }*/

        private static CGImage RgbaByteMatToCGImage(Mat bgraByte)
        {
            using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
            using (CGBitmapContext context = new CGBitmapContext(
               bgraByte.DataPointer,
               bgraByte.Width, bgraByte.Height,
               8,
               bgraByte.Width * 4,
               cspace,
               CGImageAlphaInfo.PremultipliedLast))
                return context.ToImage();
        }

        /// <summary>
        /// Converts to CGImage
        /// </summary>
        /// <returns>The CGImage.</returns>
        public CGImage ToCGImage()
        {
            int nchannels = NumberOfChannels;
            DepthType d = Depth;
            if (nchannels == 4 && d == DepthType.Cv8U)
            {
                //bgra
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(this, tmp, ColorConversion.Bgra2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else if (nchannels == 3 && d == DepthType.Cv8U)
            {
                //bgr
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(this, tmp, ColorConversion.Bgr2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else if (nchannels == 1 && d == DepthType.Cv8U)
            {
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(this, tmp, ColorConversion.Gray2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else
            {
                throw new Exception(String.Format("Converting from Mat of {0} channels {1} to CGImage is not supported. Please convert Mat to 3 channel Bgr image of Byte before calling this function.", nchannels, d));
            }
        }

#if __IOS__
      /// <summary>
      /// Initializes a new instance of the <see cref="Emgu.CV.UMat"/> class from UIImage
      /// </summary>
	  /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
      /// <param name="uiImage">The UIImage.</param>
      public UMat(UIImage uiImage, ImreadModes mode = ImreadModes.AnyColor)
         : this ()
      {
		using(CGImage cgImage = uiImage.CGImage)
		{
		CvInvoke.ConvertCGImageToArray(cgImage, this, mode);
		}
      }

      /// <summary>
      /// Converts to UIImage.
      /// </summary>
      /// <returns>The UIImage.</returns>
      public UIImage ToUIImage()
      {
         using (CGImage tmp = ToCGImage())
         {
            return UIImage.FromImage(tmp);
         }
      }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from NSImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="nsImage">The NSImage.</param>
        public UMat(NSImage nsImage, ImreadModes mode = ImreadModes.AnyColor)
           : this()
        {
            using (CGImage cgImage = nsImage.CGImage)
            {
                CvInvoke.ConvertCGImageToArray(cgImage, this, mode);
            }
        }

        /// <summary>
        /// Converts to NSImage.
        /// </summary>
        /// <returns>The NSImage.</returns>
        public NSImage ToNSImage()
        {
            using (CGImage tmp = ToCGImage())
            {
                return new NSImage(tmp, new CGSize(tmp.Width, tmp.Height));
            }
        }
#endif

    }
}

#endif