//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

#if __UNIFIED__
using System;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using CoreImage;
using Emgu.CV.CvEnum;

namespace Emgu.CV
{
    /// <summary>
    /// Provide extension method to convert IInputArray from CIImage
    /// </summary>
    public static class CIImageExtension
    {
        /// <summary>
        /// Convert a CIImage to a IOutputArray
        /// </summary>
        /// <param name="ciImage">The source CIImage</param>
        /// <param name="mat">The destination array</param>
        /// <param name="modes">The color format for the destination array</param>
        /// <exception cref="NotImplementedException">Exception will be thrown if the ImreadModes is not supported.</exception>
        public static void ToArray(this CIImage ciImage, IOutputArray mat, ImreadModes modes = ImreadModes.AnyColor)
        {
            using (UIImage uiImage = new UIImage(ciImage))
            using (UIImage uiImage2 = uiImage.Scale(uiImage.Size)) //Scaling make a copy of the above UIImage (back by ci image) into a new UIImage (back by cg image)
            using (CGImage cgimage = uiImage2.CGImage)
            {
                cgimage.ToArray(mat, ImreadModes.ColorBgr);
            }

        }

    }
}
#endif