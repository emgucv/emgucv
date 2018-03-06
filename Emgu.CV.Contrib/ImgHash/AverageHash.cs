//----------------------------------------------------------------------------
//  Copyright (C) 2004-2018 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.ImgHash
{
    /// <summary>
    /// Computes average hash value of the input image.
    /// </summary>
    /// <remarks>This is a fast image hashing algorithm, but only work on simple case.</remarks>
    public class AverageHash : ImgHashBase
    {
        /// <summary>
        /// Create an average hash object.
        /// </summary>
        public AverageHash()
        {
            _ptr = ImgHashInvoke.cveAverageHashCreate(ref _imgHashBase);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with AverageHash
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ImgHashInvoke.cveAverageHashRelease(ref _ptr);
            base.DisposeObject();
        }
    }

    internal static partial class ImgHashInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveAverageHashCreate(ref IntPtr imgHash);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveAverageHashRelease(ref IntPtr hash);
    }
}

