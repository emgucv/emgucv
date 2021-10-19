//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// DNN-based face detector
    /// </summary>
    public partial class FaceDetectorYN : SharedPtrObject
    {
        /// <summary>
        /// Creates an instance of this class with given parameters.
        /// </summary>
        /// <param name="model">The path to the requested model</param>
        /// <param name="config">The path to the config file for compability, which is not requested for ONNX models</param>
        /// <param name="inputSize">The size of the input image</param>
        /// <param name="scoreThreshold">The threshold to filter out bounding boxes of score smaller than the given value</param>
        /// <param name="nmsThreshold">The threshold to suppress bounding boxes of IoU bigger than the given value</param>
        /// <param name="topK">Keep top K bboxes before NMS</param>
        /// <param name="backendId">The id of backend</param>
        /// <param name="targetId">The id of target device</param>
        public FaceDetectorYN(
            String model,
            String config,
            Size inputSize,
            float scoreThreshold,
            float nmsThreshold,
            int topK,
            Emgu.CV.Dnn.Backend backendId,
            Emgu.CV.Dnn.Target targetId)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = CvInvoke.cveFaceDetectorYNCreate(
                    csModel,
                    csConfig,
                    ref inputSize,
                    scoreThreshold,
                    nmsThreshold,
                    topK,
                    backendId, 
                    targetId,
                    ref _sharedPtr
                );
            }
        }

        /// <summary>
        /// A simple interface to detect face from given image.
        /// </summary>
        /// <param name="image">An image to detect</param>
        /// <param name="faces">Detection results stored in a Mat</param>
        /// <returns>1 if detection is successful, 0 otherwise.</returns>
        public int Detect(IInputArray image, IOutputArray faces)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaFaces = faces.GetOutputArray())
                return CvInvoke.cveFaceDetectorYNDetect(_ptr, iaImage, oaFaces);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this FaceDetectorYN
        /// </summary>
        protected override void DisposeObject()
        {
            if (!IntPtr.Zero.Equals(_sharedPtr))
            {
                CvInvoke.cveFaceDetectorYNRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveFaceDetectorYNCreate(
            IntPtr model,
            IntPtr config,
            ref Size inputSize,
            float scoreThreshold,
            float nmsThreshold,
            int topK,
            Emgu.CV.Dnn.Backend backendId,
            Emgu.CV.Dnn.Target targetId,
            ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveFaceDetectorYNRelease(ref IntPtr faceDetector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern int cveFaceDetectorYNDetect(IntPtr faceDetector, IntPtr image, IntPtr faces);
        
    }

}