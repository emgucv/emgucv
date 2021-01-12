//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Emgu.CV;
using System.Drawing;

namespace Emgu.TF
{
    public static class TensorConvert
    {
        /// <summary>
        /// Convert a 3 channel BGR Mat to a Tensor
        /// </summary>
        /// <param name="image">The input Emgu CV Mat</param>
        /// <param name="inputHeight">The height of the image in the output tensor, if it is -1, the height will not be changed.</param>
        /// <param name="inputWidth">The width of the image in the output tensor, if it is -1, the width will not be changed.</param>
        /// <param name="inputMean">The mean, if it is not 0, the value will be subtracted from the pixel values</param>
        /// <param name="scale">The optional scale</param>
        /// <param name="dataType">The type of the data in the tensor</param>
        /// <param name="status">The tensorflow status</param>
        /// <returns>The tensorflow tensor</returns>
        public static Tensor ReadTensorFromMatBgr(Mat image, DataType dataType, int inputHeight = -1, int inputWidth = -1, float inputMean = 0.0f, float scale = 1.0f,  Status status = null)
        {
            int finalHeight = inputHeight == -1 ? image.Height : inputHeight;
            int finalWidth = inputWidth == -1 ? image.Width : inputWidth;
            Size finalSize = new Size(finalWidth, finalHeight);
            Tensor t = new Tensor(dataType, new int[] { 1, finalHeight, finalWidth, 3 });
            ReadTensorFromMatBgr(image, t, inputHeight, inputWidth, inputMean, scale, status);
            return t;
        }

        /// <summary>
        /// Convert a 3 channel BGR Mat to a Tensor
        /// </summary>
        /// <param name="image">The input Emgu CV Mat</param>
        /// <param name="tensor">The pre-allocated output tensor. Dimension must match (1, inputHeight, inputWidth, 3)</param>
        /// <param name="inputHeight">The height of the image in the output tensor, if it is -1, the height will not be changed.</param>
        /// <param name="inputWidth">The width of the image in the output tensor, if it is -1, the width will not be changed.</param>
        /// <param name="inputMean">The mean, if it is not 0, the value will be subtracted from the pixel values</param>
        /// <param name="scale">The optional scale</param>
        /// <param name="status">The tensorflow status</param>
        /// <returns>The tensorflow tensor</returns>
        public static void ReadTensorFromMatBgr(Mat image, Tensor tensor, int inputHeight = -1, int inputWidth = -1, float inputMean = 0.0f, float scale = 1.0f, Status status = null)
        {
            if (image.NumberOfChannels != 3)
            {
                throw new ArgumentException("Input must be 3 channel BGR image");
            }

            Emgu.CV.CvEnum.DepthType depth = image.Depth;
            if (!(depth == Emgu.CV.CvEnum.DepthType.Cv8U || depth == Emgu.CV.CvEnum.DepthType.Cv32F))
            {
                throw new ArgumentException("Input image must be 8U or 32F");
            }

            //resize
            int finalHeight = inputHeight == -1 ? image.Height : inputHeight;
            int finalWidth = inputWidth == -1 ? image.Width : inputWidth;
            Size finalSize = new Size(finalWidth, finalHeight);

            int[] dim = tensor.Dim;
            if (dim[0] != 1)
                throw new ArgumentException("First dimension of the tensor must be 1");
            
            if (dim[1] != finalHeight)
                throw new ArgumentException("Second dimension of the tensor must match the input height");

            if (dim[2] != finalWidth)
                throw new ArgumentException("Third dimension of the tensor must match the input width");

            if (dim[3] != 3)
                throw new ArgumentException("Fourth dimension of the tensor must be 3");

            if (image.Size != finalSize)
            {
                using (Mat tmp = new Mat())
                {
                    CvInvoke.Resize(image, tmp, finalSize);
                    ReadTensorFromMatBgr(tmp, inputMean, scale, tensor);
                }
            }
            else
            {
                ReadTensorFromMatBgr(image, inputMean, scale, tensor);
            }
        }

        private static void ReadTensorFromMatBgr(Mat image, float inputMean, float scale, Tensor t)
        {
            DataType type = t.Type;
            if (type == DataType.Float)
            {
                using (Mat matF = new Mat(image.Size, Emgu.CV.CvEnum.DepthType.Cv32F, 3, t.DataPointer, sizeof(float) * 3 * image.Width))
                {
                    image.ConvertTo(matF, Emgu.CV.CvEnum.DepthType.Cv32F, scale, -inputMean * scale);
                }
            } else if (type == DataType.Uint8)
            {
                using (Mat matB = new Mat(image.Size, Emgu.CV.CvEnum.DepthType.Cv8U, 3, t.DataPointer, sizeof(byte) * 3 * image.Width))
                {
                    if (scale == 1.0f && inputMean == 0)
                    {
                        image.CopyTo(matB);
                    } else
                        CvInvoke.ConvertScaleAbs(image, matB, scale, -inputMean * scale);
                }
            } else
            {
                throw new Exception(String.Format("Data Type of {0} is not supported.", type));
            }
        }

    }
}
