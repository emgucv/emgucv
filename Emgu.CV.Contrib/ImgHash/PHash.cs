//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// Slower than average hash, but tolerant of minor modifications
    /// </summary>
    public class PHash : ImgHashBase
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Create a PHash object
        /// </summary>
        public PHash()
        {
            _ptr = ImgHashInvoke.cvePHashCreate(ref _imgHashBase, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with AverageHash
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ImgHashInvoke.cvePHashRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

    internal static partial class ImgHashInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cvePHashCreate(ref IntPtr imgHash, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cvePHashRelease(ref IntPtr hash, ref IntPtr sharedPtr);
    }
}

