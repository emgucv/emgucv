//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
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
      public Mat(AssetManager assets, String fileName)
         : this()
      {
         using (Stream imageStream = assets.Open(fileName))
         using (MemoryStream ms = new MemoryStream())
         {
            imageStream.CopyTo(ms);
            CvInvoke.Imdecode(ms.ToArray(), LoadImageType.AnyColor | LoadImageType.AnyDepth, this);
         }
      }

      public Bitmap Bitmap
      {
         get { return ToBitmap(Android.Graphics.Bitmap.Config.Argb8888); }
      }

      public Bitmap ToBitmap(Bitmap.Config config)
      {
         System.Drawing.Size size = Size;

         if (config == Bitmap.Config.Argb8888)
         {
            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Argb8888);

            using (BitmapArgb8888Image bi = new BitmapArgb8888Image(result))
            using (Image<Rgba, Byte> tmp = ToImage<Rgba, Byte>())
            {
               tmp.Copy(bi, null);
            }
            return result;
         }
         else if (config == Bitmap.Config.Rgb565)
         {
            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Rgb565);

            using (BitmapRgb565Image bi = new BitmapRgb565Image(result))
            using (Image<Bgr, Byte> tmp = ToImage<Bgr, Byte>())
               bi.ConvertFrom(tmp);
            return result;
         }
         else
         {
            throw new NotImplementedException("Only Bitmap config of Argb888 or Rgb565 is supported.");
         }
      }
   }
}

#endif