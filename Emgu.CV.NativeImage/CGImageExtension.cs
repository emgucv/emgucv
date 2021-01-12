//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __UNIFIED__
using System;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using CoreGraphics;
using Emgu.CV.CvEnum;

namespace Emgu.CV
{
    /// <summary>
    /// Provide extension method to convert IInputArray to and from CGImage
    /// </summary>
    public static class CGImageExtension
    {
        /// <summary>
        /// Creating an Image from the CGImage
        /// </summary>
        public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this CGImage cgImage)
            where TColor : struct, IColor
            where TDepth : new()

        {
            Image<TColor, TDepth> image = new Image<TColor, TDepth>((int)cgImage.Width, (int)cgImage.Height);
            cgImage.ToImage<TColor, TDepth>(image);
            return image;
        }

        /// <summary>
        /// Copy the data from the CGImage to the current Image object
        /// </summary>
        internal static void ToImage<TColor, TDepth>(this CGImage cgImage, Image<TColor, TDepth> image)
            where TColor : struct, IColor
            where TDepth : new()
        {
            //Don't do this, Xamarin.iOS won't be able to resolve: if (this is Image<Rgba, Byte>)
            if (typeof(TColor) == typeof(Rgba) && typeof(TDepth) == typeof(byte))
            {
                Debug.Assert((image.Width == (int)cgImage.Width) && (image.Height == (int)cgImage.Height), "Incompatible dimension between the CGImage and Image<,>.");
                RectangleF rect = new RectangleF(PointF.Empty, new SizeF(cgImage.Width, cgImage.Height));
                using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
                using (CGBitmapContext context = new CGBitmapContext(
                 image.MIplImage.ImageData,
                 image.Width, image.Height,
                 8,
                 image.Width * 4,
                 cspace,
                 CGImageAlphaInfo.PremultipliedLast))
                    context.DrawImage(rect, cgImage);
            }
            else
            {
                using (Image<Rgba, Byte> tmp = cgImage.ToImage<Rgba, Byte>())
                    image.ConvertFrom(tmp);
            }
        }

        /// <summary>
        /// Convert this Image object to CGImage
        /// </summary>
        public static CGImage ToCGImage<TColor, TDepth>(this Image<TColor, TDepth> image)
         where TColor : struct, IColor
            where TDepth : new()
        {

            //Don't do this, Xamarin.iOS won't be able to resolve: if (this is Image<Rgba, Byte>)
            if (typeof(TColor) == typeof(Rgba) && typeof(TDepth) == typeof(Byte))
            {
                using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
                using (CGBitmapContext context = new CGBitmapContext(
                   image.MIplImage.ImageData,
                     image.Width, image.Height,
                     8,
                     image.Width * 4,
                   cspace,
                     CGImageAlphaInfo.PremultipliedLast))

                {
                    CGImage cgImage = context.ToImage();
                    return cgImage;
                }
            }
            else
            {
                using (Image<Rgba, Byte> tmp = image.Convert<Rgba, Byte>())
                {
                    return tmp.ToCGImage();
                }
            }
        }

