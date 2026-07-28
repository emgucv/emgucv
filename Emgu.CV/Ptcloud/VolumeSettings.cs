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
    /// Settings for a <see cref="Volume"/>.
    /// </summary>
    public class VolumeSettings : UnmanagedObject
    {
        /// <summary>
        /// Create settings for a custom Volume type.
        /// </summary>
        /// <param name="volumeType">The volume type.</param>
        public VolumeSettings(VolumeType volumeType = VolumeType.Tsdf)
        {
            _ptr = CvInvoke.cveVolumeSettingsCreate((int)volumeType);
        }

        /// <summary>
        /// The width of the image for integration.
        /// </summary>
        public int IntegrateWidth
        {
            get { return CvInvoke.cveVolumeSettingsGetIntegrateWidth(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetIntegrateWidth(_ptr, value); }
        }

        /// <summary>
        /// The height of the image for integration.
        /// </summary>
        public int IntegrateHeight
        {
            get { return CvInvoke.cveVolumeSettingsGetIntegrateHeight(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetIntegrateHeight(_ptr, value); }
        }

        /// <summary>
        /// The width of the raycasted image, used when the user does not provide it at raycast time.
        /// </summary>
        public int RaycastWidth
        {
            get { return CvInvoke.cveVolumeSettingsGetRaycastWidth(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetRaycastWidth(_ptr, value); }
        }

        /// <summary>
        /// The height of the raycasted image, used when the user does not provide it at raycast time.
        /// </summary>
        public int RaycastHeight
        {
            get { return CvInvoke.cveVolumeSettingsGetRaycastHeight(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetRaycastHeight(_ptr, value); }
        }

        /// <summary>
        /// Depth factor, which is the number used for depth scaling.
        /// </summary>
        public float DepthFactor
        {
            get { return CvInvoke.cveVolumeSettingsGetDepthFactor(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetDepthFactor(_ptr, value); }
        }

        /// <summary>
        /// The size of a voxel.
        /// </summary>
        public float VoxelSize
        {
            get { return CvInvoke.cveVolumeSettingsGetVoxelSize(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetVoxelSize(_ptr, value); }
        }

        /// <summary>
        /// TSDF truncation distance. Distances greater than this value from the surface will be truncated to 1.0.
        /// </summary>
        public float TsdfTruncateDistance
        {
            get { return CvInvoke.cveVolumeSettingsGetTsdfTruncateDistance(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetTsdfTruncateDistance(_ptr, value); }
        }

        /// <summary>
        /// Threshold for depth truncation in meters. Depth greater than this threshold is truncated to 0.
        /// </summary>
        public float MaxDepth
        {
            get { return CvInvoke.cveVolumeSettingsGetMaxDepth(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetMaxDepth(_ptr, value); }
        }

        /// <summary>
        /// Max number of frames to integrate per voxel. Represents the max number of frames over which a
        /// running average of the TSDF is calculated for a voxel.
        /// </summary>
        public int MaxWeight
        {
            get { return CvInvoke.cveVolumeSettingsGetMaxWeight(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetMaxWeight(_ptr, value); }
        }

        /// <summary>
        /// Length of a single raycast step, as a percentage of the voxel length that is skipped per march.
        /// </summary>
        public float RaycastStepFactor
        {
            get { return CvInvoke.cveVolumeSettingsGetRaycastStepFactor(_ptr); }
            set { CvInvoke.cveVolumeSettingsSetRaycastStepFactor(_ptr, value); }
        }

        /// <summary>
        /// Sets the volume pose.
        /// </summary>
        /// <param name="pose">The volume pose.</param>
        public void SetVolumePose(IInputArray pose)
        {
            using (InputArray iaPose = pose.GetInputArray())
                CvInvoke.cveVolumeSettingsSetVolumePose(_ptr, iaPose);
        }

        /// <summary>
        /// Gets the volume pose.
        /// </summary>
        /// <param name="pose">The volume pose.</param>
        public void GetVolumePose(IOutputArray pose)
        {
            using (OutputArray oaPose = pose.GetOutputArray())
                CvInvoke.cveVolumeSettingsGetVolumePose(_ptr, oaPose);
        }

        /// <summary>
        /// Sets the resolution of the voxel space (number of voxels in each dimension). Applicable only for
        /// TSDF volume. HashTSDF volume only supports equal resolution in all three dimensions.
        /// </summary>
        /// <param name="resolution">The volume resolution.</param>
        public void SetVolumeResolution(IInputArray resolution)
        {
            using (InputArray iaResolution = resolution.GetInputArray())
                CvInvoke.cveVolumeSettingsSetVolumeResolution(_ptr, iaResolution);
        }

        /// <summary>
        /// Gets the resolution of the voxel space (number of voxels in each dimension).
        /// </summary>
        /// <param name="resolution">The volume resolution.</param>
        public void GetVolumeResolution(IOutputArray resolution)
        {
            using (OutputArray oaResolution = resolution.GetOutputArray())
                CvInvoke.cveVolumeSettingsGetVolumeResolution(_ptr, oaResolution);
        }

        /// <summary>
        /// Gets the 3 integers representing strides by x, y and z dimension. Can be used to iterate over raw
        /// volume unit data.
        /// </summary>
        /// <param name="strides">The volume strides.</param>
        public void GetVolumeStrides(IOutputArray strides)
        {
            using (OutputArray oaStrides = strides.GetOutputArray())
                CvInvoke.cveVolumeSettingsGetVolumeStrides(_ptr, oaStrides);
        }

        /// <summary>
        /// Sets the intrinsics of the camera used for integration.
        /// </summary>
        /// <param name="intrinsics">
        /// A 3x3 matrix: [ fx 0 cx; 0 fy cy; 0 0 1 ], where fx and fy are the focal lengths and cx and cy are
        /// the principal point coordinates.
        /// </param>
        public void SetCameraIntegrateIntrinsics(IInputArray intrinsics)
        {
            using (InputArray iaIntrinsics = intrinsics.GetInputArray())
                CvInvoke.cveVolumeSettingsSetCameraIntegrateIntrinsics(_ptr, iaIntrinsics);
        }

        /// <summary>
        /// Gets the intrinsics of the camera used for integration.
        /// </summary>
        /// <param name="intrinsics">
        /// A 3x3 matrix: [ fx 0 cx; 0 fy cy; 0 0 1 ], where fx and fy are the focal lengths and cx and cy are
        /// the principal point coordinates.
        /// </param>
        public void GetCameraIntegrateIntrinsics(IOutputArray intrinsics)
        {
            using (OutputArray oaIntrinsics = intrinsics.GetOutputArray())
                CvInvoke.cveVolumeSettingsGetCameraIntegrateIntrinsics(_ptr, oaIntrinsics);
        }

        /// <summary>
        /// Sets the camera intrinsics used for the raycast image, when the user does not provide them at
        /// raycast time.
        /// </summary>
        /// <param name="intrinsics">
        /// A 3x3 matrix: [ fx 0 cx; 0 fy cy; 0 0 1 ], where fx and fy are the focal lengths and cx and cy are
        /// the principal point coordinates.
        /// </param>
        public void SetCameraRaycastIntrinsics(IInputArray intrinsics)
        {
            using (InputArray iaIntrinsics = intrinsics.GetInputArray())
                CvInvoke.cveVolumeSettingsSetCameraRaycastIntrinsics(_ptr, iaIntrinsics);
        }

        /// <summary>
        /// Gets the camera intrinsics used for the raycast image, when the user does not provide them at
        /// raycast time.
        /// </summary>
        /// <param name="intrinsics">
        /// A 3x3 matrix: [ fx 0 cx; 0 fy cy; 0 0 1 ], where fx and fy are the focal lengths and cx and cy are
        /// the principal point coordinates.
        /// </param>
        public void GetCameraRaycastIntrinsics(IOutputArray intrinsics)
        {
            using (OutputArray oaIntrinsics = intrinsics.GetOutputArray())
                CvInvoke.cveVolumeSettingsGetCameraRaycastIntrinsics(_ptr, oaIntrinsics);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveVolumeSettingsRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV functions
    /// </summary>
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveVolumeSettingsCreate(int volumeType);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsRelease(ref IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetIntegrateWidth(IntPtr settings, int val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveVolumeSettingsGetIntegrateWidth(IntPtr settings);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetIntegrateHeight(IntPtr settings, int val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveVolumeSettingsGetIntegrateHeight(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetRaycastWidth(IntPtr settings, int val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveVolumeSettingsGetRaycastWidth(IntPtr settings);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetRaycastHeight(IntPtr settings, int val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveVolumeSettingsGetRaycastHeight(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetDepthFactor(IntPtr settings, float val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveVolumeSettingsGetDepthFactor(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetVoxelSize(IntPtr settings, float val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveVolumeSettingsGetVoxelSize(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetTsdfTruncateDistance(IntPtr settings, float val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveVolumeSettingsGetTsdfTruncateDistance(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetMaxDepth(IntPtr settings, float val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveVolumeSettingsGetMaxDepth(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetMaxWeight(IntPtr settings, int val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveVolumeSettingsGetMaxWeight(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetRaycastStepFactor(IntPtr settings, float val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveVolumeSettingsGetRaycastStepFactor(IntPtr settings);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetVolumePose(IntPtr settings, IntPtr val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsGetVolumePose(IntPtr settings, IntPtr val);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetVolumeResolution(IntPtr settings, IntPtr val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsGetVolumeResolution(IntPtr settings, IntPtr val);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsGetVolumeStrides(IntPtr settings, IntPtr val);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetCameraIntegrateIntrinsics(IntPtr settings, IntPtr val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsGetCameraIntegrateIntrinsics(IntPtr settings, IntPtr val);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsSetCameraRaycastIntrinsics(IntPtr settings, IntPtr val);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveVolumeSettingsGetCameraRaycastIntrinsics(IntPtr settings, IntPtr val);
    }
}
