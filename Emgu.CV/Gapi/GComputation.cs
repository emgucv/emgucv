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
        public GComputation(GMat input, GMat output)
        {
            _ptr = GapiInvoke.cveGComputationCreate1(input, output);
        }

        public GComputation(GMat input, GScalar output)
        {
            _ptr = GapiInvoke.cveGComputationCreate2(input, output);
        }

        public GComputation(GMat input1, GMat input2, GMat output)
        {
            _ptr = GapiInvoke.cveGComputationCreate3(input1, input2, output);
        }

        public GComputation(GMat input1, GMat input2, GScalar output)
        {
            _ptr = GapiInvoke.cveGComputationCreate4(input1, input2, output);
        }

        public GComputation(VectorOfGMat inputs, VectorOfGMat outputs)
        {
            _ptr = GapiInvoke.cveGComputationCreate5(inputs, outputs);
        }


        public void Apply(Mat input, Mat output)
        {
            GapiInvoke.cveGComputationApply1(_ptr, input, output);
        }

        public MCvScalar ApplyS(Mat input)
        {
            MCvScalar result = new MCvScalar();
            GapiInvoke.cveGComputationApply2(_ptr, input, ref result);
            return result;
        }

        public void Apply(Mat input1, Mat input2, Mat output)
        {
            GapiInvoke.cveGComputationApply3(_ptr, input1, input2, output);
        }

        public MCvScalar ApplyS(Mat input1, Mat input2)
        {
            MCvScalar result = new MCvScalar();
            GapiInvoke.cveGComputationApply4(_ptr, input1, input2, ref result);
            return result;
        }

        public void Apply(VectorOfMat input, VectorOfMat output)
        {
            GapiInvoke.cveGComputationApply5(_ptr, input, output);
        }

        protected override void DisposeObject()
        {
            if (IntPtr.Zero == _ptr)
            {
                GapiInvoke.cveGComputationRelease(ref _ptr);
            }
        }
    }

    public static partial class GapiInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveGComputationCreate1(IntPtr input, IntPtr output);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveGComputationCreate2(IntPtr input, IntPtr output);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveGComputationCreate3(IntPtr input1, IntPtr input2, IntPtr output);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveGComputationCreate4(IntPtr input1, IntPtr input2, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveGComputationCreate5(IntPtr inputs, IntPtr outputs);
        

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGComputationRelease(ref IntPtr computation);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGComputationApply1(IntPtr computation, IntPtr input, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGComputationApply2(IntPtr computation, IntPtr input, ref MCvScalar output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGComputationApply3(IntPtr computation, IntPtr input1, IntPtr input2, IntPtr output);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGComputationApply4(IntPtr computation, IntPtr input1, IntPtr input2, ref MCvScalar output);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveGComputationApply5(IntPtr computation, IntPtr inputs, IntPtr outputs);

    }
}

