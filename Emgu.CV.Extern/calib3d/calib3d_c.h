//----------------------------------------------------------------------------
//
//  Copyright (C) 2004-2013 by EMGU. All rights reserved.
//
//----------------------------------------------------------------------------

#pragma once
#ifndef EMGU_CALIB3D_C_H
#define EMGU_CALIB3D_C_H

#include "opencv2/core/core_c.h"
#include "opencv2/calib3d/calib3d.hpp"

CVAPI(int)  CvEstimateAffine3D(CvMat* src, CvMat* dst,
                               cv::Mat* out, std::vector<unsigned char>* inliers,
                             double ransacThreshold, double confidence);

//StereoSGBM
CVAPI(cv::StereoSGBM*) CvStereoSGBMCreate(
  int minDisparity, int numDisparities, int blockSize,
  int P1, int P2, int disp12MaxDiff,
  int preFilterCap, int uniquenessRatio,
  int speckleWindowSize, int speckleRange,
  int mode);

CVAPI(void) CvStereoSGBMRelease(cv::StereoSGBM* obj);
CVAPI(void) CvStereoSGBMFindCorrespondence(cv::StereoSGBM* disparitySolver, IplImage* left, IplImage* right, IplImage* disparity);

//2D Tracker
CVAPI(bool) getHomographyMatrixFromMatchedFeatures(std::vector<cv::KeyPoint>* model, std::vector<cv::KeyPoint>* observed, CvArr* indices, CvArr* mask, double randsacThreshold, CvMat* homography);

//Find circles grid
CVAPI(bool) cvFindCirclesGrid(IplImage* image, CvSize* patternSize, std::vector<cv::Point2f>* centers, int flags, cv::FeatureDetector* blobDetector);
#endif