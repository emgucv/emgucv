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
    /// Wrapped XFeat detector and descriptor, a deep learning based local feature
    /// extractor introduced in OpenCV 5.1. Requires the XFeat ONNX model file.
    /// </summary>
    public class XFeat : Feature2D
    {
        /// <summary>
        /// Create an XFeat detector from a model file path.
        /// </summary>
        /// <param name="modelPath">Path to the XFeat ONNX model file.</param>
        /// <param name="maxKeypoints">Maximum number of keypoints to return per image. The strongest responses are kept; -1 keeps all detections.</param>
        /// <param name="scoreThreshold">Discard keypoints with network score not greater than this value.</param>
        /// <param name="inputSize">Input size fed to the network. Use an empty size (the default) for the OpenCV default of 640x640.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public XFeat(
            String modelPath,
            int maxKeypoints = -1,
            float scoreThreshold = 0.5f,
            Size inputSize = new Size(),
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            InitFromModelPath(modelPath, maxKeypoints, scoreThreshold, inputSize, backend, target);
        }

        /// <summary>
        /// Create an XFeat detector from in-memory model data. Intended for cases where the
        /// model is read from application resources (for example Android assets) and is not
        /// available as a path on the filesystem.
        /// </summary>
        /// <param name="modelData">Buffer containing the contents of the XFeat ONNX model.</param>
        /// <param name="maxKeypoints">Maximum number of keypoints to return per image. The strongest responses are kept; -1 keeps all detections.</param>
        /// <param name="scoreThreshold">Discard keypoints with network score not greater than this value.</param>
        /// <param name="inputSize">Input size fed to the network. Use an empty size (the default) for the OpenCV default of 640x640.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public XFeat(
            byte[] modelData,
            int maxKeypoints = -1,
            float scoreThreshold = 0.5f,
            Size inputSize = new Size(),
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            if (inputSize.IsEmpty)
                inputSize = new Size(640, 640);
            using (Util.VectorOfByte vbModelData = new Util.VectorOfByte(modelData))
                _ptr = FeaturesInvoke.cveXFeatCreateFromMemory(
                    vbModelData,
                    maxKeypoints,
                    scoreThreshold,
                    ref inputSize,
                    backend,
                    target,
                    ref _feature2D,
                    ref _sharedPtr);
            CvInvoke.CheckError();
        }

        /// <summary>
        /// Protected constructor for subclasses that defer native initialization (e.g. until a
        /// model file has been downloaded). The native XFeat object is not created; subclasses
        /// must call <see cref="InitFromModelPath"/> before using any Feature2D methods.
        /// </summary>
        protected XFeat() { }

        /// <summary>
        /// Initialise the native XFeat object from a model file path. Intended for subclasses
        /// that deferred construction via the protected parameterless constructor.
        /// </summary>
        protected void InitFromModelPath(
            String modelPath,
            int maxKeypoints = -1,
            float scoreThreshold = 0.5f,
            Size inputSize = new Size(),
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            if (inputSize.IsEmpty)
                inputSize = new Size(640, 640);
            using (CvString csModelPath = new CvString(modelPath))
                _ptr = FeaturesInvoke.cveXFeatCreate(
                    csModelPath, maxKeypoints, scoreThreshold,
                    ref inputSize, backend, target,
                    ref _feature2D, ref _sharedPtr);
            CvInvoke.CheckError();
        }

        /// <summary>
        /// Get or set the maximum number of keypoints to return per image. -1 keeps all detections.
        /// </summary>
        public int MaxKeypoints
        {
            get
            {
                int result = FeaturesInvoke.cveXFeatGetMaxKeypoints(_ptr);
                CvInvoke.CheckError();
                return result;
            }
            set
            {
                FeaturesInvoke.cveXFeatSetMaxKeypoints(_ptr, value);
                CvInvoke.CheckError();
            }
        }

        /// <summary>
        /// Get or set the score threshold. Keypoints with network score not greater than this value are discarded.
        /// </summary>
        public float ScoreThreshold
        {
            get
            {
                float result = FeaturesInvoke.cveXFeatGetScoreThreshold(_ptr);
                CvInvoke.CheckError();
                return result;
            }
            set
            {
                FeaturesInvoke.cveXFeatSetScoreThreshold(_ptr, value);
                CvInvoke.CheckError();
            }
        }

        /// <summary>
        /// Get or set the input size fed to the network.
        /// </summary>
        public Size InputSize
        {
            get
            {
                Size result = new Size();
                FeaturesInvoke.cveXFeatGetInputSize(_ptr, ref result);
                CvInvoke.CheckError();
                return result;
            }
            set
            {
                FeaturesInvoke.cveXFeatSetInputSize(_ptr, ref value);
                CvInvoke.CheckError();
            }
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                FeaturesInvoke.cveXFeatRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class FeaturesInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveXFeatCreate(
            IntPtr modelPath,
            int maxKeypoints,
            float scoreThreshold,
            ref Size inputSize,
            Emgu.CV.Dnn.Backend backend,
            Emgu.CV.Dnn.Target target,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveXFeatCreateFromMemory(
            IntPtr modelData,
            int maxKeypoints,
            float scoreThreshold,
            ref Size inputSize,
            Emgu.CV.Dnn.Backend backend,
            Emgu.CV.Dnn.Target target,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveXFeatSetMaxKeypoints(IntPtr xfeat, int maxKeypoints);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveXFeatGetMaxKeypoints(IntPtr xfeat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveXFeatSetScoreThreshold(IntPtr xfeat, float threshold);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern float cveXFeatGetScoreThreshold(IntPtr xfeat);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveXFeatSetInputSize(IntPtr xfeat, ref Size inputSize);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveXFeatGetInputSize(IntPtr xfeat, ref Size inputSize);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveXFeatRelease(ref IntPtr sharedPtr);
    }
}
