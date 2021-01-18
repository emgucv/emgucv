//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;

namespace Emgu.CV.Bioinspired
{
    /// <summary>
    /// Entry points to the Open CV bioinspired module
    /// </summary>
    public static partial class BioinspiredInvoke
    {
        static BioinspiredInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRetinaCreate(
            ref Size inputSize,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool colorMode,
            Retina.ColorSamplingMethod colorSamplingMethod,
            [MarshalAs(CvInvoke.BoolMarshalType)]
            bool useRetinaLogSampling,
            double reductionFactor,
            double samplingStrength, 
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaRun(IntPtr retina, IntPtr image);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaGetParvo(IntPtr retina, IntPtr parvo);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaGetMagno(IntPtr retina, IntPtr magno);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaClearBuffers(IntPtr retina);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaGetParameters(IntPtr retina, ref Retina.RetinaParameters parameters);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaSetParameters(IntPtr retina, ref Retina.RetinaParameters parameters);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveRetinaFastToneMappingCreate(ref Size inputSize, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaFastToneMappingApplyFastToneMapping(
            IntPtr toneMapping, 
            IntPtr inputImage, 
            IntPtr outputToneMappedImage);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaFastToneMappingSetup(
            IntPtr toneMapping, 
            float photoreceptorsNeighborhoodRadius, 
            float ganglioncellsNeighborhoodRadius, 
            float meanLuminanceModulatorK);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveRetinaFastToneMappingRelease(ref IntPtr sharedPtr);

    }
}
