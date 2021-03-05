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
    /// Base class for text detection networks.
    /// </summary>
    public abstract class TextDetectionModel : Model
    {
        /// <summary>
        /// The pointer to the TextDetectionModel object
        /// </summary>
        protected IntPtr _textDetectionModel;

        /// <summary>
        /// Given the input frame, prepare network input, run network inference, post-process network output and return result detections.
        /// </summary>
        /// <param name="frame">The input image</param>
        /// <param name="detections">Array with detections' quadrangles (4 points per result) in this order: bottom-left, top-left, top-right, bottom-right</param>
        /// <param name="confidences">Array with detection confidences</param>
        public void Detect(IInputArray frame, VectorOfVectorOfPoint detections, VectorOfFloat confidences)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnTextDetectionModelDetect(_textDetectionModel, iaFrame, detections, confidences);
            }
        }

        /// <summary>
        /// Given the input frame, prepare network input, run network inference, post-process network output and return result detections.
        /// </summary>
        /// <param name="frame">The input image</param>
        /// <param name="detections">Array with detections' RotationRect results</param>
        /// <param name="confidences">Array with detection confidences</param>
        public void DetectTextRectangles(IInputArray frame, VectorOfRotatedRect detections, VectorOfFloat confidences)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnTextDetectionModelDetectTextRectangles(_textDetectionModel, iaFrame, detections, confidences);
            }
        }

        /// <summary>
        /// Release the memory associated with this text detection model.
        /// </summary>
        protected override void DisposeObject()
        {
            _textDetectionModel = IntPtr.Zero;
            _model = IntPtr.Zero;     
        }
    }

    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextDetectionModelDetect(
            IntPtr textDetectionModel,
            IntPtr frame,
            IntPtr detections,
            IntPtr confidences
        );

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextDetectionModelDetectTextRectangles(
            IntPtr textDetectionModel,
            IntPtr frame,
            IntPtr detections,
            IntPtr confidences
        );
    }
}
