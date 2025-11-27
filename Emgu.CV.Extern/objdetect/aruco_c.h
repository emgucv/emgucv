//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ARUCO_C_H
#define EMGU_ARUCO_C_H

#include "objdetect_c.h"

#ifdef HAVE_OPENCV_OBJDETECT
#include "opencv2/objdetect/aruco_detector.hpp"
#include "opencv2/objdetect/charuco_detector.hpp"
#else

namespace cv
{
	namespace aruco
	{
		class Dictionary {};
		class DetectorParameters {};
		class RefineParameters {};
		class Board {};
		class GridBoard	{};
		class ArucoDetector {};
		class CharucoBoard {};
		class CharucoParameters {};
		class CharucoDetector {};
	}
}

#endif

CVAPI(void) cveArucoDictionaryGenerateImageMarker(cv::aruco::Dictionary* dict, int id, int sizePixels, cv::_OutputArray* _img, int borderBits);
CVAPI(cv::aruco::Dictionary*) cveArucoGetPredefinedDictionary(int name, cv::Ptr<cv::aruco::Dictionary>** sharedPtr);
CVAPI(cv::aruco::Dictionary*) cveArucoDictionaryCreate(cv::Ptr<cv::aruco::Dictionary>** sharedPtr);
CVAPI(cv::aruco::Dictionary*) cveArucoExtendDictionary(
	int nMarkers,
	int markerSize,
	cv::Ptr<cv::aruco::Dictionary>* baseDictionary,
	int randomSeed,
	cv::Ptr<cv::aruco::Dictionary>** sharedPtr);
CVAPI(void) cveArucoDictionaryRelease(cv::aruco::Dictionary** dict, cv::Ptr<cv::aruco::Dictionary>** sharedPtr);

//CVAPI(void) cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits);

//CVAPI(void) cveArucoDrawAxis(cv::_InputOutputArray* image, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* rvec, cv::_InputArray* tvec, float length);

CVAPI(cv::aruco::ArucoDetector*) cveArucoDetectorCreate(
	cv::aruco::Dictionary* dictionary,
	cv::aruco::DetectorParameters* detectorParams,
	cv::aruco::RefineParameters* refineParams,
	cv::Algorithm** algorithm);
CVAPI(void) cveArucoDetectorRelease(cv::aruco::ArucoDetector** arucoDetector);

CVAPI(void) cveArucoDetectorDetectMarkers(
	cv::aruco::ArucoDetector* detector,
	cv::_InputArray* image,  
	cv::_OutputArray* corners,
	cv::_OutputArray* ids, 
	cv::_OutputArray* rejectedImgPoints);

CVAPI(void) cveArucoDetectorRefineDetectedMarkers(
	cv::aruco::ArucoDetector* detector,
	cv::_InputArray* image, 
	cv::aruco::Board* board, 
	cv::_InputOutputArray* detectedCorners,
	cv::_InputOutputArray* detectedIds, 
	cv::_InputOutputArray* rejectedCorners,
	cv::_InputArray* cameraMatrix, 
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* recoveredIdxs);

/*
CVAPI(void) cveArucoEstimatePoseSingleMarkers(cv::_InputArray* corners, float markerLength,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs);
*/

CVAPI(cv::aruco::GridBoard*) cveArucoGridBoardCreate(
	int markersX, int markersY, float markerLength, float markerSeparation,
	cv::aruco::Dictionary* dictionary, cv::_InputArray* ids, cv::aruco::Board** boardPtr, cv::Ptr<cv::aruco::GridBoard>** sharedPtr);
CVAPI(void) cveArucoGridBoardRelease(cv::Ptr<cv::aruco::GridBoard>** sharedPtr);

CVAPI(void) cveArucoBoardGenerateImage(
	cv::aruco::Board* gridBoard, 
	cv::Size* outSize, 
	cv::_OutputArray* img, 
	int marginSize, 
	int borderBits);
CVAPI(void) cveArucoBoardMatchImagePoints(
	cv::aruco::Board* gridBoard,
	cv::_InputArray* detectedCorners,
	cv::_InputArray* detectedIds,
	cv::_OutputArray* objPoints, 
	cv::_OutputArray* imgPoints);

CVAPI(cv::aruco::CharucoBoard*) cveCharucoBoardCreate(
   int squaresX, int squaresY, float squareLength, float markerLength,
   cv::aruco::Dictionary* dictionary, cv::aruco::Board** boardPtr, cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr);
//CVAPI(void) cveCharucoBoardDraw(cv::aruco::CharucoBoard* charucoBoard, CvSize* outSize, cv::_OutputArray* img, int marginSize, int borderBits);
CVAPI(void) cveCharucoBoardRelease(cv::aruco::CharucoBoard** charucoBoard, cv::Ptr<cv::aruco::CharucoBoard>** sharedPtr);


CVAPI(cv::aruco::CharucoParameters*) cveCharucoParametersCreate(
	int minMarkers,
	bool tryRefineMarkers,
	bool checkMarkers);
CVAPI(void) cveCharucoParametersRelease(cv::aruco::CharucoParameters** charucoParameters);

