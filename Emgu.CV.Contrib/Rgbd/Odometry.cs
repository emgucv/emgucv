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

    public class Odometry : SharedPtrObject, IAlgorithm
    {
        private IntPtr _algorithmPtr;


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
