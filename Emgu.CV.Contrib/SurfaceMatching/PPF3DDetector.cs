//----------------------------------------------------------------------------
//  Copyright (C) 2004-2023 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.PpfMatch3d
{
    /// <summary>
    /// Class, allowing the load and matching 3D models.
    /// </summary>
    public class PPF3DDetector : UnmanagedObject
    {
        /// <summary>
        /// Create a new PPF3DDetector
        /// </summary>
        /// <param name="relativeSamplingStep">Sampling distance relative to the object's diameter. Models are first sampled uniformly in order to improve efficiency. Decreasing this value leads to a denser model, and a more accurate pose estimation but the larger the model, the slower the training. Increasing the value leads to a less accurate pose computation but a smaller model and faster model generation and matching. Beware of the memory consumption when using small values.</param>
        /// <param name="relativeDistanceStep">The discretization distance of the point pair distance relative to the model's diameter. This value has a direct impact on the hashtable. Using small values would lead to too fine discretization, and thus ambiguity in the bins of hashtable. Too large values would lead to no discrimination over the feature vectors and different point pair features would be assigned to the same bin. This argument defaults to the value of RelativeSamplingStep. For noisy scenes, the value can be increased to improve the robustness of the matching against noisy points.</param>
        /// <param name="numAngles">Set the discretization of the point pair orientation as the number of subdivisions of the angle. This value is the equivalent of RelativeDistanceStep for the orientations. Increasing the value increases the precision of the matching but decreases the robustness against incorrect normal directions. Decreasing the value decreases the precision of the matching but increases the robustness against incorrect normal directions. For very noisy scenes where the normal directions can not be computed accurately, the value can be set to 25 or 20.</param>
        public PPF3DDetector(
            double relativeSamplingStep=0.05, 
            double relativeDistanceStep=0.05, 
            double numAngles = 30)
        {
            _ptr = PpfMatch3dInvoke.cvePPF3DDetectorCreate(relativeSamplingStep, relativeDistanceStep, numAngles);
        }

        /// <summary>
        /// Trains a new model.
        /// </summary>
        /// <param name="model">Model The input point cloud with normals (Nx6)</param>
        /// <remarks>Uses the parameters set in the constructor to downsample and learn a new model. When the model is learn, the instance gets ready for calling "match".</remarks>
        public void TrainModel(Mat model)
        {
            PpfMatch3dInvoke.cvePPF3DDetectorTrainModel(_ptr, model);
        }

        /// <summary>
        /// Matches a trained model across a provided scene.
        /// </summary>
        /// <param name="scene">Scene Point cloud for the scene</param>
        /// <param name="results">Results List of output poses</param>
        /// <param name="relativeSceneSampleStep">The ratio of scene points to be used for the matching after sampling with relativeSceneDistance. For example, if this value is set to 1.0/5.0, every 5th point from the scene is used for pose estimation. This parameter allows an easy trade-off between speed and accuracy of the matching. Increasing the value leads to less points being used and in turn to a faster but less accurate pose computation. Decreasing the value has the inverse effect.</param>
        /// <param name="relativeSceneDistance">Set the distance threshold relative to the diameter of the model. This parameter is equivalent to relativeSamplingStep in the training stage. This parameter acts like a prior sampling with the relativeSceneSampleStep parameter.</param>
        public void Match(
            Mat scene, 
            VectorOfPose3D results, 
            double relativeSceneSampleStep,
            double relativeSceneDistance)
        {
            PpfMatch3dInvoke.cvePPF3DDetectorMatch(_ptr, scene, results, relativeSceneSampleStep, relativeSceneDistance);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this 3d detector
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                PpfMatch3dInvoke.cvePPF3DDetectorRelease(ref _ptr);
            }
        }
    }

    public static partial class PpfMatch3dInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cvePPF3DDetectorCreate(double relativeSamplingStep, double relativeDistanceStep, double numAngles);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePPF3DDetectorRelease(ref IntPtr detector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePPF3DDetectorTrainModel(IntPtr detector, IntPtr model);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cvePPF3DDetectorMatch(IntPtr detector, IntPtr scene, IntPtr results, double relativeSceneSampleStep, double relativeSceneDistance);

        /// <summary>
        /// Load a PLY file.
        /// </summary>
        /// <param name="fileName">The PLY model to read</param>
        /// <param name="withNormals">Wheather the input PLY contains normal information, and whether it should be loaded or not</param>
        /// <param name="result">The data loaded from the ply model</param>
        public static void LoadPLYSimple(String fileName, int withNormals, IOutputArray result)
        {
            using (CvString csFileName = new CvString(fileName))
            using (OutputArray oaResult = result.GetOutputArray())
            {
                cveLoadPLYSimple(csFileName, withNormals, oaResult);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLoadPLYSimple(IntPtr fileName, int withNormals, IntPtr result);

        /// <summary>
        /// Transforms the point cloud with a given a homogeneous 4x4 pose matrix (in double precision)
        /// </summary>
        /// <param name="pc">Input point cloud (CV_32F family). Point clouds with 3 or 6 elements per row are expected. In the case where the normals are provided, they are also rotated to be compatible with the entire transformation</param>
        /// <param name="pose">4x4 pose matrix, but linearized in row-major form.</param>
        /// <param name="result">Transformed point cloud</param>
        public static void TransformPCPose(Mat pc, Mat pose, IOutputArray result)
        {
            using (OutputArray oaResult = result.GetOutputArray())
            {
                cveTransformPCPose(pc, pose, oaResult);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTransformPCPose(IntPtr pc, IntPtr pose, IntPtr result);
    

        /// <summary>
        /// Sample a point cloud using uniform steps
        /// </summary>
        /// <param name="pc">Input point cloud (CV_32F family). Point clouds with 3 or 6 elements per row are expected. In the case where the normals are provided, they are also rotated to be compatible with the entire transformation</param>
        /// <param name="xRange">X components (min and max) of the bounding box of the model</param>
        /// <param name="yRange">Y components (min and max) of the bounding box of the model</param>
        /// <param name="zRange">Z components (min and max) of the bounding box of the model</param>
        /// <param name="sampleStepRelative">The point cloud is sampled such that all points have a certain minimum distance. This minimum distance is determined relatively using the parameter sampleStepRelative.</param>
        /// <param name="weightByCenter">The contribution of the quantized data points can be weighted by the distance to the origin. This parameter enables/disables the use of weighting.</param>
        /// <param name="result">Sampled point cloud</param>
        public static void SamplePCByQuantization(Mat pc, float[] xRange, float[] yRange, float[] zRange, float sampleStepRelative, int weightByCenter, IOutputArray result)
        {
            using (OutputArray oaResult = result.GetOutputArray())
            using (var vfXrange = new VectorOfFloat(xRange))
            using (var vfYrange = new VectorOfFloat(yRange))
            using (var vfZrange = new VectorOfFloat(zRange))
            {
                PpfMatch3dInvoke.cveSamplePCByQuantization(pc, vfXrange, vfYrange, vfZrange, sampleStepRelative, weightByCenter, oaResult);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveSamplePCByQuantization(IntPtr pc, IntPtr xRange, IntPtr yRange, IntPtr zRange, float sampleStepRelative,  int weightByCenter, IntPtr result);

        /// <summary>
        /// Compute the normals of an arbitrary point cloud 
        /// computeNormalsPC3d uses a plane fitting approach to smoothly compute local normals. Normals are obtained through the eigenvector of the covariance matrix, corresponding to the smallest eigen value. If PCNormals is provided to be an Nx6 matrix, then no new allocation is made, instead the existing memory is overwritten.
        /// </summary>
        /// <param name="pc">Input point cloud (CV_32F family). Point clouds with 3 or 6 elements per row are expected. In the case where the normals are provided, they are also rotated to be compatible with the entire transformation</param>
        /// <param name="pcNormals">Output point cloud</param>
        /// <param name="numNeighbors">Number of neighbors to take into account in a local region</param>
        /// <param name="flipViewpoint">Should normals be flipped to a viewing direction?</param>
        /// <param name="viewpoint">viewpoint</param>
        /// <returns>Returns 0 on success</returns>
        public static int ComputeNormalsPC3d(Mat pc, Mat pcNormals, int numNeighbors, bool flipViewpoint, float[] viewpoint)
        {
            using (var vfViewpoint = new VectorOfFloat(viewpoint))
            {
                return PpfMatch3dInvoke.cveComputeNormalsPC3d(pc, pcNormals, numNeighbors, flipViewpoint, vfViewpoint);
            }
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveComputeNormalsPC3d(IntPtr pc, IntPtr pcNormals, int numNeighbors, bool flipViewpoint, IntPtr viewpoint);
    }
}