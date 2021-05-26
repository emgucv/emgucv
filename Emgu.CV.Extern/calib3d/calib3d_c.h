//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2021 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CALIB3D_C_H
#define EMGU_CALIB3D_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/calib3d/calib3d.hpp"

CVAPI(int)  cveEstimateAffine3D(
   cv::_InputArray* src, cv::_InputArray* dst,
   cv::_OutputArray* out, cv::_OutputArray* inliers,
   double ransacThreshold, double confidence);

//StereoSGBM
CVAPI(cv::StereoSGBM*) cveStereoSGBMCreate(
  int minDisparity, int numDisparities, int blockSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  int mode, cv::StereoMatcher** stereoMatcher, cv::Ptr<cv::StereoSGBM>** sharedPtr);
CVAPI(void) cveStereoSGBMRelease(cv::Ptr<cv::StereoSGBM>** sharedPtr);

//StereoBM
CVAPI(cv::StereoMatcher*) cveStereoBMCreate(int mode, int numberOfDisparities, cv::Ptr<cv::StereoMatcher>** sharedPtr);

//StereoMatcher
CVAPI(void) cveStereoMatcherCompute(cv::StereoMatcher*  disparitySolver, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity);
CVAPI(void) cveStereoMatcherRelease(cv::Ptr<cv::StereoMatcher>** sharedPtr);

//2D Tracker
CVAPI(bool) getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, std::vector< std::vector< cv::DMatch > >* matches, cv::Mat* mask, double randsacThreshold, cv::Mat* homography);

//Find circles grid
CVAPI(bool) cveFindCirclesGrid(cv::_InputArray* image, CvSize* patternSize, cv::_OutputArray* centers, int flags, cv::Feature2D* blobDetector);

CVAPI(void) cveTriangulatePoints(cv::_InputArray* projMat1, cv::_InputArray* projMat2, cv::_InputArray* projPoints1, cv::_InputArray* projPoints2, cv::_OutputArray* points4D);

CVAPI(void) cveCorrectMatches(cv::_InputArray* f, cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* newPoints1, cv::_OutputArray* newPoints2);

CVAPI(bool) cveFindChessboardCornersSB(cv::_InputArray* image, CvSize* patternSize, cv::_OutputArray* corners, int flags);

CVAPI(void) cveEstimateChessboardSharpness(
	cv::_InputArray* image, 
	CvSize* patternSize, 
	cv::_InputArray* corners,
	float riseDistance, 
	bool vertical,
	cv::_OutputArray* sharpness, 
	CvScalar* result);

CVAPI(void) cveDrawChessboardCorners(cv::_InputOutputArray* image, CvSize* patternSize, cv::_InputArray* corners, bool patternWasFound);

CVAPI(void) cveFilterSpeckles(cv::_InputOutputArray* img, double newVal, int maxSpeckleSize, double maxDiff, cv::_InputOutputArray* buf);

CVAPI(bool) cveFindChessboardCorners(cv::_InputArray* image, CvSize* patternSize, cv::_OutputArray* corners, int flags);

CVAPI(bool) cveFind4QuadCornerSubpix(cv::_InputArray* image, cv::_InputOutputArray* corners, CvSize* regionSize);

CVAPI(bool) cveStereoRectifyUncalibrated(cv::_InputArray* points1, cv::_InputArray* points2, cv::_InputArray* f, CvSize* imgSize, cv::_OutputArray* h1, cv::_OutputArray* h2, double threshold);

CVAPI(void) cveStereoRectify(
   cv::_InputArray* cameraMatrix1, cv::_InputArray* distCoeffs1,
   cv::_InputArray* cameraMatrix2, cv::_InputArray* distCoeffs2,
   CvSize* imageSize, cv::_InputArray* r, cv::_InputArray* t,
   cv::_OutputArray* r1, cv::_OutputArray* r2,
   cv::_OutputArray* p1, cv::_OutputArray* p2,
   cv::_OutputArray* q, int flags,
   double alpha, CvSize* newImageSize,
   CvRect* validPixROI1, CvRect* validPixROI2);

CVAPI(void) cveRodrigues(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* jacobian);

CVAPI(double) cveCalibrateCamera(
   cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, CvSize* imageSize, 
   cv::_InputOutputArray* cameraMatrix, cv::_InputOutputArray* distCoeffs, 
   cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags, CvTermCriteria* criteria);

CVAPI(void) cveReprojectImageTo3D(cv::_InputArray* disparity, cv::_OutputArray* threeDImage, cv::_InputArray* q, bool handleMissingValues, int ddepth);

CVAPI(void) cveConvertPointsToHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst);
CVAPI(void) cveConvertPointsFromHomogeneous(cv::_InputArray* src, cv::_OutputArray* dst);


CVAPI(void) cveFindEssentialMat(cv::_InputArray* points1, cv::_InputArray* points2, cv::_InputArray* cameraMatrix, int method, double prob, double threshold, cv::_OutputArray* mask, cv::Mat* essentialMat);
CVAPI(void) cveFindFundamentalMat(cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* dst, int method, double param1, double param2, cv::_OutputArray* mask);
CVAPI(void) cveFindHomography(cv::_InputArray* srcPoints, cv::_InputArray* dstPoints, cv::_OutputArray* dst, int method, double ransacReprojThreshold, cv::_OutputArray* mask);

CVAPI(void) cveComputeCorrespondEpilines(cv::_InputArray* points, int whichImage, cv::_InputArray* f, cv::_OutputArray* lines);

CVAPI(void) cveProjectPoints(
   cv::_InputArray* objPoints, cv::_InputArray* rvec, cv::_InputArray* tvec, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
   cv::_OutputArray* imagePoints, cv::_OutputArray* jacobian, double aspectRatio);

