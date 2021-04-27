//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using Emgu.CV.CvEnum;
using Emgu.CV.ML;
using UnityEngine;
using System;
using System.Drawing;
using System.Collections;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    /// <summary>
    /// Convert between Texture2d object and Image Object
    /// </summary>
    public static class TextureConvert
    {

        /// <summary>
        /// Convert Texture2d to OutputArray
        /// </summary>
        /// <param name="texture">The unity Texture2D object</param>
        /// <param name="result">The output array (3 channel - BGR) where the results will be written to.</param>
        /// <param name="flipType">Optional Flip type</param>
        /// <param name="dstColorType">Destination color type, if null, will use typeof(Bgr)</param>
        public static void ToOutputArray(this Texture2D texture, IOutputArray result, FlipType flipType = FlipType.Vertical, Type dstColorType = null)
        {
            int width = texture.width;
            int height = texture.height;
            try
            {
                Color32[] colors = texture.GetPixels32();
                GCHandle handle = GCHandle.Alloc(colors, GCHandleType.Pinned);

                using (Mat rgba = new Mat(height, width, DepthType.Cv8U, 4, handle.AddrOfPinnedObject(), width * 4))
                {
                    Type dct = dstColorType == null ? typeof(Bgr) : dstColorType;
                    CvInvoke.CvtColor(rgba, result, typeof(Rgba), dct);
                    if (flipType != FlipType.None)
                    {
                        CvInvoke.Flip(result, result, flipType);
                    }
                }
                handle.Free();
            }
            catch (Exception excpt)
            {
                if (texture.format == TextureFormat.ARGB32 || texture.format == TextureFormat.RGB24)
                {
                    byte[] jpgBytes = texture.EncodeToJPG();
                    using (Mat tmp = new Mat())
                    {
                        CvInvoke.Imdecode(jpgBytes, ImreadModes.Color, tmp);
                        if (dstColorType == null || dstColorType == typeof(Bgr))
                            tmp.CopyTo(result);
                        else
                        {
                            CvInvoke.CvtColor(tmp, result, typeof(Bgr), dstColorType);
                        }
                    }
                }
                else
                {
                    throw new Exception(String.Format("We are not able to handle Texture format of {0} type", texture.format), excpt);
                }
            }
        }

        /// <summary>
        /// Convert an input array to texture 2D
        /// </summary>
        /// <param name="image">The input image, if 3 channel, we assume it is Bgr, if 4 channels, we assume it is Bgra</param>
        /// <param name="flipType"></param>
        /// <param name="buffer">Optional buffer for the texture conversion, should be big enough to hold the image data. e.g. width*height*pixel_size</param>
        /// <returns>The texture 2D</returns>
        public static Texture2D ToTexture2D(this IInputArray image, Emgu.CV.CvEnum.FlipType flipType = FlipType.Vertical, byte[] buffer = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            {
                Size size = iaImage.GetSize();

                if (iaImage.GetChannels() == 3 && iaImage.GetDepth() == DepthType.Cv8U && SystemInfo.SupportsTextureFormat(TextureFormat.RGB24))
                {
                    //assuming 3 channel image is of BGR color
                    Texture2D texture = new Texture2D(size.Width, size.Height, TextureFormat.RGB24, false);

                    byte[] data;
                    int bufferLength = size.Width * size.Height * 3;
                    if (buffer != null)
                    {
                        if (buffer.Length < bufferLength)
                            throw new ArgumentException(String.Format("Buffer size ({0}) is not big enough for the RBG24 texture, width * height * 3 = {1} is required.", buffer.Length, bufferLength));
                        data = buffer;
                    }
                    else
                        data = new byte[bufferLength];
                    GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    using (Mat rgb = new Mat(size, DepthType.Cv8U, 3, dataHandle.AddrOfPinnedObject(), size.Width * 3))
                    {
                        CvInvoke.CvtColor(image, rgb, ColorConversion.Bgr2Rgb);
                        if (flipType != FlipType.None)
                            CvInvoke.Flip(rgb, rgb, flipType);
                    }

                    dataHandle.Free();
                    texture.LoadRawTextureData(data);
                    texture.Apply();
                    return texture;
                }
                else if (SystemInfo.SupportsTextureFormat(TextureFormat.RGBA32))
                {
                    Texture2D texture = new Texture2D(size.Width, size.Height, TextureFormat.RGBA32, false);
                    byte[] data;
                    int bufferLength = size.Width * size.Height * 4;
                    if (buffer != null)
                    {
                        if (buffer.Length < bufferLength)
                            throw new ArgumentException(
                               String.Format(
                                  "Buffer size ({0}) is not big enough for the RGBA32 texture, width * height * 4 = {1} is required.",
                                  buffer.Length, bufferLength));
                        data = buffer;
                    }
                    else
                        data = new byte[bufferLength];
                    GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    using (Mat rgba = new Mat(size, DepthType.Cv8U, 4, dataHandle.AddrOfPinnedObject(), size.Width * 4))
                    {
                        CvInvoke.CvtColor(image, rgba, ColorConversion.Bgr2Rgb);
                        if (flipType != FlipType.None)
                            CvInvoke.Flip(rgba, rgba, flipType);
                    }

                    dataHandle.Free();
                    texture.LoadRawTextureData(data);

                    texture.Apply();
                    return texture;
                }
                else
                {
                    throw new Exception("TextureFormat of neither RBG24 nor RGBA32 are supported on this device.");
                }
            }
        }
    }
}