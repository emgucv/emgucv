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
    /// Wrapped ALIKED detector and descriptor, a deep learning based local feature
    /// extractor introduced in OpenCV 5. Requires the ALIKED ONNX model file.
    /// </summary>
    public class ALIKED : Feature2D
    {
        /// <summary>
        /// Create an ALIKED detector from a model file path.
        /// </summary>
        /// <param name="modelPath">Path to the ALIKED ONNX model file.</param>
        /// <param name="inputSize">Input image size for the network. Use an empty size (the default) for the network's default of 640x640.</param>
        /// <param name="normalizeDescriptors">Whether to L2-normalize descriptors.</param>
        /// <param name="engine">DNN engine type.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public ALIKED(
            String modelPath,
            Size inputSize = new Size(),
            bool normalizeDescriptors = true,
            Emgu.CV.Dnn.EngineType engine = Emgu.CV.Dnn.EngineType.New,
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            using (CvString csModelPath = new CvString(modelPath))
                _ptr = FeaturesInvoke.cveALIKEDCreate(
                    csModelPath,
                    ref inputSize,
                    normalizeDescriptors,
                    engine,
                    backend,
                    target,
                    ref _feature2D,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Create an ALIKED detector from in-memory model data. Intended for cases where the
        /// model is read from application resources (for example Android assets) and is not
        /// available as a path on the filesystem.
        /// </summary>
        /// <param name="modelData">Buffer containing the contents of the ALIKED ONNX model.</param>
        /// <param name="inputSize">Input image size for the network. Use an empty size (the default) for the network's default of 640x640.</param>
        /// <param name="normalizeDescriptors">Whether to L2-normalize descriptors.</param>
        /// <param name="engine">DNN engine type.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public ALIKED(
            byte[] modelData,
            Size inputSize = new Size(),
            bool normalizeDescriptors = true,
            Emgu.CV.Dnn.EngineType engine = Emgu.CV.Dnn.EngineType.New,
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            using (Util.VectorOfByte vbModelData = new Util.VectorOfByte(modelData))
                _ptr = FeaturesInvoke.cveALIKEDCreateFromMemory(
                    vbModelData,
                    ref inputSize,
                    normalizeDescriptors,
                    engine,
                    backend,
                    target,
                    ref _feature2D,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Protected constructor for subclasses that defer native initialization (e.g. until a
        /// model file has been downloaded). The native ALIKED object is not created; subclasses
        /// must call <see cref="InitFromModelPath"/> before using any Feature2D methods.
        /// </summary>
        protected ALIKED() { }

        /// <summary>
        /// Initialise the native ALIKED object from a model file path. Intended for subclasses
        /// that deferred construction via the protected parameterless constructor.
        /// </summary>
        protected void InitFromModelPath(
            String modelPath,
            Size inputSize = new Size(),
            bool normalizeDescriptors = true,
            Emgu.CV.Dnn.EngineType engine = Emgu.CV.Dnn.EngineType.New,
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            using (CvString csModelPath = new CvString(modelPath))
                _ptr = FeaturesInvoke.cveALIKEDCreate(
                    csModelPath, ref inputSize, normalizeDescriptors,
                    engine, backend, target,
                    ref _feature2D, ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                FeaturesInvoke.cveALIKEDRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class FeaturesInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveALIKEDCreate(
            IntPtr modelPath,
            ref Size inputSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool normalizeDescriptors,
            Emgu.CV.Dnn.EngineType engine,
            Emgu.CV.Dnn.Backend backend,
            Emgu.CV.Dnn.Target target,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveALIKEDCreateFromMemory(
            IntPtr modelData,
            ref Size inputSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool normalizeDescriptors,
            Emgu.CV.Dnn.EngineType engine,
            Emgu.CV.Dnn.Backend backend,
            Emgu.CV.Dnn.Target target,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveALIKEDRelease(ref IntPtr sharedPtr);
    }
}
