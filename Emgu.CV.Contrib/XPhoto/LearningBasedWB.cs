//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.CvEnum;
using Emgu.Util;

namespace Emgu.CV.XPhoto
{
    /// <summary>
    /// More sophisticated learning-based automatic white balance algorithm.
    /// As GrayworldWB, this algorithm works by applying different gains to the input image channels, but their computation is a bit more involved compared to the simple gray-world assumption. 
    /// More details about the algorithm can be found in: Dongliang Cheng, Brian Price, Scott Cohen, and Michael S Brown. Effective learning-based illuminant estimation using simple features. In Proceedings of the IEEE Conference on Computer Vision and Pattern Recognition, pages 1000-1008, 2015.
    /// To mask out saturated pixels this function uses only pixels that satisfy the following condition:
    /// max(R,G,B) / range_max_val &lt; saturation_thresh 
    /// Currently supports images of type CV_8UC3 and CV_16UC3.
    /// </summary>
    public partial class LearningBasedWB : WhiteBalancer
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a learning based white balancer.
        /// </summary>
        public LearningBasedWB()
        {
            _ptr = XPhotoInvoke.cveLearningBasedWBCreate(ref _whiteBalancerPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this white balancer
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                XPhotoInvoke.cveLearningBasedWBRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
            base.DisposeObject();
        }
    }

    public static partial class XPhotoInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLearningBasedWBCreate(ref IntPtr whiteBalancer, ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLearningBasedWBRelease(ref IntPtr sharedPtr);
    }
}
