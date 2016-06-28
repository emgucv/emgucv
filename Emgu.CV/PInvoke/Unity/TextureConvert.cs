//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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
   public static class TextureConvert
   {
      /// <summary>
      /// Convert Texture2d object to Image Object
      /// </summary>
      /// <typeparam name="TColor">The color type of the Image</typeparam>
      /// <typeparam name="TDepth">The depth type of the Image</typeparam>
      /// <param name="texture">The unity Texture2D</param>
      /// <param name="flipType">The optional flip when performing the conversion</param>
      /// <returns>The resulting Image object</returns>
      public static Image<TColor, TDepth> Texture2dToImage<TColor, TDepth>(Texture2D texture, Emgu.CV.CvEnum.FlipType flipType = FlipType.Vertical)
         where TColor : struct, IColor
         where TDepth : new()
      {
         int width = texture.width;
         int height = texture.height;

         Image<TColor, TDepth> result = new Image<TColor, TDepth>(width, height);
         try
         {
            Color32[] colors = texture.GetPixels32();
            GCHandle handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
            using (Image<Rgba, Byte> rgba = new Image<Rgba, byte>(width, height, width * 4, handle.AddrOfPinnedObject()))
            {
               result.ConvertFrom(rgba);
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
                  CvInvoke.Imdecode(jpgBytes, ImreadModes.AnyColor, tmp);
                  result.ConvertFrom(tmp);
               }
            }
            else
            {
               throw new Exception(String.Format("Texture format of {0} cannot be converted to Image<,> type", texture.format), excpt);
            }
         }
         if (flipType != FlipType.None)
            CvInvoke.Flip(result, result, flipType);
         return result;
      }

      /// <summary>
      /// Convert Image to Texture2D
      /// </summary>
      /// <typeparam name="TColor">The color type of the image</typeparam>
      /// <typeparam name="TDepth">The depth of the image</typeparam>
      /// <param name="image">The image object to be converted</param>
      /// <param name="flipType">The optional flip when performing the convertion</param>
      /// <returns>The Unity Texture2D object</returns>
      public static Texture2D ImageToTexture2D<TColor, TDepth>(Image<TColor, TDepth> image, Emgu.CV.CvEnum.FlipType flipType = FlipType.Vertical)
         where TColor : struct, IColor
         where TDepth : new()
      {
         Size size = image.Size;

         if (typeof(TColor) == typeof(Rgb) && typeof(TDepth) == typeof(Byte) && SystemInfo.SupportsTextureFormat(TextureFormat.RGB24))
         {
            Texture2D texture = new Texture2D(size.Width, size.Height, TextureFormat.RGB24, false);
            byte[] data = new byte[size.Width * size.Height * 3];
            GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (Image<Rgb, byte> rgb = new Image<Rgb, byte>(size.Width, size.Height, size.Width * 3, dataHandle.AddrOfPinnedObject()))
            {
               rgb.ConvertFrom(image);
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
            byte[] data = new byte[size.Width * size.Height * 4];
            GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            using (Image<Rgba, byte> rgba = new Image<Rgba, byte>(size.Width, size.Height, size.Width * 4, dataHandle.AddrOfPinnedObject()))
            {
               rgba.ConvertFrom(image);
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
            throw new Exception("TextureFormat of RGBA32 is not supported on this device");
         }
      }

      /// <summary>
      /// Convert Texture2d to OutputArray
      /// </summary>
      /// <param name="texture">The unity Texture2D object</param>
      /// <param name="result">The output array where the results will be written to</param>
      public static void Texture2dToOutputArray(Texture2D texture, IOutputArray result)
      {
         int width = texture.width;
         int height = texture.height;
         try
         {
            Color32[] colors = texture.GetPixels32();
            GCHandle handle = GCHandle.Alloc(colors, GCHandleType.Pinned);

            using (Mat rgba = new Mat(height, width, DepthType.Cv8U, 4, handle.AddrOfPinnedObject(), width * 4))
            {
               rgba.CopyTo(result);
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
                  CvInvoke.Imdecode(jpgBytes, ImreadModes.AnyColor, tmp);
                  tmp.CopyTo(result);
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
      public static Texture2D InputArrayToTexture2D(IInputArray image, Emgu.CV.CvEnum.FlipType flipType = FlipType.Vertical, byte[] buffer = null)
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
               using (
                  Image<Rgb, byte> rgb = new Image<Rgb, byte>(size.Width, size.Height, size.Width * 3,
                     dataHandle.AddrOfPinnedObject()))
               {
                  rgb.ConvertFrom(image);
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
               using (
                  Image<Rgba, byte> rgba = new Image<Rgba, byte>(size.Width, size.Height, size.Width * 4,
                     dataHandle.AddrOfPinnedObject()))
               {
                  rgba.ConvertFrom(image);
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
               throw new Exception("TextureFormat of RGBA32 is not supported on this device");
            }
         }
      }
   }
}