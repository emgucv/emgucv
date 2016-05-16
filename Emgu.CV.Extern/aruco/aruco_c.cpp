//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "aruco_c.h"

cv::aruco::Dictionary const* cveArucoGetPredefinedDictionary(int name)
{
   return &cv::aruco::getPredefinedDictionary(static_cast<cv::aruco::PREDEFINED_DICTIONARY_NAME>(name));
}

void cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits)
{
   cv::aruco::drawMarker(*dictionary, id, sidePixels, *img, borderBits);
}

void cveArucoDrawAxis(cv::_InputOutputArray* image, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* rvec, cv::_InputArray* tvec, float length)
{
   cv::aruco::drawAxis(*image, *cameraMatrix, *distCoeffs, *rvec, *tvec, length);
}

void cveArucoDetectMarkers(
   cv::_InputArray* image, cv::aruco::Dictionary* dictionary, cv::_OutputArray* corners,
   cv::_OutputArray* ids, cv::aruco::DetectorParameters* parameters,
   cv::_OutputArray* rejectedImgPoints)
{
   cv::aruco::detectMarkers(*image, *dictionary, *corners, *ids, *parameters, rejectedImgPoints ? *rejectedImgPoints : (cv::OutputArrayOfArrays) cv::noArray());
}

void cveArucoEstimatePoseSingleMarkers(cv::_InputArray* corners, float markerLength,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs)
{
   cv::aruco::estimatePoseSingleMarkers(*corners, markerLength, *cameraMatrix, *distCoeffs, *rvecs, *tvecs);
}

cv::aruco::GridBoard* cveArucoGridBoardCreate(
   int markersX, int markersY, float markerLength, float markerSeparation,
   cv::aruco::Dictionary* dictionary, cv::aruco::Board** boardPtr)
{
   cv::aruco::GridBoard gridBoard = cv::aruco::GridBoard::create(markersX, markersY, markerLength, markerSeparation, *dictionary);
   cv::Ptr<cv::aruco::GridBoard> ptr = cv::makePtr<cv::aruco::GridBoard>(gridBoard);
   ptr.addref();
   *boardPtr = dynamic_cast<cv::aruco::Board*>(ptr.get());
   return ptr.get();
}

void cveArucoGridBoardRelease(cv::aruco::GridBoard** gridBoard)
{
   delete * gridBoard;
   *gridBoard = 0;
}


void cveArucoRefineDetectedMarkers(
   cv::_InputArray* image, cv::aruco::Board* board, cv::_InputOutputArray* detectedCorners,
   cv::_InputOutputArray* detectedIds, cv::_InputOutputArray* rejectedCorners,
   cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   float minRepDistance, float errorCorrectionRate, bool checkAllOrders,
   cv::_OutputArray* recoveredIdxs, cv::aruco::DetectorParameters* parameters)
{
   cv::aruco::refineDetectedMarkers(
      *image, *board, *detectedCorners, *detectedIds, *rejectedCorners,
      cameraMatrix ? *cameraMatrix : static_cast<cv::InputArray>(cv::noArray()),
      distCoeffs ? *distCoeffs : static_cast<cv::InputArray>(cv::noArray()),
      minRepDistance, errorCorrectionRate, checkAllOrders,
      recoveredIdxs ? *recoveredIdxs : static_cast<cv::OutputArray>(cv::noArray()),
      parameters ? *parameters : cv::aruco::DetectorParameters());
}

void cveArucoDrawDetectedMarkers(
   cv::_InputOutputArray* image, cv::_InputArray* corners,
   cv::_InputArray* ids, CvScalar borderColor)
{
   cv::aruco::drawDetectedMarkers(*image, *corners, ids ? *ids : static_cast<cv::InputArray>(cv::noArray()), borderColor);
}

double cveArucoCalibrateCameraAruco(
   cv::_InputArray* corners, cv::_InputArray* ids, cv::_InputArray* counter, cv::aruco::Board* board,
   CvSize* imageSize, cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs,
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags,
   CvTermCriteria* criteria)
{
   return cv::aruco::calibrateCameraAruco(*corners, *ids, *counter, *board, *imageSize,
      *cameraMatrix, *distCoeffs, rvecs ? *rvecs : (cv::OutputArrayOfArrays) cv::noArray(),
      tvecs ? *tvecs : (cv::OutputArrayOfArrays) cv::noArray(), flags, *criteria);
}

void cveArucoDetectorParametersGetDefault(cv::aruco::DetectorParameters* parameters)
{
   cv::aruco::DetectorParameters p;
   memcpy(parameters, &p, sizeof(cv::aruco::DetectorParameters));
}