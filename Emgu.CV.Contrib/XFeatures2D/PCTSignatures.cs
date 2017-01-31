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

    public partial class PCTSignatures : UnmanagedObject
    {
        public PCTSignatures(int initSampleCount, int initSeedCount, int pointDistribution)
        {
            _ptr = XFeatures2DInvoke.cvePCTSignaturesCreate(initSampleCount, initSeedCount, pointDistribution);
        }

        public PCTSignatures(VectorOfPointF initSamplingPoints, int initSeedCount)
        {
            _ptr = XFeatures2DInvoke.cvePCTSignaturesCreate2(initSamplingPoints, initSeedCount);
        }

        public PCTSignatures(VectorOfPointF initSamplingPoints, VectorOfInt initClusterSeedIndexes)
        {
            _ptr = XFeatures2DInvoke.cvePCTSignaturesCreate3(initSamplingPoints, initClusterSeedIndexes);
        }

        protected override void DisposeObject()
        {
            XFeatures2DInvoke.cvePCTSignaturesRelease(ref _ptr);
        }

        public void ComputeSignature(IInputArray image, IOutputArray signature)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaSignature = signature.GetOutputArray())
            {
                XFeatures2DInvoke.cvePCTSignaturesComputeSignature(_ptr, iaImage, oaSignature);
            }
        }

        public static void DrawSignature(
            IInputArray source,
            IInputArray signature,
            IOutputArray result,
            float radiusToShorterSideRatio,
            int borderThickness)
        {
            using (InputArray iaSource = source.GetInputArray())
            using (InputArray iaSigniture = signature.GetInputArray())
            using (OutputArray oaResult = result.GetOutputArray())
                XFeatures2DInvoke.cvePCTSignaturesDrawSignature(iaSource, iaSigniture, oaResult, radiusToShorterSideRatio, borderThickness);
        }
    }

    public static partial class XFeatures2DInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesCreate(int initSampleCount, int initSeedCount, int pointDistribution);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesCreate2(IntPtr initSamplingPoints, int initSeedCount);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePCTSignaturesCreate3(IntPtr initSamplingPoints, IntPtr initClusterSeedIndexes);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesRelease(ref IntPtr pct);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesComputeSignature(IntPtr pct, IntPtr image, IntPtr signature);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePCTSignaturesDrawSignature(IntPtr source, IntPtr signature, IntPtr result, float radiusToShorterSideRatio, int borderThickness);

    }
}
