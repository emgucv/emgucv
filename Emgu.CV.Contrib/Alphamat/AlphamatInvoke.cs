//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;

namespace Emgu.CV.Alphamat
{
    /// <summary>
    /// Information Flow algorithm implementaton for alphamatting
    /// This module is dedicated to compute alpha matting of images, given the input image and an input trimap.
    /// </summary>
    public static partial class AlphamatInvoke
    {
        static AlphamatInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// The implementation is based on Designing Effective Inter-Pixel Information Flow for Natural Image Matting by Yağız Aksoy, Tunç Ozan Aydın and Marc Pollefeys, CVPR 2019.
        /// </summary>
        /// <param name="image">The input image</param>
        /// <param name="tmap">The trimap</param>
        /// <param name="result">The output mask</param>
        public static void InfoFlow(IInputArray image, IInputArray tmap, IOutputArray result)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaTmap = tmap.GetInputArray())
            using (OutputArray oaResult = result.GetOutputArray())
            {
                cveAlphamatInfoFlow(iaImage, iaTmap, oaResult);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAlphamatInfoFlow(IntPtr image, IntPtr tmap, IntPtr result);

    }
}
