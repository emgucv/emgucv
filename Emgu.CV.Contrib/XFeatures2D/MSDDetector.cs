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
    /// Class implementing the MSD (Maximal Self-Dissimilarity) keypoint detector, described in "Federico Tombari and Luigi Di Stefano. Interest points via maximal self-dissimilarities. In Asian Conference on Computer Vision - ACCV 2014, 2014".
    /// </summary>
    /// <remarks>The algorithm implements a novel interest point detector stemming from the intuition that image patches which are highly dissimilar over a relatively large extent of their surroundings hold the property of being repeatable and distinctive. This concept of "contextual self-dissimilarity" reverses the key paradigm of recent successful techniques such as the Local Self-Similarity descriptor and the Non-Local Means filter, which build upon the presence of similar - rather than dissimilar - patches. Moreover, it extends to contextual information the local self-dissimilarity notion embedded in established detectors of corner-like interest points, thereby achieving enhanced repeatability, distinctiveness and localization accuracy.</remarks>
    public class MSDDetector : Feature2D
    {
        /// <summary>
        /// Create a MSD (Maximal Self-Dissimilarity) keypoint detector.
        /// </summary>
        /// <param name="patchRadius">Patch radius</param>
        /// <param name="searchAreaRadius">Search area raduis</param>
        /// <param name="nmsRadius">Nms radius</param>
        /// <param name="nmsScaleRadius">Nms scale radius</param>
        /// <param name="thSaliency">Th saliency</param>
        /// <param name="kNN">Knn</param>
        /// <param name="scaleFactor">Scale factor</param>
        /// <param name="nScales">N scales</param>
        /// <param name="computeOrientation">Compute orientation</param>
        public MSDDetector(
            int patchRadius, 
            int searchAreaRadius,
            int nmsRadius, 
            int nmsScaleRadius, 
            float thSaliency, 
            int kNN,
            float scaleFactor, 
            int nScales, 
            bool computeOrientation)
        {
            _ptr = XFeatures2DInvoke.cveMSDDetectorCreate(
                patchRadius, 
                searchAreaRadius, 
                nmsRadius,
                nmsScaleRadius,
                thSaliency, 
                kNN,
                scaleFactor, 
                nScales, 
                computeOrientation,
                ref _feature2D, 
                ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with MSDDetector
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                XFeatures2DInvoke.cveMSDDetectorRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMSDDetectorCreate(
            int patchRadius,
            int searchAreaRadius,
            int nmsRadius,
            int nmsScaleRadius,
            float thSaliency,
            int kNN,
            float scaleFactor,
            int nScales,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool computeOrientation,
            ref IntPtr feature2D, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMSDDetectorRelease(ref IntPtr sharedPtr);
    }
}

