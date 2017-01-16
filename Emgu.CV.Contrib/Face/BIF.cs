//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Face
{
    
    public class BIF : UnmanagedObject
    {
        public BIF(int numBands, int numRotations)
        {
            _ptr = ContribInvoke.cveBIFCreate(numBands, numRotations);
        }

        public void Compute(IInputArray image, IOutputArray features)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaFeatures = features.GetOutputArray())
                ContribInvoke.cveBIFCompute(_ptr, iaImage, oaFeatures);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this BIF
        /// </summary>
        protected override void DisposeObject()
        {
            ContribInvoke.cveBIFRelease(ref _ptr);
        }


    }
}

namespace Emgu.CV
{

    public static partial class ContribInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveBIFCreate(int numBands, int numRotations);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveBIFCompute(IntPtr bif, IntPtr image, IntPtr features);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveBIFRelease(ref IntPtr bif);
    }
}