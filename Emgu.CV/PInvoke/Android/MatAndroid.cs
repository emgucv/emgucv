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
   public partial class Mat : MatDataAllocator, IInputArray, IOutputArray, IInputOutputArray, IImage
   {

      public Bitmap Bitmap
      {
         get
         {
            Size size = Size;
            Bitmap result = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Argb8888);

            using (BitmapArgb8888Image bi = new BitmapArgb8888Image(result))
            using (Image<Rgba, Byte> tmp = ToImage<Rgba, Byte>())
            {
               t...
            }
            return result;
         }
      }
   }
}