CVAPI(void) cveCalibrationMatrixValues(
   cv::_InputArray* cameraMatrix, CvSize* imageSize, double apertureWidth, double apertureHeight, 
   double* fovx, double* fovy, double* focalLength, CvPoint2D64f* principalPoint, double* aspectRatio);

CVAPI(double) cveStereoCalibrate(
   cv::_InputArray* objectPoints, cv::_InputArray* imagePoints1, cv::_InputArray* imagePoints2,
   cv::_InputOutputArray* cameraMatrix1, cv::_InputOutputArray* distCoeffs1, cv::_InputOutputArray* cameraMatrix2, cv::_InputOutputArray* distCoeffs2,
   CvSize* imageSize, cv::_OutputArray* r, cv::_OutputArray* t, cv::_OutputArray* e, cv::_OutputArray* f, int flags, CvTermCriteria* criteria);

CVAPI(bool) cveSolvePnP(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int flags);

CVAPI(bool) cveSolvePnPRansac(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int iterationsCount, float reprojectionError, double confident, cv::_OutputArray* inliers, int flags );

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
	CvTermCriteria* criteria);

CVAPI(void) cveSolvePnPRefineVVS(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	cv::_InputArray* cameraMatrix,
	cv::_InputArray* distCoeffs,
	cv::_InputOutputArray* rvec,
	cv::_InputOutputArray* tvec,
	CvTermCriteria* criteria,
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

CVAPI(void) cveGetOptimalNewCameraMatrix(
	cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
	CvSize* imageSize, double alpha, CvSize* newImgSize,
	CvRect* validPixROI,
	bool centerPrincipalPoint, 
	cv::Mat* newCameraMatrix);

CVAPI(void) cveInitCameraMatrix2D(
	cv::_InputArray* objectPoints,
	cv::_InputArray* imagePoints,
	CvSize* imageSize, 
	double aspectRatio, 
	cv::Mat* cameraMatrix);

/* Fisheye calibration */
CVAPI(void) cveFisheyeProjectPoints(cv::_InputArray* objectPoints, cv::_OutputArray* imagePoints, cv::_InputArray* rvec, cv::_InputArray* tvec,
   cv::_InputArray* K, cv::_InputArray* D, double alpha, cv::_OutputArray* jacobian);

CVAPI(void) cveFisheyeDistortPoints(cv::_InputArray* undistored, cv::_OutputArray* distorted, cv::_InputArray* K, cv::_InputArray* D, double alpha);

CVAPI(void) cveFisheyeUndistorPoints(cv::_InputArray* distorted, cv::_OutputArray* undistorted, cv::_InputArray* K, cv::_InputArray* D, cv::_InputArray* R, cv::_InputArray* P);

CVAPI(void) cveFisheyeInitUndistorRectifyMap(cv::_InputArray* K, cv::_InputArray* D, cv::_InputArray* R, cv::_InputArray* P, CvSize* size, int m1Type, cv::_OutputArray* map1, cv::_OutputArray* map2);

CVAPI(void) cveFisheyeUndistorImage(cv::_InputArray* distorted, cv::_OutputArray* undistored, cv::_InputArray* K, cv::_InputArray* D, cv::_InputArray* Knew, CvSize* newSize);

CVAPI(void) cveFisheyeEstimateNewCameraMatrixForUndistorRectify(cv::_InputArray* K, cv::_InputArray* D, CvSize* imageSize, cv::_InputArray* R, cv::_OutputArray* P, double balance, CvSize* newSize, double fovScale);

CVAPI(void) cveFisheyeStereoRectify(cv::_InputArray* K1, cv::_InputArray*D1, cv::_InputArray* K2, cv::_InputArray* D2, CvSize* imageSize,
   cv::_InputArray* R, cv::_InputArray* tvec, cv::_OutputArray* R1, cv::_OutputArray* R2, cv::_OutputArray* P1, cv::_OutputArray* P2, cv::_OutputArray* Q, int flags,
   CvSize* newImageSize, double balance, double fovScale);

CVAPI(void) cveFisheyeCalibrate(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, CvSize* imageSize,
   cv::_InputOutputArray* K, cv::_InputOutputArray* D, cv::_OutputArray* rvecs, cv::_OutputArray* tvecs, int flags,
   CvTermCriteria* criteria);

CVAPI(void) cveFisheyeStereoCalibrate(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints1,
   cv::_InputArray* imagePoints2, cv::_InputOutputArray* K1, cv::_InputOutputArray* D1, cv::_InputOutputArray* K2, cv::_InputOutputArray* D2,
   CvSize* imageSize, cv::_OutputArray* R, cv::_OutputArray* T, int flags, CvTermCriteria* criteria);

CVAPI(void) cveInitUndistortRectifyMap(cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* newCameraMatrix, CvSize* size, int m1type, cv::_OutputArray* map1, cv::_OutputArray* map2);
CVAPI(void) cveUndistort(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distorCoeffs, cv::_InputArray* newCameraMatrix);
CVAPI(void) cveUndistortPoints(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* p);

CVAPI(void) cveGetDefaultNewCameraMatrix(cv::_InputArray* cameraMatrix, CvSize* imgsize, bool centerPrincipalPoint, cv::Mat* cm);

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

CVAPI(void) cveCalibrateHandEye(cv::_InputArray* R_gripper2base, cv::_InputArray* t_gripper2base,
	cv::_InputArray* R_target2cam, cv::_InputArray* t_target2cam,
	cv::_OutputArray* R_cam2gripper, cv::_OutputArray* t_cam2gripper,
	int method);

CVAPI(void) cveRQDecomp3x3(
	cv::_InputArray* src,
	CvPoint3D64f* out,
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
#endif