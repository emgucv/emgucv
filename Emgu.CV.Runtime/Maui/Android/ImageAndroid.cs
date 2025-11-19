//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Android.Graphics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Android.Content;
using Android.Content.Res;
using Emgu.CV.CvEnum;
using Bitmap = Android.Graphics.Bitmap;

namespace Emgu.CV
{
    /// <summary>
    /// Extension method for Android platform
    /// </summary>
    public static partial class AndroidExtension
    {

        /// <summary>
        /// Read image file from Assets
        /// </summary>
        /// <param name="assets">The Asset manager</param>
        /// <param name="fileName">The name of the file</param>
        public static Image<TColor, TDepth> GetImage<TColor, TDepth>(this AssetManager assets, String fileName)
            where TColor : struct, IColor
            where TDepth : new()
        {
            using (Stream imageStream = assets.Open(fileName))
            using (Android.Graphics.Bitmap imageBmp = BitmapFactory.DecodeStream(imageStream))
                return imageBmp.ToImage<TColor, TDepth>();
        }

        /// <summary> Create a Bitmap image of certain size</summary>
        /// <param name="width">The width of the bitmap</param>
        /// <param name="height">The height of the bitmap</param>
        /// <param name="image">The Image to convert to Bitmap</param>
        /// <returns> This image in Bitmap format of the specific size</returns>
        public static Android.Graphics.Bitmap ToBitmap<TColor, TDepth>(this Image<TColor, TDepth> image, int width, int height)
            where TColor : struct, IColor
            where TDepth : new()
        {
            using (Image<TColor, TDepth> scaledImage = image.Resize(width, height, CvEnum.Inter.Linear))
                return scaledImage.ToBitmap();
        }

        /// <summary> 
        /// Convert this image into Bitmap, the pixel values are copied over to the Bitmap
        /// </summary>
        /// <returns> This image in Bitmap format, the pixel data are copied over to the Bitmap</returns>
        public static Android.Graphics.Bitmap ToBitmap<TColor, TDepth>(this Image<TColor, TDepth> image)
            where TColor : struct, IColor
            where TDepth : new()
        {
            return image.ToBitmap(Android.Graphics.Bitmap.Config.Argb8888);
        }

        /// <summary>
        /// Convert Image object to Bitmap
        /// </summary>
        /// <param name="config">The Bitmap Config</param>
        /// <param name="image">The image to convert to Bitmap</param>
        /// <returns>The Bitmap</returns>
        public static Android.Graphics.Bitmap ToBitmap<TColor, TDepth>(this Image<TColor, TDepth> image, Android.Graphics.Bitmap.Config config)
            where TColor : struct, IColor
            where TDepth : new()
        {
            System.Drawing.Size size = image.Size;

            if (config == Android.Graphics.Bitmap.Config.Argb8888)
            {
                Android.Graphics.Bitmap result = Android.Graphics.Bitmap.CreateBitmap(size.Width, size.Height, Android.Graphics.Bitmap.Config.Argb8888);

                using (BitmapArgb8888Image bi = new BitmapArgb8888Image(result))
                {
                    bi.ConvertFrom(image);
                    //CvInvoke.cvSet(bi, new MCvScalar(0, 0, 255, 255), IntPtr.Zero);
                }
                return result;
            }
            else if (config == Android.Graphics.Bitmap.Config.Rgb565)
            {
                Android.Graphics.Bitmap result = Android.Graphics.Bitmap.CreateBitmap(size.Width, size.Height, Android.Graphics.Bitmap.Config.Rgb565);

                using (BitmapRgb565Image bi = new BitmapRgb565Image(result))
                    bi.ConvertFrom(image);
                return result;
            }
            else
            {
                throw new NotImplementedException("Only Bitmap config of Argb888 or Rgb565 is supported.");
            }
        }

        /// <summary>
        /// Convert Android bitmap to Mat
        /// </summary>
        public static void ToMat(this Bitmap bitmap, Mat image)
        {
            //Mat image = new Mat();

            Android.Graphics.Bitmap.Config config = bitmap.GetConfig();
            if (config.Equals(Android.Graphics.Bitmap.Config.Argb8888))
            {
                using (BitmapArgb8888Image bi = new BitmapArgb8888Image(bitmap))
                {
                    CvInvoke.CvtColor(bi, image, ColorConversion.Bgra2Bgr);
                    //image.ConvertFrom(bi);
                }
            }
            else if (config.Equals(Android.Graphics.Bitmap.Config.Rgb565))
            {
                Size size = new Size(bitmap.Width, bitmap.Height);
                int[] values = new int[size.Width * size.Height];
                bitmap.GetPixels(values, 0, size.Width, 0, 0, size.Width, size.Height);
                GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
                using (Mat bgra = new Mat(
                           size,
                           DepthType.Cv8U,
                           4,
                           handle.AddrOfPinnedObject(),
                           size.Width * 4))
                {
                    CvInvoke.CvtColor(bgra, image, ColorConversion.Bgra2Bgr);
                }
                handle.Free();
            }
            else
            {
                throw new NotImplementedException(String.Format("Coping from Bitmap of {0} is not implemented", config));
            }

            //return image;
        }
    }
}
*/