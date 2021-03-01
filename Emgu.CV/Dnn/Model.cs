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
    /// This class allows to create and manipulate comprehensive artificial neural networks.
    /// </summary>
    public abstract class Model : UnmanagedObject
    {

        /// <summary>
        /// The pointer to the Model object
        /// </summary>
        protected IntPtr _model;

        /// <summary>
        /// Input scale
        /// </summary>
        /// <param name="scale">The scale</param>
        public void SetInputScale(double scale)
        {
            DnnInvoke.cveModelSetInputScale(_model, scale);
        }

        /// <summary>
        /// Input mean
        /// </summary>
        /// <param name="mean">The mean</param>
        public void SetInputMean(MCvScalar mean)
        {
            DnnInvoke.cveModelSetInputMean(_model, ref mean);
        }

        public void SetInputSize(Size size)
        {
            DnnInvoke.cveModelSetInputSize(_model, ref size);
        }

        public void SetInputCrop(bool crop)
        {
            DnnInvoke.cveModelSetInputCrop(_model, crop);
        }

        public void SetInputSwapRB(bool swapRB)
        {
            DnnInvoke.cveModelSetInputSwapRB(_model, swapRB);
        }

        protected override void DisposeObject()
        {
            if (_model != IntPtr.Zero)
            {
                _model = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Ask network to use specific computation backend where it supported.
        /// </summary>
        /// <param name="value">The value</param>
        public void SetPreferableBackend(Net.Backend value)
        {
            DnnInvoke.cveModelSetPreferableBackend(_ptr, value);
        }

        /// <summary>
        /// Ask network to make computations on specific target device.
        /// </summary>
        /// <param name="value">The value</param>
        public void SetPreferableTarget(Net.Target value)
        {
            DnnInvoke.cveModelSetPreferableTarget(_ptr, value);
        }
    }

    public static partial class DnnInvoke
    {
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
        internal static extern void cveModelSetPreferableBackend(IntPtr model, Net.Backend backendId);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveModelSetPreferableTarget(IntPtr model, Net.Target targetId);
    }
}
