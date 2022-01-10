//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
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
    /// This class represents high-level API for classification models.
    /// </summary>
    public partial class ClassificationModel : Model
    {
        /// <summary>
        /// Create a new classification model
        /// </summary>
        /// <param name="model">Binary file contains trained weights.</param>
        /// <param name="config">Text file contains network configuration.</param>
        public ClassificationModel(String model, String config = null)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveDnnClassificationModelCreate1(
                    csModel,
                    csConfig,
                    ref _model);
            }
        }

        /// <summary>
        /// Create model from deep learning network.
        /// </summary>
        /// <param name="net">DNN Network</param>
        public ClassificationModel(Net net)
        {
            _ptr = DnnInvoke.cveDnnClassificationModelCreate2(
                net,
                ref _model);
        }

        /// <summary>
        /// Given the input frame, create input blob, run net and return top-1 prediction.
        /// </summary>
        /// <param name="frame">The input image.</param>
        /// <param name="classId">The top label.</param>
        /// <param name="conf">The confident of the classification.</param>
        public void Classify(
            IInputArray frame,
            out int classId,
            out float conf)
        {
            classId = -1;
            conf = 0;

            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnClassificationModelClassify(
                    _ptr,
                    iaFrame,
                    ref classId,
                    ref conf);
            }
        }

        /// <summary>
        /// Release the memory associated with this detection model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnClassificationModelRelease(ref _ptr);
            }
            _model = IntPtr.Zero;

        }

    }

    public static partial class DnnInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnClassificationModelCreate1(IntPtr model, IntPtr config, ref IntPtr baseModel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnClassificationModelCreate2(IntPtr network, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnClassificationModelRelease(ref IntPtr model);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnClassificationModelClassify(
            IntPtr classificationModel,
            IntPtr frame,
            ref int classId,
            ref float conf);

    }
}
