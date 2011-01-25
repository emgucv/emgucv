using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.GPU
{
   /// <summary>
   /// An GpuImage is very similar to the Emgu.CV.Image except that it is being used for GPU processing
   /// </summary>
   /// <typeparam name="TColor">Color type of this image (either Gray, Bgr, Bgra, Hsv, Hls, Lab, Luv, Xyz, Ycc, Rgb or Rbga)</typeparam>
   /// <typeparam name="TDepth">Depth of this image (either Byte, SByte, Single, double, UInt16, Int16 or Int32)</typeparam>
   public class GpuImage<TColor, TDepth>
      : GpuMat<TDepth>, IImage
      where TColor : struct, IColor
      where TDepth : new()
   {
      /// <summary>
      /// Create a GPU image from a regular image
      /// </summary>
      /// <param name="img">The image to be converted to GPU image</param>
      public GpuImage(Image<TColor, TDepth> img)
         : base(img)
      {
      }

      /// <summary>
      /// Create a GpuImage of the specific size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      public GpuImage(int rows, int cols)
         : base(rows, cols, new TColor().Dimension)
      {
      }

      /// <summary>
      /// Create a GpuImage of the specific size
      /// </summary>
      /// <param name="size">The size of the image</param>
      public GpuImage(Size size)
         : this(size.Height, size.Width)
      {
      }

      /// <summary>
      /// Convert the current GpuImage to a regular Image.
      /// </summary>
      /// <returns>A regular image</returns>
      public Image<TColor, TDepth> ToImage()
      {
         Image<TColor, TDepth> img = new Image<TColor, TDepth>(Size);
         Download(img);
         return img;
      }

      ///<summary> Convert the current GpuImage to the specific color and depth </summary>
      ///<typeparam name="TOtherColor"> The type of color to be converted to </typeparam>
      ///<typeparam name="TOtherDepth"> The type of pixel depth to be converted to </typeparam>
      ///<returns>GpuImage of the specific color and depth </returns>
      public GpuImage<TOtherColor, TOtherDepth> Convert<TOtherColor, TOtherDepth>()
         where TOtherColor : struct, IColor
         where TOtherDepth : new()
      {
         GpuImage<TOtherColor, TOtherDepth> res = new GpuImage<TOtherColor, TOtherDepth>(Size);
         res.ConvertFrom(this);
         return res;
      }

      /// <summary>
      /// Convert the source image to the current image, if the size are different, the current image will be a resized version of the srcImage. 
      /// </summary>
      /// <typeparam name="TSrcColor">The color type of the source image</typeparam>
      /// <typeparam name="TSrcDepth">The color depth of the source image</typeparam>
      /// <param name="srcImage">The sourceImage</param>
      public void ConvertFrom<TSrcColor, TSrcDepth>(GpuImage<TSrcColor, TSrcDepth> srcImage)
         where TSrcColor : struct, IColor
         where TSrcDepth : new()
      {
         if (!Size.Equals(srcImage.Size))
         {  //if the size of the source image do not match the size of the current image
            using (GpuImage<TSrcColor, TSrcDepth> tmp = srcImage.Resize(Size, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR))
            {
               ConvertFrom(tmp);
               return;
            }
         }

         if (typeof(TColor) == typeof(TSrcColor))
         {
            #region same color
            if (typeof(TDepth) == typeof(TSrcDepth)) //same depth
            {   
               GpuInvoke.Copy(srcImage.Ptr, Ptr, IntPtr.Zero);
            }
            else //different depth
            {
               if (typeof(TDepth) == typeof(Byte) && typeof(TSrcDepth) != typeof(Byte))
               {
                  double[] minVal, maxVal;
                  Point[] minLoc, maxLoc;
                  srcImage.MinMax(out minVal, out maxVal, out minLoc, out maxLoc);
                  double min = minVal[0];
                  double max = maxVal[0];
                  for (int i = 1; i < minVal.Length; i++)
                  {
                     min = Math.Min(min, minVal[i]);
                     max = Math.Max(max, maxVal[i]);
                  }
                  double scale = 1.0, shift = 0.0;
                  if (max > 255.0 || min < 0)
                  {
                     scale = (max == min) ? 0.0 : 255.0 / (max - min);
                     shift = (scale == 0) ? min : -min * scale;
                  }

                  GpuInvoke.ConvertTo(srcImage.Ptr, Ptr, scale, shift, IntPtr.Zero);
               }
               else
               {
                  GpuInvoke.ConvertTo(srcImage.Ptr, Ptr, 1.0, 0.0, IntPtr.Zero);
               }

            }
            #endregion
         }
         else
         {
            #region different color
            if (typeof(TDepth) == typeof(TSrcDepth))
            {   //same depth
               ConvertColor(srcImage.Ptr, Ptr, typeof(TSrcColor), typeof(TColor), Size, null);
            }
            else
            {   //different depth
               using (GpuImage<TSrcColor, TDepth> tmp = srcImage.Convert<TSrcColor, TDepth>()) //convert depth
                  ConvertColor(tmp.Ptr, Ptr, typeof(TSrcColor), typeof(TColor), Size, null);
            }
            #endregion
         }
      }

      private static void ConvertColor(IntPtr src, IntPtr dest, Type srcColor, Type destColor, Size size, Stream stream)
      {
         try
         {
            // if the direct conversion exist, apply the conversion
            GpuInvoke.CvtColor(src, dest, CvToolbox.GetColorCvtCode(srcColor, destColor), stream);
         }
         catch
         {
            try
            {
               //if a direct conversion doesn't exist, apply a two step conversion
               using (GpuImage<Bgr, TDepth> tmp = new GpuImage<Bgr, TDepth>(size))
               {
                  GpuInvoke.CvtColor(src, tmp.Ptr, CvToolbox.GetColorCvtCode(srcColor, typeof(Bgr)), stream);
                  GpuInvoke.CvtColor(tmp.Ptr, dest, CvToolbox.GetColorCvtCode(typeof(Bgr), destColor), stream);
               }
            }
            catch
            {
               throw new NotSupportedException(String.Format(
                  "Convertion from Image<{0}, {1}> to Image<{2}, {3}> is not supported by OpenCV",
                  srcColor.ToString(),
                  typeof(TDepth).ToString(),
                  destColor.ToString(),
                  typeof(TDepth).ToString()));
            }
         }
      }

      /// <summary>
      /// Create a clone of this GpuImage
      /// </summary>
      /// <returns>A clone of this GpuImage</returns>
      public GpuImage<TColor, TDepth> Clone()
      {
         GpuImage<TColor, TDepth> result = new GpuImage<TColor, TDepth>(Size);
         GpuInvoke.Copy(_ptr, result, IntPtr.Zero);
         return result;
      }

      ///<summary> 
      ///Split current Image into an array of gray scale images where each element 
      ///in the array represent a single color channel of the original image
      ///</summary>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      ///<returns> 
      ///An array of gray scale images where each element  
      ///in the array represent a single color channel of the original image 
      ///</returns>
      public new GpuImage<Gray, TDepth>[] Split(Stream stream)
      {
         GpuImage<Gray, TDepth>[] result = new GpuImage<Gray, TDepth>[NumberOfChannels];
         Size size = Size;
         for (int i = 0; i < result.Length; i++)
         {
            result[i] = new GpuImage<Gray, TDepth>(size);
         }

         SplitInto(result, stream);
         return result;
      }

      /// <summary>
      /// Resize the GpuImage. 
      /// Only TDepth of Byte is supported.
      /// </summary>
      /// <param name="size">The new size</param>
      /// <param name="interpolationType">The interpolation type</param>
      /// <returns>A GpuImage of the new size</returns>
      public GpuImage<TColor, TDepth> Resize(Size size, CvEnum.INTER interpolationType)
      {
         GpuImage<TColor, TDepth> result = new GpuImage<TColor, TDepth>(size);
         GpuInvoke.Resize(_ptr, result, interpolationType);
         return result;
      }

      ///<summary> 
      ///Performs a convolution using the specific <paramref name="kernel"/> 
      ///</summary>
      ///<param name="kernel">The convolution kernel</param>
      ///<returns>The result of the convolution</returns>
      public GpuImage<TColor, Single> Convolution(ConvolutionKernelF kernel)
      {
         GpuImage<TColor, Single> result = new GpuImage<TColor, float>(Size);
         GpuInvoke.Filter2D(_ptr, result, kernel, kernel.Center);
         return result;
      }

      #region IImage Members
      /// <summary>
      /// convert the current GpuImage to its equavalent Bitmap representation
      /// </summary>
      public Bitmap Bitmap
      {
         get
         {
            using (Image<TColor, TDepth> tmp = ToImage())
            {
               return tmp.ToBitmap();
            }
         }
      }

      IImage[] IImage.Split()
      {
         return Split(null);
      }

      /// <summary>
      /// Saving the GPU image to file
      /// </summary>
      /// <param name="fileName">The file to be saved to</param>
      public void Save(string fileName)
      {
         using (Image<TColor, TDepth> tmp = ToImage())
         {
            tmp.Save(fileName);
         }
      }

      #endregion

      #region ICloneable Members

      object ICloneable.Clone()
      {
         return Clone();
      }

      #endregion
   }
}
