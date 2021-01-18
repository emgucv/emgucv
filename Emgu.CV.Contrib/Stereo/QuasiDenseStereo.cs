//----------------------------------------------------------------------------
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// Class containing the methods needed for Quasi Dense Stereo computation.
    /// </summary>
    public partial class QuasiDenseStereo : SharedPtrObject
    {
        /// <summary>
        /// Create a new instance containing the methods needed for Quasi Dense Stereo computation.
        /// </summary>
        /// <param name="monoImgSize">Image size</param>
        /// <param name="paramFilepath">The path for the parameters</param>
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

        /// <summary>
        /// Main process of the algorithm. This method computes the sparse seeds and then densifies them.
        /// Initially input images are converted to gray-scale and then the sparseMatching method is called to obtain the sparse stereo. Finally quasiDenseMatching is called to densify the corresponding points.
        /// </summary>
        /// <param name="imgLeft">The left Channel of a stereo image pair.</param>
        /// <param name="imgRight">The right Channel of a stereo image pair.</param>
        /// <remarks>If input images are in color, the method assumes that are BGR and converts them to grayscale.</remarks>
        public void Process(Mat imgLeft, Mat imgRight)
        {
            StereoInvoke.cveQuasiDenseStereoProcess(_ptr, imgLeft, imgRight);
        }

        /// <summary>
        /// Compute and return the disparity map based on the correspondences found in the "process" method.
        /// </summary>
        /// <param name="disparityLvls">The level of detail in output disparity image.</param>
        /// <returns>Mat containing a the disparity image in grayscale.</returns>
        public Mat GetDisparity(byte disparityLvls = (byte)50)
        {
            Mat disparity = new Mat();
            StereoInvoke.cveQuasiDenseStereoGetDisparity(_ptr, disparityLvls, disparity);
            return disparity;
        }

        /// <summary>
        /// The propagation parameters
        /// </summary>
        public struct PropagationParameters
        {
            /// <summary>
            /// similarity window
            /// </summary>
            public int CorrWinSizeX;

            /// <summary>
            /// similarity window
            /// </summary>
            public int CorrWinSizeY;

            /// <summary>
            /// border to ignore
            /// </summary>
            public int BorderX;

            /// <summary>
            /// border to ignore
            /// </summary>
            public int BorderY;

            /// <summary>
            /// correlation threshold
            /// </summary>
            public float CorrelationThreshold;

            /// <summary>
            /// texture threshold
            /// </summary>
            public float TextrureThreshold;

            /// <summary>
            /// neighborhood size
            /// </summary>
            public int NeighborhoodSize;

            /// <summary>
            /// disparity gradient threshold
            /// </summary>
            public int DisparityGradient;

            /// <summary>
            /// Parameters for LK flow algorithm 
            /// </summary>
            public int LkTemplateSize;

            /// <summary>
            /// Parameters for LK flow algorithm
            /// </summary>
            public int LkPyrLvl;

            /// <summary>
            /// Parameters for LK flow algorithm
            /// </summary>
            public int LkTermParam1;

            /// <summary>
            /// Parameters for LK flow algorithm
            /// </summary>
            public float LkTermParam2;

            /// <summary>
            /// Parameters for GFT algorithm.
            /// </summary>
            public float GftQualityThres;

            /// <summary>
            /// Parameters for GFT algorithm.
            /// </summary>
            public int GftMinSeperationDist;

            /// <summary>
            /// Parameters for GFT algorithm.
            /// </summary>
            public int GftMaxNumFeatures;
        }
    }

    /// <summary>
    /// Class that contains entry points for the Stereo module.
    /// </summary>
    public static partial class StereoInvoke
    {
        static StereoInvoke()
        {
            CvInvoke.Init();
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
        internal extern static void cveQuasiDenseStereoGetDisparity(IntPtr stereo, byte disparityLvls,
            IntPtr disparity);
    }
}
