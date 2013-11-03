//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;
#if ANDROID
using Bitmap = Android.Graphics.Bitmap;
#endif

namespace Emgu.CV.OpenCL
{
   /// <summary>
   /// An OclImage is very similar to the Emgu.CV.Image except that it is being used for OpenCL processing
   /// </summary>
   /// <typeparam name="TColor">Color type of this image (either Gray, Bgr, Bgra, Hsv, Hls, Lab, Luv, Xyz, Ycc, Rgb or Rbga)</typeparam>
   /// <typeparam name="TDepth">Depth of this image (either Byte, SByte, Single, double, UInt16, Int16 or Int32)</typeparam>
   public class OclImage<TColor, TDepth>
      : OclMat<TDepth>, IImage
      where TColor : struct, IColor
      where TDepth : new()
   {
      #region constructors
      /// <summary>
      /// Create an empty CudaImage
      /// </summary>
      public OclImage()
         : base()
      {
      }

      /// <summary>
      /// Create the CudaImage from the unmanaged pointer.
      /// </summary>
      /// <param name="ptr">The unmanaged pointer to the GpuMat. It is the user's responsibility that the Color type and depth matches between the managed class and unmanaged pointer.</param>
      public OclImage(IntPtr ptr)
         : base(ptr)
      {
      }

      
      /// <summary>
      /// Create a GPU image from a regular image
      /// </summary>
      /// <param name="img">The image to be converted to GPU image</param>
      public OclImage(Image<TColor, TDepth> img)
         : base(img)
      {
      }

      /*
      /// <summary>
      /// Create a OclImage of the specific size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="continuous">Indicates if the data should be continuous</param>
      public OclImage(int rows, int cols, bool continuous)
         : base(rows, cols, new TColor().Dimension, continuous)
      {
      }*/

      /// <summary>
      /// Create an OclImage of the specific size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      public OclImage(int rows, int cols)
         : base(rows, cols, new TColor().Dimension)
      {
      }

      /// <summary>
      /// Create a CudaImage of the specific size
      /// </summary>
      /// <param name="size">The size of the image</param>
      public OclImage(Size size)
         : this(size.Height, size.Width)
      {
      }

      /// <summary>
      /// Create an OclImage from the specific region of <paramref name="image"/>. The data is shared between the two OclImage
      /// </summary>
      /// <param name="image">The OclImage where the region is extracted from</param>
      /// <param name="colRange">The column range. Use MCvSlice.WholeSeq for all columns.</param>
      /// <param name="rowRange">The row range. Use MCvSlice.WholeSeq for all rows.</param>
      public OclImage(OclImage<TColor, TDepth> image, MCvSlice rowRange, MCvSlice colRange)
         :this(OclInvoke.GetRegion(image, ref rowRange, ref colRange))
      {
      }
      #endregion

      /// <summary>
      /// Resize the OclImage. The calling OclMat be OclMat%lt;Byte&gt;. 
      /// </summary>
      /// <param name="size">The new size</param>
      /// <param name="interpolationType">The interpolation type</param>
      /// <returns>An OclImage of the new size</returns>
      public OclImage<TColor, TDepth> Resize(Size size, CvEnum.INTER interpolationType)
      {
         OclImage<TColor, TDepth> result = new OclImage<TColor, TDepth>(size);
         OclInvoke.Resize(_ptr, result, 0, 0, interpolationType);
         return result;
      }

      ///<summary> 
      ///Split current Image into an array of gray scale images where each element 
      ///in the array represent a single color channel of the original image
      ///</summary>
      ///<returns> 
      ///An array of gray scale images where each element  
      ///in the array represent a single color channel of the original image 
      ///</returns>
      public new OclImage<Gray, TDepth>[] Split()
      {
         OclImage<Gray, TDepth>[] result = new OclImage<Gray, TDepth>[NumberOfChannels];
         Size size = Size;
         for (int i = 0; i < result.Length; i++)
         {
            result[i] = new OclImage<Gray, TDepth>(size);
         }

         SplitInto(result);
         return result;
      }

      /// <summary>
      /// Returns an OclImage corresponding to a specified rectangle of the current OclImage. The data is shared with the current matrix. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array.
      /// </summary>
      /// <param name="region">Zero-based coordinates of the rectangle of interest.</param>
      /// <returns>An OclImage that represent the region of the current OclImage.</returns>
      /// <remarks>The parent OclImage should never be released before the returned OclImage that represent the subregion</remarks>
      public new OclImage<TColor, TDepth> GetSubRect(Rectangle region)
      {
         return new OclImage<TColor, TDepth>(OclInvoke.GetSubRect(this, ref region));
      }
      
      ///<summary> Convert the current CudaImage to the specific color and depth </summary>
      ///<typeparam name="TOtherColor"> The type of color to be converted to </typeparam>
      ///<typeparam name="TOtherDepth"> The type of pixel depth to be converted to </typeparam>
      ///<returns>CudaImage of the specific color and depth </returns>
      public OclImage<TOtherColor, TOtherDepth> Convert<TOtherColor, TOtherDepth>()
         where TOtherColor : struct, IColor
         where TOtherDepth : new()
      {
         OclImage<TOtherColor, TOtherDepth> res = new OclImage<TOtherColor, TOtherDepth>(Size);
         res.ConvertFrom(this);
         return res;
      }

      /// <summary>
      /// Convert the source image to the current image, if the size are different, the current image will be a resized version of the srcImage. 
      /// </summary>
      /// <typeparam name="TSrcColor">The color type of the source image</typeparam>
      /// <typeparam name="TSrcDepth">The color depth of the source image</typeparam>
      /// <param name="srcImage">The sourceImage</param>
      public void ConvertFrom<TSrcColor, TSrcDepth>(OclImage<TSrcColor, TSrcDepth> srcImage)
         where TSrcColor : struct, IColor
         where TSrcDepth : new()
      {
         if (!Size.Equals(srcImage.Size))
         {  //if the size of the source image do not match the size of the current image
            using (OclImage<TSrcColor, TSrcDepth> tmp = srcImage.Resize(Size, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR))
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
               OclInvoke.Copy(srcImage.Ptr, Ptr, IntPtr.Zero);
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

                  OclInvoke.ConvertTo(srcImage.Ptr, Ptr, scale, shift);
               }
               else
               {
                  OclInvoke.ConvertTo(srcImage.Ptr, Ptr, 1.0, 0.0);
               }

            }
            #endregion
         }
         else
         {
            #region different color
            if (typeof(TDepth) == typeof(TSrcDepth))
            {   //same depth
               ConvertColor(srcImage.Ptr, Ptr, typeof(TSrcColor), typeof(TColor), Size);
            }
            else
            {   //different depth
               using (OclImage<TSrcColor, TDepth> tmp = srcImage.Convert<TSrcColor, TDepth>()) //convert depth
                  ConvertColor(tmp.Ptr, Ptr, typeof(TSrcColor), typeof(TColor), Size);
            }
            #endregion
         }
      }

      /// <summary>
      /// Convert the current CudaImage to a regular Image.
      /// </summary>
      /// <returns>A regular image</returns>
      public Image<TColor, TDepth> ToImage()
      {
         Image<TColor, TDepth> img = new Image<TColor, TDepth>(Size);
         Download(img);
         return img;
      }

      ///<summary> 
      ///Performs a convolution using the specific <paramref name="kernel"/> 
      ///</summary>
      ///<param name="kernel">The convolution kernel</param>
      ///<returns>The result of the convolution</returns>
      public OclImage<TColor, Byte> Convolution(ConvolutionKernelF kernel)
      {
         OclImage<TColor, Byte> result = new OclImage<TColor, Byte>(Size);
         OclInvoke.Filter2D(_ptr, result, kernel, kernel.Center, CvEnum.BORDER_TYPE.REFLECT101);
         return result;
      }

      private static void ConvertColor(IntPtr src, IntPtr dest, Type srcColor, Type destColor, Size size)
      {
         try
         {
            // if the direct conversion exist, apply the conversion
            OclInvoke.CvtColor(src, dest, CvToolbox.GetColorCvtCode(srcColor, destColor));
         }
         catch
         {
            try
            {
               //if a direct conversion doesn't exist, apply a two step conversion
               //in this case, needs to wait for the completion of the stream because a temporary local image buffer is used
               //we don't want the tmp image to be released before the operation is completed.
               using (OclImage<Bgr, TDepth> tmp = new OclImage<Bgr, TDepth>(size))
               {
                  OclInvoke.CvtColor(src, tmp.Ptr, CvToolbox.GetColorCvtCode(srcColor, typeof(Bgr)));
                  OclInvoke.CvtColor(tmp.Ptr, dest, CvToolbox.GetColorCvtCode(typeof(Bgr), destColor));
               }
            }
            catch
            {
               throw new NotSupportedException(String.Format(
                  "Convertion from OclImage<{0}, {1}> to OclImage<{2}, {3}> is not supported by OpenCV",
                  srcColor.ToString(),
                  typeof(TDepth).ToString(),
                  destColor.ToString(),
                  typeof(TDepth).ToString()));
            }
         }
      }

      /// <summary>
      /// Create a clone of this OclImage
      /// </summary>
      /// <returns>A clone of this CudaImage</returns>
      public OclImage<TColor, TDepth> Clone()
      {
         OclImage<TColor, TDepth> result = new OclImage<TColor, TDepth>(Size);
         OclInvoke.Copy(_ptr, result, IntPtr.Zero);
         return result;
      }

