//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.Util;

namespace Emgu.CV.Features
{
    /// <summary>
    /// Wrapped LightGlueMatcher, a deep learning based descriptor matcher introduced
    /// in OpenCV 5. Requires the LightGlue ONNX model file.
    /// </summary>
    public class LightGlueMatcher : DescriptorMatcher
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a LightGlueMatcher from a model file path.
        /// </summary>
        /// <param name="modelPath">Path to the LightGlue ONNX model file.</param>
        /// <param name="scoreThreshold">Match confidence threshold.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public LightGlueMatcher(
            String modelPath,
            float scoreThreshold = 0.0f,
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            using (CvString csModelPath = new CvString(modelPath))
                _ptr = FeaturesInvoke.cveLightGlueMatcherCreate(
                    csModelPath,
                    scoreThreshold,
                    backend,
                    target,
                    ref _descriptorMatcherPtr,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Create a LightGlueMatcher from in-memory model data. Intended for cases where the
        /// model is read from application resources (for example Android assets) and is not
        /// available as a path on the filesystem.
        /// </summary>
        /// <param name="modelData">Buffer containing the contents of the LightGlue ONNX model.</param>
        /// <param name="scoreThreshold">Match confidence threshold.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public LightGlueMatcher(
            byte[] modelData,
            float scoreThreshold = 0.0f,
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            using (Util.VectorOfByte vbModelData = new Util.VectorOfByte(modelData))
                _ptr = FeaturesInvoke.cveLightGlueMatcherCreateFromMemory(
                    vbModelData,
                    scoreThreshold,
                    backend,
                    target,
                    ref _descriptorMatcherPtr,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Sets the keypoint and image size context for the next match call. This provides
        /// the spatial context that LightGlue needs in addition to descriptors. Must be
        /// called before Match/KnnMatch/RadiusMatch unless using automatic context from
        /// in-process ALIKED instances.
        /// </summary>
        /// <param name="queryKpts">Query image keypoints (Nx2 float matrix with x,y coordinates).</param>
        /// <param name="trainKpts">Train image keypoints (Nx2 float matrix with x,y coordinates).</param>
        /// <param name="queryImageSize">Size of the query image (width, height).</param>
        /// <param name="trainImageSize">Size of the train image (width, height).</param>
        public void SetPairInfo(IInputArray queryKpts, IInputArray trainKpts, Size queryImageSize = new Size(), Size trainImageSize = new Size())
        {
            using (InputArray iaQueryKpts = queryKpts.GetInputArray())
            using (InputArray iaTrainKpts = trainKpts.GetInputArray())
                FeaturesInvoke.cveLightGlueMatcherSetPairInfo(_ptr, iaQueryKpts, iaTrainKpts, ref queryImageSize, ref trainImageSize);
        }

        /// <summary>
        /// Clears stored pair context information.
        /// </summary>
        public void ClearPairInfo()
        {
            FeaturesInvoke.cveLightGlueMatcherClearPairInfo(_ptr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                FeaturesInvoke.cveLightGlueMatcherRelease(ref _sharedPtr);
            _ptr = IntPtr.Zero;
            base.DisposeObject();
        }
    }

    public static partial class FeaturesInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLightGlueMatcherCreate(
            IntPtr modelPath,
            float scoreThreshold,
            Emgu.CV.Dnn.Backend backend,
            Emgu.CV.Dnn.Target target,
            ref IntPtr matcher,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLightGlueMatcherCreateFromMemory(
            IntPtr modelData,
            float scoreThreshold,
            Emgu.CV.Dnn.Backend backend,
            Emgu.CV.Dnn.Target target,
            ref IntPtr matcher,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLightGlueMatcherSetPairInfo(
            IntPtr matcher,
            IntPtr queryKpts,
            IntPtr trainKpts,
            ref Size queryImageSize,
            ref Size trainImageSize);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLightGlueMatcherClearPairInfo(IntPtr matcher);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLightGlueMatcherRelease(ref IntPtr sharedPtr);
    }
}
