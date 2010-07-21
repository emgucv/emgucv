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
   public class BGCodeBookModel<TColor> : UnmanagedObject, IBGFGDetector<TColor>
      where TColor : struct, IColor
   {
      private Image<Gray, Byte> _foregroundMask;
      private Image<Gray, Byte> _backgroundMask;

      /// <summary>
      /// Create a background code book model
      /// </summary>
      public BGCodeBookModel()
      {
         _ptr = CvInvoke.cvCreateBGCodeBookModel();
      }

      /// <summary>
      /// Update the BG code book model
      /// </summary>
      /// <param name="image">The image for update</param>
      /// <param name="roi">The update roi, use Rectangle.Empty for the whole image</param>
      /// <param name="mask">Can be null if not needed. The update mask</param>
      public void Update(Image<TColor, Byte> image, Rectangle roi, Image<Gray, Byte> mask)
      {
         CvInvoke.cvBGCodeBookUpdate(_ptr, image.Ptr, roi, mask);
         if (_foregroundMask == null) _foregroundMask = new Image<Gray, byte>(image.Size);
         CvInvoke.cvBGCodeBookDiff(_ptr, image, _foregroundMask, roi);
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
      public Image<Gray, Byte> ForgroundMask
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
            CvInvoke.cvNot(_foregroundMask, _backgroundMask);
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
         CvInvoke.cvBGCodeBookClearStale(_ptr, staleThresh, roi, mask);
      }

      /// <summary>
      /// Release the unmanaged resource that is associated to this object
      /// </summary>
      protected override void DisposeObject()
      {
         CvInvoke.cvReleaseBGCodeBookModel(ref _ptr);
      }

      /// <summary>
      /// Release the managed resource
      /// </summary>
      protected override void ReleaseManagedResources()
      {
         if (_backgroundMask != null) _backgroundMask.Dispose();
         if (_foregroundMask != null) _foregroundMask.Dispose();
      }

      /// <summary>
      /// Get or Set the equivalent CVBGCodeBookModel structure
      /// </summary>
      public MCvBGCodeBookModel MCvBGCodeBookModel
      {
         get
         {
            return (MCvBGCodeBookModel)Marshal.PtrToStructure(Ptr, typeof(MCvBGCodeBookModel));
         }
         set
         {
            Marshal.StructureToPtr(value, _ptr, false);
         }
      }
   }
}
