//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __MACOS__
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using CoreGraphics;
using AppKit;

namespace Emgu.CV
{
   public static class NSImageExtension
   {
      /// <summary>
      /// Creating an Image from the NSImage
      /// </summary>
      public static Image<TColor, TDepth> ToImage<TColor, TDepth> (this NSImage nsImage)
         where TColor : struct, IColor
         where TDepth : new()
         //: this( (int) uiImage.Size.Width, (int) uiImage.Size.Height)
      {
         using (CGImage cgImage = nsImage.CGImage) {
            return cgImage.ToImage<TColor, TDepth> ();
         }
      }

      /// <summary>
      /// Convert this Image object to NSImage
      /// </summary>
      public static NSImage ToNSImage<TColor, TDepth> (this Image<TColor, TDepth> image)
         where TColor : struct, IColor
         where TDepth : new()
      {
         using (CGImage cgImage = image.ToCGImage ()) {
            return new NSImage (cgImage, new CGSize(cgImage.Width, cgImage.Height));
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
         using (CGImage cgImage = nsImage.CGImage) {
            //ConvertCGImageToArray (cgImage, this, mode);
            return cgImage.ToUMat (mode);
         }
      }

      /// <summary>
      /// Converts to NSImage.
      /// </summary>
      /// <returns>The NSImage.</returns>
      public static NSImage ToNSImage (this UMat umat)
      {
         using (CGImage cgImage = umat.ToCGImage ()) {
            return new NSImage(cgImage, new CGSize(cgImage.Width, cgImage.Height));
            }
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from UIImage
      /// </summary>
      /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
      /// <param name="nsImage">The NSImage.</param>
      public static Mat ToMat(this NSImage nsImage, ImreadModes mode = ImreadModes.AnyColor)
      {
         using (CGImage cgImage = nsImage.CGImage) {
            return cgImage.ToMat (mode);
         }
      }

      /// <summary>
      /// Converts to NSImage.
      /// </summary>
      /// <returns>The NSImage.</returns>
      public static NSImage ToNSImage (this Mat mat)
      {
         using (CGImage cgImage = mat.ToCGImage ()) {
            return new NSImage(cgImage, new CGSize(cgImage.Width, cgImage.Height));
            }
      }
   }
}

#endif