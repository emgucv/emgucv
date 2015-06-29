//----------------------------------------------------------------------------
//  Copyright (C) 2004-2015 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
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

namespace Emgu.CV.Cuda
{
   /// <summary>
   /// An CudaImage is very similar to the Emgu.CV.Image except that it is being used for GPU processing
   /// </summary>
   /// <typeparam name="TColor">Color type of this image (either Gray, Bgr, Bgra, Hsv, Hls, Lab, Luv, Xyz, Ycc, Rgb or Rbga)</typeparam>
   /// <typeparam name="TDepth">Depth of this image (either Byte, SByte, Single, double, UInt16, Int16 or Int32)</typeparam>
   public class CudaImage<TColor, TDepth>
      : GpuMat<TDepth>, IImage
      where TColor : struct, IColor
      where TDepth : new()
   {
      #region constructors
      /// <summary>
      /// Create an empty CudaImage
      /// </summary>
      public CudaImage()
         : base()
      {
      }

      /// <summary>
      /// Create the CudaImage from the unmanaged pointer.
      /// </summary>
      /// <param name="ptr">The unmanaged pointer to the GpuMat. It is the user's responsibility that the Color type and depth matches between the managed class and unmanaged pointer.</param>
      internal CudaImage(IntPtr ptr, bool needDispose)
         : base(ptr, needDispose)
      {
      }

      /// <summary>
      /// Create a GPU image from a regular image
      /// </summary>
      /// <param name="img">The image to be converted to GPU image</param>
      public CudaImage(IInputArray img)
         : base(img)
      {
      }

      /// <summary>
      /// Create a CudaImage of the specific size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      /// <param name="continuous">Indicates if the data should be continuous</param>
      public CudaImage(int rows, int cols, bool continuous)
         : base(rows, cols, new TColor().Dimension, continuous)
      {
      }

      /// <summary>
      /// Create a CudaImage of the specific size
      /// </summary>
      /// <param name="rows">The number of rows (height)</param>
      /// <param name="cols">The number of columns (width)</param>
      public CudaImage(int rows, int cols)
         : base(rows, cols, new TColor().Dimension)
      {
      }

      /// <summary>
      /// Create a CudaImage of the specific size
      /// </summary>
      /// <param name="size">The size of the image</param>
      public CudaImage(Size size)
         : this(size.Height, size.Width)
      {
      }

      /// <summary>
      /// Create a CudaImage from the specific region of <paramref name="image"/>. The data is shared between the two CudaImage
      /// </summary>
      /// <param name="image">The CudaImage where the region is extracted from</param>
      /// <param name="colRange">The column range. Use MCvSlice.WholeSeq for all columns.</param>
      /// <param name="rowRange">The row range. Use MCvSlice.WholeSeq for all rows.</param>
      public CudaImage(CudaImage<TColor, TDepth> image, MCvSlice rowRange, MCvSlice colRange)
         :this(CudaInvoke.GetRegion(image, ref rowRange, ref colRange), true)
      {
      }
      #endregion


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

      ///<summary> Convert the current CudaImage to the specific color and depth </summary>
      ///<typeparam name="TOtherColor"> The type of color to be converted to </typeparam>
      ///<typeparam name="TOtherDepth"> The type of pixel depth to be converted to </typeparam>
      ///<returns>CudaImage of the specific color and depth </returns>
      public CudaImage<TOtherColor, TOtherDepth> Convert<TOtherColor, TOtherDepth>()
         where TOtherColor : struct, IColor
         where TOtherDepth : new()
      {
         CudaImage<TOtherColor, TOtherDepth> res = new CudaImage<TOtherColor, TOtherDepth>(Size);
         res.ConvertFrom(this);
         return res;
      }

      /// <summary>
      /// Convert the source image to the current image, if the size are different, the current image will be a resized version of the srcImage. 
      /// </summary>
      /// <typeparam name="TSrcColor">The color type of the source image</typeparam>
      /// <typeparam name="TSrcDepth">The color depth of the source image</typeparam>
      /// <param name="srcImage">The sourceImage</param>
      public void ConvertFrom<TSrcColor, TSrcDepth>(CudaImage<TSrcColor, TSrcDepth> srcImage)
         where TSrcColor : struct, IColor
         where TSrcDepth : new()
      {
         if (!Size.Equals(srcImage.Size))
         {  //if the size of the source image do not match the size of the current image
            using (CudaImage<TSrcColor, TSrcDepth> tmp = srcImage.Resize(Size, Emgu.CV.CvEnum.Inter.Linear, null))
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
               srcImage.CopyTo(this);
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
                  srcImage.ConvertTo(this, CvInvoke.GetDepthType(typeof(TDepth)), scale, shift, null);
                  
               } else
               {
                  srcImage.ConvertTo(this, CvInvoke.GetDepthType(typeof(TDepth)), 1.0, 0.0, null);
                  //CudaInvoke.ConvertTo(srcImage.Ptr, Ptr, 1.0, 0.0, IntPtr.Zero);
               }

            }
            #endregion
         } else
         {
            #region different color
            if (typeof(TDepth) == typeof(TSrcDepth))
            {   //same depth
               ConvertColor(srcImage, this, typeof(TSrcColor), typeof(TColor), NumberOfChannels, Size, null);
            } else
            {   //different depth
               using (CudaImage<TSrcColor, TDepth> tmp = srcImage.Convert<TSrcColor, TDepth>()) //convert depth
                  ConvertColor(tmp, this, typeof(TSrcColor), typeof(TColor), NumberOfChannels, Size, null);
            }
            #endregion
         }
      }

      private static void ConvertColor(IInputArray src, IOutputArray dest, Type srcColor, Type destColor, int dcn, Size size, Stream stream)
      {
         try
         {
            // if the direct conversion exist, apply the conversion
            CudaInvoke.CvtColor(src, dest, CvToolbox.GetColorCvtCode(srcColor, destColor), dcn, stream);
         } catch
         {
            try
            {
               //if a direct conversion doesn't exist, apply a two step conversion
               //in this case, needs to wait for the completion of the stream because a temporary local image buffer is used
               //we don't want the tmp image to be released before the operation is completed.
               using (CudaImage<Bgr, TDepth> tmp = new CudaImage<Bgr, TDepth>(size))
               {
                  CudaInvoke.CvtColor(src, tmp, CvToolbox.GetColorCvtCode(srcColor, typeof(Bgr)), 3, stream);
                  CudaInvoke.CvtColor(tmp, dest, CvToolbox.GetColorCvtCode(typeof(Bgr), destColor), dcn, stream);
                  stream.WaitForCompletion();
               }
            } catch (Exception excpt)
            {
               throw new NotSupportedException(String.Format(
                  "Conversion from CudaImage<{0}, {1}> to CudaImage<{2}, {3}> is not supported by OpenCV: {4}",
                  srcColor.ToString(),
                  typeof(TDepth).ToString(),
                  destColor.ToString(),
                  typeof(TDepth).ToString(),
                  excpt.Message));
            }
         }
      }

      /// <summary>
      /// Create a clone of this CudaImage
      /// </summary>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      /// <returns>A clone of this CudaImage</returns>
      public CudaImage<TColor, TDepth> Clone(Stream stream)
      {
         CudaImage<TColor, TDepth> result = new CudaImage<TColor, TDepth>(Size);
         CopyTo(result, null, stream);
         
         return result;
      }

      /// <summary>
      /// Resize the CudaImage. The calling GpuMat be GpuMat%lt;Byte&gt;. If stream is specified, it has to be either 1 or 4 channels.
      /// </summary>
      /// <param name="size">The new size</param>
      /// <param name="interpolationType">The interpolation type</param>
      /// <param name="stream">Use a Stream to call the function asynchronously (non-blocking) or null to call the function synchronously (blocking).</param>
      /// <returns>A CudaImage of the new size</returns>
      public CudaImage<TColor, TDepth> Resize(Size size, CvEnum.Inter interpolationType, Stream stream = null)
      {
         CudaImage<TColor, TDepth> result = new CudaImage<TColor, TDepth>(size);
         CudaInvoke.Resize(this, result, size, 0, 0, interpolationType, stream);
         return result;
      }

      /// <summary>
      /// Returns a CudaImage corresponding to a specified rectangle of the current CudaImage. The data is shared with the current matrix. In other words, it allows the user to treat a rectangular part of input array as a stand-alone array.
      /// </summary>
      /// <param name="region">Zero-based coordinates of the rectangle of interest.</param>
      /// <returns>A CudaImage that represent the region of the current CudaImage.</returns>
      /// <remarks>The parent CudaImage should never be released before the returned CudaImage that represent the subregion</remarks>
      public new CudaImage<TColor, TDepth> GetSubRect(Rectangle region)
      {
         return new CudaImage<TColor, TDepth>(CudaInvoke.GetSubRect(this, ref region), true);
      }

      /// <summary>
      /// Returns a CudaImage corresponding to the ith row of the CudaImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="i">The row to be extracted</param>
      /// <returns>The ith row of the CudaImage</returns>
      /// <remarks>The parent CudaImage should never be released before the returned CudaImage that represent the subregion</remarks>
      public new CudaImage<TColor, TDepth> Row(int i)
      {
         return RowRange(i, i + 1);
      }

      /// <summary>
      /// Returns a CudaImage corresponding to the [<paramref name="start"/> <paramref name="end"/>) rows of the CudaImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="start">The inclusive stating row to be extracted</param>
      /// <param name="end">The exclusive ending row to be extracted</param>
      /// <returns>The [<paramref name="start"/> <paramref name="end"/>) rows of the CudaImage</returns>
      /// <remarks>The parent CudaImage should never be released before the returned CudaImage that represent the subregion</remarks>
      public new CudaImage<TColor, TDepth> RowRange(int start, int end)
      {
         return new CudaImage<TColor, TDepth>(this, new MCvSlice(start, end), MCvSlice.WholeSeq);
      }

      /// <summary>
      /// Returns a CudaImage corresponding to the ith column of the CudaImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="i">The column to be extracted</param>
      /// <returns>The ith column of the CudaImage</returns>
      /// <remarks>The parent CudaImage should never be released before the returned CudaImage that represent the subregion</remarks>
      public new CudaImage<TColor, TDepth> Col(int i)
      {
         return ColRange(i, i + 1);
      }

      /// <summary>
      /// Returns a CudaImage corresponding to the [<paramref name="start"/> <paramref name="end"/>) columns of the CudaImage. The data is shared with the current Image. 
      /// </summary>
      /// <param name="start">The inclusive stating column to be extracted</param>
      /// <param name="end">The exclusive ending column to be extracted</param>
      /// <returns>The [<paramref name="start"/> <paramref name="end"/>) columns of the CudaImage</returns>
      /// <remarks>The parent CudaImage should never be released before the returned CudaImage that represent the subregion</remarks>
      public new CudaImage<TColor, TDepth> ColRange(int start, int end)
      {
         return new CudaImage<TColor, TDepth>(this, MCvSlice.WholeSeq, new MCvSlice(start, end));
      }

      #region IImage Members
#if IOS
      public MonoTouch.UIKit.UIImage ToUIImage()
      {
         throw new NotImplementedException();
      }
#elif !(NETFX_CORE || UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE || UNITY_METRO )
      /// <summary>
      /// convert the current CudaImage to its equivalent Bitmap representation
      /// </summary>
      public new Bitmap Bitmap
      {
         get
         {
#if !ANDROID
            if (typeof(TColor) == typeof(Bgr) && typeof(TDepth) == typeof(Byte))
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

      #endregion

      #region ICloneable Members

      object ICloneable.Clone()
      {
         return Clone(null);
      }

      #endregion
   }
}
