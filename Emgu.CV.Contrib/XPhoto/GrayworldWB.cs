//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV.XPhoto
{
    /// <summary>
    /// Gray-world white balance algorithm.
    /// This algorithm scales the values of pixels based on a gray-world assumption which states that the average of all channels should result in a gray image.
    /// It adds a modification which thresholds pixels based on their saturation value and only uses pixels below the provided threshold in finding average pixel values.
    /// Saturation is calculated using the following for a 3-channel RGB image per pixel I and is in the range [0, 1]:
    /// Saturation[I]= max(R,G,B)-min(R,G,B) / max(R,G,B)
    /// A threshold of 1 means that all pixels are used to white-balance, while a threshold of 0 means no pixels are used. Lower thresholds are useful in white-balancing saturated images.
    /// Currently supports images of type CV_8UC3 and CV_16UC3.
    /// </summary>
    public partial class GrayworldWB : WhiteBalancer
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Creates a gray-world white balancer
        /// </summary>
        public GrayworldWB()
        {
            _ptr = XPhotoInvoke.cveGrayworldWBCreate(ref _whiteBalancerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this white balancer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XPhotoInvoke.cveGrayworldWBRelease(ref _ptr, ref _sharedPtr);
            }
            base.DisposeObject();
        }
    }
}
