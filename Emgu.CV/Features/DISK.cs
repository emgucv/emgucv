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
    /// Wrapped DISK detector and descriptor, a deep learning based local feature
    /// extractor introduced in OpenCV 5. Requires the DISK ONNX model file.
    /// </summary>
    public class DISK : Feature2D
    {
        /// <summary>
        /// Create a DISK detector from a model file path.
        /// </summary>
        /// <param name="modelPath">Path to the DISK ONNX model file.</param>
        /// <param name="maxKeypoints">Maximum number of keypoints to return per image. The strongest responses (by network score) are kept; -1 keeps all detections.</param>
        /// <param name="scoreThreshold">Discard keypoints with network score strictly below this value.</param>
        /// <param name="imageSize">Target input size (width, height) fed to the network. Use an empty size (the default) to fall back to the network's expected fixed input shape of 1024x1024. When overriding, both dimensions must be positive multiples of 16, since DISK downsamples by a factor of 16.</param>
        /// <param name="backend">DNN backend.</param>
        /// <param name="target">DNN target.</param>
        public DISK(
            String modelPath,
            int maxKeypoints = -1,
            float scoreThreshold = 0.0f,
            Size imageSize = new Size(),
            Emgu.CV.Dnn.Backend backend = Emgu.CV.Dnn.Backend.Default,
            Emgu.CV.Dnn.Target target = Emgu.CV.Dnn.Target.Cpu)
        {
            using (CvString csModelPath = new CvString(modelPath))
                _ptr = FeaturesInvoke.cveDISKCreate(
                    csModelPath,
                    maxKeypoints,
                    scoreThreshold,
                    ref imageSize,
                    backend,
                    target,
                    ref _feature2D,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
                FeaturesInvoke.cveDISKRelease(ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class FeaturesInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDISKCreate(
            IntPtr modelPath,
            int maxKeypoints,
            float scoreThreshold,
            ref Size imageSize,
            Emgu.CV.Dnn.Backend backend,
            Emgu.CV.Dnn.Target target,
            ref IntPtr feature2D,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDISKRelease(ref IntPtr sharedPtr);
    }
}
