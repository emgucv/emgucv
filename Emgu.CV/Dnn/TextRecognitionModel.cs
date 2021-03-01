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
    public partial class TextRecognitionModel : Model
    {
        public TextRecognitionModel(String model, String config = null)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveDnnTextRecognitionModelCreate1(
                    csModel,
                    csConfig,
                    ref _model);
            }
        }

        public TextRecognitionModel(Net net)
        {

            _ptr = DnnInvoke.cveDnnTextRecognitionModelCreate2(
                net,
                ref _model);

        }

        public String[] Vocabulary
        {
            get
            {
                using (VectorOfCvString vs = new VectorOfCvString())
                {
                    DnnInvoke.cveDnnTextRecognitionModelGetVocabulary(_ptr, vs);
                    return vs.ToArray();
                }
            }
            set
            {
                using (VectorOfCvString vs = new VectorOfCvString(value))
                {
                    DnnInvoke.cveDnnTextRecognitionModelSetVocabulary(_ptr, vs);
                }
            }
        }

        public String Recognize(IInputArray frame)
        {
            using (CvString s = new CvString())
            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnTextRecognitionModelRecognize1(_ptr, iaFrame, s);
                return s.ToString();
            }
        }

        public String[] Recognize(IInputArray frame, IInputArrayOfArrays roiRects)
        {
            using (VectorOfCvString vs = new VectorOfCvString())
            using (InputArray iaFrame = frame.GetInputArray())
            using (InputArray iaRoiRects = roiRects.GetInputArray())
            {
                DnnInvoke.cveDnnTextRecognitionModelRecognize2(_ptr, iaFrame, iaRoiRects, vs);
                return vs.ToArray();
            }
        }

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnTextRecognitionModelRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    public static partial class DnnInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnTextRecognitionModelCreate1(
            IntPtr model,
            IntPtr config,
            ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnTextRecognitionModelCreate2(
            IntPtr network,
            ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextRecognitionModelRelease(ref IntPtr textDetectionModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextRecognitionModelSetVocabulary(IntPtr textRecognitionModel, IntPtr vocabulary);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextRecognitionModelGetVocabulary(IntPtr textRecognitionModel, IntPtr vocabulary);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextRecognitionModelRecognize1(IntPtr textRecognitionModel, IntPtr frame, IntPtr text);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextRecognitionModelRecognize2(
            IntPtr textRecognitionModel,
            IntPtr frame,
            IntPtr roiRects,
            IntPtr results);
    }
}
