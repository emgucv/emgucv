//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Signal
{
    /// <summary>
    /// Signal processing functions.
    /// </summary>
    public static partial class SignalInvoke
    {
        static SignalInvoke()
        {
            CvInvoke.Init();
        }

        /// <summary>
        /// Resample a signal to a new sample rate.
        /// Uses cubic interpolation and a FIR filter based on a Kaiser window and Bessel function,
        /// producing results similar to scipy.signal.resample.
        /// </summary>
        /// <param name="inputSignal">Input signal array.</param>
        /// <param name="outSignal">Output resampled signal array.</param>
        /// <param name="inFreq">Input signal frequency (sample rate).</param>
        /// <param name="outFreq">Output signal frequency (sample rate).</param>
        public static void ResampleSignal(IInputArray inputSignal, IOutputArray outSignal, int inFreq, int outFreq)
        {
            using (InputArray iaInput = inputSignal.GetInputArray())
            using (OutputArray oaOutput = outSignal.GetOutputArray())
                cveResampleSignal(iaInput, oaOutput, inFreq, outFreq);
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveResampleSignal(IntPtr inputSignal, IntPtr outSignal, int inFreq, int outFreq);
    }
}