CVAPI(cv::aruco::CharucoDetector*) cveCharucoDetectorCreate(
	cv::aruco::CharucoBoard* board,
	cv::aruco::CharucoParameters* charucoParams,
	cv::aruco::DetectorParameters* detectorParams,
	cv::aruco::RefineParameters* refineParams,
	cv::Algorithm** algorithm);
CVAPI(void) cveCharucoDetectorRelease(cv::aruco::CharucoDetector** detector);
CVAPI(void) cveCharucoDetectorDetectDiamonds(
	cv::aruco::CharucoDetector* detector,
	cv::_InputArray* image,
	cv::_OutputArray* diamondCorners,
	cv::_OutputArray* diamondIds,
	cv::_InputOutputArray* markerCorners,
	cv::_InputOutputArray* markerIds);

CVAPI(void) cveCharucoDetectorDetectBoard(
	cv::aruco::CharucoDetector* detector,
	cv::_InputArray* image, 
	cv::_OutputArray* charucoCorners, 
	cv::_OutputArray* charucoIds,
	cv::_InputOutputArray* markerCorners,
	cv::_InputOutputArray* markerIds);


CVAPI(void) cveArucoDrawDetectedMarkers(
   cv::_InputOutputArray* image, cv::_InputArray* corners,
   cv::_InputArray* ids, cv::Scalar* borderColor);

/*
CVAPI(double) cveArucoCalibrateCameraAruco(
	cv::_InputArray* corners, cv::_InputArray* ids, cv::_InputArray* counter, cv::aruco::Board* board,
	cv::Size* imageSize, cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs,
	cv::_OutputArray* rvecs, cv::_OutputArray* tvecs,
	cv::_OutputArray* stdDeviationsIntrinsics,
	cv::_OutputArray* stdDeviationsExtrinsics,
	cv::_OutputArray* perViewErrors,
	int flags, 
	cv::TermCriteria* criteria);

CVAPI(double) cveArucoCalibrateCameraCharuco(
	cv::_InputArray* charucoCorners,
	cv::_InputArray* charucoIds,
	cv::aruco::CharucoBoard* board,
	cv::Size* imageSize,
	cv::_InputOutputArray* cameraMatrix,
	cv::_InputOutputArray* distCoeffs,
	cv::_OutputArray* rvecs,
	cv::_OutputArray* tvecs,
	cv::_OutputArray* stdDeviationsIntrinsics,
	cv::_OutputArray* stdDeviationsExtrinsics,
	cv::_OutputArray* perViewErrors,
	int flags,
	cv::TermCriteria* criteria);
*/

CVAPI(void) cveArucoDetectorParametersGetDefault(cv::aruco::DetectorParameters* parameters);

/*
CVAPI(int) cveArucoInterpolateCornersCharuco(
	cv::_InputArray* markerCorners, 
	cv::_InputArray* markerIds,
	cv::_InputArray* image, 
	cv::aruco::CharucoBoard* board,
	cv::_OutputArray* charucoCorners, 
	cv::_OutputArray* charucoIds,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs, 
	int minMarkers);
	*/

CVAPI(void) cveArucoDrawDetectedCornersCharuco(
	cv::_InputOutputArray* image, 
	cv::_InputArray* charucoCorners,
	cv::_InputArray* charucoIds,
	cv::Scalar* cornerColor);

/*
CVAPI(bool) cveArucoEstimatePoseCharucoBoard(
	cv::_InputArray* charucoCorners, 
	cv::_InputArray* charucoIds,
	cv::aruco::CharucoBoard* board, 
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs, 
	cv::_InputOutputArray* rvec, 
	cv::_InputOutputArray* tvec,
	bool useExtrinsicGuess);
	*/



CVAPI(void) cveArucoDrawDetectedDiamonds(
	cv::_InputOutputArray* image, 
	cv::_InputArray* diamondCorners,
	cv::_InputArray* diamondIds,
	cv::Scalar* borderColor);

/*
CVAPI(void) cveArucoDrawCharucoDiamond(
	cv::aruco::Dictionary* dictionary, 
	int* ids, 
	int squareLength,
	int markerLength, 
	cv::_OutputArray* img, 
	int marginSize,
	int borderBits);

CVAPI(void) cveArucoDrawPlanarBoard(
	cv::aruco::Board* board, 
	cv::Size* outSize, 
	cv::_OutputArray* img,
	int marginSize, 
	int borderBits);


CVAPI(int) cveArucoEstimatePoseBoard(
	cv::_InputArray* corners, 
	cv::_InputArray* ids, 
	cv::aruco::Board* board,
	cv::_InputArray* cameraMatrix, 
	cv::_InputArray* distCoeffs, 
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec, 
	bool useExtrinsicGuess);

CVAPI(void) cveArucoGetBoardObjectAndImagePoints(
	cv::aruco::Board* board, 
	cv::_InputArray* detectedCorners,
	cv::_InputArray* detectedIds, 
	cv::_OutputArray* objPoints, 
	cv::_OutputArray* imgPoints);
*/
#endif