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

    public partial class  DetectionModel : Model
    {
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

        public DetectionModel(Net net)
        {

            _ptr = DnnInvoke.cveDnnDetectionModelCreate2(
                net,
                ref _model);

        }

        public void Detect(
            IInputArray frame,
            VectorOfInt classIds,
            VectorOfFloat confidences,
            VectorOfRect boxes,
            float confThreshold = 0.5f,
            float nmsThreshold = 0.5f)
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

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnDetectionModelRelease(ref _ptr);
            }
            base.DisposeObject();
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
