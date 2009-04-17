using System;
using System.Collections.Generic;
using System.Text;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;

namespace Emgu.CV.VideoSurveillance
{
   /// <summary>
   /// Background code book model
   /// </summary>
   /// <typeparam name="TColor"> The type of color for the image</typeparam>
   public class BGCodeBookModel<TColor> : UnmanagedObject where TColor : struct, IColor
   {
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
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="image"></param>
      /// <param name="fgmask"></param>
      /// <param name="roi"></param>
      /// <returns></returns>
      public int Diff(Image<TColor, Byte> image, Image<Gray, Byte> fgmask, Rectangle roi)
      {
         return CvInvoke.cvBGCodeBookDiff(_ptr, image, fgmask, roi);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="staleThresh"></param>
      /// <param name="roi"></param>
      /// <param name="mask"></param>
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
   }
}
