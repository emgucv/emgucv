//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;

namespace Emgu.CV.Stitching
{
    /// <summary>
    /// Blender which uses multi-band blending algorithm
    /// </summary>
    public class MultiBandBlender : Blender
    {
        /// <summary>
        /// Create a multiBandBlender
        /// </summary>
        /// <param name="tryGpu">If true, will try to use GPU</param>
        /// <param name="numBands">Number of bands</param>
        /// <param name="weightType">The weight type</param>
        public MultiBandBlender(bool tryGpu = true, int numBands = 5, CvEnum.DepthType weightType = CvEnum.DepthType.Cv32F)
        {
            _ptr = StitchingInvoke.cveMultiBandBlenderCreate(tryGpu ? 1 : 0, numBands, weightType, ref _blenderPtr);
        }

        /// <summary>
        /// Release all unmanaged resources associated with this blender
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                StitchingInvoke.cveMultiBandBlenderRelease(ref _ptr);
            }
            base.DisposeObject();
        }
    }

    public static partial class StitchingInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveMultiBandBlenderCreate(int tryGpu, int numBands, CvEnum.DepthType weightType, ref IntPtr blender);
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveMultiBandBlenderRelease(ref IntPtr blender);
    }
}