        internal static void ToArray(this CGImage cgImage, IOutputArray mat, ImreadModes modes = ImreadModes.AnyColor)
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
                if (modes == ImreadModes.Unchanged)
                {
                    m.CopyTo(mat);
                }
                if (modes == ImreadModes.Grayscale)
                {
                    CvInvoke.CvtColor(m, mat, ColorConversion.Rgba2Gray);
                }
                else if (modes == ImreadModes.AnyColor)
                {
                    CvInvoke.CvtColor(m, mat, ColorConversion.Rgba2Bgra);
                }
                else if (modes == ImreadModes.Color)
                {
                    CvInvoke.CvtColor(m, mat, ColorConversion.Rgba2Bgr);
                }
                else if (modes == ImreadModes.ReducedColor2)
                {
                    using (Mat tmp = new Mat())
                    {
                        CvInvoke.PyrDown(m, tmp);
                        CvInvoke.CvtColor(tmp, mat, ColorConversion.Rgba2Bgr);
                    }
                }
                else if (modes == ImreadModes.ReducedGrayscale2)
                {
                    using (Mat tmp = new Mat())
                    {
                        CvInvoke.PyrDown(m, tmp);
                        CvInvoke.CvtColor(tmp, mat, ColorConversion.Rgba2Gray);
                    }
                }
                else if (modes == ImreadModes.ReducedColor4 
                         || modes == ImreadModes.ReducedColor8 
                         || modes == ImreadModes.ReducedGrayscale4 
                         || modes == ImreadModes.ReducedGrayscale8 
                         || modes == ImreadModes.LoadGdal)
                {
                    throw new NotImplementedException(String.Format("Conversion from PNG using mode {0} is not supported", modes));
                }
                else
                {
                    throw new Exception(String.Format("ImreadModes of {0} is not implemented.", modes.ToString()));
                    //CvInvoke.CvtColor(m, mat, ColorConversion.Rgba2Bgr);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from CGImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="cgImage">The CGImage.</param>
        public static Mat ToMat(this CGImage cgImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            Mat m = new Mat();
            cgImage.ToArray(m, mode);
            return m;
        }

        private static CGImage RgbaByteMatToCGImage(Mat bgraByte)
        {
            using (CGColorSpace cspace = CGColorSpace.CreateDeviceRGB())
            using (CGBitmapContext context = new CGBitmapContext(
                bgraByte.DataPointer,
                bgraByte.Width, 
                bgraByte.Height,
                8,
                bgraByte.Width * 4,
                cspace,
                CGImageAlphaInfo.PremultipliedLast))
                return context.ToImage();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emgu.CV.Mat"/> class from CGImage
        /// </summary>
        /// <param name="mode">The color conversion mode. By default, it convert the UIImage to BGRA color type to preserve all the image channels.</param>
        /// <param name="cgImage">The CGImage.</param>
        public static UMat ToUMat(this CGImage cgImage, ImreadModes mode = ImreadModes.AnyColor)
        {
            UMat umat = new UMat();
            cgImage.ToArray(umat, mode);
            return umat;
        }

        /*
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
        }*/

        /// <summary>
        /// Converts to CGImage
        /// </summary>
        /// <returns>The CGImage.</returns>
        public static CGImage ToCGImage(this UMat umat)
        {
            int nchannels = umat.NumberOfChannels;
            DepthType d = umat.Depth;
            if (nchannels == 4 && d == DepthType.Cv8U)
            {
                //bgra
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(umat, tmp, ColorConversion.Bgra2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else if (nchannels == 3 && d == DepthType.Cv8U)
            {
                //bgr
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(umat, tmp, ColorConversion.Bgr2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else if (nchannels == 1 && d == DepthType.Cv8U)
            {
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(umat, tmp, ColorConversion.Gray2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else
            {
                throw new Exception(String.Format("Converting from Mat of {0} channels {1} to CGImage is not supported. Please convert Mat to 3 channel Bgr image of Byte before calling this function.", nchannels, d));
            }
        }

        /// <summary>
        /// Converts to CGImage
        /// </summary>
        /// <returns>The CGImage.</returns>
        public static CGImage ToCGImage(this Mat mat)
        {
            int nchannels = mat.NumberOfChannels;
            DepthType d = mat.Depth;
            if (nchannels == 4 && d == DepthType.Cv8U)
            {
                //bgra
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(mat, tmp, ColorConversion.Bgra2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else if (nchannels == 3 && d == DepthType.Cv8U)
            {
                //bgr
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(mat, tmp, ColorConversion.Bgr2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else if (nchannels == 1 && d == DepthType.Cv8U)
            {
                using (Mat tmp = new Mat())
                {
                    CvInvoke.CvtColor(mat, tmp, ColorConversion.Gray2Rgba);
                    return RgbaByteMatToCGImage(tmp);
                }
            }
            else
            {
                throw new Exception(String.Format("Converting from Mat of {0} channels {1} to CGImage is not supported. Please convert Mat to 3 channel Bgr image of Byte before calling this function.", nchannels, d));
            }
        }

    }

    /*
    public class CGImageFileReaderMat : Emgu.CV.IFileReaderMat
    {

        public bool ReadFile(String fileName, Mat mat, CvEnum.ImreadModes loadType)
        {
            try
            {
                using (CGDataProvider provider = new CGDataProvider(fileName))
                using (CGImage tmp = CGImage.FromPNG(provider, null, false, CGColorRenderingIntent.Default))
                {
                    tmp.ToArray(mat, loadType);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                //throw;
                return false;
            }

        }
    }*/
}
#endif