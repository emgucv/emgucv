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
    /// Class implementing BoostDesc (Learning Image Descriptors with Boosting).
    /// </summary>
    /// <remarks>
    /// See: 
    /// V. Lepetit T. Trzcinski, M. Christoudias and P. Fua. Boosting Binary Keypoint Descriptors. In Computer Vision and Pattern Recognition, 2013.
    /// M. Christoudias T. Trzcinski and V. Lepetit. Learning Image Descriptors with Boosting. submitted to IEEE Transactions on Pattern Analysis and Machine Intelligence (PAMI), 2013.
    /// </remarks>
    public class BoostDesc : Feature2D
    {
        /// <summary>
        /// The type of descriptor
        /// </summary>
        public enum DescriptorType
        {
            /// <summary>
            /// BGM is the base descriptor where each binary dimension is computed as the output of a single weak learner.
            /// </summary>
            Bgm = 100,
            /// <summary>
            /// BGM_HARD refers to same BGM but use different type of gradient binning. In the BGM_HARD that use ASSIGN_HARD binning type the gradient is assigned to the nearest orientation bin.
            /// </summary>
            BgmHard = 101,
            /// <summary>
            /// BGM_BILINEAR refers to same BGM but use different type of gradient binning. In the BGM_BILINEAR that use ASSIGN_BILINEAR binning type the gradient is assigned to the two neighbouring bins.
            /// </summary>
            BgmBilinear = 102,
            /// <summary>
            /// LBGM (alias FP-Boost) is the floating point extension where each dimension is computed as a linear combination of the weak learner responses.
            /// </summary>
            Lbgm = 200,
            /// <summary>
            /// BINBOOST and subvariants are the binary extensions of LBGM where each bit is computed as a thresholded linear combination of a set of weak learners.
            /// </summary>
            Binboost64 = 300,
            /// <summary>
            /// BINBOOST and subvariants are the binary extensions of LBGM where each bit is computed as a thresholded linear combination of a set of weak learners.
            /// </summary>
            Binboost128 = 301,
            /// <summary>
            /// BINBOOST and subvariants are the binary extensions of LBGM where each bit is computed as a thresholded linear combination of a set of weak learners.
            /// </summary>
            Binboost256 = 302
        }

        /// <summary>
        /// Create an instance of Boost Descriptor
        /// </summary>
        /// <param name="desc">type of descriptor to use</param>
        /// <param name="useScaleOrientation">sample patterns using keypoints orientation</param>
        /// <param name="scalefactor">adjust the sampling window of detected keypoints 6.25f is default and fits for KAZE, SURF detected keypoints window ratio 6.75f should be the scale for SIFT detected keypoints window ratio 5.00f should be the scale for AKAZE, MSD, AGAST, FAST, BRISK keypoints window ratio 0.75f should be the scale for ORB keypoints ratio 1.50f was the default in original implementation</param>
        public BoostDesc(
            DescriptorType desc = DescriptorType.Binboost256, 
            bool useScaleOrientation = true, 
            float scalefactor = 6.25f)
        {
            _ptr = XFeatures2DInvoke.cveBoostDescCreate(desc, useScaleOrientation, scalefactor, ref _feature2D, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with BRIEF
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                XFeatures2DInvoke.cveBoostDescRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBoostDescCreate(
            BoostDesc.DescriptorType desc,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useScaleOrientation,
            float scalefactor,
            ref IntPtr feature2D, 
            ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBoostDescRelease(ref IntPtr sharedPtr);
    }
}

