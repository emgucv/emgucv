//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features2D
{
    /// <summary>
    /// Wrapped AGAST detector
    /// </summary>
    public class AgastFeatureDetector : Feature2D
    {
        /// <summary>
        /// Agast feature type
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// AGAST_5_8
            /// </summary>
            AGAST_5_8 = 0,
            /// <summary>
            /// AGAST_7_12d
            /// </summary>
            AGAST_7_12d = 1,
            /// <summary>
            /// AGAST_7_12s
            /// </summary>
            AGAST_7_12s = 2,
            /// <summary>
            /// OAST_9_16
            /// </summary>
            OAST_9_16 = 3,
        }

        /// <summary>
        /// Create AGAST using the specific values
        /// </summary>
        /// <param name="threshold">Threshold</param>
        /// <param name="nonmaxSuppression">Non maximum suppression</param>
        /// <param name="type">Type</param>
        public AgastFeatureDetector(
            int threshold = 10,
            bool nonmaxSuppression = true,
            Type type = Type.OAST_9_16)
        {
            _ptr = Features2DInvoke.cveAgastFeatureDetectorCreate(
                threshold, nonmaxSuppression, type,
                ref _feature2D, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                Features2DInvoke.cveAgastFeatureDetectorRelease(ref _sharedPtr);
            base.DisposeObject();
        }

    }

    public static partial class Features2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveAgastFeatureDetectorCreate(
           int threshold,
           [MarshalAs(CvInvoke.BoolMarshalType)]
           bool nonmaxSuppression,
           Emgu.CV.Features2D.AgastFeatureDetector.Type type,
           ref IntPtr feature2D,
           ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAgastFeatureDetectorRelease(ref IntPtr sharedPtr);
    }
}
