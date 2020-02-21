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
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Reflection;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Android.Content;
using Android.Content.Res;

namespace Emgu.CV
{
    //public partial class Mat : MatDataAllocator, IInputArray, IOutputArray, IInputOutputArray
    public static partial class AndroidExtension
    {
        /// <summary>
        /// Create a Mat from an Android Bitmap
        /// </summary>
        /// <param name="bitmap">The android Bitmap</param>
        public static Mat ToMat(this Android.Graphics.Bitmap bitmap)
        {
            Mat m = new Mat();
            m.SetBitmap(bitmap);
            return m;
        }

        /// <summary>
        /// Read an image file from Android Asset
        /// </summary>
        /// <param name="assets">The asset manager</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="mode">The read mode</param>
        public static Mat GetMat(this AssetManager assets, String fileName, ImreadModes mode = ImreadModes.AnyColor | ImreadModes.AnyDepth)
        {
            Mat m = new Mat();
            using (Stream imageStream = assets.Open(fileName))
            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);
                CvInvoke.Imdecode(ms.ToArray(), mode, m);
            }
            return m;
        }

        /// <summary>
        /// Convert a Bitmap to and from this Mat
        /// </summary>
        /// <param name="mat">The mat to copy Bitmap into</param>
        /// <param name="bitmap">The bitmap to copy into mat</param>
        public static void SetBitmap(this Mat mat, Android.Graphics.Bitmap bitmap)
        {

            Android.Graphics.Bitmap.Config config = bitmap.GetConfig();
            if (config.Equals(Android.Graphics.Bitmap.Config.Argb8888))
            {
                using (BitmapArgb8888Image bi = new BitmapArgb8888Image(bitmap))
                {
                    CvInvoke.CvtColor(bi, mat, ColorConversion.Rgba2Bgra);
                }
            }
            else if (config.Equals(Android.Graphics.Bitmap.Config.Rgb565))
            {
                Size size = new Size(bitmap.Width, bitmap.Height);
                int[] values = new int[size.Width * size.Height];
                bitmap.GetPixels(values, 0, size.Width, 0, 0, size.Width, size.Height);
                GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
                using (Mat bgra = new Mat(size, DepthType.Cv8U, 4, handle.AddrOfPinnedObject(), size.Width * 4))
                {
                    bgra.CopyTo(mat);
                }
                handle.Free();
            }
            else
            {
                throw new NotImplementedException(String.Format("Coping from Bitmap of {0} is not implemented", config));
            }

        }

        /// <summary>
        /// Convert the Mat to Bitmap
        /// </summary>
        /// <param name="mat">The Mat to convert to Bitmap</param>
        /// <param name="config">The bitmap config type. If null, Argb8888 will be used</param>
        /// <returns>The Bitmap</returns>
        public static Android.Graphics.Bitmap ToBitmap(this Mat mat, Android.Graphics.Bitmap.Config config = null)
        {
            System.Drawing.Size size = mat.Size;

            if (config == null)
                config = Android.Graphics.Bitmap.Config.Argb8888;

            Android.Graphics.Bitmap result = Android.Graphics.Bitmap.CreateBitmap(size.Width, size.Height, config);
            mat.ToBitmap(result);
            return result;
        }

        /// <summary>
        /// Convert the Mat to Bitmap
        /// </summary>
        /// <param name="mat">The Mat to convert to Bitmap</param>
        /// <param name="bitmap">The bitmap, must be of the same size and has bitmap config type of either Argb888 or Rgb565</param>
        /// <returns>The Bitmap</returns>
        public static void ToBitmap(this Mat mat, Android.Graphics.Bitmap bitmap)
        {
            System.Drawing.Size size = mat.Size;
            if (!(size.Width == bitmap.Width && size.Height == bitmap.Height))
            {
                throw new Exception("Bitmap size doesn't match the Mat size");
            }

            Android.Graphics.Bitmap.Config config = bitmap.GetConfig();
            if (config == Android.Graphics.Bitmap.Config.Argb8888)
            {
                int channels = mat.NumberOfChannels;
                using (BitmapArgb8888Image bi = new BitmapArgb8888Image(bitmap))
                {
                    if (channels == 1)
                    {
                        CvInvoke.CvtColor(mat, bi.Mat, ColorConversion.Gray2Rgba);
                    }
                    else if (channels == 3)
                    {
                        CvInvoke.CvtColor(mat, bi, ColorConversion.Bgr2Rgba);
                    }
                    else if (channels == 4)
                    {
                        CvInvoke.CvtColor(mat, bi, ColorConversion.Bgra2Rgba);
                    }
                    else
                    {
                        using (Image<Rgba, Byte> tmp = mat.ToImage<Rgba, Byte>())
                        {
                            tmp.Copy(bi, null);
                        }

                    }
                }

            }
            else if (config == Android.Graphics.Bitmap.Config.Rgb565)
            {
                using (BitmapRgb565Image bi = new BitmapRgb565Image(bitmap))
                using (Image<Bgr, Byte> tmp = mat.ToImage<Bgr, Byte>())
                    bi.ConvertFrom(tmp);
            }
            else
            {
                throw new NotImplementedException("Only Bitmap config of Argb888 or Rgb565 is supported.");
            }
        }
    }
}

#endif