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
    /// This class represents high-level API for segmentation models.
    /// </summary>
    public partial class SegmentationModel : Model
    {
        /// <summary>
        /// Create a new segmentation model
        /// </summary>
        /// <param name="model">Binary file contains trained weights.</param>
        /// <param name="config">Text file contains network configuration.</param>
        public SegmentationModel(String model, String config = null)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveDnnSegmentationModelCreate1(
                    csModel,
                    csConfig,
                    ref _model);
            }
        }

        /// <summary>
        /// Create model from deep learning network.
        /// </summary>
        /// <param name="net">DNN Network</param>
        public SegmentationModel(Net net)
        {

            _ptr = DnnInvoke.cveDnnSegmentationModelCreate2(
                net,
                ref _model);

        }

        /// <summary>
        /// Given the input frame, create input blob, run net
        /// </summary>
        /// <param name="frame">The input image.</param>
        /// <param name="mask">Allocated class prediction for each pixel</param>
        public void Segment(IInputArray frame, IOutputArray mask)
        {
            using(InputArray iaFrame = frame.GetInputArray())
            using (OutputArray oaMask = mask.GetOutputArray())
            {
                DnnInvoke.cveDnnSegmentationModelSegment(_ptr, iaFrame, oaMask);
            }
        }

        /// <summary>
        /// Release the memory associated with this segmentation model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveDnnSegmentationModelRelease(ref _ptr);
            }
            _model = IntPtr.Zero;
        }
    }

    public static partial class DnnInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnSegmentationModelCreate1(IntPtr model, IntPtr config, ref IntPtr baseModel);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveDnnSegmentationModelCreate2(IntPtr network, ref IntPtr baseModel);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSegmentationModelRelease(ref IntPtr model);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDnnSegmentationModelSegment(
            IntPtr segmentationModel,
            IntPtr frame,
            IntPtr mask);

    }
}
