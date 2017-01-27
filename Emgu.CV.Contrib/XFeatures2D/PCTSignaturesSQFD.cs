//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.Features2D;

namespace Emgu.CV.XFeatures2D
{

    public partial class PCTSignaturesSQFD : UnmanagedObject
    {
        public PCTSignaturesSQFD(
            int distanceFunction,
            int similarityFunction,
            float similarityParameter)
        {
            _ptr = ContribInvoke.cvePCTSignaturesSQFDCreate(distanceFunction, similarityFunction, similarityParameter);
        }

        public float ComputeQuadraticFormDistance(IInputArray signature0, IInputArray signature1)
        {
            using (InputArray iaSignature0 = signature0.GetInputArray())
            using (InputArray iaSignature1 = signature1.GetInputArray())
                return ContribInvoke.cvePCTSignaturesSQFDComputeQuadraticFormDistance(_ptr, iaSignature0, iaSignature1);
        }

        public void ComputeQuadraticFormDistances(
            Mat sourceSignature,
            VectorOfMat imageSignatures,
            VectorOfFloat distances)
        {
            ContribInvoke.cvePCTSignaturesSQFDComputeQuadraticFormDistances(_ptr, sourceSignature, imageSignatures,
                distances);
        }

        protected override void DisposeObject()
        {
            ContribInvoke.cvePCTSignaturesRelease(ref _ptr);
        }
        
    }
}

namespace Emgu.CV
{
    public static partial class ContribInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesSQFDCreate(
            int distanceFunction,
            int similarityFunction,
            float similarityParameter);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static float cvePCTSignaturesSQFDComputeQuadraticFormDistance(
            IntPtr sqfd,
            IntPtr signature0,
            IntPtr signature1);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesSQFDComputeQuadraticFormDistances(
            IntPtr sqfd,
            IntPtr sourceSignature,
            IntPtr imageSignatures,
            IntPtr distances);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesSQFDRelease(ref IntPtr sqfd);
    }
}
