//----------------------------------------------------------------------------
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.       
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
    /// <summary>
    /// Represents a detector for ChArUco boards, which combines ArUco markers and chessboard corners
    /// to improve the accuracy of pose estimation and marker detection.
    /// </summary>
    /// <remarks>
    /// This class provides functionality to detect ChArUco boards and diamonds in images.
    /// It utilizes parameters for marker detection, ChArUco-specific settings, and refinement options.
    /// </remarks>
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

        /// <summary>
        /// Basic CharucoDetector constructor.
        /// </summary>
        /// <param name="board">ChAruco board</param>
        /// <param name="charucoParameters">Charuco detection parameters</param>
        /// <param name="detectorParams">Marker detection parameters</param>
        /// <param name="refineParams">Marker refine detection parameters</param>
        public CharucoDetector(
            CharucoBoard board,
            CharucoParameters charucoParameters,
            DetectorParameters detectorParams,
            RefineParameters refineParams)
        {
            _ptr = ObjdetectInvoke.cveCharucoDetectorCreate(
                board,
                charucoParameters,
                ref detectorParams,
                ref refineParams,
                ref _algorithmPtr);
        }

        /// <summary>
        /// Detect ChArUco Diamond markers.
        /// </summary>
        /// <param name="image">Input image necessary for corner subpixel.</param>
        /// <param name="diamondCorners">Output list of detected diamond corners (4 corners per diamond). The order is the same than in marker corners: top left, top right, bottom right and bottom left. Similar format than the corners returned by detectMarkers </param>
        /// <param name="diamondIds">Ids of the diamonds in diamondCorners. The id of each diamond is in fact of type Vec4i, so each diamond has 4 ids, which are the ids of the aruco markers composing the diamond.</param>
        /// <param name="markerCorners">List of detected marker corners from detectMarkers function. If markerCorners and markerCorners are empty, the function detect aruco markers and ids.</param>
        /// <param name="markerIds">List of marker ids in markerCorners. If markerCorners and markerCorners are empty, the function detect aruco markers and ids.</param>
        public void DetectDiamonds(
            IInputArray image,
            IOutputArrayOfArrays diamondCorners,
            IOutputArray diamondIds,
            IInputOutputArrayOfArrays markerCorners,
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