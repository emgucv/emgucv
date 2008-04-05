using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Emgu.CV
{
    /// <summary>
    /// The kernel that can be use as the parameter of the Convulution function in Image class
    /// </summary>
    public class ConvolutionKernelF : Matrix<float>
    {
        /// <summary>
        /// The center of the convolution kernel
        /// </summary>
        protected Point2D<int> _center;

        /// <summary>
        /// Create a convolution kernel of the specific rows and cols
        /// </summary>
        /// <param name="rows">The number of raws for the convolution kernel</param>
        /// <param name="cols">The number of columns for the convolution kernel</param>
        public ConvolutionKernelF(int rows, int cols)
            :base(rows, cols)
        {
            Debug.Assert( ! (rows <= 1 || cols <= 1) );
            _center = new Point2D<int>(-1, -1);
        }

        /// <summary>
        /// Create a convolution kernel using the specific matrix and center
        /// </summary>
        /// <param name="kernel">the values for the convolution kernel</param>
        /// <param name="center">the center of the kernel</param>
        public ConvolutionKernelF(Matrix<float> kernel, Point2D<int> center)
            : this(kernel.Data, center)
        {
        }

        /// <summary>
        /// Create a convolution kernel using the specific floating point matrix
        /// </summary>
        /// <param name="kernel">the values for the convolution kernel</param>
        public ConvolutionKernelF(float[][] kernel)
            : this(kernel, new Point2D<int>(-1, -1))
        {
        }

        /// <summary>
        /// Create a convolution kernel using the specific floating point matrix and center
        /// </summary>
        /// <param name="kernel">the values for the convolution kernel</param>
        /// <param name="center">the center for the convolution kernel</param>
        public ConvolutionKernelF(float[][] kernel, Point2D<int> center)
        {
            int rows = kernel.Length;
            int cols = kernel[0].Length;
            Debug.Assert(!(rows == 0 || cols == 0));

            if (rows == 1)
            {
                kernel = new float[2][] { kernel[0], new float[cols] };
                rows++;
            }

            if (cols == 1)
            {
                kernel = System.Array.ConvertAll<float[], float[]>(kernel, delegate(float[] fs) { return new float[2] { fs[0], 0.0f }; });
                cols++;
            }

            _ptr = CvInvoke.cvCreateMat(rows, cols, CvDepth);
            Emgu.Utils.CopyMatrix(kernel, CvMat.data);

            _center = center;
        }

        ///<summary> Return a filpped copy of the convolution kernel</summary>
        ///<param name="horizontal">if the kernel to be flipped horizontally</param>
        ///<param name="vertical">if the kernel to be flipped vertically</param>
        ///<returns> The flipped copy of <i>this</i> image </returns>
        public ConvolutionKernelF Flip(bool horizontal, bool vertical)
        {
            int code = 0;
            if (horizontal && !vertical) code = 1;
            else if (!horizontal && vertical) code = 0;
            else if (horizontal && vertical) code = -1;
            else
            {
                throw new Emgu.Exception(
                   Emgu.ExceptionHeader.CriticalException,
                   "Must Flip in at least one of the dimension");
            }

            ConvolutionKernelF res = new ConvolutionKernelF(Height, Width);
            CvInvoke.cvFlip(Ptr, res.Ptr, code);

            res.Center.X = ( Center.X == -1 ? -1 : ( horizontal ? Width - Center.X -1 : Center.X) );
            res.Center.Y = ( Center.Y == -1 ? -1 : ( vertical ? Height - Center.Y -1 : Center.Y));
            return res;
        }

        /// <summary>
        /// The center of the convolution kernel
        /// </summary>
        public Point2D<int> Center { get { return _center; } }

        /// <summary>
        /// Obtain the transpose of the convolution kernel
        /// </summary>
        /// <returns></returns>
        public new ConvolutionKernelF Transpose()
        {
            return new ConvolutionKernelF(
            base.Transpose(),
            new Point2D<int>(_center.Y, _center.X));
        }
    }
}
