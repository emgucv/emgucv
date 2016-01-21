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
using Emgu.CV.Features2D;
using Emgu.CV.Reflection;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Android.Content;
using Android.Content.Res;

namespace Emgu.CV
{
   public partial class Image<TColor, TDepth>
      : CvArray<TDepth>, IImage, IEquatable<Image<TColor, TDepth>>
      where TColor : struct, IColor
      where TDepth : new()
   {
      public Image(AssetManager assets, String fileName)
      {
         using (Stream imageStream = assets.Open(fileName))
         using (Bitmap imageBmp = BitmapFactory.DecodeStream(imageStream))
            Bitmap = imageBmp;
      }

      public Bitmap ToBitmap(Bitmap.Config config)
      {
         System.Drawing.Size size = Size;

         if (config == Bitmap.Config.Argb8888)
         {
            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Argb8888);

            using (BitmapArgb8888Image bi = new BitmapArgb8888Image(result))
            {
               bi.ConvertFrom(this);
               //CvInvoke.cvSet(bi, new MCvScalar(0, 0, 255, 255), IntPtr.Zero);
            }
            return result;
         }
         else if (config == Bitmap.Config.Rgb565)
         {
            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Rgb565);

            using (BitmapRgb565Image bi = new BitmapRgb565Image(result))
               bi.ConvertFrom(this);
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