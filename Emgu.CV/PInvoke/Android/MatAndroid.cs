//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
    public partial class Mat : MatDataAllocator, IInputArray, IOutputArray, IInputOutputArray, IImage
    {
        /// <summary>
        /// Create a Mat from an Android Bitmap
        /// </summary>
        /// <param name="bmp">The android Bitmap</param>
        public Mat(Bitmap bmp)
           : this()
        {
            this.Bitmap = bmp;
        }

        /// <summary>
        /// Read an image file from Android Asset
        /// </summary>
        /// <param name="assets">The asset manager</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="mode">The read mode</param>
        public Mat(AssetManager assets, String fileName, ImreadModes mode = ImreadModes.AnyColor | ImreadModes.AnyDepth)
           : this()
        {
            using (Stream imageStream = assets.Open(fileName))
            using (MemoryStream ms = new MemoryStream())
            {
                imageStream.CopyTo(ms);
                CvInvoke.Imdecode(ms.ToArray(), mode, this);
            }
        }

        /// <summary>
        /// Convert a Bitmap to and from this Mat
        /// </summary>
        public Bitmap Bitmap
        {
            get { return ToBitmap(Android.Graphics.Bitmap.Config.Argb8888); }

            set
            {
                Bitmap.Config config = value.GetConfig();
                if (config.Equals(Bitmap.Config.Argb8888))
                {
                    using (BitmapArgb8888Image bi = new BitmapArgb8888Image(value))

                    {
                        CvInvoke.CvtColor(bi, this, ColorConversion.Rgba2Bgra);
                    }
                }
                else if (config.Equals(Bitmap.Config.Rgb565))
                {

                    Size size = new Size(value.Width, value.Height);
                    int[] values = new int[size.Width * size.Height];
                    value.GetPixels(values, 0, size.Width, 0, 0, size.Width, size.Height);
                    GCHandle handle = GCHandle.Alloc(values, GCHandleType.Pinned);
                    using (Mat bgra = new Mat(size, DepthType.Cv8U, 4, handle.AddrOfPinnedObject(), size.Width * 4))
                    {
                        bgra.CopyTo(this);
                    }
                    handle.Free();
                }
                else
                {
                    throw new NotImplementedException(String.Format("Coping from Bitmap of {0} is not implemented", config));
                }
            }
        }

        /// <summary>
        /// Convert the Mat to Bitmap
        /// </summary>
        /// <param name="config">The bitmap config type. If null, Argb8888 will be used</param>
        /// <returns>The Bitmap</returns>
        public Bitmap ToBitmap(Bitmap.Config config = null)
        {
            System.Drawing.Size size = Size;

            if (config == null)
                config = Bitmap.Config.Argb8888;

            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, config);
            ToBitmap(result);
            return result;

        }

        /// <summary>
        /// Convert the Mat to Bitmap
        /// </summary>
        /// <param name="bitmap">The bitmap, must be of the same size and has bitmap config type of either Argb888 or Rgb565</param>
        /// <returns>The Bitmap</returns>
        public void ToBitmap(Bitmap bitmap)
        {
            System.Drawing.Size size = Size;
            if (!(size.Width == bitmap.Width && size.Height == bitmap.Height))
            {
                throw new Exception("Bitmap size doesn't match the Mat size");
            }

            Bitmap.Config config = bitmap.GetConfig();
            if (config == Bitmap.Config.Argb8888)
            {
                int channels = NumberOfChannels;
                using (BitmapArgb8888Image bi = new BitmapArgb8888Image(bitmap))
                {
                    if (channels == 1)
                    {
                        CvInvoke.CvtColor(this, bi.Mat, ColorConversion.Gray2Rgba);
                    }
                    else if (channels == 3)
                    {
                        CvInvoke.CvtColor(this, bi, ColorConversion.Bgr2Rgba);
                    }
                    else if (channels == 4)
                    {
                        CvInvoke.CvtColor(this, bi, ColorConversion.Bgra2Rgba);
                    }
                    else
                    {
                        using (Image<Rgba, Byte> tmp = ToImage<Rgba, Byte>())
                        {
                            tmp.Copy(bi, null);
                        }

                    }
                }

            }
            else if (config == Bitmap.Config.Rgb565)
            {
                using (BitmapRgb565Image bi = new BitmapRgb565Image(bitmap))
                using (Image<Bgr, Byte> tmp = ToImage<Bgr, Byte>())
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