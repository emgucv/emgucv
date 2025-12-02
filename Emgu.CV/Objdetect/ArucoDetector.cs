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
    /// <summary>
    /// Represents a detector for ArUco markers, providing functionality to detect and refine marker positions.
    /// </summary>
    /// <remarks>
    /// The <see cref="ArucoDetector"/> class is built upon the OpenCV ArUco module and allows for the detection
    /// of ArUco markers in images. It utilizes a dictionary of marker patterns, detection parameters, and optional
    /// refinement parameters to enhance detection accuracy.
    /// </remarks>
    public class ArucoDetector : UnmanagedObject, IAlgorithm
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
        /// Basic ArucoDetector constructor.
        /// </summary>
        /// <param name="dictionary">Indicates the type of markers that will be searched</param>
        /// <param name="detectorParams">Marker detection parameters</param>
        /// <param name="refineParams">Marker refine detection parameters</param>
        public ArucoDetector(
            Dictionary dictionary,
            DetectorParameters detectorParams,
            RefineParameters refineParams)
        {
            _ptr = ObjdetectInvoke.cveArucoDetectorCreate(
                dictionary,
                ref detectorParams,
                ref refineParams,
                ref _algorithmPtr);
        }

        /// <summary>
        /// Performs marker detection in the input image. Only markers included in the specific dictionary are searched. For each detected marker, it returns the 2D position of its corner in the image and its corresponding identifier. Note that this function does not perform pose estimation.
        /// </summary>
        /// <param name="image">input image</param>
        /// <param name="corners">Vector of detected marker corners. For each marker, its four corners are provided, (e.g VectorOfVectorOfPointF ). For N detected markers, the dimensions of this array is Nx4. The order of the corners is clockwise.</param>
        /// <param name="ids">vector of identifiers of the detected markers. The identifier is of type int (e.g. VectorOfInt). For N detected markers, the size of ids is also N. The identifiers have the same order than the markers in the imgPoints array.</param>
        /// <param name="rejectedImgPoints">contains the imgPoints of those squares whose inner code has not a correct codification. Useful for debugging purposes.</param>
        public void DetectMarkers(
           IInputArray image,
           IOutputArrayOfArrays corners,
           IOutputArray ids,
           IOutputArrayOfArrays rejectedImgPoints = null)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (OutputArray oaCorners = corners.GetOutputArray())
            using (OutputArray oaIds = ids.GetOutputArray())
            using (OutputArray oaRejectedImgPoints = rejectedImgPoints != null ? rejectedImgPoints.GetOutputArray() : OutputArray.GetEmpty())
            {
                ObjdetectInvoke.cveArucoDetectorDetectMarkers(
                    _ptr,
                    iaImage,
                    oaCorners,
                    oaIds,
                    oaRejectedImgPoints);
            }
        }

        /// <summary>
        /// Refine not detected markers based on the already detected and the board layout.
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="board">Layout of markers in the board.</param>
        /// <param name="detectedCorners">Vector of already detected marker corners.</param>
        /// <param name="detectedIds">Vector of already detected marker identifiers.</param>
        /// <param name="rejectedCorners">Vector of rejected candidates during the marker detection process</param>
        /// <param name="cameraMatrix">Optional input 3x3 floating-point camera matrix </param>
        /// <param name="distCoeffs">Optional vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
        /// <param name="recoveredIdxs">Optional array to returns the indexes of the recovered candidates in the original rejectedCorners array.</param>
        public void RefineDetectedMarkers(
           IInputArray image,
           IBoard board,
           IInputOutputArray detectedCorners,
           IInputOutputArray detectedIds,
           IInputOutputArray rejectedCorners,
           IInputArray cameraMatrix,
           IInputArray distCoeffs,
           IOutputArray recoveredIdxs)
        {
            using (InputArray iaImage = image.GetInputArray())
            using (InputOutputArray ioaDetectedCorners = detectedCorners.GetInputOutputArray())
            using (InputOutputArray ioaDetectedIds = detectedIds.GetInputOutputArray())
            using (InputOutputArray ioaRejectedCorners = rejectedCorners.GetInputOutputArray())
            using (InputArray iaCameraMatrix = cameraMatrix == null ? InputArray.GetEmpty() : cameraMatrix.GetInputArray())
            using (InputArray iaDistCoeffs = distCoeffs == null ? InputArray.GetEmpty() : distCoeffs.GetInputArray())
            using (
               OutputArray oaRecovervedIdx = recoveredIdxs == null
                  ? OutputArray.GetEmpty()
                  : recoveredIdxs.GetOutputArray())
            {
                ObjdetectInvoke.cveArucoDetectorRefineDetectedMarkers(
                    _ptr,
                    iaImage, 
                    board.BoardPtr, 
                    ioaDetectedCorners, 
                    ioaDetectedIds, 
                    ioaRejectedCorners,
                   iaCameraMatrix, 
                    iaDistCoeffs,
                    oaRecovervedIdx);
            }
        }

        /// <summary>
        /// Release the unmanaged memory associated with this object
        /// </summary>
        protected override void DisposeObject()
        {
            if (_ptr != IntPtr.Zero)
            {
                ObjdetectInvoke.cveArucoDetectorRelease(ref _ptr);
            }
        }
    }


    /// <summary>
    /// This class contains functions to call into object detect module
    /// </summary>
    public static partial class ObjdetectInvoke
    {
        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern IntPtr cveArucoDetectorCreate(
            IntPtr dictionary,
            ref DetectorParameters detectorParams,
            ref RefineParameters refineParams,
            ref IntPtr algorithm);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDetectorRelease(ref IntPtr detector);

        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDetectorDetectMarkers(
            IntPtr detector,
            IntPtr image,
            IntPtr corners,
            IntPtr ids,
            IntPtr rejectedImgPoints);


        [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
        internal static extern void cveArucoDetectorRefineDetectedMarkers(
            IntPtr detector,
            IntPtr image, 
            IntPtr board, 
            IntPtr detectedCorners,
            IntPtr detectedIds, 
            IntPtr rejectedCorners,
            IntPtr cameraMatrix, 
            IntPtr distCoeffs,
            IntPtr ecoveredIdxs);
    }

}