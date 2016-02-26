//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#include "aruco_c.h"

cv::aruco::Dictionary* cveArucoGetPredefinedDictionary(int name)
{
   cv::aruco::Dictionary dic = cv::aruco::getPredefinedDictionary(static_cast<cv::aruco::PREDEFINED_DICTIONARY_NAME>(name));
   return &dic;
}

void cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits)
{
   cv::aruco::drawMarker(*dictionary, id, sidePixels, *img, borderBits);
}

void cveArucoDrawAxis(cv::_InputOutputArray* image, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* rvec, cv::_InputArray* tvec, float length)
{
   cv::aruco::drawAxis(*image, *cameraMatrix, *distCoeffs, *rvec, *tvec, length);
}

void cveArucoDrawDetectedMarkers(cv::_InputOutputArray* image, cv::_InputArray* corners, cv::_InputArray* ids, CvScalar* borderColor)
{
   cv::aruco::drawDetectedMarkers(*image, *corners, *ids, *borderColor);
}

void cveArucoDetectMarkers(
   cv::_InputArray* image, cv::aruco::Dictionary* dictionary, cv::_OutputArray* corners,
   cv::_OutputArray* ids, cv::aruco::DetectorParameters* parameters,
   cv::_OutputArray* rejectedImgPoints)
{
   cv::aruco::detectMarkers(*image, *dictionary, *corners, *ids, *parameters, rejectedImgPoints ? *rejectedImgPoints : (cv::OutputArrayOfArrays) cv::noArray());
}
