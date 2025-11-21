//----------------------------------------------------------------------------
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.       
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
using System.Runtime.InteropServices.ComTypes;

namespace Emgu.CV.Aruco
{
    public class CharucoDetector : UnmanagedObject, IAlgorithm
    {
        
        private IntPtr _algorithmPtr;

        /// <summary>
        /// Pointer to the graphical code detector
        /// </summary>
        public IntPtr AlgorithmPtr
        {
            get { return _algorithmPtr; }
        }

        public CharucoDetector(
            Dictionary dictionary,
            CharucoParameters charucoParameters,
            DetectorParameters detectorParams,
            RefineParameters refineParams)
        {
            _ptr = ObjdetectInvoke.cveCharucoDetectorCreate(
                dictionary,
                charucoParameters,
                ref detectorParams,
                ref refineParams,
                ref _algorithmPtr);
        }


        public void DetectDiamonds(
            IInputArray image,
            IOutputArray diamondCorners,
            IOutputArray diamondIds,
            IInputOutputArray markerCorners,
            IInputOutputArray markerIds)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputOutputArray ioaMarkerCorners = markerCorners.GetInputOutputArray())
            using (InputOutputArray ioaMarkerIds = markerIds.GetInputOutputArray())
            using (OutputArray oaDiamondCorners = diamondCorners.GetOutputArray())
            using (OutputArray oaDiamondIds = diamondIds.GetOutputArray())
            {
                ObjdetectInvoke.cveCharucoDetectorDetectDiamonds(
                    _ptr,
                    iaImage,   
                    oaDiamondCorners, 
                    oaDiamondIds,
                    ioaMarkerCorners, 
                    ioaMarkerIds);
            }
        }
        


        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                ObjdetectInvoke.cveCharucoDetectorRelease(ref _ptr);
            }
        }
    }


    /// <summary>
    /// This class contains functions to call into object detect module
    /// </summary>
    public static partial class ObjdetectInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveCharucoDetectorCreate(
            IntPtr board,
            IntPtr charucoParameters,
            ref DetectorParameters detectorParams,
            ref RefineParameters refineParams,
            ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCharucoDetectorRelease(ref IntPtr detector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveCharucoDetectorDetectDiamonds(
            IntPtr detector,
            IntPtr image,
            IntPtr diamondCorners,
            IntPtr diamondIds,
            IntPtr markerCorners,
            IntPtr markerIds);
    }

}