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
    /// Interface for modalities that plug into the LINE template matching representation.
    /// </summary>
    public partial class Modality : SharedPtrObject
    {
        private bool _needDispose;

        internal Modality(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }

        /// <summary>
        /// Create modality by name.
        /// </summary>
        /// <param name="modalityType">The following modality types are supported: "ColorGradient", "DepthNormal"</param>
        public Modality(String modalityType)
		{
            using (CvString csModalityType = new CvString(modalityType))
                _ptr = LinemodInvoke.cveLinemodModalityCreate(csModalityType, ref _sharedPtr);
		}

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_needDispose && _sharedPtr != IntPtr.Zero)
            {
                LinemodInvoke.cveLinemodDetectorRelease(ref _sharedPtr);
            }
            _sharedPtr = IntPtr.Zero;
            _ptr = IntPtr.Zero;
        }
    }

    /// <summary>
    /// Provide interfaces to the Open CV Linemod functions
    /// </summary>
    public static partial class LinemodInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveLinemodModalityCreate(IntPtr modalityType, ref IntPtr sharedPtr);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveLinemodModalityRelease(ref IntPtr sharedPtr);


    }
}
