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

    public abstract class TextDetectionModel : Model
    {

        /// <summary>
        /// The pointer to the TextDetectionModel object
        /// </summary>
        protected IntPtr _textDetectionModel;

        public void Detect(IInputArray frame, VectorOfVectorOfPoint detections, VectorOfFloat confidences)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnTextDetectionModelDetect(_textDetectionModel, iaFrame, detections, confidences);
            }
        }

        public void DetectTextRectangles(IInputArray frame, VectorOfRotatedRect detections, VectorOfFloat confidences)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnTextDetectionModelDetectTextRectangles(_textDetectionModel, iaFrame, detections, confidences);
            }
        }

        protected override void DisposeObject()
        {
            if (_textDetectionModel != IntPtr.Zero)
            {
                _textDetectionModel = IntPtr.Zero;
            }
            base.DisposeObject();
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
