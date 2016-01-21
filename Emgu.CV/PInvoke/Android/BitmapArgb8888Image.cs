//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

using Emgu.CV.Structure;

namespace Emgu.CV
{
   /// <summary>
   /// An image that shares the data with Bitmap
   /// </summary>
   public class BitmapArgb8888Image : Image<Rgba, Byte>
   {
      private Bitmap _bmp;

      /// <summary>
      /// Get the bitmap that this object is sharing data with.
      /// </summary>
      public override Bitmap Bitmap
      {
         get
         {
            return _bmp;
         }
      }

      /// <summary>
      /// Create a Bgra Image of Bytes that shares data with Bitmap
      /// </summary>
      /// <param name="bmp">The Bitmap to create the BitmapImage from. The BitmapImage should always be disposed before this Bitmap is disposed.</param>
      public BitmapArgb8888Image(Bitmap bmp)
      {
         if (!bmp.GetConfig().Equals(Bitmap.Config.Argb8888))
            throw new NotImplementedException("Only Bitmap format of Argb8888 is supported for this class.");
         _bmp = bmp;
         MapDataToImage(bmp.Width, bmp.Height, bmp.RowBytes, _bmp.LockPixels());
      }

      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (!_bmp.IsRecycled)
            _bmp.UnlockPixels();
      }
   }

   public class BitmapRgb565Image : Image<Bgr565, Byte>
   {
      private Bitmap _bmp;

      public override Bitmap Bitmap
      {
         get
         {
            return _bmp;
         }
      }

      public BitmapRgb565Image(Bitmap bmp)
      {
         if (!bmp.GetConfig().Equals(Bitmap.Config.Rgb565))
            throw new NotImplementedException("Only Bitmap format of Rgb565 is supported for this class.");
         _bmp = bmp;
         MapDataToImage(bmp.Width, bmp.Height, bmp.RowBytes, _bmp.LockPixels());
      }

      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (!_bmp.IsRecycled)
            _bmp.UnlockPixels();
      }
   }
}

#endif