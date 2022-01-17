//----------------------------------------------------------------------------
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;


namespace Emgu.CV.Linemod
{
    /// <summary>
    /// Represents a successful template match.
    /// </summary>
    public partial class Match : UnmanagedObject
    {
        private readonly bool _needDispose;

        internal Match(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Create an empty template match result.
        /// </summary>
        public Match()
        {
            _ptr = LinemodInvoke.cveLinemodMatchCreate();
            _needDispose = true;
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_needDispose && (_ptr != IntPtr.Zero))
            {
                LinemodInvoke.cveLinemodMatchRelease(ref _ptr);
            }
        }
    }
    
    public static partial class LinemodInvoke
    {
        static LinemodInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLinemodMatchCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLinemodMatchRelease(ref IntPtr ptr);


    }
}
