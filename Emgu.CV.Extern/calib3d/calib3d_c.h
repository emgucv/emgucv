//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2014 by EMGU Corporation. All rights reserved.
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
CVAPI(cv::StereoSGBM*) CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int blockSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  int mode, cv::StereoMatcher** stereoMatcher);
CVAPI(void) CvStereoSGBMRelease(cv::StereoSGBM** obj);

//StereoBM
CVAPI(cv::StereoMatcher*) CvStereoBMCreate(int mode, int numberOfDisparities); 

//StereoMatcher
CVAPI(void) CvStereoMatcherCompute(cv::StereoMatcher*  disparitySolver, cv::_InputArray* left, cv::_InputArray* right, cv::_OutputArray* disparity);
CVAPI(void) CvStereoMatcherRelease(cv::StereoMatcher** matcher);

//2D Tracker
CVAPI(bool) getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, std::vector< std::vector< cv::DMatch > >* matches, CvArr* mask, double randsacThreshold, CvMat* homography);

//Find circles grid
CVAPI(bool) cveFindCirclesGrid(cv::_InputArray* image, CvSize* patternSize, cv::_OutputArray* centers, int flags, cv::FeatureDetector* blobDetector);

CVAPI(void) cveTriangulatePoints(cv::_InputArray* projMat1, cv::_InputArray* projMat2, cv::_InputArray* projPoints1, cv::_InputArray* projPoints2, cv::_OutputArray* points4D);

CVAPI(void) cveCorrectMatches(cv::_InputArray* f, cv::_InputArray* points1, cv::_InputArray* points2, cv::_OutputArray* newPoints1, cv::_OutputArray* newPoints2);

CVAPI(void) cveDrawChessboardCorners(cv::_InputOutputArray* image, CvSize* patternSize, cv::_InputArray* corners, bool patternWasFound);

CVAPI(bool) cveFindChessboardCorners(cv::_InputArray* image, CvSize* patternSize, cv::_OutputArray* corners, int flags);

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

CVAPI(void) cveSolvePnPRansac(cv::_InputArray* objectPoints, cv::_InputArray* imagePoints, cv::_InputArray* cameraMatrix, cv::_InputArray* distCoeffs, cv::_OutputArray* rvec, cv::_OutputArray* tvec, bool useExtrinsicGuess, int iterationsCount, float reprojectionError, int minInliersCount, cv::_OutputArray* inliers, int flags );
#endif