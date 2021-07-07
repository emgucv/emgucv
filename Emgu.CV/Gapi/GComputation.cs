//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.IO;

namespace Emgu.CV
{
    /// <summary>
    /// The equivalent of cv::GComputation
    /// </summary>

    public partial class GComputation : UnmanagedObject
    {
        /// <summary>
        /// Defines an unary (one input – one output) computation.
        /// </summary>
        /// <param name="input">Input GMat of the defined unary computation</param>
        /// <param name="output">Output GMat of the defined unary computation</param>
        public GComputation(GMat input, GMat output)
        {
            _ptr = GapiInvoke.cveGComputationCreate1(input, output);
        }

        /// <summary>
        /// Defines an unary (one input – one output) computation.
        /// </summary>
        /// <param name="input">Input GMat of the defined unary computation</param>
        /// <param name="output">Output GScalar of the defined unary computation</param>
        public GComputation(GMat input, GScalar output)
        {
            _ptr = GapiInvoke.cveGComputationCreate2(input, output);
        }

        /// <summary>
        /// Defines a binary (two inputs – one output) computation.
        /// </summary>
        /// <param name="input1">First input GMat of the defined binary computation</param>
        /// <param name="input2">Second input GMat of the defined binary computation</param>
        /// <param name="output">Output GMat of the defined binary computation</param>
        public GComputation(GMat input1, GMat input2, GMat output)
        {
            _ptr = GapiInvoke.cveGComputationCreate3(input1, input2, output);
        }

        /// <summary>
        /// Defines a binary (two inputs – one output) computation.
        /// </summary>
        /// <param name="input1">First input GMat of the defined binary computation</param>
        /// <param name="input2">Second input GMat of the defined binary computation</param>
        /// <param name="output">Output GScalar of the defined binary computation</param>
        public GComputation(GMat input1, GMat input2, GScalar output)
        {
            _ptr = GapiInvoke.cveGComputationCreate4(input1, input2, output);
        }

        /// <summary>
        /// Defines a computation with arbitrary input/output number.
        /// </summary>
        /// <param name="inputs">Vector of inputs GMats for this computation</param>
        /// <param name="outputs">Vector of outputs GMats for this computation</param>
        public GComputation(VectorOfGMat inputs, VectorOfGMat outputs)
        {
            _ptr = GapiInvoke.cveGComputationCreate5(inputs, outputs);
        }

        /// <summary>
        /// Execute an unary computation (with compilation on the fly)
        /// </summary>
        /// <param name="input">Input Mat for unary computation</param>
        /// <param name="output">Output Mat for unary computation</param>
        public void Apply(Mat input, Mat output)
        {
            GapiInvoke.cveGComputationApply1(_ptr, input, output);
        }

        /// <summary>
        /// Execute an unary computation (with compilation on the fly)
        /// </summary>
        /// <param name="input">Input Mat for unary computation</param>
        /// <returns>Resulting scalar for unary computation</returns>
        public MCvScalar ApplyS(Mat input)
        {
            MCvScalar result = new MCvScalar();
            GapiInvoke.cveGComputationApply2(_ptr, input, ref result);
            return result;
        }

        /// <summary>
        /// Execute a binary computation (with compilation on the fly)
        /// </summary>
        /// <param name="input1">First input Mat for binary computation</param>
        /// <param name="input2">Second input Mat for binary computation</param>
        /// <param name="output">Output Mat for binary computation</param>
        public void Apply(Mat input1, Mat input2, Mat output)
        {
            GapiInvoke.cveGComputationApply3(_ptr, input1, input2, output);
        }

        /// <summary>
        /// Execute an binary computation (with compilation on the fly)
        /// </summary>
        /// <param name="input1">First input Mat for binary computation</param>
        /// <param name="input2">Second input Mat for binary computation</param>
        /// <returns>Output scalar for binary computation</returns>
        public MCvScalar ApplyS(Mat input1, Mat input2)
        {
            MCvScalar result = new MCvScalar();
            GapiInvoke.cveGComputationApply4(_ptr, input1, input2, ref result);
            return result;
        }

        /// <summary>
        /// Execute a computation with arbitrary number of inputs/outputs (with compilation on-the-fly).
        /// </summary>
        /// <param name="input">Vector of input Mat objects to process by the computation.</param>
        /// <param name="output">Vector of output Mat objects to produce by the computation.</param>
        public void Apply(VectorOfMat input, VectorOfMat output)
        {
            GapiInvoke.cveGComputationApply5(_ptr, input, output);
        }

        /// <summary>
        /// Release all the unmanaged memory associated with the GComputation
        /// </summary>
        protected override void DisposeObject()
        {
            if (IntPtr.Zero != _ptr)
            {
                GapiInvoke.cveGComputationRelease(ref _ptr);
            }
        }
    }

    public static partial class GapiInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGComputationCreate1(IntPtr input, IntPtr output);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGComputationCreate2(IntPtr input, IntPtr output);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGComputationCreate3(IntPtr input1, IntPtr input2, IntPtr output);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGComputationCreate4(IntPtr input1, IntPtr input2, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGComputationCreate5(IntPtr inputs, IntPtr outputs);
        

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGComputationRelease(ref IntPtr computation);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGComputationApply1(IntPtr computation, IntPtr input, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGComputationApply2(IntPtr computation, IntPtr input, ref MCvScalar output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGComputationApply3(IntPtr computation, IntPtr input1, IntPtr input2, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGComputationApply4(IntPtr computation, IntPtr input1, IntPtr input2, ref MCvScalar output);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGComputationApply5(IntPtr computation, IntPtr inputs, IntPtr outputs);

    }
}

