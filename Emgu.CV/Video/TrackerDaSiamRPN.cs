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
using Emgu.CV.Dnn;

namespace Emgu.CV
{
    /// <summary>
    /// TrackerDaSiamRPN
    /// </summary>
    public partial class TrackerDaSiamRPN : Tracker
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a new TrackerDaSiamRPN
        /// </summary>
        /// <param name="model">The model file</param>
        /// <param name="kernelCls1">The kernelCls1 file</param>
        /// <param name="kernelR1">The kernelR1 file</param>
        /// <param name="backend">The preferred DNN backend</param>
        /// <param name="target">The preferred DNN target</param>
        public TrackerDaSiamRPN(
            String model = "dasiamrpn_model.onnx",
            String kernelCls1 = "dasiamrpn_kernel_cls1.onnx",
            String kernelR1 = "dasiamrpn_kernel_r1.onnx",
            Dnn.Backend backend = Dnn.Backend.Default,
            Dnn.Target target = Target.Cpu)
        {
            using (CvString csModel = new CvString(model))
            using (CvString csKernelCls1 = new CvString(kernelCls1))
            using (CvString csKernelR1 = new CvString(kernelR1))
                _ptr = CvInvoke.cveTrackerDaSiamRPNCreate(
                    csModel,
                    csKernelCls1,
                    csKernelR1,
                    backend,
                    target,
                    ref _trackerPtr,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged resources associated with this tracker
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
                CvInvoke.cveTrackerDaSiamRPNRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveTrackerDaSiamRPNCreate(
            IntPtr model,
            IntPtr kernel_cls1,
            IntPtr kernel_r1,
            Dnn.Backend backend,
            Dnn.Target target,
            ref IntPtr tracker,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveTrackerDaSiamRPNRelease(ref IntPtr tracker, ref IntPtr sharedPtr);
    }
}