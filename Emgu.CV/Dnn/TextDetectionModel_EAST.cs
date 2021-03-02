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
    /// This class represents high-level API for text detection DL networks compatible with EAST model.
    /// </summary>
    public partial class TextDetectionModel_EAST : TextDetectionModel
    {
        /// <summary>
        /// Create text detection model from network represented in one of the supported formats.
        /// </summary>
        /// <param name="model">Binary file contains trained weights.</param>
        /// <param name="config">Text file contains network configuration.</param>
        public TextDetectionModel_EAST(String model, String config = null)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveDnnTextDetectionModelEastCreate1(
                    csModel,
                    csConfig,
                    ref _textDetectionModel,
                    ref _model);
            }
        }

        /// <summary>
        /// Create text detection algorithm from deep learning network.
        /// </summary>
        /// <param name="net">Dnn network</param>
        public TextDetectionModel_EAST(Net net)
        {
            _ptr = DnnInvoke.cveDnnTextDetectionModelEastCreate2(
                    net,
                    ref _textDetectionModel,
                    ref _model);
        }

        /// <summary>
        /// Release the memory associated with this text detection model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnTextDetectionModelEastRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    public static partial class DnnInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnTextDetectionModelEastCreate1(IntPtr model, IntPtr config, ref IntPtr textDetectionModel, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnTextDetectionModelEastCreate2(IntPtr network, ref IntPtr textDetectionModel, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextDetectionModelEastRelease(ref IntPtr textDetectionModel);
    }
}
