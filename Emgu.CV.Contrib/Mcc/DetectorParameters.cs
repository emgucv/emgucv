//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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

namespace Emgu.CV.Mcc
{
    /// <summary>
    /// Parameters for the detectMarker process
    /// </summary>
    public partial class DetectorParameters : UnmanagedObject
    {

        /// <summary>
        /// Parameters for the detectMarker process
        /// </summary>
        public DetectorParameters()
        {
            _ptr = MccInvoke.cveCCheckerDetectorParametersCreate();

        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                MccInvoke.cveCCheckerDetectorParametersRelease(ref _ptr);
            }
        }
    }


    /// <summary>
    /// Class that contains entry points for the Mcc module.
    /// </summary>
    public static partial class MccInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCCheckerDetectorParametersCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCCheckerDetectorParametersRelease(ref IntPtr sharedPtr);

    }
}
