using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.CoreGraphics;
using MonoTouch.UIKit;

namespace Emgu.CV
{
   public partial class Image<TColor, TDepth>
      : CvArray<TDepth>, IImage, IEquatable<Image<TColor, TDepth>>
      where TColor : struct, IColor
      where TDepth : new()
   {
      public Image(CGImage cgImage)
         : this(cgImage.Width, cgImage.Height)
      {
         if (this is Image<Rgba, Byte>)
         {
            RectangleF rect = new RectangleF(PointF.Empty, new SizeF(cgImage.Width, cgImage.Height));
            using (CGBitmapContext context = new CGBitmapContext(
             MIplImage.imageData,
             Width, Height,
             8,
             Width * 4,
             CGColorSpace.CreateDeviceRGB(),
             CGImageAlphaInfo.PremultipliedLast))
               context.DrawImage(rect, cgImage);
         } else
         {
            using (Image<Rgba, Byte> tmp = new Image<Rgba, Byte>(cgImage))
               ConvertFrom(tmp);
         }
      }

      public Image(UIImage uiImage)
         : this (uiImage.CGImage)
      {
      }

      public UIImage ToUIImage()
      {
         if (this is Image<Rgba, Byte>)
         {
            using (CGBitmapContext context = new CGBitmapContext(
         		MIplImage.imageData,
         		Width, Height,
         		8,
         		Width * 4,
         		CGColorSpace.CreateDeviceRGB(),
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

