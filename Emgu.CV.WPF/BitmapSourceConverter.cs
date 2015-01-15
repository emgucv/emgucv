//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Emgu.CV;

namespace Emgu.CV.WPF
{
   public static class BitmapSourceConvert
   {
      /// <summary>
      /// Delete a GDI object
      /// </summary>
      /// <param name="o">The poniter to the GDI object to be deleted</param>
      /// <returns></returns>
      [DllImport("gdi32")]
      private static extern int DeleteObject(IntPtr o);

      /// <summary>
      /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
      /// </summary>
      /// <param name="image">The Emgu CV Image</param>
      /// <returns>The equivalent BitmapSource</returns>
      public static BitmapSource ToBitmapSource(IImage image)
      {
         using (System.Drawing.Bitmap source = image.Bitmap)
         {
            IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr); //release the HBitmap
            return bs;
         }
      }
   }
}