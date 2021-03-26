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
using System.Drawing;

namespace Emgu.CV.Dnn
{
    /// <summary>
    /// This class is presented high-level API for neural networks.
    /// </summary>
    public class Model : UnmanagedObject
    {
        internal Model()
        {
        }

        /// <summary>
        /// Create DNN model from network represented in one of the supported formats.
        /// </summary>
        /// <param name="model">Binary file contains trained weights.</param>
        /// <param name="config">Text file contains network configuration.</param>
        public Model(String model, String config)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csConfig = new CvString(config))
            {
                _ptr = DnnInvoke.cveModelCreate(csModel, csConfig);
                _model = _ptr;
            }
        }

        /// <summary>
        /// Create model from deep learning network.
        /// </summary>
        /// <param name="network">DNN Network</param>
        public Model(Net network)
        {
            _ptr = DnnInvoke.cveModelCreateFromNet(network);
            _model = _ptr;
        }

        /// <summary>
        /// Given the input frame, create input blob, run net and return the output blobs.
        /// </summary>
        /// <param name="frame">The input image.</param>
        /// <param name="outs">Allocated output blobs, which will store results of the computation.</param>
        public void Predict(IInputArray frame, IOutputArrayOfArrays outs)
        {
            using (InputArray iaFrame = frame.GetInputArray())
            using (OutputArray oaOuts = outs.GetOutputArray())
            {
                DnnInvoke.cveModelPredict(_model, iaFrame, oaOuts);
            }
        }

        /// <summary>
        /// The pointer to the Model object
        /// </summary>
        protected IntPtr _model;

        /// <summary>
        /// Set scalefactor value for frame.
        /// </summary>
        /// <param name="scale">Multiplier for frame values.</param>
        public void SetInputScale(double scale)
        {
            DnnInvoke.cveModelSetInputScale(_model, scale);
        }

        /// <summary>
        /// Set mean value for frame.
        /// </summary>
        /// <param name="mean">Scalar with mean values which are subtracted from channels.</param>
        public void SetInputMean(MCvScalar mean)
        {
            DnnInvoke.cveModelSetInputMean(_model, ref mean);
        }

        /// <summary>
        /// Set input size for frame.
        /// </summary>
        /// <param name="size">New input size.</param>
        /// <remarks>If shape of the new blob less than 0, then frame size not change.</remarks>
        public void SetInputSize(Size size)
        {
            DnnInvoke.cveModelSetInputSize(_model, ref size);
        }

        /// <summary>
        /// Set flag crop for frame.
        /// </summary>
        /// <param name="crop">Flag which indicates whether image will be cropped after resize or not.</param>
        public void SetInputCrop(bool crop)
        {
            DnnInvoke.cveModelSetInputCrop(_model, crop);
        }

        /// <summary>
        /// Set flag swapRB for frame.
        /// </summary>
        /// <param name="swapRB">Flag which indicates that swap first and last channels.</param>
        public void SetInputSwapRB(bool swapRB)
        {
            DnnInvoke.cveModelSetInputSwapRB(_model, swapRB);
        }

        /// <summary>
        /// Release the memory associated with this Model.
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                DnnInvoke.cveModelRelease(ref _ptr);
            }
            _model = IntPtr.Zero;
        }

        /// <summary>
        /// Ask network to use specific computation backend where it supported.
        /// </summary>
        /// <param name="value">The value</param>
        public void SetPreferableBackend(Backend value)
        {
            DnnInvoke.cveModelSetPreferableBackend(_model, value);
        }

        /// <summary>
        /// Ask network to make computations on specific target device.
        /// </summary>
        /// <param name="value">The value</param>
        public void SetPreferableTarget(Target value)
        {
            DnnInvoke.cveModelSetPreferableTarget(_model, value);
        }
    }

    public static partial class DnnInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveModelCreate(IntPtr model, IntPtr config);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveModelCreateFromNet(IntPtr network);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelRelease(ref IntPtr model);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelPredict(IntPtr model, IntPtr frame, IntPtr outs);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetInputScale(
           IntPtr model,
           double scale);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetInputMean(
           IntPtr model,
           ref MCvScalar mean);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetInputSize(
            IntPtr model,
            ref Size size);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetInputCrop(
            IntPtr model,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool crop);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetInputSwapRB(
            IntPtr model,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool swapRB);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetPreferableBackend(IntPtr model, Backend backendId);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetPreferableTarget(IntPtr model, Target targetId);
    }
}
