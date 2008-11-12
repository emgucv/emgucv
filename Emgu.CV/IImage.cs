using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Emgu.CV.Reflection;

namespace Emgu.CV
{
   /// <summary>
   /// IImage interface
   /// </summary>
   public interface IImage : IDisposable, ICloneable
   {
      /// <summary>
      /// Convert this image into Bitmap 
      /// </summary>
      /// <returns></returns>
      Bitmap Bitmap
      {
         get;
      }

      /// <summary>
      /// The width of this image
      /// </summary>
      int Width
      {
         get;
      }

      /// <summary>
      /// The height of this image
      /// </summary>
      int Height
      {
         get;
      }

      /// <summary>
      /// Returns the min / max location and values for the image
      /// </summary>
      /// <returns>
      /// Returns the min / max location and values for the image
      /// </returns>
      void MinMax(out double[] minValues, out double[] maxValues, out MCvPoint[] minLocations, out MCvPoint[] maxLocations);

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
      
   }
}
