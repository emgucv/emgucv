using System;
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
      public CGImage ToCGImage ()
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
               return context.ToImage ();
         } else
         {
            using (Image<Bgra, Byte> tmp = Convert<Bgra, Byte>())
            {
               return tmp.ToCGImage ();
            }
         }
      }
   }
}

