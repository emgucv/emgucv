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
    /// A TSDF / HashTSDF / ColorTSDF volume, used for fusing a sequence of depth (and, for ColorTSDF, color)
    /// frames into a 3D reconstruction, and for rendering the reconstructed surface back out via raycasting.
    /// </summary>
    public class Volume : UnmanagedObject
    {
        /// <summary>
        /// The precision used when computing a volume's bounding box.
        /// </summary>
        public enum BoundingBoxPrecision
        {
            /// <summary>
            /// Up to volume unit.
            /// </summary>
            VolumeUnit = 0,
            /// <summary>
            /// Up to voxel (currently not supported).
            /// </summary>
            Voxel = 1
        }

        /// <summary>
        /// Create a custom volume.
        /// </summary>
        /// <param name="volumeType">The volume type: TSDF, HashTSDF or ColorTSDF.</param>
        /// <param name="settings">The settings for the volume. If not specified, the default settings for the given volume type will be used.</param>
        public Volume(VolumeType volumeType = VolumeType.Tsdf, VolumeSettings settings = null)
        {
            if (settings == null)
            {
                using (VolumeSettings defaultSettings = new VolumeSettings(volumeType))
                    _ptr = CvInvoke.cveVolumeCreate((int)volumeType, defaultSettings);
            }
            else
            {
                _ptr = CvInvoke.cveVolumeCreate((int)volumeType, settings);
            }
        }

        /// <summary>
        /// Integrates the input depth data into the volume. Camera intrinsics are taken from the volume's
        /// settings.
        /// </summary>
        /// <param name="depth">The depth image.</param>
        /// <param name="pose">The pose of the camera in global coordinates.</param>
        public void Integrate(IInputArray depth, IInputArray pose)
        {
            using (InputArray iaDepth = depth.GetInputArray())
            using (InputArray iaPose = pose.GetInputArray())
                CvInvoke.cveVolumeIntegrate(_ptr, iaDepth, iaPose);
        }

        /// <summary>
        /// Integrates the input depth and color data into the volume (ColorTSDF only). Camera intrinsics are
        /// taken from the volume's settings.
        /// </summary>
        /// <param name="depth">The depth image.</param>
        /// <param name="image">The color image. Must be registered with the depth data, i.e. have the same intrinsics and camera pose.</param>
        /// <param name="pose">The pose of the camera in global coordinates.</param>
        public void IntegrateColor(IInputArray depth, IInputArray image, IInputArray pose)
        {
            using (InputArray iaDepth = depth.GetInputArray())
            using (InputArray iaImage = image.GetInputArray())
            using (InputArray iaPose = pose.GetInputArray())
                CvInvoke.cveVolumeIntegrateColor(_ptr, iaDepth, iaImage, iaPose);
        }

        /// <summary>
        /// Integrates the input data, from a precalculated <see cref="OdometryFrame"/>, into the volume.
        /// Camera intrinsics are taken from the volume's settings.
        /// </summary>
        /// <param name="frame">The frame from which to take depth (and, for ColorTSDF, color) data.</param>
        /// <param name="pose">The pose of the camera in global coordinates.</param>
        public void IntegrateFrame(OdometryFrame frame, IInputArray pose)
        {
            using (InputArray iaPose = pose.GetInputArray())
                CvInvoke.cveVolumeIntegrateFrame(_ptr, frame, iaPose);
        }

        /// <summary>
        /// Renders the volume's contents into an image. The resulting points and normals are in the camera's
        /// coordinate system. Rendered image size and camera intrinsics are taken from the volume's settings.
        /// </summary>
        /// <param name="cameraPose">The pose of the camera in global coordinates.</param>
        /// <param name="points">Storage for the rendered points.</param>
        /// <param name="normals">Storage for the rendered normals corresponding to the points.</param>
        public void Raycast(IInputArray cameraPose, IOutputArray points, IOutputArray normals)
        {
            using (InputArray iaCameraPose = cameraPose.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
                CvInvoke.cveVolumeRaycast(_ptr, iaCameraPose, oaPoints, oaNormals);
        }

        /// <summary>
        /// Renders the volume's contents into an image, including colors (ColorTSDF only). The resulting
        /// points and normals are in the camera's coordinate system. Rendered image size and camera intrinsics
        /// are taken from the volume's settings.
        /// </summary>
        /// <param name="cameraPose">The pose of the camera in global coordinates.</param>
        /// <param name="points">Storage for the rendered points.</param>
        /// <param name="normals">Storage for the rendered normals corresponding to the points.</param>
        /// <param name="colors">Storage for the rendered colors corresponding to the points.</param>
        public void RaycastColor(IInputArray cameraPose, IOutputArray points, IOutputArray normals, IOutputArray colors)
        {
            using (InputArray iaCameraPose = cameraPose.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
            using (OutputArray oaColors = colors.GetOutputArray())
                CvInvoke.cveVolumeRaycastColor(_ptr, iaCameraPose, oaPoints, oaNormals, oaColors);
        }

        /// <summary>
        /// Renders the volume's contents into an image of the given size, using the given camera intrinsics.
        /// The resulting points and normals are in the camera's coordinate system.
        /// </summary>
        /// <param name="cameraPose">The pose of the camera in global coordinates.</param>
        /// <param name="height">The height of the resulting image.</param>
        /// <param name="width">The width of the resulting image.</param>
        /// <param name="k">The camera intrinsics to use for the raycast.</param>
        /// <param name="points">Storage for the rendered points.</param>
        /// <param name="normals">Storage for the rendered normals corresponding to the points.</param>
        public void RaycastEx(IInputArray cameraPose, int height, int width, IInputArray k, IOutputArray points, IOutputArray normals)
        {
            using (InputArray iaCameraPose = cameraPose.GetInputArray())
            using (InputArray iaK = k.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
                CvInvoke.cveVolumeRaycastEx(_ptr, iaCameraPose, height, width, iaK, oaPoints, oaNormals);
        }

        /// <summary>
        /// Renders the volume's contents into an image of the given size, including colors (ColorTSDF only),
        /// using the given camera intrinsics. The resulting points and normals are in the camera's coordinate
        /// system.
        /// </summary>
        /// <param name="cameraPose">The pose of the camera in global coordinates.</param>
        /// <param name="height">The height of the resulting image.</param>
        /// <param name="width">The width of the resulting image.</param>
        /// <param name="k">The camera intrinsics to use for the raycast.</param>
        /// <param name="points">Storage for the rendered points.</param>
        /// <param name="normals">Storage for the rendered normals corresponding to the points.</param>
        /// <param name="colors">Storage for the rendered colors corresponding to the points.</param>
        public void RaycastExColor(IInputArray cameraPose, int height, int width, IInputArray k, IOutputArray points, IOutputArray normals, IOutputArray colors)
        {
            using (InputArray iaCameraPose = cameraPose.GetInputArray())
            using (InputArray iaK = k.GetInputArray())
            using (OutputArray oaPoints = points.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
            using (OutputArray oaColors = colors.GetOutputArray())
                CvInvoke.cveVolumeRaycastExColor(_ptr, iaCameraPose, height, width, iaK, oaPoints, oaNormals, oaColors);
        }

        /// <summary>
        /// Extracts the normals corresponding to the given points from the volume.
        /// </summary>
        /// <param name="points">The input points that already exist.</param>
        /// <param name="normals">Storage for the normals corresponding to the input points.</param>
        public void FetchNormals(IInputArray points, IOutputArray normals)
        {
            using (InputArray iaPoints = points.GetInputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
                CvInvoke.cveVolumeFetchNormals(_ptr, iaPoints, oaNormals);
        }

        /// <summary>
        /// Extracts all points and their corresponding normals from the volume.
        /// </summary>
        /// <param name="points">Storage for all points.</param>
        /// <param name="normals">Storage for all normals, corresponding to the points.</param>
        public void FetchPointsNormals(IOutputArray points, IOutputArray normals)
        {
            using (OutputArray oaPoints = points.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
                CvInvoke.cveVolumeFetchPointsNormals(_ptr, oaPoints, oaNormals);
        }

        /// <summary>
        /// Extracts all points, normals and colors from the volume (ColorTSDF only).
        /// </summary>
        /// <param name="points">Storage for all points.</param>
        /// <param name="normals">Storage for all normals, corresponding to the points.</param>
        /// <param name="colors">Storage for all colors, corresponding to the points.</param>
        public void FetchPointsNormalsColors(IOutputArray points, IOutputArray normals, IOutputArray colors)
        {
            using (OutputArray oaPoints = points.GetOutputArray())
            using (OutputArray oaNormals = normals.GetOutputArray())
            using (OutputArray oaColors = colors.GetOutputArray())
                CvInvoke.cveVolumeFetchPointsNormalsColors(_ptr, oaPoints, oaNormals, oaColors);
        }

        /// <summary>
        /// Clears all data in the volume.
        /// </summary>
        public void Reset()
        {
            CvInvoke.cveVolumeReset(_ptr);
        }

        /// <summary>
        /// The number of visible blocks in the volume.
        /// </summary>
        //TODO: remove this
        public int VisibleBlocks
        {
            get { return CvInvoke.cveVolumeGetVisibleBlocks(_ptr); }
        }

        /// <summary>
        /// The number of volume units in the volume.
        /// </summary>
        public long TotalVolumeUnits
        {
            get { return CvInvoke.cveVolumeGetTotalVolumeUnits(_ptr).ToInt64(); }
        }

        /// <summary>
        /// Gets the bounding box in volume coordinates, with the given precision.
        /// </summary>
        /// <param name="boundingBox">A 6-float 1d array containing (min_x, min_y, min_z, max_x, max_y, max_z) in volume coordinates.</param>
        /// <param name="precision">The bounding box calculation precision.</param>
        public void GetBoundingBox(IOutputArray boundingBox, BoundingBoxPrecision precision)
        {
            using (OutputArray oaBoundingBox = boundingBox.GetOutputArray())
                CvInvoke.cveVolumeGetBoundingBox(_ptr, oaBoundingBox, (int)precision);
        }

        /// <summary>
        /// Enables or disables new volume unit allocation during integration. Only meaningful for HashTSDF.
        /// </summary>
        public bool EnableGrowth
        {
            get { return CvInvoke.cveVolumeGetEnableGrowth(_ptr); }
            set { CvInvoke.cveVolumeSetEnableGrowth(_ptr, value); }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveVolumeRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV functions
    /// </summary>
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVolumeCreate(int volumeType, IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeRelease(ref IntPtr volume);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeIntegrate(IntPtr volume, IntPtr depth, IntPtr pose);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeIntegrateColor(IntPtr volume, IntPtr depth, IntPtr image, IntPtr pose);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeIntegrateFrame(IntPtr volume, IntPtr frame, IntPtr pose);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeRaycast(IntPtr volume, IntPtr cameraPose, IntPtr points, IntPtr normals);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeRaycastColor(IntPtr volume, IntPtr cameraPose, IntPtr points, IntPtr normals, IntPtr colors);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeRaycastEx(IntPtr volume, IntPtr cameraPose, int height, int width, IntPtr k, IntPtr points, IntPtr normals);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeRaycastExColor(IntPtr volume, IntPtr cameraPose, int height, int width, IntPtr k, IntPtr points, IntPtr normals, IntPtr colors);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeFetchNormals(IntPtr volume, IntPtr points, IntPtr normals);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeFetchPointsNormals(IntPtr volume, IntPtr points, IntPtr normals);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeFetchPointsNormalsColors(IntPtr volume, IntPtr points, IntPtr normals, IntPtr colors);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeReset(IntPtr volume);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveVolumeGetVisibleBlocks(IntPtr volume);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVolumeGetTotalVolumeUnits(IntPtr volume);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeGetBoundingBox(IntPtr volume, IntPtr boundingBox, int precision);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSetEnableGrowth(IntPtr volume, [MarshalAs(CvInvoke.BoolMarshalType)] bool v);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveVolumeGetEnableGrowth(IntPtr volume);
    }
}
