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
    /// Image hash based on block mean.
    /// </summary>
    public class BlockMeanHash : ImgHashBase
    {
        private IntPtr _sharedPtr;

        /// <summary>
        /// Block Mean Hash mode
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// use fewer block and generate 16*16/8 uchar hash value
            /// </summary>
            HashMode0 = 0,
            /// <summary>
            /// use block blocks(step sizes/2), generate 31*31/8 + 1 uchar hash value
            /// </summary>
            HashMode1 = 1,
        }

        /// <summary>
        /// Create a Block Mean Hash object
        /// </summary>
        /// <param name="mode">The hash mode</param>
        public BlockMeanHash(Mode mode = Mode.HashMode0)
        {
            _ptr = ImgHashInvoke.cveBlockMeanHashCreate(ref _imgHashBase, mode, ref _sharedPtr);
        }

        /// <summary>
        /// Release all the unmanaged resource associated with BlockMeanHash
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
                ImgHashInvoke.cveBlockMeanHashRelease(ref _ptr, ref _sharedPtr);
            base.DisposeObject();
        }
    }

    internal static partial class ImgHashInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveBlockMeanHashCreate(ref IntPtr imgHash, BlockMeanHash.Mode mode, ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveBlockMeanHashRelease(ref IntPtr hash, ref IntPtr sharedPtr);
    }
}

