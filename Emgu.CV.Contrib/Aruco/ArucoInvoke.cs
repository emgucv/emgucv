//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using System.Diagnostics;
using Emgu.CV.CvEnum;

namespace Emgu.CV.Aruco
{

   public static partial class ArucoInvoke
   {
      static ArucoInvoke()
      {
         CvInvoke.CheckLibraryLoaded();
      }

      /// <summary>
      /// Draw a canonical marker image.
      /// </summary>
      /// <param name="dictionary">dictionary of markers indicating the type of markers</param>
      /// <param name="id">identifier of the marker that will be returned. It has to be a valid id in the specified dictionary.</param>
      /// <param name="sidePixels">size of the image in pixels</param>
      /// <param name="img">output image with the marker</param>
      /// <param name="borderBits">width of the marker border.</param>
      public static void DrawMarker(Dictionary dict, int id, int sidePixels, IOutputArray img, int borderBits = 1)
      {
         using (OutputArray oaImg = img.GetOutputArray())
            cveArucoDrawMarker(dict, id, sidePixels, oaImg, borderBits);
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDrawMarker(IntPtr dictionary, int id, int sidePixels, IntPtr img, int borderBits);


      /// <summary>
      /// Performs marker detection in the input image. Only markers included in the specific dictionary are searched. For each detected marker, it returns the 2D position of its corner in the image and its corresponding identifier. Note that this function does not perform pose estimation.
      /// </summary>
      /// <param name="image">input image</param>
      /// <param name="dict">indicates the type of markers that will be searched</param>
      /// <param name="corners">	vector of detected marker corners. For each marker, its four corners are provided, (e.g VectorOfVectorOfPointF ). For N detected markers, the dimensions of this array is Nx4. The order of the corners is clockwise.</param>
      /// <param name="ids">vector of identifiers of the detected markers. The identifier is of type int (e.g. VectorOfInt). For N detected markers, the size of ids is also N. The identifiers have the same order than the markers in the imgPoints array.</param>
      /// <param name="parameters">marker detection parameters</param>
      /// <param name="rejectedImgPoints">contains the imgPoints of those squares whose inner code has not a correct codification. Useful for debugging purposes.</param>
      public static void DetectMarkers(
         IInputArray image, Dictionary dict, IOutputArrayOfArrays corners,
         IOutputArray ids, DetectorParameters parameters,
         IOutputArrayOfArrays rejectedImgPoints = null
         )
      {
         using (InputArray iaImage = image.GetInputArray())
         using (OutputArray oaCorners = corners.GetOutputArray())
         using (OutputArray oaIds = ids.GetOutputArray())
         using (OutputArray oaRejectedImgPoints = rejectedImgPoints != null ? rejectedImgPoints.GetOutputArray() : OutputArray.GetEmpty())
         {
            cveArucoDetectMarkers(iaImage, dict, oaCorners, oaIds, ref parameters, oaRejectedImgPoints);
         }
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDetectMarkers(IntPtr image, IntPtr dictionary, IntPtr corners,
         IntPtr ids, ref DetectorParameters parameters,
         IntPtr rejectedImgPoints);

      /// <summary>
      /// Given the pose estimation of a marker or board, this function draws the axis of the world coordinate system, i.e. the system centered on the marker/board. Useful for debugging purposes.
      /// </summary>
      /// <param name="image">input/output image. It must have 1 or 3 channels. The number of channels is not altered.</param>
      /// <param name="cameraMatrix">input 3x3 floating-point camera matrix</param>
      /// <param name="distCoeffs">vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
      /// <param name="rvec">rotation vector of the coordinate system that will be drawn.</param>
      /// <param name="tvec">translation vector of the coordinate system that will be drawn.</param>
      /// <param name="length">length of the painted axis in the same unit than tvec (usually in meters)</param>
      public static void DrawAxis(
         IInputOutputArray image, IInputArray cameraMatrix, IInputArray distCoeffs,
         IInputArray rvec, IInputArray tvec, float length)
      {
         using (InputOutputArray ioaImage = image.GetInputOutputArray())
         using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
         using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
         using (InputArray iaRvec = rvec.GetInputArray())
         using (InputArray iaTvec = tvec.GetInputArray())
         {
            cveArucoDrawAxis(ioaImage, iaCameraMatrix, iaDistCoeffs, iaRvec, iaTvec, length);
         }
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoDrawAxis(IntPtr image, IntPtr cameraMatrix, IntPtr distCoeffs, IntPtr rvec, IntPtr tvec, float length);

      /// <summary>
      /// This function receives the detected markers and returns their pose estimation respect to the camera individually. So for each marker, one rotation and translation vector is returned. The returned transformation is the one that transforms points from each marker coordinate system to the camera coordinate system. The marker corrdinate system is centered on the middle of the marker, with the Z axis perpendicular to the marker plane. The coordinates of the four corners of the marker in its own coordinate system are: (-markerLength/2, markerLength/2, 0), (markerLength/2, markerLength/2, 0), (markerLength/2, -markerLength/2, 0), (-markerLength/2, -markerLength/2, 0)
      /// </summary>
      /// <param name="corners">vector of already detected markers corners. For each marker, its four corners are provided, (e.g VectorOfVectorOfPointF ). For N detected markers, the dimensions of this array should be Nx4. The order of the corners should be clockwise.</param>
      /// <param name="markerLength">the length of the markers' side. The returning translation vectors will be in the same unit. Normally, unit is meters.</param>
      /// <param name="cameraMatrix">input 3x3 floating-point camera matrix</param>
      /// <param name="distCoeffs">vector of distortion coefficients (k1,k2,p1,p2[,k3[,k4,k5,k6],[s1,s2,s3,s4]]) of 4, 5, 8 or 12 elements</param>
      /// <param name="rvecs">array of output rotation vectors. Each element in rvecs corresponds to the specific marker in imgPoints.</param>
      /// <param name="tvecs">array of output translation vectors (e.g. VectorOfPoint3D32F ). Each element in tvecs corresponds to the specific marker in imgPoints.</param>
      public static void EstimatePoseSingleMarkers(IInputArrayOfArrays corners, float markerLength,
         IInputArray cameraMatrix, IInputArray distCoeffs,
         IOutputArrayOfArrays rvecs, IOutputArrayOfArrays tvecs)
      {
         using (InputArray iaCorners = corners.GetInputArray())
         using (InputArray iaCameraMatrix = cameraMatrix.GetInputArray())
         using (InputArray iaDistCoeffs = distCoeffs.GetInputArray())
         using (OutputArray oaRvecs = rvecs.GetOutputArray())
         using (OutputArray oaTvecs = tvecs.GetOutputArray())
         {
            cveArucoEstimatePoseSingleMarkers(iaCorners, markerLength, iaCameraMatrix, iaDistCoeffs, oaRvecs, oaTvecs);
         }
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoEstimatePoseSingleMarkers(IntPtr corners, float markerLength,
         IntPtr cameraMatrix, IntPtr distCoeffs,
         IntPtr rvecs, IntPtr tvecs);

      

      public static void RefineDetectedMarkers(
         IInputArray image, IBoard board, IInputOutputArray detectedCorners,
         IInputOutputArray detectedIds, IInputOutputArray rejectedCorners,
         IInputArray cameraMatrix, IInputArray distCoeffs,
         float minRepDistance, float errorCorrectionRate,
         bool checkAllOrders,
         IOutputArray recoveredIdxs, DetectorParameters parameters)
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
            cveArucoRefineDetectedMarkers(iaImage, board.BoardPtr, ioaDetectedCorners, ioaDetectedIds, ioaRejectedCorners,
               iaCameraMatrix, iaDistCoeffs, minRepDistance, errorCorrectionRate, checkAllOrders, oaRecovervedIdx, ref parameters);
         }
      }
      [DllImport(CvInvoke.ExternLibrary, CallingConvention = CvInvoke.CvCallingConvention)]
      internal static extern void cveArucoRefineDetectedMarkers(
         IntPtr image, IntPtr board, IntPtr detectedCorners,
         IntPtr detectedIds, IntPtr rejectedCorners,
         IntPtr cameraMatrix, IntPtr distCoeffs,
         float minRepDistance, float errorCorrectionRate,
         [MarshalAs(CvInvoke.BoolMarshalType)]
         bool checkAllOrders,
         IntPtr ecoveredIdxs, ref DetectorParameters parameters);
   }
}