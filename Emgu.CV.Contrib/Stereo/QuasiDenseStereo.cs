//----------------------------------------------------------------------------
//  Copyright (C) 2004-2019 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.Util;
using System.Drawing;

namespace Emgu.CV.Stereo
{
    public partial class QuasiDenseStereo : SharedPtrObject
    {
        public QuasiDenseStereo(Size monoImgSize, String paramFilepath = "")
        {
            using (CvString csParamFilePath = new CvString(paramFilepath))
                _ptr = StereoInvoke.cveQuasiDenseStereoCreate(
                    ref monoImgSize,
                    csParamFilePath,
                    ref _sharedPtr);
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_sharedPtr == IntPtr.Zero)
            {
                StereoInvoke.cveQuasiDenseStereoRelease(ref _sharedPtr);
                _ptr = IntPtr.Zero;
            }
        }

        public void Process(Mat imgLeft, Mat imgRight)
        {
            StereoInvoke.cveQuasiDenseStereoProcess(_ptr, imgLeft, imgRight);
        }

        public Mat GetDisparity(byte disparityLvls = (byte)50)
        {
            Mat disparity = new Mat();
            StereoInvoke.cveQuasiDenseStereoGetDisparity(_ptr, disparityLvls, disparity);
            return disparity;
        }

        public struct PropagationParameters
        {
            /// <summary>
            /// similarity window
            /// </summary>
            int CorrWinSizeX;
            /// <summary>
            /// similarity window
            /// </summary>
            int CorrWinSizeY;

            /// <summary>
            /// border to ignore
            /// </summary>
            int BorderX;
            /// <summary>
            /// border to ignore
            /// </summary>
            int BorderY;

            /// <summary>
            /// correlation threshold
            /// </summary>
            float CorrelationThreshold;
            /// <summary>
            /// texture threshold
            /// </summary>
            float TextrureThreshold;

            /// <summary>
            /// neighborhood size
            /// </summary>
            int NeighborhoodSize;
            /// <summary>
            /// disparity gradient threshold
            /// </summary>
            int DisparityGradient;

            /// <summary>
            /// Parameters for LK flow algorithm 
            /// </summary>
            int LkTemplateSize;
            /// <summary>
            /// Parameters for LK flow algorithm
            /// </summary>
            int LkPyrLvl;
            /// <summary>
            /// Parameters for LK flow algorithm
            /// </summary>
            int LkTermParam1;
            /// <summary>
            /// Parameters for LK flow algorithm
            /// </summary>
            float LkTermParam2;

            /// <summary>
            /// Parameters for GFT algorithm.
            /// </summary>
            float GftQualityThres;
            /// <summary>
            /// Parameters for GFT algorithm.
            /// </summary>
            int GftMinSeperationDist;
            /// <summary>
            /// Parameters for GFT algorithm.
            /// </summary>
            int GftMaxNumFeatures;
        }
    }


    /// <summary>
    /// Class that contains entry points for the Stereo module.
    /// </summary>
    public static partial class StereoInvoke
    {
        static StereoInvoke()
        {
            CvInvoke.CheckLibraryLoaded();
        }

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static IntPtr cveQuasiDenseStereoCreate(
            ref Size monoImgSize,
            IntPtr paramFilepath,
            ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQuasiDenseStereoRelease(ref IntPtr sharedPtr);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQuasiDenseStereoProcess(IntPtr stereo, IntPtr imgLeft, IntPtr imgRight);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal extern static void cveQuasiDenseStereoGetDisparity(IntPtr stereo, byte disparityLvls, IntPtr disparity);
    }

}
