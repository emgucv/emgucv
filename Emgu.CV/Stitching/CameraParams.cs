//----------------------------------------------------------------------------
//  Copyright (C) 2004-2024 by EMGU Corporation. All rights reserved.       
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
    /// Camera parameters used for stitching.
    /// </summary>
    public partial class CameraParams : UnmanagedObject
    {
        private readonly bool _needDispose;

        /// <summary>
        /// Create a default camera parameters
        /// </summary>
        public CameraParams()
            :this(StitchingInvoke.cveCameraParamsCreate(), true)
        {
        }

        internal CameraParams(IntPtr ptr, bool needDispose)
        {
            _ptr = ptr;
            _needDispose = needDispose;
        }


        /// <summary>
        /// Reset the unmanaged pointer associated to this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_needDispose && _ptr != IntPtr.Zero)
                StitchingInvoke.cveCameraParamsRelease(ref _ptr);

            _ptr = IntPtr.Zero;
        }

        /// <summary>
        /// Construct the camera calibration matrix using the camera parameters
        /// </summary>
        /// <returns>The camera calibration matrix</returns>
        public Mat K()
        {
            Mat m = new Mat();
            using (OutputArray ioM = m.GetOutputArray())
            {
                StitchingInvoke.cveCameraParamsGetK(_ptr, ioM);
                return m;
            }
        }
    }

    public static partial class StitchingInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCameraParamsCreate();

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCameraParamsRelease(ref IntPtr cameraParams);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCameraParamsGetK(IntPtr cameraParams, IntPtr k);
    }
}
