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
    /// The equivalent of cv::GMat
    /// </summary>
    
    public partial class GMat : UnmanagedObject
    {
        internal bool _needDispose;
        internal GMat(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Create a new GMat
        /// </summary>
        public GMat()
            : this(GapiInvoke.cveGMatCreate(), true)
        {
        }

        /// <summary>
        /// Release all the unmanaged memory associated with this GMat
        /// </summary>
        protected override void DisposeObject()
        {
            if (_needDispose && (IntPtr.Zero != _ptr))
            {
                GapiInvoke.cveGMatRelease(ref _ptr);
            }
        }
    }

    public static partial class GapiInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveGMatCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveGMatRelease(ref IntPtr gmat);

    }
}

