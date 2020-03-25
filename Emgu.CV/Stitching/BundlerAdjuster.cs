//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
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
    /// Base class for all camera parameters refinement methods.
    /// </summary>
    public abstract class BundleAdjusterBase : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the native BundleAdjusterBase object.
        /// </summary>
        protected IntPtr _bundleAdjusterPtr;

        /// <summary>
        /// Pointer to the native BundleAdjusterBase object.
        /// </summary>
        public IntPtr BundleAdjusterPtr
        {
            get { return _bundleAdjusterPtr; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_bundleAdjusterPtr != IntPtr.Zero)
                _bundleAdjusterPtr = IntPtr.Zero;
        }
    }


    /// <summary>
    /// Stub bundle adjuster that does nothing.
    /// </summary>
    public class NoBundleAdjuster : BundleAdjusterBase
    {
        /// <summary>
        /// Create a stub bundle adjuster that does nothing.
        /// </summary>
        public NoBundleAdjuster()
        {
            _ptr = StitchingInvoke.cveNoBundleAdjusterCreate(ref _bundleAdjusterPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this bundle adjuster
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveNoBundleAdjusterRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Implementation of the camera parameters refinement algorithm which minimizes sum of the reprojection
    /// error squares.
    /// It can estimate focal length, aspect ratio, principal point.
    /// </summary>
    public class BundleAdjusterReproj : BundleAdjusterBase
    {
        /// <summary>
        /// Create an implementation of the camera parameters refinement algorithm which minimizes sum of the reprojection
        /// error squares.
        /// </summary>
        public BundleAdjusterReproj()
        {
            _ptr = StitchingInvoke.cveBundleAdjusterReprojCreate(ref _bundleAdjusterPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this bundle adjuster
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBundleAdjusterReprojRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Implementation of the camera parameters refinement algorithm which minimizes sum of the distances between the rays passing through the camera center and a feature.
    /// </summary>
    public class BundleAdjusterRay : BundleAdjusterBase
    {
        /// <summary>
        /// Create a new bundle adjuster
        /// </summary>
        public BundleAdjusterRay()
        {
            _ptr = StitchingInvoke.cveBundleAdjusterRayCreate(ref _bundleAdjusterPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this bundle adjuster
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBundleAdjusterRayRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Bundle adjuster that expects affine transformation represented in homogeneous coordinates in R for each camera param. Implements camera parameters refinement algorithm which minimizes sum of the reprojection error squares.
    /// </summary>
    public class BundleAdjusterAffine : BundleAdjusterBase
    {
        /// <summary>
        /// Create a new Bundle adjuster.
        /// </summary>
        public BundleAdjusterAffine()
        {
            _ptr = StitchingInvoke.cveBundleAdjusterAffineCreate(ref _bundleAdjusterPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this bundle adjuster
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBundleAdjusterAffineRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Bundle adjuster that expects affine transformation with 4 DOF represented in homogeneous coordinates in R for each camera param. Implements camera parameters refinement algorithm which minimizes sum of the reprojection error squares.
    /// </summary>
    public class BundleAdjusterAffinePartial : BundleAdjusterBase
    {
        /// <summary>
        /// Create a new affine bundler adjuster
        /// </summary>
        public BundleAdjusterAffinePartial()
        {
            _ptr = StitchingInvoke.cveBundleAdjusterAffinePartialCreate(ref _bundleAdjusterPtr);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this bundle adjuster
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveBundleAdjusterAffinePartialRelease(ref _ptr);
            }
        }
    }

    public static partial class StitchingInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveNoBundleAdjusterCreate(ref IntPtr bundleAdjusterPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveNoBundleAdjusterRelease(ref IntPtr bundleAdjuster);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBundleAdjusterReprojCreate(ref IntPtr bundleAdjusterPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBundleAdjusterReprojRelease(ref IntPtr bundleAdjuster);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBundleAdjusterRayCreate(ref IntPtr bundleAdjusterPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBundleAdjusterRayRelease(ref IntPtr bundleAdjuster);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBundleAdjusterAffineCreate(ref IntPtr bundleAdjusterPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBundleAdjusterAffineRelease(ref IntPtr bundleAdjuster);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveBundleAdjusterAffinePartialCreate(ref IntPtr bundleAdjusterPtr);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveBundleAdjusterAffinePartialRelease(ref IntPtr bundleAdjuster);

    }
}
