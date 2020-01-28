//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    /// Class implementing VGG (Oxford Visual Geometry Group) descriptor trained end to end using "Descriptor Learning Using Convex Optimisation" (DLCO) aparatus
    /// </summary>
    /// <remarks>See: K. Simonyan, A. Vedaldi, and A. Zisserman. Learning local feature descriptors using convex optimisation. IEEE Transactions on Pattern Analysis and Machine Intelligence, 2014.</remarks>
    public class VGG : Feature2D
    { 
        /// <summary>
        /// The VGG descriptor type
        /// </summary>
        public enum DescriptorType
        {
            /// <summary>
            /// 120 dimension float
            /// </summary>
            Vgg120 = 100,
            /// <summary>
            /// 80 dimension float
            /// </summary>
            Vgg80 = 101,
            /// <summary>
            /// 64 dimension float
            /// </summary>
            Vgg64 = 102,
            /// <summary>
            /// 48 dimension float
            /// </summary>
            Vgg48 = 103,
        }

        /// <summary>
        /// Create an instance of VGG
        /// </summary>
        /// <param name="desc">Type of descriptor to use</param>
        /// <param name="isigma">gaussian kernel value for image blur</param>
        /// <param name="imgNormalize">use image sample intensity normalization</param>
        /// <param name="useScaleOrientation">	sample patterns using keypoints orientation</param>
        /// <param name="scaleFactor">adjust the sampling window of detected keypoints to 64.0f (VGG sampling window) 6.25f is default and fits for KAZE, SURF detected keypoints window ratio 6.75f should be the scale for SIFT detected keypoints window ratio 5.00f should be the scale for AKAZE, MSD, AGAST, FAST, BRISK keypoints window ratio 0.75f should be the scale for ORB keypoints ratio</param>
        /// <param name="dscNormalize">clamp descriptors to 255 and convert to uchar CV_8UC1</param>
        public VGG(
            DescriptorType desc = DescriptorType.Vgg120, 
            float isigma = 1.4f,
            bool imgNormalize = true, 
            bool useScaleOrientation = true,
            float scaleFactor = 6.25f, 
            bool dscNormalize = false)
        {
            _ptr = XFeatures2DInvoke.cveVGGCreate(desc, isigma, imgNormalize, useScaleOrientation, scaleFactor, dscNormalize,
                ref _feature2D, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with VGG
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                XFeatures2DInvoke.cveVGGRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVGGCreate(
            VGG.DescriptorType desc, float isigma,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool imgNormalize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useScaleOrientation,
            float scaleFactor,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool dscNormalize, 
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVGGRelease(ref IntPtr sharedPtr);
    }
}

