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
    /// This class represents high-level API for text recognition networks
    /// </summary>
    public partial class TextRecognitionModel : Model
    {
        /// <summary>
        /// Create Text Recognition model from deep learning network
        /// </summary>
        /// <param name="model">Binary file contains trained weights</param>
        /// <param name="config">Text file contains network configuration</param>
        /// <remarks>Set DecodeType and Vocabulary after constructor to initialize the decoding method.</remarks>
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

        /// <summary>
        /// Create Text Recognition model from deep learning network
        /// </summary>
        /// <param name="net">Dnn network</param>
        /// <remarks>Set DecodeType and Vocabulary after constructor to initialize the decoding method.</remarks>
        public TextRecognitionModel(Net net)
        {

            _ptr = DnnInvoke.cveDnnTextRecognitionModelCreate2(
                net,
                ref _model);

        }

        /// <summary>
        /// Get or Set the vocabulary for recognition.
        /// </summary>
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

        /// <summary>
        /// Set the decoding method options for "CTC-prefix-beam-search" decode usage
        /// </summary>
        /// <param name="beamSize">Beam size for search</param>
        /// <param name="vocPruneSize">Parameter to optimize big vocabulary search, only take top <paramref name="vocPruneSize"/> tokens in each search step, <paramref name="vocPruneSize"/> &lt;= 0 stands for disable this prune.</param>
        public void SetDecodeOptsCTCPrefixBeamSearch(int beamSize, int vocPruneSize)
        {
            DnnInvoke.cveDnnTextRecognitionModelSetDecodeOptsCTCPrefixBeamSearch(_ptr, beamSize, vocPruneSize);
        }

        /// <summary>
        /// Given the input frame, create input blob, run net and return recognition result.
        /// </summary>
        /// <param name="frame">The input image</param>
        /// <returns>The text recognition result</returns>
        public String Recognize(IInputArray frame)
        {
            using (CvString s = new CvString())
            using (InputArray iaFrame = frame.GetInputArray())
            {
                DnnInvoke.cveDnnTextRecognitionModelRecognize1(_ptr, iaFrame, s);
                return s.ToString();
            }
        }

        /// <summary>
        /// Given the input frame, create input blob, run net and return recognition result.
        /// </summary>
        /// <param name="frame">The input image</param>
        /// <param name="roiRects">Vector of text detection regions of interest (Rect, CV_32SC4). ROIs is be cropped as the network inputs</param>
        /// <returns>A set of text recognition results.</returns>
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

        /// <summary>
        /// Release the memory associated with this text recognition model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnTextRecognitionModelRelease(ref _ptr);
            }
            _model = IntPtr.Zero;
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
        internal static extern void cveDnnTextRecognitionModelSetDecodeOptsCTCPrefixBeamSearch(IntPtr textRecognitionModel, int beamSize, int vocPruneSize);

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
