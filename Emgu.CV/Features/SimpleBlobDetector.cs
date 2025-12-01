//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Features
{
    /// <summary>
    /// Simple Blob detector
    /// </summary>
    public class SimpleBlobDetector : Feature2D
    {

        /// <summary>
        /// Create a simple blob detector
        /// </summary>
        /// <param name="parameters">The parameters for creating a simple blob detector</param>
        public SimpleBlobDetector(SimpleBlobDetectorParams parameters = null)
        {
            if (parameters == null)
                _ptr = FeaturesInvoke.cveSimpleBlobDetectorCreate(ref _feature2D, ref _sharedPtr);
            else
            {
                _ptr = FeaturesInvoke.cveSimpleBlobDetectorCreateWithParams(ref _feature2D, parameters, ref _sharedPtr);
            }
        }

        /// <summary>
        /// Retrieves the contours of detected blobs.
        /// </summary>
        /// <returns>
        /// A <see cref="VectorOfVectorOfPoint"/> containing the contours of the detected blobs.
        /// </returns>
        public VectorOfVectorOfPoint GetBlobContours()
        {
            return new VectorOfVectorOfPoint(FeaturesInvoke.cveSimpleBlobDetectorGetBlobContours(_ptr), false);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this detector.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                FeaturesInvoke.cveSimpleBlobDetectorRelease(ref _sharedPtr);

            base.DisposeObject();
        }
    }

    /// <summary>
    /// Parameters for the simple blob detector
    /// </summary>
    public partial class SimpleBlobDetectorParams : UnmanagedObject
    {
        /// <summary>
        /// Create parameters for simple blob detector and use default values.
        /// </summary>
        public SimpleBlobDetectorParams()
        {
            _ptr = FeaturesInvoke.cveSimpleBlobDetectorParamsCreate();
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this simple blob detector parameter.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                FeaturesInvoke.cveSimpleBlobDetectorParamsRelease(ref _ptr);
            }
        }
    }

    public static partial class FeaturesInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSimpleBlobDetectorCreate(ref IntPtr feature2DPtr, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSimpleBlobDetectorCreateWithParams(ref IntPtr feature2DPtr, IntPtr parameters, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSimpleBlobDetectorRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSimpleBlobDetectorParamsCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSimpleBlobDetectorParamsRelease(ref IntPtr parameters);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSimpleBlobDetectorGetBlobContours(IntPtr detector);

    }
}