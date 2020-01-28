//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

/*
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.LineDescriptor
{
    /// <summary>
    /// The lines extraction methodology described in the following is mainly based on: R Grompone Von Gioi, Jeremie Jakubowicz, Jean-Michel Morel, and Gregory Randall. Lsd: A fast line segment detector with a false detection control. IEEE Transactions on Pattern Analysis and Machine Intelligence, 32(4):722-732, 2010.
    /// </summary>
    public class LSDDetector : SharedPtrObject
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public LSDDetector()
        {
            _ptr = LineDescriptorInvoke.cveLineDescriptorLSDDetectorCreate(ref _sharedPtr);
        }

        /// <summary>
        /// Detect lines inside an image.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="keylines">Vector that will store extracted lines for one or more images</param>
        /// <param name="scale">Scale factor used in pyramids generation</param>
        /// <param name="numOctaves">Number of octaves inside pyramid</param>
        /// <param name="mask">Mask matrix to detect only KeyLines of interest</param>
        public void Detect(Mat image, VectorOfKeyLine keylines, int scale, int numOctaves, Mat mask = null)
        {
            LineDescriptorInvoke.cveLineDescriptorLSDDetectorDetect(_ptr, image, keylines, scale, numOctaves, mask);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                LineDescriptorInvoke.cveLineDescriptorLSDDetectorRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }
    }

    public static partial class LineDescriptorInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLineDescriptorLSDDetectorCreate(ref IntPtr sharedPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineDescriptorLSDDetectorDetect(IntPtr detector, IntPtr image, IntPtr keypoints, int scale, int numOctaves, IntPtr mask);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLineDescriptorLSDDetectorRelease(ref IntPtr sharedPtr);
    }

}*/