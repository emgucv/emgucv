//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// An object that keeps per-frame data for Odometry algorithms, from user-provided images to
    /// algorithm-specific precalculated data. When not empty, it contains a depth image, a mask of valid
    /// pixels and a set of pyramids generated from that data. A BGR/Gray image and normals are optional.
    /// OdometryFrame is made to be used together with the <see cref="Odometry"/> class and with
    /// <see cref="Volume"/>'s IntegrateFrame method, to reuse precalculated data between computations.
    /// </summary>
    public class OdometryFrame : UnmanagedObject
    {
        /// <summary>
        /// Construct a new OdometryFrame object. All non-empty images should have the same size.
        /// </summary>
        /// <param name="depth">A depth image, should be CV_8UC1.</param>
        /// <param name="image">A BGR or grayscale image (or null if it's not required for the used ICP algorithm). Should be CV_8UC3 or CV_8UC4 if it's a BGR image, or CV_8UC1 if it's grayscale. If it's BGR then it is converted to grayscale automatically.</param>
        /// <param name="mask">A user-provided mask of valid pixels, should be CV_8UC1.</param>
        /// <param name="normals">User-provided normals to the depth surface, should be CV_32FC4.</param>
        public OdometryFrame(
            IInputArray depth = null,
            IInputArray image = null,
            IInputArray mask = null,
            IInputArray normals = null)
        {
            using (InputArray iaDepth = depth == null ? InputArray.GetEmpty() : depth.GetInputArray())
            using (InputArray iaImage = image == null ? InputArray.GetEmpty() : image.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            using (InputArray iaNormals = normals == null ? InputArray.GetEmpty() : normals.GetInputArray())
                _ptr = CvInvoke.cveOdometryFrameCreate(iaDepth, iaImage, iaMask, iaNormals);
        }

        /// <summary>
        /// Get the original user-provided BGR/Gray image.
        /// </summary>
        /// <param name="image">Output image.</param>
        public void GetImage(IOutputArray image)
        {
            using (OutputArray oaImage = image.GetOutputArray())
                CvInvoke.cveOdometryFrameGetImage(_ptr, oaImage);
        }

        /// <summary>
        /// Get the gray image generated from the user-provided BGR/Gray image.
        /// </summary>
        /// <param name="image">Output image.</param>
        public void GetGrayImage(IOutputArray image)
        {
            using (OutputArray oaImage = image.GetOutputArray())
                CvInvoke.cveOdometryFrameGetGrayImage(_ptr, oaImage);
        }

        /// <summary>
        /// Get the original user-provided depth image.
        /// </summary>
        /// <param name="depth">Output image.</param>
        public void GetDepth(IOutputArray depth)
        {
            using (OutputArray oaDepth = depth.GetOutputArray())
                CvInvoke.cveOdometryFrameGetDepth(_ptr, oaDepth);
        }

        /// <summary>
        /// Get the depth image generated from the user-provided one, after conversion, rescale or
        /// filtering for the ICP algorithm's needs.
        /// </summary>
        /// <param name="depth">Output image.</param>
        public void GetProcessedDepth(IOutputArray depth)
        {
            using (OutputArray oaDepth = depth.GetOutputArray())
                CvInvoke.cveOdometryFrameGetProcessedDepth(_ptr, oaDepth);
        }

        /// <summary>
        /// Get the valid pixels mask generated for the ICP calculations, intersected with the
        /// user-provided mask.
        /// </summary>
        /// <param name="mask">Output image.</param>
        public void GetMask(IOutputArray mask)
        {
            using (OutputArray oaMask = mask.GetOutputArray())
                CvInvoke.cveOdometryFrameGetMask(_ptr, oaMask);
        }

        /// <summary>
        /// Get the normals image, either generated for the ICP calculations or user-provided.
        /// </summary>
        /// <param name="normals">Output image.</param>
        public void GetNormals(IOutputArray normals)
        {
            using (OutputArray oaNormals = normals.GetOutputArray())
                CvInvoke.cveOdometryFrameGetNormals(_ptr, oaNormals);
        }

        /// <summary>
        /// Get the number of levels in the pyramids (all of them, if not empty, should have the same
        /// number of levels), or 0 if no pyramids have been prepared yet.
        /// </summary>
        public int PyramidLevels
        {
            get { return CvInvoke.cveOdometryFrameGetPyramidLevels(_ptr); }
        }

        /// <summary>
        /// Get the image generated for the ICP calculations from one of the pyramids specified by
        /// <paramref name="pyramidType"/>. Returns an empty image if the pyramid is empty or there's no
        /// such pyramid level.
        /// </summary>
        /// <param name="img">Output image.</param>
        /// <param name="pyramidType">Type of pyramid.</param>
        /// <param name="level">Level in the pyramid.</param>
        public void GetPyramidAt(IOutputArray img, OdometryFramePyramidType pyramidType, int level)
        {
            using (OutputArray oaImg = img.GetOutputArray())
                CvInvoke.cveOdometryFrameGetPyramidAt(_ptr, oaImg, pyramidType, new IntPtr(level));
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveOdometryFrameRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV functions
    /// </summary>
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOdometryFrameCreate(IntPtr depth, IntPtr image, IntPtr mask, IntPtr normals);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameRelease(ref IntPtr ptr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameGetImage(IntPtr frame, IntPtr image);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameGetGrayImage(IntPtr frame, IntPtr image);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameGetDepth(IntPtr frame, IntPtr depth);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameGetProcessedDepth(IntPtr frame, IntPtr depth);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameGetMask(IntPtr frame, IntPtr mask);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameGetNormals(IntPtr frame, IntPtr normals);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveOdometryFrameGetPyramidLevels(IntPtr frame);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryFrameGetPyramidAt(IntPtr frame, IntPtr img, OdometryFramePyramidType pyrType, IntPtr level);
    }
}
