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


namespace Emgu.CV
{
    /// <summary>
    /// Base class for computation of odometry.
    /// </summary>
    public class Odometry : UnmanagedObject
    {
        /// <summary>
        /// Set a type of data which odometry will use
        /// </summary>
        public enum OdometryType
        {
            /// <summary>
            /// Only depth data
            /// </summary>
            Depth = 0,
            /// <summary>
            /// Only rgb image
            /// </summary>
            Rgb = 1,
            /// <summary>
            /// Only depth and rgb data simultaneously
            /// </summary>
            RgbDepth = 2
        };

        /// <summary>
        /// Create an Odometry instance.
        /// </summary>
        /// <param name="odometryType">One of the odometry type: "RgbdOdometry", "ICPOdometry", "RgbdICPOdometry" or "FastICPOdometry" </param>
        public Odometry(OdometryType odometryType)
        {
            _ptr = CvInvoke.cveOdometryCreate((int)odometryType);
        }

        public bool Compute(
            IInputArray srcFrame,
            IInputArray dstFrame,
            IOutputArray rt)
        {
            using (InputArray iaSrcFrame = srcFrame.GetInputArray())
            using (InputArray iaDstFrame = dstFrame.GetInputArray())
            using (OutputArray oaRt = rt.GetOutputArray())
                return CvInvoke.cveOdometryCompute1(
                    _ptr,
                    iaSrcFrame,
                    iaDstFrame,
                    oaRt);
        }

        public bool Compute(
            IInputArray srcDepthFrame,
            IInputArray srcRGBFrame,
            IInputArray dstDepthFrame,
            IInputArray dstRGBFrame,
            IOutputArray rt)
        {
            using (InputArray iaSrcDepthFrame = srcDepthFrame.GetInputArray())
            using (InputArray iaSrcRGBFrame = srcRGBFrame.GetInputArray())
            using (InputArray iaDstDepthFrame = dstDepthFrame.GetInputArray())
            using (InputArray iaDstRGBFrame = dstRGBFrame.GetInputArray())
            using (OutputArray oaRt = rt.GetOutputArray())
                return CvInvoke.cveOdometryCompute2(
                    _ptr,
                    iaSrcDepthFrame,
                    iaSrcRGBFrame,
                    iaDstDepthFrame,
                    iaDstRGBFrame,
                    oaRt);
        }


        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                CvInvoke.cveOdometryRelease(ref _ptr);
            }
        }
    }

    /// <summary>
    /// Entry points for the cv::rgb functions
    /// </summary>
    public static partial class CvInvoke
    {

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOdometryCreate(int odometryType);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveOdometryCompute1(
            IntPtr odometry,
            IntPtr srcFrame,
            IntPtr dstFrame,
            IntPtr rt);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveOdometryCompute2(
            IntPtr odometry,
            IntPtr srcDepthFrame,
            IntPtr srcRGBFrame,
            IntPtr dstDepthFrame,
            IntPtr dstRGBFrame,
            IntPtr rt);
    }
}
