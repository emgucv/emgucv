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
            [MarshalAs(CvInvoke.BoolMarshalType)] bool depthDilation);

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

        /// <summary>
        /// Removes statistical outliers from a point cloud. For each point the mean distance to its meanK nearest neighbors is computed.
        /// Points whose mean distance exceeds global_mean + stddevMulThresh * global_stddev are dropped. Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="outputCloud">Output filtered point cloud, Nx1 CV_32FC3.</param>
        /// <param name="meanK">Number of nearest neighbors used to estimate the mean distance of each point.</param>
        /// <param name="stddevMulThresh">Standard-deviation multiplier for the distance threshold.</param>
        /// <param name="keptIndices">Optional output, indices (CV_32S) into the input cloud of the points that were kept.</param>
        public static void RemoveStatisticalOutliers(
            IInputArray inputCloud,
            IOutputArray outputCloud,
            int meanK = 20,
            double stddevMulThresh = 2.0,
            IOutputArray keptIndices = null)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (OutputArray oaOutputCloud = outputCloud.GetOutputArray())
            using (OutputArray oaKeptIndices = keptIndices == null ? OutputArray.GetEmpty() : keptIndices.GetOutputArray())
            {
                cveRemoveStatisticalOutliers(iaInputCloud, oaOutputCloud, meanK, stddevMulThresh, oaKeptIndices);
                CvInvoke.CheckError();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRemoveStatisticalOutliers(
            IntPtr inputCloud,
            IntPtr outputCloud,
            int meanK,
            double stddevMulThresh,
            IntPtr keptIndices);

        /// <summary>
        /// Removes radius outliers from a point cloud. Any point that has fewer than minNeighbors other points within radius is dropped.
        /// Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="outputCloud">Output filtered point cloud, Nx1 CV_32FC3.</param>
        /// <param name="radius">Search radius.</param>
        /// <param name="minNeighbors">Minimum number of neighbors (not counting the point itself) required to keep a point.</param>
        /// <param name="keptIndices">Optional output, indices (CV_32S) into the input cloud of the points that were kept.</param>
        public static void RemoveRadiusOutliers(
            IInputArray inputCloud,
            IOutputArray outputCloud,
            double radius,
            int minNeighbors = 16,
            IOutputArray keptIndices = null)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (OutputArray oaOutputCloud = outputCloud.GetOutputArray())
            using (OutputArray oaKeptIndices = keptIndices == null ? OutputArray.GetEmpty() : keptIndices.GetOutputArray())
            {
                cveRemoveRadiusOutliers(iaInputCloud, oaOutputCloud, radius, minNeighbors, oaKeptIndices);
                CvInvoke.CheckError();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveRemoveRadiusOutliers(
            IntPtr inputCloud,
            IntPtr outputCloud,
            double radius,
            int minNeighbors,
            IntPtr keptIndices);

        /// <summary>
        /// Flips point-cloud normals so that they face a given viewpoint. Each normal that points away from the viewpoint is negated.
        /// Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="normals">Per-point normals to orient in place, Nx1 CV_32FC3.</param>
        /// <param name="viewpoint">Point the normals should face towards (e.g. the camera center).</param>
        public static void OrientNormals(IInputArray inputCloud, IInputOutputArray normals, MCvPoint3D32f viewpoint)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (InputOutputArray ioaNormals = normals.GetInputOutputArray())
            {
                cveOrientNormals(iaInputCloud, ioaNormals, ref viewpoint);
                CvInvoke.CheckError();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveOrientNormals(IntPtr inputCloud, IntPtr normals, ref MCvPoint3D32f viewpoint);

        /// <summary>
        /// Orients point-cloud normals consistently across the whole cloud, by propagating a consistent sign along the minimum spanning tree
        /// of a k-nearest-neighbor graph. If the graph is disconnected the normals are oriented outward about the centroid instead.
        /// Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="normals">Per-point normals to orient in place, Nx1 CV_32FC3.</param>
        /// <param name="k">Number of nearest neighbors used to build the graph.</param>
        public static void OrientNormalsConsistent(IInputArray inputCloud, IInputOutputArray normals, int k = 30)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (InputOutputArray ioaNormals = normals.GetInputOutputArray())
            {
                cveOrientNormalsConsistent(iaInputCloud, ioaNormals, k);
                CvInvoke.CheckError();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveOrientNormalsConsistent(IntPtr inputCloud, IntPtr normals, int k);

        /// <summary>
        /// Estimates the median spacing between neighboring points of a point cloud, i.e. the median distance from a sampled set of points to
        /// their nearest neighbor. Useful for picking a ball radius for <see cref="CreateMeshBPA"/>. Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <returns>Median nearest-neighbor distance, or 0 if the cloud has fewer than 2 points.</returns>
        public static float EstimateMedianSpacing(IInputArray inputCloud)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            {
                float result = cveEstimateMedianSpacing(iaInputCloud);
                CvInvoke.CheckError();
                return result;
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern float cveEstimateMedianSpacing(IntPtr inputCloud);

        /// <summary>
        /// Reconstructs a triangle mesh from an oriented point cloud using Ball-Pivoting. The mesh vertices are the input points,
        /// so estimate and orient the normals first. Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="normals">Per-point normals, Nx1 CV_32FC3 (same count as the cloud).</param>
        /// <param name="vertices">Output mesh vertices (a copy of the input points), Nx1 CV_32FC3.</param>
        /// <param name="triangles">Output triangle vertex indices, Mx3 CV_32S.</param>
        /// <param name="radii">Optional ball radii (1D CV_32F/CV_64F). If null or empty, radii are derived from the median spacing as {1x, 2x, 4x}.</param>
        public static void CreateMeshBPA(
            IInputArray inputCloud,
            IInputArray normals,
            IOutputArray vertices,
            IOutputArray triangles,
            IInputArray radii = null)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (InputArray iaNormals = normals.GetInputArray())
            using (OutputArray oaVertices = vertices.GetOutputArray())
            using (OutputArray oaTriangles = triangles.GetOutputArray())
            using (InputArray iaRadii = radii == null ? InputArray.GetEmpty() : radii.GetInputArray())
            {
                cveCreateMeshBPA(iaInputCloud, iaNormals, oaVertices, oaTriangles, iaRadii);
                CvInvoke.CheckError();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveCreateMeshBPA(
            IntPtr inputCloud,
            IntPtr normals,
            IntPtr vertices,
            IntPtr triangles,
            IntPtr radii);

        /// <summary>
        /// Computes the axis-aligned bounding box of a point cloud. Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="minBound">Output minimum corner (x,y,z), 3x1 CV_32F.</param>
        /// <param name="maxBound">Output maximum corner (x,y,z), 3x1 CV_32F.</param>
        public static void BoundingBox3D(IInputArray inputCloud, IOutputArray minBound, IOutputArray maxBound)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (OutputArray oaMinBound = minBound.GetOutputArray())
            using (OutputArray oaMaxBound = maxBound.GetOutputArray())
            {
                cveBoundingBox3D(iaInputCloud, oaMinBound, oaMaxBound);
                CvInvoke.CheckError();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveBoundingBox3D(IntPtr inputCloud, IntPtr minBound, IntPtr maxBound);

        /// <summary>
        /// Computes a PCA-based oriented bounding box of a point cloud. The box axes are the principal axes of the cloud; this is
        /// not guaranteed to be the minimum-volume box. A point p is inside the box iff abs(axes * (p - center)) is at most halfExtents
        /// component-wise. Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="center">Output box center (x,y,z), 3x1 CV_32F.</param>
        /// <param name="axes">Output 3x3 CV_32F; each row is a unit box axis.</param>
        /// <param name="halfExtents">Output half-size along each axis, 3x1 CV_32F.</param>
        public static void OrientedBoundingBox3D(
            IInputArray inputCloud,
            IOutputArray center,
            IOutputArray axes,
            IOutputArray halfExtents)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (OutputArray oaCenter = center.GetOutputArray())
            using (OutputArray oaAxes = axes.GetOutputArray())
            using (OutputArray oaHalfExtents = halfExtents.GetOutputArray())
            {
                cveOrientedBoundingBox3D(iaInputCloud, oaCenter, oaAxes, oaHalfExtents);
                CvInvoke.CheckError();
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern void cveOrientedBoundingBox3D(
            IntPtr inputCloud,
            IntPtr center,
            IntPtr axes,
            IntPtr halfExtents);

        /// <summary>
        /// Computes an approximate minimum enclosing sphere of a point cloud (Ritter's algorithm). Available since OpenCV 5.1.
        /// </summary>
        /// <param name="inputCloud">Input point cloud, 3-channel float array (CV_32FC3), or an Nx3 / 3xN CV_32F matrix.</param>
        /// <param name="center">Output sphere center (x,y,z), 3x1 CV_32F.</param>
        /// <returns>Sphere radius, or 0 if the cloud is empty.</returns>
        public static double ApproxEnclosingSphere3D(IInputArray inputCloud, IOutputArray center)
        {
            using (InputArray iaInputCloud = inputCloud.GetInputArray())
            using (OutputArray oaCenter = center.GetOutputArray())
            {
                double result = cveApproxEnclosingSphere3D(iaInputCloud, oaCenter);
                CvInvoke.CheckError();
                return result;
            }
        }

        [DllImport(ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        private static extern double cveApproxEnclosingSphere3D(IntPtr inputCloud, IntPtr center);
    }
}
