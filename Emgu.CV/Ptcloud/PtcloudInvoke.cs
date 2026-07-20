//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Runtime.InteropServices;

namespace Emgu.CV
{
    public static partial class CvInvoke
    {
        /// <summary>
        /// The function loads point cloud from the specified file and returns it. If the cloud cannot be read, throws an error. Vertex coordinates, normals and colors are returned as they are saved in the file even if these arrays have different sizes and their elements do not correspond to each other (which is typical for OBJ files for example)
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="verticies">Vertex coordinates, each value contains 3 floats</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="rgb">Per-vertex colors, each value contains 3 floats</param>
        public static void LoadPointCloud(String filename, IOutputArray verticies, IOutputArray normals = null, IOutputArray rgb = null)
        {
            using (CvString csFileName = new CvString(filename))
            using (OutputArray oaVerticies = verticies.GetOutputArray())
            using (OutputArray oaNormals = normals == null ? OutputArray.GetEmpty() : normals.GetOutputArray())
            using (OutputArray oaRgb = rgb == null ? OutputArray.GetEmpty() : rgb.GetOutputArray())
            {
                cveLoadPointCloud(csFileName, oaVerticies, oaNormals, oaRgb);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveLoadPointCloud(IntPtr filename, IntPtr vertices, IntPtr normals, IntPtr rgb);

        /// <summary>
        /// Saves a point cloud to a specified file.
        /// The function saves point cloud to the specified file. File format is chosen based on the filename extension.
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="vertices">Vertex coordinates, each value contains 3 floats</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="rgb">Per-vertex colors, each value contains 3 floats</param>
        public static void SavePointCloud(String filename, IInputArray vertices, IInputArray normals = null, IInputArray rgb = null)
        {
            using (CvString csFileName = new CvString(filename))
            using (InputArray iaVertices = vertices.GetInputArray())
            using (InputArray iaNormals = normals == null ? InputArray.GetEmpty() : normals.GetInputArray())
            using (InputArray iaRgb = rgb == null ? InputArray.GetEmpty() : rgb.GetInputArray())
            {
                cveSavePointCloud(csFileName, iaVertices, iaNormals, iaRgb);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSavePointCloud(IntPtr filename, IntPtr vertices, IntPtr normals, IntPtr rgb);

        /// <summary>
        /// Loads a mesh from a file.
        /// The function loads mesh from the specified file and returns it.
        /// If the mesh cannot be read, throws an error Vertex attributes (i.e.space and texture coodinates, normals and colors) are returned in same-sized arrays with corresponding elements having the same indices.
        /// This means that if a face uses a vertex with a normal or a texture coordinate with different indices (which is typical for OBJ files for example), this vertex will be duplicated for each face it uses.
        /// Currently, the following file formats are supported:
        /// Wavefront obj file *.obj(ONLY TRIANGULATED FACES);
        /// Polygon File Format*.ply
        /// </summary>
        /// <param name="filename">Name of the file</param>
        /// <param name="vertices">vertex coordinates, each value contains 3 floats</param>
        /// <param name="indices">Per-face list of vertices, each value is a vector of ints</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="colors">Per-vertex colors, each value contains 3 floats</param>
        /// <param name="texCoords">Per-vertex texture coordinates, each value contains 2 or 3 floats</param>
        public static void LoadMesh(
            String filename,
            IOutputArray vertices,
            IOutputArrayOfArrays indices,
            IOutputArray normals = null,
            IOutputArray colors = null,
            IOutputArray texCoords = null)
        {
            using (CvString csFilename = new CvString(filename))
            using (OutputArray oaVertices = vertices.GetOutputArray())
            using (OutputArray oaIndices = indices.GetOutputArray())
            using (OutputArray oaNormals = normals == null ? OutputArray.GetEmpty() : normals.GetOutputArray())
            using (OutputArray oaColors = colors == null ? OutputArray.GetEmpty() : colors.GetOutputArray())
            using (OutputArray oaTexCoords = texCoords == null ? OutputArray.GetEmpty() : texCoords.GetOutputArray())
            {
                cveLoadMesh(csFilename, oaVertices, oaIndices, oaNormals, oaColors, oaTexCoords);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveLoadMesh(
            IntPtr filename,
            IntPtr vertices,
            IntPtr indices,
            IntPtr normals,
            IntPtr colors,
            IntPtr texCoords);

        /// <summary>
        /// Saves a mesh to a specified file.
        /// The function saves mesh to the specified file.File format is chosen based on the filename extension.
        /// </summary>
        /// <param name="filename">Name of the file.</param>
        /// <param name="vertices">Vertex coordinates, each value contains 3 floats</param>
        /// <param name="indices">Per-face list of vertices, each value is a vector of ints</param>
        /// <param name="normals">Per-vertex normals, each value contains 3 floats</param>
        /// <param name="colors">Per-vertex colors, each value contains 3 floats</param>
        /// <param name="texCoords">Per-vertex texture coordinates, each value contains 2 or 3 floats</param>
        public static void SaveMesh(
            String filename,
            IInputArray vertices,
            IInputArrayOfArrays indices,
            IInputArray normals = null,
            IInputArray colors = null,
            IInputArray texCoords = null)
        {
            using (CvString csFilename = new CvString(filename))
            using (InputArray iaVertices = vertices.GetInputArray())
            using (InputArray iaIndices = indices.GetInputArray())
            using (InputArray iaNormals = normals == null ? InputArray.GetEmpty() : normals.GetInputArray())
            using (InputArray iaColors = colors == null ? InputArray.GetEmpty() : colors.GetInputArray())
            using (InputArray iaTexCoords = texCoords == null ? InputArray.GetEmpty() : texCoords.GetInputArray())
            {
                cveSaveMesh(
                    csFilename,
                    iaVertices,
                    iaIndices,
                    iaNormals,
                    iaColors,
                    iaTexCoords);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveSaveMesh(
            IntPtr filename,
            IntPtr vertices,
            IntPtr indices,
            IntPtr normals,
            IntPtr colors,
            IntPtr texCoords);

        /// <summary>
        /// Converts a depth image to an array of 3D points.
        /// The coordinate system is x pointing right, y down, z away from the camera.
        /// </summary>
        /// <param name="depth">
        /// The depth image. CV_16U values are assumed to be depth in millimetres (Kinect convention);
        /// CV_32F or CV_64F values are assumed to be in metres.
        /// </param>
        /// <param name="K">The 3x3 camera calibration matrix.</param>
        /// <param name="points3d">
        /// Output array of 3D points with the same rows/cols as <paramref name="depth"/>
        /// (or a 1D vector when <paramref name="mask"/> is non-empty).
        /// Each point is a 4-channel value [x, y, z, 0].
        /// </param>
        /// <param name="mask">Optional mask; only pixels where mask != 0 are converted.</param>
        public static void DepthTo3d(IInputArray depth, IInputArray K, IOutputArray points3d, IInputArray mask = null)
        {
            using (InputArray iaDepth = depth.GetInputArray())
            using (InputArray iaK = K.GetInputArray())
            using (OutputArray oaPoints3d = points3d.GetOutputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            {
                cveDepthTo3d(iaDepth, iaK, oaPoints3d, iaMask);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDepthTo3d(IntPtr depth, IntPtr K, IntPtr points3d, IntPtr mask);

        /// <summary>
        /// Converts depth values at a sparse set of 2D image points to 3D points.
        /// </summary>
        /// <param name="depth">The depth image.</param>
        /// <param name="inK">The 3x3 camera calibration matrix.</param>
        /// <param name="inPoints">Nx2 array of (x, y) pixel coordinates to convert.</param>
        /// <param name="points3d">Output Nx4 array of 3D points [x, y, z, 0].</param>
        public static void DepthTo3dSparse(IInputArray depth, IInputArray inK, IInputArray inPoints, IOutputArray points3d)
        {
            using (InputArray iaDepth = depth.GetInputArray())
            using (InputArray iaK = inK.GetInputArray())
            using (InputArray iaPoints = inPoints.GetInputArray())
            using (OutputArray oaPoints3d = points3d.GetOutputArray())
            {
                cveDepthTo3dSparse(iaDepth, iaK, iaPoints, oaPoints3d);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveDepthTo3dSparse(IntPtr depth, IntPtr inK, IntPtr inPoints, IntPtr points3d);

        /// <summary>
        /// Rescales a depth image to floating-point metres.
        /// CV_16U input is divided by <paramref name="depthFactor"/> and zeros become NaN.
        /// CV_32F/CV_64F input is simply converted to the requested type.
        /// </summary>
        /// <param name="input">The input depth image.</param>
        /// <param name="type">Desired output depth type: CV_32F or CV_64F.</param>
        /// <param name="output">The rescaled depth image.</param>
        /// <param name="depthFactor">Scale factor applied to CV_16U input (default 1000 for Kinect mm→m).</param>
        public static void RescaleDepth(IInputArray input, CvEnum.DepthType type, IOutputArray output, double depthFactor = 1000.0)
        {
            using (InputArray iaInput = input.GetInputArray())
            using (OutputArray oaOutput = output.GetOutputArray())
            {
                cveRescaleDepth(iaInput, (int)type, oaOutput, depthFactor);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRescaleDepth(IntPtr input, int type, IntPtr output, double depthFactor);

        /// <summary>
        /// Registers (reprojects) a depth image from one camera into the frame of a second (e.g. RGB) camera.
        /// </summary>
        /// <param name="unregisteredCameraMatrix">Intrinsics of the depth camera.</param>
        /// <param name="registeredCameraMatrix">Intrinsics of the target (RGB) camera.</param>
        /// <param name="registeredDistCoeffs">Distortion coefficients of the target camera.</param>
        /// <param name="Rt">Rigid-body transform from depth camera to target camera (3x4 or 4x4).</param>
        /// <param name="unregisteredDepth">Input depth image.</param>
        /// <param name="outputImagePlaneSize">Output image size (width, height) of the target camera.</param>
        /// <param name="registeredDepth">Output depth image in the target camera frame.</param>
        /// <param name="depthDilation">If true, dilate the result to fill holes from occlusion.</param>
        public static void RegisterDepth(
            IInputArray unregisteredCameraMatrix,
            IInputArray registeredCameraMatrix,
            IInputArray registeredDistCoeffs,
            IInputArray Rt,
            IInputArray unregisteredDepth,
            System.Drawing.Size outputImagePlaneSize,
            IOutputArray registeredDepth,
            bool depthDilation = false)
        {
            using (InputArray iaUnregCam = unregisteredCameraMatrix.GetInputArray())
            using (InputArray iaRegCam = registeredCameraMatrix.GetInputArray())
            using (InputArray iaRegDist = registeredDistCoeffs.GetInputArray())
            using (InputArray iaRt = Rt.GetInputArray())
            using (InputArray iaDepth = unregisteredDepth.GetInputArray())
            using (OutputArray oaRegistered = registeredDepth.GetOutputArray())
            {
                cveRegisterDepth(iaUnregCam, iaRegCam, iaRegDist, iaRt, iaDepth, ref outputImagePlaneSize, oaRegistered, depthDilation);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRegisterDepth(
            IntPtr unregisteredCameraMatrix,
            IntPtr registeredCameraMatrix,
            IntPtr registeredDistCoeffs,
            IntPtr Rt,
            IntPtr unregisteredDepth,
            ref System.Drawing.Size outputImagePlaneSize,
            IntPtr registeredDepth,
            [MarshalAs(UnmanagedType.U1)] bool depthDilation);

        /// <summary>
        /// Warps a depth (and optionally RGB) image by reprojecting it into 3D, applying a rigid-body
        /// transform, and projecting back — useful for visualising odometry results.
        /// </summary>
        /// <param name="depth">Input depth image (CV_16U, CV_16S, CV_32F or CV_64F).</param>
        /// <param name="image">Optional RGB image to warp alongside the depth.</param>
        /// <param name="mask">Optional validity mask.</param>
        /// <param name="Rt">3x4 or 4x4 rotation+translation matrix to apply.</param>
        /// <param name="cameraMatrix">3x3 camera intrinsics matrix.</param>
        /// <param name="warpedDepth">Output warped depth image.</param>
        /// <param name="warpedImage">Output warped RGB image.</param>
        /// <param name="warpedMask">Output validity mask for the warped image.</param>
        public static void WarpFrame(
            IInputArray depth,
            IInputArray image,
            IInputArray mask,
            IInputArray Rt,
            IInputArray cameraMatrix,
            IOutputArray warpedDepth = null,
            IOutputArray warpedImage = null,
            IOutputArray warpedMask = null)
        {
            using (InputArray iaDepth = depth.GetInputArray())
            using (InputArray iaImage = image == null ? InputArray.GetEmpty() : image.GetInputArray())
            using (InputArray iaMask = mask == null ? InputArray.GetEmpty() : mask.GetInputArray())
            using (InputArray iaRt = Rt.GetInputArray())
            using (InputArray iaCam = cameraMatrix.GetInputArray())
            using (OutputArray oaWarpedDepth = warpedDepth == null ? OutputArray.GetEmpty() : warpedDepth.GetOutputArray())
            using (OutputArray oaWarpedImage = warpedImage == null ? OutputArray.GetEmpty() : warpedImage.GetOutputArray())
            using (OutputArray oaWarpedMask = warpedMask == null ? OutputArray.GetEmpty() : warpedMask.GetOutputArray())
            {
                cveWarpFrame(iaDepth, iaImage, iaMask, iaRt, iaCam, oaWarpedDepth, oaWarpedImage, oaWarpedMask);
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveWarpFrame(
            IntPtr depth,
            IntPtr image,
            IntPtr mask,
            IntPtr Rt,
            IntPtr cameraMatrix,
            IntPtr warpedDepth,
            IntPtr warpedImage,
            IntPtr warpedMask);
    }
}
