//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Android.Graphics;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Android.Content;
using Android.Content.Res;
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

        //#region Conversion with Bitmap
        /// <summary>
        /// The Get property provide a more efficient way to convert Image&lt;Gray, Byte&gt;, Image&lt;Bgr, Byte&gt; and Image&lt;Bgra, Byte&gt; into Bitmap
        /// such that the image data is <b>shared</b> with Bitmap. 
        /// If you change the pixel value on the Bitmap, you change the pixel values on the Image object as well!
        /// For other types of image this property has the same effect as ToBitmap()
        /// <b>Take extra caution not to use the Bitmap after the Image object is disposed</b>
        /// The Set property convert the bitmap to this Image type.
        /// </summary>
        public static Image<TColor, TDepth> ToImage<TColor, TDepth>(this Bitmap bitmap)
            where TColor : struct, IColor
            where TDepth : new()
        {

            #region reallocate memory if necessary
            Size size = new Size(bitmap.Width, bitmap.Height);
            Image<TColor, TDepth> image = new Image<TColor, TDepth>(size);
            /*
            if (image.Ptr == IntPtr.Zero)
            {
                image.AllocateData(size.Height, size.Width, image.NumberOfChannels);
            }
            else if (!Size.Equals(size))
            {
                image.DisposeObject();
                image.AllocateData(size.Height, size.Width, image.NumberOfChannels);
            }*/
            #endregion

            Android.Graphics.Bitmap.Config config = bitmap.GetConfig();
            if (config.Equals(Android.Graphics.Bitmap.Config.Argb8888))
            {
                using (BitmapArgb8888Image bi = new BitmapArgb8888Image(bitmap))
                {
                    image.ConvertFrom(bi);
                }
            }
            else if (config.Equals(Android.Graphics.Bitmap.Config.Rgb565))
            {
                int[] values = new int[size.Width * size.Height];
                bitmap.GetPixels(values, 0, size.Width, 0, 0, size.Width, size.Height);
                GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
                using (Image<Bgra, Byte> bgra = new Image<Bgra, byte>(size.Width, size.Height, size.Width * 4, handle.AddrOfPinnedObject()))
                {
                    image.ConvertFrom(bgra);
                }
                handle.Free();
            }
            else
            {
                throw new NotImplementedException(String.Format("Coping from Bitmap of {0} is not implemented", config));
            }

            return image;
        }
    }
}

#endif