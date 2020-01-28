//----------------------------------------------------------------------------
//  Copyright (C) 2004-2020 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;

namespace Emgu.CV
{
    /// <summary>
    /// Dense Optical flow
    /// </summary>
    public interface IDenseOpticalFlow : IAlgorithm
    {
        /// <summary>
        /// Gets the dense optical flow pointer.
        /// </summary>
        /// <value>
        /// The dense optical flow .
        /// </value>
        IntPtr DenseOpticalFlowPtr { get; }
    }

    /// <summary>
    /// Extension methods for IDenseOpticalFlow
    /// </summary>
    public static class DenseOpticalFlowExtensions
    {
        /// <summary>
        /// Calculates an optical flow.
        /// </summary>
        /// <param name="i0">First 8-bit single-channel input image.</param>
        /// <param name="i1">Second input image of the same size and the same type as prev.</param>
        /// <param name="flow">Computed flow image that has the same size as prev and type CV_32FC2 </param>
        /// <param name="opticalFlow">The dense optical flow object</param>
        public static void Calc(this IDenseOpticalFlow opticalFlow, IInputArray i0, IInputArray i1, IInputOutputArray flow)
        {
            using (InputArray iaI0 = i0.GetInputArray())
            using (InputArray iaI1 = i1.GetInputArray())
            using (InputOutputArray ioaFlow = flow.GetInputOutputArray())
                CvInvoke.cveDenseOpticalFlowCalc(opticalFlow.DenseOpticalFlowPtr, iaI0, iaI1, ioaFlow);
        }
    }

    public static partial class CvInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDenseOpticalFlowCalc(IntPtr dof, IntPtr i0, IntPtr i1, IntPtr flow);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveDenseOpticalFlowRelease(ref IntPtr sharedPtr);
    }
}
