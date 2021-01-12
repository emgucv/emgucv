//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Rotation estimator base class.
    /// </summary>
    public abstract class Estimator : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native Estimator object.
        /// </summary>
        protected IntPtr _estimatorPtr;

        /// <summary>
        /// Pointer to the native Estimator object.
        /// </summary>
        public IntPtr EstimatorPtr
        {
            get { return _estimatorPtr; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_estimatorPtr != IntPtr.Zero)
                _estimatorPtr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Homography based rotation estimator.
    /// </summary>
    public class HomographyBasedEstimator : Estimator
    {
        /// <summary>
        /// Create a  new homography based rotation estimator
        /// </summary>
        /// <param name="isFocalsEstimated">Is focals estimated</param>
        public HomographyBasedEstimator(bool isFocalsEstimated)
        {
            _ptr = StitchingInvoke.cveHomographyBasedEstimatorCreate(isFocalsEstimated, ref _estimatorPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this estimator
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveHomographyBasedEstimatorRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Affine transformation based estimator.
    /// </summary>
    public class AffineBasedEstimator : Estimator
    {
        /// <summary>
        /// Create a new affine transformation based estimator.
        /// </summary>
        public AffineBasedEstimator()
        {
            _ptr = StitchingInvoke.cveAffineBasedEstimatorCreate(ref _estimatorPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this estimator
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveAffineBasedEstimatorRelease(ref _ptr);
            }
        }
    }

    public static partial class StitchingInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveHomographyBasedEstimatorCreate(
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool isFocalsEstimated, 
            ref IntPtr estimator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveHomographyBasedEstimatorRelease(ref IntPtr estimator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveAffineBasedEstimatorCreate(ref IntPtr estimator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveAffineBasedEstimatorRelease(ref IntPtr estimator);

    }
}
