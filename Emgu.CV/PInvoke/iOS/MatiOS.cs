//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __IOS__
using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using CoreGraphics;
using Emgu.CV.CvEnum;
using UIKit;

namespace Emgu.CV
{
   public partial class Mat
   {
      public Mat(CGImage cgImage)
         : this()
      {
         ConvertFromCGImage(cgImage);
      }

      private void ConvertFromCGImage(CGImage cgImage)
      {
         using (Mat m = new Mat(new Size((int)cgImage.Width, (int)cgImage.Height), DepthType.Cv8U, 4))
         {
            RectangleF rect = new RectangleF(PointF.Empty, new SizeF(cgImage.Width, cgImage.Height));
            using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
            using (CGBitmapContext context = new CGBitmapContext(
            m.DataPointer,             
             Width, Height,
             8,
             Width * 4,
             cspace,
             CGImageAlphaInfo.PremultipliedLast))
               context.DrawImage(rect, cgImage);
CvInvoke.CvtColor(m, this, ColorConversion.Rgba2Bgr);
         } 
      }

      public Mat(UIImage uiImage)
         : this (uiImage.CGImage)
      {
      }

      private CGImage RgbaByteMatToCGImage(Mat bgraByte)
      {
         using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
         using (CGBitmapContext context = new CGBitmapContext(
            bgraByte.DataPointer,
            Width, Height,
            8,
            Width*4,
            cspace,
            CGImageAlphaInfo.PremultipliedLast))
            return context.ToImage();
      }

      public UIImage ToUIImage()
      {
         using (CGImage tmp = ToCGImage())
         {
            return UIImage.FromImage(tmp);
         }
      }
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
         }else if (nchannels == 3 && d == DepthType.Cv8U)
         {
           //bgr
            using (Mat tmp = new Mat())
            {
               CvInvoke.CvtColor(this, tmp, ColorConversion.Bgr2Rgba);
               return RgbaByteMatToCGImage(tmp);
            }
         } else if (nchannels == 1 && d == DepthType.Cv8U)
         {
            using (Mat tmp = new Mat())
            {
               CvInvoke.CvtColor(this, tmp, ColorConversion.Gray2Rgba);
               return RgbaByteMatToCGImage();
            }
         } else
         {
            throw new Exception(String.Format("Converting from Mat of {0} channels {1} to CGImage is not supported. Please convert Mat to 3 channel Bgr image of Byte before calling this function.", nchannels, d));
         }
      }
   }
}

#endif