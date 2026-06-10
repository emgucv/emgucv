//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2026 by EMGU Corporation. All rights reserved.
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
#include "opencv2/objdetect.hpp"

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

CVAPI(void) cveRodrigues(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* jacobian);

CVAPI(void) cveFindHomography(cv::_InputArray* srcPoints, cv::_InputArray* dstPoints, cv::_OutputArray* dst, int method, double ransacReprojThreshold, cv::_OutputArray* mask);

CVAPI(void) cveRQDecomp3x3(
	cv::_InputArray* src,
	cv::Point3d* out,
	cv::_OutputArray* mtxR,
	cv::_OutputArray* mtxQ,
	cv::_OutputArray* Qx,
	cv::_OutputArray* Qy,
	cv::_OutputArray* Qz);

CVAPI(void) cveDecomposeProjectionMatrix(
	cv::_InputArray* projMatrix,
	cv::_OutputArray* cameraMatrix,
	cv::_OutputArray* rotMatrix,
	cv::_OutputArray* transVect,
	cv::_OutputArray* rotMatrixX,
	cv::_OutputArray* rotMatrixY,
	cv::_OutputArray* rotMatrixZ,
	cv::_OutputArray* eulerAngles);

CVAPI(void) cveProjectPoints(
	cv::_InputArray* objPoints, cv::_InputArray* rvec, cv::_InputArray* tvec, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
	cv::_OutputArray* imagePoints, cv::_OutputArray* jacobian, double aspectRatio);

CVAPI(bool) cveSolvePnP(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int flags);

CVAPI(bool) cveSolvePnPRansac(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int iterationsCount, float reprojectionError, double confident, cv::_OutputArray* inliers, int flags);

CVAPI(int) cveSolveP3P(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvecs,
	cv::_OutputArray* tvecs,
	int flags);

CVAPI(void) cveSolvePnPRefineLM(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	cv::TermCriteria* criteria);

CVAPI(void) cveSolvePnPRefineVVS(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	cv::TermCriteria* criteria,
	double VVSlambda);

CVAPI(int) cveSolvePnPGeneric(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_OutputArray* rvecs,
	cv::_OutputArray* tvecs,
	bool useExtrinsicGuess,
	int flags,
	cv::_InputArray* rvec,
	cv::_InputArray* tvec,
	cv::_OutputArray* reprojectionError);

CVAPI(void) cveFindFundamentalMat(cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* dst, int method, double param1, double param2, cv::_OutputArray* mask);

CVAPI(void) cveFindEssentialMat(
	cv::_InputArray* points1,
	cv::_InputArray* points2,
	cv::_InputArray* cameraMatrix,
	int method,
	double prob,
	double threshold,
	int maxIter,
	cv::_OutputArray* mask,
	cv::Mat* essentialMat);

CVAPI(void) cveDecomposeEssentialMat(cv::_InputArray* e, cv::_OutputArray* r1, cv::_OutputArray* r2, cv::_OutputArray* t);

CVAPI(int) cveDecomposeHomographyMat(
	cv::_InputArray* h,
	cv::_InputArray* k,
	cv::_OutputArray* rotations,
	cv::_OutputArray* translations,
	cv::_OutputArray* normals);

CVAPI(void) cveComputeCorrespondEpilines(cv::_InputArray* points, int whichImage, cv::_InputArray* f, cv::_OutputArray* lines);

CVAPI(void) cveConvertPointsToHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveConvertPointsFromHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst);

CVAPI(void) cveTriangulatePoints(cv::_InputArray* projMat1, cv::_InputArray* projMat2, cv::_InputArray* projPoints1, cv::_InputArray* projPoints2, cv::_OutputArray* points4D);

CVAPI(void) cveCorrectMatches(cv::_InputArray* f, cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* newPoints1, cv::_OutputArray* newPoints2);

CVAPI(int) cveEstimateAffine3D(
	cv::_InputArray* src, cv::_InputArray* dst,
	cv::_OutputArray* out, cv::_OutputArray* inliers,
	double ransacThreshold, double confidence);

CVAPI(void) cveEstimateAffine2D(
	cv::_InputArray* from, cv::_InputArray* to,
	cv::_OutputArray* inliers,
	int method, double ransacReprojThreshold,
	int maxIters, double confidence,
	int refineIters,
	cv::Mat* affine);

CVAPI(void) cveEstimateAffinePartial2D(
	cv::_InputArray* from, cv::_InputArray* to,
	cv::_OutputArray* inliers,
	int method, double ransacReprojThreshold,
	int maxIters, double confidence,
	int refineIters,
	cv::Mat* affine);

CVAPI(void) cveInitUndistortRectifyMap(cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* newCameraMatrix, cv::Size* size, int m1type, cv::_OutputArray* map1, cv::_OutputArray* map2);
CVAPI(void) cveUndistort(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distorCoeffs, cv::_InputArray* newCameraMatrix);
CVAPI(void) cveUndistortPoints(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* p);

CVAPI(void) cveGetDefaultNewCameraMatrix(cv::_InputArray* cameraMatrix, cv::Size* imgsize, bool centerPrincipalPoint, cv::Mat* cm);

CVAPI(void) cveGetOptimalNewCameraMatrix(
	cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
	cv::Size* imageSize, double alpha, cv::Size* newImgSize,
	cv::Rect* validPixROI,
	bool centerPrincipalPoint,
	cv::Mat* newCameraMatrix);

CVAPI(void) cveDrawFrameAxes(
	cv::_InputOutputArray* image,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputArray* rvec,
	cv::_InputArray* tvec,
	float length,
	int thickness);


#endif