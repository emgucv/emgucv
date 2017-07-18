//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
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
	public partial class Mat
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from CGImage
		/// </summary>
		/// <param name="cgImage">The CGImage.</param>
		public Mat(CGImage cgImage)
		   : this()
		{
			ConvertFromCGImage(cgImage);
		}

		private void ConvertFromCGImage(CGImage cgImage, ImreadModes modes = ImreadModes.AnyColor)
		{
			Size sz = new Size((int)cgImage.Width, (int)cgImage.Height);
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
			    if (modes == ImreadModes.Grayscale)
			    {
                    CvInvoke.CvtColor(m, this, ColorConversion.Rgba2Gray);
			    } else if (modes == ImreadModes.AnyColor)
			    {
			        CvInvoke.CvtColor(m, this, ColorConversion.Rgba2Bgra);
			    } else if (modes == ImreadModes.ReducedColor2)
			    {
			        using (Mat tmp = new Mat())
			        {
			            CvInvoke.PyrDown(m, tmp);
			            CvInvoke.CvtColor(tmp, this, ColorConversion.Rgba2Bgr);
			        }
			    } else if (modes == ImreadModes.ReducedGrayscale2)
			    {
			        using (Mat tmp = new Mat())
			        {
			            CvInvoke.PyrDown(m, tmp);
			            CvInvoke.CvtColor(tmp, this, ColorConversion.Rgba2Gray);
			        }
                }
                else if (modes == ImreadModes.ReducedColor4 || modes == ImreadModes.ReducedColor8 || modes == ImreadModes.ReducedGrayscale4 || modes == ImreadModes.ReducedGrayscale8 || modes == ImreadModes.LoadGdal)
			    {
			        throw  new NotImplementedException(String.Format("Conversion from PNG using mode {0} is not supported", modes));
			    }
			    else
			    {
			        CvInvoke.CvtColor(m, this, ColorConversion.Rgba2Bgr);
                }
			}
		}

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
      /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from UIImage
      /// </summary>
      /// <param name="uiImage">The UIImage.</param>
      public Mat(UIImage uiImage)
         : this ()
      {
		using(CGImage cgImage = uiImage.CGImage)
		{
		ConvertFromCGImage(cgImage);
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
		/// <param name="uiImage">The NSImage.</param>
		public Mat(NSImage nsImage)
		   : this()
		{
			using (CGImage cgImage = nsImage.CGImage)
			{
				ConvertFromCGImage(cgImage);
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