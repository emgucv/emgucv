//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2022 by EMGU Corporation. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_3D_C_H
#define EMGU_3D_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/3d.hpp"

CVAPI(void) cveRodrigues(cv::_InputArray* src, cv::_OutputArray* dst, cv::_OutputArray* jacobian);

CVAPI(void) cveFindHomography(cv::_InputArray* srcPoints, cv::_InputArray* dstPoints, cv::_OutputArray* dst, int method, double ransacReprojThreshold, cv::_OutputArray* mask);

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


CVAPI(cv::Odometry*) cveOdometryCreate(cv::OdometryType odometryType);
CVAPI(void) cveOdometryRelease(cv::Odometry** sharedPtr);
CVAPI(bool) cveOdometryCompute1(
	cv::Odometry* odometry,
	cv::_InputArray* srcFrame, 
	cv::_InputArray* dstFrame, 
	cv::_OutputArray* rt);

CVAPI(bool) cveOdometryCompute2(
	cv::Odometry* odometry,
	cv::_InputArray* srcDepthFrame, 
	cv::_InputArray* srcRGBFrame, 
	cv::_InputArray* dstDepthFrame, 
	cv::_InputArray* dstRGBFrame, 
	cv::_OutputArray* rt);


CVAPI(cv::RgbdNormals*) cveRgbdNormalsCreate(
	int rows,
	int cols,
	int depth,
	cv::_InputArray* K,
	int window_size,
	int method,
	cv::Algorithm** algorithm,
	cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsRelease(cv::Ptr<cv::RgbdNormals>** sharedPtr);
CVAPI(void) cveRgbdNormalsApply(
	cv::RgbdNormals* rgbdNormals,
	cv::_InputArray* points,
	cv::_OutputArray* normals);

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

CVAPI(int)  cveEstimateAffine3D(
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

CVAPI(void) cveInitUndistortRectifyMap(cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* newCameraMatrix, CvSize* size, int m1type, cv::_OutputArray* map1, cv::_OutputArray* map2);
CVAPI(void) cveUndistort(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distorCoeffs, cv::_InputArray* newCameraMatrix);
CVAPI(void) cveUndistortPoints(cv::_InputArray* src, cv::_OutputArray* dst, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_InputArray* r, cv::_InputArray* p);

CVAPI(void) cveGetDefaultNewCameraMatrix(cv::_InputArray* cameraMatrix, CvSize* imgsize, bool centerPrincipalPoint, cv::Mat* cm);

CVAPI(void) cveGetOptimalNewCameraMatrix(
	cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs,
	CvSize* imageSize, double alpha, CvSize* newImgSize,
	CvRect* validPixROI,
	bool centerPrincipalPoint,
	cv::Mat* newCameraMatrix);
#endif