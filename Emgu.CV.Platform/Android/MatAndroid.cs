//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Android.Content.Res;

namespace Emgu.CV
{
    //public partial class Mat : MatDataAllocator, IInputArray, IOutputArray, IInputOutputArray
    public static partial class AndroidExtension
    {

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
        /// Create a Mat from an Android Bitmap
        /// </summary>
        /// <param name="bitmap">The android Bitmap</param>
        public static Mat ToMat(this Android.Graphics.Bitmap bitmap)
        {
            Mat m = new Mat();
            bitmap.ToMat(m);
            return m;
        }

        /// <summary>
        /// Convert a Bitmap to a Mat
        /// </summary>
        /// <param name="mat">The mat to copy Bitmap into</param>
        /// <param name="bitmap">The bitmap to copy into mat</param>
        public static void ToMat(this Android.Graphics.Bitmap bitmap, Mat mat)
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

    /// <summary>
    /// Class to read file into Mat using Android Bitmap
    /// </summary>
    public class BitmapFileReaderMat : Emgu.CV.IFileReaderMat
    {
        /// <summary>
        /// Read the file into a Mat
        /// </summary>
        /// <param name="fileName">The file to read from</param>
        /// <param name="mat">The Mat to read the file into</param>
        /// <param name="loadType">The load type</param>
        /// <returns>True if successfully read file into Mat</returns>
        public bool ReadFile(String fileName, Mat mat, CvEnum.ImreadModes loadType)
        {
            try
            {
                int rotation = 0;
                Android.Media.ExifInterface exif = new Android.Media.ExifInterface(fileName);
                int orientation = exif.GetAttributeInt(Android.Media.ExifInterface.TagOrientation, int.MinValue);
                switch (orientation)
                {
                    case (int)Android.Media.Orientation.Rotate270:
                        rotation = 270;
                        break;
                    case (int)Android.Media.Orientation.Rotate180:
                        rotation = 180;
                        break;
                    case (int)Android.Media.Orientation.Rotate90:
                        rotation = 90;
                        break;
                }

                using (Android.Graphics.Bitmap bmp = Android.Graphics.BitmapFactory.DecodeFile(fileName))
                {
                    if (rotation == 0)
                    {
                        bmp.ToMat(mat);
                    }
                    else
                    {
                        Android.Graphics.Matrix matrix = new Android.Graphics.Matrix();
                        matrix.PostRotate(rotation);
                        using (Android.Graphics.Bitmap rotated = Android.Graphics.Bitmap.CreateBitmap(bmp, 0, 0, bmp.Width, bmp.Height, matrix, true))
                        {
                            rotated.ToMat(mat);
                        }
                    }
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
    }

    /// <summary>
    /// Class that can be used to write the Mat into file using Android Bitmap
    /// </summary>
    public class BitmapFileWriterMat : Emgu.CV.IFileWriterMat
    {
        /// <summary>
        /// Write the Mat into file
        /// </summary>
        /// <param name="mat">The mat to be written</param>
        /// <param name="fileName">The name of the file</param>
        /// <returns>True if successfully written Mat into file</returns>
        public bool WriteFile(Mat mat, String fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                using (Android.Graphics.Bitmap bmp = mat.ToBitmap())
                using (FileStream fs = fileInfo.Open(FileMode.Append, FileAccess.Write))
                {
                    String extension = fileInfo.Extension.ToLower();
                    Debug.Assert(extension.Substring(0, 1).Equals("."));
                    switch (extension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Jpeg, 90, fs);
                            break;
                        case ".png":
                            bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 90, fs);
                            break;
                        case ".webp":
                            bmp.Compress(Android.Graphics.Bitmap.CompressFormat.Webp, 90, fs);
                            break;
                        default:
                            throw new NotImplementedException(String.Format("Saving to {0} format is not supported",
                                extension));
                    }
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
    }
}

#endif