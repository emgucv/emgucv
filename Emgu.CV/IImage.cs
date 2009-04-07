using System;
using System.Text;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// IImage interface
   /// </summary>
   public interface IImage : IDisposable, ICloneable
   {
      /// <summary>
      /// Convert this image into Bitmap, when avaialbe, data is shared with this image.
      /// </summary>
      /// <returns>The Bitmap, when avaialbe, data is shared with this image</returns>
      Bitmap Bitmap
      {
         get;
      }

      /// <summary>
      /// Convert this image into Bitmap, the data is always copied over.
      /// </summary>
      /// <returns>The Bitmap, the data is always copied over.</returns>
      Bitmap ToBitmap();

      /// <summary>
      /// The size of this image
      /// </summary>
      System.Drawing.Size Size
      {
         get;
      }

      /// <summary>
      /// Returns the min / max location and values for the image
      /// </summary>
      /// <returns>
      /// Returns the min / max location and values for the image
      /// </returns>
      void MinMax(out double[] minValues, out double[] maxValues, out System.Drawing.Point[] minLocations, out System.Drawing.Point[] maxLocations);

      ///<summary> 
      /// Split current IImage into an array of gray scale images where each element 
      /// in the array represent a single color channel of the original image
      ///</summary>
      ///<returns> 
      /// An array of gray scale images where each element 
      /// in the array represent a single color channel of the original image 
      ///</returns>
      IImage[] Split();

      /// <summary>
      /// Get the pointer to the unmanaged memory
      /// </summary>
      IntPtr Ptr
      {
         get;
      }

      /// <summary>
      /// Save the image to the specific <paramref name="fileName"/> 
      /// </summary>
      /// <param name="fileName">The file name of the image</param>
      void Save(String fileName);

      /// <summary>
      /// Resize the image
      /// </summary>
      /// <param name="width">The new width</param>
      /// <param name="height">The new height</param>
      /// <param name="interpolationType">The type of interpolation for resize</param>
      /// <param name="preserveScale">if true, the scale is preservered and the resulting image has maximum width(height) possible that is &lt;= <paramref name="width"/> (<paramref name="height"/>), if false, this function is equaivalent to Resize(int width, int height)</param>
      /// <returns>The resized image</returns>
      IImage Resize(int width, int height, CvEnum.INTER interpolationType, bool preserveScale);

      /// <summary>
      /// Make a copy of the specific ROI (Region of Interest) from the image
      /// </summary>
      /// <param name="roi">The roi to be copied</param>
      /// <returns>The roi region on the image</returns>
      IImage Copy(Rectangle roi);

      /// <summary>
      /// Get or Set the ROI for this IImage
      /// </summary>
      Rectangle ROI
      {
         get;
         set;
      }
   }
}
