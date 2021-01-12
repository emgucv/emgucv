//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;

namespace Emgu.CV.CvEnum
{
    /// <summary>
    /// Interpolation type
    /// </summary>
    public enum SmoothType
    {
        /// <summary>
        /// (simple blur with no scaling) - summation over a pixel param1 x param2 neighborhood. If the neighborhood size may vary, one may precompute integral image with cvIntegral function
        /// </summary>
        BlurNoScale = 0,
        /// <summary>
        /// (simple blur) - summation over a pixel param1 x param2 neighborhood with subsequent scaling by 1/(param1 x param2). 
        /// </summary>
        Blur = 1,
        /// <summary>
        /// (Gaussian blur) - convolving image with param1 x param2 Gaussian kernel. 
        /// </summary>
        Gaussian = 2,
        /// <summary>
        /// (median blur) - finding median of param1 x param1 neighborhood (i.e. the neighborhood is square). 
        /// </summary>
        Median = 3,
        /// <summary>
        /// (bilateral filter) - applying bilateral 3x3 filtering with color sigma=param1 and space sigma=param2. Information about bilateral filtering can be found 
        /// </summary>
        Bilateral = 4
    }

}
