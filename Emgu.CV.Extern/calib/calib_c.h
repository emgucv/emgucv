//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2025 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CALIB_C_H
#define EMGU_CALIB_C_H

#include "opencv2/core.hpp"
#include "cvapi_compat.h"

#ifdef HAVE_OPENCV_CALIB
#include "opencv2/calib.hpp"
#include "opencv2/calib3d.hpp"

#else
static inline CV_NORETURN void throw_no_calib() { CV_Error(cv::Error::StsBadFunc, "The library is compiled without calib support. To use this module, please switch to the full Emgu CV runtime."); }

namespace cv {
	class Feature2D {};
}

#endif




//2D Tracker
CVAPI(bool) cveGetHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, std::vector< std::vector< cv::DMatch > >* matches, cv::Mat* mask, double randsacThreshold, cv::Mat* homography);

//Find circles grid
CVAPI(bool) cveFindCirclesGrid(cv::_InputArray* image, cv::Size* patternSize, cv::_OutputArray* centers, int flags, cv::Feature2D* blobDetector);

CVAPI(bool) cveFindChessboardCornersSB(cv::_InputArray* image, cv::Size* patternSize, cv::_OutputArray* corners, int flags);

CVAPI(void) cveEstimateChessboardSharpness(
	cv::_InputArray* image, 
	cv::Size* patternSize, 
	cv::_InputArray* corners,
	float riseDistance, 
	bool vertical,
	cv::_OutputArray* sharpness, 
	cv::Scalar* result);

CVAPI(void) cveDrawChessboardCorners(cv::_InputOutputArray* image, cv::Size* patternSize, cv::_InputArray* corners, bool patternWasFound);

CVAPI(void) cveFilterSpeckles(cv::_InputOutputArray* img, double newVal, int maxSpeckleSize, double maxDiff, cv::_InputOutputArray* buf);

CVAPI(bool) cveFindChessboardCorners(cv::_InputArray* image, cv::Size* patternSize, cv::_OutputArray* corners, int flags);

CVAPI(bool) cveFind4QuadCornerSubpix(cv::_InputArray* image, cv::_InputOutputArray* corners, cv::Size* regionSize);


CVAPI(double) cveCalibrateCamera(
   cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::Size* imageSize, 
   cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs, 
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags, cv::TermCriteria* criteria);

CVAPI(void) cveReprojectImageTo3D(cv::_InputArray* disparity, cv::_OutputArray* threeDImage, cv::_InputArray* q, bool handleMissingValues, int ddepth);


CVAPI(void) cveCalibrationMatrixValues(
   cv::_InputArray* cameraMatrix, cv::Size* imageSize, double apertureWidth, double apertureHeight, 
   double* fovx, double* fovy, double* focalLength, cv::Point2d* principalPoint, double* aspectRatio);

CVAPI(double) cveStereoCalibrate1(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints1,
	cv::_InputArray* imagePoints2,
	cv::_InputOutputArray* cameraMatrix1,
	cv::_InputOutputArray* distCoeffs1,
	cv::_InputOutputArray* cameraMatrix2,
	cv::_InputOutputArray* distCoeffs2,
	cv::Size* imageSize,
	cv::_InputOutputArray* r,
	cv::_InputOutputArray* t,
	cv::_OutputArray* e,
	cv::_OutputArray* f,
	cv::_OutputArray* rvecs,
	cv::_OutputArray* tvecs,
	cv::_OutputArray* perViewErrors,
	int flags,
	cv::TermCriteria* criteria);

CVAPI(double) cveStereoCalibrate2(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints1,
	cv::_InputArray* imagePoints2,
	cv::_InputOutputArray* cameraMatrix1,
	cv::_InputOutputArray* distCoeffs1,
	cv::_InputOutputArray* cameraMatrix2,
	cv::_InputOutputArray* distCoeffs2,
	cv::Size* imageSize,
	cv::_OutputArray* r,
	cv::_OutputArray* t,
	cv::_OutputArray* e,
	cv::_OutputArray* f,
	int flags,
	cv::TermCriteria* criteria);

