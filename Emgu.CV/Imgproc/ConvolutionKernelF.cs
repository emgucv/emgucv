//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;

namespace Emgu.CV
{
   /// <summary>
   /// A convolution kernel 
   /// </summary>
   public class ConvolutionKernelF : Matrix<float>
   {
      /// <summary>
      /// The center of the convolution kernel
      /// </summary>
      protected Point _center;

      /// <summary>
      /// Create a convolution kernel with the specific number of <paramref name="rows"/> and <paramref name="cols"/>
      /// </summary>
      /// <param name="rows">The number of raws for the convolution kernel</param>
      /// <param name="cols">The number of columns for the convolution kernel</param>
      public ConvolutionKernelF(int rows, int cols)
         : base(rows, cols)
      {
         Debug.Assert(!(rows <= 1 || cols <= 1));
         _center = new Point(-1, -1);
      }

      /// <summary>
      /// Create a convolution kernel using the specific matrix and center
      /// </summary>
      /// <param name="kernel">The values for the convolution kernel</param>
      /// <param name="center">The center of the kernel</param>
      public ConvolutionKernelF(Matrix<float> kernel, System.Drawing.Point center)
         : this(kernel.Data, center)
      {
      }

      /// <summary>
      /// Create a convolution kernel using the specific floating point matrix
      /// </summary>
      /// <param name="kernel">The values for the convolution kernel</param>
      public ConvolutionKernelF(float[,] kernel)
         : this(kernel, new Point(-1, -1))
      {
      }

      /// <summary>
      /// Create a convolution kernel using the specific floating point matrix and center
      /// </summary>
      /// <param name="kernel">The values for the convolution kernel</param>
      /// <param name="center">The center for the convolution kernel</param>
      public ConvolutionKernelF(float[,] kernel, System.Drawing.Point center)
         : base(Math.Max(2, kernel.GetLength(0)), Math.Max(2, kernel.GetLength(1)))
      {
         int rows = kernel.GetLength(0);
         int cols = kernel.GetLength(1);
         Debug.Assert(!(rows == 0 || cols == 0));

         float[,] data = Data;
         for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
               data[i, j] = kernel[i, j];

         _center = center;
      }

      /// <summary> Get a flipped copy of the convolution kernel</summary>
      /// <param name="flipType">The type of the flipping</param>
      /// <returns> The flipped copy of <i>this</i> image </returns>
      public ConvolutionKernelF Flip(CvEnum.FlipType flipType)
      {
         ConvolutionKernelF res = new ConvolutionKernelF(Height, Width);
         CvInvoke.Flip(this, res, flipType);

         res.Center = new System.Drawing.Point(
          (Center.X == -1 ? -1 : ((flipType & Emgu.CV.CvEnum.FlipType.Horizontal) == Emgu.CV.CvEnum.FlipType.Horizontal ? Width - Center.X - 1 : Center.X)),
          (Center.Y == -1 ? -1 : ((flipType & Emgu.CV.CvEnum.FlipType.Vertical) == Emgu.CV.CvEnum.FlipType.Vertical ? Height - Center.Y - 1 : Center.Y)));
         return res;
      }

      /// <summary>
      /// The center of the convolution kernel
      /// </summary>
      public Point Center
      {
         get { return _center; }
         set { _center = value; }
      }

      /// <summary>
      /// Obtain the transpose of the convolution kernel
      /// </summary>
      /// <returns>A transposed convolution kernel</returns>
      public new ConvolutionKernelF Transpose()
      {
         return new ConvolutionKernelF(
         base.Transpose(),
         new Point(_center.Y, _center.X));
      }
   }
}
