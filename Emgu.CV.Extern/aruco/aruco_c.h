//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_ARUCO_C_H
#define EMGU_ARUCO_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/aruco.hpp"


CVAPI(cv::aruco::Dictionary*) cveArucoGetPredefinedDictionary(int name);

CVAPI(void) cveArucoDrawMarker(cv::aruco::Dictionary* dictionary, int id, int sidePixels, cv::_OutputArray* img, int borderBits);

CVAPI(void) cveArucoDrawAxis(cv::_InputOutputArray* image, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* rvec, cv::_InputArray* tvec, float length);

CVAPI(void) cveArucoDrawDetectedMarkers(cv::_InputOutputArray* image, cv::_InputArray* corners, cv::_InputArray* ids, CvScalar* borderColor);


CVAPI(void) detectMarkers(
   cv::_InputArray* image, cv::aruco::Dictionary* dictionary, cv::_OutputArray* corners,
   cv::_OutputArray* ids, cv::aruco::DetectorParameters* parameters,
   cv::_OutputArray* rejectedImgPoints);
#endif