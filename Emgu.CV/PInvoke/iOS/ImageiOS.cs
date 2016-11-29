//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using CoreGraphics;
using UIKit;

namespace Emgu.CV
{
   public partial class Image<TColor, TDepth>
      : CvArray<TDepth>, IImage, IEquatable<Image<TColor, TDepth>>
      where TColor : struct, IColor
      where TDepth : new()
   {
      /// <summary>
      /// Creating an Image from the CGImage
      /// </summary>
      public Image(CGImage cgImage)
         : this( (int) cgImage.Width, (int) cgImage.Height)
      {
         ConvertFromCGImage(cgImage);
      }

      /// <summary>
      /// Copy the data from the CGImage to the current Image object
      /// </summary>
      private void ConvertFromCGImage(CGImage cgImage)
      {
         //Don't do this, Xamarin.iOS won't be able to resolve: if (this is Image<Rgba, Byte>)
         if (typeof(TColor) == typeof(Rgba) && typeof(TDepth) == typeof(byte))
         {
            RectangleF rect = new RectangleF(PointF.Empty, new SizeF(cgImage.Width, cgImage.Height));
            using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
            using (CGBitmapContext context = new CGBitmapContext(
             MIplImage.ImageData,
             Width, Height,
             8,
             Width * 4,
             cspace,
             CGImageAlphaInfo.PremultipliedLast))
               context.DrawImage(rect, cgImage);
         } else
         {
            using (Image<Rgba, Byte> tmp = new Image<Rgba, Byte>(cgImage))
               ConvertFrom(tmp);
         }
      }

      /// <summary>
      /// Creating an Image from the UIImage
      /// </summary>
      public Image(UIImage uiImage)
         : this (uiImage.CGImage)
      {
      }

      /// <summary>
      /// Convert this Image object to UIImage
      /// </summary>
      public UIImage ToUIImage()
      {
         //Don't do this, Xamarin.iOS won't be able to resolve: if (this is Image<Rgba, Byte>)
         if (typeof(TColor) == typeof(Rgba) && typeof(TDepth) == typeof(Byte))
         {
            using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
            using (CGBitmapContext context = new CGBitmapContext(
               MIplImage.ImageData,
         		Width, Height,
         		8,
         		Width * 4,
               cspace,
         		CGImageAlphaInfo.PremultipliedLast))
            using (CGImage cgImage =  context.ToImage())
            {
               return UIImage.FromImage(cgImage);
            }
         } else
         {
            using (Image<Rgba, Byte> tmp = Convert<Rgba, Byte>())
            {
               return tmp.ToUIImage();
            }
         }
      }
   }
}

#endif