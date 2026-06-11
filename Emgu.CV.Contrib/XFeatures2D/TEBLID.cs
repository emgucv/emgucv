//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Features;

namespace Emgu.CV.XFeatures2D
{
    /// <summary>
    /// Class implementing TEBLID (Triplet-based Efficient Binary Local Image Descriptor)
    /// </summary>
    public class TEBLID : Feature2D
    {
        /// <summary>
        /// Teblid size
        /// </summary>
        public enum TeblidSize
        {
            /// <summary>
            /// 256 bit descriptor
            /// </summary>
            BitSize256 = 102,
            /// <summary>
            /// 512 bit descriptor
            /// </summary>
            BitSize512 = 103
        }

        /// <summary>
        /// Creates TEBLID (Triplet-based Efficient Binary Local Image Descriptor).
        /// </summary>
        /// <param name="scaleFactor">Adjust the sampling window around detected keypoints: 1.00f should be the scale for ORB keypoints; 6.75f should be the scale for SIFT detected keypoints; 6.25f is default and fits for KAZE, SURF detected keypoints; 5.00f should be the scale for AKAZE, MSD, AGAST, FAST, BRISK keypoints</param>
        /// <param name="size">Determine the number of bits in the descriptor.</param>
        public TEBLID(float scaleFactor, TeblidSize size = TeblidSize.BitSize256)
        {
            _ptr = XFeatures2DInvoke.cveTEBLIDCreate(
                scaleFactor,
                size,
                ref _feature2D,
                ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with TEBLID
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                XFeatures2DInvoke.cveTEBLIDRelease(ref _sharedPtr);
            }
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTEBLIDCreate(
            float scaleFactor,
            TEBLID.TeblidSize size,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTEBLIDRelease(ref IntPtr sharedPtr);
    }
}
