//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// This class represents high-level API for object detection networks.
    /// </summary>
    public partial class DetectionModel : Model
    {
        /// <summary>
        /// Create detection model from network represented in one of the supported formats.
        /// </summary>
        /// <param name="model">Binary file contains trained weights.</param>
        /// <param name="config">Text file contains network configuration.</param>
        public DetectionModel(String model, String config = null)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveDnnDetectionModelCreate1(
                    csModel,
                    csConfig,
                    ref _model);
            }
        }

        /// <summary>
        /// Create model from deep learning network.
        /// </summary>
        /// <param name="net">DNN Network</param>
        public DetectionModel(Net net)
        {
            _ptr = DnnInvoke.cveDnnDetectionModelCreate2(
                net,
                ref _model);
        }

        /// <summary>
        /// Given the input frame, create input blob, run net and return result detections.
        /// </summary>
        /// <param name="frame">The input image.</param>
        /// <param name="classIds">Class indexes in result detection.</param>
        /// <param name="confidences">A set of corresponding confidences.</param>
        /// <param name="boxes">A set of bounding boxes.</param>
        /// <param name="confThreshold">A threshold used to filter boxes by confidences.</param>
        /// <param name="nmsThreshold">A threshold used in non maximum suppression. The default value 0 means we will not perform non-maximum supression.</param>
        public void Detect(
            IInputArray frame,
            VectorOfInt classIds,
            VectorOfFloat confidences,
            VectorOfRect boxes,
            float confThreshold = 0.5f,
            float nmsThreshold = 0.0f)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnDetectionModelDetect(
                    _ptr,
                    iaFrame,
                    classIds,
                    confidences,
                    boxes,
                    confThreshold,
                    nmsThreshold);
            }
        }

        /// <summary>
        /// Release the memory associated with this detection model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnDetectionModelRelease(ref _ptr);
            }
            _model = IntPtr.Zero;
        }

    }

    public static partial class DnnInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnDetectionModelCreate1(IntPtr model, IntPtr config, ref IntPtr baseModel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnDetectionModelCreate2(IntPtr network, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnDetectionModelRelease(ref IntPtr model);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnDetectionModelDetect(
            IntPtr detectionModel,
            IntPtr frame,
            IntPtr classIds,
            IntPtr confidences,
            IntPtr boxes,
            float confThreshold,
            float nmsThreshold);

    }
}
