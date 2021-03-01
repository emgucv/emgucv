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

    public partial class TextDetectionModel_DB : TextDetectionModel
    {
        public TextDetectionModel_DB(String model, String config = null)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveDnnTextDetectionModelDbCreate1(
                    csModel,
                    csConfig,
                    ref _textDetectionModel,
                    ref _model);
            }
        }

        public TextDetectionModel_DB(Net net)
        {

            _ptr = DnnInvoke.cveDnnTextDetectionModelDbCreate2(
                net,
                ref _textDetectionModel,
                ref _model);

        }

        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnTextDetectionModelDbRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    public static partial class DnnInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnTextDetectionModelDbCreate1(IntPtr model, IntPtr config, ref IntPtr textDetectionModel, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnTextDetectionModelDbCreate2(IntPtr network, ref IntPtr textDetectionModel, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnTextDetectionModelDbRelease(ref IntPtr textDetectionModel);
    }
}
