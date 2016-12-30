//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.CvEnum;

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

      public static Mat ToMat(BitmapSource source)
      {

         if (source.Format == PixelFormats.Bgra32)
         {
            Mat result = new Mat();
            result.Create(source.PixelHeight, source.PixelWidth, DepthType.Cv8U, 4);
            source.CopyPixels(Int32Rect.Empty, result.DataPointer, result.Step*result.Rows, result.Step);
            return result;
         } else if (source.Format == PixelFormats.Bgr24)
         {
            Mat result = new Mat();
            result.Create(source.PixelHeight, source.PixelWidth, DepthType.Cv8U, 3);
            source.CopyPixels(Int32Rect.Empty, result.DataPointer, result.Step * result.Rows, result.Step);
            return result;
         }
         else
         {
            throw new Exception(String.Format("Convertion from BitmapSource of format {0} is not supported.", source.Format));
         }
      }
   }
}