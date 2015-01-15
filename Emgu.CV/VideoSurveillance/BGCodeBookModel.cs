/*
//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// Background code book model
   /// </summary>
   /// <typeparam name="TColor"> The type of color for the image</typeparam>
   public class BGCodeBookModel<TColor> : DisposableObject
      where TColor : struct, IColor
   {
      /// <summary>
      /// The CvBGCodeBookModel structure
      /// </summary>
      public MCvBGCodeBookModel MCvBGCodeBookModel;

      private MemStorage _storage;

      private Image<Gray, Byte> _foregroundMask;
      private Image<Gray, Byte> _backgroundMask;

      /// <summary>
      /// Create a background code book model
      /// </summary>
      public BGCodeBookModel()
      {
         _storage = new MemStorage();
         MCvBGCodeBookModel.Storage = _storage.Ptr;
         MCvBGCodeBookModel.CbBounds0 = MCvBGCodeBookModel.CbBounds1 = MCvBGCodeBookModel.CbBounds2 = 10;
         MCvBGCodeBookModel.ModMin0 = 3;
         MCvBGCodeBookModel.ModMax0 = 10;
         MCvBGCodeBookModel.ModMin1 = MCvBGCodeBookModel.ModMin2 = 1;
         MCvBGCodeBookModel.ModMax1 = MCvBGCodeBookModel.ModMax2 = 1;
      }

      /// <summary>
      /// Update the BG code book model
      /// </summary>
      /// <param name="image">The image for update</param>
      /// <param name="roi">The update roi, use Rectangle.Empty for the whole image</param>
      /// <param name="mask">Can be null if not needed. The update mask</param>
      public void Update(Image<TColor, Byte> image, Rectangle roi, Image<Gray, Byte> mask)
      {
         CvInvoke.cvBGCodeBookUpdate(ref MCvBGCodeBookModel, image.Ptr, roi, mask);
      }

      /// <summary>
      /// Find the forground by codebook method. The result will be stored in the ForgroundMask property
      /// </summary>
      /// <param name="image">The image to run diff against</param>
      /// <param name="roi">The region of interest. Use Rectangle.Empty for the whole region</param>
      public int Diff(Image<TColor, Byte> image, Rectangle roi)
      {
         if (_foregroundMask == null) _foregroundMask = new Image<Gray, byte>(image.Size);
         return CvInvoke.cvBGCodeBookDiff(ref MCvBGCodeBookModel, image, _foregroundMask, roi);
      }

      /// <summary>
      /// Update the BG code book model
      /// </summary>
      /// <param name="image">The image for update</param>
      public void Update(Image<TColor, Byte> image)
      {
         Update(image, Rectangle.Empty, null);
      }

      /// <summary>
      /// Get the foreground mask. Do not dispose this image.
      /// </summary>
      public Image<Gray, Byte> ForegroundMask
      {
         get
         {
            return _foregroundMask;
         }
      }

      /// <summary>
      /// Get the background mask. Do not dispose this image.
      /// </summary>
      public Image<Gray, Byte> BackgroundMask
      {
         get
         {
            if (_foregroundMask == null) return null;
            if (_backgroundMask == null) _backgroundMask = new Image<Gray, byte>(_foregroundMask.Size);
            CvInvoke.BitwiseNot(_foregroundMask, _backgroundMask, null);
            return _backgroundMask;
         }
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="staleThresh"></param>
      /// <param name="roi">The region of interest. Use Rectangle.Empty for the whole image</param>
      /// <param name="mask">Mask for Clear Stale. Can be null if not needed.</param>
      public void ClearStale(int staleThresh, Rectangle roi, Image<Gray, Byte> mask)
      {
         CvInvoke.cvBGCodeBookClearStale(ref MCvBGCodeBookModel, staleThresh, roi, mask);
      }

      /// <summary>
      /// Release the unmanaged resource that is associated to this object
      /// </summary>
      protected override void DisposeObject()
      {
      }

      /// <summary>
      /// Release the managed resource
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         if (_backgroundMask != null) _backgroundMask.Dispose();
         if (_foregroundMask != null) _foregroundMask.Dispose();
         _storage.Dispose();
      }

   }
}
*/