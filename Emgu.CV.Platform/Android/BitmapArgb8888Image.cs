//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
      public Bitmap Bitmap
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

      /// <summary>
      /// Release the managed resources associated with this object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (!_bmp.IsRecycled)
            _bmp.UnlockPixels();
      }
   }

   /// <summary>
   /// An Image that shares data with the Bitmap
   /// </summary>
   public class BitmapRgb565Image : Image<Bgr565, Byte>
   {
      private Bitmap _bmp;

      /// <summary>
      /// Get the Bitmap object
      /// </summary>
      public Bitmap Bitmap
      {
         get
         {
            return _bmp;
         }
      }

      /// <summary>
      /// Create an image that shares data  with the Bitmap
      /// </summary>
      /// <param name="bmp">The bitmap object to create the image from</param>
      public BitmapRgb565Image(Bitmap bmp)
      {
         if (!bmp.GetConfig().Equals(Bitmap.Config.Rgb565))
            throw new NotImplementedException("Only Bitmap format of Rgb565 is supported for this class.");
         _bmp = bmp;
         MapDataToImage(bmp.Width, bmp.Height, bmp.RowBytes, _bmp.LockPixels());
      }

      /// <summary>
      /// Release the memory associated with this Image object
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         base.ReleaseManagedResources();
         if (!_bmp.IsRecycled)
            _bmp.UnlockPixels();
      }
   }
}

#endif