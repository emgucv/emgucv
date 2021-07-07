//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{
    /// <summary>
    /// Class implementing BEBLID (Boosted Efficient Binary Local Image Descriptor)
    /// </summary>
    public class BEBLID : Feature2D
    {
        /// <summary>
        /// Beblid size
        /// </summary>
        public enum BeblidSize
        {
            /// <summary>
            /// 512 bit descriptor
            /// </summary>
            BitSize512 = 100,
            /// <summary>
            /// 256 bit descriptor
            /// </summary>
            BitSize256 = 101
        }

        /// <summary>
        /// Creates BEBLID (Boosted Efficient Binary Local Image Descriptor).
        /// </summary>
        /// <param name="scaleFactor">Adjust the sampling window around detected keypoints: 1.00f should be the scale for ORB keypoints; 6.75f should be the scale for SIFT detected keypoints; 6.25f is default and fits for KAZE, SURF detected keypoints; 5.00f should be the scale for AKAZE, MSD, AGAST, FAST, BRISK keypoints</param>
        /// <param name="size">Determine the number of bits in the descriptor. </param>
        public BEBLID(float scaleFactor, BeblidSize size)
        {
            _ptr = XFeatures2DInvoke.cveBEBLIDCreate(
                scaleFactor,
                size,
                ref _feature2D,
                ref _sharedPtr);
        }


        /// <summary>
        /// Release all the unmanaged resource associated with BEBLID
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XFeatures2DInvoke.cveBEBLIDRelease(ref _sharedPtr);
            }
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBEBLIDCreate(
            float scaleFactor,
            BEBLID.BeblidSize size,
            ref IntPtr beblid,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBEBLIDRelease(ref IntPtr shared);
    }
}