#if !NETFX_CORE
      /// <summary>
      /// Get the Bitmap representation of this IclImage
      /// </summary>
      public Bitmap Bitmap
      {
         get
         {
#if !ANDROID
            if (this is Image<Bgr, Byte>)
            {
               Size s = Size;
               Bitmap result = new Bitmap(s.Width, s.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
               System.Drawing.Imaging.BitmapData data = result.LockBits(new Rectangle(Point.Empty, result.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, result.PixelFormat);
               using (Image<TColor, TDepth> tmp = new Image<TColor, TDepth>(s.Width, s.Height, data.Stride, data.Scan0))
               {
                  Download(tmp);
               }
               result.UnlockBits(data);
               return result;
            }
            else
#endif
               using (Image<TColor, TDepth> tmp = ToImage())
               {
                  return tmp.ToBitmap();
               }
         }  
      }
#endif

      /// <summary>
      /// Split up the image channels.
      /// </summary>
      /// <returns>An array of image channels</returns>
      IImage[] IImage.Split()
      {
         return Split();
      }

      /// <summary>
      /// Save the OclImage to file
      /// </summary>
      /// <param name="fileName">The fileName</param>
      public void Save(string fileName)
      {
         using (Image<TColor, TDepth> tmp = ToImage())
         {
            tmp.Save(fileName);
         }
      }

      /// <summary>
      /// Create a clone of the current OclImage
      /// </summary>
      /// <returns></returns>
      object ICloneable.Clone()
      {
         return Clone();
      }
   }
}
