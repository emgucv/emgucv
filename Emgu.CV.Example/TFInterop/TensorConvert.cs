//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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
        /// Conver a 3 channel BGR Mat to a Tensor
        /// </summary>
        /// <param name="image">The input Emgu CV Mat</param>
        /// <param name="inputHeight">The height of the image in the output tensor, if it is -1, the height will not be changed.</param>
        /// <param name="inputWidth">The width of the image in the output tensor, if it is -1, the width will not be changed.</param>
        /// <param name="inputMean">The mean, if it is not 0, the value will be substracted from the pixel values</param>
        /// <param name="scale">The optional scale</param>
        /// <param name="status">The tensorflow status</param>
        /// <returns>The tensorflow tensor</returns>
        public static Tensor ReadTensorFromMatBgr(Mat image, int inputHeight = -1, int inputWidth = -1, float inputMean = 0.0f, float scale = 1.0f, Status status = null)
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

            if (image.Size != finalSize)
            {
                using (Mat tmp = new Mat())
                {
                    CvInvoke.Resize(image, tmp, finalSize);
                    return ReadTensorFromMatBgrF(tmp, inputMean, scale);
                }
            }
            else
            {
                return ReadTensorFromMatBgrF(image, inputMean, scale);
            }
        }

        private static Tensor ReadTensorFromMatBgrF(Mat image, float inputMean, float scale)
        {
            Tensor t = new Tensor(DataType.Float, new int[] { 1, image.Height, image.Width, 3 });
            using (Mat matF = new Mat(image.Size, Emgu.CV.CvEnum.DepthType.Cv32F, 3, t.DataPointer, sizeof(float) * 3 * image.Width))
            {
                image.ConvertTo(matF, Emgu.CV.CvEnum.DepthType.Cv32F, scale, -inputMean * scale);
            }
            return t;
        }

    }
}
