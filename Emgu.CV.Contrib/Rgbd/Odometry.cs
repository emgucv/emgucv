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


namespace Emgu.CV.Rgbd
{
    /// <summary>
    /// Base class for computation of odometry.
    /// </summary>
    public class Odometry : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Create an Odometry instance.
        /// </summary>
        /// <param name="odometryType">One of the odometry type: "RgbdOdometry", "ICPOdometry", "RgbdICPOdometry" or "FastICPOdometry" </param>
        public Odometry(String odometryType)
        {
            using (CvString csOdometryType = new CvString(odometryType))
                _ptr = RgbdInvoke.cveOdometryCreate(csOdometryType, ref _algorithmPtr, ref _sharedPtr);
        }

        /// <summary>
        /// Pointer to the native algorithm object
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        /// <summary>
        /// Method to compute a transformation from the source frame to the destination one. Some odometry algorithms do not used some data of frames (eg. ICP does not use images). In such case corresponding arguments can be set as empty Mat. The method returns true if all internal computations were possible (e.g. there were enough correspondences, system of equations has a solution, etc) and resulting transformation satisfies some test if it's provided by the Odometry inheritor implementation (e.g. thresholds for maximum translation and rotation).
        /// </summary>
        /// <param name="srcImage">Image data of the source frame (CV_8UC1)</param>
        /// <param name="srcDepth">Depth data of the source frame (CV_32FC1, in meters)</param>
        /// <param name="srcMask">Mask that sets which pixels have to be used from the source frame (CV_8UC1)</param>
        /// <param name="dstImage">Image data of the destination frame (CV_8UC1)</param>
        /// <param name="dstDepth">Depth data of the destination frame (CV_32FC1, in meters)</param>
        /// <param name="dstMask">Mask that sets which pixels have to be used from the destination frame (CV_8UC1)</param>
        /// <param name="rt">Resulting transformation from the source frame to the destination one (rigid body motion): dst_p = Rt * src_p, where dst_p is a homogeneous point in the destination frame and src_p is homogeneous point in the source frame, Rt is 4x4 matrix of CV_64FC1 type.</param>
        /// <param name="initRt">Initial transformation from the source frame to the destination one (optional)</param>
        /// <returns>True if all internal computations were possible</returns>
        public bool Compute(
            Mat srcImage,
            Mat srcDepth,
            Mat srcMask,
            Mat dstImage,
            Mat dstDepth,
            Mat dstMask,
            IOutputArray rt,
            Mat initRt = null)
        {
            using (OutputArray oaRt = rt.GetOutputArray())
                return RgbdInvoke.cveOdometryCompute(
                    _ptr,
                    srcImage,
                    srcDepth,
                    srcMask,
                    dstImage,
                    dstDepth,
                    dstMask,
                    oaRt,
                    initRt);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr != IntPtr.Zero)
            {
                RgbdInvoke.cveOdometryRelease(ref _sharedPtr);
                _algorithmPtr = IntPtr.Zero;
                _ptr = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// Entry points for the cv::rgb functions
    /// </summary>
    public static partial class RgbdInvoke
    {

        static RgbdInvoke()
        {
            CvInvoke.Init();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveOdometryCreate(
            IntPtr odometryType,
            ref IntPtr algorithm,
            ref IntPtr sharedPtr
        );

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveOdometryRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        [return: MarshalAs(CvInvoke.BoolMarshalType)]
        internal static extern bool cveOdometryCompute(
            IntPtr odometry, 
            IntPtr srcImage,
            IntPtr srcDepth,
            IntPtr srcMask,
            IntPtr dstImage,
            IntPtr dstDepth,
            IntPtr dstMask,
            IntPtr rt,
            IntPtr initRt);
    }
}
