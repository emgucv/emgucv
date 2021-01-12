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
    /// Rotation-only model image warper interface.
    /// </summary>
    public abstract class RotationWarper : UnmanagedObject
    {

        /// <summary>
        /// Pointer to the unmanaged RotationWarper object
        /// </summary>
        protected IntPtr _rotationWarper;


        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_rotationWarper != IntPtr.Zero)
                _rotationWarper = IntPtr.Zero;

        }

        /// <summary>
        /// Builds the projection maps according to the given camera data.
        /// </summary>
        /// <param name="srcSize">Source image size</param>
        /// <param name="K">Camera intrinsic parameters</param>
        /// <param name="R">Camera rotation matrix</param>
        /// <param name="xmap">Projection map for the x axis</param>
        /// <param name="ymap">Projection map for the y axis</param>
        /// <returns>Projected image minimum bounding box</returns>
        public Rectangle BuildMaps(Size srcSize, IInputArray K, IInputArray R, IOutputArray xmap, IOutputArray ymap)
        {
            Rectangle result = new Rectangle();
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaR = R.GetInputArray())
            using (OutputArray oaXmap = xmap.GetOutputArray())
            using (OutputArray oaYmap = ymap.GetOutputArray())
            {
                StitchingInvoke.cveRotationWarperBuildMaps(_rotationWarper, ref srcSize, iaK, iaR, oaXmap, oaYmap, ref result);
                return result;
            }
        }

        /// <summary>
        /// Projects the image.
        /// </summary>
        /// <param name="src">Source image</param>
        /// <param name="K">Camera intrinsic parameters</param>
        /// <param name="R">Camera rotation matrix</param>
        /// <param name="interpMode">Interpolation mode</param>
        /// <param name="borderMode">Border extrapolation mode</param>
        /// <param name="dst">Projected image</param>
        /// <returns>Project image top-left corner</returns>
        public Point Warp(IInputArray src, IInputArray K, IInputArray R, CvEnum.Inter interpMode, CvEnum.BorderType borderMode, IOutputArray dst)
        {
            Point corner = new Point();
            using (InputArray iaSrc = src.GetInputArray())
            using (InputArray iaK = K.GetInputArray())
            using (InputArray iaR = R.GetInputArray())
            using (OutputArray oaDst = dst.GetOutputArray())
            {
                StitchingInvoke.cveRotationWarperWarp(_rotationWarper, iaSrc, iaK, iaR, interpMode, borderMode, oaDst, ref corner);
                return corner;
            }
        }
    }

    /// <summary>
    /// Warper that maps an image onto the z = 1 plane.
    /// </summary>
    public class DetailPlaneWarper : RotationWarper
    {
        /// <summary>
        /// Construct an instance of the plane warper class.
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailPlaneWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailPlaneWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailPlaneWarperRelease(ref _ptr);
            }

        }
    }

    /// <summary>
    /// Warper that maps an image onto the x\*x + z\*z = 1 cylinder.
    /// </summary>
    public class DetailCylindricalWarper : RotationWarper
    {
        /// <summary>
        /// Construct an instance of the cylindrical warper class.
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailCylindricalWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailCylindricalWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailCylindricalWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Warper that maps an image onto the unit sphere located at the origin.
    /// </summary>
    public class DetailSphericalWarper : RotationWarper
    {
        /// <summary>
        /// Construct an instance of the spherical warper class.
        /// </summary>
        /// <param name="scale">Radius of the projected sphere, in pixels. An image spanning the whole sphere will have a width of 2 * scale * PI pixels.</param>
        public DetailSphericalWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailSphericalWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailSphericalWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Fisheye Warper
    /// </summary>
    public class DetailFisheyeWarper : RotationWarper
    {
        /// <summary>
        /// Create a fisheye warper
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailFisheyeWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailFisheyeWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailFisheyeWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Stereographic Warper
    /// </summary>
    public class DetailStereographicWarper : RotationWarper
    {
        /// <summary>
        /// Create a stereographic warper
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailStereographicWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailStereographicWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailStereographicWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Compressed rectilinear warper
    /// </summary>
    public class DetailCompressedRectilinearWarper : RotationWarper
    {
        /// <summary>
        /// Create a compressed rectilinear warper
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailCompressedRectilinearWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailCompressedRectilinearWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailCompressedRectilinearWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Panini warper
    /// </summary>
    public class DetailPaniniWarper : RotationWarper
    {
        /// <summary>
        /// Create a Panini warper
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailPaniniWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailPaniniWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailPaniniWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Panini portrait warper
    /// </summary>
    public class DetailPaniniPortraitWarper : RotationWarper
    {
        /// <summary>
        /// Create a panini portrait warper
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailPaniniPortraitWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailPaniniPortraitWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailPaniniPortraitWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Mercator warper
    /// </summary>
    public class DetailMercatorWarper : RotationWarper
    {
        /// <summary>
        /// Create a Mercator Warper
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailMercatorWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailMercatorWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailMercatorWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Transverse mercator warper
    /// </summary>
    public class DetailTransverseMercatorWarper : RotationWarper
    {
        /// <summary>
        /// Create a transverse mercator warper
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailTransverseMercatorWarper(float scale)
        {
            _ptr = StitchingInvoke.cveDetailTransverseMercatorWarperCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailTransverseMercatorWarperRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Warper that maps an image onto the z = 1 plane.
    /// </summary>
    public class DetailPlaneWarperGpu : RotationWarper
    {
        /// <summary>
        /// Construct an instance of the plane warper class.
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailPlaneWarperGpu(float scale)
        {
            _ptr = StitchingInvoke.cveDetailPlaneWarperGpuCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailPlaneWarperGpuRelease(ref _ptr);
            }

        }
    }

    /// <summary>
    /// Warper that maps an image onto the x\*x + z\*z = 1 cylinder.
    /// </summary>
    public class DetailCylindricalWarperGpu : RotationWarper
    {
        /// <summary>
        /// Construct an instance of the cylindrical warper class.
        /// </summary>
        /// <param name="scale">Projected image scale multiplier</param>
        public DetailCylindricalWarperGpu(float scale)
        {
            _ptr = StitchingInvoke.cveDetailCylindricalWarperGpuCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailCylindricalWarperGpuRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Warper that maps an image onto the unit sphere located at the origin.
    /// </summary>
    public class DetailSphericalWarperGpu : RotationWarper
    {
        /// <summary>
        /// Construct an instance of the spherical warper class.
        /// </summary>
        /// <param name="scale">Radius of the projected sphere, in pixels. An image spanning the whole sphere will have a width of 2 * scale * PI pixels.</param>
        public DetailSphericalWarperGpu(float scale)
        {
            _ptr = StitchingInvoke.cveDetailSphericalWarperGpuCreate(scale, ref _rotationWarper);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this warper
        /// </summary>
        protected override void DisposeObject()
        {
            base.DisposeObject();
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveDetailSphericalWarperGpuRelease(ref _ptr);
            }
        }
    }


    public static partial class StitchingInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailPlaneWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailPlaneWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailCylindricalWarperCreate(float scale, ref IntPtr rotationWarper);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailCylindricalWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailSphericalWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailSphericalWarperRelease(ref IntPtr warper);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailFisheyeWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailFisheyeWarperRelease(ref IntPtr warper);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailStereographicWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailStereographicWarperRelease(ref IntPtr warper);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailCompressedRectilinearWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailCompressedRectilinearWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailPaniniWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailPaniniWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailPaniniPortraitWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailPaniniPortraitWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailMercatorWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailMercatorWarperRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailTransverseMercatorWarperCreate(float scale, ref IntPtr rotationWarper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailTransverseMercatorWarperRelease(ref IntPtr warper);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRotationWarperBuildMaps(IntPtr warper, ref Size srcSize, IntPtr K, IntPtr R, IntPtr xmap, IntPtr ymap, ref Rectangle boundingBox);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRotationWarperWarp(IntPtr warper, IntPtr src, IntPtr K, IntPtr R, CvEnum.Inter interpMode, CvEnum.BorderType borderMode, IntPtr dst, ref Point corner);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailPlaneWarperGpuCreate(float scale, ref IntPtr rotationWarper);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailPlaneWarperGpuRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailCylindricalWarperGpuCreate(float scale, ref IntPtr rotationWarper);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailCylindricalWarperGpuRelease(ref IntPtr warper);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDetailSphericalWarperGpuCreate(float scale, ref IntPtr rotationWarper);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDetailSphericalWarperGpuRelease(ref IntPtr warper);

    }
}
