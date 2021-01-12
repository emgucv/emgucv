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
    /// Image warper factories base class.
    /// </summary>
    public abstract class WarperCreator : UnmanagedObject
    {
        /// <summary>
        /// Pointer to the unmanaged WarperCreator object
        /// </summary>
        protected IntPtr _warperCreator;

        /// <summary>
        /// Pointer to the unmanaged WarperCreator object
        /// </summary>
        public IntPtr WarperCreatorPtr
        {
            get { return _warperCreator; }
        }

        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_warperCreator != IntPtr.Zero)
                _warperCreator = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Warper that maps an image onto the z = 1 plane.
    /// </summary>
    public class PlaneWarper : WarperCreator
    {
        /// <summary>
        /// Construct an instance of the plane warper class.
        /// </summary>
        public PlaneWarper()
        {
            _ptr = StitchingInvoke.cvePlaneWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cvePlaneWarperRelease(ref _ptr);
            }

        }
    }

    /// <summary>
    /// Warper that maps an image onto the x\*x + z\*z = 1 cylinder.
    /// </summary>
    public class CylindricalWarper : WarperCreator
    {
        /// <summary>
        /// Construct an instance of the cylindrical warper class.
        /// </summary>
        public CylindricalWarper()
        {
            _ptr = StitchingInvoke.cveCylindricalWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveCylindricalWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Warper that maps an image onto the unit sphere located at the origin.
    /// </summary>
    public class SphericalWarper : WarperCreator
    {
        /// <summary>
        /// Construct an instance of the spherical warper class.
        /// </summary>
        public SphericalWarper()
        {
            _ptr = StitchingInvoke.cveSphericalWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveSphericalWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Fisheye Warper
    /// </summary>
    public class FisheyeWarper : WarperCreator
    {
        /// <summary>
        /// Create a fisheye warper
        /// </summary>
        public FisheyeWarper()
        {
            _ptr = StitchingInvoke.cveFisheyeWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveFisheyeWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Stereographic Warper
    /// </summary>
    public class StereographicWarper : WarperCreator
    {
        /// <summary>
        /// Create a stereographic warper
        /// </summary>
        public StereographicWarper()
        {
            _ptr = StitchingInvoke.cveStereographicWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveStereographicWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Compressed rectilinear warper
    /// </summary>
    public class CompressedRectilinearWarper : WarperCreator
    {
        /// <summary>
        /// Create a compressed rectilinear warper
        /// </summary>
        public CompressedRectilinearWarper()
        {
            _ptr = StitchingInvoke.cveCompressedRectilinearWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveCompressedRectilinearWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Panini warper
    /// </summary>
    public class PaniniWarper : WarperCreator
    {
        /// <summary>
        /// Create a Panini warper
        /// </summary>
        public PaniniWarper()
        {
            _ptr = StitchingInvoke.cvePaniniWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cvePaniniWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Panini portrait warper
    /// </summary>
    public class PaniniPortraitWarper : WarperCreator
    {
        /// <summary>
        /// Create a panini portrait warper
        /// </summary>
        public PaniniPortraitWarper()
        {
            _ptr = StitchingInvoke.cvePaniniPortraitWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cvePaniniPortraitWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Mercator warper
    /// </summary>
    public class MercatorWarper : WarperCreator
    {
        /// <summary>
        /// Create a Mercator Warper
        /// </summary>
        public MercatorWarper()
        {
            _ptr = StitchingInvoke.cveMercatorWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveMercatorWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Transverse mercator warper
    /// </summary>
    public class TransverseMercatorWarper : WarperCreator
    {
        /// <summary>
        /// Create a transverse mercator warper
        /// </summary>
        public TransverseMercatorWarper()
        {
            _ptr = StitchingInvoke.cveTransverseMercatorWarperCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveTransverseMercatorWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Warper that maps an image onto the z = 1 plane.
    /// </summary>
    public class PlaneWarperGpu : WarperCreator
    {
        /// <summary>
        /// Construct an instance of the plane warper class.
        /// </summary>
        public PlaneWarperGpu()
        {
            _ptr = StitchingInvoke.cvePlaneWarperGpuCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cvePlaneWarperGpuRelease(ref _ptr);
            }

        }
    }

    /// <summary>
    /// Warper that maps an image onto the x\*x + z\*z = 1 cylinder.
    /// </summary>
    public class CylindricalWarperGpu : WarperCreator
    {
        /// <summary>
        /// Construct an instance of the cylindrical warper class.
        /// </summary>
        public CylindricalWarperGpu()
        {
            _ptr = StitchingInvoke.cveCylindricalWarperGpuCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveCylindricalWarperGpuRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Warper that maps an image onto the unit sphere located at the origin.
    /// </summary>
    public class SphericalWarperGpu : WarperCreator
    {
        /// <summary>
        /// Construct an instance of the spherical warper class.
        /// </summary>
        public SphericalWarperGpu()
        {
            _ptr = StitchingInvoke.cveSphericalWarperGpuCreate(ref _warperCreator);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveSphericalWarperGpuRelease(ref _ptr);
            }
        }
    }

    public static partial class StitchingInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cvePlaneWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePlaneWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCylindricalWarperCreate(ref IntPtr warperCreator);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCylindricalWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSphericalWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSphericalWarperRelease(ref IntPtr warper);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFisheyeWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFisheyeWarperRelease(ref IntPtr warper);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveStereographicWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveStereographicWarperRelease(ref IntPtr warper);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCompressedRectilinearWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCompressedRectilinearWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cvePaniniWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePaniniWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cvePaniniPortraitWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePaniniPortraitWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMercatorWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMercatorWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTransverseMercatorWarperCreate(ref IntPtr warperCreator);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTransverseMercatorWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cvePlaneWarperGpuCreate(ref IntPtr warperCreator);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePlaneWarperGpuRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCylindricalWarperGpuCreate(ref IntPtr warperCreator);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCylindricalWarperGpuRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveSphericalWarperGpuCreate(ref IntPtr warperCreator);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSphericalWarperGpuRelease(ref IntPtr warper);

    }
}
