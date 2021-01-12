//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __ANDROID__
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Emgu.CV.Cuda
{
    /// <summary>
    /// An CudaImage is very similar to the Emgu.CV.Image except that it is being used for GPU processing
    /// </summary>
    /// <typeparam name="TColor">Color type of this image (either Gray, Bgr, Bgra, Hsv, Hls, Lab, Luv, Xyz, Ycc, Rgb or Rbga)</typeparam>
    /// <typeparam name="TDepth">Depth of this image (either Byte, SByte, Single, double, UInt16, Int16 or Int32)</typeparam>
    public partial class CudaImage<TColor, TDepth>
       : GpuMat<TDepth>
       where TColor : struct, IColor
       where TDepth : new()
    {
        /// <summary>
        /// convert the current CudaImage to its equivalent Bitmap representation
        /// </summary>
        public new Android.Graphics.Bitmap Bitmap
        {
            get
            {
                using (Image<TColor, TDepth> tmp = ToImage())
                {
                    return tmp.ToBitmap();
                }
            }
        }
    }
}

#endif