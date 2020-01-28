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
    /// Class implementing the MSD (Maximal Self-Dissimilarity) keypoint detector, described in "Federico Tombari and Luigi Di Stefano. Interest points via maximal self-dissimilarities. In Asian Conference on Computer Vision - ACCV 2014, 2014".
    /// </summary>
    /// <remarks>The algorithm implements a novel interest point detector stemming from the intuition that image patches which are highly dissimilar over a relatively large extent of their surroundings hold the property of being repeatable and distinctive. This concept of "contextual self-dissimilarity" reverses the key paradigm of recent successful techniques such as the Local Self-Similarity descriptor and the Non-Local Means filter, which build upon the presence of similar - rather than dissimilar - patches. Moreover, it extends to contextual information the local self-dissimilarity notion embedded in established detectors of corner-like interest points, thereby achieving enhanced repeatability, distinctiveness and localization accuracy.</remarks>
    public class MSDDetector : Feature2D
    {
        /// <summary>
        /// Create a MSD (Maximal Self-Dissimilarity) keypoint detector.
        /// </summary>
        /// <param name="m_patch_radius">Patch radius</param>
        /// <param name="m_search_area_radius">Search area raduis</param>
        /// <param name="m_nms_radius">Nms radius</param>
        /// <param name="m_nms_scale_radius">Nms scale radius</param>
        /// <param name="m_th_saliency">Th saliency</param>
        /// <param name="m_kNN">Knn</param>
        /// <param name="m_scale_factor">Scale factor</param>
        /// <param name="m_n_scales">N scales</param>
        /// <param name="m_compute_orientation">Compute orientation</param>
        public MSDDetector(
            int m_patch_radius, int m_search_area_radius,
            int m_nms_radius, int m_nms_scale_radius, float m_th_saliency, int m_kNN,
            float m_scale_factor, int m_n_scales, bool m_compute_orientation)
        {
            _ptr = XFeatures2DInvoke.cveMSDDetectorCreate(
                m_patch_radius, m_search_area_radius, m_nms_radius,
                m_nms_scale_radius, m_th_saliency, m_kNN,
                m_scale_factor, m_n_scales, m_compute_orientation,
                ref _feature2D, ref _sharedPtr);
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
        internal extern static IntPtr cveMSDDetectorCreate(
            int m_patch_radius, int m_search_area_radius, int m_nms_radius, 
            int m_nms_scale_radius, float m_th_saliency, int m_kNN,
            float m_scale_factor, int m_n_scales, 
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool m_compute_orientation, 
            ref IntPtr feature2D, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveMSDDetectorRelease(ref IntPtr sharedPtr);
    }
}

