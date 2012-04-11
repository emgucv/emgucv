using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using MonoTouch.CoreGraphics;

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
         if (this is Image<Bgra, Byte>)
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
            using (Image<Bgra, Byte> tmp = new Image<Bgra, Byte>(cgImage))
               ConvertFrom(tmp);
         }
      }

      public CGImage ToCGImage()
      {
         if (this is Image<Bgra, Byte>)
         {
            using (CGBitmapContext context = new CGBitmapContext(
         		MIplImage.imageData,
         		Width, Height,
         		8,
         		Width * 4,
         		CGColorSpace.CreateDeviceRGB(),
         		CGImageAlphaInfo.PremultipliedLast))
               return context.ToImage();
         } else
         {
            using (Image<Bgra, Byte> tmp = Convert<Bgra, Byte>())
            {
               return tmp.ToCGImage();
            }
         }
      }
   }
}