CVAPI(void) cveInitCameraMatrix2D(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::Size* imageSize, 
	double aspectRatio, 
	cv::Mat* cameraMatrix);

/* Fisheye calibration */
CVAPI(void) cveFisheyeProjectPoints(cv::_InputArray* objectPoints, cv::_OutputArray* imagePoints, cv::_InputArray* rvec, cv::_InputArray* tvec,
   cv::_InputArray* K, cv::_InputArray* D, double alpha, cv::_OutputArray* jacobian);

CVAPI(void) cveFisheyeDistortPoints(cv::_InputArray* undistored, cv::_OutputArray* distorted, cv::_InputArray* K, cv::_InputArray* D, double alpha);

CVAPI(void) cveFisheyeUndistortPoints(
	cv::_InputArray* distorted, 
	cv::_OutputArray* undistorted, 
	cv::_InputArray* K, 
	cv::_InputArray* D, 
	cv::_InputArray* R, 
	cv::_InputArray* P,
	cv::TermCriteria* criteria);

CVAPI(void) cveFisheyeInitUndistortRectifyMap(cv::_InputArray* K, cv::_InputArray* D, cv::_InputArray* R, cv::_InputArray* P, cv::Size* size, int m1Type, cv::_OutputArray* map1, cv::_OutputArray* map2);

CVAPI(void) cveFisheyeUndistortImage(cv::_InputArray* distorted, cv::_OutputArray* undistored, cv::_InputArray* K, cv::_InputArray* D, cv::_InputArray* Knew, cv::Size* newSize);

CVAPI(void) cveFisheyeEstimateNewCameraMatrixForUndistortRectify(cv::_InputArray* K, cv::_InputArray* D, cv::Size* imageSize, cv::_InputArray* R, cv::_OutputArray* P, double balance, cv::Size* newSize, double fovScale);

CVAPI(void) cveFisheyeStereoRectify(cv::_InputArray* K1, cv::_InputArray*D1, cv::_InputArray* K2, cv::_InputArray* D2, cv::Size* imageSize,
   cv::_InputArray* R, cv::_InputArray* tvec, cv::_OutputArray* R1, cv::_OutputArray* R2, cv::_OutputArray* P1, cv::_OutputArray* P2, cv::_OutputArray* Q, int flags,
   cv::Size* newImageSize, double balance, double fovScale);

CVAPI(double) cveFisheyeCalibrate(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::Size* imageSize,
   cv::_InputOutputArray* K, cv::_InputOutputArray* D, cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags,
   cv::TermCriteria* criteria);

CVAPI(double) cveFisheyeStereoCalibrate(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints1,
   cv::_InputArray* imagePoints2, cv::_InputOutputArray* K1, cv::_InputOutputArray* D1, cv::_InputOutputArray* K2, cv::_InputOutputArray* D2,
   cv::Size* imageSize, cv::_OutputArray* R, cv::_OutputArray* T, int flags, cv::TermCriteria* criteria);

CVAPI(bool) cveFisheyeSolvePnP(
	cv::_InputArray* objectPoints, 
	cv::_InputArray* imagePoints, 
	cv::_InputArray* cameraMatrix, 
	cv::_InputArray* distCoeffs, 
	cv::_OutputArray* rvec, 
	cv::_OutputArray* tvec, 
	bool useExtrinsicGuess, 
	int flags, 
	cv::TermCriteria* criteria);

CVAPI(bool) cveFisheyeSolvePnPRansac(
	cv::_InputArray* opoints,
	cv::_InputArray* ipoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvec,
	cv::_OutputArray* tvec,
	bool useExtrinsicGuess,
	int iterationsCount,
	float reprojectionError,
	double confidence,
	cv::_OutputArray* inliers,
	int flags,
	cv::TermCriteria* criteria);

CVAPI(void) cveCalibrateHandEye(cv::_InputArray* R_gripper2base, cv::_InputArray* t_gripper2base,
	cv::_InputArray* R_target2cam, cv::_InputArray* t_target2cam,
	cv::_OutputArray* R_cam2gripper, cv::_OutputArray* t_cam2gripper,
	int method);


#endif