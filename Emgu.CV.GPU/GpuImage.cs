//----------------------------------------------------------------------------
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
#if ANDROID
using Bitmap = Android.Graphics.Bitmap;
#endif

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
      #region constructors
      /// <summary>
      /// Create an empty GpuImage
      /// </summary>
      public GpuImage()
         : base()
      {
      }

      /// <summary>
      /// Create the GpuImage from the unmanaged pointer.
      /// </summary>
      /// <param name="ptr">The unmanaged pointer to the GpuMat. It is the user's responsibility that the Color type and depth matches between the managed class and unmanaged pointer.</param>
      public GpuImage(IntPtr ptr)
         : base(ptr)
      {
      }

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
      /// <param name="continuous">Indicates if the data should be continuous</param>
      public GpuImage(int rows, int cols, bool continuous)
         : base(rows, cols, new TColor().Dimension, continuous)
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
      /// Create a GpuImage from the specific region of <paramref name="image"/>. The data is shared between the two GpuImage
      /// </summary>
      /// <param name="image">The GpuImage where the region is extracted from</param>
      /// <param name="colRange">The column range. Use MCvSlice.WholeSeq for all columns.</param>
      /// <param name="rowRange">The row range. Use MCvSlice.WholeSeq for all rows.</param>
      public GpuImage(GpuImage<TColor, TDepth> image, MCvSlice rowRange, MCvSlice colRange)
         :this(GpuInvoke.GpuMatGetRegion(image, rowRange, colRange))
      {
      }
      #endregion


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
            using (GpuImage<TSrcColor, TSrcDepth> tmp = srcImage.Resize(Size, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, null))
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
               GpuInvoke.Copy(srcImage.Ptr, Ptr, IntPtr.Zero, IntPtr.Zero);
            } else //different depth
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
               } else
               {
                  GpuInvoke.ConvertTo(srcImage.Ptr, Ptr, 1.0, 0.0, IntPtr.Zero);
               }

            }
            #endregion
         } else
         {
            #region different color
            if (typeof(TDepth) == typeof(TSrcDepth))
            {   //same depth
               ConvertColor(srcImage.Ptr, Ptr, typeof(TSrcColor), typeof(TColor), Size, null);
            } else
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
         } catch
         {
            try
            {
               //if a direct conversion doesn't exist, apply a two step conversion
               //in this case, needs to wait for the completion of the stream because a temporary local image buffer is used
               //we don't want the tmp image to be released before the operation is completed.
               using (GpuImage<Bgr, TDepth> tmp = new GpuImage<Bgr, TDepth>(size))
               {
                  GpuInvoke.CvtColor(src, tmp.Ptr, CvToolbox.GetColorCvtCode(srcColor, typeof(Bgr)), stream);
                  GpuInvoke.CvtColor(tmp.Ptr, dest, CvToolbox.GetColorCvtCode(typeof(Bgr), destColor), stream);
                  stream.WaitForCompletion();
               }
            } catch
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
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      /// <returns>A clone of this GpuImage</returns>
      public GpuImage<TColor, TDepth> Clone(Stream stream)
      {
         GpuImage<TColor, TDepth> result = new GpuImage<TColor, TDepth>(Size);
         GpuInvoke.Copy(_ptr, result, IntPtr.Zero, stream);
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
      /// Resize the GpuImage. The calling GpuMat be GpuMat%lt;Byte&gt;. If stream is specified, it has to be either 1 or 4 channels.
      /// </summary>
      /// <param name="size">The new size</param>
      /// <param name="interpolationType">The interpolation type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      /// <returns>A GpuImage of the new size</returns>
      public GpuImage<TColor, TDepth> Resize(Size size, CvEnum.INTER interpolationType, Stream stream)
      {
         GpuImage<TColor, TDepth> result = new GpuImage<TColor, TDepth>(size);
         GpuInvoke.Resize(_ptr, result, interpolationType, stream);
         return result;
      }

      ///<summary> 
      ///Performs a convolution using the specific <paramref name="kernel"/> 
      ///</summary>
      ///<param name="kernel">The convolution kernel</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      ///<returns>The result of the convolution</returns>
      public GpuImage<TColor, TDepth> Convolution(ConvolutionKernelF kernel, Stream stream)
      {
         GpuImage<TColor, TDepth> result = new GpuImage<TColor, TDepth>(Size);
         using (GpuLinearFilter<TColor, TDepth> linearFilter = new GpuLinearFilter<TColor, TDepth>(kernel, kernel.Center, CvEnum.BORDER_TYPE.REFLECT101, new MCvScalar()))
         {
            linearFilter.Apply(this, result, stream);
         }
         
         return result;
      }

      /// <summary>
      /// Returns a GpuImage corresponding to a specified rectangle of the current GpuImage. The data is shared with the current matrix. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array.
      /// </summary>
      /// <param name="region">Zero-based coordinates of the rectangle of interest.</param>
      /// <returns>A GpuImage that represent the region of the current GpuImage.</returns>
      /// <remarks>The parent GpuImage should never be released before the returned GpuImage that represent the subregion</remarks>
      public new GpuImage<TColor, TDepth> GetSubRect(Rectangle region)
      {
         return new GpuImage<TColor, TDepth>(GpuInvoke.GetSubRect(this, region));
      }

      /// <summary>
      /// Returns a GpuImage corresponding to the ith row of the GpuImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="i">The row to be extracted</param>
      /// <returns>The ith row of the GpuImage</returns>
      /// <remarks>The parent GpuImage should never be released before the returned GpuImage that represent the subregion</remarks>
      public new GpuImage<TColor, TDepth> Row(int i)
      {
         return RowRange(i, i + 1);
      }

      /// <summary>
      /// Returns a GpuImage corresponding to the [<paramref name="start"/> <paramref name="end"/>) rows of the GpuImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="start">The inclusive stating row to be extracted</param>
      /// <param name="end">The exclusive ending row to be extracted</param>
      /// <returns>The [<paramref name="start"/> <paramref name="end"/>) rows of the GpuImage</returns>
      /// <remarks>The parent GpuImage should never be released before the returned GpuImage that represent the subregion</remarks>
      public new GpuImage<TColor, TDepth> RowRange(int start, int end)
      {
         return new GpuImage<TColor, TDepth>(this, new MCvSlice(start, end), MCvSlice.WholeSeq);
      }

      /// <summary>
      /// Returns a GpuImage corresponding to the ith column of the GpuImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="i">The column to be extracted</param>
      /// <returns>The ith column of the GpuImage</returns>
      /// <remarks>The parent GpuImage should never be released before the returned GpuImage that represent the subregion</remarks>
      public new GpuImage<TColor, TDepth> Col(int i)
      {
         return ColRange(i, i + 1);
      }

      /// <summary>
      /// Returns a GpuImage corresponding to the [<paramref name="start"/> <paramref name="end"/>) columns of the GpuImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="start">The inclusive stating column to be extracted</param>
      /// <param name="end">The exclusive ending column to be extracted</param>
      /// <returns>The [<paramref name="start"/> <paramref name="end"/>) columns of the GpuImage</returns>
      /// <remarks>The parent GpuImage should never be released before the returned GpuImage that represent the subregion</remarks>
      public new GpuImage<TColor, TDepth> ColRange(int start, int end)
      {
         return new GpuImage<TColor, TDepth>(this, MCvSlice.WholeSeq, new MCvSlice(start, end));
      }

      #region IImage Members
#if IOS
      public MonoTouch.UIKit.UIImage ToUIImage()
      {
         throw new NotImplementedException();
      }
#elif !NETFX_CORE
      /// <summary>
      /// convert the current GpuImage to its equavalent Bitmap representation
      /// </summary>
      ///
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
            } else
#endif
               using (Image<TColor, TDepth> tmp = ToImage())
               {
                  return tmp.ToBitmap();
               }
         }
      }
#endif

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
         return Clone(null);
      }

      #endregion
   }
}
